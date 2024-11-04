using Microsoft.EntityFrameworkCore;
using SkillSphere.Interaction.Core;
using SkillSphere.Interaction.Core.Enums;
using SkillSphere.Interaction.Core.Interfaces;

namespace SkillSphere.Interaction.DataAccess.Repositories;

public class ReactionRepository : IReactionRepository
{
    private readonly DatabaseContext _context;

    public ReactionRepository(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IAsyncEnumerable<Reaction> GetEntityReactions(Guid entityId, EntityType entityType)
    {
        return _context.Reactions
            .Where(e => e.EntityId == entityId
                && e.EntityType == entityType)
            .AsNoTracking()
            .AsAsyncEnumerable();
    }

    public async Task<Reaction?> GetReactionByUserAndEntity(Guid userId, Guid entityId, EntityType entityType)
    {
        return await _context.Reactions
            .FirstOrDefaultAsync(e => e.EntityId == entityId &&
                                 e.EntityType == entityType &&
                                 e.UserId == userId)
            .ConfigureAwait(false);
    }

    public async Task<Reaction?> GetReactionById(Guid reactionId, EntityType type, Guid entityId)
    {
        return await _context.Reactions
            .Where(r => r.EntityType == type && r.EntityId == entityId)
            .FirstOrDefaultAsync(r => r.Id == reactionId)
            .ConfigureAwait(false);
    }

    public async Task AddReaction(Reaction reaction)
    {
        await _context.Reactions.AddAsync(reaction);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateReaction(Reaction reaction)
    {
        _context.Reactions.Attach(reaction);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task RemoveReaction(Reaction reaction)
    {
        _context.Reactions.Remove(reaction);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
