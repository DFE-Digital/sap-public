# Disaster Recovery testing
## Goal
Test the main scenarios for disaster recovery so that we can ensure our system can recover quickly and effectively from any disruptions.

## Documentation organisation
Schools Digital have a general disaster recovery plan [here](https://github.com/DFE-Digital/teacher-services-cloud/blob/main/documentation/disaster-recovery.md) 
and disaster recovery test plan that all projects follow.
Completed test plans are [here](https://educationgovuk.sharepoint.com/:f:/r/sites/TeacherServices/Shared%20Documents/DR%20tests?d=wf34b7e9561024e9fa78d757e1fa696f6&csf=1&web=1&e=pMl01W)
This document is intended to complement that plan with specific details about the disaster recovery testing scenarios for this application.

## Prerequisites
- Identified environment for the test e.g. qa, staging, test, etc
- Identified storage account and container for the test
- Identified the technical and non technical stakeholders who will participate in the test, based on the Teacher services list
- Followed the Documentation requirements and Initial set-up as for the [Schools Digital disaster recovery test plan](https://github.com/DFE-Digital/teacher-services-cloud/blob/main/documentation/disaster-recovery-testing.md)

## Scenario 1: Loss of Blob 
- delete a blob from the container
- attempt to restore the blob using the steps outlined in the DR plan

## Scenario 2: Loss of Blob Container
- delete the blob container
- attempt to restore the container using the steps outlined in the DR plan
 
## Verification

After recovery:

1. Confirm the container or blob is visible.
2. Verify the expected files exist.
3. Attempt a database rebuild using the restored data if appropriate.
4. Record recovery time and any issues encountered.