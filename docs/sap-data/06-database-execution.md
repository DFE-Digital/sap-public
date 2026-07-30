
# Database Execution (AKS and konduit)

The pipeline runs SQL inside Kubernetes to access the private PostgreSQL instance.

## What is konduit

konduit.sh creates a secure tunnel into AKS and runs commands inside a pod.

Used to execute psql.

## Execution Flow

GitHub Actions
-> az aks get-credentials
-> kubelogin convert-kubeconfig
-> konduit.sh
-> AKS pod
-> PostgreSQL

## Entry Script

SAPData/Sql/run_all.sql

## Timeouts

konduit timeout is configured in the workflow (for example 7200 seconds).
