using AutoMapper;
using SkillSphere.Interaction.Contracts.DTOs;
using SkillSphere.Interaction.UseCases.Reactions.Commands.AddOrUpdateReaction;

namespace SkillSphere.Interaction.API.Profiles;

/// <summary>
/// Профиль AutoMapper для маппинга объектов запросов из контроллера на команды.
/// </summary>
public class ControllerMappingProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ControllerMappingProfile"/> и задает конфигурации маппинга.
    /// </summary>
    public ControllerMappingProfile()
    {
        CreateMap<ReactionRequestDto, AddOrUpdateReactionCommand>();
    }
}
