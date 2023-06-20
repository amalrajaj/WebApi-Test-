using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DAL.Models;

public partial class DemoDbContext : DbContext
{
    private IConfiguration configuration;
  
    public DemoDbContext()
    {        
        var cConstr = configuration.GetConnectionString("conString");

    }

    public DemoDbContext(DbContextOptions<DemoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HobbyTable> HobbyTables { get; set; }

    public virtual DbSet<StudentDbTable> StudentDbTables { get; set; }

    public virtual DbSet<Stunthobby> Stunthobbies { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-RI2ASIB\\SQLEXPRESS;Initial Catalog=DemoDB;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    

    modelBuilder.Entity<HobbyTable>(entity =>
        {
            entity.HasKey(e => e.HobbyId);

            entity.ToTable("HobbyTable");

            entity.Property(e => e.HobbyName)
                .HasMaxLength(10)
                .IsFixedLength();
        }); 

        modelBuilder.Entity<StudentDbTable>(entity =>
        {
            entity.HasKey(e => e.StudentId);

            entity.ToTable("studentDbTable");

            entity.Property(e => e.Dob).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.StudentName)
                .HasMaxLength(20)
                .IsFixedLength();
        });
        modelBuilder.Entity<Stunthobby>(entity =>
        {
            entity.ToTable("stunthobbies");

            entity.HasOne(d => d.Hobby).WithMany(p => p.Stunthobbies)
                .HasForeignKey(d => d.HobbyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_stunthobbies_HobbyTable");

            entity.HasOne(d => d.Student).WithMany(p => p.Stunthobbies)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_stunthobbies_studentDbTable");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
