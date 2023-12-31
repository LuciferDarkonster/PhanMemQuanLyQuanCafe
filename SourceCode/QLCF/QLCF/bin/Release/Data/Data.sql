USE [master]
GO
/****** Object:  Database [QLCF]    Script Date: 12/22/2022 2:44:32 AM ******/
CREATE DATABASE [QLCF]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QLCF', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.LUCIFER\MSSQL\DATA\QLCF.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'QLCF_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.LUCIFER\MSSQL\DATA\QLCF_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [QLCF] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QLCF].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QLCF] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QLCF] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QLCF] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QLCF] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QLCF] SET ARITHABORT OFF 
GO
ALTER DATABASE [QLCF] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QLCF] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QLCF] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QLCF] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QLCF] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QLCF] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QLCF] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QLCF] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QLCF] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QLCF] SET  ENABLE_BROKER 
GO
ALTER DATABASE [QLCF] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QLCF] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QLCF] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QLCF] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QLCF] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QLCF] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QLCF] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QLCF] SET RECOVERY FULL 
GO
ALTER DATABASE [QLCF] SET  MULTI_USER 
GO
ALTER DATABASE [QLCF] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QLCF] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QLCF] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QLCF] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [QLCF] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [QLCF] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'QLCF', N'ON'
GO
ALTER DATABASE [QLCF] SET QUERY_STORE = OFF
GO
USE [QLCF]
GO
/****** Object:  UserDefinedFunction [dbo].[fuConvertToUnsign1]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fuConvertToUnsign1] ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END
GO
/****** Object:  Table [dbo].[Account]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[UserName] [nvarchar](100) NOT NULL,
	[DisplayName] [nvarchar](100) NOT NULL,
	[PassWord] [nvarchar](1000) NOT NULL,
	[Type] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bill]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bill](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[DateCheckIn] [date] NULL,
	[DateCheckOut] [date] NULL,
	[idTable] [int] NOT NULL,
	[status] [int] NOT NULL,
	[discount] [int] NULL,
	[totalPrice] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillInfo]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillInfo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idBill] [int] NOT NULL,
	[idFood] [int] NOT NULL,
	[count] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Food]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[idCategory] [int] NOT NULL,
	[price] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FoodCategory]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FoodCategory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TableCF]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TableCF](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[status] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Account] ([UserName], [DisplayName], [PassWord], [Type]) VALUES (N'Ad', N'Admin', N'13491237210251168254321844023779612100164', 1)
INSERT [dbo].[Account] ([UserName], [DisplayName], [PassWord], [Type]) VALUES (N'staff', N'Nhân viên', N'13491237210251168254321844023779612100164', 0)
GO
SET IDENTITY_INSERT [dbo].[Bill] ON 

INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (1, CAST(N'2022-12-22' AS Date), CAST(N'2022-12-22' AS Date), 1, 1, 0, 35000)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (2, CAST(N'2022-12-22' AS Date), CAST(N'2022-12-22' AS Date), 2, 1, 50, 17500)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (3, CAST(N'2022-12-22' AS Date), CAST(N'2022-12-22' AS Date), 1, 1, 0, 0)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (5, CAST(N'2022-12-22' AS Date), CAST(N'2022-12-22' AS Date), 1, 1, 0, 30000)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (6, CAST(N'2022-12-22' AS Date), CAST(N'2022-12-22' AS Date), 1, 1, 0, 30000)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (7, CAST(N'2022-12-22' AS Date), CAST(N'2022-12-22' AS Date), 1, 1, 30, 42000)
INSERT [dbo].[Bill] ([id], [DateCheckIn], [DateCheckOut], [idTable], [status], [discount], [totalPrice]) VALUES (10, CAST(N'2022-12-22' AS Date), CAST(N'2022-12-22' AS Date), 1, 1, 50, 17500)
SET IDENTITY_INSERT [dbo].[Bill] OFF
GO
SET IDENTITY_INSERT [dbo].[BillInfo] ON 

INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (1, 1, 1, 2)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (2, 2, 1, 2)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (7, 5, 24, 3)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (8, 6, 24, 3)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (9, 7, 24, 3)
INSERT [dbo].[BillInfo] ([id], [idBill], [idFood], [count]) VALUES (13, 10, 13, 1)
SET IDENTITY_INSERT [dbo].[BillInfo] OFF
GO
SET IDENTITY_INSERT [dbo].[Food] ON 

INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (1, N'Espresso/Long Black', 1, 35000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (2, N'Americano', 1, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (3, N'Cappuccino', 1, 35000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (4, N'Latte', 1, 35000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (5, N'Black Coffee', 1, 25000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (6, N'Milk Coffee', 1, 25000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (7, N'Trà đào cam sả', 2, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (8, N'Trà sữa', 2, 25000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (9, N'Trà vải', 2, 25000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (10, N'Trà atiso', 2, 25000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (11, N'Trà táo bạc hà', 2, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (12, N'Trà gừng', 2, 25000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (13, N'Caramel', 3, 35000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (14, N'Mocha', 3, 35000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (15, N'BlueBerry', 3, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (16, N'Matcha', 3, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (17, N'Oreo', 3, 35000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (18, N'Coffee', 3, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (19, N'Chuối', 4, 35000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (20, N'Bơ ', 4, 35000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (21, N'Chanh leo tuyết', 4, 40000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (22, N'Dưa hấu', 4, 35000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (23, N'Xoài', 4, 35000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (24, N'Thơm', 5, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (25, N'Chanh dây', 5, 20000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (26, N'Táo', 5, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (27, N'Dừa', 5, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (28, N'Bưởi', 5, 25000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (29, N'Cam', 5, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (30, N'Cà rốt', 5, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (31, N'Dừa tắc hạt chia', 6, 35000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (32, N'Soda việt quất', 6, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (33, N'Sữa chua trái cây', 6, 35000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (34, N'Nước sấu đá', 6, 20000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (35, N'Sữa bắp', 6, 20000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (36, N'Cheese Cake', 7, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (37, N'Panna Cotta', 7, 32000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (38, N'Mousse Cake', 7, 37000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (39, N'Cookies', 7, 30000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (40, N'Hạt hướng dương', 7, 20000)
INSERT [dbo].[Food] ([id], [name], [idCategory], [price]) VALUES (41, N'Hạt bí', 7, 15000)
SET IDENTITY_INSERT [dbo].[Food] OFF
GO
SET IDENTITY_INSERT [dbo].[FoodCategory] ON 

INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (1, N'Itailan Coffee')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (2, N'Tea')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (3, N'Ice Blended')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (4, N'Smoothies')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (5, N'Juices')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (6, N'Special Drinks')
INSERT [dbo].[FoodCategory] ([id], [name]) VALUES (7, N'Cake & Snacks')
SET IDENTITY_INSERT [dbo].[FoodCategory] OFF
GO
SET IDENTITY_INSERT [dbo].[TableCF] ON 

INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (1, N'Bàn số 1', N'Trống')
INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (2, N'Bàn số 2', N'Trống')
INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (3, N'Bàn số 3', N'Trống')
INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (4, N'Bàn số 4', N'Trống')
INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (5, N'Bàn số 5', N'Trống')
INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (6, N'Bàn số 6', N'Trống')
INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (7, N'Bàn số 7', N'Trống')
INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (8, N'Bàn số 8', N'Trống')
INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (9, N'Bàn số 9', N'Trống')
INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (10, N'Bàn số 10', N'Trống')
INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (11, N'Bàn số 11', N'Trống')
INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (12, N'Bàn số 12', N'Trống')
INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (13, N'Bàn số 13', N'Trống')
INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (14, N'Bàn số 14', N'Trống')
INSERT [dbo].[TableCF] ([id], [name], [status]) VALUES (15, N'Bàn số 15', N'Trống')
SET IDENTITY_INSERT [dbo].[TableCF] OFF
GO
ALTER TABLE [dbo].[Bill] ADD  DEFAULT ((0)) FOR [status]
GO
ALTER TABLE [dbo].[BillInfo] ADD  DEFAULT ((0)) FOR [count]
GO
ALTER TABLE [dbo].[Food] ADD  DEFAULT (N'Chưa đặt tên') FOR [name]
GO
ALTER TABLE [dbo].[FoodCategory] ADD  DEFAULT (N'Chưa đặt tên') FOR [name]
GO
ALTER TABLE [dbo].[TableCF] ADD  DEFAULT (N'Chưa đặt tên') FOR [name]
GO
ALTER TABLE [dbo].[TableCF] ADD  DEFAULT (N'Trống') FOR [status]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD FOREIGN KEY([idTable])
REFERENCES [dbo].[TableCF] ([id])
GO
ALTER TABLE [dbo].[BillInfo]  WITH CHECK ADD FOREIGN KEY([idBill])
REFERENCES [dbo].[Bill] ([id])
GO
ALTER TABLE [dbo].[BillInfo]  WITH CHECK ADD FOREIGN KEY([idFood])
REFERENCES [dbo].[Food] ([id])
GO
ALTER TABLE [dbo].[Food]  WITH CHECK ADD FOREIGN KEY([idCategory])
REFERENCES [dbo].[FoodCategory] ([id])
GO
/****** Object:  StoredProcedure [dbo].[USP_ClearBill]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_ClearBill]
@idTable INT
AS
BEGIN
	DELETE dbo.BillInfo WHERE BillInfo.idBill = 
	(SELECT TOP 1 b.id FROM Bill AS b, TableCF AS t 
	WHERE  t.id = @idTable AND b.idTable = t.id 
	ORDER BY id DESC)
	DELETE dbo.Bill
	WHERE  id = (SELECT TOP 1 id FROM dbo.Bill
	WHERE  idTable = @idTable
	ORDER BY id DESC)
END
GO
/****** Object:  StoredProcedure [dbo].[USP_GetAccountByUserName]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetAccountByUserName]
@userName nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName
END
GO
/****** Object:  StoredProcedure [dbo].[USP_GetListBillByDate]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetListBillByDate]
@checkIn date, @checkOut date
AS
BEGIN
	SELECT t.name AS [Tên bàn], b.totalPrice AS [Tổng tiền], DateCheckIn AS [Ngày vào], DateCheckOut AS [Ngày ra], discount AS [Giảm giá %]
	FROM dbo.Bill AS b, dbo.TableCF AS t, dbo.BillInfo AS bi, dbo.Food AS f
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1
	AND t.id = b.idTable AND bi.idBill = b.id AND bi.idFood = f.id
END
GO
/****** Object:  StoredProcedure [dbo].[USP_GetListBillByDateAndPage]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetListBillByDateAndPage]
@checkIn date, @checkOut date, @page int
AS 
BEGIN
	DECLARE @pageRows INT = 20
	DECLARE @selectRows INT = @pageRows
	DECLARE @exceptRows INT = (@page - 1) * @pageRows
	
	;WITH BillShow AS( SELECT b.ID, t.name AS [Tên bàn], b.totalPrice AS [Tổng tiền], DateCheckIn AS [Ngày vào], DateCheckOut AS [Ngày ra], discount AS [Giảm giá]
	FROM dbo.Bill AS b,dbo.TableCF AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1
	AND t.id = b.idTable)
	
	SELECT TOP (@selectRows) * FROM BillShow WHERE id NOT IN (SELECT TOP (@exceptRows) id FROM BillShow)
END
GO
/****** Object:  StoredProcedure [dbo].[USP_GetListBillByDateForReport]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetListBillByDateForReport]
@checkIn date, @checkOut date
AS 
BEGIN
	SELECT t.name, b.totalPrice, DateCheckIn, DateCheckOut, discount
	FROM dbo.Bill AS b,dbo.TableCF AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1
	AND t.id = b.idTable
END
GO
/****** Object:  StoredProcedure [dbo].[USP_GetListFood]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetListFood]
AS
BEGIN
	SELECT f.id AS [Mã món], f.name AS [Tên món], fc.name AS [Tên loại món], f.price AS [Giá tiền]
	FROM dbo.Food AS f, dbo.FoodCategory AS fc
	WHERE f.idCategory = fc.id
END
GO
/****** Object:  StoredProcedure [dbo].[USP_GetNumBillByDate]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetNumBillByDate]
@checkIn date, @checkOut date
AS 
BEGIN
	SELECT COUNT(*)
	FROM dbo.Bill AS b,dbo.TableCF AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1
	AND t.id = b.idTable
END
GO
/****** Object:  StoredProcedure [dbo].[USP_GetTableList]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_GetTableList]
AS SELECT * FROM dbo.TableCF
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertBill]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_InsertBill]
@idTable INT
AS
BEGIN
	INSERT dbo.Bill (DateCheckIn, DateCheckOut, idTable, status, discount)
	VALUES ( GETDATE(), NULL, @idTable, 0, 0)
END
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertBillInfo]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--DROP TRIGGER UTG_UpdateBill
CREATE PROC [dbo].[USP_InsertBillInfo]
@idBill INT, @idFood INT, @count INT
AS
BEGIN

	DECLARE @isExitsBillInfo INT
	DECLARE @foodCount INT = 1
	
	SELECT @isExitsBillInfo = id, @foodCount = b.count 
	FROM dbo.BillInfo AS b 
	WHERE idBill = @idBill AND idFood = @idFood

	IF (@isExitsBillInfo > 0)
	BEGIN
		DECLARE @newCount INT = @foodCount + @count
		IF (@newCount > 0)
			UPDATE dbo.BillInfo	SET count = @foodCount + @count WHERE idFood = @idFood
		ELSE
			DELETE dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood
	END
	ELSE
	BEGIN
		INSERT	dbo.BillInfo
        ( idBill, idFood, count )
		VALUES  ( @idBill, -- idBill - int
          @idFood, -- idFood - int
          @count  -- count - int
          )
	END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_Login]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_Login]
@userName nvarchar(100), @passWord nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName AND PassWord = @passWord
END
GO
/****** Object:  StoredProcedure [dbo].[USP_SwitchTable]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_SwitchTable]
@idTable1 INT, @idTable2 int
AS BEGIN
	DECLARE @idFirstBill int
	DECLARE @idSeconrdBill INT	
	DECLARE @isFirstTablEmty INT = 1
	DECLARE @isSecondTablEmty INT = 1		
	SELECT @idSeconrdBill = id FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0
	SELECT @idFirstBill = id FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0	
	IF (@idFirstBill IS NULL)
	BEGIN
		INSERT dbo.Bill
		        ( DateCheckIn ,
		          DateCheckOut ,
		          idTable ,
		          status
		        )
		VALUES  ( GETDATE() , -- DateCheckIn - date
		          NULL , -- DateCheckOut - date
		          @idTable1 , -- idTable - int
		          0  -- status - int
		        )	        
		SELECT @idFirstBill = MAX(id) FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0		
	END	
	SELECT @isFirstTablEmty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idFirstBill	
	IF (@idSeconrdBill IS NULL)
	BEGIN
		INSERT dbo.Bill
		        ( DateCheckIn ,
		          DateCheckOut ,
		          idTable ,
		          status
		        )
		VALUES  ( GETDATE() , -- DateCheckIn - date
		          NULL , -- DateCheckOut - date
		          @idTable2 , -- idTable - int
		          0  -- status - int
		        )
		SELECT @idSeconrdBill = MAX(id) FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0	
	END	
	SELECT @isSecondTablEmty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idSeconrdBill	
	SELECT id INTO IDBillInfoTable FROM dbo.BillInfo WHERE idBill = @idSeconrdBill	
	UPDATE dbo.BillInfo SET idBill = @idSeconrdBill WHERE idBill = @idFirstBill	
	UPDATE dbo.BillInfo SET idBill = @idFirstBill WHERE id IN (SELECT * FROM IDBillInfoTable)	
	DROP TABLE IDBillInfoTable	
	IF (@isFirstTablEmty = 0)
		UPDATE dbo.TableCF SET status = N'Trống' WHERE id = @idTable2		
	IF (@isSecondTablEmty= 0)
		UPDATE dbo.TableCF SET status = N'Trống' WHERE id = @idTable1
END
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateAccount]    Script Date: 12/22/2022 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_UpdateAccount]
@userName NVARCHAR(100), @displayName NVARCHAR(100), @password NVARCHAR(100), @newPassword NVARCHAR(100)
AS
BEGIN
	DECLARE @isRightPass INT = 0
	
	SELECT @isRightPass = COUNT(*) FROM dbo.Account WHERE USERName = @userName AND PassWord = @password
	
	IF (@isRightPass = 1)
	BEGIN
		IF (@newPassword = NULL OR @newPassword = '')
		BEGIN
			UPDATE dbo.Account SET DisplayName = @displayName WHERE UserName = @userName
		END		
		ELSE
			UPDATE dbo.Account SET DisplayName = @displayName, PassWord = @newPassword WHERE UserName = @userName
	end
END
GO
USE [master]
GO
ALTER DATABASE [QLCF] SET  READ_WRITE 
GO
