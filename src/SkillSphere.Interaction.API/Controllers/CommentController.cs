using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillSphere.Infrastructure.Security.UserAccessor;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Interaction.Contracts.DTOs;
using SkillSphere.Interaction.Core;
using SkillSphere.Interaction.UseCases.Comments.Commands.AddComment;
using SkillSphere.Interaction.UseCases.Comments.Commands.EditComment;
using SkillSphere.Interaction.UseCases.Comments.Commands.RemoveComment;
using SkillSphere.Interaction.UseCases.Comments.Queries.GetPostComments;

namespace SkillSphere.Interaction.API.Controllers;

[Route("api/interactions")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IUserAccessor _userAccessor;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ReactionController"/>.
    /// </summary>
    /// <param name="mediator"> Интерфейс для отправки команд и запросов через Mediator. </param>
    /// <param name="userAccessor"> Интерфейс для получения идентификатора пользователя из токена. </param>
    /// <exception cref="ArgumentNullException"> Ошибка загрузки интерфейса. </exception>
    public CommentController(IMediator mediator, IUserAccessor userAccessor)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
    }

    [HttpGet("posts/{postId:guid}/comments")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Reaction>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public IAsyncEnumerable<Comment> GetPostComments(Guid postId)
    {
        var query = new GetPostCommentsQuery(postId);

        return _mediator.CreateStream(query);
    }

    [HttpPost("posts/{postId:guid}/comments")]
    public async Task<IActionResult> AddComment(Guid postId, [FromBody] CommentRequestDto commentRequest)
    {
        var userId = _userAccessor.GetUserId();
        var command = new AddCommentCommand(postId, userId, commentRequest.Content);

        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }

    [HttpPut("posts/{postId:guid}/comments/{commentId:guid}")]
    public async Task<IActionResult> EditComment(Guid postId, Guid commentId,
        [FromBody] CommentRequestDto commentRequest)
    {
        var command = new EditCommentCommand(commentId, postId, commentRequest.Content);

        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }

    [HttpDelete("posts/{postId:guid}/comments/{commentId:guid}")]
    public async Task<IActionResult> RemoveComment(Guid postId, Guid commentId)
    {
        var command = new RemoveCommentCommand(commentId, postId);

        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }
}
