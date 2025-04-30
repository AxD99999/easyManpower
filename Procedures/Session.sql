ALTER PROCEDURE spSession
    @ComID VARCHAR(250) = NULL,
	@UserID INT = NULL,
	@AuthCode VARCHAR(MAX) = NULL,
    @Device VARCHAR(2) = NULL
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

	-- Return required values
	SELECT
		a.clientID as ComID,
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
	JOIN comInfo a ON u.comID = a.clientID
	WHERE u.id = @UserID;

    -- Success response
    SELECT 200 AS StatusCode, 'Success' AS Message;
    RETURN;
END;

