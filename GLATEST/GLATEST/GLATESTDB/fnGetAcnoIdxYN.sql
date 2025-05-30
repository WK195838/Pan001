/****** Object:  UserDefinedFunction [dbo].[fnGetAcnoIdxYN]    Script Date: 08/16/2013 11:01:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ==================================================================
-- Author:			Wangme
-- Create date:	2009/09/09
-- Description:	自定Function，傳入公司別、科目明細定義碼
-- 傳回科目明細碼之是否有PopupDialog
-- ==================================================================
ALTER FUNCTION [dbo].[fnGetAcnoIdxYN] (@Company char(2), @ColID char(2) )
RETURNS char(1) AS
BEGIN
	If LTrim(RTrim(@ColID)) = ''  RETURN 'N'

	Declare @ChkProgram varchar(20)
	Declare @ParmIn varchar(20)
	Declare @KeyFieldName1 varchar(20)
	Declare @KeyFieldName2 varchar(20)
	Declare @DataFieldName1 varchar(20)
	Declare @DataFieldName2 varchar(20)
	Declare @rtnYN char(1)

	-- 先找明細欄位定義檔相關設定
	Select @ChkProgram = ChkProgram,  @ParmIn = ParmIn, @KeyFieldName1 = KeyFieldName1, @KeyFieldName2 = KeyFieldName2 
	, @DataFieldName1 = DataFieldName1, @DataFieldName2 = DataFieldName2 from GLColDef with (nolock)
	 Where Company = @Company  and ColID =  @ColID
	-- 找不到
	If @@ROWCOUNT = 0 Set @rtnYN = 'N'
	else Set @rtnYN = 'Y'
	
	-- 沒有設定檢查的Table
	If LTrim(@ChkProgram) = ''  Set @rtnYN = 'N'
	
	-- 有檢查、有資料則傳回 'Y'
	
	
	RETURN @rtnYN
END

