# DataMap (datamap.csv) - View Mapping Configuration Guide

The file `datamap.csv` defines how raw CSV source data is transformed into SQL warehouse views.

It is the primary configuration layer that controls:

- Which datasets feed which views
- Which source columns become output columns
- Filtering rules
- Normalisation rules
- Column-level inclusion/exclusion

The SQL generator reads this file and produces materialized views prefixed with `v_`.

---

## File Location

DataMap lives at:

SAPData/DataMap/datamap.csv

Master copy is here:  https://educationgovuk.sharepoint.com/:x:/r/sites/TeacherServices/_layouts/15/Doc.aspx?sourcedoc=%7B2CF800C6-FDCF-4AFE-9E4F-6D881854D889%7D&file=Data%20Map%20Overview.xlsx&action=default&mobileredirect=true

Raw source files used by DataMap are downloaded to:

SAPData/DataMap/SourceFiles/

The generator reads both to build SQL.

---

## High-Level Concept

Each row in datamap.csv describes:

- One output column
- For one output view
- From one source file
- With optional filters and transformations

Multiple rows combine to form a complete view.

---

## Column Groups

The DataMap columns fall into logical groups:

- Metadata and documentation
- Dataset identity
- Source mapping
- Filtering
- Normalisation
- Multi-record handling
- Control flags

Each group is described below.

---

# Metadata and Documentation Columns

These columns exist to make the mapping understandable to humans. They do not directly affect SQL generation logic.

## Range

Human grouping label.

Examples:
- KS4
- Absence
- Workforce

Used for grouping rows in spreadsheets.

---

## PropertyName

Logical name of the metric or attribute.

Example:
- Attainment8
- PersistentAbsenceRate


---

## PropertyDescription

Long human-readable description.

Example:
- Percentage of pupils achieving grade 5 or above in English and Maths.

Used for documentation only.

---

## PropertyDescriptionShort

Short description version.

Useful for UI display or summaries.

---

# Dataset Identity Columns

These fields identify which dataset a mapping row belongs to.

They must match entries in raw_sources.json and downloaded source files.

---

## Source

Identifies the data provider.

Typical values:
- GIAS
- EES

Used by the generator to resolve correct dataset origin.

---

## SectorCount / PublicCount

Used for dataset classification and reporting logic.

May be used to differentiate public vs sector statistics.

If not used by the generator logic, they remain informational.

---

## Type

Dataset category.

Examples:
- Performance
- Absence
- Workforce

Used to group datasets and align with raw_sources.json.

---

## Subtype

Sub-category within Type.

Examples:
- KS4
- Primary
- Secondary

Helps distinguish similar dataset families.

---

## Year

Academic year identifier.

Examples:
- 2024
- 202425

Used to align with dataset naming and versioning.

---

## YearDesc

Human readable year label.

Example:
- 2024 to 2025 academic year

Used for documentation and reporting metadata.

---

# Source Mapping Columns

These columns control how the generator reads raw input data.

---

## File

Logical source file key.

Must match the logical file name resolved by Step 6 and Step 7.

Examples:
- edubasealldata20260128
- ks4performance_v1.2

This is NOT the physical SQL table name.

The generator resolves this logical name to a physical table via:

SAPData/Sql/tablemapping.csv

---

## Field

Column name in the raw CSV file.

Important rules:

- Must match CSV header exactly
- Case-sensitive (PostgreSQL quoted identifiers)
- No automatic aliasing

Example:
- "URN"
- "Attainment8Score"

---

## DataType

Primary SQL datatype for the output column.

Examples:
- integer
- numeric
- text
- boolean
- date

The generator casts source fields using this type.

---

## DataTypeAlt

Alternative datatype.

Used when:

- Source columns change type
- Legacy datasets require fallback handling

If not required, leave blank.

---

# Record Filtering Columns

These fields filter rows at extraction time.

They allow DataMap to define row-level selection logic.

---

## RecordFilterBy

Column used to apply filtering.

Example:
- SchoolType
- Gender

---

## Filter / FilterValue

Primary filter condition.

Example:

RecordFilterBy: Gender  
Filter: =  
FilterValue: Male  

This becomes:

WHERE Gender = 'Male'

---

## Filter2 through Filter5

Additional filters.

These allow combining up to five conditions per mapping row.

Example:

Filter2: !=  
Filter2Value: NotApplicable  

Used to build compound WHERE clauses.

---

# Normalisation Columns

These support lookup-driven value standardisation.

---

## ShouldBeNormalised

If set to Y:

- Generator applies lookup transformation

Example use cases:

- Convert codes to readable labels
- Normalise inconsistent categorical values

---

## NormalisedLookup

Name of lookup mapping to apply.

Example:
- SchoolPhaseLookup
- GenderCodeLookup

Lookup definitions live in generator configuration or reference tables.

---

# Compound and Multi-Record Columns

These control advanced mapping behavior.

---

## CompoundFields

Used when a logical output column is composed from multiple source columns.

Example:

- Combining subject code + grade into one output field.

Generator implementation determines exact behaviour.

---

## ReturnMultipleRecords

If Y:

- Mapping produces multiple output rows per source record.

Common use case:

- Subject-level breakdowns
- Repeating attributes

Default behaviour is one output row per source row.

---

# Control Flags

These fields control whether mappings are active.

---

## IgnoreMapping

IgnoreMapping controls whether a DataMap row is used for **semantic warehouse view construction** or treated as part of a **mirrored raw-structure view**.

When IgnoreMapping = Y:

- A mirrored SQL view is produced that matches the source CSV schema

This is used for datasets that will be used by the application to build .

---

# How a View Is Built

Example:

View: v_establishment_performance

DataMap rows:

Row 1 -> maps Attainment8Score  
Row 2 -> maps Progress8Score  
Row 3 -> maps EnglishPassRate  

The generator:

- Resolves raw source table
- Selects mapped fields
- Applies filters
- Applies normalisation
- Builds SELECT clause
- Creates materialized view

---

# Common Developer Mistakes

## Case mismatch in Field

"URN" is not the same as "urn".

Always copy headers directly from CSV.

---

## Referencing files not downloaded in Step 7

Remember:

Only these exist in SourceFiles:

- manual_* blobs
- latest managed blobs from Step 6

---

## Breaking existing columns

Avoid renaming OutputColumns unless absolutely necessary.

Prefer:

- Adding new columns

---

## Overusing filters

Filters reduce row counts silently.

Always verify filtered output size after changes.

---

# Safe Change Workflow

When modifying DataMap:

1. Edit datamap.csv
2. Run generator locally or in pipeline
3. Inspect generated SQL
4. Verify raw CSV headers
5. Confirm Step 7 downloaded expected files
6. Validate view SQL executes successfully

---

# Debugging Tips

Helpful outputs:

SAPData/Sql/tablemapping.csv  
- Maps logical file keys to physical raw tables

Generated SQL view scripts  
- Show exact column references and filters

Postgres errors  
- Usually indicate missing columns or type mismatches

---

# Best Practices

- Keep OutputColumn naming stable
- Document complex mappings in PropertyDescription
- Use IgnoreMapping for tables that will be copied as a mirror of the raw source
- Treat datamap.csv as version-controlled schema definition
