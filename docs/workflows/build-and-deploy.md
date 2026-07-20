# Build and deploy workflow

The main workflow for building and deploying the application to the specified environment.

PR with 'deploy' label when you want to build a review deployment, 
Push to main branch when you want to deploy to test.
Manual trigger when you want to deploy to production.

# General Flow

```mermaid
flowchart TD

    subgraph Inputs
        A1["Code Change<br/>(PR or Main)"]
        A2["Deployment Environment"]
    end

    subgraph Build & Deploy Process
        B1["Build & Test Application"]
        B2["Publish Docker Image<br/>GHCR"]
        B3["Deploy Application to AKS"]
        B4["Restore Review Seed DB<br/>(Review Deployments Only)</br>from db-backups/sappub_review_seed_latest.sql.gz"]

        B1 --> B2
        B2 --> B3
    end

    subgraph Outputs
        C1["Docker Image<br/>GitHub Container Registry"]
        C2["Test Results<br/>GitHub Artifacts"]
        C3["AKS Application Deployment"]
        C4["PR Review DB"]
    end

    A1 --> B1
    A2 --> B3

    B2 --> C1
    B1 --> C2
    B3 --> C3

    B3 --> B4
    B4 --> C4
```

# Review Deployment with Refresh Data

When your code change includes changes to the data pipeline and you want to deploy to a review app.
```mermaid
flowchart TD

    subgraph Inputs
        A1["Code Change<br/>(PR)"]
        A2["refresh data Label"]
        A3["GIAS Datasets"]
        A4["EES Datasets"]
        A5["Manual Files<br/>Azure Blob Storage"]
    end

    subgraph Refresh Data Process
        B1["Build & Test Application"]
        B2["Deploy Review Environment"]
        B3["Run School Data Ingestion Pipeline"]
        B4["Download & Version Source Data"]
        B5["Generate SQL & Run ETL"]
        B6["Populate Review Database"]

        B1 --> B2
        B2 --> B3
        B3 --> B4
        B4 --> B5
        B5 --> B6
    end

    subgraph Outputs
        C1["Review Application"]
        C2["Review Database<br/>(Freshly Built from Source Data)"]
    end

    A1 --> B1
    A2 --> B3
    A3 --> B4
    A4 --> B4
    A5 --> B4

    B2 --> C1
    B6 --> C2
```