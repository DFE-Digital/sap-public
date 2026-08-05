# Database Backup Workflow

## Purpose

Creates a PostgreSQL database backup and stores it in Azure Blob Storage.


```mermaid
flowchart TD

    subgraph Inputs
        A1["Environment<br/>(Test or Production)"]
        A2["Backup File Name<br/>(optional)"]
        A3["Database Server<br/>(optional)"]
    end

    B["Backup PostgreSQL Database"]

    subgraph Outputs
        C1["SQL Backup File (.sql)"]
        C2["Azure Blob Storage"]
        C3["Teams Notification"]
    end

    A1 --> B
    A2 --> B
    A3 --> B

    B --> C1
    C1 --> C2
    B --> C3
```
## Notes: backup filename

Scheduled backup:
sappub_[env]_YYYY-MM-DD.sql

Manual backup:
sappub_[env]_adhoc_YYYY-MM-DD.sql

Custom backup:
[user-specified-name].sql

