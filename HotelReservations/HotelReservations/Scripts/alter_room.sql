--ALTER TABLE dbo.[room]
--ADD is_deleted BIT DEFAULT 0;
UPDATE dbo.[user]
SET is_deleted = 0;
