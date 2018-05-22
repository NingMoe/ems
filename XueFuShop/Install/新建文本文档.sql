SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[F_Split]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'Create FUNCTION [dbo].[F_Split]
 (
     @SplitString nvarchar(max),  --源字符串
     @Separator nvarchar(10)='' ''  --分隔符号，默认为空格
 )
 RETURNS @SplitStringsTable TABLE  --输出的数据表
 (
     [id] int identity(1,1),
     [value] nvarchar(max)
 )
 AS
 BEGIN
     DECLARE @CurrentIndex int;
     DECLARE @NextIndex int;
     DECLARE @ReturnText nvarchar(max);

     SELECT @CurrentIndex=1;
     WHILE(@CurrentIndex<=len(@SplitString))
         BEGIN
             SELECT @NextIndex=charindex(@Separator,@SplitString,@CurrentIndex);
             IF(@NextIndex=0 OR @NextIndex IS NULL)
                 SELECT @NextIndex=len(@SplitString)+1;
                 SELECT @ReturnText=substring(@SplitString,@CurrentIndex,@NextIndex-@CurrentIndex);
                 INSERT INTO @SplitStringsTable([value]) VALUES(@ReturnText);
                 SELECT @CurrentIndex=@NextIndex+1;
             END
     RETURN;
 END' 
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_ExistsKPITemplet]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_ExistsKPITemplet]
@kpiContent varchar(1000),
@companyId int
AS 
		DECLARE @returnId INT
        DECLARE @id INT
        DECLARE @templetID int
        DECLARE @tempKPIContent varchar(1000)
        DECLARE @TempREFER TABLE(id int identity(1,1),templetID INT,kpiContent varchar(1000))
        INSERT @TempREFER(templetID,kpiContent)
        SELECT Id,KPIContent From [_KPITemplet] where CompanyId=@companyId order by Id desc
        DECLARE @kpiIdNum INT
        DECLARE @tempKPIIdNum INT
		select @kpiIdNum=count(*) From dbo.F_Split(@kpiContent, '','')
		set @tempKPIIdNum=0
        set @returnId=0
        set @id=1
        while 1=1
        begin
            select @templetID=templetID,@tempKPIContent=kpiContent from @TempREFER where id=@id
            if @@ROWCOUNT=0 
        	begin
        		break
        	end
            else
        	begin
				select @tempKPIIdNum=count(*) From dbo.F_Split(@tempKPIContent, '','')
				if @tempKPIIdNum=@kpiIdNum
				begin
					Select @tempKPIIdNum=count([Id]) From [_KPI] Where [Id] in (select value From dbo.F_Split(@tempKPIContent, '','')) and [Id] not in (select value From dbo.F_Split(@kpiContent, '',''))
					if @tempKPIIdNum=0
					begin
        				set @returnId=@templetID
        				break
        			end
				end
        	end
            set @id=@id+1
        end
		select @returnId
' 
END


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_ReadWorkingPostView]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_ReadWorkingPostView]
@id int
AS
		SELECT * FROM [_WorkingPostView] WHERE [PostId]=@id
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_SearchWorkingPostViewList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_SearchWorkingPostViewList]
@condition nvarchar(4000)
AS 
	IF @condition=''''
		SELECT * FROM [_WorkingPostView] order by [PostName]
	ELSE
		EXEC(''SELECT * FROM [_WorkingPostView] WHERE ''+ @condition + '' order by [PostName]'')

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_DeleteWorkingPost]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[_DeleteWorkingPost]
@strID nvarchar(800)
AS 
	IF @strID=''''
		RETURN	

		EXEC (''Update [_WorkingPost] Set [State]=1 WHERE [ID] IN (''+@strID+'')'')
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_DeleteWorkingPostByCompanyID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[_DeleteWorkingPostByCompanyID]
@companyID nvarchar(800)
AS 
	IF @companyID=''''
		RETURN	

		EXEC (''Update [_WorkingPost] Set [State]=1 WHERE [CompanyID] IN(''+@companyID+'')'')
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_SearchWorkingPostList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_SearchWorkingPostList]
@condition nvarchar(4000)
AS 
	IF @condition=''''
		SELECT * FROM [_WorkingPost] Order By [CompanyId],[ParentID],[Sort]
	ELSE
		EXEC(''SELECT * FROM [_WorkingPost] WHERE ''+ @condition + '' Order By [CompanyId],[ParentID],[Sort]'')

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_AddWorkingPost]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_AddWorkingPost]
@postName nvarchar(100),
@parentId int,
@companyId int,
@state int,
@isPost int,
@kpiTempletId int,
@sort int,
@path varchar(100)
AS 
	INSERT INTO [_WorkingPost]([PostName],[ParentId],[CompanyId],[State],[IsPost],[KPITempletId],[Sort],[Path]) VALUES(@postName,@parentId,@companyId,@state,@isPost,@kpiTempletId,@sort,@path)
	SELECT @@identity
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_UpdateWorkingPost]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_UpdateWorkingPost]
@id int,
@postName nvarchar(100),
@parentId int,
@companyId int,
@state int,
@isPost int,
@kpiTempletId int,
@sort int,
@path varchar(100)
AS 
	Update [_WorkingPost] Set [PostName]=@postName,[ParentId]=@parentId,[CompanyId]=@companyId,[State]=@state,[IsPost]=@isPost,[KPITempletId]=@kpiTempletId,[Sort]=@sort,[Path]=@path Where ID=@id
	SELECT @@identity
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_ReadWorkingPost]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_ReadWorkingPost]
@id int
AS
		SELECT * FROM [_WorkingPost] WHERE [ID]=@id
