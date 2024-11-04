using Microsoft.EntityFrameworkCore;
using SkillSphere.Interaction.Core;
using SkillSphere.Interaction.Core.Interfaces;

namespace SkillSphere.Interaction.DataAccess.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly DatabaseContext _context;

    public CommentRepository(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IAsyncEnumerable<Comment> GetPostComments(Guid postId)
    {
        return _context.Comments
            .Where(c => c.PostId == postId)
            .AsNoTracking()
            .AsAsyncEnumerable();
    }

    public async Task<Comment?> GetCommentById(Guid id)
    {
        return await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == id)
            .ConfigureAwait(false);
    }

    public async Task AddComment(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateComment(Comment comment)
    {
        _context.Comments.Attach(comment);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task RemoveComment(Comment comment)
    {
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }  
}
