USE [master]
GO
/****** Object:  Database [GradeFlow]    Script Date: 21.06.2025 17:06:10 ******/
CREATE DATABASE [GradeFlow]
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GradeFlow].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GradeFlow] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GradeFlow] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GradeFlow] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GradeFlow] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GradeFlow] SET ARITHABORT OFF 
GO
ALTER DATABASE [GradeFlow] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GradeFlow] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GradeFlow] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GradeFlow] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GradeFlow] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GradeFlow] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GradeFlow] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GradeFlow] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GradeFlow] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GradeFlow] SET  ENABLE_BROKER 
GO
ALTER DATABASE [GradeFlow] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GradeFlow] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GradeFlow] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GradeFlow] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GradeFlow] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GradeFlow] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [GradeFlow] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GradeFlow] SET RECOVERY FULL 
GO
ALTER DATABASE [GradeFlow] SET  MULTI_USER 
GO
ALTER DATABASE [GradeFlow] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GradeFlow] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GradeFlow] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GradeFlow] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [GradeFlow] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [GradeFlow] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'GradeFlow', N'ON'
GO
ALTER DATABASE [GradeFlow] SET QUERY_STORE = ON
GO
ALTER DATABASE [GradeFlow] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [GradeFlow]
GO
/****** Object:  Table [dbo].[Criteria]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Criteria](
	[CriterionId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NULL,
	[CriterionNumber] [int] NOT NULL,
	[CriterionTitle] [nvarchar](255) NOT NULL,
	[MaxScore] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CriterionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Disciplines]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Disciplines](
	[DisciplineId] [int] IDENTITY(1,1) NOT NULL,
	[DisciplineName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Disciplines] PRIMARY KEY CLUSTERED 
(
	[DisciplineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExamPractices]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExamPractices](
	[ExamPracticeId] [int] IDENTITY(1,1) NOT NULL,
	[ExamId] [uniqueidentifier] NOT NULL,
	[PracticeTimeToComplete] [int] NOT NULL,
 CONSTRAINT [PK_ExamPractices] PRIMARY KEY CLUSTERED 
(
	[ExamPracticeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Exams]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exams](
	[ExamId] [uniqueidentifier] NOT NULL,
	[ExamName] [nvarchar](max) NOT NULL,
	[OpenDate] [date] NULL,
	[OpenTime] [time](0) NULL,
	[DisciplineId] [int] NOT NULL,
	[OwnerTeacherId] [int] NOT NULL,
 CONSTRAINT [PK_Exams] PRIMARY KEY CLUSTERED 
(
	[ExamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExamTests]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExamTests](
	[ExamTestId] [int] IDENTITY(1,1) NOT NULL,
	[ExamId] [uniqueidentifier] NOT NULL,
	[TimeToComplete] [int] NOT NULL,
	[TotalPoints] [tinyint] NOT NULL,
 CONSTRAINT [PK_ExamTests] PRIMARY KEY CLUSTERED 
(
	[ExamTestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[CourseNumber] [tinyint] NOT NULL,
	[GroupNumber] [tinyint] NOT NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupsExams]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupsExams](
	[GroupExamId] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NOT NULL,
	[ExamId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GroupsExams] PRIMARY KEY CLUSTERED 
(
	[GroupExamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modules]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modules](
	[ModuleId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QualificationExamScores]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QualificationExamScores](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Score] [tinyint] NULL,
	[Number] [tinyint] NULL,
 CONSTRAINT [PK_QualificationExamScores] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuestionAnswers]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuestionAnswers](
	[QuestionAnswerId] [int] IDENTITY(1,1) NOT NULL,
	[QuestionId] [int] NOT NULL,
	[IsCorrect] [bit] NOT NULL,
	[QuestionAnswerText] [nvarchar](max) NULL,
	[FileName] [nvarchar](max) NULL,
	[FileData] [varbinary](max) NULL,
 CONSTRAINT [PK_QuestionAnswers] PRIMARY KEY CLUSTERED 
(
	[QuestionAnswerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questions]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questions](
	[QuestionId] [int] IDENTITY(1,1) NOT NULL,
	[ExamTestId] [int] NOT NULL,
	[TopicId] [int] NOT NULL,
	[QuestionTypeId] [int] NOT NULL,
	[QuestionText] [nvarchar](max) NOT NULL,
	[FileName] [nvarchar](max) NULL,
	[FileData] [varbinary](max) NULL,
	[IsSelected] [bit] NOT NULL,
 CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED 
(
	[QuestionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuestionTypes]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuestionTypes](
	[QuestionTypeId] [int] IDENTITY(1,1) NOT NULL,
	[QuestionTypeName] [nvarchar](max) NOT NULL,
	[QuestionTypeDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_QuestionTypes] PRIMARY KEY CLUSTERED 
(
	[QuestionTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ScoreOptions]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScoreOptions](
	[ScoreOptionIdId] [int] IDENTITY(1,1) NOT NULL,
	[CriterionId] [int] NULL,
	[ScoreValue] [int] NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ScoreOptionIdId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentAttempts]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentAttempts](
	[StudentAttemptId] [int] IDENTITY(1,1) NOT NULL,
	[ExamId] [uniqueidentifier] NOT NULL,
	[StudentId] [int] NOT NULL,
	[RemainingAttempts] [tinyint] NOT NULL,
 CONSTRAINT [PK_StudentAttempts] PRIMARY KEY CLUSTERED 
(
	[StudentAttemptId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentExamResults]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentExamResults](
	[StudentExamId] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NOT NULL,
	[ExamId] [uniqueidentifier] NOT NULL,
	[TimeEnded] [time](0) NULL,
	[DateEnded] [date] NULL,
	[TestTimeSpent] [nvarchar](max) NULL,
	[PracticeTimeSpent] [nvarchar](max) NULL,
	[MDKCode] [nvarchar](max) NULL,
	[MDKCriteria] [nvarchar](max) NULL,
	[QualCriteria] [nvarchar](max) NULL,
	[TestCriteria] [nvarchar](max) NULL,
	[TestTotalScore] [nvarchar](max) NULL,
	[PracticeTotalScore] [nvarchar](max) NULL,
	[TaskNumber] [tinyint] NULL,
	[VariantNumber] [int] NULL,
	[TestStart] [bit] NULL,
 CONSTRAINT [PK_StudentExam] PRIMARY KEY CLUSTERED 
(
	[StudentExamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[StudentId] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teachers]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teachers](
	[TeacherId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Teachers] PRIMARY KEY CLUSTERED 
(
	[TeacherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TopicsDisciplines]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TopicsDisciplines](
	[TopicDisciplinesId] [int] IDENTITY(1,1) NOT NULL,
	[DisciplineId] [int] NOT NULL,
	[TopicName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_TopicsDisciplines] PRIMARY KEY CLUSTERED 
(
	[TopicDisciplinesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TopicsExamTest]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TopicsExamTest](
	[TopicExamTestId] [int] IDENTITY(1,1) NOT NULL,
	[TopicId] [int] NOT NULL,
	[ExamTestId] [int] NOT NULL,
	[IsSelected] [bit] NOT NULL,
 CONSTRAINT [PK_TopicsExamTest] PRIMARY KEY CLUSTERED 
(
	[TopicExamTestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [uniqueidentifier] NOT NULL,
	[LastName] [nvarchar](max) NULL,
	[FirstName] [nvarchar](max) NULL,
	[MiddleName] [nvarchar](max) NULL,
	[Mail] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NULL,
	[RoleId] [int] NOT NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Variants]    Script Date: 21.06.2025 17:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Variants](
	[VariantId] [int] IDENTITY(1,1) NOT NULL,
	[VariantNumber] [tinyint] NULL,
	[VariantText] [nvarchar](max) NULL,
 CONSTRAINT [PK_Variants] PRIMARY KEY CLUSTERED 
(
	[VariantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
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
SET IDENTITY_INSERT [dbo].[ExamPractices] ON 
GO
INSERT [dbo].[ExamPractices] ([ExamPracticeId], [ExamId], [PracticeTimeToComplete]) VALUES (2, N'751c98fe-19d7-431a-9266-9716837a5d0a', 40)
GO
INSERT [dbo].[ExamPractices] ([ExamPracticeId], [ExamId], [PracticeTimeToComplete]) VALUES (3, N'5207b3b5-c78a-4888-890f-e931308fb5bf', 1)
GO
SET IDENTITY_INSERT [dbo].[ExamPractices] OFF
GO
INSERT [dbo].[Exams] ([ExamId], [ExamName], [OpenDate], [OpenTime], [DisciplineId], [OwnerTeacherId]) VALUES (N'751c98fe-19d7-431a-9266-9716837a5d0a', N'ZlHj2EYUtwFtWNLrUo3JfFsTD1cuEnUKzM2ZWtnCr9QJJPBwlywy5w==', CAST(N'2025-06-21' AS Date), CAST(N'10:00:00' AS Time), 2, 1)
GO
INSERT [dbo].[Exams] ([ExamId], [ExamName], [OpenDate], [OpenTime], [DisciplineId], [OwnerTeacherId]) VALUES (N'5207b3b5-c78a-4888-890f-e931308fb5bf', N'8+g+rskiApHokyOl761mHetGr7i3V2Xe7YBLn9Pk9R1HEOuD14xiUQ==', CAST(N'2025-06-21' AS Date), CAST(N'10:00:00' AS Time), 1, 1)
GO
SET IDENTITY_INSERT [dbo].[ExamTests] ON 
GO
INSERT [dbo].[ExamTests] ([ExamTestId], [ExamId], [TimeToComplete], [TotalPoints]) VALUES (1, N'5207b3b5-c78a-4888-890f-e931308fb5bf', 60, 24)
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
INSERT [dbo].[GroupsExams] ([GroupExamId], [GroupId], [ExamId]) VALUES (26, 1, N'751c98fe-19d7-431a-9266-9716837a5d0a')
GO
INSERT [dbo].[GroupsExams] ([GroupExamId], [GroupId], [ExamId]) VALUES (27, 2, N'751c98fe-19d7-431a-9266-9716837a5d0a')
GO
INSERT [dbo].[GroupsExams] ([GroupExamId], [GroupId], [ExamId]) VALUES (28, 3, N'751c98fe-19d7-431a-9266-9716837a5d0a')
GO
INSERT [dbo].[GroupsExams] ([GroupExamId], [GroupId], [ExamId]) VALUES (29, 4, N'751c98fe-19d7-431a-9266-9716837a5d0a')
GO
INSERT [dbo].[GroupsExams] ([GroupExamId], [GroupId], [ExamId]) VALUES (30, 1, N'5207b3b5-c78a-4888-890f-e931308fb5bf')
GO
INSERT [dbo].[GroupsExams] ([GroupExamId], [GroupId], [ExamId]) VALUES (31, 2, N'5207b3b5-c78a-4888-890f-e931308fb5bf')
GO
INSERT [dbo].[GroupsExams] ([GroupExamId], [GroupId], [ExamId]) VALUES (32, 3, N'5207b3b5-c78a-4888-890f-e931308fb5bf')
GO
INSERT [dbo].[GroupsExams] ([GroupExamId], [GroupId], [ExamId]) VALUES (33, 4, N'5207b3b5-c78a-4888-890f-e931308fb5bf')
GO
SET IDENTITY_INSERT [dbo].[GroupsExams] OFF
GO
SET IDENTITY_INSERT [dbo].[Modules] ON 
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName]) VALUES (1, N'МДК 01.01')
GO
INSERT [dbo].[Modules] ([ModuleId], [ModuleName]) VALUES (2, N'МДК 01.02')
GO
SET IDENTITY_INSERT [dbo].[Modules] OFF
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
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (5, 2, 0, N'dwgnlrbBmhKLOrY05kkRQvUVyw3KqmEhnEn3oya2LSqlmJe9aT9J36snOjC1Mv2qhDiKTW0z36S5Qdxf0R19WDbGVDW1zeAeqH8VcRLiA12ikAg1lz3vl9hlEBit7ieS413A1oux75A0SwqKclMEvFjAGadnkfNL07nqIQ==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (6, 2, 0, N'nuW8A2OMBVysx8ixPhns/G67iu3/08IfT3BljACpe9ccQ0zySCrmagp6U28o0792u+Qx6YT8t7eCds0S8NXtzz8Yp4uRbwyb1AbFcSy1Rpl/EM/EzkYQQrmFF9FhGajcdonYlUspvbVf4c9PGO++1oQtLeI3xQh1WSAjhA==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (7, 2, 1, N'DSkKq2pRG5bi+r5sOrAk6IBJeScYkNqcbL31jjZzkrsxPjeW646WP/6hiKvHkxZUUdwVKRmrn15Q8My0hC74QFVqrxGxTj7byYmUCZ5poUte4R0CucdEZrou4Hy7liNlsS2jHeIhG35JciwziwP8y9FylUfy4Sg2i6WaIplSVqhEW0UmIQjnww==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (8, 2, 0, N'BgkrHMFesgwpx1CtLq8NXbbgkE6tWCBxKCbXvpSQqtlGe+RYSaR3uxwNC5dPX5xPF73NnPLZbUEUoMecp/ob4lvEndRNoYRnZK70yiYewlzW1kGX213pA/gTDYKdtdbE36sYwbEPjhprOTzJ37Z2ZG5L8hZyB+p0PDLDT8618oBJbs0y', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (9, 3, 0, N'/2tN49IOVg3XDiN1nrJfwf6oBZs7maeqPSmyXFAdiv0KRwOGv8BABJz0zTOiunPOGXmmp2LxuF4y1EXhPI85mJCyS1Uzq1Mp3tLMMujuqDsoZHdv4G5W104078soXtD7aL5Xjx7kEvloAJ8zNbIqXdsT6mY4h1hkf6OfwCUVYOaTuoDe84MwS6mla3oObjv48C3KqQ1EhaaBLJUCMjPvnL9/8kEqw8I54srEI3mtHp/rnIx7TySLPXrD2nxxeRVyA+CKgBTfAwrtlXqlHm4yZGqei3FhIyhhyjd9iRTstJwKv8yd', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (10, 3, 1, N'o/TQgVhUZ3FZCpwgj7sJUSmL4K1hN9B7z/1p5PS7VOuN6Ubs+wkdYZqUjUfkyBZTieOst4b6E4sd1UX26gDdrYZk7aXIBZIKWjOFGYkSXYLtQ2F6SiSSmM0Hzedo5CCXmdW5U2fifBPvbUIz3RI6eNMx0n1KoNtOo7k7SL51u7XRIBKYn7zomPBh4SHb8RrmZO3H0uTeA29aEVksBzWr9C91xhxMp+qjgO8cE//lPN80il1PLXrauufPIbo7eYDxcEzB09Is9NBVwpCCmrVxF0zAUCiHwFttC7jQtP3V0d0VvLT3GRoFTZg2iqUdcJIe9s1XFhmiw96yTLNDoI+4StBo4P3C9Z0DDJcL2JAl3/RuzqdqrHD9gg==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (11, 3, 0, N'bea8TZzjcv067U5q+gfZntDRZeS2Zw0Add41OMpJ9uGZnkYVW1bC8ate6s5RxKgUr4gO8tlDesQ8E/yyDEac2FrfD2PNNUvfKtjy83Ji3THEtiFbAVL23wPKMKijP6yx6Y0fuaep6lownkNHpVXkv4rqeMLl1HZdAC7lLIpscpCY4yv3DqaBL+5fjjlHEfkDk3Mt6oejYx97EUQJjj9sVjAPkJeD/ApTn2kvQBzyQaKv0OpXUL+xq67sE9k=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (12, 3, 0, N'yc6SAssBhScQY/gPv5DVyZnE4kPJk+yLJgaQwmzSGJtWh6jt4XEGv4iefu1N/IhlWZ8hbCbGEJdGpCWixvdVZ9Pp5xYTyLujgTEI6u8w4FcoW6qLh5PySRynEijjf1Z4Bmma1l/MbDPEvRi6ZpEX496CLmHxY/0grYdLtyfV4oe9GUTCF3n8giBFnn6Mp3uHDbyG4sjGuaWeXxgSN0nVVbK74lZBcy/QQFggsQdQSBjFdrta9wINXw==', NULL, NULL)
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
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (85, 1, 0, N'Lxd5bDHjxjZRz1gXHPbxSsSXWXcLaRz4roufV0ff3Dw4GphKzBZ3XSwGXEN+hsSMkmHJecQUuTipm2FR4x4MHzThi1wFpvB/3/ijy7m6Xg79fMeU1il+KYQWADjPK59Pk0FBBN+8ALSUmTCCltRQ6v8Gcmokq9E5CFwOEnZUp5TfObqfe/f6L3AEEt6uwJ/KMJH7ew==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (86, 1, 1, N'2g5zFlgg0+eIMePFBIF4obRXYwjUna9BdS3yRE5hvFpMZm55GEquXmSxihAs5rykuZ9zERw9DLxIFYTmqo/uIXCk3SiVIg4oQSFCmoKYoBmGrWwkgbGC1vo4BS2fWPfp+W2+PIxTXJWCd2eMntR+JToofo2bxQWbJj2ieATFSE7qbDMexM+m0rb4B7JwqiDwbxKK8NnhaFFF6ZInvRo1Rq6Iyo5qH35shXYKcztw9MbhKBYMAAn5SAwnRjbPjOR/', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (87, 1, 0, N'SSQKDYTzUWKaoUVPWlyokRHw6wHSH7M2pMFlkO2kuSj2of17kxVSF4uhMyVVPDF5++5T8p4e7H7Lu71/ZVL8cnN4jjNFk+vfjs1WlGTcxEMvvLn1oHe371xvGG6/V0DxnRIImG4cZKZZJGuO8Zu7NJHV1aMyacdVt24/yeGLlZ+S3ZfxcrwbYMY81Jjjic4XmS4vH02/iivvc+uDnl5g23wVlhsS9OPQO1RhGBRfB5Ln7I7XpE09+PSfeDjkBvd7yeY3bg==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (88, 1, 0, N'BU6MjLLvKaZEONGdKC7XoOgjqzMInCGlPIHaNvwMQPUP1zIv74PqgLGfdHiZOrmrl3UYivfAm5vWFMPbLYcurcNbgKG7nOrRAvpJVIrIFnjZx5qRMjHFHEgi/nE8DwfoiL4PGNM2JzPAzYmoUUOMIzJE5Le/HyRRC9bK9qusZ+G8F8mfRPBcMA==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (89, 1, 0, N'AV2DlCu0+hr529ay6IpBKKM+3YIqkGrpf9f62+Vd9geNkxQP1Xkf9VMQlA3AuvEpOWTz+vYdn7SGhL13AWvhO8tx6jEwt3zzZXGGumD/KnSahmENzyGzBqTv+xyirWKrwMPyJpCuHgyp8WUsItsh9AbDOhCNBSTpRKNqmd3Pgwi0L+6KnmyTdojvzm4oyYx6ujFraQ==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (90, 1, 1, N'w72Kf3hLavdYlLicWr7xnJEv/hlFgPzhJjTg5BnNGAZh9QgiHj8W3R2aYy51doyARI1uoD9KxMQWP6h40hhtYQVDkkjaqaf5UaXRlogLrgLveP/zvYZlCh0L49LQ/G6m5hEnrMZibu4myKtd4VwBonM0ihbRyujYdrw/Fb39CsaJ9PIrFxJ+B4ZkSwouo8EUB/bjsj2q4CU7bgTxL5XEGV7ux18p8omJfMlyF5x3dAWaeIfu4iELeQlRmM8JQxng', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (91, 1, 0, N'iaqA4HGiehiDm+ieiv6EniRId347ix+hU3UNWfHGSIA7d5I78YFiuJvnXjBUDuIo/wSu9TOX+MEgmIwwoUJdcp9edGbpf0AFM4sW7QZNIGQ+ZIyTKOnS5msXjSlNNk8tXsbBfN/3Ows5vyVCIbhmNkhBF7qSb8si7bWrxZzCeOfcnA+2RnBqrbfswX2dcZI2H1dd9Te/0GmtUUZOiqZHO1e2GYgP+r3QWyyHBJfbiL+hBJ3nhtj+pARDqqvZ+++9glulUw==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (92, 1, 0, N'TWZxGKRf4gKuxWeg4m3HHn0ZfHBnjB84TUCTXqo+PX6ZMKcgkVYqEozeZwrnar9oMEEYP8zP5hNqBEE2vPzjMU4rwSrk6ZOUGMUrhjfRmXqVOZe0prQ2708YeCtALYxOW//saHnhoTDs1E+aWNpZFyNrzJdV0yx1JlcHxYVNhYQfMl81tZ1CeQ==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (93, 1, 0, N'/pk/URd/AJAVM4rnYeopqBzQdn6qyz1LrM0SIZqvHzRvkwu2+1CpuBB/G8Yv0zRb8YmBgbjCAyTN4gD/A+rCBuZlK4+AEfY1ukHaXRGbuGe6KZf4+mWM7GviSOxzsOxMMRh38NbL/DqenDLrWInuKNWNZRoUSg4plYmt1/JvzJ6gO6TgoBF9TXc5jKXAoqDIZJKjk4ozI+S3gYjFh/6tTD8eeHz7VCxbK8TQS8QTwq+8LgE6FvKc/22HwhQ3sUpFELastIwSe13xQlrHlHeec5lOvdf7PW2neW8T8OtteFJ1c0Lo', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (94, 1, 1, N'oV82qdPVGX0o+1ghbjr8yyW0jVWwYKZ0xzasdGdjk9KWK8wYFk4M2Wr3C2oEYbu26AvN8FjsqSLonc1fyqmhJnO6MTOUZi+2K2V1ip//RNylgpBQej2V9wUmngbotLgGIvO9XE+Obr7syQb+GWhwM3yUnMLRJ0vAyhrVqoPGxwNoKqTxwl4mh/oLUHbId4yAJBEODUUSlNEER+PMF8LMtoFPWlqE1zr6sE/DXGmW/vcATDceZUVaeW5hOm/lbg09V1GJJR+FQlenhWH9ZrCASINu7bXnD/ECSon12WBH331hOYRyDYg1ahWAiGL6ZeXbmeJrS6kyRQ0m+ZubxcqzvFkfIgVd/j5hSiJb/huCHt3iMQHMoCANhPjfwak=', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (95, 1, 0, N'yJvEFKp6lZoPYFjMVdyPgxo8uZLa4kN72a/vFGVsDyNgi5nG64+3YQM2mNyWiQSrgXJnvtQBr+yhs/mkWhYs0oSES5BaXnXnZ0YiaBI1ZVSPtcfMrZZH1dzZ/8NE07QeJMX+FeSE6g1hiwhQcIMVCQimbT085X/a0ZDkrafOO5kAYWeldXasEubC1I7bTLv/SYQhbygsG8ySFVSgO7t3d9YGRp2YT1AybiFdLvwXmk4mW7HbV+jB4xp/xrDUO+jotqwrl9ve2Cuh/N37y5aJ19+7kLWL09ZxKUz0e95N6832jJvgZevGamdViBDGkC9DryMApsr6W/84xoV1aU+K8NFK40Cyhtjq3xvWQ6EdAUSCpxxVtntwY3n2roLkmKFyojDy7w==', NULL, NULL)
GO
INSERT [dbo].[QuestionAnswers] ([QuestionAnswerId], [QuestionId], [IsCorrect], [QuestionAnswerText], [FileName], [FileData]) VALUES (96, 1, 0, N'cZLZR6wX74IFDTn+VSn80AgWC0lm5FMOJFWGoOgNagnt1UF7bEq8x3WApqIxx1D7SznpZr5Ei3S0AX1Bs+Npo54NbRQUmZgQNDaX92QgmkmndHIK0fw1h6HSvtRhYPsmB538gNB9CZuklJUSt+1DvZZXQMRrL390ueeKsTjrWDJmFfuLobjkjd7HwJnBj3RsPJKQiCQvJv1QGi7RloD0dBkRWtHMe8K/YCL02uMLP5a9ocuaQSirw9bDxtKSf+YTYd+A1q2jKAmh0IdLvdi2gEU149g=', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[QuestionAnswers] OFF
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
SET IDENTITY_INSERT [dbo].[QuestionTypes] ON 
GO
INSERT [dbo].[QuestionTypes] ([QuestionTypeId], [QuestionTypeName], [QuestionTypeDescription]) VALUES (1, N'mND7iEFYJpjYdtN4yCvmFeuqJN8auVgkD6XITTC2HbYz9OJuAvKcJNBE4SerTpKYOjQ+Jz15q7hB', NULL)
GO
INSERT [dbo].[QuestionTypes] ([QuestionTypeId], [QuestionTypeName], [QuestionTypeDescription]) VALUES (2, N'UaPe6kjbuqj0hfbkBYNOfUzkvGSfKUQ15VzaGz8PLnJulWeJWAm+6qkP17klbFL6L86cG5kDtN7UPdR7rf9bo0LkDQ==', NULL)
GO
SET IDENTITY_INSERT [dbo].[QuestionTypes] OFF
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
SET IDENTITY_INSERT [dbo].[StudentExamResults] ON 
GO
INSERT [dbo].[StudentExamResults] ([StudentExamId], [StudentId], [ExamId], [TimeEnded], [DateEnded], [TestTimeSpent], [PracticeTimeSpent], [MDKCode], [MDKCriteria], [QualCriteria], [TestCriteria], [TestTotalScore], [PracticeTotalScore], [TaskNumber], [VariantNumber], [TestStart]) VALUES (3, 2, N'751c98fe-19d7-431a-9266-9716837a5d0a', CAST(N'16:18:09' AS Time), CAST(N'2025-06-21' AS Date), NULL, NULL, N'rlsr4EHljg2av6TOikJ+xDl4ZzD5dp9icvQLKg==', N'T3lL/Hd34lpUYuVgCs2xjDHq5VbP0RtRoxXIWWhIrWuwKnjgPwPhKeMhX3IW/lXFLsPe70nIjkKmZKVzSjS2URsr89bZ7/dtwsMWC4Z0BnNgRGTk4EBv0XL7x3ToKWF71oga6+86hL8pRNSy6dgLW0PNhx+l+0zHub43XGlspxZrkMdSOu0H+eKw8XMdBzZVdy9Ikh3uaBmvFMyHcmt0ZqMC/wyK/64eUPbVeRU5Hj94dN+DMLuo1gqsIi2Y7i1d1SnAd3X/r1zyan6hhL8uyXY6lSQtau3onBclJDYXEs5oAfWMkMyP0BddoarKs/nZ/ZW9gHiOzzvUaF4K0yo5Hr4viZKb8Q+G/heCRhPF4OvI/88z2JS6tBX4/BUVSsJU+mzoOcIup4+SaUFhOuOfjDCDnq4DQhIoUylDQ6auZzlM', NULL, NULL, NULL, N'x61fAKYXKAYgB3uFcBCA80R6NdcaeZuLNCSu4DElyg==', 7, NULL, 0)
GO
INSERT [dbo].[StudentExamResults] ([StudentExamId], [StudentId], [ExamId], [TimeEnded], [DateEnded], [TestTimeSpent], [PracticeTimeSpent], [MDKCode], [MDKCriteria], [QualCriteria], [TestCriteria], [TestTotalScore], [PracticeTotalScore], [TaskNumber], [VariantNumber], [TestStart]) VALUES (4, 2, N'5207b3b5-c78a-4888-890f-e931308fb5bf', CAST(N'16:25:08' AS Time), CAST(N'2025-06-21' AS Date), NULL, N'b8AMF5e9otdJ4NCMxJcP/40cWPc3tCCPIn17miEgn4NfkQqZ5xY=', N'bnfm8dfQj3QQD/dCTNhwB1PlZ/4rC1tnpPgKcg==', N'+T+yaUk4HvZeI4rG3m+66IfeRVZ+5MuCvLQXuYogTru3v4f40Is1Rc5T6X+H98qVttnGil4EzCh3rRragVSNOW4VmnzrRS04IPcqEmuL9vK4ngIkEgKLCzld0VoK6Q4wu5qSI+AGIMIpX79TiReuCibS2IBRjjI+V2aS0gZvwO8da8r2JkN58zhBZKNyA/IktWUo/mNxPTd9v5w2OUiN+OKMZgrQq2oNPbRJp491AKsNTZwhxubD7/YapnJIxOkL8qDiB74FYMA8hx3eYb0CfFw/ej+P4LfoLH8fIAwwQhlObniiawliD0KI3DWtyHaDtIWWRsJanxzeK92cm+EHWLK5XpLXt/9VBhsRhP5ipTFanQysiLZSCyzRTw1387A8ZyursTfMPJEWbKE0Q4y37Ry75CjChohZKmApYjA39LoB/13BgxQ8ucwUeWN1WLJ27cmPWhoxJUUrQx8k0lu7kGheQ5yDcHk/rKIKw7Cis926fYdyit2cEM685NZwjZhBP9tOT5RVDUgvuWFI094eFZvLC613UG73Tr6oY5RsR0FMlUBAhJbt8EEl', NULL, NULL, NULL, N'1qyyVRXBnovES6QtqGkhM0nNtFxSaZ5ZIJnMg8wIJw==', 20, NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[StudentExamResults] OFF
GO
SET IDENTITY_INSERT [dbo].[Students] ON 
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (1, 1, N'2b851fe4-a999-425c-a803-04ce73859cfb')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (2, 1, N'97b39861-998e-4dc6-8f72-3fe0397c2565')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (3, 1, N'c6747902-791d-4a61-831c-fa988e5c8583')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (4, 1, N'aabe1687-5f3f-4153-a2b9-094da5e4bafb')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (5, 1, N'ce19e8e1-622e-4649-b8ab-a46b6be55848')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (6, 1, N'5cc7c667-faa3-4520-b905-d6ea5231d7bd')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (7, 1, N'11b353fd-ba55-4043-b798-1d452d5e062c')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (8, 1, N'9cebd524-ea23-41c3-b4ce-28d1a4b9322a')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (9, 1, N'03964fb9-f566-45aa-a120-401c56655971')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (10, 1, N'91730687-57d5-4cfe-a686-be2993e6587c')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (11, 1, N'4d64564f-b712-497f-ad00-47c9f4cbc666')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (12, 2, N'eaaeb167-a852-4d1f-8e1f-2d75ebeae4cd')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (13, 2, N'a4d28499-d741-49f2-aee8-563b92421bb4')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (14, 2, N'74c34fc9-c2f9-4dad-8283-9bdb69675468')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (15, 2, N'7844e7eb-1fff-4600-8a25-a94cde630d8f')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (16, 2, N'5b176e1d-27f0-44df-be4b-dc65761882ba')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (17, 2, N'25ff215c-84c1-44f8-acb7-dfb62d3009f1')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (18, 2, N'fbd42786-7f7d-4855-b760-c917430af1a4')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (19, 2, N'5d998e7b-fe5d-4d07-8a2a-51a5ef61b222')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (20, 2, N'01873198-8f68-4032-92f0-ab10aac16313')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (21, 2, N'51d170f0-0251-4d77-a20c-a59f2454e6d0')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (22, 3, N'e74a4a15-11ca-4fd6-a332-aebf8ab8a53a')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (23, 3, N'fed14746-e88f-437f-bcd2-8062e8a8c4ec')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (24, 3, N'1dff9aa0-427c-4382-a4f9-924494862f3a')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (25, 3, N'2bbcdd4e-83ef-4485-8794-1770501d9320')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (26, 3, N'47897281-8ed2-4e15-abba-523a7428ca93')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (27, 3, N'5f62dce1-13fa-434f-ac1f-cdba286ec52e')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (28, 3, N'ff28dd8f-7c51-4f65-8a7d-c9891f2d5dd6')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (29, 3, N'8c0e98d3-ef84-4b7b-a1db-4c01e26fa55c')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (30, 3, N'56a6f54e-0534-4d92-9fdd-00197d9c4da3')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (31, 4, N'59369f8d-32f1-4d12-aa19-f71011262fcb')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (32, 4, N'd48edd5f-9aec-4330-b2af-eb1ab8eca13c')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (33, 4, N'8cb458c0-3ed2-432a-bd09-ce482f526589')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (34, 4, N'79074d85-e7e1-4c39-bee6-dbaf8a278c28')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (35, 4, N'9a818015-a0e2-4edf-be78-337232d00f7c')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (36, 4, N'281d56eb-e7cd-42ef-b311-96b37f50cef9')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (37, 4, N'a9b6a944-bbc1-4c51-929e-f17740a804e1')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (38, 4, N'93bbb0ea-34d5-4008-8332-3cd7e38af015')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (39, 4, N'd0e13928-2b33-4373-9c78-13e4e4edc649')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (40, 4, N'278fac2c-3d91-4716-85b0-a8518d15d609')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (41, 4, N'dea26269-fa10-4df5-a02c-68a1b67e4172')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (42, 4, N'6e618c00-27a7-48c6-8b38-98babcc0cbd2')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (43, 4, N'c81b4099-0ae0-44af-b5ed-68faa9b0ec2b')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (44, 4, N'8c2f3f0d-f062-4f38-9e40-cc93454ee358')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (45, 4, N'06937f6a-b098-4966-b5f7-5024ef2997d6')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (46, 4, N'f48e0139-07f0-4cb0-8705-09be1cbf1d31')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (47, 4, N'daa9e7f9-1b36-479c-9a6d-12518392f344')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (48, 4, N'e4964cb4-ba00-466e-ad26-c7066d43ec44')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (49, 4, N'9c9036dd-5faf-41c4-bbc8-5c6e51bbd703')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (50, 4, N'0eaa2cdc-9197-4efc-a539-a01cdb8b0a2c')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (51, 4, N'dfd43d03-d9bc-4208-9d3d-c11cba152d6e')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (52, 4, N'c71fac6e-f2b7-4e62-b12a-a52b902ae350')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (53, 4, N'9627fde8-9044-4810-a30d-ee4b27bb71b1')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (54, 4, N'9d9893d4-f93c-4a25-b8c1-c3d36413e792')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (55, 4, N'41d2ac6c-d991-4eed-9e08-dafc0bdbe11e')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (56, 4, N'714b6eff-b564-4d16-ae5c-1f4a1e352be7')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (57, 4, N'2b81433f-2cbf-46f5-874a-d7c65d52c3de')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (58, 4, N'cec1670a-146c-4269-8964-e3434a2f2427')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (59, 4, N'8353e157-a9e1-49a0-a8a3-c3ea29f6a05d')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (60, 5, N'637c6e06-218f-4c24-a9b2-a451d95447cc')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (61, 5, N'70deb66a-3f90-41f0-9688-096973b1e222')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (62, 5, N'963da7aa-50f6-42bc-a2b7-f9d222789303')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (63, 5, N'a595957f-3b28-43b6-b2a0-27a32a3956b2')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (64, 5, N'5d94a360-12aa-4d86-a7e4-114b39e22e6a')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (65, 5, N'37c10b2a-3eaf-4abe-abac-14a633615caf')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (66, 5, N'194f3e1e-9ffe-4370-ba17-83dcb7f93b47')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (67, 5, N'47847604-80a6-4892-ba5e-230a9a67f791')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (68, 5, N'9ffa6d1e-45bb-4aed-a331-ba705d392d07')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (69, 5, N'60019116-df81-40a9-a8ec-15d09b78d274')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (70, 6, N'e3f36405-0a5e-4a30-b10f-b8a5bf299f8a')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (71, 6, N'b4bbe80e-6976-4a46-af47-ce4ca32f0da2')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (72, 6, N'd7c8a0fd-7571-47cf-9e04-1292350d7776')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (73, 6, N'6c98a9ce-2218-45cf-b3de-c548e13efa33')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (74, 6, N'f2c71964-3a53-41fb-a9f9-8cf690439a3c')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (75, 6, N'ffa3a6ed-8b88-4aff-94a2-caccc1f172da')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (76, 6, N'2bb35183-9e7b-4e74-9b07-5f8446af7d9b')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (77, 6, N'2a9f383c-beaf-411c-850f-b3bd80aed4d6')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (78, 6, N'93642951-b35c-47d8-8607-0e357c64266d')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (79, 6, N'bcb84e22-d665-40fb-bb11-82ca0fe6df4d')
GO
INSERT [dbo].[Students] ([StudentId], [GroupId], [UserId]) VALUES (80, 6, N'c93c0604-b975-43d9-b310-85a1c0f37518')
GO
SET IDENTITY_INSERT [dbo].[Students] OFF
GO
SET IDENTITY_INSERT [dbo].[Teachers] ON 
GO
INSERT [dbo].[Teachers] ([TeacherId], [UserId], [Code]) VALUES (1, N'213279f3-12c1-477b-8fa0-dca0b01882bd', N'RdqdgTYgcQnUfY0nBx8RUT1dOTVx5BcYSgw+l9Dt5lk=')
GO
SET IDENTITY_INSERT [dbo].[Teachers] OFF
GO
SET IDENTITY_INSERT [dbo].[TopicsDisciplines] ON 
GO
INSERT [dbo].[TopicsDisciplines] ([TopicDisciplinesId], [DisciplineId], [TopicName]) VALUES (1, 1, N'j14xCaiIUP1/qXsVhlBFOMXKuOFjWukrBO2YC8aApi/xIbfP2hBXAIwVaLwNzLJT1HhSOYFiD+MMexDZtsT33KWuj95wMW+J8JfAin7fBdIy')
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
SET IDENTITY_INSERT [dbo].[TopicsExamTest] ON 
GO
INSERT [dbo].[TopicsExamTest] ([TopicExamTestId], [TopicId], [ExamTestId], [IsSelected]) VALUES (57, 1, 1, 1)
GO
INSERT [dbo].[TopicsExamTest] ([TopicExamTestId], [TopicId], [ExamTestId], [IsSelected]) VALUES (58, 2, 1, 1)
GO
INSERT [dbo].[TopicsExamTest] ([TopicExamTestId], [TopicId], [ExamTestId], [IsSelected]) VALUES (59, 3, 1, 1)
GO
INSERT [dbo].[TopicsExamTest] ([TopicExamTestId], [TopicId], [ExamTestId], [IsSelected]) VALUES (60, 4, 1, 1)
GO
INSERT [dbo].[TopicsExamTest] ([TopicExamTestId], [TopicId], [ExamTestId], [IsSelected]) VALUES (61, 5, 1, 1)
GO
INSERT [dbo].[TopicsExamTest] ([TopicExamTestId], [TopicId], [ExamTestId], [IsSelected]) VALUES (62, 6, 1, 1)
GO
INSERT [dbo].[TopicsExamTest] ([TopicExamTestId], [TopicId], [ExamTestId], [IsSelected]) VALUES (63, 7, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[TopicsExamTest] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 
GO
INSERT [dbo].[UserRoles] ([RoleId], [RoleName]) VALUES (1, N'3/WjfdFPYic9uodJA82nTPsL5TzjcPxBA4A24r+SZVg63/2msnN5E7pZ')
GO
INSERT [dbo].[UserRoles] ([RoleId], [RoleName]) VALUES (2, N'N1u0rlIkVmbfXNJhXISkc07oyrd4sTFAXEPjs/Jkw09kKoNYbqLwyZAM4ataRF5P/3feg7nC')
GO
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'56a6f54e-0534-4d92-9fdd-00197d9c4da3', N'/2qJ7nKz2Jh1j7Mh43xyofYnrPPa1LH5MXgYpELYiCJn/ke5tl6IxIeXrx8=', N'7lNqaHBR94qb9Rpk+RTgOkUrRFJCqaPE/JIkf5/ztm0nDg9U', N'b8i0BA6bS+T7WEMYzOPdMszySYaa/vcrMOsBa2ogZNTblVCdhxQIe5R57vc=', N'NpmVRmCBkD6VX5l+Fu/p+WZxScR5TNnkILgJFSd1/pPKEeGQVPkdRuRNGf7TkxzaYss=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'2b851fe4-a999-425c-a803-04ce73859cfb', N'RE9TEFPC2Esh/RSezHdR6reaNFu9xvV08+jCvwTEe03AShWxBhuqvZMpboQ=', N'of67Hl3yVsjh6OHW2gW8yfsopLTg+36pKOTWdDAsKSDM2wRN', N'+v5+9keYjInecZndiBiy27mwReJ36barlcHinIQ4QbyjqS2CkWRJkeoq/1Leg6AH', N'8l/DLzenizAQErOrtHYIAFP0VPyGaZBDHDoI/32g82LVZG32JcyTODz4j5ZcjpewpL0=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'aabe1687-5f3f-4153-a2b9-094da5e4bafb', N'Z+aQLtxEru2ZssrWsEO8wBupB7bps94XrMfEjoAIgCr33eQWbZL9hdNX', N'1Zv4ZbTyS6xdQVt9Hs7NcqlKZlGvlQVB5rTHOpcKjU0nWByMeLxEoYkx6FM=', N'OvYNXe9es1nlSYFwkFQfBm1uZHWLEvsf1qKbTFTrwPBAchzkY40+4KeTuIBlGg5ONyWhEw==', N'1cWogDqzuwN7kIu4cYBHvo5elL6+rBDe+mFlEAmrm+QShhpoD4jlZeGGVIlBAVMdFQ==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'70deb66a-3f90-41f0-9688-096973b1e222', N'QeI3syzxsAg1w/uUfw1p/NyGmp05P2XAquCMjS4XVD69hbMUTTidOlkGZ8Q=', N'lfReBIRp/g1FQXRALMLMvfdwmE7GcLp/upz6FkAkLODpFL9PxmAam3xDHhMTaMSveYQ=', N'r1KvfzGXGb2caTCqnwcBMSmA2VD+/IaS2eYDZTG+ulXQndDMMkY3/LXP', N'0LMs3FSvxdonLJjaTyNFnzsvT9tc+/x2vUuxJnzdMOlThubG5CojysR1L17hrg4s', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'f48e0139-07f0-4cb0-8705-09be1cbf1d31', N'vK0q+MFGAJ/mjugvA0nXWGxEgAZz9NH6yLC5AYPi8dhxt+eOtGfKgFy0', N'fwA2Ow1QZY3SRHOKUkQSyNtjkNqaKiHhUUB3WjjphWCLeU/JEvw3WseKUQd33x1KtCg=', N'rAvPIM+m9/eimteUbMT6bgN4QRpSYmkYxhdulLO2yiIghVQ6cs/jO26x7Ys=', N'4yYXKaEOVe+esfv53qIUR3izLKdGlFi5OXChWr9CTb2/GnplKpRVZHNgXSvDMkIfFRY=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'93642951-b35c-47d8-8607-0e357c64266d', N'XqCyMfdOtXErYGeqFv4of/9YHedT25eybwOcPEF3VakHCPR4yOeGiWmrGd2Buw==', N'iJzdS8VyqPhk3glE9O6cluDoUTzEkEeTKXC58HCHQnblDeaToFU=', N'1pxiYwyuHZ9buevfAfAliDy9PIZozdbJfFCN0OlYGeQ3x5MOzy4oHw==', N'bLEyrB3DFZAi5FDDyhsNnKOyeShL5QkW77RAImKFVPF0Y3SjeNlUlpWU1iLp3t6cuw==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'5d94a360-12aa-4d86-a7e4-114b39e22e6a', N'uPtbh9e1Lj/tQ1LhMkxpYRgCmbYH4o0qe7n4JCsYhK5k6lA0hbyJuquYvYkx5Vs/', N'vgTJVCJGoFglJpEoGgBw+Z0pR9I1hAek9MSM8jd6aJ6cNW5S6kpQuLwd9ZwUqnk8M+WUdwo2', N'urKqjSs4AniIKO/RXlihY9k0If02tLl7S9MWb8XUVwO8/baRW9GIrjCY9eY=', N'a4vF/ycLovglQi+4QNjDov71emNjDcNpl6oI4wHRGz7Ty9+IQ9eIv855zwYSavDEQ6Y=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'daa9e7f9-1b36-479c-9a6d-12518392f344', N'a1D8TXvAzibFwIkso2CfRgLUYwAmbh4xq7R5WeX29ZJWaesbEjIzpOSm', N'/rWt0bAAapIUlwAA+47ap1hWOXWhuq7OxBknoPMvaWV6YH/S0z/0RjuVnoyqATQWMdc=', N'ycD18OkNe1cpv4wHodwAPRSQIxh+4t5Dg0I5yBZF3ngsZQ9JbyaOLV7mRN4=', N'YFHsD+Ij9AJDeF1fDdNpORXsfiA7AalDl+jRQfPnePiLXygs65aCFeWpeQNbyyGrwnI=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'd7c8a0fd-7571-47cf-9e04-1292350d7776', N'wG/N8/0IWwnPpEkt3NPJONJp6adDDB8j4U+FM5Gku3z2uUET3H7h52eM+j+4olMg', N'kB+B1mx7kp1UQegOs1jV0EIFQsrCUYAAflmZOnQbgJWLH/dm6CAmlA==', N'E2d8Ia+jl+II51kqREwNslRyFR56FoOT5B0acJVv+CDR8DouVderOSVatag=', N'EVbnLO1kg0QgOWt5a972mit8EwZgdswWskPvdY9Nngi+LQ1uj752UcHQPhszXaHOdBk=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'd0e13928-2b33-4373-9c78-13e4e4edc649', N'/4a2Kyko0osyy0l8+I5K/I/DWhu/IhHWlP/ECe0lNxSS1Flb9bU=', N'kejplxv7glxxerjxRADW/uw/S9UDHrLo9x8zWb2ffYnNpXsYd12Z2vIhSHi7sw==', N'w9AEFXQfEyuwfgWc/V1jg2tbmXcjIJ3kG3VAddSfOM6XxWsvjgC26QtNAP8=', N'rRVnFZdVqQ7k1I7hzGqthp+3NqVAcr5vvZsgjxYIhYkIRQjNoVGFfgTx7kbkfTreooOw', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'37c10b2a-3eaf-4abe-abac-14a633615caf', N'tFTcT9l8GogavMXXP3NVmMCS7lnYSqEAujcNwj0mHm7SzktbEwwJng==', N'NxexmoTYEbWwylyhcVYmthNCnARwd0C0OJcLMelh54BoIprYRP/RX+3Wi5C0Tx+2', N'2FxMbpYoBikRATZV7ysC91Kzqhlj8AJ44j4p/44Q7olX8On9CEr4lSLncYEOkQ==', N'2xZir5kuYjuGI/c5w089agzxG50e35Xa9d+8gh3qoFXZFPOmzMrbsJ4clp1I84IEungt', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'60019116-df81-40a9-a8ec-15d09b78d274', N'gKIK/ZZbvtLtXjFq2WC8sw0iS66FR7HXXI98szpMj8cSus3JncQ=', N'mOVEzxmvDHXEQdGNMIoGh1jWXPQ3a2zwlW38xvJbZ0IprTqombMGZAUhG7Ubgw==', N'Z/U2ZhLKsVSzcDutBZP3ZpZIFbOnn36JDrfxJPnqqt8VuZychNldunlD', N'aIljd0A8Qhf/eqFFS4CqlWpf3LqwlAbYvuMNdz8ycjhSGf8pFijZnWCkdL8OS3SI/Q==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'2bbcdd4e-83ef-4485-8794-1770501d9320', N'ev0YLNJzho8ulZduUVDIezfgrkt4JT5p7sewa4Mlh2ki8Y9PGcK7Wm8q8GWvQ2nw', N'PJIM8VcQhmBtdWSjXjqgFWi0tfh8+T+dhkHzCrAgNoczGjp3xVnH4w==', N'gOxim8UEMbGbmXcOQOvtxriOWdc6o/4Dca/ck8eJExYYpTU7fG2XVxd2', N'NDUNjyqfqe/ZesNbhbvWGQIlYFYSYFxiVMddCgESe/TzOker1dFPGWg28IZyRgTm+A==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'11b353fd-ba55-4043-b798-1d452d5e062c', N'9Cc3Nhrh/ZLReQ57GncFWEoW4WV2gB7BsCyVOUi1DUO2mh5io0Vuq2CO', N'fNIextwGFS50ZKmshc1WXQIC7HJ4IXmw5e0MRF26WMJzfv36fdg=', N'TZYiPUGi7hsKwT7LaLybiO4gy7SYXIZIRPsLKa4HaPVN/61yw2gPS2AmLYE=', N'LwHmOM4DSh/k+z60X3YMcp7c/eQs996rSN2XHFABBnZzvDodaBXi2Z/zf7bWRqWIHw==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'714b6eff-b564-4d16-ae5c-1f4a1e352be7', N'RgCet9GNkpGJeX0C9d/v5Lodr993Spxp0Oa31pB/844pGHhd', N'8CyVXPrOoEYvDzN24NMXDMMVFgw5bjE4d9v6cbW2z+k1GNDtREf5CZQOOEs=', N'an4a/t1kejrfIr/N6ry/UfUZtpJHBw25X+eo8By3jwvZRzOm8nmd/naZfhGPIg==', N'xDq2s1winMrlHMqBOpCxC688CltNnWaF8t9jxeh6X8WH+WFBvqgKLGa/CizEi4L53vHF', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'47847604-80a6-4892-ba5e-230a9a67f791', N'4rUY5TrZXDWKLvw/GuG/0ZWCukXXnAQmDLfy+tm/yJ+LBsSX3043MY1B', N'aMb4+SCLAzGhoii822WVyVnYhIXQg6kv4ejQpR20Od8EuFIHBJbG6hCWLUTBqg/Q', N'iu/J+BnNPLD+SgNgIwj/EJipgkJVP+tpHlR7pL7IQSjN/5rdXyPqYA==', N'KDDcKNBJkhUiiyjOU6n+xMOG+yGdrkxstZUua9ItjRMhe7NkTncw5srAl3/iSayV0w==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'a595957f-3b28-43b6-b2a0-27a32a3956b2', N'nTUAaPScHS2Ahd2E2nmkeDQALIyvdyUeLjPwPYzrQgQtQQ/slziGPuCg', N'R4FdHgxNIpH03NcKHhY76GD+Ip2B/S/dOdU1XNa1mIZ01eF+WWlov9yQBkafdfru', N'L+7POzrQxtCymEzyexmU8XDf04F438wwVKirdta1+rl1S2s4nN8gtSq6dm8=', N'UDIbSAeYUladbQ3lrCjabed5/VIBQ1oPG2wozJ+/nKj0mhfWF/GOmcas8IwUfgG4sZQ=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'9cebd524-ea23-41c3-b4ce-28d1a4b9322a', N'M3DXJ3wZb98MqKl51oOWLgNf/Zd2tzorAsf4hL1W8t475Y+pmk0GIL9ccNlgzA==', N'OXEI31mtgb7n370CD+aj92eMtcUQ/K78JtIiLVnXI3iw4XgseeI=', N'nv9QKY38fkGBaFORaHuJmxMmcwhfCScQdOGYfqIE05HyujRGkIFS3/cHVyHsOw==', N'uHEH2TpETxiBNX8lC//P/7q4CAqY98mmXsrgXUCJ0NmAxKbA7rn+0IEabMwFizPKTpz+', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'eaaeb167-a852-4d1f-8e1f-2d75ebeae4cd', N'o4seZPn4LDnday66jzXHXbHgHvLhp8WU7vhSrv05d4yXD9QSCJJj+A==', N'Fq5pJ5mGlv8JJgxqUvv3fxRtnRgUVYgTi+I45BzbMxe1jGDF', N'uCOzXz7BqlbJGtlQFG0q6CLBuZhXcepsjgRP80awG4j4+xaVEkhuLXD2kpo=', N'UysGRFxxN+ygXz7ArSuxgu0mIX6a1va9lSdlzbltODVM9E1bBy+N+Rg/j1bSy5SX', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'9a818015-a0e2-4edf-be78-337232d00f7c', N'hokX/DWHrxgVTgI6jsMxNCveMfJ1sXhHgLo7dRBUkbMIcWt4L3X2NXQ1', N'wpStgJ4h+DXTneKj4GogPAjSAlpJ94SkxPJNTzyxSNAguvPbWt/CXHfZ37eQeKiTZqo=', N'S6Pmkfe4uW8GgKJqNrW3rw5xE7DXu9EWVXIXfqpccvie+ZgNVsBpmtnv', N'pIJVfZ6qyzegm7qEH7xkNuZFzAxbRNJIxF5/kXYOJXojcCbPnAaF+GeWB5XSV9042sQ=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'93bbb0ea-34d5-4008-8332-3cd7e38af015', N'VZmICWSgsShEQQmnHBH6R+OLkEt54YK+m2Aw0bjCvy0bNVxzb6L5OJ/P', N'xurEL5uEa6zpChljTRxjW6+vjSgpgbniuNazYTa4DfWAWxSRX5ulJmlIND59A3en', N'VXqSvW0UMYLyuFIQ3AWpY8B25o49OasyoZXCCO4rlrBbmALeO39AEw==', N'L6gCI33qMLWBaZJGTgtrPx0cQvJjY2Cb66Ikhep8jrHvMUOIQYtAfqVN3SU6zwxQ', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'97b39861-998e-4dc6-8f72-3fe0397c2565', N'gWWhax4pdUzyee63gAf3WGuBisKzmox0N5drluh3JYA+wvn33M6ZNmw/', N'58zhF/XLyS6aOXFa2SfNp3hrQQ4oD/0SjOJEIfEnzCKLIIIxC2o=', N'1fBjW9+rUvufXmVNiKmJhxuDWG+zxssJErbVexDfpoRfX4aN2OnCknNMJV5x+g==', N'/2Pg3FpYpIE3DDWaZsQ7GmeJyMhQ6H8AuwnVYbzbYUE9pMIt41tZOna7S0x1reFUjA==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'03964fb9-f566-45aa-a120-401c56655971', N'XrVhr2polY23tbIm56EHINoNRiOr0P94cFxUm/76VsEnHp15rkp5+yXZ+S4=', N'HtdJN7fw2cXAfxtE/ABZsCcV7hxSw5cZK1fR+0en24OzNPV6ja8=', N'm46x9OAIXeXVmqvKym7vyZG5hlkIPa/0BWP/tb8FYCIxTAdhvIq3V3F2lmQ=', N'KAL31c6Eyricix6WaELamcun64zLT3FHzwHQfCFgBGxoy+9LpWPKPV6TuVG1IO5g2jA=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'4d64564f-b712-497f-ad00-47c9f4cbc666', N'W6nzVtTiHieTjcTZJnagHLON5vwOkZKkSJ3GnyYFp1ef6RFr/848MA==', N'FswqKEP7sr97dpbfW7YRXnCku6pmIoB0kmcNaqllRp7t8bzgBLQp6A==', N'0JcOtcpoC4yM5A3wwz2ac69HZM38Y4FrMmHTNgwMs+3005uZFCZooD+02CcKDoRB', N'7mNX4dKHUMU2S60veOQBOSHprlMgpkvuyqzPOjVZ77TK81HsN4dxoxgRMGLezrM5pQ==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'8c0e98d3-ef84-4b7b-a1db-4c01e26fa55c', N'RrpxKJk/MkhQhClZzZwzwAfYaEtTWHc0P6Or6+u/YaEY12frcxoQK1YoWud0fekEZpnwRdPa+s4=', N'7A2vQNYrkMAG/83DebhwvTd9fpOmrA/Sajw2hikXtk2/w4I5rwDVUJ47rVoRKihu', N'tUFgfnvuXuU1tsyJXqtm1V4FO5ssDUb8cfYIbDgZSnUeHKSGtqQJv5IQ', N'm/U2lqdR0ba5aLGjul8Xd19Vy0vBZv0oPvw8+Ku1iXpP4YUzIi1wLN+k9ikFLFs9zuM=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'06937f6a-b098-4966-b5f7-5024ef2997d6', N'q2q9o2LNmBxCJxh+Dv14A0biIMMpaSBMv9t3Vf15aRc=', N'TRP4aHVCRng1AajDI3iaSFVgku5kyZ4AyiMjTgYZ9/fFSwcTYil0UQ==', N'1zFlVV2Pv8xzoCahQ4WdWBMWemcy4aO/ZSQfwT0RxgcMPyxIAaSl+kZR', N'WVoRkeytsEMum6jHG1b36IBMj7Rd3fu9ZNmSMo0Krv+qtvlKnilJLvyqI/tRyIJk/g==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'5d998e7b-fe5d-4d07-8a2a-51a5ef61b222', N'FldGwdVIaoE1+9YCOUc7vdfs9wGg1KZw3zzgRF+ePidmRiBOAjXzRi89', N'EPo0gMC0TZwHfkq84lma/1Gxye0aKXWiyL10yeUBG/jVisn+qGtMb2yL', N'D9fsWEwIWe6YvsK2CSCIs60L6O6/I+BiDBME7HKmWagRe5sG4Pw5CxZfsT3ybBna', N'RL05gJyaeudEgV+1d1hzZN3ablKVBpeO0Q6cMl1lQKSGcGv8oIP79JAmCdFNN2AxxA==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'47897281-8ed2-4e15-abba-523a7428ca93', N'OG3GbFI09NN6EI3vtBUSMpo2JSOgRV40btJ7hjgzhCq+s1R92RE=', N'OOYFnjXYFQ861PsETF3sb989KFU++vTKguOb74ixFVPXkA9T', N'ba1S+/JP2jD4cK82hLoz5T8RhpRdV/4J7jLoHR4v2pmT6V4yTT+jmBX3', N'JrbxqHWiVphRnhEC0N8I+F85dU3QmcEsGyxSvhzuOXC9gziDEZKkVLaE9KukPKRqRA==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'a4d28499-d741-49f2-aee8-563b92421bb4', N'N+SNUnIzpQ3YmcRftsTC06Mp3ICLzTkHEKRrGzouQVV0fTk6+AQnzA==', N'nd9Z2EILjdQqi25nR0SJ68pnvx4Ueo511vxbVyyddJm8jmUk', N'hCbrub/te3rrRDwqdlUibjpVFXMoJglbff5x8b+19mnN2uMAAV1CZLC6XhM=', N'UpF7f6d7nVuBaof6R6XpQGdwWoK8Sebkv8hIo1b7zhkskdvfXD0gowCHLU4KjKl5', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'9c9036dd-5faf-41c4-bbc8-5c6e51bbd703', N'/jMZOxyWYiKy2c3YQEBgzu8+cAGkhmmrXd3BFEluwp0eq2MCyI+kpfGKaZJOCQ==', N'02IC9Iw+jEQ4JrQ+JUlGwikYNEJnWmA0SCLzzEHz2d3exLIgla5y/1+SOm9BHtC3Cby4oIkf', N'DPBJVMOY7foQgBjNpp4wvYu4Xwj/rGPajwoHtmXcnvtGj/u46Eq6d+/bDFk=', N'UI0MjT1bRyL/HayAMjxEksNGKsrEc9+HQVz4HFycxwymk3FpNglrdgRHXcUZ4XTcFPr9fEs=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'2bb35183-9e7b-4e74-9b07-5f8446af7d9b', N'6sCZfQflfpT9/RsSmkpu8iaBsFEPXniDPbZnH0l04gfZICEGfHMQoI3YbDpKRg==', N'viuSWb72qr1cYd3X4eAJwjvQZTuWq/Q7d71X99WMbS8VFLp/QD0=', N'eyGAfL/btz9gudSAu0mq+t3FtUJJChTZmFsmcWNMeMnTUc+6vZS7xuYSUQYtZSTm', N'mYv40WgqxZfPa+GcFfC3Y1HmqBOqv3+ZfDdj7J+CIuvoNszPuc7HrrWRpXwxgBTZ8FQACFE=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'dea26269-fa10-4df5-a02c-68a1b67e4172', N'97BkrmtIKkdyUU4DIWtRbjSvOTJXJ7Iitu/SOMXov+DXste0rRMuHdcU', N'qkumS8IRxdImYnXDudiYdtpzJZ4QLXPURug1zfVfdJdFyNOI1JhLSFjr6o4XgeZEdfY=', N'Ck9q0k/Dc9YYjX2UX9IVTIkfLzdIX81bZVw+8pFTzOsInOSvvGY=', N'amh1c/vv6rNiJ1PiITpeEi/YuXWjnzPTWhSFc6S9nVkGeFEfJnfuz8JnM4xCW2iz', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'c81b4099-0ae0-44af-b5ed-68faa9b0ec2b', N'QxzzP59qCMBcZKtCOhykowaLIjOSjv7+hZPaWjOzUNrq2KU1TucAEw==', N'MMLmOnECMC7v/Jypf8v+fzWpv48Rzymc6ZURQH38jiGcHjft3WO/tERU7couxsMc', N'VF0VSkgudaprIbg7WI7MB52eLEGupqL6wg8hynpzAfWMdxQE6MkadwcMXUY=', N'u/8BgrPXyvVl31khKGowHyduBjgjytsZUs93lHoDX3Haxk/c4X/5uWiBATefpU/q1Rw=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'fed14746-e88f-437f-bcd2-8062e8a8c4ec', N'lY1wXspHBW5j1rL/JvBaR69vpX3ARQO9Er/bOeyAIIBxEQ1doZA1kuv8t2l/dgehii8=', N'j6NTOEfOnWdcrPJfsWqemmaPPdGFUiD0jgGK4bWHtF+BNhlotXqxKGkl', N'PaTGcIE9u17F4C38hWUiYPYVcPjiGdZKotsLXpYiaYkfUyNjdkHgesgP', N'jNkIgBEfIcgvbLYHNyTXM32z6u36SvVGjMuT5MBKkyfpwiqdniAEN09Qbsy+WNG5rg==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'bcb84e22-d665-40fb-bb11-82ca0fe6df4d', N'2kJrcfgfihFtooaKtFcx8QGHCFUutQ1TlTf7ceyo6PRRkChlOlSTu4YiX6Am7Ja9qH0=', N'WVq3NakuhMujEExU+QWOzcvRJa36FdMjfQYQZ6s6vKv0+WRWditXo0mAQ5Y=', N'QSb1DkI/vbcJB/geJOuAfTgf1wOaxOFS7RM2LqOC5Tck/VfA2as=', N'ylLvxIRGYgQAb2rNZuff7oOocmv8Yt2lXpxiLo3I7eEnZtCQIKB7Ob1QkrbukaQ=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'194f3e1e-9ffe-4370-ba17-83dcb7f93b47', N'Mxs51bzXJUKUs8leS2uk517I7NExYT1UZmuikSXGZpkgqd8oxMM9hvHtTHw=', N'DQg4VJQ3N4SbGIuep2ATKRFfpOoNn2/BplKCgtA+G9PelLPnYMsLX2S3w2BJZcbHb2Y=', N'0R19PkT8UWBQuPlM3wThW+apAAWewEQsd5VMpIj/U8fdkPDgsT0kYA==', N'SQ3eXeDQpnXbk13p3deTubuktklhjTyyNlSdMMPPd/KmxBwejFlZgjG6GU5FWeCTOw==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'c93c0604-b975-43d9-b310-85a1c0f37518', N'LKEhE5cZCoAHMiqHZKlnkQ9/TM7VCkLlkqZDU3JIhBivLiCgDtlZzdyFdxlJcvST', N'1zrsFi0MuFSjYmZMuo03boN4tXJiV27FX7fMR38beUNKBdGVvnqW9A==', N'bBzYzpddkDzZxyLNlJDGw0GxEOmjbFlNhlXSaeSxbkrTDuZ26wSPjmn8', N'JzcuNajVyV5Dfn8gSSTUSzZfX4Me7Yeflp2kVVR8NxAVuPUXVAvodWWCtCQLi7Sllw==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'f2c71964-3a53-41fb-a9f9-8cf690439a3c', N'plYxNfAqyhffYSrGysx4IvT1CmEV5Ew3SI0ZRZUVkQxzqst+EYYdJcOqyzImVQ==', N'M9qCkgy5kbeYEAiFL54l2vc8grF1sGERS4cKHdXVCIDZOl3062U=', N'MfJdO2T13smUsYx4Hfx5LDL0/vVrhwfX/mGfiFDd0QYtpFII9XmDe7/KtlY=', N'qjER/q7Fy4OIa1jJ2SwM9mhangjyEoLNm5+BCoaZPzMiE1bt2HwEdqwko+77jDjeyzL5', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'1dff9aa0-427c-4382-a4f9-924494862f3a', N'diPnu5NadUxRoSpVnkuOe15FEWe6lB1mqp9Wr4sWhF/+MA1HTdF+XmkW3lfHqA==', N'XteH8OCciF4EjQVupwx6eTNpHB31FIaMA7o/c0yS6QCwN688lJQ=', N'CEDlst/wJIGet8iUXiO+O4HcmY8d9ybKycCVlfHAXaj89+b80ZfdYA==', N'WrHuUeoNwykxoMjFnY2Kw8XNYbwEu4BxpL9BomRhA4MZmWNFfyV+3cMNgkwCjUSK', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'281d56eb-e7cd-42ef-b311-96b37f50cef9', N'/AGccwiUogKZimrdNdJNg6a1lxSwDW4BBU8gXR2VjOqY00Eq3ywBuHdU3rg=', N'N7ZF2C4S5IiVtWVyPkZk4zQgdj6jDW55FA7CMQxXcZV6cMFJqvlWi+lOgVENqv7OeADuUQ==', N'xI6qzpZmpd6WnoVLxwhSdGdBgWs92UB6SYHk2dg7ivErpM0RO2GJSw==', N'Rq5OVHf5rvalNz8KUQ9z3dR/vN1Df2YoMU575X9UmRj9i/9eGLH3qXZlO07FndYz', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'6e618c00-27a7-48c6-8b38-98babcc0cbd2', N'9B343i+DexXlJiyoM7Sc6M4W1kNJlIdOY70OJheoKaaLprUsrZBRAEKE6lOf8A==', N'0Wux3gJezay5KctYbC4pbnqbQDiXgSPJ+yrrIPS10mfienqwPEDB4QPqFjZPjZ49PzE5XUbH', N'ZZQToVuxmgsweXS4uOamPFDE9l8nDn3hkR1olb5o7GnG38cenLnOmZQW6m2Haw==', N'kBGVRwK+W9bbr2JOSgu0KvDxFFCJB710oMbU31Pz8JSBB9pQwQ37uT9eWsZ5C8JzoDel', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'74c34fc9-c2f9-4dad-8283-9bdb69675468', N'lUkazRI4HVP9NaOUNU0MLoML7zQQ1FKVscW4H5bR5r86iNKOwoWtLTp4', N'RhNj7lAzaeD8lDYTtDK3tUle50X1CTW9cSHNbZXapNNCqxqszKw=', N'Hf6A2aCraMUV0o/i9reCohEalLr4pPa7YXMQeFkNE35reEJwQ5I4oVXpXXb3TA==', N'l4gTY63uwnEOrS1X1/Pa+j7DFptzoGb9tZiHnyEzZ6paey5Xqurp9C+1EsydeZa2rw==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'0eaa2cdc-9197-4efc-a539-a01cdb8b0a2c', N'yeZDYBgI1o5NGUJREKxX2AEmQuRtPAtn2iabIIrlyW6Ahb9DM3SguTUC', N'wGyySOYCYmkomrsAdToLrB/qUuTfg5V1T5AYVQWL1BnAS+b7Lt+ke8zcfxVs7Isr2WA=', N'j5SCvW+ZDPvIQ2/e7BtoX9uGp7GR6J7sOCAgYnEI2LB5lK2iZmI=', N'i+5n1ZR313UGHlSJ4MGeTCfmwCPL5ylPoETSdXHk+/bB6eC4RJrnocluN8fWg0o=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'637c6e06-218f-4c24-a9b2-a451d95447cc', N'AosLo9F7/NyeCAJ5p3C40p8nHsYcVHeJD1NujdiDVRqtVg==', N'6JPusmrJQDgeKj0KoIECP7IpVtxmXtoN5f3e6zPEMj2X7Jle4nT2JSL3', N'yuvPupsgFaQWDUWSLyv6s0Z6TQFaKdF3v40qgNnkUKp6nmWAGegDK7766/E=', N'xX4/wIgC4OmXLSmSY6ftlWktRidPhIOrtfJH5X7QgJ4TeA4uPvU9lNQY32tv0Vt1UAM=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'ce19e8e1-622e-4649-b8ab-a46b6be55848', N'OzxJ+/Kp7JZMAGf2NgXnPb96WNw2MCuWxSbmIJ/y8izlOTDK5dE=', N'aiMoCqgjoHVDgddUnwdKLU+Mm3Z9cLMf5omauADMn6GkCqs5wF8=', N'g3fLEt9656o4+DDl1YOBrsgxbU24t2pCa2mlXuKLtrLuYZI7Cbdh7Zz3o0BpYw==', N'xF3c4BweRgEu03/BGm4j2WuKEdCH72M6bjhQMvwmES4dDQhFFU3lniRHiYNPAYs=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'c71fac6e-f2b7-4e62-b12a-a52b902ae350', N'NgUxSEzgjXF0+fVXTVGT1BiL3fUl3tDyQXIeJrIWIl4BJavO9kPtnfar4LmwXA==', N'4liIBUm+l+I5XKH3B4qGOkU59GTXX+QfTNassx6HyvZrj41zO1hW1J4wnCa+wZHrFjrU9wD9', N'v46oZs7ppuood7fp72oqDXXVtuX+qnE3QHnbZZ/hHukxuNDNd2d7aTCi', N's15IofwTnn33PvIpwRh/CQeZCTguSABvLlw4noBWjAUhBfVRzJ0xntunSzfekqwRElY=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'51d170f0-0251-4d77-a20c-a59f2454e6d0', N'1gvblbeacin+Tf44XigfgozOxrFuzz/G2u1dNDwfzHVQB2KcNOIpsbak', N'3lB+aoU+9YhF2l9bZoicyqcWIF30jmuWq7Zb/efEmcf44OSJKJLh/VDOs6/dNA==', N'rqrOSCBylxY4oPv/emH0ZXACCTTo1GeLWdgHU6wokbsXQCHAusvfw+LKBN3oIIv3lCiji4P7', N'eICb41U9nK/9U1ywmydTy1Vb/WAMs8GfCl4ive1Ojv01r6j1lbbpZAYaYIs9rxpDaQ==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'278fac2c-3d91-4716-85b0-a8518d15d609', N'cMHfZGP792Twm4t6jWoBCbB4xxmDmakd1SH2nXGIE87PJJfQxqeWhQ==', N'3+uvfLCPUGuYjse2ecbufqtTVzRiJFeOqfzr0s4+1SIkojuy9tReJUWJ9vZuVqeL', N'WtYa/zUk3FZkOb6bSZv6PVaRarU54Lo5+qXZKeCYdu/kGNe+eu5AmnGv', N'IVU4vrWAyp8v4CKlSv+qoeSVXc1nwuYEuH6uQ8F8EBOlQvei1rNSLDys/IgxIbT9cAI=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'7844e7eb-1fff-4600-8a25-a94cde630d8f', N'JqUgEyUw9Xn4MBVnwD7MPnpj+FAEsO1iuOnNum4uRPg1QQ1In7XeFmgqnjY=', N'967jrWGMQbHyRnUftNSKlN4Ys8Td6wL0g+I2jdUswR6Gy1WyVDOJMWoh', N'xPvZynBaXxBN3M8sPFrMVa26nInhstnw2czBq/xQUyWV90e05g3uP47ez0VAWer0', N'2oc1pMRI/HZJ94lp3kSU5pD6wJ0FiQPuIuzvF5UQ3JNRTNQLyGIEl5pdJg7lrkzVSXy3', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'01873198-8f68-4032-92f0-ab10aac16313', N'rpGLY2lKiIpIIAzM7317z52tIqRjXi8tqeH7HeMpN85r3+zqMLBZEVNnFhY=', N'S7KxgkmUaME8Wr3P57torcpU8jfPmSJ5Ii0H7+7xJu3d2EqYLNTPCw==', N'D/k1NWIivQUBcpYfUKsfYeA5h72PQXfdZnfVzx5VTFkn9SKSagszaQ2NWXB8RAjn', N'KAsfl5wPT6O2Jz9SGob7iOGKPuLwQwXuZM7BKYOQoqIiV5r+n8Z72Jw6Ejk+BU4POw2o', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'e74a4a15-11ca-4fd6-a332-aebf8ab8a53a', N'Uun88+xwjWjKMaqGrcbSplXZwNLGI/blDbzYNVcJ2gp45BYzYkA9zPmLqD6D3sxq12SxDqEb', N'+7n5zNJdDc5nCi8jJkYrr6isBvyGVyDKHtZU4bGlBiZtH2XbOIjbC1pOli0gOA==', N'UtQnUVTK3JZWgQoQM2pVgGqodqEVHRctuHZ3UKPddcvKWCs+JjJiju1S', N'8s/C56MYogpxR36r0DP1YBguC3GylumEx/EDPPYHZcXq7f2mPDLtHpDXDlbyKNcCWQ==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'2a9f383c-beaf-411c-850f-b3bd80aed4d6', N'RL4TFISQgTvS9q7FbahX2/tXwl7m4ULIISulMA6BpjyE8NGt3JcAJvDGDkk=', N'iOxkEb42u2LsLrMGp8qZUJgEGNXeqyZsOjCoHgGkbPL/Zemg', N'u3zpYZU+LSpTuXn/DLjRO3Gk+ndZ794bfuljRuTJ2IuAmtBHo+1P3FwX', N'AlHezP0JPMEsYVcCGVpAuIvLUbRQksLygXIbGWYnOENLCfkV61tGlgtPLk9m73ljng==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'e3f36405-0a5e-4a30-b10f-b8a5bf299f8a', N'3SFgJOey7PgqBzy4VGORKjZI3u0Wnq6KolPS0eZJ8CxZnxYw+o3APfCtVJNCX2ki', N'kVQz+/qTFE74t0GbeSbFtC92hRcwKoJ/PKQ77rIL2bjzbqpto+2mvA==', N'3NM3AMc9q3sUzdNfXVB6tsEQlWE2uM8Q2gTdeXkERzlWiidioCoGTA==', N'HNyKj7saz8NCKaSbUIFco3enyiuIjK1zqQFRv3aScHD5lt4HoknstQq6VPmaJPXZ', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'9ffa6d1e-45bb-4aed-a331-ba705d392d07', N'Rpv/4fsAdWjvrDLsyT7Iufq+yIXEsWD0ew/X1VgFSr1EQCgv', N'RxajQz1wIOXTlb7ZyDC6iCC71KnxZvX5lACub+Zh1u/fyuVNtPOomi74', N'Y3Z24AcKXQSJoldGZYdMC0dfbcxW87xLX0185iMDHFNdX8qKIS3VYnxp', N'tX0BAfMtQ2hTst+H1HJcRX/P9v4603Dgd1xGv4GnrJamQxy2EHjcFqdwmAR4O0lDEw==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'91730687-57d5-4cfe-a686-be2993e6587c', N'RXvmBtT02SSeV9GCB3C7KbhEPuvax962THIveNYEtxQw06++9O6kWQ==', N'yuNx3Pp9yz7WwNJke6WOAlvHtJ+1tty9aJJWhEfuDI8dz2VM', N'wGEZ3lqMV4eQrLPEoWHxX4QIceubpMmkI9+2GVDwPvP1x5DFt/OSyE3m0Qc=', N'5+8yg/C6REWjYUa8sJb+tWnDfCNrFzUStLzgiTh75+2Y7bNQZK5KxmN5hXsvrbaf', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'dfd43d03-d9bc-4208-9d3d-c11cba152d6e', N'uq1GqYQTtrcy8B6sVY66fXI6TIvoEkyp+Tg5B+1c6r+hAUYRS4FDqA==', N'iVPKzdYG30RodTGSH+4ZSG7dWXaU+c+efb8aIYoktlqlFJMy24VLKb7va+BntYEy', N'hXP2guTlsViQijLwscxlzOSnMZH+VY+Ibt8616YoAY6JjJfftfaL7ml64Fs=', N'9RCQUgc1pkEB1mHlOqcufgGlwbfvapl1OP1LfI/4O8O87yDIcRmeJJ+kb9QLCkZ+AHs=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'9d9893d4-f93c-4a25-b8c1-c3d36413e792', N'eaMfPuyo6+NtF+9vKu+nV6mtbtbthvMShZKNOLfPycxMrt5GqJE8aWDLzpM=', N'+YF+o5pfB2WyZPTJHf+2U0EGKUiB2WGIJMbnMsOI8Fo41wRd/roFxL1jhzJ1q0KRVGhwTw==', N'mi+7Gn5m/rh80qxD93xVBTAGhRNEitDBEzEPI4hbniGk64vNZ5JA7A==', N'p3V9LAmLK4mVgRS3wit776iMk4x7jGbE85cfpuQ4qzt9oHP2jwflK/WtKfOc+pqA', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'8353e157-a9e1-49a0-a8a3-c3ea29f6a05d', N'tRY8i3UUTDQwOWAOzsGGh3Q8WMC5ZBSMAvgHPGAALtVb7MzZxqGjq1pA', N'34whPSnV2jYIPwJYip/dTKUZOuO7JEtW+OQs/kg1ZDUBJLw2gImg4eqEBLJ5SCzM', N'IeMgsYpmzYMwfrVO4sGEnPODrez+uShOy2P5GgScgblu4adtlpflRHSk', N'5CN1Q/NMaGJh5WG2HSDwK9tkcpFdIeG7pkwojURMp9hzfhIg5Bh2Aacf80zmhfucRPE=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'6c98a9ce-2218-45cf-b3de-c548e13efa33', N'5w3p1wRbWvYZvPCT96GytfZL3B3SNlYv5U3UzRQCR0T+DQi69vd4aSlTFahAQ1B4', N'9GYZyrV26S/4jXxD+A3vp1MIt+hEv+/pOqwsCI+ttz7O/jWIIeMjrw==', N'jaA9ar2DSFyutLnXoGQVqxYtISPy+UHHNzjoMiJKG3b7dLoSC24=', N'eJwv9m1sxe+SUP3R+ASj67H2jx8nBozTqVsnMvn926yEbjwg3vlS48E3lLx1cN3I', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'e4964cb4-ba00-466e-ad26-c7066d43ec44', N'LaYsb23l2Iot6osjnEJCo81qVdNlWwVr8sZMiu7b/xoKvbD2TCaKVC4Gqbamvi+3', N'pL/j30Hxkl0jm9A6zlE1tN5pHyGI0tm1CPIoHn/9iC/s1J+vrCDzZ/xm+5w2R+j3VICYgk0R', N'FVIqChHLQ+acFKjT0kxmSJJlkLnttTOifUw/FsEMCD47VeH08TscyQ==', N'EtnXqSLlgCk9jI8mxKiT+iuNqQkdxb2C7RowLFGcexH4hwyWD4OYpZG5pXD6n0n9MA==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'fbd42786-7f7d-4855-b760-c917430af1a4', N'5zd/JrgLKQTYj2K95YD8w/kIlQcWnCH7tTfPdcTKa2l+YKjFkBE=', N'/l3ZwWMx1ChFez0V1KHnBWgELiUnRUTUxsznfvSzv3sXBiZhVKGF5A==', N'4lNSlvwVX09VTW/OlLxZqgKvGr03Vtc3kpzJjeBWCtCHptWl0kABLOB0rq6UQA==', N'471AARqdF6v0KDfOXYfe6aHgEbvfAvVUOhSWgtOP2ql29XLJPNLek7YLuQmxyEE=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'ff28dd8f-7c51-4f65-8a7d-c9891f2d5dd6', N'7fR+XM6hEDernV0gtsV/PQdmVKR2Gr4/RjRzNH16D/38ojyhj596jZ/46ag=', N'rxXkkvtYPeFoczNeFbqOWReMNancLm/ZGvlPS0+PWiZVSkJs', N'PWbMfHlrMSRGA+3v2GFXJh9qpFpAXrNIKZ3LSMPz0TxjNkSTg8rTUSbB', N'B1MhByTZq6D2m4InmOuR1tI/ajY1vesZnM3KhzO+I22UOOHMC4H2FfH3v8Fjvw/fDw==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'ffa3a6ed-8b88-4aff-94a2-caccc1f172da', N'fL+jm4RajlzWY4UfloEq/v3ZxoIXk/KlnjslAE9jjbcKov8kl6v/X+SIy+s=', N'MWz3p/GwcFyVT/L+TJs3bPnV7ySGljfnzPbJWRVGXxdYXdgBulo=', N'GT9Xx11mtWVmg3Nb6B+zFQR9MGEum4qpH1zEHe0aDAy5lnXeRtJ9rlHZ', N'u7H2rvotHMMwJTubgOUOsTRnXNPgPAhkUtWhA6uEynaT0qwEMWAG9MXEuIrrtFqZFg==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'8c2f3f0d-f062-4f38-9e40-cc93454ee358', N'WBg/dRUufyuc3uPx1AfkiIEcLAkgNI+OSgyiyrDJJwgfgz6EOi7aG9no4fA=', N'Lr0o6qE8Bkvvk0OSnuaobOFeWUv93uBzKw0VOL9jBDulQQh9ByczNY6lBKFezNyt43pO2A==', N'7GfCQ3f+g6f2MyRA8NBq4WTg9AR7jNT1wDgCIpj/D2r0Pr0cEP9Oy4zbSUs=', N'uhbQneyKYn8vHsA3AnBtHQByDAmee/gvfCqP5fWNvK28j+Y0zP+XQngdQL0ZrlfgqyU=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'5f62dce1-13fa-434f-ac1f-cdba286ec52e', N'eADWFYawsmnLOTKxeh3XNJEXFc7TXHidU8mkGuQn4UTMyrSMwbAbgT3R6PLdh6CL', N'HrcwoRMiu2jncMVQgM82vZLyhBCD8Ynf9x6j9T7H90ERO4S7qRL2wTN/', N'Bl289Hlx/X28VoktmpvF3zcnHKLXwQHqyr+6CkpkGyLEZprqAd98ye/F', N'FQiyMjqIaPrFy+ifgcy+DqoRi2nBrw3lBG/VUJdCO2hWbKhSPzJqA6jNCcubWsxkAg==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'8cb458c0-3ed2-432a-bd09-ce482f526589', N'8ITNzfvWes2tushkbmx5KNqCEGgMWOyzl+mJuzc2I5jZgtofWCxAtA==', N'W/kPOmlcWKriK2vzh40AWJLrSBoOZiGnivA4CqDmdhcH33m4DQnGxdJdCq15MtRi', N'AkY0FlacOzBk5oDDBv4fAJRutT/czVt+MlfuVhV0aaeI1eGXha8=', N'xZUXSFYd1qz+lJFshoOB2qZ1pZXLWuu8b44uY5t0DH5qqtjyZeqQu77HgDf1ASUN', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'b4bbe80e-6976-4a46-af47-ce4ca32f0da2', N'VzoMY3GYpzoQZqr+npqgnrScYbvQKyqY5SbFuQUV8TSt/CSP69ATahH8fyq7P7Kx', N'7quvre+FO/g0VeIICmCmxitLrnyTpb9x0pAKDEeavWSHsaMA/taK5A==', N'S1DQF+2tqyjw6UPUHOV1WpUyABRIEGjyiSwsDSpykukYzalnWcs=', N'4UF6+rKUpiUHtVrEZtRXvZpFSwyM7b3PyPDcwUQTNIDBrThjDSdCdzx2hzNisw0=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'5cc7c667-faa3-4520-b905-d6ea5231d7bd', N'hx+rFUUChwZV5F9L+pH4rIKjEK6j2bBX5b5mtLqNCH9sPHCsnbcjo6k3', N'6gO4MEMvFvYszujgnbBLc6wEh9QQhwt7AUlg7YAg74SXyXyw4C7tvQ==', N'3BI8KgDba4U0FMqQaapODis+81+0GYZFvwprOH6abllJ2WRLhJ/AYB7RpUMu2g==', N'NfdVp/8zTK2/Kqy0KkArhgjIvfNzbMXPYskRlDvd4VaJkN15WyDX7EcQ/VvWnmVRtg==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'2b81433f-2cbf-46f5-874a-d7c65d52c3de', N'y3heTpe6ZebnnzWCsa826PAArWC/LBSZy3QQtsUFEEH5EyGavKJ57g==', N'egpCHisTWXrSSZ/Vvdvo/SanZndjshh5Dzq0tyZDSYbb96StayNACMIPXTXlJubt', N'MDMGkRiRf+DcNLxMEUh8w8X9BQL3BYdlrwLq7BOJbFMgSu78OVMTnTfG', N'aCQ5+3OGRSM0rVOHYCENsqxDjUmOU+9xKrt/+1zHydq9SD37+xsIzUT3djEnt6lPjg==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'41d2ac6c-d991-4eed-9e08-dafc0bdbe11e', N'3H6WAGkMpqSon8fPU+2Jz6iYmL1NXnHeAmPtYQqIYUWntP2KsKWCWdvf', N'/vUOBKIZODzxDc+QSWdGdiSVsz8NjHurNJgTIERB2EoRg8ic0YCts7pyhD0FCLxO', N'OU+bG9fY4SdaePop/QyvqfzqAlIOQEuczkLcWvKX1H27rq//Wn8=', N'MIx1iqtzDUqlaGAuUwg+GRO+/mItXOQegIaDmK2WztJwYLdItgl8h+09NsE4TaTWhak=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'79074d85-e7e1-4c39-bee6-dbaf8a278c28', N'I0PaO7V3Mzns954RiZDPwd+5FF2UpWvPK5PA7Dg1K8e10JsXvOOrFIBx', N'2oOZ7XXidu2AjYrvjBMwzAujZUeXZqp3+rvH4Nl9NvbLDwY0gHR45H/7n6ahbs5/', N'ksmN+3ZxWwz2iBb94q9usY2zUGwvbieIrSGC+fQf1Z1h2NBYsa8=', N'28a5EayjLXd6vn9bnHshQ8LWpaMHo6YQWIyOrHUcvU7i6AtluKyayJAmuigQeBA=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'5b176e1d-27f0-44df-be4b-dc65761882ba', N'Y7TXJYIjPH2Cpe9/6aMCrP8C/Sb1DFjKBE1Gx7XGqaeDfqIRb6wWyXvQ', N'YSUelubGSJ3KLPVXiShXdjETUSRYcTNJMhb0OFT5ou1Or0jrai0AR7BX', N'xoUKoEuEikSLM+bmleyWkYU0S0cV9jKYojAfYn1fxZDGA6VTR18RVC6WStdSMrXc', N'u7LCHEpq+HWl2hPmPpO7XU2SNUzNfeDrlTjfrLxHjMylxHhpCPyDptWrrpDaNaMv9g==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'213279f3-12c1-477b-8fa0-dca0b01882bd', N's0f00wU51KoquhjfIbu6VUwvbgeYU5cEZxftxbGdwqN6SE/+lC08xf3wpHE=', N'jiCn+Xj411Kk0k4EycXW6D1gKB1IvmDy4ZpdBo9oISX4Gy0PtJco+8Ke', N'N7HexTxb7IWmkKVwaUUkxcRIdLRS4BPSoHZ1q0TvPG9AWxv2sYpWD28mkErNKoV1hi/0rf4y', N'vewBiMMobub7oe1pfgVagfKXYEkXAh2nm8I4d2uOcb9pT/Vj4aK4io+U03vYFPHGj54v', N'JAfrNSBJWbUcSwMwm3LRo+XECnLTLlKKnyY/LO0YvxBBbnhI8n/66vHJM18MlAtZ', 2, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'25ff215c-84c1-44f8-acb7-dfb62d3009f1', N'uVy8Is8HpbDO/JTm8tYP/KQuBZ3Zdiy7BbJdH8yBe6pkQi8+zOKns/4rSik=', N'EKlcsGC1Ejelm9x6M0F2T7uLVCJdrv9hvFhKT+mqX8vD3zTLtkeiogR6', N'NgWPgB80rJagCrxfkDgy4IvPIOxrUJZeeExICXOSM1/BbuMD9AR8qVLVE49eXf6P', N'zxU6gvshZ0Uf6RTinK+26QD+V5rGK6ICZhqaLZ9c214KEF+6+OK+yg4zc9AwQjuQtFs=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'cec1670a-146c-4269-8964-e3434a2f2427', N'tVck87Lzqm4kKbtROOsymfwWfYhhy6OpDIT8V0qVgcNjxdXwlfECHKWK', N'OTGPwCVDCK+947fqnzRxMk+22fTGxuwAQovglvwe5J3bNHeZaIexyIyicADUjcaLFu8=', N'xZj1FFSscJpiSEQ1pNPtmbdCWkmooZNfSonneZ7h9EnwxnXstNH7GlqM', N'vWPzNx7iTsUl1KZUZLyUPA656xAzFsQzuhM1qpy0+auEJj9A6WWa7rQxCB8Kc/umxA==', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'd48edd5f-9aec-4330-b2af-eb1ab8eca13c', N'eLJb7weHyBsX2HpDNLQilKwCTx2+F5ohIVSf1atJ/z+hZKUEgM0=', N'JiaTMrmjqA+9qVMYSsWPp38E1h+WdQcHHsSs5KJiQLMlNdYw2MCPyNLfdmALNw==', N'cMcruwH1kP0EQvOKIJTRVCvOTK79I4G2Q1J25YDaecl/uy9zNA3wkN/kRFgF7w==', N'PkCfykmAp8L0CgyJONKj0kahgYQe9hqXaIDcULnkjwzXvZ6v7RMScLrTck3AVbloQjf7', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'9627fde8-9044-4810-a30d-ee4b27bb71b1', N'xotoAl9vdpSFcRGSMkVS1QnoiLhi72Y1Fh0VONcxrdxj1ApwIqHHOr+4', N'1Ue1QPlEPSJatynm+HNZsPI00g+5QPMs8GDA25l2zSR+QCtyTEMaI8w4ySB9TStu', N'DqmSI8qZ9/KsmCHOQxIvaVjrujx9rQ23wAzpkOHzurxuj1NYNCztsw==', N'6NUPpip+XVpkuNZ0TEYpkQ1xEJ95a5O3yulFbE3Q60qQOE93FL+bK7e2zu1hrHMn', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'a9b6a944-bbc1-4c51-929e-f17740a804e1', N'ofwx78qX6YcA83QGmQxam/CEKZyDI8LF/ezw/Pr2Q0z1IMvk0B+9OA==', N'MkeGnsRpgtq/T2M/FaNMcfi5LHFioHlujazbXP13EbKAwGBeHBJzCGVR5A9K6Q==', N'B2PWqj0MeEFV3vGdSf8nM7aeSQQA/6x/mdHnBFNol+GJeR5RDpQ=', N'n80dwJMx3xlZEJX2aHcMrUO/skKUVSEOt/Xz/thmblum86B5MSgSe0hkMDjf5Zw=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'59369f8d-32f1-4d12-aa19-f71011262fcb', N'k1wuT0RpZ0JsZ/e8HBsIxDC4vwgrvramIx3/l3LxUFZ4Mv1rFlQXFw==', N'7bmA5R+8kKeslOuWLqGwKdSBcv5eGX39TYImKtEsXf1iJEuYKWMUfh0WpuHUBKyV', N'Y03Y3buGohwxEJNXB9HqWCCxEp7J98CdxkOd/fRYmlhcSdRYy/gMma7AstXBgg==', N'YpDaEux2ryJYjrBKrLmm9KjRzh3QNZnMkA0STMZaOLfCLjJJK82VDth9iuSoPJsujmLdIcE=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'963da7aa-50f6-42bc-a2b7-f9d222789303', N'8rzMIx9f0+iPq4984R9+iwqXJ9gb9wwk/0wDgFiSvOpirGNp1DQBcyqKlhk=', N'+hVEIR+cQc15/j0aE0T7tFyIbAp7BgI3KQqthnNkIz6667qk9ajrjQz8yaMO5h3Pwsz2gg==', N'WC1eiY3304CVTdWbyuC2Ag0nL4W0L92P3R0Meg3PZNyQ/JSZ3C2qu+PNeLs=', N'vX6sOpXZeVgwedkqQWeLIyoprP7Gv4j/fIghB9iT+9ifE03qBhRhMVVb/CrLNcysXfU=', NULL, 1, 1)
GO
INSERT [dbo].[Users] ([UserId], [LastName], [FirstName], [MiddleName], [Mail], [Password], [RoleId], [Status]) VALUES (N'c6747902-791d-4a61-831c-fa988e5c8583', N'WmoNdEYmli4zCsj0Tt4rFN4lwK6MCdl0YNPUl2x4+qaTSFQUE9mmNyLoweI=', N'eB86oZ9q3s40m96ojOFk5VLrjqfSyFOB7m/VECCu+yx/Z2hG287Vo+Ph', N'lUPoq03O9qnrhkD6mdWGF1oZpujD6Uca9FndJ+crYKP/zdNb7URl4p1ieRu8OcE6', N'wY0PUrCBxG0qmFYzGjM3N0hP8O0H5bQl4hpdgh/jY6C5MmmTCtT3PS71Lw+18XAuIoo=', NULL, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Variants] ON 
GO
INSERT [dbo].[Variants] ([VariantId], [VariantNumber], [VariantText]) VALUES (1, 1, N'udiT3YAlbnImew8QGjCkXaokfhipiZZaYjwzRMcknAZ0vN6yv6C1q8wA3pwlVMqS9jt5+Z6dO7Of2zY2Gc//fR2bH9pCJLonuGNEMjzqW0lXiN6zyl+47Lj0A6PQSAIADNjSp6ku7zzvAWO5WrSsqxWPEMAMU8QJn4zmbjuV2iPsOkusPQJNagwBYXD92dGi+k17m56pyVObzJ67UAICx1Gzs1Nf+geVn+qH+DDwEUd5rqX5H2cFT4+vHQLq+UysDELap7YgGyCr3Ce+UqQePjpd5F99kFyI3LfLmsHLaeTlUWJKeFvPW2d0nmsohrY7up9RgAeYx1SxGCI9Jo+OiXZhwj+Ku8F0mEUoUoW6jIzMmBBdbonZ9A1/V+93wiQMpCzya1icWH4yEUK9s7FIXzsw/ZiFMmnHGfRVnUWPe7oAErktl7Eoj7nHu2LkbXDLpj/Wr1sgBzgcxT2KKUaAiTG8Bjlhx0DXVW5zQrS+ELNXtiv4UoHw/6NpjCt3B+wX3gaNRtcktTEQwUf0BjmSVj0HK7TRcFMpH/LsGzg/hgV+ShSALpDGt5eGsX+CpjbmpqdsrbYxM4t4wQ/af9ZL+XYx1YYWPGUIibw7klF/QXfyve/rP0pPG6xcpBaDjpH6LGQuXqNxVFwxTCcjCBC2xu/sQZ6LPZ+jwgKohekMWpkSYhf/LwVJYuqiF/yN7Fy9FZaFsS76')
GO
INSERT [dbo].[Variants] ([VariantId], [VariantNumber], [VariantText]) VALUES (2, 2, N'XqIsmrLAgf18fie/K339yBmXwrwySz2cYB8EgAeFV4XlbpjA9W9pLHdIHHolkiRlgawLE/27VSUk+VaqWisvDsOc8hjMRa24Q54uCb2bD4GcrQx9DU4YypNT3+MIQKia3r8NGnk3xw0FitbK7QsfPlMN82cUOHJFgwwo+TjCRyPWbvLK/0tcQ2hroVUnToHfYhOIZB6EWQs4Pea98OFtl/isLo8sODpZ2KYmufWm1fARPKHpwbMp5sNxoj3HyaK8FLMM/CTmTaJkoRVhjL+x9Nu0OMMk5kd6oVvDF1G4QCu1P0dShkS37ZeplSj7+G8e/NOtAHrkna+oBsbrvrf+/GhngMdQIjP6f5jaotUpGNSHGLYxkVHPBggZ5xiGc3rAPGJqnVR0wozn1kDUJgVfFPujZtIJm9knKHubVAQmWMiwalxbRI53q7ZYo1abSH1ofNnBh040lsMYyN69yhKRjrjbCW21Sgco9wxLNOuO7xZZPSR2AhL5/kUSQBmWxtLbHeTwoa1LUnBppmKz6n99DEupaoYXTl1eGJZhFlCi8TVSKCZIxDWB5Vdfe7B3i/30mxOkWf4wk0Lu0C5OwFsuaB/rMmnx')
GO
INSERT [dbo].[Variants] ([VariantId], [VariantNumber], [VariantText]) VALUES (3, 3, N'Pl7OY1tZU7nTuIQ/4qYty6PyzgZHj8kIPACMaInQKWxXlBL0c536k5UT2Vf9QqO6VRoSgYBpvoOM14Ien+Y0hnAqQe+0AcBEs/q0qMqA4u3h/XWyMa3Yz6MBXw8Dx6+fa86eFpa/JxVLHZF5a1rLZhOnTvIvD7hi7vsVD/c/gQBXaUk0cGjYIOfFhWlr1O0oS/qBrUGSWnwcJOflHSHCcCbSMxNgZUP6FhlDFEwBL3fOnmHSO0FFMMeCLb8KphcXensvhQYtbv48b3+LtTAvR9hdqiWorM8and+vq98ndPT8ROozYyYDBgo16XIdKsBZkUYK5kGGrgQrwvsKzkYIK4ESDY/rIXo2ASefI/OIs5rDSE6r+3jT2W/EzTbx9Uxq1MNsq+Z7ftAoH34AtwaM1y7gIUjYju2fICCGJzr8EjY+7/ElkHnKyxeBI69ZsxtErnHmRiBjxv1US6UcjTFm+/Hb79Ybr8kI+fSNgrJ+eOwuz69uLj+nCZKle3ie8Mr6ZBkUSzAs+hCKY9gEeuiKgQttRi+0tZzuDKJxvOFRhY128b/AytJH/wRp/iabfUdHgUM8IYzYDVnGQV3s7vojVe/MdDKVOeu87n6lHHjFdfk6dpKk5FRf7i2DnwVBIjV01VeJfryPw34AihYdW+99D6bK96oN3C3lHz01XjyWz/3joDPpve3+/XW/dx/dlzC2Q2FruyEyaw==')
GO
INSERT [dbo].[Variants] ([VariantId], [VariantNumber], [VariantText]) VALUES (4, 4, N'1m40HUz+C23ongsZeYXnyOwcw2ZqS4OpRSYwRY37VXqt9+6+0wv//9ocrZSRV/ZxahZcF4fNX/nu2kB+Xbw5YE1CAQsTfrVHawn8Ww0J2fJD9gWEGK1c28FBWs3fi+ifFVCD1KAR1Nh/FYGh/CLST5WbWaB5lw/F1Vt69oFs6HtCT2KKKnN0RqWAiNPLs2NtfmRY3wsEhVG2zDxseuzA+6p/piqCy3AbfY+ph1SihGTBHpwTXS5r45Whkk3K1NY66arKJdJEPEwdjZjmOHw4wmrVC7VqTDlpVg7QmkWDug6L9vPRlVJmRRJa3ZZjpsC5gL+Zi5+mkS0G0Zo0bYI7N0sOLl5sjYVDKpEID4+W7rb8TVwTq5I4H2Nli3S+TR/SDja0T5oqp1BiAFXvTcLtKBs9u/Oa0Wu90ij37maz09BDBwqkqmuO9JXOfRaxpacDQg7r2UFTCdwFwbpjsMvsllBzq1+rOEjGVu3dxn/hzq7LiY7W2Ai3Q/lwk7bQ5GmkV/F6B3t8TZ1PorzxK1EAsZ+3fL0NVljuVUlz9gyiDAs+soi9SBE4LcSTfSzhQ8zU/G9FzQEbKnvzGcAbfo8SSKgciR5mX0tDnVUrErSCoKr2RmFGoSV/MLJxJpzvwHQVSS7z1lG10AYpGeRui41Ov6OvWUCHyvYNQ7kCljLbViSnwIGHLaesI75Ru6PrzFPYEb+D0a7opxTxCyYqrZ/D0f3aaH4YOcDO9pihitQP3dcCd+wY5VuLxJaTlPO78IJPZ8P7n2oKD29dZh3ms7OcDA833+SmhOG4018wDmLg')
GO
INSERT [dbo].[Variants] ([VariantId], [VariantNumber], [VariantText]) VALUES (5, 5, N'rI3daUZU8d1RVEvDWAA3tA4E/mkEITs8HfXHFCwlmi2/qcDslrHy1KE5PeoGOo5jbrUpgOGQG6wj/bGVAdzkAOCI4mXGJnogS9rBvYf1IddX+G5aVkVLils7IhU8mLqNSqThRFrs7t9wR/dnL7FgcTdLviP/1Wd+z09nluSmwIxuytVcXgDwb1Ne2Q738kep4XS7QJJ9+F54YMyu5TX8IP4qXknr0/aUghGJan0nPqjmHJPGg8X4EnBKoZegF8/UxKxkTm3bUuDtmGCvys15GhmiEQQeO6zDTAjPvPb+lN1T4gAOLcp+9yPagsmedkPECJtJpNeIUBk3z1S0Sfmkpm+ZWZbBSRBp9PDKiUEoePObquCCucohWe8YvbO3zdiW+rNHylIPbLqbtTwEWLeCm4zgTVNmzfZ0KzaU5bgSnpLxauCDr5uy2NcUD6ycPCnN8Ritloi0AyrocHVI0v3MJABE/8KfwG7cRZO6Rf69W5wj0keuIjLqGqkU/8FY91jlom2dHMusb8RqbnqbKYfcB6DXh2/8SXhI0o+RtI7a+wTxNi9xLqrJp8ZgaebPVox+U4JgS6dEdnu8xxR3uQw5UMApv2tAydqfv9+GbmZFMPLDUuzrZGGrkFinpKCaVEmRLhjxOZDGyTOu')
GO
INSERT [dbo].[Variants] ([VariantId], [VariantNumber], [VariantText]) VALUES (6, 6, N'N3m8szv+Mo5vT6ClWxWJ+dqaWDOmru77Eqe0BpNuXRGaXxBQwXCl1sFJVDN06B1ZyhKozO+nbId1jE0nGcPnLIMOcJg+3hMwIcKEd+19ebrfBRXBPmuGQzgCNyzfJlX2qDLaKLFg2niedu2FrFOpPc63zyNY0k2jnQsg/58dnj5RPIkMpo9LZoGPIx2UWdyKN/55EPgTx2lkVpwhp+lIZNZtjEQgMmBG7Sx/M6Bhf6cKlkfVEMCU+wHj0OCsMOtLgb5CVB6LyFpmpmo+855/xJdN6o+HRPmIdeL+/8jrm7kybFwy/njZWVzkhfNgDMJ0IpNr4UB66+/9t0eY20k+c9FWDlvgjoq2tzvwMJ9og3zXiuR0/LFAyy+Y0nXBkk5NP6fGKvXM0IlBqfXiNsuhpqaEESIbbv7jsOgD/v2nYIOaRKRAArRHk7ocLtMolYc3SE6Ui7oU1pVqRocCxNCITJ+v2OrCGx/7X1+B0HBXxEYKu7e2TZ1jqWatIzEBCrL4tinYfQyk8qKBh4HFu8ExJ+KhAn70tyBD6LTdiWGeTqyqhTHwpFPfrZdqPzEkMZhjRcOjnoQ=')
GO
INSERT [dbo].[Variants] ([VariantId], [VariantNumber], [VariantText]) VALUES (7, 7, N'6xQMgEHQnirdKarAG+U38S7HpYEv8BkpDtckzmDCZ0g3u888mkbmwBeY15GyvnWYDdm27sHAVrACIxpeDiDZQel6Nob9e1y9WXopjOHa0pb3M6z6UPKq9gNDSEgWPt3jWB32w4QxHAMS0LfbzGtiOCKi7eyQbJCOe4EYwCA08VWtZZPuA2w1arX68Kyc2VQmMWDPoJZuJa77GbrNRdn//3DeZMn8UVF3DqECMeY2SXVup6+3Pb1gkmtGo1OsJqzF/tIgsHlRpr5AMn3wCXhovW/p6OFaCbGAP0IYm9J0G+oM7bzNH5Ex+CZtgRBh8163Zf4lwv/GRQwtdQ3xQVuB/4265v2NKSgdfp1RGCc2dKH23Fb9ZLBZFI10N2oXwBKRdU6Dxe9/447KpgveBjvYr6i0CTVJ7e2Cj45yZfwCp+qaW9xwVc43FxNYuC84/5jzLzjPPai/J/kg8khQFvr3BGafP3fHZZrvCTyv/rztO9Vt7/rs6R3rcVZxCZpdKzGvkVLTgKSN7uafGiJLphRnewd65g/Kz2meaOBFlIkTgInl2ra2ziVIAkL0CkBJxZmu')
GO
INSERT [dbo].[Variants] ([VariantId], [VariantNumber], [VariantText]) VALUES (8, 8, N'4WPUYZGrM227Aui3FxNvsHttQEkNxGlCDvdpPeKdmZkdj2DPgwGKVm/5Vdckg60gfmSfN/p1CbGc9uA7MgnT4Pq9XkIgBFRiNT3ffRrGw662d21brMDe4WUiYY5j1ktEKGVcqFKnrC15LSHupxCjZmE663T/7EOr3CMtdUDMCzDH4o+RXnOifsmTRZtaE7RHltNReiBmVtbzm5C+0YOmOTKCS8jpMgzjHyY5UiqM7tipKchk5AqwpKkTaIO9Bw4yxwLhoKsuKMfhTlQZ/xlgiIRQTTyBuFsMA52aaTnrMakyOApFQZZkdVksqa1njcSnls2G9H2lQ7EPXhyQy+zdXORSLrcamKZwQNzgQIkNgunmF7CF/HMYWW7zoOKgesqwZPi7JdlTYhnqh0loSrRLW3AxLD6IoQ9Y+BNAvR0KrnJ8HJCoFbQRa6mB/3A8jZtZwdMWN5Y/KOkuwiYzCMCg6bCm2xOmgiYVEslxsAuX+2Fqlie+K24OPi96A8VzsLBhByBQXgzn1GgZ5hlxwjWWIK3RodgxHuQvs+qOkkL1bdVlRJpv6DgsYUrxnKeMdP0cV6atS8sLJcJ1EMQsHh+sP3BYD9otVEgt5JHwSB7Q')
GO
INSERT [dbo].[Variants] ([VariantId], [VariantNumber], [VariantText]) VALUES (9, 9, N'kP4GV8aKFRjNMP85gyjbwvjXDPb1fdluZaAQRRocnSA9BYzZ6YMPMPT0i8pfRVe12wYQc3oV5fW/9+/sjbgI4SAILU+UrSV6Xw4GzWQ7zf2uFnyXVOwJAcKCJgtehx1coRjnAh/fgsW1Lh/a/VSYq0Q1vCju13fYfJHjYMU8f8h3xBrYOWQhYstb4AwvYPfEyhSM0PsK+uTfnc0DH6yWjDNSibFI3UYFxiPJyq24BNPWK/VM9j/57Bna2gml6tUIah/VSmQtfo0u1MHUctNjbyWgY0adbBGs5XUQgcxwjIbo+/nw76j7W4oHAkg5d19LgRHXmRDGvf4S4VIDaRh2flBF275tf+6vXAAns1pu0hdgbnLXt7usxwGTHLdo79fVJuqKIvbKy/RRD65x29IwW9q2MYPDM1Q8rY30SnGWWbQXcE5mb+HbEY+1UHonuWhm71tpR0IUiBwWdO/S4yrk0kpTlEwNOhXFQkU3/M38dgZjJI3UUmE9yalZPE69n8MDFPwdqAakhlxb3PFd7XBVONkE4JaVdhZ0ht8+PnuxEGv71KlEB8tD+sJ14fDRWP83UeRQRyZHhjJR9F4ZhfawJta6sKvBO8qdbFFeTaqoI5hXh+t06uP/SNJmky01czShLGCS5hNsn5OnInCR7kIjyr2Qu95VIcVy4djNfDuHI/OSJzEYtA8BJ5fjaiTlPZKJb+Jd70cT4JxI+C2p70BJOK+6FkW2p29joeYZzGbz6UNo8dQYbiOgF8ZlQAMHki8h8cCwaTsFqs2wxzBbaq7uQjYQpXplWtZ67fMQ60vQ+XmWPrjGifT2dms1jtGOIUTvepIPhsoJa5zw5E7SYY7s9A43JTrt6cKIkw==')
GO
INSERT [dbo].[Variants] ([VariantId], [VariantNumber], [VariantText]) VALUES (10, 10, N'ZCpYbANKY10RpNxQF9o7UaNM0DUpcEmMgSF0KykzHRwYHENqp3CWsIanPaBb0fRZhewFRrc2WEwNNzj84X5FFAiSLzjB7r6FzTbi9MN4aFTLXvqOgCPKOacumw9gKrdE0PkQG+65Z0FHB901thledaQHek+1+v8TEGX/NGz5tgaQskZAXgrRgY0ZQIxAsS1XZJSa0laxHfk9o5IqeNCsfiPF0wac2UUt+UeI5ymsRsZh3wbBknF3YizExVf27bfRP+BJADXC2SzPyb2whl0yg0wHcq4JHR8hrG2j9WKTcK5Q9ZT9BtgHfqTj51FW7Wqo8uGZqTa549NXQeoAVRrvlgRRQjrSTVPFF1owmVjygIHNJphc8UhtynrcwfHkvMUHZtWN/wxdn891s1rlaHTVjLBREUt+g1ew1SHzqa1R8zFl8Vby3vdmGDiuWV1HeR86mnbCYPC3187snqAWTAdUBYieK37EtjmuuD3THdvSaDHKxrlAgFHuqavSQ8Oyx6x/YeijG1VxoLSxh8OT+ictTP1UjGHnMXq3KP305q0OoQBFbV1QjOuNlhgPjUE1')
GO
INSERT [dbo].[Variants] ([VariantId], [VariantNumber], [VariantText]) VALUES (11, 11, N'Cf8q5mHQgrpWQs2VjZt4wdA0tiugr16UEmNJiZtJgTdRAsfz/54iu+eRnnpTB5Kp8h+WSyE/zCZhILrT05eRm2ixRJXq400xZil4YJagZBJvikUxT8cM7lYFr9LiGEk/VKjJZYJd4911lXL8SGddOYHDMZQv7tyIKZ9JOE6p9xDSp5Hl2GvrpfAMejcpuXcUI2bn4vVv/U5zoXa50w+DKBa1xjzpo8nH31lTAFbT5OS1PDqmAxJBqdcQmwr4x4wKLL2BlUu7ZUKhEdra3qbzZLBnqq9kSZxpXfcZxji5sRnv9BPLxofWYrfMyTGkxa9V+AS7nrgpla5TLHQbFq7i1EoCLLhoV+4/Or4WEUrE3+YlvZdM6+GZ4ltEg2HyGrdPt/qdoX6B0OZY7KbI6UEicmNDSwY4RVHJBZgHNBq8s3LfKD0uH3UeLVGiQMsrawbHxTCetjAMuAwW6hFL0LKXc3bzgpghw3b6yDWvdlfp9kE7gUtqgmVGXe+/YG0yleOazrwAPrQSpEPZxSTisAG1OcDb0cCFP/NBbSplm26iPKitti47/XyI9jW7i1gX2KubumU0X/1YneLF9B0b92Vheg8hGHBGkYvudFcSkHORoWro9gdBEzoMeG6/Ecy453pxyGFfhk4mfIkvcRaEbyLMksd4do/moi+GiKHoN6VPP2hFXXZcsiK77K2tq9SxmOAk3YI=')
GO
INSERT [dbo].[Variants] ([VariantId], [VariantNumber], [VariantText]) VALUES (12, 12, N'KNxtqQgA1D/ST40/uszYsmFSLIkuFeSO+cRC7VEwAKeS1M6X7dw6Z7ch0HJD4alCwETIJHsTdXv6G3TZBMsAoj1btgZ1zLt1hEVd51JR3I0Si5U3X/jJbKt2vufuOAhNrqPPoHY+HcWiN6eh8VQxI8zdrMEm0PvLTmCxhEYCeD6k7wWQOqefAOC8aHkOfpLNziOmO3aWtmsGzAAObo8LLhjf+hYpSzzIDLO0ANDIEmeelfS+dZsaRRBuenjbwmkqwBHWFhaFEAwn778yhEBqp+1Yqma5MTm98B+A6UtQ3fMhjjmbtroFle1fmunxaMgZtPqMQuAkYPZ7fEgYW+Gq2up9WVebit8y8bCFPKfEXJZNmWF+Op8/Ij6Ea2Hkssta7bpXzgkr+HC600E8EnbVd7b3N66PyLoOd8C3Dbl2JwMYX+nBDSmkohO3s5cWPEoTKI4XoN9ROyKD+wEdtnhjdGfPnoo2gWDceelxjZDGL5l8GewG2P5h8M5hTwNMe2nAfhfxo87suEM/w0JP77ULzhepeP65G6By014KuRjHATqB+iiaOGMDAsjnPSIha4bJm5c0v6uLeOqVdL7p/RJ5MOyL++57ojGh26Z3DBwkhXtYjOz6fJFc4nRF5cD5yI300Srlyc8F5ynIgRt3CI/5LjBGUmDBvgbODv9NZoyFdpYAi+IVIq++e+reqBj2LyvLEmbAksnU/rFuwOU3+dBfrTtk3kVH26DLBrIF')
GO
INSERT [dbo].[Variants] ([VariantId], [VariantNumber], [VariantText]) VALUES (13, 13, N'VIbGuETXJndUcDsEW816riTaZdqbhj/NbZCXk+TRwJIaNVbm7/vZ6Z86rK+F3bAUkH/Woe/SvXBshRQI29sBryRgXD2xNLsVPTH4ZUMVdCrQ80E+ur8X2/c54PCko5uqV+QAD28FWXJu7rvyY+xw0i63tnAfomNKaY+gF+zzfQ7jMdzviJOVt79Jo7Yi3FZwvuaML2GSdDJfvHPevao9gE+B7Cwc/9G3VxGRL4P+i7gU8VLCxc5/6HHOhIf11Ic4yy+bJokp8V5SHbSkCEGt5zEL2I/MVtRQqb0iLDZQUnfYEb+ihHSYIBoMrYobG4hRSmLZLrjaPFat/b5Gh2wQGHd4QM2t0/bobG7UdtYxi2Q0uOYcjbXTlVtyScB6UL7BWII+Hv2YSr6GlZTMpeHPhC0ZzF2dQGyCJZ3kIm+tUV+oHy7zpqJ6DkwztpIGYJIKmy3Kq7Mps4+R/pyFOP1jIx7keUiSqEXRUyYEaN5APwf9LNznKHpPjmNtKladJO85viz7m7vAO7yrJyZmaDt/BxfoMq82Zx/8sro2Va7ZeKSVxZ3gX78u61Cit3eQdn7zYLtFssybmsmTiFcTQK0vWKCNeocBbE5LYYt78RWI8XOUrlI50cpDqxcI+wr8rloFvI7aW7HNfpplQpwAbcLJbvFqJFSWlxkafDFttMF2gNYIeaF2OSa9tI22evVhhu6uGjGZlc0hwzC/tqDqMzpLg7NCPkdtBgnTxZxRYEBMyHSvhRauVSYs1EpEXrfwL4W5Gimvp/NyEFfExQgn26kackC4LmNmxS5WkkwB4SRR1Hiciol1RxZrSTo3nrV2IYqzQxHGtX1HSQXBeClosWYAhw==')
GO
INSERT [dbo].[Variants] ([VariantId], [VariantNumber], [VariantText]) VALUES (14, 14, N'q95obcg7CbSKx2qQuKmouAbobWxDdTKMv1dCIpVsr2Wy01JdHf6FyHC2fSkCGL1LeXhx44GDNZL6bWhswm+V9btPSQ0+s0yd22Usb/Ue43QnTwHdjpow+mT2Htg+/cTW6nVkD5KVeJbCZtDTirj8JGSJpqSVsHOxD5gEI36iv2jJPQmRvvOl7M1O47/kWsU13SoruoPTUHfptoFhtODtYo9Q1JVuC6KH6njiZP0tc9YnTpATQUExY0+KA0pTa0TgfCpP2U070xAGpchgWwR944t+vgx4iIAkb10Ppj8a7IN9AqcCKbq2iE6k+Io487XiQaGW0S9poH6snETXWElOlG2JcKT2csoT9cy9RzzSP89pcQupyWrrAdxwredRIwHGpxhWd3RFUS3FBOXDsNEabhSKhr2LBEdTMe1Faxkt5SNc2ByN2Lq/ZuN4fITD0em6uJzB7alTxDSjKiZtJrRJ/rOPp/m1T+ZU1qeP66D6TanaMfr0NYVlxnTX8VbeJPRc4f0AHYVlKOacflDiuGUBequJ7Ff0LJN8DsvkUdZQUHTmZ7Haj6FV4Htrc4yg6XPoe8sE1hTfvcKYxI+iSzR4UEMniKdqvO9Vz411d6Y20mXgwV5CIGQMn4B0OxcEBtBwpBEB9FH5jS1wBPY516EYdivKB1J+71h1NJYC8nVsY4ytw8rmRbrSvnrQjD1+JiGhccaEZExAeeT0T1vXaPIiV4wZFLy73itfQdiR1wjC1YkrNpc8ccznfgu6y9rS')
GO
SET IDENTITY_INSERT [dbo].[Variants] OFF
GO
/****** Object:  Index [IX_Exams_DisciplineId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_Exams_DisciplineId] ON [dbo].[Exams]
(
	[DisciplineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Exams_OwnerTeacherId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_Exams_OwnerTeacherId] ON [dbo].[Exams]
(
	[OwnerTeacherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ExamTests_ExamId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_ExamTests_ExamId] ON [dbo].[ExamTests]
(
	[ExamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_GroupsExams_ExamId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_GroupsExams_ExamId] ON [dbo].[GroupsExams]
(
	[ExamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_GroupsExams_GroupId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_GroupsExams_GroupId] ON [dbo].[GroupsExams]
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_QuestionAnswers_QuestionId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_QuestionAnswers_QuestionId] ON [dbo].[QuestionAnswers]
(
	[QuestionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Questions_ExamTestId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_Questions_ExamTestId] ON [dbo].[Questions]
(
	[ExamTestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Questions_QuestionTypeId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_Questions_QuestionTypeId] ON [dbo].[Questions]
(
	[QuestionTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Questions_TopicId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_Questions_TopicId] ON [dbo].[Questions]
(
	[TopicId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_StudentAttempts_ExamId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_StudentAttempts_ExamId] ON [dbo].[StudentAttempts]
(
	[ExamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_StudentAttempts_StudentId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_StudentAttempts_StudentId] ON [dbo].[StudentAttempts]
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_StudentExamResults_ExamId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_StudentExamResults_ExamId] ON [dbo].[StudentExamResults]
(
	[ExamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_StudentExamResults_StudentId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_StudentExamResults_StudentId] ON [dbo].[StudentExamResults]
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Students_GroupId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_Students_GroupId] ON [dbo].[Students]
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Students_UserId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_Students_UserId] ON [dbo].[Students]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Teachers_UserId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_Teachers_UserId] ON [dbo].[Teachers]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TopicsDisciplines_DisciplineId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_TopicsDisciplines_DisciplineId] ON [dbo].[TopicsDisciplines]
(
	[DisciplineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TopicsExamTest_ExamTestId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_TopicsExamTest_ExamTestId] ON [dbo].[TopicsExamTest]
(
	[ExamTestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Users_RoleId]    Script Date: 21.06.2025 17:06:10 ******/
