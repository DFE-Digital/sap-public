# Konduit/DB cheatsheet

Konduit is Komplicated and often quite Komplex. 

Here's some quick commands I (Dan) have built up to run with:

## Zero to Hero (I forgot what to do and need to get to postgres)

New Command Prompt

```
cd wsl
wsl
az login (may need tenant or device code login)
```

This will - 
Move to folder that contains our makefile scripts, and anything else fun or interesting
Open Windows Subsystem for Linux (WSL)
Initiate windows login


This step could be improved, but needs some testing and understanding (of which I have little)
```
make test get-cluster-credentials

source ~/.bashrc
export KUBECONFIG=/mnt/c/Users/<user>/.kube/config

kubectl -n sip-test get deployments
```

Use the Makefile to prep us for the test environment

Set the config created into the kubeconfig config (This seems like there should be a better way to do this)

Use Kubectl on sip-test to "get deployments" This will list all deployments/pods available in the area.

## Connect to a specific pod/environment

Connect to PSQL for PR661
```
bin/konduit.sh -n sip-development -x sap-public-pr-661 -- psql
```


Connect to PSQL for TEST
```
bin/konduit.sh -n sip-test -x sap-public-test -- psql
```

The following relies on being connected to the Prod config 
```
    make production get-cluster-credentials CONFIRM_PRODUCTION=yes
    source ~/.bashrc
    export KUBECONFIG=/mnt/c/Users/<user>/.kube/config
```



```
bin/konduit.sh -n sip-production -x sap-public-production -- psql
```


## View ConfigMaps/ Secrets

These will connect to a pod to show it's configmap or secrets, but may need two-step to get the actual details (pods and pr etc will change)

```
kubectl -n sip-development get cm |grep sappub-pr-294
kubectl -n sip-development get cm/sappub-pr-294-b267135e8663adfb73a8870238ff504ef3191638 -o yaml
```

```
kubectl -n sip-development get secrets |grep sappub-pr-294
kubectl -n sip-development get secrets/sappub-pr-294-19e8f23f67bddfefcaf18928892bae193f38325e -o yaml
```

## View status of pod

Can be used to get status of pod, i.e. if it's failing, here's why! (and more)

```
kubectl -n sip-development describe pod get-school-improvement-insights-pr-138
```


## Backup database for local perusal

```
bin/konduit.sh -n "sip-development"  -t 28800  -x  "sap-public-pr-401" -- pg_dump --format=plain --clean --if-exists --no-owner --no-privileges | gzip -c > "sappub_backup.sql.gz"
```

## LOCAL - Open PSQL command in the folder you're current on 

i.e. navigate to output of SAPDATA folder (sql) open new cmd

```
& "C:\Program Files\PostgreSQL\18\bin\psql.exe" -h localhost -U postgres -d postgres
```

Enter local password and then run command

## PSQL commands

View All tables
```
\dt
```

View All Materialised views

```
select schemaname as schema_name, matviewname as view_name from pg_matviews order by schema_name, view_name;
```

Export data to local CSV
(Once connected via konduit and --psql)

```
\COPY (select * from v_england_performance where "Id" = 'National') TO 'v_england_performance.csv' WITH (FORMAT CSV, HEADER);
\COPY (select * from v_establishment_performance ) TO 'v_establishment_performance.csv' WITH (FORMAT CSV, HEADER);
\COPY (select * from v_la_performance ) TO 'v_la_performance.csv' WITH (FORMAT CSV, HEADER);
```
