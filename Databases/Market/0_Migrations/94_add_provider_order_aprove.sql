\c market
BEGIN;
UPDATE db_version SET version = 13 WHERE TRUE;

ALTER TABLE order_fill
DROP COLUMN IF EXISTS is_approved;

ALTER TABLE order_fill
ADD COLUMN is_approved BOOL NULL;

CREATE OR REPLACE FUNCTION trg_chane_order_state_when_all_approve()
RETURNS TRIGGER
AS
$trg$
DECLARE
    count BIGINT;
    state INT;
BEGIN
    SELECT o.state_id INTO state FROM orders o WHERE o.id = OLD.order_id;
    SELECT COUNT(*) INTO count FROM order_fill o WHERE o.order_id = OLD.order_id AND (o.is_approved IS NULL OR o.is_approved = false) /*AND
                                                       (o.item_id != OLD.item_id AND o.provider_id != OLD.provider_id)*/;
    IF count = 0 AND state = 3
        THEN
            BEGIN
                UPDATE orders SET state_id = 4 WHERE id = OLD.order_id;
            END;
    END IF;
    SELECT o.state_id INTO state FROM orders o WHERE o.id = OLD.order_id;

	RETURN NEW;
END
$trg$
LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_change_order_state_when_all_approves ON order_fill;
CREATE TRIGGER trg_change_order_state_when_all_approves AFTER UPDATE ON order_fill
    FOR EACH ROW
    EXECUTE PROCEDURE trg_chane_order_state_when_all_approve();
COMMIT;