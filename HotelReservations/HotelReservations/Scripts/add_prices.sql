--CREATE TABLE price (
--	price_id INT IDENTITY(1,1) PRIMARY KEY,
--	room_type_id INT NOT NULL,
--	reservation_type INT NOT NULL,
--	price_value FLOAT NOT NULL,
--	price_is_active BIT NOT NULL,
--	CONSTRAINT FK_PRICE_ROOM_TYPE
--	FOREIGN KEY (room_type_id) REFERENCES dbo.[room_type] (room_type_id)
--);
INSERT INTO dbo.[price] (room_type_id, reservation_type, price_value, price_is_active)
VALUES (1,1,999,1)