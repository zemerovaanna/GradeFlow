SET IDENTITY_INSERT [dbo].[Modules] ON 
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName]) VALUES (1, N'МДК 01.01')
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName]) VALUES (2, N'МДК 01.02')
GO
SET IDENTITY_INSERT [dbo].[Modules] OFF
GO
SET IDENTITY_INSERT [dbo].[Criteria] ON 
GO
INSERT [dbo].[Criteria] ([CriterionId], [ModuleId], [CriterionNumber], [CriterionTitle], [MaxScore]) VALUES (1, 1, 1, N'Описан базовый класс', 5)
GO
INSERT [dbo].[Criteria] ([CriterionId], [ModuleId], [CriterionNumber], [CriterionTitle], [MaxScore]) VALUES (2, 1, 2, N'Описан класс потомок', 3)
GO
INSERT [dbo].[Criteria] ([CriterionId], [ModuleId], [CriterionNumber], [CriterionTitle], [MaxScore]) VALUES (3, 1, 3, N'Методы классов', 5)
GO
INSERT [dbo].[Criteria] ([CriterionId], [ModuleId], [CriterionNumber], [CriterionTitle], [MaxScore]) VALUES (4, 1, 4, N'Классы описаны в отдельных файлах', 2)
GO
INSERT [dbo].[Criteria] ([CriterionId], [ModuleId], [CriterionNumber], [CriterionTitle], [MaxScore]) VALUES (5, 1, 5, N'Стиль оформления кода', 3)
GO
INSERT [dbo].[Criteria] ([CriterionId], [ModuleId], [CriterionNumber], [CriterionTitle], [MaxScore]) VALUES (6, 2, 6, N'Подключен проект модульных тестов', 1)
GO
INSERT [dbo].[Criteria] ([CriterionId], [ModuleId], [CriterionNumber], [CriterionTitle], [MaxScore]) VALUES (7, 2, 7, N'Разработаны тесты для методов классов', 5)
GO
SET IDENTITY_INSERT [dbo].[Criteria] OFF
GO
SET IDENTITY_INSERT [dbo].[ScoreOptions] ON 
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (1, 1, 0, N'реализация отсутствует')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (2, 1, 1, N'имеется частичное решение')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (3, 1, 4, N'имеется частичное решение')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (4, 1, 5, N'полное решение')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (5, 2, 0, N'реализация отсутствует')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (6, 2, 1, N'частичное решение')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (7, 2, 2, N'частичное решение')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (8, 2, 3, N'полное решение')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (9, 3, 0, N'реализация отсутствует')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (10, 3, 1, N'частично реализованы')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (11, 3, 4, N'частично реализованы')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (12, 3, 5, N'реализованы верно (есть контроль корректности значения переменной P)')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (13, 4, 1, N'классы описаны в одном отдельном файле')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (14, 4, 2, N'каждый класс описан в отдельном файле')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (15, 5, 0, N'нет комментариев, имена переменных не отражают их назначение')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (16, 5, 1, N'частично отражают назначение или есть частичные комментарии')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (17, 5, 2, N'имена отражают назначение и частично есть комментарии')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (18, 5, 3, N'всё отражает назначение, есть поясняющие комментарии')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (19, 6, 0, N'реализация отсутствует')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (20, 6, 1, N'полное решение')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (21, 7, 0, N'нет тестов')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (22, 7, 1, N'разработан 1 тест')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (23, 7, 2, N'разработано 2 теста')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (24, 7, 3, N'разработано 3 теста')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (25, 7, 4, N'разработано 4 теста')
GO
INSERT [dbo].[ScoreOptions] ([ScoreOptionIdId], [CriterionId], [ScoreValue], [Description]) VALUES (26, 7, 5, N'доп. балл за точность сравнения вещественных значений')
GO
SET IDENTITY_INSERT [dbo].[ScoreOptions] OFF
GO
