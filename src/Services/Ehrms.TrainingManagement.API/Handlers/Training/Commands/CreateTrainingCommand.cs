﻿using FluentValidation;

namespace Ehrms.TrainingManagement.API.Handlers.Training.Commands;

public sealed class CreateTrainingCommand : IRequest<Database.Models.Training>
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public DateTime PlannedAt { get; set; }
	public DateTime StartsAt { get; set; }
	public DateTime EndsAt { get; set; }
	public ICollection<Guid> Participants { get; set; } = [];
}

public sealed class CreateTrainingCommandHandler : IRequestHandler<CreateTrainingCommand, Database.Models.Training>
{
	private readonly IMapper _mapper;
	private readonly TrainingDbContext _dbContext;

	public CreateTrainingCommandHandler(IMapper mapper, TrainingDbContext dbContext)
	{
		_mapper = mapper;
		_dbContext = dbContext;
	}

	public async Task<Database.Models.Training> Handle(CreateTrainingCommand request, CancellationToken cancellationToken)
	{
		if (request.StartsAt > request.EndsAt)
		{
			throw new ValidationException("Training end data should be later date than start date.");
		}
		
		Database.Models.Training training = new();
		_mapper.Map(request, training);

		var participants = _dbContext.Employees
				.Where(x => request.Participants.Contains(x.Id));

		await participants.ForEachAsync(training.Participants.Add, cancellationToken: cancellationToken);

		await _dbContext.AddAsync(training, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return training;
	}
}