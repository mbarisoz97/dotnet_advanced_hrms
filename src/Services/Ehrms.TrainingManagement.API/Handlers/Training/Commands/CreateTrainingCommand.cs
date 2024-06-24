namespace Ehrms.TrainingManagement.API.Handlers.Training.Commands;

public sealed class CreateTrainingCommand : IRequest<Models.Training>
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public DateTime PlannedAt { get; set; }
	public ICollection<Guid> Participants { get; set; } = [];
}

public sealed class CreateTrainingCommandHandler : IRequestHandler<CreateTrainingCommand, Models.Training>
{
	private readonly IMapper _mapper;
	private readonly TrainingDbContext _dbContext;

	public CreateTrainingCommandHandler(IMapper mapper, TrainingDbContext dbContext)
	{
		_mapper = mapper;
		_dbContext = dbContext;
	}

	public async Task<Models.Training> Handle(CreateTrainingCommand request, CancellationToken cancellationToken)
	{
		Models.Training training = new();
		_mapper.Map(request, training);

		await _dbContext.Employees
				.AsNoTracking()
				.Where(x => request.Participants.Contains(x.Id))
				.ForEachAsync(training.Participants.Add);

		await _dbContext.AddAsync(training, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return training;
	}
}