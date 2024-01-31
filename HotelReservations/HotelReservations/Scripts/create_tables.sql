

CREATE TABLE room_type (
	room_type_id INT IDENTITY(1,1) PRIMARY KEY,
	room_type_name VARCHAR(255) NOT NULL UNIQUE,
	room_type_is_active BIT NOT NULL
);

CREATE TABLE dbo.user_type
(
    user_type_id INT IDENTITY(1,1) PRIMARY KEY,
    user_type_name VARCHAR(50) NOT NULL,
    user_type_is_active BIT NOT NULL
);

INSERT INTO dbo.[user_type] ( user_type_name, user_type_is_active)
VALUES ( 'Administrator', 1);

INSERT INTO dbo.[user_type] ( user_type_name, user_type_is_active)
VALUES ('Receptionist', 1);

INSERT INTO dbo.[user_type] ( user_type_name, user_type_is_active)
VALUES ('Guest', 1);

UPDATE dbo.user_type
SET user_type_name = 'administrator'
WHERE user_type_id = 1;

-- Update Receptionist
UPDATE dbo.user_type
SET user_type_name = 'receptionist'
WHERE user_type_id = 2;

-- Update Guest
UPDATE dbo.user_type
SET user_type_name = 'guest'
WHERE user_type_id = 3;

CREATE TABLE room (
	room_id INT IDENTITY(1,1) PRIMARY KEY,
	room_number VARCHAR(25) NOT NULL UNIQUE,
	has_TV BIT NOT NULL,
	has_mini_bar BIT NOT NULL,
	room_is_active BIT NOT NULL,
	room_type_id INT NOT NULL,
	CONSTRAINT FK_ROOM_ROOM_TYPE
	FOREIGN KEY (room_type_id) REFERENCES dbo.room_type (room_type_id)
);

CREATE TABLE price (
	price_id INT IDENTITY(1,1) PRIMARY KEY,
	room_type_id INT NOT NULL,
	reservation_type INT NOT NULL,
	price_value FLOAT NOT NULL,
	price_is_active BIT NOT NULL,
	CONSTRAINT FK_PRICE_ROOM_TYPE
	FOREIGN KEY (room_type_id) REFERENCES dbo.[room_type] (room_type_id)
);

CREATE TABLE "user" (
	"user_id" INT IDENTITY(1,1) PRIMARY KEY,
	"first_name" VARCHAR(40) NOT NULL,
	"last_name" VARCHAR(50) NOT NULL,
	"JMBG" VARCHAR(13) NOT NULL,
	"username" VARCHAR(20) NOT NULL UNIQUE,
	"password" VARCHAR(50) NOT NULL,
	"user_type" VARCHAR(15) NOT NULL
);

CREATE TABLE reservation (
	reservation_id INT IDENTITY(1,1) PRIMARY KEY,
	reservation_type INT NOT NULL,
	start_date_time DATETIME,
	end_date_time DATETIME,
	total_price FLOAT NOT NULL,
	reservation_is_active BIT NOT NULL,
);
--room_number VARCHAR(50) u reservation ne secam se koji sam dodo 

CREATE TABLE guest (
	guest_id INT IDENTITY(1,1) PRIMARY KEY,
	guest_name VARCHAR(40) NOT NULL,
	guest_surname VARCHAR(50) NOT NULL,
	guest_id_number VARCHAR(25) NOT NULL,
	guest_is_active BIT NOT NULL
);
CREATE TABLE reservation_guest (
    reservation_id INT,
    guest_id INT,
    FOREIGN KEY (reservation_id) REFERENCES reservation(reservation_id),
    FOREIGN KEY (guest_id) REFERENCES guest(guest_id)
);

SELECT * FROM dbo."user" u
WHERE u."user_type" = 'administrator'

--CREATE TABLE "administrator" (
--	"user_id" INT PRIMARY KEY,
--	CONSTRAINT FK_ADMIN_USER
--	FOREIGN KEY ("user_id") REFERENCES dbo."user" ("user_id")
--);

--SELECT * FROM dbo.administrator a
--LEFT JOIN dbo."user" u ON u."user_id" = a."user_id" 