CREATE NONCLUSTERED INDEX [IX_Users_RoleId] ON [dbo].[Users]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StudentExamResults] ADD  CONSTRAINT [DF_StudentExamResults_TestStart]  DEFAULT ((0)) FOR [TestStart]
GO
ALTER TABLE [dbo].[Criteria]  WITH CHECK ADD FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Modules] ([ModuleId])
GO
ALTER TABLE [dbo].[Exams]  WITH CHECK ADD  CONSTRAINT [FK_Exams_Disciplines] FOREIGN KEY([DisciplineId])
REFERENCES [dbo].[Disciplines] ([DisciplineId])
GO
ALTER TABLE [dbo].[Exams] CHECK CONSTRAINT [FK_Exams_Disciplines]
GO
ALTER TABLE [dbo].[Exams]  WITH CHECK ADD  CONSTRAINT [FK_Exams_Teachers] FOREIGN KEY([OwnerTeacherId])
REFERENCES [dbo].[Teachers] ([TeacherId])
GO
ALTER TABLE [dbo].[Exams] CHECK CONSTRAINT [FK_Exams_Teachers]
GO
ALTER TABLE [dbo].[ExamTests]  WITH CHECK ADD  CONSTRAINT [FK_ExamTests_Exams1] FOREIGN KEY([ExamId])
REFERENCES [dbo].[Exams] ([ExamId])
GO
ALTER TABLE [dbo].[ExamTests] CHECK CONSTRAINT [FK_ExamTests_Exams1]
GO
ALTER TABLE [dbo].[GroupsExams]  WITH CHECK ADD  CONSTRAINT [FK_GroupsExams_Exams] FOREIGN KEY([ExamId])
REFERENCES [dbo].[Exams] ([ExamId])
GO
ALTER TABLE [dbo].[GroupsExams] CHECK CONSTRAINT [FK_GroupsExams_Exams]
GO
ALTER TABLE [dbo].[GroupsExams]  WITH CHECK ADD  CONSTRAINT [FK_GroupsExams_Groups1] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Groups] ([GroupId])
GO
ALTER TABLE [dbo].[GroupsExams] CHECK CONSTRAINT [FK_GroupsExams_Groups1]
GO
ALTER TABLE [dbo].[QuestionAnswers]  WITH CHECK ADD  CONSTRAINT [FK_QuestionAnswers_Questions] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Questions] ([QuestionId])
GO
ALTER TABLE [dbo].[QuestionAnswers] CHECK CONSTRAINT [FK_QuestionAnswers_Questions]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_ExamTests] FOREIGN KEY([ExamTestId])
REFERENCES [dbo].[ExamTests] ([ExamTestId])
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_Questions_ExamTests]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_QuestionTypes] FOREIGN KEY([QuestionTypeId])
REFERENCES [dbo].[QuestionTypes] ([QuestionTypeId])
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_Questions_QuestionTypes]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_TopicsDisciplines] FOREIGN KEY([TopicId])
REFERENCES [dbo].[TopicsDisciplines] ([TopicDisciplinesId])
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_Questions_TopicsDisciplines]
GO
ALTER TABLE [dbo].[ScoreOptions]  WITH CHECK ADD FOREIGN KEY([CriterionId])
REFERENCES [dbo].[Criteria] ([CriterionId])
GO
ALTER TABLE [dbo].[StudentAttempts]  WITH CHECK ADD  CONSTRAINT [FK_StudentAttempts_Exams] FOREIGN KEY([ExamId])
REFERENCES [dbo].[Exams] ([ExamId])
GO
ALTER TABLE [dbo].[StudentAttempts] CHECK CONSTRAINT [FK_StudentAttempts_Exams]
GO
ALTER TABLE [dbo].[StudentAttempts]  WITH CHECK ADD  CONSTRAINT [FK_StudentAttempts_Students] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Students] ([StudentId])
GO
ALTER TABLE [dbo].[StudentAttempts] CHECK CONSTRAINT [FK_StudentAttempts_Students]
GO
ALTER TABLE [dbo].[StudentExamResults]  WITH CHECK ADD  CONSTRAINT [FK_StudentExam_Exams] FOREIGN KEY([ExamId])
REFERENCES [dbo].[Exams] ([ExamId])
GO
ALTER TABLE [dbo].[StudentExamResults] CHECK CONSTRAINT [FK_StudentExam_Exams]
GO
ALTER TABLE [dbo].[StudentExamResults]  WITH CHECK ADD  CONSTRAINT [FK_StudentExam_Students] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Students] ([StudentId])
GO
ALTER TABLE [dbo].[StudentExamResults] CHECK CONSTRAINT [FK_StudentExam_Students]
GO
ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_Groups1] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Groups] ([GroupId])
GO
ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_Groups1]
GO
ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_Users]
GO
ALTER TABLE [dbo].[Teachers]  WITH CHECK ADD  CONSTRAINT [FK_Teachers_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Teachers] CHECK CONSTRAINT [FK_Teachers_Users]
GO
ALTER TABLE [dbo].[TopicsDisciplines]  WITH CHECK ADD  CONSTRAINT [FK_TopicsDisciplines_Disciplines] FOREIGN KEY([DisciplineId])
REFERENCES [dbo].[Disciplines] ([DisciplineId])
GO
ALTER TABLE [dbo].[TopicsDisciplines] CHECK CONSTRAINT [FK_TopicsDisciplines_Disciplines]
GO
ALTER TABLE [dbo].[TopicsExamTest]  WITH CHECK ADD  CONSTRAINT [FK_TopicsExamTest_TopicsExamTest] FOREIGN KEY([ExamTestId])
REFERENCES [dbo].[ExamTests] ([ExamTestId])
GO
ALTER TABLE [dbo].[TopicsExamTest] CHECK CONSTRAINT [FK_TopicsExamTest_TopicsExamTest]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserRoles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[UserRoles] ([RoleId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UserRoles]
GO
USE [master]
GO
ALTER DATABASE [GradeFlow] SET  READ_WRITE 
GO
