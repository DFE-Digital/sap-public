
# Troubleshooting

## GIAS file not found

Causes:

- File not yet published
- Wrong secret URL template
- Missing {fileDatePostfix}

## EES no published versions

Check dataset versions endpoint.

## SQL execution failures

Common causes:

- Column changes
- DataMap mismatch - check filename in map match filenames in blob (minus 'manual_')
- View dependency order
