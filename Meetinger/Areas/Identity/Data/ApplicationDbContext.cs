using Meetinger.Areas.Identity.Data;
using Meetinger.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Meetinger.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<MeetingParticipant> Participants { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        builder.Entity<MeetingParticipant>()
            .HasKey(mp => new { mp.ParticipantId, mp.MeetingId });

        builder.Entity<MeetingParticipant>()
            .HasOne(mp => mp.Meeting)
            .WithMany(m => m.Participants)
            .HasForeignKey(mp => mp.MeetingId)
            .OnDelete(DeleteBehavior.NoAction);

        // builder.Entity<MeetingParticipant>()
        //     .HasOne(mp => mp.ParticipantId)
        //     .WithMany()
        //     .HasForeignKey(mp => mp.ParticipantId)
        //     .OnDelete(DeleteBehavior.NoAction);

    }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(255);
        builder.Property(u => u.LastName).HasMaxLength(255);

    }
}