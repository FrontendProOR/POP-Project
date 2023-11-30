--INSERT INTO dbo.user_type (user_type_id, user_type_name, user_type_is_active)
--VALUES (1, 'Administrator', 1);

--INSERT INTO dbo.user_type (user_type_id, user_type_name, user_type_is_active)
--VALUES (2, 'Receptionist', 1);

--INSERT INTO dbo.user_type (user_type_id, user_type_name, user_type_is_active)
--VALUES (3, 'Guest', 1);
-- Update Administrator
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
