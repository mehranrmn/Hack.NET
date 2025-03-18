# GridPersistence

## Overview

The GridPersistence project demonstrates how Telerik's PersistenceFramework saves and restores the state of a `RadGridView` in a WPF application, using mock data from a JSON file. I wrote a [blog post](https://armiyon.me/blog/2025/cve-2024-10095-unsafe-deserialization-enables-rce-in-telerik-ui/) analyzing CVE-2024-10095, an insecure deserialization vulnerability in Telerik UI for WPF, and this project serves as a practical demonstration of the issue. For a full breakdown of the vulnerability and its exploitation, check it out.


## Key Components
- **CustomPropertyProvider**: Implements the `ICustomPropertyProvider` interface to define which properties of the `RadGridView` should be persisted. This includes specifying how each property is saved and restored.
- **PersistenceManager**: Manages the persistence process, utilizing the custom property provider to handle the serialization and deserialization of the `RadGridView` state.

## Functionality
The project showcases the following capabilities:​
- **Saving Grid State**: Captures the current state of the `RadGridView`, including column settings and data descriptors, and saves it using the PersistenceFramework.
- **Loading Grid State**: Restores the `RadGridView` to a previously saved state, ensuring that the user interface reflects the persisted configurations.
- **Custom Property Definitions**: Demonstrates how to create proxy classes that represent the properties to be persisted, providing control over the serialization process.​

## Requirements
- **Telerik UI for WPF**: Ensure that the necessary Telerik assemblies are referenced in the project. For more information on required assemblies, refer to the [Telerik documentation](https://docs.telerik.com/devtools/wpf/controls/radpersistenceframework/persistence-framework-getting-started).

- **.NET Framework**: This project targets the .NET Framework compatible with the Telerik UI for WPF controls.​

## Getting Started
- **Clone the Repository**: Clone the repository to your local machine.​
- **Open the Solution**: Open the solution in Visual Studio.​
- **Restore NuGet Packages**: Restore the NuGet packages to ensure all dependencies are resolved.​
- **Build and Run**: Build the solution and run the application to see the PersistenceFramework in action.​