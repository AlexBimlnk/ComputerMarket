\c market
insert into providers (id, name, margin, bank_account, inn, is_approved)
values  (1, 'Horns and hooves', 1.5000, '12345123451234512344', '1234512344', true),
        (2, 'Ivanov', 1.8500, '12345123451234512345', '1234512345', true);

insert into public.property_group (id, name)
values  (1, 'Общие'),
        (2, 'Основные характеристики'),
        (3, 'Разъемы'),
        (4, 'Особености');

insert into item_properties (id, name, group_id, is_filterable, data_type_id)
values  (1, 'Брэнд', 1, true, 1),
        (2, 'Сокет', 2, true, 1),
        (3, 'Колличество ядер', 2, true, 2),
        (4, 'Объем памяти', 2, true, 1),
        (5, 'Интерфейс', 2, true, 1),
        (6, 'Форм фактор', 2, true, 1),
        (7, 'Видеочипсет', 2, true, 1),
        (10, 'Частота', 2, true, 1),
        (11, 'Тип памяти', 2, true, 1),
        (12, 'Потребляемая мощность', 1, false, 1),
        (14, 'Тепловыделение', 2, false, 1);

insert into public.item_type (id, name, url_image)
values  (1, 'Процессор', 'https://c.dns-shop.ru/thumb/st1/fit/220/150/9f2382f80196fe69081442ae4995c60d/b70d20da48dfebda15c88f04044237bfa6234fe05d539ff68a61395785620202.jpg'),
        (2, 'HDD и SSD', 'https://c.dns-shop.ru/thumb/st1/fit/220/150/fffda74669f660a0008b3713aa57b90f/c590f476bfc0e3437595ab8ae2770905544ff1439f99e2ec599b5e89be647695.jpg'),
        (3, 'Видеокарта', 'https://c.dns-shop.ru/thumb/st1/fit/220/150/be5b6c66e33224d6d712ad4c3b0f5f44/809d70b81f66d3f730c567c442e3ad98baeef227e15b2413dd3b0ca2c4afeca0.jpg'),
        (4, 'Материнская плата', 'https://c.dns-shop.ru/thumb/st4/fit/220/150/4aa1adddbd68bd0ac0739fff47327355/114d29cfe20f17933bac65e17c2edf53aeb4d5db5a0d683b5191195099eb437c.jpg');

insert into public.item_type_properties (type_id, property_id)
values  (1, 1),
        (1, 2),
        (1, 3),
        (2, 1),
        (2, 4),
        (2, 5),
        (2, 6),
        (3, 1),
        (3, 7),
        (3, 4),
        (3, 5),
        (4, 1),
        (4, 2),
        (4, 6);

/*SELECT t.name, ip.name, ip.id FROM item_type_properties i
LEFT JOIN item_properties ip on ip.id = i.property_id
LEFT JOIN item_type t on t.id = i.type_id;*/

INSERT INTO items(id, name, type_id, url_image) VALUES
(1, 'Intel Core i5-12400F OEM', 1, 'https://c.dns-shop.ru/thumb/st4/fit/wm/0/0/7ad86053c27f424add781cf3fdbf87a0/200e4a08e74afcc3cf1d54d47b758cbbf14c71973a64009553347d2b234b5af4.jpg.webp');

INSERT INTO item_description(item_id, property_id, property_value) VALUES
(1, 1, 'Intel'),
(1, 2, 'LGA 1700'),
(1, 3, '6');

INSERT INTO items(id, name, type_id, url_image) VALUES
(2, 'Intel Core i5-12400 OEM', 1, 'https://c.dns-shop.ru/thumb/st1/fit/wm/0/0/76a5b711c02c3221509186caf51accf3/eba7abd6873f6f2ecf1ec1d1f64526253a392ee0dcf2afdf52e91eaa3d6cbaca.jpg.webp');

INSERT INTO item_description(item_id, property_id, property_value) VALUES
(2, 1, 'Intel'),
(2, 2, 'LGA 1700'),
(2, 3, '6');

INSERT INTO items(id, name, type_id, url_image) VALUES
(3, 'AMD Ryzen 5 5600X BOX', 1, 'https://c.dns-shop.ru/thumb/st4/fit/wm/0/0/ba9bb953663164aff24173927ba76524/1df080ce2ba9939556bea15d1ba11cd384a90caa033715539a68a52569d373b6.jpg.webp');

INSERT INTO item_description(item_id, property_id, property_value) VALUES
(3, 1, 'AMD'),
(3, 2, 'AM4'),
(3, 3, '6');

INSERT INTO items(id, name, type_id, url_image) VALUES
(4, 'Intel Core i7-13700K BOX', 1, 'https://c.dns-shop.ru/thumb/st4/fit/wm/0/0/a82e00f887a47ffb55b2b008ddfc42d5/2d66506e9cba91fed8c76c0090f51b0f3b4111b1a43a91648fbc5dbb5fe47041.jpg.webp');

