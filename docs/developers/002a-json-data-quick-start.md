# SAPPub.Web – Quick Start (Local JSON Data)

Use this guide when you want to run the site quickly without setting up Postgres or importing CSV data.

This uses `JSONRepository<T>` as the data source.

---

## What this mode does

- Reads data from JSON files instead of the database
- Uses `UseLocalDataSource=true` to enable JSON-backed repositories
- Registers `JSONRepository<>` via dependency injection when local mode is enabled

JSON files are loaded from `Data/Files` under the app runtime folder (copied from `SAPPub.Infrastructure/Data/Files` during build).

---

## Prerequisites

- .NET 10 SDK
- Node.js + npm (for frontend assets)

---

## 1) Clone and restore

```bash
git clone https://github.com/DFE-Digital/sap-public.git
cd sap-public
dotnet restore
```

---

## 2) Install frontend dependencies

From the web project:

```bash
cd SAPPub.Web
npm install
```

---

## 3) Confirm local JSON mode

Set `SAPPub.Web/appsettings.json` to:

```json
"UseLocalDataSource": true
```

By default this is currently `false`, so update it before running in JSON mode.

No local Postgres connection is required for this quick start path.

### Running in VS Code

You can also force JSON mode from `.vscode/launch.json`.

Current launch config:

```json
{
  "name": "Design",
  "type": "node-terminal",
  "request": "launch",
  "command": "dotnet run -e  UseLocalDataSource=\"true\"",
  "cwd": "${workspaceFolder}/SAPPub.Web"
}
```

Run this profile from **Run and Debug** in VS Code to start the app with `UseLocalDataSource=true`.

---

## 4) Run the app with a launch profile

Launch profiles are defined in `SAPPub.Web/Properties/launchSettings.json`.

From `SAPPub.Web`:

```bash
dotnet run --launch-profile https
```

Available local URLs:

- `https://localhost:3000`
- `http://localhost:5013`

You can also run the `http` profile explicitly:

```bash
dotnet run --launch-profile http
```

---

## 5) Common checks

- If styles/scripts are missing, rerun `npm install`.
- If pages are empty, ensure JSON files exist under `SAPPub.Infrastructure/Data/Files` and rebuild.
- If you need live database-backed data, switch to the full setup guide: `002-dev-setup.md`.