# DLRG-ExamRegistrationSystem

A system to collect exam requirements from participants of complex DLRG exams such as the Boat Operators License exam.

> [!IMPORTANT]  
> This is a non official project, created to augment existing proccesses and work as an inspiration. It is not approved nor developed by the [AK IT of the DLRG](https://www.dlrg.de/fuer-mitglieder/arbeitskreise/arbeitskreis-informationstechnik/).

In correspondence with the [MIT License](https://github.com/abeckDev/DLRG-ExamRegistrationSystem/blob/main/LICENSE), you may use the software free of chart without limitations but WITHOUT WARRANTY OF ANY KIND. Please review the terms of the [MIT LICENSE](https://opensource.org/license/MIT/) for further information.

## Architecture

### Architecture Draft

![Architecture Draft](https://github.com/abeckDev/DLRG-ExamRegistrationSystem/assets/8720854/bb637d12-86ff-49e6-a776-b168b631899c)

### Architecture Description

The DLRG-ExamRegistrationSystem is built using Azure Functions and an Azure Storage Account. It consists of three Azure Functions and a Storage Account.

#### ClassifyInput Function

The first Azure Function, called the ClassifyInput Function, is responsible for receiving user input and documents via HTTP. It uploads the picture and requirements document to different blob containers in the Storage Account.

#### PhotoProcessing Function

The second Azure Function, called the PhotoProcessing Function, is triggered when a photo is uploaded to the Storage Account. It processes the photo by altering the file, renaming it, and uploading it to the configured NextCloud system.

#### PDFProcessing Function

The third Azure Function, called the PDFProcessing Function, is triggered when a PDF document is uploaded to the Storage Account. It processes the document by altering the file, renaming it, and uploading it to the configured NextCloud system.

### Azure Storage Account

The Azure Storage Account is used to store the uploaded pictures and requirements documents. The ClassifyInput Function uploads the files to different blob containers within the Storage Account. The PhotoProcessing and PDFProcessing Functions retrieve the files from the Storage Account, process them, and upload them to the NextCloud system.

This architecture allows for the collection and processing of exam requirements from participants, ensuring that the files are properly classified, processed, and stored.

## Getting Started

Welcome to the DLRG-ExamRegistrationSystem! This project is based on Azure Functions, which are serverless compute resources that allow you to run your code in the cloud without worrying about infrastructure. It is also built with .NET 8, a powerful and versatile framework for developing various types of applications.

To execute the code locally, please make sure you have the following prerequisites installed:

### Prerequisites

You need the following prerequisites to operate the solution:

* [Azure Storage Account](https://azure.microsoft.com/services/storage/)
* [Azure Function Runtime](https://azure.microsoft.com/services/functions/)
* [.NET 8 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)
* (Optional) A [rest client](https://insomnia.rest/) to trigger the first function

#### Configuring the application

To configure the application, you will need to create a `local.settings.json` file in the root directory of your project. This file will contain the necessary configuration settings for your application to run locally.

Here is the basic format of the `local.settings.json` file:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "<StorageAccountConnectionString>",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "dlrgCloudUsername": "<Value>",
    "dlrgCloudPassword": "<Value>",
    "dlrgCloudBasePath": "<Value>",
    "blobStorageConnectionString": "<StorageAccountConnectionString>"
  },
  "Host": {
    "LocalHttpPort": 7071,
    "CORS": "*",
    "CORSCredentials": false
  },
  "ConnectionStrings": {
  }
}
```

The following Tables explains the usage of the needed config parameters:

| Config Parameter            | Explanation                                      | Example                           |
|----------------------------|--------------------------------------------------|-----------------------------------|
| AzureWebJobsStorage        | Connection string for the storage account         | "DefaultEndpointsProtocol=https;AccountName=myaccount;AccountKey=mykey;EndpointSuffix=core.windows.net" |
| FUNCTIONS_WORKER_RUNTIME   | Specifies the runtime for Azure Functions         | "dotnet-isolated"                 |
| dlrgCloudUsername          | Username for DLRG cloud service                   | "myusername"                      |
| dlrgCloudPassword          | Password for DLRG cloud service                   | "mypassword"                      |
| dlrgCloudBasePath          | Base path for DLRG cloud service                  | "WebDav BaseUrl from Nextcloud"                            |
| blobStorageConnectionString | Connection string for the blob storage account    | "DefaultEndpointsProtocol=https;AccountName=myaccount;AccountKey=mykey;EndpointSuffix=core.windows.net" |

> [!CAUTION]
> Storing passwords in a config file is meant for local debugging only! Please consider the usage of [.NET Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=linux) to also store local settings in a non plain text mode. Please note that the local.settings.json file will be ignored by git. In a production scenario we will be using an [Azure KeyVault](https://learn.microsoft.com/en-us/azure/key-vault/general/basic-concepts) to secretly store these secrets.

#### Setting up .NET 8 to run the code locally

To run the code locally, you will need to have the .NET 8 runtime or SDK installed on your machine. Follow the steps below to set it up:

1. Visit the [.NET downloads](https://dotnet.microsoft.com/download/dotnet/8.0) page.

2. Choose the appropriate installer for your operating system and download it.

3. Run the installer and follow the on-screen instructions to complete the installation.

4. After the installation is complete, open a terminal or command prompt and run the following command to verify that the .NET runtime or SDK is installed:

    ```bash
    dotnet --version
    ```

    This command should display the version number of the installed .NET runtime or SDK.

5. You are now ready to run the code locally using the .NET 8 runtime or SDK.

#### Setting up Azure Functions locally (Version 4)

1. Install the Azure Functions Core Tools by following the instructions in the [official documentation](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=macos%2Ccsharp%2Cbash#v2).

2. Open a terminal or command prompt and navigate to the [root directory](./AzureFunctions/) of your project.

3. Run the following command to start the local runtime:

    ```bash
    func host start
    ```

4. Your Azure Functions should now be running locally. You can access them using the provided URLs.

    > Note: Make sure to update the necessary configuration settings, such as connection strings and app settings, in the local.settings.json file. See the preceding section for configuration details.

For more information on developing and debugging Azure Functions locally, refer to the [official documentation](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=macos%2Ccsharp%2Cbash).

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

## Running the tests

There are no tests and so far, none are planned. Feel free to file an Issue if you think it might be valuable.

## Deployment

Since the project is still in MVP phase, there is no deployment action yet. It is Design to run in Microsoft Azure Function Apps in the future.

## Built With

This project is built with the following tools:

* [Azure Functions](https://azure.microsoft.com/en-us/services/functions/)
* [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)

## Contributing

Feel free to file for an issue, or create a pull request in case you have any improvement ideas. Since this is a small PoC project, there is no need for a full contribution document.

## Authors

* **Alexander Beck** - *Initial work* - [abeckDev](https://github.com/abeckDev)

See also the list of [contributors](https://github.com/abeckDev/DLRG-ExamRegistrationSystem/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [MIT License](https://github.com/abeckDev/DLRG-ExamRegistrationSystem/blob/main/LICENSE) file for details
