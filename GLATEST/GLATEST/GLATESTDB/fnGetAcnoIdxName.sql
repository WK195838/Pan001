/****** Object:  UserDefinedFunction [dbo].[fnGetAcnoIdxName]    Script Date: 08/12/2013 11:13:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- ==================================================================
-- Author:			Wangme
-- Create date:	2009/09/09
-- Modify Date:2010/9/14 Hans Lin
-- Description:	自定Function，傳入公司別、科目明細定義碼、科目明細碼
-- 傳回科目明細碼之資料說明欄位值1
-- 因系統整合修正錯誤原本由CodeMaster改由CodeDesc取資料
--欄位名稱修改DeptName->DepName DeptCode->DepCode
-- ==================================================================
ALTER FUNCTION [dbo].[fnGetAcnoIdxName] (@Company char(2), @ColID char(2), @CodeCode char(20) )
RETURNS varchar(50) AS
BEGIN
	Declare @ChkProgram varchar(20)
	Declare @ParmIn varchar(20)
	Declare @KeyFieldName1 varchar(20)
	Declare @KeyFieldName2 varchar(20)
	Declare @DataFieldName1 varchar(20)
	Declare @DataFieldName2 varchar(20)
	Declare @MyResult varchar(50)
	Declare @sq char(1)	  -- 單引號
	Declare @SQLString nvarchar(500)
--	DECLARE @ParmDefinition nvarchar(500)
	Declare @CodeName char(50)

	set @CodeCode = LTrim(RTrim(@CodeCode))
	-- 先找明細欄位定義檔相關設定
	Select @ChkProgram = ChkProgram,  @ParmIn = ParmIn
      from GLColDef with (nolock)
	 Where Company = @Company  
       and ColID =  @ColID

	-- 若找不到則返回空字串
	If @@ROWCOUNT = 0 RETURN ''
	-- 若沒有設定檢查的Table則返回空字串
	If LTrim(RTrim(@ChkProgram)) = ''  RETURN ''
	
	-- 組合查詢條件SQL指令
	-- 由 CodeMaster 取回代號的名稱
	If @ChkProgram = 'CodeDesc' 
		Begin
			Select @CodeName = [CodeName] 
              From CodeDesc with (nolock)
			 Where [CodeID] = @ParmIn
			   and [CodeCode] = @CodeCode
		End
	-- 由 Department 取回部門的名稱
	ELSE If @ChkProgram = 'Department'
		Begin
			Select @CodeName = [DepName] 
              From Department with (nolock)
			 Where  [DepCode] = @CodeCode
			 --[Company] = @Company
		End

--	-- 若找不到則返回空字串
--	If @@ROWCOUNT = 0 RETURN ''
--	-- 若沒有設定欄位值則返回空字串
--	If LTrim(@CodeName) = ''  RETURN ''
	-- Return the result of the function
	RETURN @CodeName
END






