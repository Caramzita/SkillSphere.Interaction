namespace SkillSphere.Interaction.Core;

public class Comment
{
    public Guid Id {  get; init; }

    public Guid PostId { get; private set; }

    public Guid UserId { get; private set; }

    public string Content { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public bool IsEdited { get; private set; }

    public bool IsDeleted { get; private set; }

    public Comment(Guid postId, Guid userId, string content)
    {
        Id = Guid.NewGuid();
        PostId = postId;
        UserId = userId;
        SetContent(content);
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
        IsEdited = false;
    }

    public Comment(Guid id, Guid postId, Guid userId, string content, 
        DateTime createdAt, bool isEdited, bool isDeleted)
    {
        Id = id;
        PostId = postId;
        UserId = userId;
        Content = content;
        CreatedAt = createdAt;
        IsDeleted = isDeleted;
        IsEdited = isEdited;
    }

    public void UpdateContent(string newContent)
    {
        SetContent(newContent);
        IsEdited = true;
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    private void SetContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Comment content cannot be empty", nameof(content));
        }

        Content = content;
    }
}
