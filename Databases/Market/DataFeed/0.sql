INSERT INTO property_group(id, name)
    VALUES 
        (1,'Общие'),
        (2,'Основные характеристики'),
        (3,'Разъемы'),
        (4,'Особености');

INSERT INTO item_type(id,name)
    VALUES
        (1,'Процессор'),
        (2,'HDD и SSD'),
        (3,'Видеокарта'),
        (4,'Материнская плата'),
        (5,'Система охлаждения компьютера'),
        (6,'ОЗУ'),
        (7,'Блок питания'),
        (8,'Корпус');

insert into item_properties(id,name, group_id, is_filterable, property_data_type)
    VALUES
        (1,'Брэнд',1,true,'varchar'),
        (2,'Сокет',2,false,'varchar'),
        (3,'Число ядер',2,false,'int'),
        (4,'Объем памяти',2, true,'varchar'),
        (5,'Интерфейс',2,false,'varchar'),
        (6,'Форм фактор',2,true,'varchar'),
        (7,'Видеочипсет',2,false,'varchar'),
        --(8,'Разъемы',2,false,'varchar'),
        --(9,'Особености',2,false,'varchar'),

        -- TODO разъемы + слоты и т.п.

        (10,'Частота',2, true,'varchar'),
        (11,'Тип памяти',2,true,'varchar'),
        (12,'Потребляемая мощность',1,false,'varchar'),
        --(13,'Слоты',2,false,'varchar');
        (14,'Тепловыделение',2,false,'varchar');

INSERT INTO item_type_properties(type_id, property_id)
    VALUES
           (1,1), (1,2), (1,3),
           
           (2,1), (2,4), (2,5), (2,6),
           
           (3,1), (3,7), (3,4), (3,5), -- (3,8),
           
           (4,1), (4,2), (4,6), -- (4,13), -- (4,8),
           
           (5,1), (5,2),
           
           (6,1), (6,4), (6,10), (6,6),
           
           (7,1), (7,6), (7,12),
           
           (8,1), (8,6); -- (8,8);
