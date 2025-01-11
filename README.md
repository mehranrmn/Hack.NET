# DotNetSandbox

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/mehranrmn/DotNetSandbox/CI)

## Overview

DotNetSandbox is an ASP.NET Core project designed to facilitate learning in web security by implementing, exploiting, and mitigating various security vulnerabilities. Through a series of versioned releases, the project demonstrates different bug classes, showcasing the lifecycle from insecure code to exploitation and eventual mitigation.

## Features

- **File Upload Functionality**: Enables uploading of files to the server.
- **Assigning Metadata to Files**: Allows metadata to be assigned to uploaded files using serialized data.

## Security Lifecycle

DotNetSandbox employs a security lifecycle approach to demonstrate the progression from vulnerable code to secure implementation. Each stage of the lifecycle is marked with a corresponding git tag, following a structured naming convention to indicate the current state:

- **Bugged**: Initial implementation with intentional security vulnerabilities.
- **Proof of Concept (PoC)**: Demonstrates how the vulnerabilities can be exploited.
- **Secured**: Implements security measures to mitigate the identified vulnerabilities.

## Installation

### Prerequisites

- **.NET Core SDK**: [Download](https://dotnet.microsoft.com/download) (version 6.0 or later recommended)
  
  _Note_: Version `v0.2.0` utilizes `BinaryFormatter`, which is only supported up to .NET 8.0. `BinaryFormatter` has been removed in .NET 9.0 due to security concerns. For more information, see the [BinaryFormatter security guide](https://learn.microsoft.com/en-us/dotnet/standard/serialization/binaryformatter-security-guide) and the [BinaryFormatter migration guide](https://learn.microsoft.com/en-us/dotnet/standard/serialization/binaryformatter-migration).

- **Bash Shell**: Required to execute the `run.sh` script (available on Unix-based systems and Windows via [WSL](https://docs.microsoft.com/en-us/windows/wsl/install) or [Git Bash](https://gitforwindows.org/))
- **ASP.NET Core HTTPS Development Certificates**: Ensure that your development environment trusts the ASP.NET Core HTTPS certificates by running:

    ```bash
    dotnet dev-certs https --trust
    ```

- **Operating System Compatibility**: The `run.sh` script is designed for Unix-based environments. Windows users should use WSL or Git Bash to execute the script.

- **Verify .NET SDK Installation** (optional):

    To confirm that the .NET SDK is installed correctly and to check its version, run:

    ```bash
    dotnet --version
    ```

### Running the Project

A `run.sh` script is provided to streamline the build and execution process.

1. **Make the script executable** (if not already):

    ```bash
    chmod +x run.sh
    ```

2. **Execute the script**:

    ```bash
    ./run.sh
    ```

### Accessing Swagger

After running the project, you can access the Swagger UI to interact with the APIs:

- **Swagger UI**: [https://localhost:7037/swagger/index.html](https://localhost:7037/swagger/index.html)

Swagger provides a user-friendly interface to explore and test the available web APIs.

### Database and Cleanup

DotNetSandbox uses the [`InMemory` database provider](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/) for simplicity and ease of testing. This eliminates the need for setting up an external database, allowing developers to focus on the security aspects of the project.

Uploaded images are stored in the `wwwroot/images` directory. A cleanup mechanism ensures that all images are automatically deleted when the `run.sh` script is terminated (e.g., via Ctrl-C). This is handled by trapping the SIGINT signal in the script, which triggers the cleanup function to remove the images. This maintains a clean environment by removing residual data after each session.

## Contact

For any questions, suggestions, or feedback, please reach out to [mehranrmn@disroot.org](mailto:mehranrmn@disroot.org) or open an issue on the [GitHub repository](https://github.com/mehranrmn/DotNetSandbox).