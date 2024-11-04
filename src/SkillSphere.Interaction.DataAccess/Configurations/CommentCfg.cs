using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillSphere.Interaction.Core;

namespace SkillSphere.Interaction.DataAccess.Configurations;

public class CommentCfg : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
               .IsRequired();

        builder.HasIndex(c => new { c.Id, c.PostId })
            .IsUnique(false);

        builder.Property(c => c.UserId)
               .IsRequired();

        builder.Property(c => c.PostId)
               .IsRequired();

        builder.Property(c => c.Content)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(c => c.CreatedAt)
               .IsRequired();

        builder.Property(c => c.IsEdited)
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(c => c.IsDeleted)
               .IsRequired()
               .HasDefaultValue(false);
    }
}
