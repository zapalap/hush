CREATE OR REPLACE FUNCTION update_message_geom() 
RETURNS TRIGGER 
AS $$
DECLARE
	latitude double precision;
	longitude double precision;
BEGIN
latitude := NEW.lat;
longitude := NEW.long;

  NEW.geom := ST_GeomFromText('POINT(' || longitude || ' ' || latitude || ')', 4326);
RETURN NEW;
END $$ LANGUAGE plpgsql;


CREATE TRIGGER update_message_geom_trigger
BEFORE INSERT OR UPDATE ON messages
FOR EACH ROW 
EXECUTE PROCEDURE update_message_geom();