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

## How to operate

In VS Code, go to ![Program.cs](/Program.cs) and press F5 to run the app
