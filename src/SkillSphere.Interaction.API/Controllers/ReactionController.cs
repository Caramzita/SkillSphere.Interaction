using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSphere.Infrastructure.Security.UserAccessor;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Interaction.Contracts.DTOs;
using SkillSphere.Interaction.Core;
using SkillSphere.Interaction.Core.Enums;
using SkillSphere.Interaction.UseCases.Reactions.Commands.AddOrUpdateReaction;
using SkillSphere.Interaction.UseCases.Reactions.Commands.RemoveReaction;
using SkillSphere.Interaction.UseCases.Reactions.Queries.GetEntityReactions;

namespace SkillSphere.Interaction.API.Controllers;

/// <summary>
/// Предоставляет Rest API для работы с реакциями.
/// </summary>
[ApiController]
[Route("api/interactions")]
[Authorize]
public class ReactionController : ControllerBase
{
    private readonly IMapper _mapper;

    private readonly IMediator _mediator;

    private readonly IUserAccessor _userAccessor;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ReactionController"/>.
    /// </summary>
    /// <param name="mediator"> Интерфейс для отправки команд и запросов через Mediator. </param>
    /// <param name="mapper"> Интерфейс для маппинга данных между моделями. </param>
    /// <param name="userAccessor"> Интерфейс для получения идентификатора пользователя из токена. </param>
    /// <exception cref="ArgumentNullException"> Ошибка загрузки интерфейса. </exception>
    public ReactionController(IMapper mapper, IMediator mediator, IUserAccessor userAccessor)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
    }

    /// <summary>
    /// Получить все реакции поста.
    /// </summary>
    /// <param name="postId"> Идентификатор поста. </param>
    [HttpGet("posts/{postId:guid}/reactions")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Reaction>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public IAsyncEnumerable<Reaction> GetPostReactions(Guid postId)
    {
        var query = new GetEntityReactionsQuery(postId, EntityType.Post);
        return _mediator.CreateStream(query);
    }

    /// <summary>
    /// Получить все реакции комментария.
    /// </summary>
    /// <param name="commentId"> Идентификатор комментария. </param>
    [HttpGet("comments/{commentId:guid}/reactions")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Reaction>), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public IAsyncEnumerable<Reaction> GetCommentReactions(Guid commentId)
    {
        var query = new GetEntityReactionsQuery(commentId, EntityType.Comment);
        return _mediator.CreateStream(query);
    }

    /// <summary>
    /// Добавить или обновить реакцию.
    /// </summary>
    /// <param name="entityId">Идентификатор сущности (поста или комментария).</param>
    /// <param name="reactionDto">Модель данных реакции.</param>
    /// <param name="entityType">Тип сущности: Post или Comment.</param>
    [HttpPost("{entityType}/{entityId:guid}/reactions")]
    [ProducesResponseType(typeof(Reaction), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> AddOrUpdateReaction(Guid entityId, 
        [FromBody] ReactionRequestDto reactionDto, EntityType entityType)
    {
        var command = _mapper.Map<AddOrUpdateReactionCommand>(reactionDto);
        command.EntityId = entityId;
        command.EntityType = entityType;
        command.UserId = _userAccessor.GetUserId();

        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить реакцию
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="entityType"></param>
    /// <param name="reactionId"></param>
    /// <returns></returns>
    [HttpDelete("{entityType}/{entityId:guid}/reactions/{reactionId:guid}")]
    [ProducesResponseType(typeof(Unit), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> DeleteReaction(Guid entityId, 
        EntityType entityType, Guid reactionId)
    {
        var userId = _userAccessor.GetUserId();

        var command = new RemoveReactionCommand(
            reactionId, entityId, userId, entityType);

        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }
}
