# Updates - How to update the datamap (step by step process)

## TODO

## Trim files by year for ingestion

Script below can be used to trim down large source files manually locally before adding to source files for data ingestion. 
TODO: Automate this in data pipeline.

```
Import-Csv -Path "{filename}" | Where-Object { $_.{column} -in {comma separated filter values in single quotes} | Export-Csv -Path "{filename for data ingestion}" -NoTypeInformation -Append -UseQuotes AsNeeded
```
e.g
```
Import-Csv -Path ".\ees_ks4_202324.csv" | Where-Object { $_.time_period -in '202122', '202223', '202324' } | Export-Csv -Path ".\manual_ees_ks4_202324_api.csv" -NoTypeInformation -Append -UseQuotes AsNeeded
```

Same script can be used to trim down large files to provide data sample for QA:

e.g.
```
Import-Csv -Path ".\ees_ks4_202324.csv" | Where-Object { $_.school_urn -in '111731', '148684' } | Export-Csv -Path ".\qa_ees_ks4_202324.csv" -NoTypeInformation -Append -UseQuotes AsNeeded
```