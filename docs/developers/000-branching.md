# Branching workflow

This document explain current Standard feature development workflow and also using Long running branch workflow.

1. Standard feature flow — developer branches directly from main and merges back into main using PR process.

2. Long-running feature flow — a long-running branch is created from main; developers branch from it, merge back into it using PR process, and eventually the long-running branch is merged into main using PR Process.

```mermaid
gitGraph
    commit id: "Last Commit / Main"

    branch feature-A
    checkout feature-A
    commit id: "Developer A work"
    commit id: "More changes"
    checkout main
    merge feature-A tag: "PR approved"

    branch long-running-feature
    checkout long-running-feature
    commit id: "Long-running stream starts"

    branch feature-B
    checkout feature-B
    commit id: "Developer B work"
    commit id: "More changes"
    checkout long-running-feature
    merge feature-B tag: "PR approved"

    checkout main
    checkout long-running-feature
    merge main tag: "Sync with main"

    branch feature-C
    checkout feature-C
    commit id: "Developer C work"
    checkout long-running-feature
    merge feature-C tag: "PR approved"

    checkout main
    checkout long-running-feature
    merge main tag: "Keep updated with main"

    checkout main
    merge long-running-feature tag: "Final PR approved"
```

```mermaid
flowchart TD
    MAIN["main"]

    %% Stream 1
    MAIN --> F1["Developer creates Feature Branch"]
    F1 --> F1W["Developer works on feature"]
    F1W --> PR1["Create Pull Request"]
    PR1 --> REV1{"Review & Approval"}
    REV1 -->|Changes required| F1W
    REV1 -->|Approved| M1["Merge Feature Branch → main"]

    %% Stream 2
    MAIN --> LRB["Create Long-Running Feature Branch"]

    LRB --> SYNC["Regularly sync Long-Running Branch with main"]

    LRB --> F2["Developer creates Feature Branch"]
    F2 --> F2W["Developer works on feature"]
    F2W --> PR2["Create Pull Request"]
    PR2 --> REV2{"Review & Approval"}
    REV2 -->|Changes required| F2W
    REV2 -->|Approved| M2["Merge Feature Branch → Long-Running Branch"]

    M2 --> MORE{"More features/work remaining?"}
    MORE -->|Yes| F3["Create next Developer Feature Branch"]
    F3 --> F3W["Developer works on feature"]
    F3W --> PR3["Create Pull Request"]
    PR3 --> REV3{"Review & Approval"}
    REV3 -->|Changes required| F3W
    REV3 -->|Approved| M2

    MORE -->|No - Ready for Main| FINAL["Create Pull Request\nLong-Running Branch → main"]
    FINAL --> REV4{"Final Review & Approval"}
    REV4 -->|Changes required| SYNC
    REV4 -->|Approved| MERGE["Merge Long-Running Branch → main"]

    SYNC --> F2

    classDef main fill:#238636,color:#fff,stroke:#1a632c;
    classDef branch fill:#8957e5,color:#fff,stroke:#6e40aa;
    classDef pr fill:#0969da,color:#fff,stroke:#0757b5;
    classDef review fill:#d29922,color:#fff,stroke:#9a6700;
    classDef work fill:#f6f8fa,color:#24292f,stroke:#8c959f;

    class MAIN,M1,MERGE main;
    class LRB,F1,F2,F3 branch;
    class PR1,PR2,PR3,FINAL pr;
    class REV1,REV2,REV3,REV4 review;
    class F1W,F2W,F3W,SYNC,M2 work;
```

## Simple explanation
### Stream 1 — Normal feature development

This is your existing process.

1. Start from main
    
    - main contains the current approved code.

2. Developer creates a feature branch
    
    > - For example:
    >
    >       main ---> feature/customer-search

3. Developer does their work
    
    - All changes for that feature are made in the feature branch.
    
    - main is not directly changed by the developer.

4. Developer creates a Pull Request
    
    - When the feature is ready, they create a PR:
    
    - feature/customer-search → main

5. Code review takes place
    
    - Other developers/reviewers review the changes.
    
    - If changes are needed, the developer updates the feature branch.
    
    - The PR is reviewed again.

6. PR is approved
    
    - Once the required reviewers approve it, the PR can be merged.

