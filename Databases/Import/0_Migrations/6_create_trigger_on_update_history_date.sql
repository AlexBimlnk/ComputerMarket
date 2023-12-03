\c import
BEGIN;

CREATE OR REPLACE FUNCTION update_history_date()
RETURNS TRIGGER
AS
$trg$
BEGIN
	NEW.receive_date = NOW();
	RETURN NEW;
END
$trg$
LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_update_history_date ON histories;
CREATE TRIGGER trg_update_history_date BEFORE UPDATE ON histories
    FOR EACH ROW
    EXECUTE PROCEDURE update_history_date();

COMMIT;