ALTER PROCEDURE spReferenceCountry
	@UserID INT = NULL,
	@AuthCode NVARCHAR(MAX) = NULL,
    @Device NVARCHAR(2) = NULL,
	@ID INT = NULL,
	@Name NVARCHAR(MAX) = NULL,
	@Address NVARCHAR(MAX) = NULL,
	@Phone NVARCHAR(MAX) = NULL,
	@Mobile NVARCHAR(MAX) = NULL,
	@Mail NVARCHAR(MAX) = NULL,
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
		SELECT ID, agentName, address, phone, mobile, mail, activateInactivate FROM empAgentCountry;

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

		IF NOT EXISTS (SELECT 1 FROM empAgentCountry WHERE @ID=id)
		BEGIN
			SELECT 206 AS StatusCode, 'No such agent in list.' AS MESSAGE;
			RETURN;
		END

		IF @Status NOT IN ('Active', 'Inactive')
		BEGIN
			SELECT 207 AS StatusCode, 'Invalid Status.' AS MESSAGE;
			RETURN;
		END

		UPDATE empAgentCountry
		SET
			agentName = ISNULL(@Name, agentName),
			address = ISNULL(@Address, address) ,
			phone = ISNULL(@Phone, phone),
			mobile = ISNULL(@Mobile, mobile),
			mail = ISNULL(@Mail, mail),
			activateInactivate = @Status
		WHERE id = @ID;

		-- Success response
		SELECT 200 AS StatusCode, 'Success' AS Message;
		RETURN;

	END	

	-- To Create
	IF @Flag = 'c'
	BEGIN

		IF (@Name IS NULL OR @Status IS NULL)
		BEGIN
			SELECT 400 AS StatusCode, 'Empty Validation.' AS Message;
			RETURN;
		END

		IF EXISTS (SELECT 1 FROM empAgentCountry WHERE @Mobile=mobile)
		BEGIN
			SELECT 208 AS StatusCode, 'Mobile number already exists.' AS MESSAGE;
			RETURN;
		END

		IF @Status NOT IN ('Active', 'Inactive')
		BEGIN
			SELECT 207 AS StatusCode, 'Invalid Status.' AS MESSAGE;
			RETURN;
		END

		--Insert new post
		INSERT INTO empAgentCountry(agentName, address, phone, mobile, mail, activateInactivate, userDateTime)
		VALUES (@Name, @Address, @Phone, @Mobile, @Mail, @Status, CAST(GETDATE() AS NVARCHAR) + '~~' + CAST(@UserID AS NVARCHAR))

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

		IF NOT EXISTS (SELECT 1 FROM empAgentCountry WHERE @ID=id)
		BEGIN
			SELECT 208 AS StatusCode, 'Agent does not exist in list.' AS MESSAGE;
			RETURN;
		END

		--Delete if exists
		DELETE FROM empAgentCountry WHERE @ID=id;

		-- Success response
		SELECT 200 AS StatusCode, 'Success' AS Message;
		RETURN;

	END

END;