7. Feature branch is merged into main
    
    - The completed feature becomes part of the main codebase.

### Stream 2 — Long-running feature development

This is the new process for work that is too large or takes too long to go directly into main.

- Long running feature branch process only will be used for changes which can't be put behind feature flag and impact running data pipelines and application against old data sets.

1. Create a Long-Running Feature Branch
   
   - Start from main and create something like:
        
    >    main → long-running-feature
    >    
    >    - Think of this branch as a temporary version of main dedicated to a larger piece of work.
    >    
    >    - It may contain multiple features developed by multiple developers.

2. Keep the Long-Running Branch updated
    
    - As other work gets merged into main, the long-running branch should be regularly updated from main.
        
    >    For example:
    >        
    >        main → normal-feature-1
    >        
    >        main → normal-feature-2
    >        
    >        main → long-running-feature
    
    - The long-running branch periodically incorporates the latest changes from main.
    
    - This is important because the long-running branch may exist for weeks or months. Keeping it updated reduces the risk of having a huge number of conflicts when you eventually merge it back into main.

3. Developer creates a branch from the Long-Running Branch

    - Instead of branching from main, developers working on this stream branch from the long-running branch.
        
    >    For example:
    >        main → long-running-feature
    >    
    >               long-running-feature → feature-A
    >                
    >                long-running-feature → feature-B
    >                
    >                long-running-feature → feature-C

    - So a developer working on Feature A creates:
        
        - feature-A from: long-running-feature

4. Developer works on their feature
    
    - The developer makes their changes in their own feature branch.
     
      >  For example:
      >  
      >      long-running-feature → feature-A
      >                             feature-A → commit 1
      >                             feature-A → commit 2
      >                             feature-A → commit 3

    - They can work normally without directly changing the long-running branch.

5. Developer creates a Pull Request
    
    - When their work is ready, they create:
        
    >    - feature-A → long-running-feature
    >    
    >    - Not: feature-A → main
    
    - This is the key difference between the two streams.

6. Review and approval happens
    
    - The team reviews the PR just as they do with the normal process.
    
    >   If changes are required:
    >
    >   - Developer → makes changes → PR reviewed again
    >
    >   If everything is approved:
    >
    >   - feature-A → long-running-feature

    - The feature is merged into the long-running branch.

7. Repeat for other features
    
    - Other developers can independently create branches from the long-running branch:
      
      >  long-running-feature → feature-A
      >
      >  long-running-feature → feature-B
      >
      >  long-running-feature → feature-C
      >
      >  long-running-feature → feature-D

    - Each feature follows the same process:
      
      >  Developer Branch → Developer Work → PR → Code Review → Approval → Merge → Long-Running Branch

    - The long-running branch therefore becomes the integration branch for that stream of work.

8. Continue synchronizing with main
    
    - While developers are working, normal development may continue on main.
      
      >  For example:
      >
      >      main → normal feature
      >
      >      main → bug fix
      >
      >      main → another feature
      >
      >      main → ...
    
    - You should regularly bring those changes into the long-running branch.

      >  Conceptually: (Regular Sync)
      >  
      >  main → long-running-feature
    
    - This means the long-running branch contains:

        - the latest relevant changes from main

        - plus the work being developed for the long-running feature

9. When all work is complete
    
    - Once all the features belonging to the long-running stream are finished, reviewed, and merged into the long-running branch, you have something like:
        
      >  main → long-running-feature
      >
      >                             → feature-A ✓
      >
      >                             → feature-B ✓
      >
      >                             → feature-C ✓
      >                             
      >                             → feature-D ✓

    - At this point, the long-running branch is considered ready to go back to main.

10. Create the final Pull Request
    
    > Now create:
    >
    >   - long-running-feature → main
    >    
    >        - This is a separate PR from the individual feature PRs.
    >    
    >        - The purpose of this final PR is to review the complete set of changes that will be introduced into main.

11. Final review and approval
    
     - The team reviews the final PR.
       
    >    - If problems are found:
    >        
    >        - Long-running branch → Apply Fix / update → Final PR reviewed again
    >    
    >    - If everything is approved:
    >       
    >       - long-running-feature → main
    
    - The entire long-running stream is now integrated into main.