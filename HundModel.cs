namespace WebHundApi
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HundModel : DbContext
    {
        public HundModel()
            : base("name=HundModel")
        {
        }

        public virtual DbSet<C__EFMigrationsHistory> C__EFMigrationsHistory { get; set; }
        public virtual DbSet<Hunds> Hunds { get; set; }
        public virtual DbSet<Owners> Owners { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hunds>()
                .HasMany(e => e.Owners)
                .WithMany(e => e.Hunds)
                .Map(m => m.ToTable("HundOwners").MapLeftKey("HundId").MapRightKey("OwnerId"));
        }
    }
}
