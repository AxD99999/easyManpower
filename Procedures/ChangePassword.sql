CREATE PROCEDURE spChangePassword
    @ComID VARCHAR(250) = NULL,
	@UserID INT = NULL,
	@Auth NVARCHAR(MAX) = NULL,
	@OldPwd VARCHAR(MAX) = NULL,
    @NewPwd Varchar(MAX) = NULL
AS
BEGIN
	-- Empty Validation
    IF (@ComID IS NULL OR @UserID IS NULL OR @OldPwd IS NULL OR @NewPwd IS NULL)
    BEGIN
        SELECT 400 AS StatusCode, 'Empty Validation.' AS Message;
        RETURN;
    END

	-- Get UserName
	DECLARE @UserName NVARCHAR(255);
	SELECT @UserName = userName FROM usernamePwd WHERE id = @UserID;

	-- Validate AuthCode
	IF NOT EXISTS (SELECT 1 FROM AuthCodes WHERE @Auth = Auth AND @UserName = UserName)
	BEGIN
		SELECT 401 AS StatusCode, 'Auth Code Expired.' AS Message;
        RETURN;
	END

    -- Validate Company ID
    IF NOT EXISTS (SELECT 1 FROM comInfo WHERE @ComID=id)
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

	-- Validate License
	IF ((SELECT LicenseExp FROM comInfo WHERE @ComID=id) < GETDATE())
	BEGIN
		SELECT 205 AS StatusCode, 'License Expired.' AS Message;
        RETURN;
	END

	-- Validate Old Password
	IF ((SELECT pwdcompare(@OldPwd,userPWD) as PWD from usernamePwd where id=@UserID)!='1')
	BEGIN
		SELECT 203 AS StatusCode, 'Old Password does not match.' AS Message;
        RETURN;
	END

	-- Validate New Password
	IF (@OldPwd = @NewPwd)
	BEGIN
		SELECT 203 AS StatusCode, 'New Password cannot be same as Old password.' AS Message;
        RETURN;
	END

	-- Encrypt and update new password
	UPDATE usernamePwd
	SET userPWD = PWDENCRYPT(@NewPwd)
	WHERE userName = @UserName;

    -- Success response
    SELECT 200 AS StatusCode, 'Success' AS Message;
    RETURN;
END;