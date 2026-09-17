#
#   Little Powershell script to extract the headers from all CSV files in a given directory and write them to a Markdown file.
#
#   Navigate to the directory containing this script and run it. It will create a file named 'csv_headers.md' in the same directory, listing the headers of all CSV files found.
#
#   To run the script, you can use the following command in PowerShell:
#   .\Extract-CsvHeaders.ps1 -Path "C:\Path\To\Your\CSV\Files" -OutputFile "C:\Path\To\Your\Output\csv_headers.md"
#   or simply run it without parameters to use the current directory and default output file name.
#   .\Extract-CsvHeaders.ps1 in the SourceFiles directory
#



param(
    [string]$Path = (Split-Path -Parent $MyInvocation.MyCommand.Path),
    [string]$OutputFile = (Join-Path (Split-Path -Parent $MyInvocation.MyCommand.Path) 'csv_headers.md')
)

$resolvedPath = Resolve-Path $Path
$csvFiles = Get-ChildItem -Path $resolvedPath -Filter *.csv | Sort-Object Name

$lines = @(
    '# CSV Headers',
    ''
)

foreach ($csv in $csvFiles) {
    $lines += "## $($csv.Name)"

    try {
        $firstLine = Get-Content -Path $csv.FullName -TotalCount 1 -ErrorAction Stop

        if ([string]::IsNullOrWhiteSpace($firstLine)) {
            $headers = '(no header row found)'
        }
        else {
            $delimiter = ','
            $commaCount = ($firstLine.ToCharArray() | Where-Object { $_ -eq ',' }).Count
            $semicolonCount = ($firstLine.ToCharArray() | Where-Object { $_ -eq ';' }).Count

            if ($semicolonCount -gt $commaCount) {
                $delimiter = ';'
            }

            $headerFields = $firstLine -split [regex]::Escape($delimiter)
            $headerFields = $headerFields | ForEach-Object { $_.Trim().Trim('"') }
            $headers = if ($null -ne $headerFields) { $headerFields -join ', ' } else { '(no header row found)' }
        }

        $lines += $headers
    }
    catch {
        $lines += "Error reading file: $($_.Exception.Message)"
    }

    $lines += ''
}

$lines | Set-Content -Path $OutputFile -Encoding UTF8
Write-Host "Processed $($csvFiles.Count) CSV files. Output written to $OutputFile"