' 
END


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_DeleteKPI]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[_DeleteKPI]
@strID nvarchar(800)
AS 
	IF @strID=''''
		RETURN	

		EXEC (''Update _KPI Set [State]=1 WHERE [ID] IN (''+@strID+'')'')
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_DeleteKPIBycompanyID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[_DeleteKPIBycompanyID]
@companyID nvarchar(800)
AS 
	IF @companyID=''''
		RETURN	

		EXEC (''Update _KPI Set [State]=1 WHERE [CompanyID] IN(''+@companyID+'')'')
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_DeleteKPIEvaluateByuserID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[_DeleteKPIEvaluateByuserID]
@userID nvarchar(800)
AS 
	IF @userID=''''
		RETURN	

		EXEC (''Update _KPIEvaluate Set [State]=1 WHERE [userID] IN(''+@userID+'')'')
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_ReadKPIEvaluate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_ReadKPIEvaluate]
@id int
AS
		SELECT * FROM _KPIEvaluate WHERE [ID]=@id
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_SearchKPIEvaluateList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_SearchKPIEvaluateList]
@condition nvarchar(4000)
AS 
	IF @condition=''''
		SELECT * FROM _KPIEvaluate order by [Id] desc
	ELSE
		EXEC(''SELECT * FROM _KPIEvaluate WHERE ''+ @condition + '' order by [Id] desc'')

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_DeleteKPITemplet]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[_DeleteKPITemplet]
@strID nvarchar(800)
AS 
	IF @strID=''''
		RETURN	

		EXEC (''Update _KPITemplet Set [State]=1 WHERE [ID] IN (''+@strID+'')'')
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_DeleteKPITempletBycompanyID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[_DeleteKPITempletBycompanyID]
@companyID nvarchar(800)
AS 
	IF @companyID=''''
		RETURN	

		EXEC (''Update _KPITemplet Set [State]=1 WHERE [CompanyID] IN(''+@companyID+'')'')
