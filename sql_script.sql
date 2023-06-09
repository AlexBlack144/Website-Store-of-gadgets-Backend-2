USE [asp_tables]
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'0187e706-2a39-4625-9723-76d6b5499b5d', N'User', N'USER', N'8ba74f75-3ab8-4a1c-8aa8-6c39dc11e68d')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'51f3eeb9-fca1-461a-893e-a7922af66c87', N'Admin', N'ADMIN', N'cd5d4a24-aba3-4949-bbae-9490034869a7')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'9d7f5d6e-5c25-4343-8d49-16fefe987df3', N'Manager', N'MANAGER', N'd3ec8a96-a92f-4dd7-aded-d3b52b8f97a9')
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'0e21813f-9080-4862-91c6-e60ef22880ba', N'UserTest', N'USERTEST', N'userTest@gmail.com', N'USERTEST@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEFD/qf66NqKo0I9hBLelvtK8hkav8invdWUoFlZckhkSFIYFq5cKzmyWPhV15duEPw==', N'YSHQQ2ORQUXKIRRNCW6ULHCKR2HQNQXZ', N'703da32f-dfd5-409d-8f56-9186f71f728f', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'25fcaf84-9065-4127-bf33-450618d1da1a', N'Admin', N'ADMIN', N'admin@gmail.com', N'ADMIN@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAENaggL6pmHiwrGn6kzY1a6GtkVus5Gwwo3Cv0Ed0OuusbC7bY+Y6GQr1aUnkyBWsMQ==', N'H5ND5PEMR2EBWRJQUGNYN2IQ4CFVYBTC', N'6051e1c5-4ff8-4e93-be1b-898bf35bc841', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'8aa10a6d-ebc1-48ee-801b-189d36e679e0', N'Manager', N'MANAGER', N'manager@gmail.com', N'MANAGER@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEMJAbo537mog4QbkxCqPU3DplSClLZDqrjZDpqGI3NGuvtCdDtnUMDZXTX7lf3/RzQ==', N'LQ3RF6J473SVLCCAEAWW52JJSX7WEVMY', N'745e803e-74b2-4513-8822-4c4c3e797a5b', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'25fcaf84-9065-4127-bf33-450618d1da1a', N'0187e706-2a39-4625-9723-76d6b5499b5d')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'25fcaf84-9065-4127-bf33-450618d1da1a', N'51f3eeb9-fca1-461a-893e-a7922af66c87')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'25fcaf84-9065-4127-bf33-450618d1da1a', N'9d7f5d6e-5c25-4343-8d49-16fefe987df3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'8aa10a6d-ebc1-48ee-801b-189d36e679e0', N'9d7f5d6e-5c25-4343-8d49-16fefe987df3')
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Id], [Name]) VALUES (1, N'Smartphone')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (2, N'TV')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (3, N'Tablets')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (4, N'watch')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Gadgets] ON 

INSERT [dbo].[Gadgets] ([Id], [Image], [Name], [Model], [Price], [Quantity], [Sold], [Status], [IdCategory], [IdCategoryNavigationId]) VALUES (38, N'https://content2.rozetka.com.ua/goods/images/big/286090453.jpg', N'Infinix ', N'Note 12 (X663D) 6/128GB Force Black', 7999, 100, 10, 1, 1, NULL)
INSERT [dbo].[Gadgets] ([Id], [Image], [Name], [Model], [Price], [Quantity], [Sold], [Status], [IdCategory], [IdCategoryNavigationId]) VALUES (39, N'https://content.rozetka.com.ua/goods/images/big/263857427.jpg', N'Samsung ', N'Galaxy M33 5G 6/128GB Green ', 8999, 200, 10, 0, 1, NULL)
INSERT [dbo].[Gadgets] ([Id], [Image], [Name], [Model], [Price], [Quantity], [Sold], [Status], [IdCategory], [IdCategoryNavigationId]) VALUES (40, N'https://content2.rozetka.com.ua/goods/images/big/221214135.jpg', N'Apple', N'iPhone 13 128GB Starlight', 32999, 200, NULL, 0, 1, NULL)
INSERT [dbo].[Gadgets] ([Id], [Image], [Name], [Model], [Price], [Quantity], [Sold], [Status], [IdCategory], [IdCategoryNavigationId]) VALUES (42, N'https://content1.rozetka.com.ua/goods/images/big/303985191.jpg', N'Samsung', N'UE43AU7100UXUA', 17699, 200, NULL, 1, 2, NULL)
INSERT [dbo].[Gadgets] ([Id], [Image], [Name], [Model], [Price], [Quantity], [Sold], [Status], [IdCategory], [IdCategoryNavigationId]) VALUES (43, N'https://content.rozetka.com.ua/goods/images/big/314482007.jpg', N'LG ', N'55UQ75006LF', 21999, 200, NULL, 1, 2, NULL)
INSERT [dbo].[Gadgets] ([Id], [Image], [Name], [Model], [Price], [Quantity], [Sold], [Status], [IdCategory], [IdCategoryNavigationId]) VALUES (44, N'https://content1.rozetka.com.ua/goods/images/big/292024516.png', N'Lenovo', N'Tab M10 HD (2nd Gen) Wi-Fi 3/32GB Iron Grey', 6499, 500, NULL, 1, 3, NULL)
INSERT [dbo].[Gadgets] ([Id], [Image], [Name], [Model], [Price], [Quantity], [Sold], [Status], [IdCategory], [IdCategoryNavigationId]) VALUES (45, N'https://content.rozetka.com.ua/goods/images/big/224010066.jpg', N'Apple ', N'iPad 10.2" 2021 Wi-Fi 64GB Silver', 16999, 500, NULL, 1, 3, NULL)
INSERT [dbo].[Gadgets] ([Id], [Image], [Name], [Model], [Price], [Quantity], [Sold], [Status], [IdCategory], [IdCategoryNavigationId]) VALUES (46, N'https://content2.rozetka.com.ua/goods/images/big/186993601.jpg', N'Samsung', N'Galaxy Tab A7 Lite LTE 32GB Grey', 6399, 200, NULL, 1, 3, NULL)
INSERT [dbo].[Gadgets] ([Id], [Image], [Name], [Model], [Price], [Quantity], [Sold], [Status], [IdCategory], [IdCategoryNavigationId]) VALUES (48, N'http://web-imgs-kursak.s3.eu-west-2.amazonaws.com/325584586.webp?X-Amz-Expires=3600&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAXTVX5PM2WVUPY72A/20230504/eu-west-2/s3/aws4_request&X-Amz-Date=20230504T202006Z&X-Amz-SignedHeaders=host&X-Amz-Signature=ed309ce93034d3918b04f47599e88614e123f0e88e20fecc83ed8da18a209b1a', N'Apple ', N'Watch SE (2022)', 12599, 0, 10, 0, 4, NULL)
SET IDENTITY_INSERT [dbo].[Gadgets] OFF
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230202100221_m1', N'6.0.6')
GO
