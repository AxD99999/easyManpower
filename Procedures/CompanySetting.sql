CREATE PROCEDURE spCompanySet
	@UserID INT = NULL,
	@AuthCode NVARCHAR(MAX) = NULL,
    @Device NVARCHAR(2) = NULL,
	@ID INT = NULL,
	@Company NVARCHAR(MAX) = NULL,
	@Cell NVARCHAR(MAX) = NULL,
	@Mail NVARCHAR(MAX) = NULL,
	@City NVARCHAR(100) = NULL,
	@Contact NVARCHAR(MAX) = NULL,
	@Owner NVARCHAR(MAX) = NULL,
	@Address NVARCHAR(MAX) = NULL,
	@CR NVARCHAR(MAX) = NULL,
	@Location NVARCHAR(255) = NULL,
	@Status NVARCHAR(255) = NULL,
	@Flag NVARCHAR(2) = NULL
AS
BEGIN
	-- Empty Validation
    IF (@UserID IS NULL OR @AuthCode IS NULL OR @Device IS NULL OR @Flag IS NULL)
    BEGIN
        SELECT 400 AS StatusCode, 'Empty Validation.' AS Message;
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

	-- Validate Flag
	IF @Flag NOT IN ('l', 'u', 'c', 'd')
	BEGIN
		SELECT 205 AS StatusCode, 'Invalid Flag.' AS Message;
        RETURN;
	END

	--To List
	IF @Flag = 'l'
	BEGIN

		--List all
		SELECT 
			id as ID,
			companyName as Company,
			mobile as Cell,
		    email as Email,
			cityName as City,
			contactPerson as Contact,
			phone as OwnerName,
			address as Address, 
			url as CR,
			locationName as Location,
			activateInactivate as Activate
		FROM empCompanyName;

		-- Success response
		SELECT 200 AS StatusCode, 'Success' AS Message;
		RETURN;

	END

	-- To Update
	IF @Flag = 'u'
	BEGIN

		IF (@ID IS NULL)
		BEGIN
			SELECT 400 AS StatusCode, 'Empty Validation.' AS Message;
			RETURN;
		END

		IF NOT EXISTS (SELECT 1 FROM empCompanyName WHERE @ID=id)
		BEGIN
			SELECT 206 AS StatusCode, 'No such company in list.' AS MESSAGE;
			RETURN;
		END

		IF @Status NOT IN ('Active', 'Inactive')
		BEGIN
			SELECT 207 AS StatusCode, 'Invalid Status.' AS MESSAGE;
			RETURN;
		END

		UPDATE empCompanyName
		SET
			companyName = ISNULL(@Company, companyName),
			mobile = ISNULL(@Cell, mobile),
			email = ISNULL(@Mail, email),
			cityName = ISNULL(@City, cityName),
			contactPerson = ISNULL(@Contact, contactPerson),
			phone = ISNULL(@Owner, phone),
			address = ISNULL(@Address, address) ,
			url = ISNULL(@CR, url),
			locationName = ISNULL(@Location, locationName),
			activateInactivate = ISNULL(@Status, activateInactivate)
		WHERE id = @ID;

		-- Success response
		SELECT 200 AS StatusCode, 'Success' AS Message;
		RETURN;

	END	

	-- To Create
	IF @Flag = 'c'
	BEGIN

		IF (@Company IS NULL OR @Status IS NULL)
		BEGIN
			SELECT 400 AS StatusCode, 'Empty Validation.' AS Message;
			RETURN;
		END

		IF EXISTS (SELECT 1 FROM empCompanyName WHERE @ID=id)
		BEGIN
			SELECT 208 AS StatusCode, 'Altready exists.' AS MESSAGE;
			RETURN;
		END

		IF @Status NOT IN ('Active', 'Inactive')
		BEGIN
			SELECT 207 AS StatusCode, 'Invalid Status.' AS MESSAGE;
			RETURN;
		END

		--Insert new post
		INSERT INTO empCompanyName(companyName, mobile, email, cityName, contactPerson, phone, address, url, locationName, activateInactivate, userDateTime)
		VALUES (@Company, @Cell, @Mail, @City, @Contact, @Owner, @Address, @CR, @Location, @Status, CAST(GETDATE() AS NVARCHAR) + '~~' + CAST(@UserID AS NVARCHAR))

		-- Success response
		SELECT 200 AS StatusCode, 'Success' AS Message;
		RETURN;
	END	

	--To Delete
	IF @Flag = 'd'
	BEGIN

		IF (@ID IS NULL)
		BEGIN
			SELECT 400 AS StatusCode, 'Empty Validation.' AS Message;
			RETURN;
		END

		IF NOT EXISTS (SELECT 1 FROM empCompanyName WHERE @ID=id)
		BEGIN
			SELECT 208 AS StatusCode, 'Does not exist in list.' AS MESSAGE;
			RETURN;
		END

		--Delete if exists
		DELETE FROM empCompanyName WHERE @ID=id;

		-- Success response
		SELECT 200 AS StatusCode, 'Success' AS Message;
		RETURN;

	END

END;