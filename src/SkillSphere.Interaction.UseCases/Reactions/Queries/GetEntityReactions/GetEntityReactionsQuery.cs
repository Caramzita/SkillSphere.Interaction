using MediatR;
using SkillSphere.Interaction.Core;
using SkillSphere.Interaction.Core.Enums;
using SkillSphere.Interaction.Core.Interfaces;
using System.Runtime.CompilerServices;

namespace SkillSphere.Interaction.UseCases.Reactions.Queries.GetEntityReactions;

public record GetEntityReactionsQuery(Guid EntityId, EntityType EntityType) : IStreamRequest<Reaction>;

public class GetEntityReactionsQueryHandler : IStreamRequestHandler<GetEntityReactionsQuery, Reaction>
{
    private readonly IReactionRepository _reactionRepository;

    public GetEntityReactionsQueryHandler(IReactionRepository reactionRepository)
    {
        _reactionRepository = reactionRepository ?? throw new ArgumentNullException(nameof(reactionRepository));
    }

    public async IAsyncEnumerable<Reaction> Handle(GetEntityReactionsQuery request, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var reactions = _reactionRepository.GetEntityReactions(request.EntityId, request.EntityType);

        await foreach (var reaction in reactions.WithCancellation(cancellationToken))
        {
            yield return reaction;
        }
    }
}
