BEGIN;
UPDATE db_version SET version = 13 WHERE TRUE;

DROP TABLE IF EXISTS provider_order_approve;

CREATE TABLE provider_order_approve(
    provider_id BIGINT NOT NULL,
    order_id BIGINT NOT NULL,
    FOREIGN KEY (provider_id) REFERENCES providers(Id) ON DELETE CASCADE,
    FOREIGN KEY (order_id) REFERENCES orders(Id) ON DELETE CASCADE,
    PRIMARY KEY (order_id, provider_id));

CREATE OR REPLACE FUNCTION trg_add_provider_order_approve()
RETURNS TRIGGER
AS
$trg$
DECLARE
    count BIGINT;
    state INT;
BEGIN
    SELECT o.state_id INTO state FROM orders o WHERE o.id = NEW.order_id;
    SELECT o.provider_id INTO count FROM provider_order_approve o WHERE o.order_id = NEW.order_id AND o.provider_id = NEW.provider_id;

    IF NOT FOUND
        THEN
            INSERT INTO provider_order_approve VALUES (NEW.provider_id, NEW.order_id);
    END IF;

	RETURN NEW;
END
$trg$
LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_add_provider_approve_when_insert_item ON order_fill ;
CREATE TRIGGER trg_add_provider_approve_when_insert_item AFTER INSERT ON order_fill
    FOR EACH ROW
    EXECUTE PROCEDURE trg_add_provider_order_approve();

CREATE OR REPLACE FUNCTION trg_chane_order_state_when_all_approve()
RETURNS TRIGGER
AS
$trg$
DECLARE
    count BIGINT;
    state INT;
BEGIN
    SELECT o.state_id INTO state FROM orders o WHERE o.id = OLD.order_id;
    SELECT o.provider_id INTO count FROM provider_order_approve o WHERE o.order_id = OLD.order_id AND o.provider_id = OLD.provider_id;

    IF NOT FOUND AND state = 3
        THEN
            UPDATE orders SET state_id = 4 WHERE id = old.order_id;
    END IF;

	RETURN OLD;
END
$trg$
LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_change_order_state_when_all_approves ON provider_order_approve;
CREATE TRIGGER trg_change_order_state_when_all_approves AFTER DELETE ON provider_order_approve
    FOR EACH ROW
    EXECUTE PROCEDURE trg_chane_order_state_when_all_approve();
COMMIT;