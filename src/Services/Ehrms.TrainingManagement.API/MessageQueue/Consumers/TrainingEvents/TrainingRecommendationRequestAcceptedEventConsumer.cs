using LanguageExt;

namespace Ehrms.TrainingManagement.API.MessageQueue.Consumers.TrainingEvents;

public sealed class TrainingRecommendationRequestAcceptedEventConsumer
    : IConsumer<TrainingRecommendationRequestAcceptedEvent>
{
    private readonly TrainingDbContext _dbContext;
    private readonly ILogger<TrainingRecommendationRequestAcceptedEventConsumer> _logger;

    public TrainingRecommendationRequestAcceptedEventConsumer(TrainingDbContext dbContext,
        ILogger<TrainingRecommendationRequestAcceptedEventConsumer> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<TrainingRecommendationRequestAcceptedEvent> context)
    {
        var request = await _dbContext.RecommendationRequests.FirstOrDefaultAsync(x => x.Id == context.Message.RequestId);
        if (request == null)
        {
            _logger.LogError("Ignored null event : <{event}>", nameof(TrainingRecommendationRequestAcceptedEvent));
            return;
        }

        var project = await _dbContext.Projects
            .Include(x => x.Employees)
            .Include(x => x.RequiredSkills)
            .FirstOrDefaultAsync(x => x.Id == request.ProjectId);

        if (project == null)
        {
            _logger.LogError("Could not find project with id : <{id}>", request.ProjectId);
            await PublishRecommendationCancelledEvent(context);
            return;
        }

        await SetRequestStatusAsPending(request);
        await CreateTrainingRecommendations(request, project);
        await PublishRecommendationCompletedEvent(context);
    }

    private async Task SetRequestStatusAsPending(TrainingRecommendationRequest request)
    {
        request.UpdatedAt = DateTime.UtcNow;
        request.RequestStatus = RequestStatus.Pending;
        _dbContext.Update(request);
        await _dbContext.SaveChangesAsync();
    }

    private async Task CreateTrainingRecommendations(TrainingRecommendationRequest request, Project project)
    {
        var preferences = _dbContext.TrainingRecommendationPreferences
            .Include(x => x.Project)
            .Include(x => x.Title)
            .Include(x => x.Skills)
            .AsSplitQuery()
            .Where(x => x.Project!.Id == project.Id);

        if (!preferences.Any())
        {
            //ToDo: Add log message
            return;
        }

        Dictionary<Title, List<Skill>> titleSkillMapping = CreateTitleSkillMatrix(preferences);
        Dictionary<Skill, TrainingRecommendationResult> recommendationMapping = CreateTrainingRecommendationMatrix(request, project, titleSkillMapping);

        await _dbContext.AddRangeAsync(recommendationMapping.Values);
        await _dbContext.SaveChangesAsync();
    }

    private Dictionary<Skill, TrainingRecommendationResult> CreateTrainingRecommendationMatrix(TrainingRecommendationRequest request, Project project, Dictionary<Title, List<Skill>> titleSkillMapping)
    {
        var employees = _dbContext.Employees
            .Include(x => x.Title)
            .Include(x => x.Skills)
            .Where(x => project.Employees.Contains(x));

        Dictionary<Skill, TrainingRecommendationResult> recommendationMapping = [];
        foreach (var employee in employees)
        {
            if (!titleSkillMapping.TryGetValue(employee.Title!, out var skills))
            {
                //No preference found for given title.
                continue;
            }

            foreach (var skill in skills)
            {
                if (employee.Skills.Contains(skill))
                {
                    continue;
                }

                if (!recommendationMapping.TryGetValue(skill, out TrainingRecommendationResult? recommendationResult))
                {
                    recommendationResult = new TrainingRecommendationResult() { Skill = skill, RecommendationRequest = request };
                    recommendationMapping[skill] = recommendationResult;
                }

                recommendationResult.Employees.Add(employee);
            }
        }

        return recommendationMapping;
    }

    private static Dictionary<Title, List<Skill>> CreateTitleSkillMatrix(IQueryable<TrainingRecommendationPreferences> preferences)
    {
        Dictionary<Title, List<Skill>> titleSkillMapping = [];
        foreach (var preference in preferences)
        {
            if (!titleSkillMapping.TryGetValue(preference.Title!, out var skills))
            {
                skills = [];
                titleSkillMapping[preference.Title!] = skills;
            }
            skills.AddRange(preference.Skills);
        }

        return titleSkillMapping;
    }

    private static async Task PublishRecommendationCompletedEvent(ConsumeContext<TrainingRecommendationRequestAcceptedEvent> context)
    {
        TrainingRecommendationCompletedEvent recommendationCompletedEvent = new()
        {
            RequestId = context.Message.RequestId
        };
        await context.Publish(recommendationCompletedEvent);
    }
    private static async Task PublishRecommendationCancelledEvent(ConsumeContext<TrainingRecommendationRequestAcceptedEvent> context)
    {
        TrainingRecommendationCancelledEvent recommendationCancelledEvent = new()
        {
            RequestId = context.Message.RequestId
        };
        await context.Publish(recommendationCancelledEvent);
    }
}