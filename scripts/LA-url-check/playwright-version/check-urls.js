const fs = require('fs');
const { chromium } = require('playwright');

function parseCsv(text) {
    const lines = text.trim().split(/\r?\n/);

    const headers = lines[0]
        .split(',')
        .map(h => h.replace(/^"|"$/g, '').trim());

    return lines.slice(1).map(line => {
        const values = line
            .match(/(".*?"|[^",]+)(?=\s*,|\s*$)/g)
            ?.map(v => v.replace(/^"|"$/g, '').trim()) ?? [];

        const row = {};

        headers.forEach((header, index) => {
            row[header] = values[index] || '';
        });

        return row;
    });
}

function csvEscape(value) {
    return `"${String(value ?? '').replace(/"/g, '""')}"`;
}

(async () => {

    const inputFile = 'manual_la_urls.csv';
    const outputFile = 'url-statuses.csv';

    const data = parseCsv(
        fs.readFileSync(inputFile, 'utf8')
    );

    const browser = await chromium.launch({
        headless: true
    });

    const results = [];

    for (const row of data) {

        const host = row['Cropped URL'];

        let testedUrl = `https://${host}`;
        let finalUrl = '';
        let finalHost = '';
        let statusCode = '';
        let error = '';

        console.log(`Testing ${host}`);

        const page = await browser.newPage({
            userAgent: 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0 Safari/537.36'
        });

        try {

            const response = await page.goto(testedUrl, {
                waitUntil: 'networkidle',
                timeout: 30000
            });

            await page.waitForTimeout(3000);

            finalUrl = page.url();

            if (response) {
                statusCode = response.status();
            }

            try {
                finalHost = new URL(finalUrl).host;
            }
            catch { }

        }
        catch (ex) {
            error = ex.message;
        }
        finally {
            await page.close();
        }

        results.push({
            host,
            testedUrl,
            finalUrl,
            finalHost,
            statusCode,
            error
        });

        console.log(`${host} -> ${finalUrl || error}`);
    }

    await browser.close();

    const output = [
        [
            'CroppedURL',
            'TestedURL',
            'FinalURL',
            'FinalHost',
            'StatusCode',
            'Error'
        ].join(',')
    ];

    for (const row of results) {
        output.push([
            csvEscape(row.host),
            csvEscape(row.testedUrl),
            csvEscape(row.finalUrl),
            csvEscape(row.finalHost),
            csvEscape(row.statusCode),
            csvEscape(row.error)
        ].join(','));
    }

    fs.writeFileSync(
        outputFile,
        output.join('\n'),
        'utf8'
    );

    console.log(`Results written to ${outputFile}`);

})();