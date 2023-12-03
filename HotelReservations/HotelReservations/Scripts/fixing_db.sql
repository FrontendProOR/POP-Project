--DROP TABLE dbo.[user];

CREATE TABLE "user" (
	"user_id" INT IDENTITY(1,1) PRIMARY KEY,
	"first_name" VARCHAR(40) NOT NULL,
	"last_name" VARCHAR(50) NOT NULL,
	"JMBG" VARCHAR(13) NOT NULL,
	"username" VARCHAR(20) NOT NULL UNIQUE,
	"password" VARCHAR(50) NOT NULL,
	"user_type" VARCHAR(15) NOT NULL
);