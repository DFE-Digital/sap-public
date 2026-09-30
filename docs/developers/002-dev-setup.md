# SAPPub.Web – Local Development Setup

This guide explains how to run the **SAPPub.Web** project locally.

---

## Support Requests

It will probably save time in the long run by submitting these requests before starting to set up the development environment, as some may take a number of days to be approved and then actioned.


1. Request Local Admin privileges

 - at: https://dfe.service-now.com/mydfe?id=dfe_ec_pro_dashboard

 - search for 'Elevated Administrative Rights on DFE Device'

 - fill out request

   * Computer name: <computer_name>
   * Access required until: 3/6 months
   * Area of Business: Digital
   * Reason elevated admin access is required: I am a new .Net developer on the sap-public team and need to install all the development tools to develop and test the database and code.

---

2. Request Visual Studio Subscription - (formerly Visual Studio with MSDN)

 - at: https://dfe.service-now.com/mydfe?id=dfe_ec_pro_dashboard

 - search for 'Visual Studio Subscription'

 - fill out request

   * Please describe your request, providing as much detail as possible.: I am a new .Net developer on the sap-public project and need Visual Studio 2026 to undertake development work.
   * Please provide your full DfE email address: <your_dfe_email_address>
   * Employee Type: <select_employee_type>
   * Requested for: <your_name>
   * Line Manager: <your_line_manager>
   * Subscription Type: Visual Studio Professional
   * How long do you require the licence for?: Ongoing

---

3. Request Windows Defender exclusion list

 - at: https://dfe.service-now.com/mydfe?id=dfe_ec_pro_dashboard

 - find form 'Please use this form if you cannot find the relevant request form in any other area of the portal'

 - fill out request

   * Requested for: <your_name>
   * Please give a short description of your request: Windows Defender exclusion list
   * Working from: <select_location>
   * Contact Telephone Number: <your_phone_number>
   * Select an appropriate Category: Non Standard
   * Please describe your request in as much detail as possible. If there is not enough information, your request will be placed on hold until this is received : I am a new developer on the sap-public team and need to be able to run code on my laptop which is blocked by Windows Defender. Please could I be added to the exclusion list.
   * Business Service: End User Computing
   * Service Offering: End User Device Security (Windows Defender)

---

4. request access or remove access to Restricted Groups

 - at: https://dfe.service-now.com/mydfe?id=dfe_ec_pro_dashboard

 - search for 'request access or remove access to Restricted Groups'

 - fill out request

   * Requested By: <your_name>
   * Requested for: <your_name>
   * Do you want to add or remove access: Add Access
   * Which Group(s) would you like to add the user to: Azure Internet Exclude Inspection PA approval
   * Is there any other information you would like to provide?: Required to be able to install Playwright automated testing tool currently being used by the sap-public development team.

---

## Team Requests

1. Request Azure permissions

 - request a current team to raise a ticket for you for 'CIP Access'
 - you should receive an email invite
 - request a current team to raise a ticket for you to be added to the 'DfE Platform'
 - access link: https://portal.azure.com/#servicemenu/Microsoft_Azure_Resources/ResourceManager/browseAll

---

2. Request GitHub permissions

 - request a current team to have you added to the relevant repository for 'sap-public'

---

3. Figma access - screen designs

 - request a current team invite you into Trello
 - you should receive an email invite
 - access link: https://www.figma.com

4. Lucid access- work planning

 - request a current team invite you into Lucid
 - you should receive an email invite
 - access link: https://lucid.app

5. Trello access - agile backlog, sprints and work items

 - request a current team invite you into Trello
 - access link: https://trello.com

---

## Prerequisites

Ensure the following are installed:

* **.NET SDK 10**
* **Node.js (LTS) and npm**
* **PostgreSQL** (or Docker if Postgres is containerised)

Verify installations:

```bash
dotnet --version
node --version
npm --version
```

---

## Clone the repository

```bash
git clone https://github.com/DFE-Digital/sap-public.git
cd sap-public
```

---

## Navigate to the Web project

```bash
cd SAPPub.Web
```

---

## Install frontend dependencies

Frontend assets (CSS/JS) require npm dependencies:

```bash
npm install
```

Re-run this if assets appear broken or missing.

---

## Configure user secrets

The following secrets are required to run the web app locally.
Values are provided by the team and **must not be committed**.

```json
{
  "LOGIT_HTTP_URL": "",
  "LOGIT_API_KEY": "",
  "ConnectionStrings:PostgresConnectionString": ""
}
```

