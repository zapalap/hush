#!/bin/bash

#Update packages
echo "Updating package list"
sudo sh -c 'echo "deb http://apt.postgresql.org/pub/repos/apt trusty-pgdg main" >> /etc/apt/sources.list' > /dev/null
wget --quiet -O - http://apt.postgresql.org/pub/repos/apt/ACCC4CF8.asc | sudo apt-key add - > /dev/null
sudo apt-get update > /dev/null

#Install PostgreSQL and PostGIS
echo "Getting postgreSQL"
sudo apt-get -y install postgresql-9.4-postgis-2.1 postgresql-contrib > /dev/null

#Update network configuration
echo "Updating network configuration"
sudo service postgresql stop
sudo echo "host all all all password" >> /etc/postgresql/9.4/main/pg_hba.conf
sed -i "s:#listen_addresses = 'localhost':listen_addresses = '*':" /etc/postgresql/9.4/main/postgresql.conf
sudo service postgresql start

#Setup initial database
echo "Setting up database"
sudo -u postgres -i psql <<'EOF' > /dev/null
ALTER USER postgres PASSWORD 'postgres';
CREATE DATABASE hushgis
WITH OWNER = postgres
ENCODING = 'UTF8'
TABLESPACE = pg_default
CONNECTION LIMIT = -1;
ALTER DATABASE hushgis
SET application_name = 'DEFAULT';
       
\connect hushgis
     
CREATE EXTENSION postgis;
CREATE EXTENSION postgis_topology;
CREATE EXTENSION fuzzystrmatch;
CREATE EXTENSION postgis_tiger_geocoder;
CREATE SEQUENCE message_id_seq
INCREMENT 1
MINVALUE 1
MAXVALUE 9223372036854775807
START 103
CACHE 1;
ALTER TABLE message_id_seq
OWNER TO postgres;
       
CREATE TABLE messages
(
 id bigint NOT NULL DEFAULT nextval('message_id_seq'::regclass),
 text character varying,
 geom geometry,
 "long" double precision,
 lat double precision,
 created date,
 CONSTRAINT id PRIMARY KEY (id)
)
WITH (
 OIDS=FALSE
);
ALTER TABLE messages
OWNER TO postgres;
          
CREATE OR REPLACE FUNCTION update_message_geom()
  RETURNS trigger AS
$BODY$
DECLARE
	latitude double precision;
	longitude double precision;
BEGIN
latitude := NEW.lat;
longitude := NEW.long;

NEW.geom := ST_GeomFromText('POINT(' || longitude || ' ' || latitude || ')', 4326);
RETURN NEW;
END $BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION update_message_geom()
  OWNER TO postgres;
        
CREATE TRIGGER update_message_geom_trigger
BEFORE INSERT OR UPDATE
ON messages
FOR EACH ROW
EXECUTE PROCEDURE update_message_geom();
EOF
echo "Done"