ALTER PROCEDURE spCountrySetting
	@UserID INT = NULL,
	@AuthCode NVARCHAR(MAX) = NULL,
    @Device NVARCHAR(2) = NULL,
	@ID INT = NULL,
	@Country NVARCHAR(MAX) = NULL,
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

		--List all Countries and Status
		SELECT id, countryName, activateInactivate FROM empCountryName;

		-- Success response
		SELECT 200 AS StatusCode, 'Success' AS Message;
		RETURN;

	END

	-- To Update
	IF @Flag = 'u'
	BEGIN

		IF (@ID IS NULL OR @Status IS NULL)
		BEGIN
			SELECT 400 AS StatusCode, 'Empty Validation.' AS Message;
			RETURN;
		END

		IF NOT EXISTS (SELECT 1 FROM empCountryName WHERE @ID=id)
		BEGIN
			SELECT 206 AS StatusCode, 'No such country in list.' AS MESSAGE;
			RETURN;
		END

		IF @Status NOT IN ('Active', 'Inactive')
		BEGIN
			SELECT 207 AS StatusCode, 'Invalid Status.' AS MESSAGE;
			RETURN;
		END

		IF (EXISTS (SELECT 1 FROM empCountryName WHERE @Country=countryName))
		BEGIN
			SELECT 206 AS StatusCode, 'Country already in list.' AS MESSAGE;
			RETURN;
		END

		UPDATE empCountryName
		SET 
			countryName = ISNULL(@Country, countryName),
			activateInactivate = ISNULL(@Status, activateinactivate)
		WHERE countryName = @Country;

		-- Success response
		SELECT 200 AS StatusCode, 'Success' AS Message;
		RETURN;

	END	

	-- To Create
	IF @Flag = 'c'
	BEGIN

		IF (@Country IS NULL OR @Status IS NULL)
		BEGIN
			SELECT 400 AS StatusCode, 'Empty Validation.' AS Message;
			RETURN;
		END

		IF EXISTS (SELECT 1 FROM empCountryName WHERE @Country=countryName)
		BEGIN
			SELECT 208 AS StatusCode, 'Country already exists in list.' AS MESSAGE;
			RETURN;
		END

		IF @Status NOT IN ('Active', 'Inactive')
		BEGIN
			SELECT 207 AS StatusCode, 'Invalid Status.' AS MESSAGE;
			RETURN;
		END

		--Insert new country
		INSERT INTO empCountryName(countryName, activateInactivate, userDateTime)
		VALUES (@Country, @Status, CAST(GETDATE() AS NVARCHAR) + '~~' + CAST(@UserID AS NVARCHAR))

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

		IF NOT EXISTS (SELECT 1 FROM empCountryName WHERE @ID=id)
		BEGIN
			SELECT 208 AS StatusCode, 'Country does not exist in list.' AS MESSAGE;
			RETURN;
		END

		--Delete if exists
		DELETE FROM empCountryName WHERE @ID=id;

		-- Success response
		SELECT 200 AS StatusCode, 'Success' AS Message;
		RETURN;

	END

END;