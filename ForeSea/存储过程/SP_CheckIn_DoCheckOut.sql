﻿CREATE PROCEDURE [dbo].[SP_CheckIn_DoCheckOut]
	@id INT,
	@state NVARCHAR(50)='' OUTPUT,
	@result SMALLINT=700 OUTPUT
AS
	DECLARE @datetime DATETIME2(0)=GETDATE()
	DECLARE @time TIME=@datetime
	DECLARE @date DATE=@datetime
	IF (SELECT ID FROM CheckIn_Details WHERE ID=@id AND Date=@date AND State=0) IS NULL
	BEGIN
		SET @result=703
		RETURN 1
	END
	IF (SELECT Lesson FROM CheckIn_Time WHERE @datetime BETWEEN StartOut AND EndOut) IS NULL
	BEGIN
		SET @result=702
		SET @state=(SELECT ID FROM CheckIn_Details WHERE ID=@id AND Date=@date AND State =0)
		RETURN 1
	END
	UPDATE CheckIn_Details SET State=CASE LateState WHEN 1 THEN 2 ELSE 3 END,CheckOut=@time,CheckOutState=1
		WHERE ID=@id AND State=0
	SET @result=701
RETURN 0
