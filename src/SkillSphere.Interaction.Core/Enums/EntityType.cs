using System.Text.Json.Serialization;

namespace SkillSphere.Interaction.Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EntityType
{
    Post,
    Comment
}
