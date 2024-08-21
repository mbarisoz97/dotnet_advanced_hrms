namespace Ehrms.TrainingManagement.API.Handlers.Training.Commands;
using Training = Database.Models.Training;

public sealed class UpdateTrainingCommand : IRequest<Training>
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public DateTime? StartsAt { get; set; }
	public DateTime? EndsAt { get; set; }
	public ICollection<Guid> Participants { get; set; } = [];
}

internal sealed class UpdateTrainingCommandHandler : IRequestHandler<UpdateTrainingCommand, Training>
{
	private readonly IMapper _mapper;
	private readonly TrainingDbContext _trainingDbContext;

	public UpdateTrainingCommandHandler(IMapper mapper, TrainingDbContext trainingDbContext)
	{
		_mapper = mapper;
		_trainingDbContext = trainingDbContext;
	}

	public async Task<Training> Handle(UpdateTrainingCommand request, CancellationToken cancellationToken)
	{
		var training = await GetTraining(request, cancellationToken);
		_mapper.Map(request, training);
		training = await SetParticipants(training, request.Participants);

		_trainingDbContext.Update(training);
		await _trainingDbContext.SaveChangesAsync(cancellationToken);

		return training;
	}

	private async Task<Training> SetParticipants(Training training, ICollection<Guid> participants)
	{
		RemoveParticipants(training, participants);
		await AddPariticipants(training, participants);

		return training;
	}
	private void RemoveParticipants(Training training, ICollection<Guid> participants)
	{
		var participantsToRemove = training.Participants
			.Where(x => !participants.Contains(x.Id))
			.ToList();

		foreach (var participant in participantsToRemove)
		{
			training.Participants.Remove(participant);
		}
	}

	private async Task AddPariticipants(Training training, ICollection<Guid> participants)
	{
		var participantsToAdd = _trainingDbContext.Employees
			.Where(x => participants.Contains(x.Id));

		await participantsToAdd.ForEachAsync(x => training.Participants.Add(x));
	}

	private async Task<Training> GetTraining(UpdateTrainingCommand request, CancellationToken cancellationToken)
	{
		return await _trainingDbContext.Trainings
			.Include(x => x.Participants)
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
			?? throw new TrainingNotFoundException($"Could not find training with id '{request.Id}'");
	}
}