' 
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_UpdateKPIEvaluate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_UpdateKPIEvaluate]
@id int,
@scorse float,
@rate int,
@state int,
@evaluateNameId int,
@evaluateUserId int
AS 
	Update _KPIEvaluate Set [Scorse]=@scorse,[Rate]=@rate,[State]=@state,[EvaluateNameId]=@evaluateNameId,[EvaluateUserId]=@evaluateUserId Where ID=@id
	SELECT @@identity
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_AddPostKPISetting]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_AddPostKPISetting]
@name nvarchar(50),
@kPIContent varchar(1000),
@companyId int,
@state int
AS 
	INSERT INTO _PostKPISetting([Name],[KPIContent],[CompanyId],[State]) VALUES(@name,@kPIContent,@companyId,@state)
	SELECT @@identity
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_AddKPI]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_AddKPI]
@name nvarchar(100),
@parentId int,
@evaluateInfo nvarchar(500),
@method nvarchar(500),
@scorse float,
@other ntext,
@companyID int,
@state int,
@sort int,
@refPostId int,
@type int
AS 
	INSERT INTO _KPI([Name],[ParentId],[evaluateInfo],[method],[scorse],[other],[companyID],[state],[sort],[refPostId],[Type]) VALUES(@name,@parentId,@evaluateInfo,@method,@scorse,@other,@companyID,@state,@sort,@refPostId,@type)
	SELECT @@identity
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_UpdateKPI]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_UpdateKPI]
@id int,
@name nvarchar(100),
@parentId int,
@evaluateInfo nvarchar(500),
@method nvarchar(500),
@scorse float,
@other ntext,
@companyID int,
@state int,
@sort int,
@refPostId int,
@type int
AS 
	Update _KPI Set [Name]=@name,[ParentId]=@parentId,[EvaluateInfo]=@evaluateInfo,[Method]=@method,[Scorse]=@scorse,[Other]=@other,[CompanyID]=@companyID,[State]=@state,[Sort]=@sort,[RefPostId]=@refPostId,[Type]=@type Where ID=@id
	SELECT @@identity
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_SearchKPIList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_SearchKPIList]
@condition nvarchar(4000)
AS 
	IF @condition=''''
		SELECT * FROM [_KPI] order by [Name],[Id] desc
	ELSE
		EXEC(''SELECT * FROM [_KPI] WHERE ''+ @condition + '' order by [Sort],[Name]'')

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_ReadKPI]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_ReadKPI]
@id int
AS
		SELECT * FROM _KPI WHERE [ID]=@id
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_AddKPITemplet]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_AddKPITemplet]
@postId int,
@kpiContent varchar(1000),
@companyId int,
@state int
AS 
	INSERT INTO _KPITemplet([PostId],[KPIContent],[CompanyId],[State]) VALUES(@postId,@kpiContent,@companyId,@state)
	SELECT @@identity
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_UpdateKPITemplet]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_UpdateKPITemplet]
@id int,
@postId int,
@kpiContent varchar(1000),
@companyId int,
@state int
AS 
	Update _KPITemplet Set [postId]=@postId,[KPIContent]=@kpiContent,[CompanyId]=@companyId,[State]=@state Where ID=@id
	SELECT @@identity
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_ReadKPITemplet]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_ReadKPITemplet]
@id int
AS
		SELECT * FROM _KPITemplet WHERE [ID]=@id
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_SearchKPITempletList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_SearchKPITempletList]
@condition nvarchar(4000)
AS 
	IF @condition=''''
		SELECT * FROM _KPITemplet order by [Id] desc
	ELSE
		EXEC(''SELECT * FROM _KPITemplet WHERE ''+ @condition + '' order by [Id] desc'')

