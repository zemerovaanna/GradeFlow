USE [GradeFlow]
GO
SET IDENTITY_INSERT [dbo].[Disciplines] ON 
GO
INSERT [dbo].[Disciplines] ([DisciplineId], [DisciplineName]) VALUES (1, N'9TYubNGzdF4a1CzU02NTEsBHp8roLij644sQZUQ9t2cNiWotunD76A==')
GO
INSERT [dbo].[Disciplines] ([DisciplineId], [DisciplineName]) VALUES (2, N'5T+xC1oDLB3OID1PG2pFb1tLUtc5ps8i0Azdt/DAM8QjxY+sOMvKOA==')
GO
INSERT [dbo].[Disciplines] ([DisciplineId], [DisciplineName]) VALUES (3, N'dmi88gJd1n/2ozSxcFciOxex7f2e5beOaI80J4O34rc0thpileiBGISlrTNe3/Rphb4J1q1ymhxwM4IIxTgfc4J2tHfmKlz7IuHp')
GO
SET IDENTITY_INSERT [dbo].[Disciplines] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 
GO
INSERT [dbo].[UserRoles] ([RoleId], [RoleName]) VALUES (1, N'3/WjfdFPYic9uodJA82nTPsL5TzjcPxBA4A24r+SZVg63/2msnN5E7pZ')
GO
INSERT [dbo].[UserRoles] ([RoleId], [RoleName]) VALUES (2, N'N1u0rlIkVmbfXNJhXISkc07oyrd4sTFAXEPjs/Jkw09kKoNYbqLwyZAM4ataRF5P/3feg7nC')
GO
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'213279f3-12c1-477b-8fa0-dca0b01882bd', N's0f00wU51KoquhjfIbu6VUwvbgeYU5cEZxftxbGdwqN6SE/+lC08xf3wpHE=', N'jiCn+Xj411Kk0k4EycXW6D1gKB1IvmDy4ZpdBo9oISX4Gy0PtJco+8Ke', N'N7HexTxb7IWmkKVwaUUkxcRIdLRS4BPSoHZ1q0TvPG9AWxv2sYpWD28mkErNKoV1hi/0rf4y', N'vewBiMMobub7oe1pfgVagfKXYEkXAh2nm8I4d2uOcb9pT/Vj4aK4io+U03vYFPHGj54v', N'JAfrNSBJWbUcSwMwm3LRo+XECnLTLlKKnyY/LO0YvxBBbnhI8n/66vHJM18MlAtZ', 2, 1)
GO
SET IDENTITY_INSERT [dbo].[Teachers] ON 
GO
INSERT [dbo].[Teachers] ([TeacherId], [UserId], [Code]) VALUES (1, N'213279f3-12c1-477b-8fa0-dca0b01882bd', N'RdqdgTYgcQnUfY0nBx8RUT1dOTVx5BcYSgw+l9Dt5lk=')
GO
SET IDENTITY_INSERT [dbo].[Teachers] OFF
GO
INSERT [dbo].[Exams] ([ExamId], [ExamName], [OpenDate], [OpenTime], [DisciplineId], [OwnerTeacherId]) VALUES (N'5207b3b5-c78a-4888-890f-e931308fb5bf', N'kLJNyII28xzqp6E6ihqc/pbkaj+etTL4aY15tCzVvqS5LMeoRGsDRSbnXze9CHNC', CAST(N'2025-06-25' AS Date), CAST(N'10:00:00' AS Time), 1, 1)
GO
SET IDENTITY_INSERT [dbo].[TopicsDisciplines] ON 
GO
INSERT [dbo].[TopicsDisciplines] ([TopicDisciplinesId], [DisciplineId], [TopicName]) VALUES (1, 1, N'zAJg4uzzWb7+j8HYOnk9ipiWh5NmpmhDI6cKSMD/I45CJrzqScr5M0d2nYlxqxwf2LngJLB4eY2qr4NgI8An41OZCu0ZQ4LSbuDEWw3hwupW')
GO
INSERT [dbo].[TopicsDisciplines] ([TopicDisciplinesId], [DisciplineId], [TopicName]) VALUES (2, 1, N'9RlV7rscjNuOAk7dOmP51n9pV+6/lbB/KwWDpuv12iTJ6RNmLLjQQsl5C+udBi5vQWLKvJzqBt7xEcQztLYLc+ZBVr0tXc48H24AwkwL2+c=')
GO
INSERT [dbo].[TopicsDisciplines] ([TopicDisciplinesId], [DisciplineId], [TopicName]) VALUES (3, 1, N'LHnyjvE9+ZvubL5M4GvjUDYzyErwuyF9b8psnW4PnX4pWn802OvPq7YS4si3i9J8CqEnJLiNu7aKXR/LCsAB7Yb8EnwRjYmT93m6oFmof9EjgFfXOJq2MhjJuyiPfFwYs9BFUUNRjx+bI66ahQL0R2NwCCk=')
GO
INSERT [dbo].[TopicsDisciplines] ([TopicDisciplinesId], [DisciplineId], [TopicName]) VALUES (4, 1, N'CsgT6aL0pB/9pFI+85gLQ6ofJAetDCqmtX4twj2XLTKeeuYdn+BpR7YPFQtO9zAgAggJ1/QhzXFNKwQ+2Pi08vtvG3QALy+TGQ==')
GO
INSERT [dbo].[TopicsDisciplines] ([TopicDisciplinesId], [DisciplineId], [TopicName]) VALUES (5, 1, N'OjzgiyDjzFKMmBBUXT9mmfdflQZf5juPw9fWaS0pM3v/HdkOl06s0Q6ETcxg9i6JoPjHSKeGdfGnJlT26wNsiRj1biK4858fZlYyiym3jhPbwYQAQXO9i0GAQBqU/UOL93HfyA==')
GO
INSERT [dbo].[TopicsDisciplines] ([TopicDisciplinesId], [DisciplineId], [TopicName]) VALUES (6, 1, N'TlUhDptK7+AksWAZDOAPBjbDiraVctMw1j3Co1l/faUcKhzRJK8eqOrUL3OMo7eiwQZ4UXg0Q0y5TAiY8xQeJzEB')
GO
INSERT [dbo].[TopicsDisciplines] ([TopicDisciplinesId], [DisciplineId], [TopicName]) VALUES (7, 1, N'YB4FoGPaDoRXYvH3e0Vlddpj3lfw8tZYtA1FchNIIRGo/qK+2H3RS1FzVIkLTimYyssU6G6dfePcqR3IeD7Ap6hTPEMd/rx+fox0twItVQ==')
GO
SET IDENTITY_INSERT [dbo].[TopicsDisciplines] OFF
GO
SET IDENTITY_INSERT [dbo].[ExamTests] ON 
GO
INSERT [dbo].[ExamTests] ([ExamTestId], [ExamId], [TimeToComplete], [TotalPoints]) VALUES (1, N'5207b3b5-c78a-4888-890f-e931308fb5bf', 60, 0)
GO
SET IDENTITY_INSERT [dbo].[ExamTests] OFF
GO
SET IDENTITY_INSERT [dbo].[Groups] ON 
GO
INSERT [dbo].[Groups] ([GroupId], [CourseNumber], [GroupNumber]) VALUES (1, 2, 1)
GO
INSERT [dbo].[Groups] ([GroupId], [CourseNumber], [GroupNumber]) VALUES (2, 2, 2)
GO
INSERT [dbo].[Groups] ([GroupId], [CourseNumber], [GroupNumber]) VALUES (3, 2, 3)
GO
INSERT [dbo].[Groups] ([GroupId], [CourseNumber], [GroupNumber]) VALUES (4, 2, 4)
GO
INSERT [dbo].[Groups] ([GroupId], [CourseNumber], [GroupNumber]) VALUES (5, 3, 1)
GO
INSERT [dbo].[Groups] ([GroupId], [CourseNumber], [GroupNumber]) VALUES (6, 3, 2)
GO
INSERT [dbo].[Groups] ([GroupId], [CourseNumber], [GroupNumber]) VALUES (7, 3, 3)
GO
INSERT [dbo].[Groups] ([GroupId], [CourseNumber], [GroupNumber]) VALUES (8, 3, 4)
GO
SET IDENTITY_INSERT [dbo].[Groups] OFF
GO
SET IDENTITY_INSERT [dbo].[GroupsExams] ON 
GO
INSERT [dbo].[GroupsExams] ([GroupExamId], [GroupId], [ExamId]) VALUES (1, 1, N'5207b3b5-c78a-4888-890f-e931308fb5bf')
GO
SET IDENTITY_INSERT [dbo].[GroupsExams] OFF
GO
SET IDENTITY_INSERT [dbo].[QuestionTypes] ON 
GO
INSERT [dbo].[QuestionTypes] ([QuestionTypeId], [QuestionTypeName], [QuestionTypeDescription]) VALUES (1, N'mND7iEFYJpjYdtN4yCvmFeuqJN8auVgkD6XITTC2HbYz9OJuAvKcJNBE4SerTpKYOjQ+Jz15q7hB', NULL)
GO
INSERT [dbo].[QuestionTypes] ([QuestionTypeId], [QuestionTypeName], [QuestionTypeDescription]) VALUES (2, N'UaPe6kjbuqj0hfbkBYNOfUzkvGSfKUQ15VzaGz8PLnJulWeJWAm+6qkP17klbFL6L86cG5kDtN7UPdR7rf9bo0LkDQ==', NULL)
GO
SET IDENTITY_INSERT [dbo].[QuestionTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[Questions] ON 
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (1, 1, 1, 1, N'khpKNSc1ljlwr/ecGdUzZoJiW1BKFwsCNNk8FAz0lQd7tSS/R+e5oViEgrB7QH//wAQTmkuDQFjjDDSCRKZLXszZV6uMu6JCvQpw/QdIklTXZf9H++VnClpPbjWvg8E8Jxsmf855Lg==', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (2, 1, 1, 1, N'wFv+USiBn/j61M6MeoioPI/++ok49tq93D7cN/NqvpAwBGOHoueaDeVh95FOsrkJaYpvwnCHyvaalVxaudIfGfgWeUwOeXN7XosGOoypRc4JLWbZnoSw9uRRcOh2BFjj6lp1zw==', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (3, 1, 1, 1, N'XB8G7HOE9CdOu+r5cI2lZe5HkZp666ebFT4ssx/A6jFLQ54VUTCylkz93YEgm0Lf7qEyPjEssHa9Iu89Br5P1PH3aoa6dfy1iCPleWPKIiPQcxvq', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (4, 1, 2, 1, N'pRlgEYXCs7ZpCE1vDz3cIhEQaSIHRJykZM9hp4sifel+bhaFl06+kz9dvo9rDlM12ADHSCvlOjTnqMXrpIwY', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (5, 1, 2, 1, N'xkmfduFrrs2VXhbTq212tWEKsbMkIr0tlgPigRQRwH1TUl8QkALyrEybUBLcftafddYmYnbwLbvFU9q3iSUnDCQn73SIp0jHogGuRIkJF1ptkMHSkwfddCdjGlijp+MaPhECbV+ePRGbA+o=', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (6, 1, 2, 1, N'C3EDOHtQSWhw9EFWAj6OOt52h4s4Qzap4GNTvw1QPNd4/qe7CCqou4gtKa1oZkwOtWGW0dGLp2G4CU5WEGkXljMo5v9Q+77zLxKfFL6LGw+GTJ7IGACWXjseidE/xr8=', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (7, 1, 3, 1, N'xsVOvGhS3+jambCeKysSX4J470lXs6NR3VUQNhdhbwWrOnG3I5+NfKgAv2iMiOVaVCBrr3QC0/fGFebZhVdfC3nAYoYaxHrewURG1E8ENu2EhmNLjt4SPBW3nOMpSiY+/ic0kqtI8O8osKfzOpLPru659J6LuydOrErvhAzXq2Q=', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (8, 1, 3, 1, N'QZ7oHCY0WnFqc3MkJSfbAYxIPzqAJBwOKXHpxNMkw0gQppdw033EPnqfPBlWZhvPknTXp6UHmTUIEt6xLuYfrLFgHoT0ZfP8keoJK5qSDNR7gtrY0g/cSsj0RI/mEc26UOFvK/TjrSSADrHhGSr6RVIITihOQg==', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (9, 1, 3, 1, N'X8Ghe9pQmIY6FhtVezrXd9XXycLWfPPbORoJgiXLyzBe6VTLP6WJoRlV+JpcqSuzaXwvw9FFcOkzwYynIL6JiWVOfHwNCGBTJDgHB0ZypX8Fl+BbfAjRyQfVxTK4F/djEG54', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (10, 1, 4, 1, N'TONPx4XyiecEnmrC9+WXkWQ0HZYlO/mybfQpoLKK8N/8hClkVjGNZUhzPHGRP6h9okG28QjvEwbqBzzWv369/EZsQ9ZLmFrKegtUetGCrEu7Qha9wLd+qL0qnb95cUDlGxJ8KqzA6MHzRG6QHz07uGzU2IdbBCZdmGWs+RPYoS2bK7bDteqGsQ==', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (11, 1, 4, 1, N'nY41EP+3MOaAYmVGn2oWahYieGu8FwqvgSVeyqL0qKGRA23LUXRUy9LeWwaQ+3sOEXIcWjbpNJd4o7D7GYLlzZ0LzWTb+OSnzcjNrXIfWjzX5sMqfL+N1M3lqoaoVqJCFoVt600uy9Y6IuTU', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (12, 1, 4, 1, N'QoPwoN9Q0Fi4UkTa4h+tJEZEyHpWc/62oNIVPB2rnuceYLr+HU98nxHDSHrvrxlmRb+20CYQtPBshMTQ5061hq+/qg==', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (13, 1, 5, 1, N'UkOR3JGSVUiqoDglZeGMzF3aMRqF5JtiLqZAbxMcTBzczndHwNMb4Q112VTNKtT4jPLw38JcHJTH5fVXpgjoj8stuxZmjnuA/6Jh3iK95mUlPPx2KmbeKvL8QKs+SL7v8FDyCndmKeBTqrfjIA==', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (14, 1, 5, 1, N'9g3P3wDr6nu+N9iLqKY7KSCOt7ScBimv7GXAkngfGd5qbPutfjTdkomVY72WOrc3TxNEWGrCPxnH0ueLsXcz6gMHtd+Ub5SM9WJKv4m9V6fX3ge486Z1D1boVUWVhlanOln8tOyTZ+SdPEscNg2A50jp', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (15, 1, 5, 1, N'xHmXIjag/bpG1H46oEk9ebzhChzyDOLhO05FGJ8nya8dReeKo5CQvKVEufqRuem7D1lM022KufYN/1q3EN+YUcrpKjU/y9XN9m3tB1occuZ06CDeqazaFrQ=', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (16, 1, 6, 1, N'yypFQLBlxA9HfkCwJXedwzufUug6cF8+/VtsO1uKsnUbVfdr/wz+grfl5BOLMAMUL9mGDHQPU+epzz23Y5W8N+eR5PQtPvbwSGgZWwNGXzFK6I2chJ+2jU+s5jsMB6oCADS33g==', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (17, 1, 6, 1, N'fO9h/y3lK4IdCISGTo+W2LLTiVS2sCz0j0rf3cipueLyabhoAQQLFsiD2z2r9RRUKkdC/dUMdMSIZPv/vpJWK6Pk3SYxXHWSbkO7TF9L8dBh6CZOcbS95NFsYOCicuvRoZwI90+m+Na+nik=', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (18, 1, 6, 1, N'lZ3w2kiskSlNpaMOTB0a4PYqD64tVFVuZ3vKtV9lOsjXYTQfI2GkiLpF64qSfAWjvTYAvW+RxD8250LCNwxTBlGCYd7jAhQqldOSnA==', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (19, 1, 7, 1, N'4gr0mRx6dE4qtteo45yBij7ZDbGC3zpRk3SuoXZ+5Us2lOEHd+pRHwD+0dfM/nAxKJWINgQXvyLkrMzWwg1aG7wtB6EwT4Kd/wtW8hOu92G11lyG3p+7qA==', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (20, 1, 7, 1, N'AiKHoY+N67qb8J/VsH68VMA+4LaHAMO5+wjDh5ZEbcTKt+vFH20uT8P8pOWtqdpTimD46tgVFG18tX9z4Hd0/GjwzanrzCR7n9eH', NULL, NULL, 1)
GO
INSERT [dbo].[Questions] ([QuestionId], [ExamTestId], [TopicId], [QuestionTypeId], [QuestionText], [FileName], [FileData], [IsSelected]) VALUES (21, 1, 7, 1, N'N2e5kGc363ZosITBwXaaUdpKqtWzaskhZD0k7MPibpYnZrzI4tRCFOBk/hxNnQvohxJPi78ULWRO2f1Jk3EoEgiWbPFv7pdlPzJr0zjSdgGuqpzH2nOvi12rzZ1UyG1m04K3', NULL, NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[Questions] OFF
GO
SET IDENTITY_INSERT [dbo].[QuestionAnswers] ON 
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (1, 1, 0, N'xsWCNMe06ffSgvFFAZUKI4skQmtnEKmvLbywkwdc56H1J3gjiGPGz/ZZjXKq+cPMvFWcjQMcNElYxWMqrMYMX6gvsDA1DWefYTGXV9smqQ1FJJnAOe43bygI', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (2, 1, 1, N'/dqZ2FpX1diWRKA6bt3rybz8iq6IZJXZypoG1BugSN3vju/YozZ8H98x1yV7sK5TTDRmI9NMSZIg5t3aMuE4zExPqxPoiQrtEVPulBH1CZ1MZonkWbD5pHRCEi/t/vVZroW0H5m4K8uD0zlsug33g0/TpcqVMei6ug==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (3, 1, 0, N'GtfsbDviRJB2lNZgIqm0ate9ESGmv1jARtdHxZTzLUAnn7MLSCN22rNtNr4sat1gMg2s1n4Z1hR+e76yhxSedXCxvvi5G0SFoY0NGtervWRbZF6XPzgyWblvY3qnD8QioGmtLFvg5bNIRLhxS/vc5/YzNFYWpkdsdm83/g==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (4, 1, 0, N'8k/6++i/sbipfK4iGllp+wMtEHVgIAeGN9VSLgjOn5+DQkGrcx3z7lfbiziHtG2Es6lblB5bb5dITIhf5Rywo6c5BEDfjIpn2UlDNEjOTw==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (5, 2, 0, N'Zmv44SAl3S9yvW/K8QBhREWzJaoIWCf87AUj1Xfywg==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (6, 2, 0, N'A2pqXyK3cfQMEdV8VEXAm6t0mqZocd/qo6hASavgp1Q=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (7, 2, 1, N'q3Llj4jqzaIOL8w925u3j+pB+w77fO6stcs85qHRMd7kDWpJsg==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (8, 2, 0, N'dtu3hQE1qpa+Joda1YOoYchcdS1Z0hkU7eperFMAwe7rdA==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (9, 3, 0, N'QWHJvXcDKr476ju2YaakWGXSzQhjGNCBMwOqQFpUvpB9z5fEz9Q/pz/0jl6HSgZ6rzAoZeeLIjmNMXMBk+h8vkzaxsy6bDXBBJh/eE4DNf88nTKuV77QFtIn', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (10, 3, 1, N'MJ1pmrRxmRuqFAVhWqxLs7Puz3R1Hh6yI+Zkat/I296OGCRoE7YU5PTI4nlRG1SG2nP80zdzUb3W0erCo81pcdzQq25exYcHzheo84yZAhCQBd+MP2srLde59nQbzBhiBD5JoGLfuAvo7pN2a5FC9jy0bJsq9ikN', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (11, 3, 0, N'q7aC4azUviPywtOKkHLQkdJCaUK6Ve04QfewKn9FfGfc+LVs3V8ZugWsnQHCiKpqp8Ib16T+KtxtVE6mzfReNN6ka4e0', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (12, 3, 0, N'82BtkQtxwyCSQ1iJGGDmO2Z1fs/bbvc+Qvm0At0HwKf8mfbzuGPPpUG9hLuhpIVsFph+EWbnl8CnA4aYow65Pw0=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (13, 4, 0, N'jy5Swpp2Eo4HZf6YZU8a8w9/Oa2Dg6VZrkrwDaKY5TP+2dY4OD08JMGvsllLZTKqIprYGMu8GxPKNJx2zu+XXUl/CFbZEg==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (14, 4, 1, N'3s41s0j3SDIHs14pwDMWJR+17Ej7VH4L6oF6nPWD3D9BxLJN0aDkIAypHTuyk/io2Cne5zEVwbmJ1hKtdluXNEMzSZfcD7OPX8NnIh+iY6LmABCrZSSkZh9jDTTtxePWx0DwBWwiqy5zWvMyqJVI', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (15, 4, 0, N'H2D3wp68egND8avHNN01QT+GKFGlOQaKRtNm9WLzSiKt/goDCkFGLffawqE1p9BfhpWv2fdQ/A==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (16, 4, 0, N'eQm4ZCMHRbMCBsmRXAmN2LRmM51foxCf0ermw2ZU2Bgx0yKCypKjnCYbo4K40qUoyl//NC3n', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (17, 5, 0, N'EsHQu4ggzqGHLQjF7nHkEipfSn0vBMw/XMwJbdVu', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (18, 5, 1, N'kFN2wN9prsh43rD1imvG6hYCZNVkiXH0TQWPaSiW0ieyYg==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (19, 5, 0, N'Y+28S0NXTEBvw6eCVF5hjNo6/UJz5OxKpm+65hoJGw==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (20, 5, 0, N'8DfOXmaTAP8Fm9XgdwA6+8TCgk568Q7X4g9QhIPCoZ0=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (21, 6, 1, N'6I/pejKH3AMQilpJoXFn/Cl7EONet5zoP1/QV4nxntBb5aAhLXRPTnp4Fp+lC6uSVad67dwmJZfnFPFP2ZVQIR+b9Tpmyhn+9BhdeXEchJjxNataM2f/or7dqXPycMvMGKl+Ghts9KCDZegYpqcUObtRl3lUSorVPCe8', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (22, 6, 0, N'rJ75M1k0cn/IvROVnSh2nCr/STU0ZrN7WX7yLy+9FWIHUXlidSq6RVEo/ALColgGtaXiQ/tlu4pmkVK9KkI=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (23, 6, 0, N'GA8dZMmJxCWcfpGGUT5PoQzQUBCJAtKldnTNW8dnugNWfN7m12YbBuuUj1T5ImSbduNWX21QYWlbGSCi27JmfZCau6X+2w==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (24, 6, 0, N'7JuxO2FyFXhjHD+AncM4e3EyNRGQsCWKVZpLXDBxY/GuGqkS3lOjpKqzZWekeHBr', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (25, 7, 0, N'uRuK48qbUJ7eXmyyyOY6H5Anzm2z2RekUhwBtQQOkbe7qx6WWRgNFziEaDo1Yp7CV9UP4aIzKMcXyZMVY7lAHKtVkIJd4xVKrEi2EkDF9lXSgZ7pmiu81XWarA4Ga6WLNUTix6k8YTMXLVtirQ==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (26, 7, 1, N'oa3lphS16zV23moDYYO3ulQ7qRIkDws+Y49TC+MtNpgx5Q470gNb019SiiW7Ud1rDrCz2f1vHrTYEBJkL3EbcDP9TM2PS3Sj5yuGsA==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (27, 7, 0, N'f8rKtm6c0if8U4kA2ZvJoQZRPsSevnNB28k7o+cyyH0yt/hToJvXNRrsfOsPrezjcobPNCmc0XgkmjhNW7l7s+kwWWGLtHLEKlAEv6BW9L39Q3CCR+9Oc0WC', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (28, 7, 0, N'vRlCDU1bopChQMjQg7n9mfufYGJCqEDWkrVRXmgXx7G8ql/WoDQUWMHLlK0cW+UlrhEmAoeW7gRjZ8BV3+Rrv3R1YKbrDM9sXmI=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (29, 8, 0, N'A+hKyd3ZiDod1aqmDb8sl1cGNxWO/QfQ4MDSsbveOzQSjWDy0w9qhiBeelG5M1l9Rml79nC5wdA5CGDaADWRYd0Nfdp2iaHMgSDgeyc6aCv3n64=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (30, 8, 1, N'rR3ChyuMgifHRXLzc274ylQMOFmeTMb30OnkrAx3iEW5zMpMv6TaeimP8ZQ/mfV9DCh0mqR1KjK4mpTaKupyi53vFbZDRbrfR8z+eemeV74c5ZELc6syIE40Cxnous6o7PVnggo5aIjqDdHk4ItXUkuPM+jzBXvvORY=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (31, 8, 0, N'FPZG6U3JnqXyrixLTaAXLKgnnd6kPyKjRrvhNUAdeUViOY4RcyOPRfinqLlkOnJZTTydvU/XvFOXyi44j0iI7duFPBzKZQCteAlW5aFtPQ==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (32, 8, 0, N'TtpwjpAOGegAyXpSAb/6/f79kQiDfyl4f+rZQftHZ5mbKU2efW6zSaqUwAQNaIgau8HCg4CHhp6ccNjIaBVo70ShqbypVQ==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (33, 9, 0, N'TVmNR9FF1b2mKttM8x2mZXz+EDW30JVt10wMNlRMkrI=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (34, 9, 1, N'z7UvQJeu3aQLqt/NuaiHdnLUfmSOJp091MH00rOP', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (35, 9, 0, N'+EjjUFnDBMfPmvT7dGyJ/ce1hdUP2PTZkQhLC0GKYldgQw==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (36, 9, 0, N'wqYk5vHBqAHgII6bbEweDUTpmomNEWsRqkPO41MMrP68wg==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (37, 10, 0, N'9Deqvwbx/zxIp2U36dmSKMgpuHyyMJ+uj+1YJzbp9k67', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (38, 10, 0, N'VHYF7EuHJXvsIq+CA0XMdm2nRRJNp/2BNzVrSn/r8SDwpQ==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (39, 10, 1, N'groGYsQHnezcFh4qxEFAcYhOZHm2OmGLcPIYWDrTnA==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (40, 10, 0, N'VekdlWFOshLEDPQp2ZH+4Lka1nz1Qg7R+7Pdh5x+HGg=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (41, 11, 0, N'kXXeHxJxMKE9VNP/s5upFBrTaEvlmmmeNquI/SI=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (42, 11, 0, N'qJ3mhBof3Z2KBul5ezj1Cku4IALm7MujwRRhFdgZlQXjEmwRCbj0Trz3', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (43, 11, 1, N'krYQ7pSQTgzPWwpLFVoza0KjcWvmlExZqd+BKcYtVKXxcZ3sslk=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (44, 11, 0, N'WN6TaVDTwSx5QDIKjc5jww6qpCapq0a3cwJV2uyZTibtTTt/gsDC2A==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (45, 12, 1, N'cQPKv2A6yhXihYDhfFlJrPuEQ3hcwYisBMdSo9ot02e+Gbo8MwvsmS/HZL8Ku3DcHm2ThwYYaVS08LnG95VHityciqOSk2Am4sGZ6kD2', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (46, 12, 0, N'kQxAeA6WwMEIbWetLRvTi4PclyJNjeIudwHrVNieAfHtrc2NFtGxCGaGTDYC29sB/CwcPK37l4KbZg==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (47, 12, 0, N'X3fJVGb4e3uAZgL42AGKGPtdUqrpmxYvVe54MVGYbPk2p9Qd+4iW90i6ezWBvOd9OAFkgfkUG0W3Sg==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (48, 12, 0, N'bYrMSs8avU6rKmF+5xmHodxUE1hlGNQbYYQLs3ScyQRO8f/iiZxGyFklqvy4jrvIziy3nTAOBrS37H7y0RAiaXAx+BkaxJOy3R+ldFPcIQ==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (49, 13, 1, N'8X2DALsY250W1saIOjkUW7fP9JGKqyrUWrMMjGWzvHsYxpQ=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (50, 13, 0, N'95W4J5PqpGsO/es2FfxXb/FFSdtfoQErWPKW6/CX1A==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (51, 13, 0, N'eLME24i1HNpdgcDAga2wy/wShSb56gGb2r/CduM=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (52, 13, 0, N'N9SjHiGPy4SBW3SZPzCipn2Zi+R5nHRWDHvCHd4=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (53, 14, 1, N'RvdF+YptH63yYUkCPCb3ro8nDs29SUUc0ikFk2/l5H+snIxuPK9A4F3BGCDVAQD6CeUGiDk=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (54, 14, 0, N'6LPRDtOIKMw5WS44w0SmBrNBEjHYXNbsdduUXSE0rET+adPBN9mphTo+uMU=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (55, 14, 0, N'3g5gz3mtWHPgIBdGS3JFkGr50mBNahIoTo0wRAE8Ux3tK6rfyQ==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (56, 14, 0, N'T+nGu0lUk5XcW3EiPrmOJqFZLZ/s8hOm9uWOPQBWLhcGaKigp/9tKv/q', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (57, 15, 0, N'zrsbKRyQawhC9rrgbZi35KY819pJ7a+boUUlABIDBX7gwIT+vOBsi03cE7hBGDp7lWH4qW1sgoCrBCZPqwegPZk1Hw==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (58, 15, 1, N'O7LoKwCZw+c916HtUbbHm9tml8vGBFeOio2KiJqvltLYAekD2/klbW6j3aesvxjFk3SexQdDrB2FC8RqiXdIZ8D3icL/gh1Q6qM96e0kppz3Cyok0phIxWCEx39wKnS94QC60NsLAPZPED2Ndw==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (59, 15, 0, N'giRdwz4LGCKuOG+3BuxCypkJNgU7v6eh1Dzhi10kXApZq8Gd8nMPsP1TMK4QsZmiGTeDjea2BgY=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (60, 15, 0, N'OXdH0h8xG1KyfNiY3Lar2xpJzWXW+RqJLidMceapVQqEThaPHz7Le7+coz9G8YOt8Q==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (61, 16, 1, N'e1okFst/UUhnUZ314byXvsQ293ByMAs7mHGAuM+ooFHMcFpS2/0+3uv7wJHo4sS6XxYT', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (62, 16, 0, N's7VylukU2zeUux67lbs3io5RpVRxUGd28Sfb9HZyYF91rDepfEkdLJMFeQ==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (63, 16, 0, N'ttHMvCoF11YFd989wTQwF4GP8uvS656AxP4Ye7jgvYKOvHSYUK+gJ3sUtWHQ4JUEtViorTU=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (64, 16, 0, N'ds1Bit+WbffDDbFV5WOY5dl51gsCcmYkfz9ioTOw4pE8fTbYu7x//4Ro9LIqGw==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (65, 17, 0, N'lYg4Ug/XzZXCD97727NVqdDFRbkwcHSiJV4PRLwtKPQzILUxdLn0mwFANnnxObrUChEWRfVfoePf66I+WDiLFw==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (66, 17, 1, N'Vs+uvNv35+1CYiSVfKjZ4f7GJgQ+RfvZkG+HL5x3No7CBit+fGQHNVJYGAy0p8frkWbTI/r/nRErSEG17gxS0XR7jck=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (67, 17, 0, N'YKf16Kp5pIH3PRAaL96FaI8IBIDmku0VBNIonXbUbdJhUe9og/tX+30qA4G+Sa1r3ulv8r+pGAMWOTt3e3pZBf08', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (68, 17, 0, N'SoUIX29THjtR2wcB6yd+H9QGqmd7uLJjHs2sYrAYTMs+b/lvmAj94d7uzwmZS+yP13pm1LuqCBGlzsWubNrh/A==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (69, 18, 0, N'tDTFPN/ClsBf+bf8IrJkK2YTGzfwWqwOaluh2uoyyH1STwCmqg==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (70, 18, 1, N'EMrewktYPBGwQodBdXJxHRcCml26o1sHaFEBeJZq1bjmshSECN8=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (71, 18, 0, N'WjOJL47IZ/+uFAeJSqtNnxJ0hwmsT3ZN6ncSzaKAs3McIHUV', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (72, 18, 0, N'iNtiFaVEF8m4L35JL3XhfuB+VfsGmqln5RnlvxJ4uDK5C/wi', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (73, 19, 1, N'yhfJuwfPC1A+kziAV3FisYH3UKQF+hy1nsAmZb/0126RfZjtT0KOei7cDYXRyZoMntm+t9+L', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (74, 19, 0, N'fXlF53wGRZf73rvJc1Mbz/FKij/JQgUYW/nKnrYKRDC+eAksKWrKRykOPiPGxw8IxidkuWXAUPg=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (75, 19, 0, N'qRLzyutK3AYtHLKMw4SgwrjHIw2DfmbVjsCUQ8Bn1W/ULNF4b0oDWzhvBGc=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (76, 19, 0, N'uSvDN+DGV9TE/mwCHnUSTN+G5Dzd/aan7lCSNELAMHq2BvXgTo6zyRhpsS/0', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (77, 20, 1, N'AuSCmOQD/BEGxT/icQ3LYpQVMDtQ0rYpkPTwZwKmV6+Jh2Pzgd8QIiibGPcQdUIImA4Iu3voSp+jia4iR+aLR+GF9mWujeYw4xaIhV+0FGs=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (78, 20, 0, N'g14F7uuZyjzVStrr+yTgSMaRwzXwCjRVAcDSa1Q1k4t0mZuYKWBwJAqWEqsNtnmf1V6U3f2nFaMOgmjGiUai95et', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (79, 20, 0, N'USG117euszsaKN3VKrTYBxdfgfQECuHpwzYFYnHKSRpspNDx7aKN05tkzyw+/6iUI2vHwzYi1L4q8cdhC4Fny/81/Q==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (80, 20, 0, N'+NPJyxYIMRV8s7ajzW5C5vgHPC4B4xJ2OaHsF9T6P+4t9Li8VckYbmMeD8lofliQEAndeELTt778HP2oSxUdAQ==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (81, 21, 1, N'pggQz5bIXe6EcTImZRrxBb5/kImFimG2xrD/zrqhw+LMf+Si/oj9G3fQ3l+R0WX4gc+SPdOGTBtP5G2vbBqUWlb6s9aF/kEEE3yByzA=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (82, 21, 0, N'woMuDgy1HCokE+m8yjppq7GRBhuFx7TpZPgiOhpQO93N1IuA2J58mhlhURmJu+ftw+aJiVxM', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (83, 21, 0, N'aicBYGMLpVeu+C021bRwf0SJSgoUZy7hXwYkZsMzKFQ3UWMdSZ02Ogat22I5Q76QFwiBVMGINHeEYXCbD7E=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (84, 21, 0, N'Wwkg2ig9agB3+1VFK+2YnIE4KmSlJY/1ailOboxRTRBFOsBQg7fF3LuMx+HWJCYI9kCSy1F2oiRqX1UXpC4=', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[QuestionAnswers] OFF
GO
