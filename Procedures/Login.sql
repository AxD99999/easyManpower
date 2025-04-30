ALTER PROCEDURE spLogin
    @ComID VARCHAR(250) = NULL,
	@UserName VARCHAR(250) = NULL,
	@Password VARCHAR(MAX) = NULL,
    @Device VARCHAR(2) = NULL
AS
BEGIN
	-- Empty Validation
    IF (@ComID IS NULL OR @UserName IS NULL OR @Password IS NULL OR @Device IS NULL)
    BEGIN
        SELECT 400 AS StatusCode, 'Empty Validation.' AS Message;
        RETURN;
    END

    -- Validate Company ID
    IF NOT EXISTS (SELECT 1 FROM comInfo WHERE @ComID=id)
    BEGIN
        SELECT 201 AS StatusCode, 'Company does not exist.' AS Message;
        RETURN;
    END

	-- Validate if User exists
    IF NOT EXISTS (SELECT 1 FROM usernamePwd WHERE @UserName=userName)
    BEGIN
        SELECT 202 AS StatusCode, 'Username does not Exist.' AS Message;
        RETURN;
    END

	-- Validate Password
	IF ((SELECT pwdcompare(@Password,userPWD) as PWD from usernamePwd where userName=@userName)!='1')
	BEGIN
		SELECT 203 AS StatusCode, 'Password does not match.' AS Message;
        RETURN;
	END

	-- Validate Active/Inactive
	IF ((SELECT activateInactivate FROM usernamePwd WHERE @UserName=userName)!='Active')
	BEGIN
		SELECT 204 AS StatusCode, 'User Inactive.' AS Message;
        RETURN;
	END

	-- Validate License
	IF ((SELECT LicenseExp FROM comInfo WHERE @ComID=id) < GETDATE())
	BEGIN
		SELECT 205 AS StatusCode, 'License Expired.' AS Message;
        RETURN;
	END

    -- Update Last Login
    UPDATE usernamePwd
	SET lastLogin = CONVERT(NVARCHAR(100), GETDATE(), 23)
	WHERE userName = @UserName;

	-- Delete Old AuthCode for User
	IF EXISTS (SELECT 1 FROM AuthCodes WHERE UserName = @UserName)
	BEGIN
		DELETE FROM AuthCodes
		WHERE UserName = @UserName;
	END

	-- Create New AuthCode for User
	DECLARE @AuthToken VARCHAR(64) = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', CAST(NEWID() AS NVARCHAR(36))), 2);
	INSERT INTO AuthCodes (UserName, Auth)
	VALUES (@UserName, @AuthToken);

	-- Return required values
	SELECT
		@AuthToken as AuthCode,
		a.id as ComID,
		a.comName as ComName,
		a.comAddress as ComAddress,
		a.comTelephone as ComTel,
		a.comLogo as ComLogo,
		u.id as UserID,
		u.fullName as FullName,
		u.userAddress as Address,
		u.userPhone as Phone,
		u.userMobile as Mobile,
		u.branchAccess as BranchAddress,
		u.branchID as BranchID
	FROM usernamePwd u
	JOIN comInfo a ON u.myID = a.myID
	WHERE u.userName = @UserName;

    -- Success response
    SELECT 200 AS StatusCode, 'Success' AS Message;
    RETURN;
END;