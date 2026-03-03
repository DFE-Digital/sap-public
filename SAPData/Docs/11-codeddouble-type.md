## CodedDouble – how it works

`CodedDouble` is used for KS4 metrics where a value may be numeric **or** suppressed (for example: confidentiality, not applicable, unavailable).

Instead of returning just a `double?`, we store:
- the raw coded value from the database
- the parsed numeric value (if available)
- a human-readable reason when it isn’t

### Entity pattern

Each metric follows this structure:

<pre>
```csharp
public CodedDouble Metric_Coded { get; set; }
public double? Metric { get; set; }
public string Metric_Reason { get; set; }
</pre>

### Data flow 

Database views expose *_Coded columns
Dapper maps them into CodedDouble via a type handler (`CodedDoubleTypeHandler.cs`)
The coded value mapper (`ReflectionCodedValueMapper.cs`):
extracts numeric values when possible
translates suppression codes into readable reasons

### Services and UI:

use the numeric value if present
otherwise display the reason

### Suppression codes
Examples:

z → Not applicable
c → Redacted for confidentiality
x → Not available
low → Positive % less than 0.5

These are defined during dependency registration (`DependenciesExtensions.cs`) and can be updated centrally as needed.

Why this exists

Keeps repositories simple (no parsing logic)
Standardises handling of suppressed stats
Supports both calculations and UI messaging from the same model
Scales consistently across all KS4 datasets