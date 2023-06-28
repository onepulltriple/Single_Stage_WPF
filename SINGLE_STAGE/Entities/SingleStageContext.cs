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
            entity.HasKey(e => e.Id).HasName("PK__APPEARAN__3213E83F96CD1D6B");

            entity.ToTable("APPEARANCES");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArtistId).HasColumnName("Artist_id");
            entity.Property(e => e.PerformanceId).HasColumnName("Performance_id");
            entity.Property(e => e.RoyaltyAdvance).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.RoyaltyAtEnd).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Artist).WithMany(p => p.Appearances)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APPEARANC__Artis__300424B4");

            entity.HasOne(d => d.Performance).WithMany(p => p.Appearances)
                .HasForeignKey(d => d.PerformanceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APPEARANC__Perfo__30F848ED");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ARTISTS__3213E83FA7E5AE5D");

            entity.ToTable("ARTISTS");

            entity.HasIndex(e => e.Name, "UQ__ARTISTS__737584F649B2E282").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cavent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CAVENTS__3213E83F001EC8F3");

            entity.ToTable("CAVENTS");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TicketPrice).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EMPLOYEE__3213E83F1D1723FC");

            entity.ToTable("EMPLOYEES");

            entity.HasIndex(e => e.Username, "UQ__EMPLOYEE__536C85E4111B8B2F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Performance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PERFORMA__3213E83F049B7103");

            entity.ToTable("PERFORMANCES");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CaventId).HasColumnName("Cavent_id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Cavent).WithMany(p => p.Performances)
                .HasForeignKey(d => d.CaventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PERFORMAN__Caven__2D27B809");
        });

        modelBuilder.Entity<Seatnumber>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SEATNUMB__3213E83FB28AEAE6");

            entity.ToTable("SEATNUMBERS");

            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<Seatrow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SEATROWS__3213E83F80AAEC61");

            entity.ToTable("SEATROWS");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Row)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TICKETS__3213E83FB8A99C6B");

            entity.ToTable("TICKETS");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CaventId).HasColumnName("Cavent_id");
            entity.Property(e => e.SeatNumberId).HasColumnName("SeatNumber_id");
            entity.Property(e => e.SeatRowId).HasColumnName("SeatRow_id");
            entity.Property(e => e.TicketholderId).HasColumnName("Ticketholder_id");

            entity.HasOne(d => d.Cavent).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.CaventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TICKETS__Cavent___3B75D760");

            entity.HasOne(d => d.SeatNumber).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatNumberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TICKETS__SeatNum__3E52440B");

            entity.HasOne(d => d.SeatRow).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatRowId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TICKETS__SeatRow__3D5E1FD2");

            entity.HasOne(d => d.Ticketholder).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketholderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TICKETS__Ticketh__3C69FB99");
        });

        modelBuilder.Entity<Ticketholder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TICKETHO__3213E83F0BDDB66A");

            entity.ToTable("TICKETHOLDERS");

            entity.HasIndex(e => e.EmailAddress, "UQ__TICKETHO__49A147409E98667C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
