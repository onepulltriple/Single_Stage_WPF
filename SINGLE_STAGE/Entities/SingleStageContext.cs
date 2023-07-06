using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SINGLE_STAGE.Entities;

public partial class SingleStageContext : DbContext
{
    public SingleStageContext()
    {
    }

    public SingleStageContext(DbContextOptions<SingleStageContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appearance> Appearances { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Cavent> Cavents { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Performance> Performances { get; set; }

    public virtual DbSet<Seatnumber> Seatnumbers { get; set; }

    public virtual DbSet<Seatrow> Seatrows { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Ticketholder> Ticketholders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; Database=SINGLE_STAGE; Integrated Security=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appearance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__APPEARAN__3213E83F734B2777");

            entity.ToTable("APPEARANCES");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArtistId).HasColumnName("Artist_id");
            entity.Property(e => e.PerformanceId).HasColumnName("Performance_id");
            entity.Property(e => e.RoyaltyAdvance).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.RoyaltyAtEnd).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Artist).WithMany(p => p.Appearances)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("FK_ParentArtistChildAppearances");

            entity.HasOne(d => d.Performance).WithMany(p => p.Appearances)
                .HasForeignKey(d => d.PerformanceId)
                .HasConstraintName("FK_ParentPerformanceChildAppearances");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ARTISTS__3213E83FFD7F261D");

            entity.ToTable("ARTISTS");

            entity.HasIndex(e => e.Name, "UQ__ARTISTS__737584F6048FEA1D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cavent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CAVENTS__3213E83F0AB66E34");

            entity.ToTable("CAVENTS");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TicketPrice).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EMPLOYEE__3213E83F5127ADBB");

            entity.ToTable("EMPLOYEES");

            entity.HasIndex(e => e.Username, "UQ__EMPLOYEE__536C85E4438826F7").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Performance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PERFORMA__3213E83F7D1E2D6F");

            entity.ToTable("PERFORMANCES");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CaventId).HasColumnName("Cavent_id");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Cavent).WithMany(p => p.Performances)
                .HasForeignKey(d => d.CaventId)
                .HasConstraintName("FK_ParentCaventChildPerformances");
        });

        modelBuilder.Entity<Seatnumber>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SEATNUMB__3213E83F64A0E4CA");

            entity.ToTable("SEATNUMBERS");

            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<Seatrow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SEATROWS__3213E83FC4F222DB");

            entity.ToTable("SEATROWS");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Row)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TICKETS__3213E83F54B9C497");

            entity.ToTable("TICKETS");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CaventId).HasColumnName("Cavent_id");
            entity.Property(e => e.SeatNumberId).HasColumnName("SeatNumber_id");
            entity.Property(e => e.SeatRowId).HasColumnName("SeatRow_id");
            entity.Property(e => e.TicketholderId).HasColumnName("Ticketholder_id");

            entity.HasOne(d => d.Cavent).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.CaventId)
                .HasConstraintName("FK_ParentCaventChildTickets");

            entity.HasOne(d => d.SeatNumber).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatNumberId)
                .HasConstraintName("FK_ParentSeatNumberChildTickets");

            entity.HasOne(d => d.SeatRow).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatRowId)
                .HasConstraintName("FK_ParentSeatRowChildTickets");

            entity.HasOne(d => d.Ticketholder).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketholderId)
                .HasConstraintName("FK_ParentTicketholderChildTickets");
        });

        modelBuilder.Entity<Ticketholder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TICKETHO__3213E83F7DA4B9F7");

            entity.ToTable("TICKETHOLDERS");

            entity.HasIndex(e => e.Email, "UQ__TICKETHO__A9D10534E5D879A5").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
