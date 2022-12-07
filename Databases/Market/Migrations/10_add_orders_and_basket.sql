BEGIN;

UPDATE db_version SET version = 10 WHERE TRUE;

DROP TABLE IF EXISTS order_fill;
DROP TABLE IF EXISTS orders;
DROP TABLE IF EXISTS order_state;

CREATE TABLE order_state(
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(20) NOT NULL
);

INSERT INTO order_state VALUES
(1,'Cancel'),
(2,'PaymentWait'),
(3,'ProviderAnswerWait'),
(4,'ProductDeliveryWait'),
(5,'Ready'),
(6,'Recivied');



CREATE TABLE orders(
  Id BIGSERIAL PRIMARY KEY,
  user_id BIGINT NOT NULL,
  Date DATE NOT NULL DEFAULT NOW(),
  state_id INT NOT NULL,
    FOREIGN KEY (state_id) REFERENCES order_state(Id) ON DELETE RESTRICT,
    FOREIGN KEY (user_id) REFERENCES users(Id) ON DELETE CASCADE
);


CREATE TABLE order_fill(
    order_id BIGINT NOT NULL,
    provider_id BIGINT NOT NULL,
    item_id BIGINT NOT NULL,
    Quantity INT NOT NULL,
    Paid_Price DECIMAL(20,2) NOT NULL,
    FOREIGN KEY (order_id) REFERENCES orders(Id) ON DELETE CASCADE,
    FOREIGN KEY (provider_id) REFERENCES providers(Id) ON DELETE CASCADE,
    FOREIGN KEY (item_id) REFERENCES items(Id) ON DELETE CASCADE,
    CHECK ( Paid_Price >= 0 AND Quantity > 0),
    PRIMARY KEY (order_id, provider_id, item_id)
);

DROP TABLE IF EXISTS basket_items;

CREATE TABLE basket_items(
  user_id BIGINT NOT NULL,
  provider_id BIGINT NOT NULL,
  item_id BIGINT NOT NULL,
  Quantity INT NOT NULL,
  FOREIGN KEY (user_id) REFERENCES users(Id) ON DELETE CASCADE,
  FOREIGN KEY (provider_id) REFERENCES providers(Id) ON DELETE CASCADE,
  FOREIGN KEY (item_id) REFERENCES items(Id) ON DELETE CASCADE,
  CHECK ( Quantity> 0 ),
  PRIMARY KEY(user_id, provider_id, item_id)
);

CREATE OR REPLACE FUNCTION trg_delete_basket_items()
RETURNS TRIGGER
AS
$trg$
DECLARE
    id_of_user BIGINT;
BEGIN

    SELECT o.user_id INTO id_of_user  FROM orders o WHERE o.id = NEW.order_id;
	DELETE FROM basket_items bi
            WHERE item_id = NEW.item_id AND provider_id = NEW.provider_id AND bi.user_id = id_of_user;
	RETURN NEW;
END
$trg$
LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_delete_basket_items_when_add_order_item ON order_fill ;
CREATE TRIGGER trg_delete_basket_items_when_add_order_item AFTER INSERT ON order_fill
    FOR EACH ROW
    EXECUTE PROCEDURE trg_delete_basket_items();

CREATE OR REPLACE FUNCTION trg_basket_items_recall()
RETURNS TRIGGER
AS
$trg$
BEGIN
    IF (NEW.state_id != 1)
        THEN RETURN NEW;
    END IF;

	INSERT INTO basket_items(user_id, provider_id, item_id, quantity)
	SELECT NEW.user_id, of.provider_id, of.item_id, of.quantity FROM order_fill of WHERE of.order_id = NEW.id;

	RETURN NEW;
END
$trg$
LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_add_basket_items_when_order_cancel ON orders ;
CREATE TRIGGER trg_add_basket_items_when_order_cancel BEFORE UPDATE ON orders
    FOR EACH ROW
    EXECUTE PROCEDURE trg_basket_items_recall();

COMMIT;