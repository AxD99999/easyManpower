ALTER PROCEDURE spPostSetting
	@UserID INT = NULL,
	@AuthCode NVARCHAR(MAX) = NULL,
    @Device NVARCHAR(2) = NULL,
	@ID INT = NULL,
	@Post NVARCHAR(MAX) = NULL,
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
		SELECT id, postName, activateInactivate FROM empPost;

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

		IF NOT EXISTS (SELECT 1 FROM empPost WHERE @ID=id)
		BEGIN
			SELECT 206 AS StatusCode, 'No such post in list.' AS MESSAGE;
			RETURN;
		END

		IF @Status NOT IN ('Active', 'Inactive')
		BEGIN
			SELECT 207 AS StatusCode, 'Invalid Status.' AS MESSAGE;
			RETURN;
		END

		UPDATE empPost
		SET 
			postName = ISNULL(@Post, postName),
			activateInactivate = ISNULL(@Status, activateInactivate)
		WHERE id = @ID;

		-- Success response
		SELECT 200 AS StatusCode, 'Success' AS Message;
		RETURN;

	END	

	-- To Create
	IF @Flag = 'c'
	BEGIN

		IF (@Post IS NULL OR @Status IS NULL)
		BEGIN
			SELECT 400 AS StatusCode, 'Empty Validation.' AS Message;
			RETURN;
		END

		IF EXISTS (SELECT 1 FROM empPost WHERE @Post=postName)
		BEGIN
			SELECT 208 AS StatusCode, 'Post already exists in list.' AS MESSAGE;
			RETURN;
		END

		IF @Status NOT IN ('Active', 'Inactive')
		BEGIN
			SELECT 207 AS StatusCode, 'Invalid Status.' AS MESSAGE;
			RETURN;
		END

		--Insert new post
		INSERT INTO empPost(postName, activateInactivate, userDateTime)
		VALUES (@Post, @Status, CAST(GETDATE() AS NVARCHAR) + '~~' + CAST(@UserID AS NVARCHAR))

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

		IF NOT EXISTS (SELECT 1 FROM empPost WHERE @ID=id)
		BEGIN
			SELECT 208 AS StatusCode, 'Post does not exist in list.' AS MESSAGE;
			RETURN;
		END

		--Delete if exists
		DELETE FROM empPost WHERE @ID=id;

		-- Success response
		SELECT 200 AS StatusCode, 'Success' AS Message;
		RETURN;

	END

END;

