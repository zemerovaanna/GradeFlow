using System;
using System.Collections.Generic;
using GradeFlowECTS.Models;
using Microsoft.EntityFrameworkCore;

namespace GradeFlowECTS.Infrastructure;

public partial class GradeFlowContext : DbContext
{
    public GradeFlowContext()
    {
    }

    public GradeFlowContext(DbContextOptions<GradeFlowContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamPractice> ExamPractices { get; set; }

    public virtual DbSet<ExamTest> ExamTests { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupsExam> GroupsExams { get; set; }

    public virtual DbSet<QualificationExamScore> QualificationExamScores { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentAttempt> StudentAttempts { get; set; }

    public virtual DbSet<StudentExamResult> StudentExamResults { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TopicsDiscipline> TopicsDisciplines { get; set; }

    public virtual DbSet<TopicsExamTest> TopicsExamTests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Criterion> Criteria { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<ScoreOption> ScoreOptions { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=Home-PC;Database=GradeFlow;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Criterion>(entity =>
        {
            entity.HasKey(e => e.CriterionId).HasName("PK__Evaluati__3214EC0725107624");

            entity.Property(e => e.CriterionTitle).HasMaxLength(255);

            entity.HasOne(d => d.Module).WithMany(p => p.Criteria)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("FK__Evaluatio__Modul__398D8EEE");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("PK__Modules__3214EC077EFCF48A");

            entity.Property(e => e.ModuleName).HasMaxLength(100);
        });

        modelBuilder.Entity<ScoreOption>(entity =>
        {
            entity.HasKey(e => e.ScoreOptionIdId).HasName("PK__ScoreOpt__3214EC079336F92F");

            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasOne(d => d.Criterion).WithMany(p => p.ScoreOptions)
                .HasForeignKey(d => d.CriterionId)
                .HasConstraintName("FK__ScoreOpti__Crite__3C69FB99");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.Property(e => e.ExamId).ValueGeneratedNever();
            entity.Property(e => e.OpenTime).HasPrecision(0);

            entity.HasOne(d => d.Discipline).WithMany(p => p.Exams)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Exams_Disciplines");

            entity.HasOne(d => d.OwnerTeacher).WithMany(p => p.Exams)
                .HasForeignKey(d => d.OwnerTeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Exams_Teachers");
        });

        modelBuilder.Entity<ExamPractice>(entity =>
        {
            entity.HasOne(d => d.Discipline).WithMany(p => p.ExamPractices)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExamPractices_Disciplines");
        });

        modelBuilder.Entity<ExamTest>(entity =>
        {
            entity.HasOne(d => d.Exam).WithMany(p => p.ExamTests)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExamTests_Exams1");
        });

        modelBuilder.Entity<GroupsExam>(entity =>
        {
            entity.HasKey(e => e.GroupExamId);

            entity.HasOne(d => d.Exam).WithMany(p => p.GroupsExams)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupsExams_Exams");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupsExams)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupsExams_Groups1");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasOne(d => d.ExamTest).WithMany(p => p.Questions)
                .HasForeignKey(d => d.ExamTestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Questions_ExamTests");

            entity.HasOne(d => d.QuestionType).WithMany(p => p.Questions)
                .HasForeignKey(d => d.QuestionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Questions_QuestionTypes");

            entity.HasOne(d => d.Topic).WithMany(p => p.Questions)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Questions_TopicsDisciplines");
        });

        modelBuilder.Entity<QuestionAnswer>(entity =>
        {
            entity.HasOne(d => d.Question).WithMany(p => p.QuestionAnswers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionAnswers_Questions");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasOne(d => d.Group).WithMany(p => p.Students)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_Groups1");

            entity.HasOne(d => d.User).WithMany(p => p.Students)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_Users");
        });

        modelBuilder.Entity<StudentAttempt>(entity =>
        {
            entity.HasOne(d => d.Exam).WithMany(p => p.StudentAttempts)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentAttempts_Exams");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentAttempts)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentAttempts_Students");
        });

        modelBuilder.Entity<StudentExamResult>(entity =>
        {
            entity.HasKey(e => e.StudentExamId).HasName("PK_StudentExam");

            entity.Property(e => e.TestTimeSpent).HasPrecision(0);
            entity.Property(e => e.TimeEnded).HasPrecision(0);

            entity.HasOne(d => d.Exam).WithMany(p => p.StudentExamResults)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentExam_Exams");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentExamResults)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentExam_Students");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teachers_Users");
        });

        modelBuilder.Entity<TopicsDiscipline>(entity =>
        {
            entity.HasKey(e => e.TopicDisciplinesId);

            entity.HasOne(d => d.Discipline).WithMany(p => p.TopicsDisciplines)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TopicsDisciplines_Disciplines");
        });

        modelBuilder.Entity<TopicsExamTest>(entity =>
        {
            entity.HasKey(e => e.TopicExamTestId);

            entity.ToTable("TopicsExamTest");

            entity.HasOne(d => d.ExamTest).WithMany(p => p.TopicsExamTests)
                .HasForeignKey(d => d.ExamTestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TopicsExamTest_TopicsExamTest");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).ValueGeneratedNever();

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_UserRoles");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
