using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FicRecs.DatabaseLib
{
    public partial class FicRecsDbContext : DbContext
    {
        public FicRecsDbContext()
        {
        }

        public FicRecsDbContext(DbContextOptions<FicRecsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<StoryDetails> StoryDetails { get; set; }
        public virtual DbSet<StoryMatrix> StoryMatrix { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=/var/run/postgresql;Database=db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoryDetails>(entity =>
            {
                entity.HasKey(e => e.StoryId);

                entity.ToTable("story_details");

                entity.Property(e => e.StoryId)
                    .HasColumnName("story_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Author)
                    .HasColumnName("author")
                    .HasMaxLength(100);

                entity.Property(e => e.Chapters).HasColumnName("chapters");

                entity.Property(e => e.Characters)
                    .HasColumnName("characters")
                    .HasMaxLength(100);

                entity.Property(e => e.Complete).HasColumnName("complete");

                entity.Property(e => e.Favs).HasColumnName("favs");

                entity.Property(e => e.Follows).HasColumnName("follows");

                entity.Property(e => e.Published)
                    .HasColumnName("published")
                    .HasColumnType("date");

                entity.Property(e => e.Reviews).HasColumnName("reviews");

                entity.Property(e => e.Summary)
                    .HasColumnName("summary")
                    .HasMaxLength(1000);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(200);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(250);

                entity.Property(e => e.Words).HasColumnName("words");
            });

            modelBuilder.Entity<StoryMatrix>(entity =>
            {
                entity.HasKey(e => new { e.StoryA, e.StoryB });

                entity.ToTable("story_matrix");

                entity.HasIndex(e => e.Similarity)
                    .HasName("idx_similarity");

                entity.HasIndex(e => e.StoryA)
                    .HasName("idx_story_a");

                entity.HasIndex(e => e.StoryB)
                    .HasName("idx_story_b");

                entity.Property(e => e.StoryA).HasColumnName("story_a");

                entity.Property(e => e.StoryB).HasColumnName("story_b");

                entity.Property(e => e.Similarity).HasColumnName("similarity");

                entity.HasOne(d => d.StoryANavigation)
                    .WithMany(p => p.StoryMatrixStoryANavigation)
                    .HasForeignKey(d => d.StoryA)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_story_a");

                entity.HasOne(d => d.StoryBNavigation)
                    .WithMany(p => p.StoryMatrixStoryBNavigation)
                    .HasForeignKey(d => d.StoryB)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_story_b");
            });
        }
    }
}
