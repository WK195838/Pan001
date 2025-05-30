/****** Object:  UserDefinedFunction [dbo].[fnGetAcnoIdx]    Script Date: 08/12/2013 11:51:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==================================================================
-- Author:			Wangme
-- Create date:	2009/09/09
-- Description:	自定Function，傳入公司別、科目明細定義碼
-- 傳回科目明細碼之欄位名稱
-- ==================================================================
ALTER FUNCTION [dbo].[fnGetAcnoIdx] (@Company char(2), @ColID char(2) )
RETURNS varchar(20) 
AS
BEGIN
	Declare @ColName varchar(20)
	-- 先找明細欄位定義檔相關設定
	Select @ColName = ColName from GLColDef with (nolock)
	 Where Company = @Company  and ColID =  @ColID
	 
	if(@ColName is null )
	 select @ColName=ColName from GLColDef with (nolock)
	 Where  ColID =  @ColID
	-- 返回欄位名稱
	RETURN @ColName
END

