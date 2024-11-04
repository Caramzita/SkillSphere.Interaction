using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillSphere.Interaction.Core;

namespace SkillSphere.Interaction.DataAccess.Configurations;

public class ReactionCfg : IEntityTypeConfiguration<Reaction>
{
    public void Configure(EntityTypeBuilder<Reaction> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => new { r.UserId, r.EntityId, r.EntityType })
               .IsUnique();

        builder.Property(r => r.Id)
               .IsRequired();

        builder.Property(r => r.UserId)
               .IsRequired();

        builder.Property(r => r.EntityId)
               .IsRequired();

        builder.Property(r => r.EntityType)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.Property(r => r.ReactionType)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(15);

        builder.Property(r => r.CreatedAt)
               .IsRequired();
    }
}
