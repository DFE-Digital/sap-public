# Disaster recovery

## Goal
Detail the main scenarios for disaster recovery so that we can ensure our system can recover quickly and effectively from any disruptions.

## Documentation organisation
Schools Digital have a general disaster recovery plan [here](https://github.com/DFE-Digital/teacher-services-cloud/blob/main/documentation/disaster-recovery.md) and test plan that all projects follow.
Completed test plans are [here](https://educationgovuk.sharepoint.com/:f:/r/sites/TeacherServices/Shared%20Documents/DR%20tests?d=wf34b7e9561024e9fa78d757e1fa696f6&csf=1&web=1&e=pMl01W)
This document is intended to complement that plan with specific details about the disaster recovery scenarios for this application.

## Loss of Blob storage
Because our database is rebuilt from files in a blob storage container, if that container, or blobs within it, were deleted we would not be able to rebuild the database. 
To mitigate this risk, we  
- enable blob storage container soft delete, so that if it is deleted it can be restored within 7 days
- enable blob soft delete on the container, so that if a blob is deleted it can be restored within 7 days

Options *not* taken:
- enabling blob versioning, which would allow us to restore a blob to a previous version, but this is not needed because we don't update most files this way. 
New data releases are done with new files. The GIAS data file is updated in place, but we have the ability to retrieve it from the GIAS source if needed.
- increasing retention period to more than 7 days, which would increase costs

## How to recover a deleted Container

### Azure Portal

1. Open the Storage Account.
2. Navigate to **Data Storage > Containers**.
3. Select **Show deleted containers**.
4. Find the deleted container.
5. Select **Restore**.

### Azure CLI

List deleted containers:

```bash
az storage container list \
  --include-deleted \
  --account-name <storage-account>
```

Restore the container:

```bash
az storage container restore \
  --account-name <storage-account> \
  --deleted-container-name <container-name> \
  --deleted-container-version <version-id>
```

The `deleted-container-version` value is obtained from the list command output.

## How to recover a deleted Blob

1. Open the Storage Account.
2. Open the relevant container.
3. Select **Show deleted blobs**.
4. Select the deleted blob.
5. Choose **Undelete**.