INSERT INTO item_description(item_id, property_id, property_value) VALUES
(4, 1, 'Intel'),
(4, 2, 'LGA 1700'),
(4, 3, '16');

INSERT INTO items(id, name, type_id, url_image) VALUES
(5, 'AMD Ryzen 9 5950X BOX', 1, 'https://c.dns-shop.ru/thumb/st4/fit/wm/0/0/9bb86f727bca1e0120cb7cb30e2fae08/4331628f3b2e2c36ee859638c701ab2d861436c732dc90c2d3e822e38a2f00aa.jpg.webp');

INSERT INTO item_description(item_id, property_id, property_value) VALUES
(5, 1, 'AMD'),
(5, 2, 'AM4'),
(5, 3, '16');

INSERT INTO items(id, name, type_id, url_image) VALUES
(6, 'GIGABYTE B550 AORUS ELITE V2', 4, 'https://c.dns-shop.ru/thumb/st4/fit/0/0/b9a79af8030c5b9365c2721e150f2e4d/cbeada593290a99b56177e3c496c9ae68eabaa00ef47178f7e303a3ec398b647.png.webp');

INSERT INTO item_description(item_id, property_id, property_value) VALUES
(6, 1, 'GIGABYTE'),
(6, 2, 'AM4'),
(6, 6, 'Standard-ATX');

INSERT INTO items(id, name, type_id, url_image) VALUES
(7, 'MSI MAG B560 TOMAHAWK WIFI', 4, 'https://c.dns-shop.ru/thumb/st4/fit/0/0/6cfad818ac1bad33fcfa725d83f7ffc8/d2029373cd6dc76982d7f94502d88db650af68d0a184c9d8f136cc11e45a3a4c.png.webp');

INSERT INTO item_description(item_id, property_id, property_value) VALUES
(7, 1, 'MSI'),
(7, 2, 'LGA 1200'),
(7, 6, 'Standard-ATX');

INSERT INTO items(id, name, type_id, url_image) VALUES
(8, 'AMD Radeon R5 Series [R5SL128G]', 2, 'https://c.dns-shop.ru/thumb/st1/fit/wm/0/0/baf8ec7a50daf99b15ca9eea2cff9b97/215ceab6a194e68dc88913de14371564bd698bda2742c48d0cb564b7e72aacc5.jpg.webp');

INSERT INTO item_description(item_id, property_id, property_value) VALUES
(8, 1, 'AMD'),
(8, 4, '128 ГБ'),
(8, 5, 'SATA'),
(8, 6, '2.5"');

INSERT INTO items(id, name, type_id, url_image) VALUES
(9, 'Seagate BarraCuda [ST2000DM008]', 2, 'https://c.dns-shop.ru/thumb/st1/fit/0/0/8fc8e919410eaec185c1f3eb6240f66a/388d249f2647a68a2a5281c68ff6df754843fcf3c433ba22ef257b228b715574.jpg.webp');

INSERT INTO item_description(item_id, property_id, property_value) VALUES
(9, 1, 'Seagate'),
(9, 4, '2 ТБ'),
(9, 5, 'SATA'),
(9, 6, '3.5"');

INSERT INTO items(id, name, type_id, url_image) VALUES
(10, 'GIGABYTE GF RTX 3090 TI GAMING', 3, 'https://c.dns-shop.ru/thumb/st4/fit/wm/0/0/8c2526fa59c532283b04382250e9e717/427b0bd2ca9a1c3609217aab6353ff9f6868da6bd505c7f72c3bad8286b0951d.jpg.webp');

INSERT INTO item_description(item_id, property_id, property_value) VALUES
(10, 1, 'GIGABYTE'),
(10, 7, 'GeForce RTX 3090 Ti'),
(10, 4, '24 ГБ'),
(10, 5, 'PCI-E 4.0');

/*SELECT it.name, i.name, i.id FROM items i
    LEFT JOIN item_type it on it.id = i.type_id;*/

INSERT INTO products(item_id, provider_cost, quantity, provider_id)
 VALUES
        (1, 13000.00, 100, 1),
        (2, 12000.00, 100, 1),
        (3, 12000.00, 100, 1),
        (4, 44000.00, 100, 1),
        (5, 40000.00, 100, 1),
        (6, 11000.00, 100, 1),
        (7, 10000.00, 100, 1),
        (8, 700.00, 100, 1),
        (9, 4200.50, 100, 1),
        (10, 105000.00, 100, 1);

INSERT INTO products(item_id, provider_cost, quantity, provider_id)
 VALUES
        (1, 12499.99, 100, 2),
        (2, 11499.99, 100, 2),
        (3, 11499.99, 100, 2),
        (4, 42499.99, 100, 2),
        (5, 39599.99, 100, 2),
        (6, 10099.99, 100, 2);



