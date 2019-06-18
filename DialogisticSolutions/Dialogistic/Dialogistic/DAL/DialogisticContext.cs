namespace Dialogistic.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Dialogistic.Models;

    public partial class DialogisticContext : DbContext
    {
        public DialogisticContext()
            : base("name=DialogisticContext")
        {
        }

        public virtual DbSet<CallAssignment> CallAssignments { get; set; }
        public virtual DbSet<CallLog> CallLogs { get; set; }
        public virtual DbSet<Constituent> Constituents { get; set; }
        public virtual DbSet<Gift> Gifts { get; set; }
        public virtual DbSet<ProposedConstituentsChanges> ProposedConstituentsChanges { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CallLog>()
                .HasMany(e => e.CallAssignments)
                .WithOptional(e => e.CallLog)
                .HasForeignKey(e => e.CallLogID);

            modelBuilder.Entity<CallLog>()
                .HasMany(e => e.Gifts)
                .WithRequired(e => e.CallLog)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Constituent>()
                .Property(e => e.Latitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<Constituent>()
                .Property(e => e.Longitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<Constituent>()
                .HasMany(e => e.CallAssignments)
                .WithRequired(e => e.Constituent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Constituent>()
                .HasMany(e => e.CallLogs)
                .WithRequired(e => e.Constituent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Constituent>()
                .HasMany(e => e.Gifts)
                .WithRequired(e => e.Constituent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Gift>()
                .Property(e => e.GiftAmount)
                .HasPrecision(19, 2);

            modelBuilder.Entity<ProposedConstituentsChanges>()
                .Property(e => e.Latitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<ProposedConstituentsChanges>()
                .Property(e => e.Longitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.DonationsRaised)
                .HasPrecision(19, 2);
        }
    }
}
