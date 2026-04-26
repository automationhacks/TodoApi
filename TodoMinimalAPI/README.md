# TodoMinimalAPI

This repo shows how you can build quick simple APIs using ASP.NET Core

Follows [this](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-10.0&tabs=visual-studio-code) tutorial

## Setup

```shell
# how to init a new project
dotnet new web -o <name_of_solution>
# in this case, we don't need to run this as this service is
# already present
dotnet new web -o TodoMinimalAPI
```

```shell
# we need to trust the dev certificate
dotnet dev-certs https --trust
```

It prints a message like below and shows how can trust be established and revoked later on if required.

```shell
Trusting the HTTPS development certificate was requested. If the certificate is not already trusted we will run the following command:
'security add-trusted-cert -p basic -p ssl -k <<login-keychain>> <<certificate>>'
This command might prompt you for your password to install the certificate on the keychain. To undo these changes: 'security remove-trusted-cert <<certificate>>'

Successfully trusted the existing HTTPS certificate.
```

Include Nuget packages to support database and diagnostics

```shell
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
```

Add Swagger

```shell
dotnet add package NSwag.AspNetCore
```

## How to operate

In VS Code, go to ![Program.cs](/Program.cs) and press F5 to run the app

You can run the application by

```shell
dotnet run
```

In case you have a specific file you want to run that has the service initialization code

```shell
dotnet run DeveloperExceptionPage.cs
```

## Testing

- We use [NSwag.AspNetCore](https://www.nuget.org/packages/NSwag.AspNetCore/) that integrates swagger into ASP.Net core applications providing middleware and configuration
- Swagger provides tools like OpenAPIGenerator and SwaggerUI that generates API testing pages that follow OpenAPI specification
- OpenAPI specification: document that describes capabilities of an API, based on XML and attribute annotations within the controller and models

## Deployment

Using [Azure tools extension](https://code.visualstudio.com/docs/azure/gettingstarted) in VSCodeb, select `Azure app service: Deploy to web app` to publish the API. Follow the steps [here](https://learn.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore?tabs=net10&pivots=development-environment-vscode) to deploy your application to Azure app service

Get todo

```shell
curl -i -X GET "https://todominimalapi-dpduh6fvbnhtbac7.southindia-01.azurewebsites.net/todoitems"
```

Create todo

```shell
curl -i -X POST "https://todominimalapi-dpduh6fvbnhtbac7.southindia-01.azurewebsites.net/todoitems" \
     -H "Content-Type: application/json" \
     -d "{\"name\":\"finish azure tutorial\",\"isComplete\":false}"
```

## Monitoring

You can see app service logs by clicking on **Log stream** in portal.azure.com
