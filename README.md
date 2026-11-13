# DnD Trading Website

ASP.NET Web Forms application for **DnD Trading & General Supplies**, a stationery supplier. The website connects client ordering with the sales team's supplier, fulfilment and payment workflows.

Developed as an **ISTN3SI Group 22** project by **Horizon Innovations**. The related [Windows desktop application](https://github.com/ArjunRamphal/DnD-Trading) is maintained in a separate repository.

## Website preview

![DnD Trading website homepage hero section](docs/images/homepage.png)

Preserved homepage hero-section capture from the original website.

The original SQL Server database is no longer available. This repository preserves the website source and its existing homepage capture; running database-backed workflows requires a compatible replacement database.

## Features

| Area | Functionality |
| --- | --- |
| Client accounts | Registration, sign-in, account updates and password recovery. |
| Products and orders | Browse supplier products, submit product requests, and view orders and payment details. |
| Staff workflows | Maintain clients, suppliers and supplier products; fulfil client orders and update payment records. |
| Manager reports | Revenue, popular products and sales representative performance charts. |
| Contact | Send enquiries to a configured email inbox. |
| Help | A keyword-based chatbot with predefined responses. |

## Technology

- C# and ASP.NET Web Forms on .NET Framework 4.7.2
- SQL Server accessed through ADO.NET
- HTML, CSS, JavaScript, Bootstrap and jQuery
- NuGet dependencies recorded in `packages.config`
- IIS Express for local development

## Repository layout

| Path | Contents |
| --- | --- |
| `DNDWebsite.sln` | Visual Studio solution. |
| `src/DNDWebsite/` | Web application project, pages, code-behind and page designers. |
| `src/DNDWebsite/App_Start/` | Routing and bundle configuration. |
| `src/DNDWebsite/Content/` and `Scripts/` | Browser styles and scripts used by the application. |
| `src/DNDWebsite/packages.config` | Pinned NuGet package versions. |
| `NuGet.Config` | Restores dependencies to the repository's local `packages` folder. |
| `docs/SETUP.md` | Development setup, database configuration and email settings. |
| `docs/images/homepage.png` | Preserved website homepage capture. |

Build output, restored NuGet packages, personal IDE settings and deployment profiles are excluded from version control. The browser assets referenced by the pages remain with the application.

## Getting started

1. Install Visual Studio on Windows with the **ASP.NET and web development** workload and the **.NET Framework 4.7.2 Developer Pack**.
2. Copy `src/DNDWebsite/ConnectionStrings.example.config` to `src/DNDWebsite/ConnectionStrings.config` and enter your database settings.
3. Open `DNDWebsite.sln`, restore NuGet packages, build the solution and run it with IIS Express.

See the [setup guide](docs/SETUP.md) for complete instructions.

**A compatible SQL Server database is required.** The repository does not contain a database creation script, database backup or seeded demo account.

## Configuration

Database credentials belong in the local, ignored `ConnectionStrings.config` file. Email uses the `DND_SMTP_USER` and `DND_SMTP_PASSWORD` environment variables. `DND_CONTACT_EMAIL` optionally selects the inbox for contact form submissions; it defaults to the sender address.

When SMTP is not configured, the contact form keeps the entered message and explains that email is unavailable. Order finalisation still saves the order and reports any email notifications that were not sent.

## Team

Horizon Innovations:

- Darian Robert
- Arjun Ramphal
- Lokadi Naicker
- Keolin Naicker
- Zaheera Ganie

