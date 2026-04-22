
--\COPY <X> TO '/tables/<x>.csv' WITH (FORMAT CSV, HEADER);


\COPY (select * from gateway_local_authority ORDER BY "LocalAuthorityName" DESC) TO 'output/local_authority.csv' WITH (FORMAT CSV, HEADER);
\COPY (select * from gateway_settings) TO 'output/settings.csv' WITH (FORMAT CSV, HEADER);
\COPY (select * from gateway_user ORDER BY "CreatedOn") TO 'output/user.csv' WITH (FORMAT CSV, HEADER);
\COPY (select * from gateway_user_audit ORDER BY "CreatedOn") TO 'output/user_audit.csv' WITH (FORMAT CSV, HEADER);