### Set secrets locally

```bash
dotnet user-secrets init

dotnet user-secrets set "LOGIT_HTTP_URL" "<value>"
dotnet user-secrets set "LOGIT_API_KEY" "<value>"


dotnet user-secrets set "ConnectionStrings:PostgresConnectionString" "<value>"
```

Verify:

```bash
dotnet user-secrets list
```

---

## Database setup (SAPData + local Postgres)

SAPPub.Web expects a local Postgres database populated using the **SAPData** project.

> **Note:** CSV data files must be obtained from the team/storage account and **must not be committed**.

### 1) Install Postgres locally

Install Postgres on your machine (or run it via Docker) and make sure you can connect with `psql`.

### 2) Install postgis

 - at: https://postgis.net/documentation/getting_started/install_windows
 - download and install: https://download.osgeo.org/postgis/windows/pg18/postgis-bundle-pg18x64-setup-3.6.2-1.exe
 - the version must match the version of Postgres installed
 - this should also install PgAdmin which is required for viewing the db schema and data

### 3) Install Postgres browser extensions

 - Install Wave Extension for browser
 - Install Axe Accessibility Tool Extension for browser

### 4) Create a local Postgres database

Create an empty database for local development.

### 5) Get the CSV source data

Download **all CSV files** from the **sap-public** storage account `s189t01sappubdptssa`, container `alldata`, into:

* `SAPData/DataMap/SourceFiles`

Do **not** check these files into git.

### 6) Generate SQL scripts using SAPData

From the repo root:

```bash
cd SAPData

dotnet run
```

This generates the SQL scripts used to create/populate tables and views.

### 7) Run all SQL scripts via psql

From the SQL script directory, run:

```bash
psql -d <DATABASE_NAME>
```

Then inside `psql`:

```sql
\i run_all.sql
```

#### If `run_all.sql` fails due to file encoding

Some scripts can fail due to encoding issues.

Fix by re-saving the failing script(s) as **UTF-8 without signature**:

* Visual Studio: **File → Save As… → Save with encoding… → Unicode (UTF-8 without signature)** (Code page 65001)

Re-run:

```sql
\i run_all.sql
```

### 8) Point SAPPub.Web at the local database

Set the connection string in user secrets:

```bash
dotnet user-secrets set "ConnectionStrings:PostgresConnectionString" "<value>"
```

---

## Run the application

From the `SAPPub.Web` directory:

```bash
dotnet run
```

The application URLs will be shown in the console output.

---

## Common issues

### Frontend assets not loading

```bash
npm install
dotnet run
```

### Search index not initialised

* Required CSV file is missing
* Verify CSV path/configuration and rerun

### Schools not displayed

* Incorrect Postgres connection string
* Database not seeded
* Required scripts not run

---

## Debug checklist

1. Check console logs (first error is usually the root cause)
2. Verify secrets:

   ```bash
   dotnet user-secrets list
   ```
3. Confirm database connectivity using a Postgres client
4. Re-run:

   ```bash
   npm install
   dotnet run
   ```

---

If issues persist, share the **first startup error or stack trace** with the team.


## Running the tests
In Visual Studio, you can run the tests using the Test Explorer.


### Install Playwright

This is dependent on '4. request access or remove access to Restricted Groups' having been complete.

In PowerShell:
1. change directory to 'CD C:\dev\git\sap-public\Tests\SAPPub.Integration.Tests\bin\Debug\net10.0'
2. run 'powershell -ExecutionPolicy Bypass -File playwright.ps1 install'

### Running Playwright tests in headed mode

To run the Playwright tests in **headed mode**, configure your test run to use the `playwright.runsettings` file.

In Visual Studio:
1. Open the **Test** menu
2. Select **Configure Run Settings**
3. Choose **Select Solution Wide Run Settings File**
4. Select `playwright.runsettings`

### E2E Tests

We have end-to-end tests that run against a deployed review app and its database.
They use Playwright to test the application in a browser, scrape the data displayed on the page and compare it to expected values. 
These tests are located in the `SAPPub.E2E.Tests` project.

To run locally:
in VS menu: Tests->Configure run settings-> select the playwright.runsettings file from the solution root folder
1. run the web app in a terminal: `dotnet run --project SAPPub.Web --no-build`
2. run the tests in a separate terminal: `dotnet test Tests/SAPPub.E2E.Tests --no-build`
 *or* run them from test explorer (ensuring you've followed the steps above for configuring playwright.runsettings)