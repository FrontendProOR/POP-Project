
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