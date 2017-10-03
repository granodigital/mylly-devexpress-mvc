CREATE TABLE [dbo].[Order](
	[OrderId] [int] NOT NULL,
	[OrderName] [nvarchar](150) NULL,
	[TotalPrice] [int] NULL,
	[DesiredDeliveryDate] [datetime] NULL,
	[ValidUntil] [datetime] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

------------------------------------------------------------

CREATE TABLE [dbo].[Product](
	[RowId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductName] [nvarchar](150) NULL,
	[MaterialId] [int] NULL,
	[ProductPrice] [int] NULL,
	[IncludeProductPrice] [bit] NULL,
 CONSTRAINT [PK_RowId] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO

ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_OrderId]
GO

------------------------------------------------------------

CREATE PROCEDURE [dbo].[AddNewOrder]
    @OrderId [INT],
	@OrderName [NVARCHAR](150) ,
	@DesiredDeliveryDate [DATETIME] = NULL ,
	@ValidUntil [DATETIME] = NULL ,
	@TotalPrice [INT] = NULL 
	
AS
BEGIN
	--SET NOCOUNT OFF;

	BEGIN
	INSERT INTO [dbo].[Order] 
	( [OrderId] ,  
	[OrderName] ,
	[DesiredDeliveryDate] ,
	[ValidUntil] ,
	[TotalPrice]
	)
	OUTPUT  inserted.OrderId
	VALUES	   ( @OrderId,
	@OrderName ,
	@DesiredDeliveryDate ,
	@ValidUntil ,
	@TotalPrice
	)
	END
END

------------------------------------------------------------

CREATE PROCEDURE [dbo].[AddNewProduct]
    @ProductId [INT],
	 @OrderId [INT],
	@ProductName [NVARCHAR](150) ,
	@MaterialId [INT] = NULL ,
	@ProductPrice [INT] = NULL ,
	@IncludeProductPrice [BIT] = NULL
	
AS
BEGIN
	--SET NOCOUNT OFF;

	
	BEGIN
		INSERT INTO [dbo].[Product] 
		( [ProductId] ,
		  [OrderId],  
		[ProductName] ,
		[MaterialId] ,
		[ProductPrice] ,
		[IncludeProductPrice]
		)
	OUTPUT  inserted.OrderId

	VALUES	 (     @ProductId ,
		 @OrderId ,
		 @ProductName,
		@MaterialId  ,
		@ProductPrice  ,
		@IncludeProductPrice
		)
	END
END

-----------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GetLastOrder]
	
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT TOP(1) [OrderId],
	     	[OrderName] ,
	            [TotalPrice],	
		[DesiredDeliveryDate] ,
		[ValidUntil] 
			
	FROM [Order]
	Order By [OrderId] Desc 
END




GO

--------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GetOrderInformation]
	@OrderId [INT]
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT  [OrderName] ,
	        [TotalPrice],	
	       [DesiredDeliveryDate] ,
	       [ValidUntil] 
			
	FROM [Order]
	WHERE [OrderId] = @OrderId;
END


GO

-------------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UpdateOrder]
    @OrderId [INT],
	@OrderName [NVARCHAR](150) ,
	@DesiredDeliveryDate [DATETIME] = NULL ,
	@ValidUntil [DATETIME] = NULL ,
	@TotalPrice [INT] = NULL 
	
AS
BEGIN
		
	BEGIN
		UPDATE [dbo].[Order] 
		SET  
		[OrderName] = @OrderName  ,
		[DesiredDeliveryDate] = @DesiredDeliveryDate,
		[ValidUntil]  =@ValidUntil  ,
		[TotalPrice] = @TotalPrice
				
	WHERE [OrderId]= @OrderId
					
	END
END

-----------------------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UpdateProduct]
    @ProductId [INT],
	@OrderId [INT],
	@ProductName [NVARCHAR](150) ,
	@MaterialId [INT] = NULL ,
	@ProductPrice [INT] = NULL ,
	@IncludeProductPrice [BIT] = NULL
	
AS
BEGIN
	
	BEGIN
		
	UPDATE [dbo].[Product] 
	SET            [ProductName] = @ProductName ,
		[MaterialId] = @MaterialId,
		[ProductPrice] = @ProductPrice ,
		[IncludeProductPrice] = @IncludeProductPrice
	WHERE 	[OrderId]= @OrderId	AND   [ProductId] = @ProductId
				 
	
	END
END
