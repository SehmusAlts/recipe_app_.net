namespace RecipeApp.Web.Models;

public class RatingDto
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Value { get; set; }
    public string? Comment { get; set; }
    public DateTime RatedAt { get; set; }
}
