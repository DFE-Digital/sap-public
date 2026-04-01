
--\COPY <X> TO '/tables/<x>.csv' WITH (FORMAT CSV, HEADER);


\COPY (select * from gateway_local_authority) TO '/output/local_authority.csv' WITH (FORMAT CSV, HEADER);
\COPY (select * from gateway_settings) TO '/output/settings.csv' WITH (FORMAT CSV, HEADER);
\COPY (select * from gateway_user) TO '/output/user.csv' WITH (FORMAT CSV, HEADER);
\COPY (select * from gateway_user_audit) TO '/output/user_audit.csv' WITH (FORMAT CSV, HEADER);