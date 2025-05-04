ALTER PROCEDURE spUpdateUserProfile
    @ComID NVARCHAR(250) = NULL,
	@UserID INT = NULL,
	@AuthCode NVARCHAR(MAX) = NULL,
    @Device NVARCHAR(2) = NULL,
	@FullName NVARCHAR(MAX) = NULL,
	@Address NVARCHAR(MAX) = NULL,
	@Phone NVARCHAR = NULL,
	@Mobile NVARCHAR = NULL
AS
BEGIN
	-- Empty Validation
    IF (@ComID IS NULL OR @UserID IS NULL OR @AuthCode IS NULL OR @Device IS NULL)
    BEGIN
        SELECT 400 AS StatusCode, 'Empty Validation.' AS Message;
        RETURN;
    END

    -- Validate Company ID
    IF NOT EXISTS (SELECT 1 FROM comInfo WHERE @ComID=clientID)
    BEGIN
        SELECT 201 AS StatusCode, 'Company does not exist.' AS Message;
        RETURN;
    END

	-- Validate if User exists
    IF NOT EXISTS (SELECT 1 FROM usernamePwd WHERE @UserID=id)
    BEGIN
        SELECT 202 AS StatusCode, 'User does not Exist.' AS Message;
        RETURN;
    END

	-- Validate Active/Inactive
	IF ((SELECT activateInactivate FROM usernamePwd WHERE @UserID=id)!='Active')
	BEGIN
		SELECT 204 AS StatusCode, 'User Inactive.' AS Message;
        RETURN;
	END

	-- Validate License
	IF ((SELECT LicenseExp FROM comInfo WHERE @ComID=clientID) < GETDATE())
	BEGIN
		SELECT 205 AS StatusCode, 'License Expired.' AS Message;
        RETURN;
	END

	-- Update profile elements that are not null
	UPDATE usernamePwd
	SET
		fullName = ISNULL(@FullName, fullName),
		userAddress = ISNULL(@Address, userAddress),
		userPhone = ISNULL(@Phone, userPhone),
		userMobile = ISNULL(@Mobile, userMobile)
	WHERE @UserID = id;

    -- Success response
    SELECT 200 AS StatusCode, 'Success' AS Message;
    RETURN;
END;