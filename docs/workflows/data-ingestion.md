# Data ingestion pipeline

## Run as a scheduled task only

```mermaid
flowchart TD

    subgraph Workflow Inputs
        A1["Environment<br/>test"]
    end

    subgraph Data Inputs
        D1["GIAS Datasets"]
        D2["EES Datasets"]
        D3["Manual Source Files<br/>Azure Blob Storage"]
    end

    B["School Data Ingestion Pipeline<br/>Ingest & Build Data"]

    subgraph Outputs
        C1["Refreshed Application DB<br/>in Test Environment"]
        C2["Versioned Source Files<br/>Azure Blob Storage"]
        C3["Review Seed Backup<br>Azure Blob Storage<br/>container:<br/>/alldata/db-backups/ <br/>file:<br/>sappub_review_seed_latest.sql.gz"]
    end

    A1 --> B

    D1 --> B
    D2 --> B
    D3 --> B

    B --> C1
    B --> C2
    B --> C3
```

## If it were called from another pipeline
```mermaid
flowchart TD

    subgraph Workflow Inputs
        A1["Environment<br/>(Test, production, review)"]
        A2["PR Number<br/>(review only)"]
    end

    subgraph Data Inputs
        D1["GIAS Datasets"]
        D2["EES Datasets"]
        D3["Manual Source Files<br/>Azure Blob Storage"]
    end

    B["School Data Ingestion Pipeline<br/>Ingest & Build Data"]


    A1 --> B
    A2 --> B

    D1 --> B
    D2 --> B
    D3 --> B

```

