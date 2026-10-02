## Github secrets

The following secrets are stored in GitHub and are required for the data pipeline and review app database restore to run:

- AZURE_CLIENT_ID
- AZURE_SUBSCRIPTION_ID
- AKS_CLUSTER_NAME
- AKS_NAMESPACE
- AKS_RESOURCE_GROUP
- AKS_REVIEW_NAMESPACE
- AZURE_STORAGE_CONNECTION_STRING
- AZURE_STORAGE_CONTAINER
- AZURE_TENANT_ID
- KONDUIT_APP_NAME
- SENSITIVE_DATASET_URL - GIAS
- SENSITIVE_ESTABLISHMENT_LINKS_URL - GIAS
- SENSITIVE_MAT_LINKS_URL - GIAS


### Environment secrets

'AZURE_CLIENT_ID' and 'AZURE_SUBSCRIPTION_ID' are set per GitHub environment (review, test and production). They identify the managed identity used to login to the Azure with OIDC.

### Blob storage

The storage connection string is **not** stored in GitHuhb. After logging in to Azure with OIDC, the workflows fetch it from the environment's storage account with `az storage account show-connection-string`

| Environment | Storage account | Container |
| --- | --- | --- |
| Review | st189t01sappubdptssa (test) | alldata | 
| Test | st189t01sappubdptssa | alldata | 
| Production | st189t01sappubdppdsa | alldata | 

The storage accounts and containers are created by terraform (`terraform/application/storage.tf` and the `containers` list in `terraform/applicaton/config/<env>.tfvars.json`). Review apps have no storage of their own - they use the test one.