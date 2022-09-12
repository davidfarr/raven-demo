using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Raven.Core;
using Raven.Core.Models;

namespace Raven.Data
{
    public partial class RavenDbContext : DbContext
    {
        public RavenDbContext()
        {
        }

        public RavenDbContext(DbContextOptions<RavenDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Requirement> Requirements { get; set; }
        public virtual DbSet<TestCase> TestCases { get; set; }
        public virtual DbSet<TestCaseStep> TestCaseSteps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Raven_demo;Integrated Security=True;"); //use for localhost/non-Docker
            //optionsBuilder.UseSqlServer("Data Source=host.docker.internal;Initial Catalog=Raven_demo;Integrated Security=True"); //use for Docker - requires more CORS configuration

            //connect to Azure secrets to avoid having to swap this info out
            optionsBuilder.UseSqlServer("Server=tcp:davidfarr-dev.database.windows.net,1433;Initial Catalog=Raven_Demo;Persist Security Info=False;User ID={user id};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasIndex(e => e.ShortCode, "UQ__Projects__76E6BB82C3E43C9D")
                    .IsUnique();

                entity.Property(e => e.ProjectId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ImageLocation)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('\"~/Icons/002-raven-1.png\"')");

                entity.Property(e => e.Info)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ShortCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Requirement>(entity =>
            {
                entity.Property(e => e.RequirementId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Info)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VersionIntroduced)
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Requirements)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectsRequirements");
            });

            modelBuilder.Entity<TestCase>(entity =>
            {
                entity.Property(e => e.TestCaseId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Info)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TestCases)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectsTestCase");

                entity.HasOne(d => d.Requirement)
                    .WithMany(p => p.TestCases)
                    .HasForeignKey(d => d.RequirementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequirementsTestCase");
            });

            modelBuilder.Entity<TestCaseStep>(entity =>
            {
                entity.Property(e => e.TestCaseStepId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Step)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TestCaseSteps)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectsTestCaseStep");

                entity.HasOne(d => d.TestCase)
                    .WithMany(p => p.TestCaseSteps)
                    .HasForeignKey(d => d.TestCaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TestCasesStep");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
