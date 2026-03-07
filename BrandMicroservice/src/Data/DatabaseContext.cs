using BrandMicroservice.src.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace BrandMicroservice.src.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) 
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var make = modelBuilder.Entity<Make>();
            make.ToTable("makes");
            make.Property(x => x.Id).HasColumnName("_id").ValueGeneratedOnAdd();
            make.Property(x => x.MakeName).HasColumnName("make_name").HasColumnType("VARCHAR(100)").IsRequired();
            make.Property(x => x.Origin).HasColumnName("origin").HasColumnType("VARCHAR(100)").IsRequired();
            make.Property(x => x.MakeLogo).HasColumnName("make_logo").HasColumnType("TEXT");
            make.Property(x => x.Active).HasColumnName("active").IsRequired();
            make.Property(x => x.YearFounded).HasColumnName("year_founded").IsRequired();
            make.Property(x => x.Company).HasColumnName("company").HasColumnType("VARCHAR(100)").IsRequired();
            make.Property(x => x.MakeCode).HasColumnName("make_code").HasColumnType("VARCHAR(20)").IsRequired();
            make.Property(x => x.MakeAbbreviation).HasColumnName("make_abbreviation").HasColumnType("VARCHAR(10)").IsRequired();
            make.Property(x => x.DateCreated).HasColumnName("date_created").HasColumnType("TIMESTAMPTZ").HasDefaultValueSql("CURRENT_TIMESTAMP");
            make.Property(x => x.DateUpdated).HasColumnName("date_updated").HasColumnType("TIMESTAMPTZ").IsRequired(false);
            make.HasMany(x => x.Models).WithOne(x => x.Makes).HasForeignKey(x => x.MakeId);

            var model = modelBuilder.Entity<Model>();
            model.ToTable("models");
            model.Property(x => x.Id).HasColumnName("_id").ValueGeneratedOnAdd();
            model.Property(x => x.ModelName).HasColumnName("model_name").HasColumnType("VARCHAR(100)").IsRequired();
            model.Property(x => x.MakeId).HasColumnName("make_id").IsRequired();
            model.Property(x => x.Active).HasColumnName("active").IsRequired();
            model.Property(x => x.BodyType).HasColumnName("body_type").HasColumnType("VARCHAR(100)").IsRequired();
            model.Property(x => x.YearFounded).HasColumnName("year_founded").IsRequired();
            model.Property(x => x.ModelCode).HasColumnName("model_code").HasColumnType("VARCHAR(20)").IsRequired();
            model.Property(x => x.DateCreated).HasColumnName("date_created").HasColumnType("TIMESTAMPTZ").HasDefaultValueSql("CURRENT_TIMESTAMP");
            model.Property(x => x.DateUpdated).HasColumnName("date_updated").HasColumnType("TIMESTAMPTZ").IsRequired(false);

        }
    }
}
