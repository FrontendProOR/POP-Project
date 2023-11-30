CREATE TABLE dbo.user_type
(
    user_type_id INT PRIMARY KEY,
    user_type_name VARCHAR(50) NOT NULL,
    user_type_is_active BIT NOT NULL
);