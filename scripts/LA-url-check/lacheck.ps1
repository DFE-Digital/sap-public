$inputCsv = "manual_la_urls.csv"
$outputCsv = "url-statuses.csv"

$headers = @{
    'User-Agent' = 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0 Safari/537.36'
}

$results = Import-Csv $inputCsv | ForEach-Object {

    $croppedUrl = $_.'Cropped URL'

    $statusCode = $null
    $testedUrl = $null
    $finalUrl = $null
    $finalHost = $null
    $errorMessage = $null

    $testUrls = @(
        "https://$croppedUrl"
        "https://www.$croppedUrl"
        "http://$croppedUrl"
        "http://www.$croppedUrl"
    )

    foreach ($url in $testUrls) {

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
        CroppedURL = $croppedUrl
        TestedUrl  = $testedUrl
        FinalUrl   = $finalUrl
        FinalHost  = $finalHost
        StatusCode = $statusCode
        Error      = $errorMessage
    }
}

$results | Export-Csv $outputCsv -NoTypeInformation

Write-Host "Results exported to $outputCsv"