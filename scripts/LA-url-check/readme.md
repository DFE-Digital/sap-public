# LA Check script


Both scripts in here are a WIP, and have a built with heavy support from CoPilot. 

## Powershell

First one is powershell, navigate to the folder and type .\lacheck.ps1, it'll do a basic check for the url. 
80ish% returned fine, the others are 403 forbidden i.e. they have bot protection on. 

## Playwright (via npm)

The second attempt was to run a playwright session and check the urls, this is much slower and still not as reliable. 

## Tl;dr 

Neither of these are 100% guaranteed, and some review/planning would be needed in future to automate these scripts. 