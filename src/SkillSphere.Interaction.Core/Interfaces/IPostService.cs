namespace SkillSphere.Interaction.Core.Interfaces;

public interface IPostService
{
    Task<bool> PostExists(Guid postId);
}
