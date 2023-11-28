-- Enable IDENTITY_INSERT for the table
SET IDENTITY_INSERT dbo.room_type ON;

-- Perform the insert with explicit values for identity column and other columns
INSERT INTO dbo.room_type (room_type_id, room_type_name, room_type_is_active)
VALUES (1, N'Single Bed', 1);

INSERT INTO dbo.room_type (room_type_id, room_type_name, room_type_is_active)
VALUES (2, N'Double Bed', 1);

-- Disable IDENTITY_INSERT after the insert
SET IDENTITY_INSERT dbo.room_type OFF;
