# Council URL Canonicalisation Checker (Playwright)

This script uses Playwright and Chromium to discover the final destination URL for a list of council domains.

Unlike `Invoke-WebRequest`, Playwright behaves like a real browser, allowing it to:

- Follow HTTP redirects (301/302/etc.)
- Follow JavaScript redirects
- Handle many sites protected by Cloudflare, Akamai, and similar services
- Capture the final URL and hostname after all redirections

## Prerequisites

Install Node.js:

https://nodejs.org

Install Playwright:

```powershell
npm init -y
npm install playwright
npx playwright install chromium
```

## Input

Create a CSV file named:

```text
manual_la_urls.csv
```

Example:

```csv
GSS,Authority Name,Cropped URL
E08000016,Barnsley Metropolitan Borough Council,barnsley.gov.uk
E06000022,Bath and North East Somerset Council,bathnes.gov.uk
```

The script uses the **Cropped URL** column.

## Running

Execute:

```powershell
node check-urls.js
```

## Output

The script creates:

```text
url-statuses.csv
```

Example output:

```csv
CroppedURL,TestedURL,FinalURL,FinalHost,StatusCode,Error
new.devon.gov.uk,https://new.devon.gov.uk,https://www.devon.gov.uk/,www.devon.gov.uk,200,
barnsley.gov.uk,https://barnsley.gov.uk,https://www.barnsley.gov.uk/,www.barnsley.gov.uk,200,
```

## Output Columns

- **CroppedURL**: Domain from the source CSV
- **TestedURL**: Initial URL opened by Playwright
- **FinalURL**: URL after all redirects
- **FinalHost**: Final hostname after redirects
- **StatusCode**: HTTP status code from initial navigation
- **Error**: Any navigation error encountered

## Notes

- Navigation timeout is 30 seconds per site.
- The script uses headless Chromium.
- Each URL is opened in a fresh browser page to avoid cookies and session data affecting subsequent results.
- Some highly protected sites may still block automated browsers and appear in the `Error` column for manual review.
