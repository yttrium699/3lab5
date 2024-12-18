using EmotionsApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmotionsApp.Infrastructure
{
    public partial class EmotionsAppContext : DbContext
    {
        public EmotionsAppContext()
        {
        }

        public EmotionsAppContext(DbContextOptions<EmotionsAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Emotion> Emotions { get; set; } = null!;
        public virtual DbSet<EmotionNote> EmotionNotes { get; set; } = null!;
        public virtual DbSet<Psychologist> Psychologists { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      => optionsBuilder.UseNpgsql("Name=DefaultConnection");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("users_pkey");
                entity.ToTable("users");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("login");

                entity.Property(e => e.PassHash)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("pass_hash");
            });

            modelBuilder.Entity<Emotion>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("emotions_pkey");
                entity.ToTable("emotions");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.StateDate)
                    .HasColumnType("date")
                    .HasColumnName("state_date");

                entity.Property(e => e.EmotionType)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("emotion_type");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Emotions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_emotions_user");
            });

            modelBuilder.Entity<EmotionNote>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("emotion_notes_pkey");
                entity.ToTable("emotion_notes");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.EmotionId)
                    .IsRequired()
                    .HasColumnName("emotion_id");

                entity.Property(e => e.NoteText)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("note_text");

                entity.HasOne(d => d.Emotion)
                    .WithMany(p => p.EmotionNotes)
                    .HasForeignKey(d => d.EmotionId)
                    .HasConstraintName("fk_emotion_notes_emotion");
            });

            modelBuilder.Entity<Psychologist>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("psychologists_pkey");
                entity.ToTable("psychologists");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.ContactInfo)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("contact_info");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Psychologists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_psychologists_user");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
