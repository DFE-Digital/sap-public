$inputCsv = "manual_la_urls.csv"
$outputCsv = "url-statuses.csv"

$headers = @{
    'User-Agent'      = 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0 Safari/537.36'
    'Accept'          = 'text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8'
    'Accept-Language' = 'en-GB,en;q=0.9'
}

$results = Import-Csv $inputCsv | ForEach-Object {

    $croppedUrl = $_.'Cropped URL'
    $gss = $_.'GSS'
    $authority = $_.'Authority Name'

    $statusCode = $null
    $testedUrl = $null
    $finalUrl = $null
    $finalHost = $null
    $errorMessage = $null

    $testUrls = @(
    "$croppedUrl"
    #    "https://$croppedUrl"
    #    "https://www.$croppedUrl"
    #    "http://$croppedUrl"
    #   "http://www.$croppedUrl"
    )

    foreach ($url in $testUrls) {

        #$url = ([uri]$url).GetLeftPart([System.UriPartial]::Authority)

        Write-Host "Testing $url"

        try {
            $response = Invoke-WebRequest `
                -Uri $url `
                -Method Get `
                -MaximumRedirection 10 `
                -Headers $headers `
                -TimeoutSec 30 `
                -ErrorAction Stop

            $statusCode = [int]$response.StatusCode
            $testedUrl = $url
            $finalUrl = $response.BaseResponse.ResponseUri.AbsoluteUri
            $finalHost = $response.BaseResponse.ResponseUri.Host

            break
        }
        catch {

            if ($_.Exception.Response) {

                $webResponse = $_.Exception.Response
                $statusCode = [int]$webResponse.StatusCode

                # Look for redirect target even on error responses
                $location = $webResponse.Headers['Location']

                if ($location) {

                    try {

                        if ($location.StartsWith('/')) {
                            $uri = [System.Uri]::new($url)
                            $location = [System.Uri]::new($uri, $location).AbsoluteUri
                        }

                        $redirectResponse = Invoke-WebRequest `
                            -Uri $location `
                            -Method Get `
                            -MaximumRedirection 10 `
                            -Headers $headers `
                            -TimeoutSec 30 `
                            -ErrorAction Stop

                        $testedUrl = $url
                        $finalUrl = $redirectResponse.BaseResponse.ResponseUri.AbsoluteUri
                        $finalHost = $redirectResponse.BaseResponse.ResponseUri.Host
                        $statusCode = [int]$redirectResponse.StatusCode

                        break
                    }
                    catch {
                    }
                }

                # Accept non-403 responses as valid findings
                if ($statusCode -ne 403) {
                    $testedUrl = $url
                    $finalUrl = $url
                    $finalHost = ([uri]$url).Host
                    break
                }
            }

            $errorMessage = $_.Exception.Message
        }
    }

    [PSCustomObject]@{
        GSS       = $gss
        'Authority Name' = $authority
        TestedUrl  = $testedUrl
        StatusCode = $statusCode
        Error      = $errorMessage
    }
}

$results | Export-Csv $outputCsv -NoTypeInformation

Write-Host "Results exported to $outputCsv"