# Development setup

## Requirements

Use a Windows development machine with:

- Visual Studio and the **ASP.NET and web development** workload. The solution was saved with Visual Studio 2022.
- The [.NET Framework 4.7.2 Developer Pack](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net472).
- IIS Express, installed with the Visual Studio web development tools.
- Access to a compatible SQL Server database.

The application uses the .NET Framework Web Forms project format and IIS hosting. Its dependency versions are retained in `src/DNDWebsite/packages.config`.

## Configure the database

From the repository root in PowerShell:

```powershell
Copy-Item 'src\DNDWebsite\ConnectionStrings.example.config' 'src\DNDWebsite\ConnectionStrings.config'
notepad 'src\DNDWebsite\ConnectionStrings.config'
```

Create the local file before opening or running the web application. Replace `YOUR_SQL_SERVER` and `YOUR_DATABASE` with the server and database you are authorised to use. The example uses Windows authentication, which runs under the IIS Express or IIS worker process identity.

For SQL Server authentication, replace the example connection string with local values using this format:

```text
Data Source=YOUR_SQL_SERVER;Initial Catalog=YOUR_DATABASE;User ID=YOUR_USERNAME;Password=YOUR_PASSWORD;Encrypt=True;TrustServerCertificate=False
```

Keep the connection name `DNDConnectionString` and provider `System.Data.SqlClient` unchanged. XML-escape special characters in attribute values, including `&` as `&amp;`.

`Web.config` reads this file using the .NET Framework [external connection strings configuration](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/connection-strings-and-configuration-files) mechanism. It is ignored by Git and included as local web content when present, so publishing the project can include the configured file. Treat published copies as credentials and configure each deployment for its own database.

### Database availability

A working database is a separate prerequisite. The repository does not include a SQL creation script, database backup or demo credentials. The application queries business tables including `Client`, `User`, `Product`, `Supplier`, `SupplierProduct`, `Order`, `ClientOrderProduct`, `OrderSupplierProduct` and `Payment`.

Client registration and staff sign-in require the matching schema. Staff accounts must already exist in the `User` table. Use a compatible development database with test records when checking the workflows.

## Restore dependencies

Open `DNDWebsite.sln`, right-click the solution and select **Restore NuGet Packages**. Then build the solution.

With the [NuGet CLI](https://learn.microsoft.com/en-us/nuget/tools/cli-ref-restore), you can also run this from the repository root:

```powershell
nuget restore DNDWebsite.sln -PackagesDirectory packages
```

The root `NuGet.Config` sets `repositoryPath` to `packages` for this `packages.config` project. Assembly HintPaths and the CodeDom build import in `src/DNDWebsite/DNDWebsite.csproj` point to that location. Package versions are unchanged.

The `Content` and `Scripts` folders are application content and stay in source control. Restored NuGet packages and compiled DLLs are generated locally and are ignored.

## Run locally

1. Open `DNDWebsite.sln` in Visual Studio.
2. Restore NuGet packages and build.
3. Set the web project as the startup project if necessary.
4. Select IIS Express and run the application.
5. Use the local URL assigned by Visual Studio. Existing local IIS Express settings may need to be recreated after the project directory move.

For IIS hosting, set the application's physical directory to the web application or published output directory, use a .NET Framework 4.x application pool, and supply the database configuration for that host. Deployment profiles and saved publishing credentials are local files and are excluded from the repository.

## Configure email

Both the contact form and order notifications use the existing Gmail SMTP endpoint at `smtp.gmail.com:587` with SSL enabled.

| Variable | Purpose |
| --- | --- |
| `DND_SMTP_USER` | The sender's Gmail address. |
| `DND_SMTP_PASSWORD` | That account's SMTP credential, such as an app password where required. |
| `DND_CONTACT_EMAIL` | Optional recipient inbox for contact form submissions; defaults to the sender address. |

These are the same SMTP variable names as the related desktop application. Set them in the environment inherited by the web worker process. For IIS Express, launch Visual Studio from the configured shell; an already-running Visual Studio or IIS Express process will not inherit newly set variables.

For example, set the sender and read the credential without displaying it in PowerShell:

```powershell
$env:DND_SMTP_USER = 'your-address@gmail.com'
$smtpSecret = Read-Host 'SMTP app password' -AsSecureString
$env:DND_SMTP_PASSWORD = [System.Net.NetworkCredential]::new('', $smtpSecret).Password
$env:DND_CONTACT_EMAIL = 'your-test-inbox@example.com'
```

Then launch Visual Studio from the same shell, for example with `devenv .\DNDWebsite.sln` if the Visual Studio command is available in that shell. For IIS, configure the environment for the application pool and recycle it after changes.

When either SMTP credential is missing, contact submissions show an availability message without clearing the fields. Order finalisation still saves the order and indicates that some email notifications were not sent. Sending failures are also reported without undoing a saved order.

Configured email sends real messages. Check the contact inbox and use test client and supplier email addresses when validating order notifications. Existing business contact details shown by the pages are separate from the SMTP sender configuration.

## Troubleshooting

| Problem | Check |
| --- | --- |
| Missing `ConnectionStrings.config` or a configuration error | Copy and edit the local example in `src/DNDWebsite` before starting the web app. |
| SQL connection or sign-in error | Verify the server, authentication, network access, process identity and compatible database schema. |
| SQL certificate error | Confirm that the SQL Server certificate is trusted and matches the server name. |
| Missing DLL or CodeDom targets | Restore NuGet packages to the repository root `packages` folder. |
| Missing .NET Framework reference assemblies | Install the 4.7.2 Developer Pack. |
| IIS Express still points to the old directory | Close Visual Studio and IIS Express, then reopen the moved project and recreate its local web settings. |
| Contact email is unavailable | Set both SMTP variables in the web worker process environment. |
| Order finalises with an email warning | Confirm the sender credentials and the client or supplier recipient addresses. |

## Repository maintenance

Commit each Web Forms page with its `.aspx.cs` and `.aspx.designer.cs` files when they change. Keep project inputs and pinned package versions together. Build output, restored packages, per-user project settings and publish profiles are generated or configured locally.

The cleanup preserves Git history. Removing compiled output and downloaded packages from the current tree does not reduce older history, and it does not invalidate previously committed database or email credentials. Rotate those credentials separately.
