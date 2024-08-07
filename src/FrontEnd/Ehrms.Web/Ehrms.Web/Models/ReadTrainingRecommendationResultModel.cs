namespace Ehrms.Web.Models;

public sealed class ReadTrainingRecommendationResultModel
{
    public string Title { get; set; } = string.Empty;
    public string Skill { get; set; } = string.Empty;
    public ICollection<RecommendationResultEmployeeModel> Employees { get; set; } = [];
}

public sealed class RecommendationResultEmployeeModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
}