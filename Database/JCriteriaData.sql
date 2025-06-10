SET IDENTITY_INSERT [dbo].[Modules] ON 
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName]) VALUES (1, N'��� 01.01')
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName]) VALUES (2, N'��� 01.02')
GO
SET IDENTITY_INSERT [dbo].[Modules] OFF
GO
SET IDENTITY_INSERT [dbo].[Criteria] ON 
GO
INSERT [dbo].[Criteria] ([CriterionId], [ModuleId], [CriterionNumber], [CriterionTitle], [MaxScore]) VALUES (1, 1, 1, N'������ ������� �����', 5)
GO
INSERT [dbo].[Criteria] ([CriterionId], [ModuleId], [CriterionNumber], [CriterionTitle], [MaxScore]) VALUES (2, 1, 2, N'������ ����� �������', 3)
GO
INSERT [dbo].[Criteria] ([CriterionId], [ModuleId], [CriterionNumber], [CriterionTitle], [MaxScore]) VALUES (3, 1, 3, N'������ �������', 5)
GO
INSERT [dbo].[Criteria] ([CriterionId], [ModuleId], [CriterionNumber], [CriterionTitle], [MaxScore]) VALUES (4, 1, 4, N'������ ������� � ��������� ������', 2)
GO
INSERT [dbo].[Criteria] ([CriterionId], [ModuleId], [CriterionNumber], [CriterionTitle], [MaxScore]) VALUES (5, 1, 5, N'����� ���������� ����', 3)
GO
INSERT [dbo].[Criteria] ([CriterionId], [ModuleId], [CriterionNumber], [CriterionTitle], [MaxScore]) VALUES (6, 2, 6, N'��������� ������ ��������� ������', 1)
GO
INSERT [dbo].[Criteria] ([CriterionId], [ModuleId], [CriterionNumber], [CriterionTitle], [MaxScore]) VALUES (7, 2, 7, N'����������� ����� ��� ������� �������', 5)
GO
SET IDENTITY_INSERT [dbo].[Criteria] OFF
GO
SET IDENTITY_INSERT [dbo].[ScoreOptions] ON 
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (1, 1, 0, N'���������� �����������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (2, 1, 1, N'������� ��������� �������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (3, 1, 4, N'������� ��������� �������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (4, 1, 5, N'������ �������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (5, 2, 0, N'���������� �����������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (6, 2, 1, N'��������� �������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (7, 2, 2, N'��������� �������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (8, 2, 3, N'������ �������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (9, 3, 0, N'���������� �����������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (10, 3, 1, N'�������� �����������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (11, 3, 4, N'�������� �����������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (12, 3, 5, N'����������� ����� (���� �������� ������������ �������� ���������� P)')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (13, 4, 1, N'������ ������� � ����� ��������� �����')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (14, 4, 2, N'������ ����� ������ � ��������� �����')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (15, 5, 0, N'��� ������������, ����� ���������� �� �������� �� ����������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (16, 5, 1, N'�������� �������� ���������� ��� ���� ��������� �����������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (17, 5, 2, N'����� �������� ���������� � �������� ���� �����������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (18, 5, 3, N'�� �������� ����������, ���� ���������� �����������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (19, 6, 0, N'���������� �����������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (20, 6, 1, N'������ �������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (21, 7, 0, N'��� ������')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (22, 7, 1, N'���������� 1 ����')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (23, 7, 2, N'����������� 2 �����')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (24, 7, 3, N'����������� 3 �����')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (25, 7, 4, N'����������� 4 �����')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (26, 7, 5, N'���. ���� �� �������� ��������� ������������ ��������')
GO
SET IDENTITY_INSERT [dbo].[ScoreOptions] OFF
GO