' 
END


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_DeleteEvaluateNameBycompanyID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[_DeleteEvaluateNameBycompanyID]
@companyID nvarchar(800)
AS 
	IF @companyID=''''
		RETURN	

		EXEC (''delete from [_EvaluateName] WHERE [CompanyID] IN(''+@companyID+'')'')
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_AddEvaluateName]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_AddEvaluateName]
@evaluateName nvarchar(100),
@startDate datetime,
@endDate datetime,
@companyID int,
@state int
AS 
	INSERT INTO [_EvaluateName]([EvaluateName],[StartDate],[EndDate],[CompanyId],[State]) VALUES(@evaluateName,@startDate,@endDate,@companyID,@state)
	SELECT @@identity
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_UpdateEvaluateName]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_UpdateEvaluateName]
@id int,
@evaluateName nvarchar(100),
@startDate datetime,
@endDate datetime,
@companyID int,
@state int
AS 
	Update [_EvaluateName] Set [EvaluateName]=@evaluateName,[StartDate]=@startDate,[EndDate]=@endDate,[CompanyID]=@companyID,[State]=@state Where ID=@id
	SELECT @@identity
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_ReadEvaluateName]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_ReadEvaluateName]
@id int
AS
		SELECT [Id],[EvaluateName],CONVERT(varchar(100), [StartDate], 23) as [StartDate],CONVERT(varchar(100), [EndDate], 23) as [EndDate],CONVERT(varchar(100), [Date], 23) as [Date],[CompanyId],[State] FROM [_EvaluateName] WHERE [ID]=@id
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_SearchEvaluateNameList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_SearchEvaluateNameList]
@condition nvarchar(4000)
AS 
	IF @condition=''''
		SELECT [Id],[EvaluateName],CONVERT(varchar(100), [StartDate], 23) as [StartDate],CONVERT(varchar(100), [EndDate], 23) as [EndDate],CONVERT(varchar(100), [Date], 23) as [Date],[CompanyId],[State] FROM [_EvaluateName] order by [ID] desc
	ELSE
		EXEC(''SELECT [Id],[EvaluateName],CONVERT(varchar(100), [StartDate], 23) as [StartDate],CONVERT(varchar(100), [EndDate], 23) as [EndDate],CONVERT(varchar(100), [Date], 23) as [Date],[CompanyId],[State] FROM [_EvaluateName] WHERE ''+ @condition + '' order by [ID] desc'')

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_DeleteEvaluateName]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_DeleteEvaluateName]
@strID varchar(500)
AS 
	IF @strID=''''
		RETURN	

		Update [_EvaluateName] Set [State]=1 WHERE [ID] in (@strID)
' 
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_ChangeKPIEvaluateStateByEvaluateNameId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_ChangeKPIEvaluateStateByEvaluateNameId]
@evaluateNameId nvarchar(800),
@state int
AS 
	IF @evaluateNameId=''''
		RETURN	

		EXEC (''Update [_KPIEvaluate] Set [State]=''+@state+'' WHERE [EvaluateNameId] IN (''+@evaluateNameId+'')'')
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_AddKPIEvaluate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_AddKPIEvaluate]
@userId int,
@postId int,
@kPIId int,
@scorse float,
@rate int,
@state int,
@evaluateNameId int,
@evaluateUserId int
AS 
    declare @recordNum int
    select @recordNum=count(*) from [_KPIEvaluate] where [UserId]=@userId and [PostId]=@postId and [KPIId]=@kPIId and [EvaluateNameId]=@evaluateNameId and [evaluateUserId]=@evaluateUserId
    if (@recordNum>0)
	begin
		Update [_KPIEvaluate] Set [Scorse]=@scorse,[Rate]=@rate,[State]=@state,[EvaluateNameId]=@evaluateNameId,[EvaluateUserId]=@evaluateUserId Where [UserId]=@userId and [PostId]=@postId and [KPIId]=@kPIId and [EvaluateNameId]=@evaluateNameId and [evaluateUserId]=@evaluateUserId
		SELECT [ID] From [_KPIEvaluate] Where [UserId]=@userId and [PostId]=@postId and [KPIId]=@kPIId and [EvaluateNameId]=@evaluateNameId and [evaluateUserId]=@evaluateUserId
	end
	else
	begin
		INSERT INTO [_KPIEvaluate]([UserId],[PostId],[KPIId],[Scorse],[Rate],[State],[EvaluateNameId],[EvaluateUserId]) VALUES(@userId,@postId,@kPIId,@scorse,@rate,@state,@evaluateNameId,@evaluateUserId)
		SELECT @@identity
	end
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_SearchFixedKPIEvaluateList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_SearchFixedKPIEvaluateList]
@condition nvarchar(4000)
AS 
	IF @condition=''''
		SELECT * FROM [_KPIEvaluate] order by [Id] desc
	ELSE
		EXEC(''select * from (select row_number() over(partition by [KPIId] order by [Rate] Desc,[enddate] Desc) as [rownum],[kpiid],[Rate] from [_KPIEvaluate],[_Evaluatename] where [_evaluatename].id=evaluatenameid and  ''+ @condition + '' group by kpiid,Rate,enddate) as T where T.rownum = 1'')

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_CompletelyDeleteKPIEvaluate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[_CompletelyDeleteKPIEvaluate]
@userId Int,
@postId Int,
@evaluateNameId int
AS
	DELETE FROM [_KPIEvaluate] WHERE [UserId]=@userId and [PostId]=@postId and [EvaluateNameId]=@evaluateNameId
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_NewKPIEvaluateReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_NewKPIEvaluateReport]
@condition nvarchar(4000)
AS 
	IF @condition=''''
		SELECT (SELECT [EvaluateName] FROM [_EvaluateName] WHERE [Id]=[EvaluateNameId]) AS [EvaluateName],[EvaluateNameId],(SELECT [RealName] FROM [_User] WHERE [ID]=[UserId]) AS [UserName],[UserId],(SELECT ISNULL([PostName],'''') FROM [_WorkingPostView] WHERE [PostId]=[_KPIEvaluateView].[PostId]) AS [PostName],[PostId],(SELECT len([KPIContent])-len(replace([KPIContent],'','',''''))+1 FROM [_WorkingPostView] WHERE [PostId]=[_KPIEvaluateView].[PostId]) AS [WorkingPostKPINum],SUM (CASE WHEN [Type] =1 THEN 1 ELSE 0 END) AS [FixKPINum],SUM (CASE WHEN [Type] =0 THEN 1 ELSE 0 END) AS [TempKPINum],SUM(CASE WHEN [Rate]=100 THEN 1 ELSE 0 END) AS [CompleteNum],SUM(CASE WHEN [Rate]=100 THEN [Scorse] ELSE 0 END) AS [TotalScore] FROM [_KPIEvaluateView] WHERE [PostId]<>0 GROUP BY EvaluateNameId,UserId,PostId ORDER BY [EvaluateNameId] DESC,[UserName],[PostName]
	ELSE
		EXEC(''SELECT (SELECT [EvaluateName] FROM [_EvaluateName] WHERE [Id]=[EvaluateNameId]) AS [EvaluateName],[EvaluateNameId],(SELECT [RealName] FROM [_User] WHERE [ID]=[UserId]) AS [UserName],[UserId],(SELECT ISNULL([PostName],'''''''') FROM [_WorkingPostView] WHERE [PostId]=[_KPIEvaluateView].[PostId]) AS [PostName],[PostId],(SELECT len([KPIContent])-len(replace([KPIContent],'''','''',''''''''))+1 FROM [_WorkingPostView] WHERE [PostId]=[_KPIEvaluateView].[PostId]) AS [WorkingPostKPINum],SUM (CASE WHEN [Type] =1 THEN 1 ELSE 0 END) AS [FixKPINum],SUM (CASE WHEN [Type] =0 THEN 1 ELSE 0 END) AS [TempKPINum],SUM(CASE WHEN [Rate]=100 THEN 1 ELSE 0 END) AS [CompleteNum],SUM(CASE WHEN [Rate]=100 THEN [Scorse] ELSE 0 END) AS [TotalScore] FROM [_KPIEvaluateView] WHERE [PostId]<>0 AND ''+ @condition + '' GROUP BY EvaluateNameId,UserId,PostId ORDER BY [EvaluateNameId] DESC,[UserName],[PostName]'')
' 
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_DeleteCourseCate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_DeleteCourseCate]
@CateId int
AS
	 DECLARE @temp int
	 SELECT @temp=COUNT(*) FROM [_CourseCate] WHERE [ParentCateId]=@CateId
	 IF @temp=0
	 DELETE FROM [_CourseCate] WHERE [CateId]=@CateId
' 
END


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_MoveDownPost]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_MoveDownPost]
@id int
AS 
	DECLARE @tempID int
	DECLARE @tempOrderID int
	DECLARE @orderID int
	DECLARE @fatherID int
	SELECT @orderID=[OrderIndex],@fatherID=[ParentId] FROM [_Post] WHERE [PostId]=@id
	SELECT TOP 1 @tempID=[PostId],@tempOrderID=[OrderIndex] FROM [_Post] WHERE [OrderIndex]>@orderID AND [ParentId]=@fatherID ORDER BY [OrderIndex] ASC

	IF @tempID is null
		RETURN		
	ELSE
		BEGIN
		UPDATE [_Post] SET [OrderIndex]=@tempOrderID WHERE [PostId]=@id
		UPDATE [_Post] SET [OrderIndex]=@orderID WHERE [PostId]=@tempID
		END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_MoveUpPost]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_MoveUpPost]
@id int
AS 
	DECLARE @tempID int
	DECLARE @tempOrderID int
	DECLARE @OrderIndex int
	DECLARE @fatherID int
	SELECT @OrderIndex=[OrderIndex],@fatherID=[ParentId] FROM [_Post] WHERE [PostId]=@id
	SELECT TOP 1 @tempID=[PostId],@tempOrderID=[OrderIndex] FROM [_Post] WHERE [OrderIndex]<@OrderIndex AND [ParentId]=@fatherID ORDER BY [OrderIndex] DESC

	IF @tempID is null
		RETURN		
	ELSE
		BEGIN
		UPDATE [_Post] SET [OrderIndex]=@tempOrderID WHERE [PostId]=@id
		UPDATE [_Post] SET [OrderIndex]=@OrderIndex WHERE [PostId]=@tempID
		END
' 
END


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_UpdatePostPlan]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_UpdatePostPlan]
@PostId int,
@PostPlan varchar(2000)
AS
	UPDATE [_Post] Set [PostPlan]=@PostPlan WHERE [PostId]=@PostId
' 
END
