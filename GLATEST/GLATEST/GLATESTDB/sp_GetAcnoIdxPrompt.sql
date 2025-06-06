/****** Object:  StoredProcedure [dbo].[sp_GetAcnoIdxPrompt]    Script Date: 08/09/2013 14:08:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==================================================================
-- Author:			Wangme
-- Create date:	2009/09/09
-- Description:	自定預存程序，傳入公司別、科目明細定義碼
-- 傳回科目明細碼之資料提示SQL指令
-- ==================================================================
ALTER PROCEDURE [dbo].[sp_GetAcnoIdxPrompt] (@Company char(2), @ColID char(2), @CodeCode varchar(10) )
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare @ChkProgram varchar(20)
	Declare @ParmIn varchar(20)
	Declare @KeyFieldName1 varchar(20)
	Declare @KeyFieldName2 varchar(20)
	Declare @DataFieldName1 varchar(20)
	Declare @DataFieldName2 varchar(20)
	Declare @MyResult varchar(50)
	Declare @sq char(1)	  -- 單引號
	Declare @SQLString nvarchar(500)


	-- 先找明細欄位定義檔相關設定
	Select @ChkProgram = ChkProgram,  @ParmIn = ParmIn, @KeyFieldName1 = KeyFieldName1, @KeyFieldName2 = KeyFieldName2 
	, @DataFieldName1 = DataFieldName1, @DataFieldName2 = DataFieldName2 from GLColDef with (nolock)
	 Where Company = @Company  and ColID =  @ColID
	Set @SQLString = ''
	-- 若找不到則返回原代碼
	If @@ROWCOUNT = 0 RETURN @SQLString
	-- 若沒有設定檢查的Table則返回原代碼
	If LTrim(@ChkProgram) = ''  RETURN @SQLString
	
	-- 組合查詢條件SQL指令
	Set @sq = char(39)
	If LTrim(@ParmIn) <> '' 
	Begin
		if ltrim(@CodeCode) <> ''
		begin
			SET @SQLString = 'Select [Code] = ' + @KeyFieldName2  + ',  [Name] = ' + @DataFieldName1 + ' From ' +  @ChkProgram
			 + ' Where ' + @KeyFieldName1 +  ' = ' + @sq + @ParmIn + @sq + ' And (' + @KeyFieldName2 + ' like ' + @sq +'%'+ @CodeCode + '%' + @sq 
			 + 'or ' +@DataFieldName1 +' like ' + @sq +'%'+ @CodeCode + '%' + @sq +')'
			 + ' Order by [Code]'
		end
		else
		begin
			SET @SQLString = 'Select [Code] = ' + @KeyFieldName2  + ',  [Name] = ' + @DataFieldName1 + ' From ' +  @ChkProgram
			 + ' Where ' + @KeyFieldName1 +  ' = ' + @sq + @ParmIn + @sq + ' Order by [Code]'
		end
	End
	ELSE
		if ltrim(@CodeCode) <> ''
		begin
			SET @SQLString = 'Select [Code] = ' + @KeyFieldName1  + ',  [Name] = ' + @DataFieldName1 
			 + ' From ' +  @ChkProgram + ' Where (' + @KeyFieldName1 + ' like ' + @sq +'%'+ @CodeCode + '%' + @sq
			  + 'or ' +@DataFieldName1 +' like ' + @sq +'%'+ @CodeCode + '%' + @sq +')'
			 + ' Order by [Code]'
		end
		else
		begin
			SET @SQLString = 'Select [Code] = ' + @KeyFieldName1  + ',  [Name] = ' + @DataFieldName1 
			 + ' From ' +  @ChkProgram + ' Order by [Code]'
		end
	
	-- 返回字串結果值
	Select @SQLString as 'SQLString'

END

