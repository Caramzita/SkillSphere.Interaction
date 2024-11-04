using SkillSphere.Interaction.Core.Interfaces;

namespace SkillSphere.Interaction.UseCases.Services;

public class PostService : IPostService
{
    private readonly HttpClient _httpClient;

    public PostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> PostExists(Guid postId)
    {
        var response = await _httpClient.GetAsync($"/api/posts/{postId}");

        return response.IsSuccessStatusCode;
    }
}
