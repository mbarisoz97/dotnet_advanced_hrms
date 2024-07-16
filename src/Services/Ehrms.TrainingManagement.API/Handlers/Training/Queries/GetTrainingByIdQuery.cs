using Ehrms.TrainingManagement.API.Database.Context;
using Ehrms.TrainingManagement.API.Exceptions;

namespace Ehrms.TrainingManagement.API.Handlers.Training.Queries;

internal sealed class GetTrainingByIdQuery : IRequest<Database.Models.Training>
{
    public Guid Id { get; set; }    
}

internal sealed class GetTrainingByIdQueryHandler : IRequestHandler<GetTrainingByIdQuery, Database.Models.Training>
{
    private readonly TrainingDbContext _dbContext;

    public GetTrainingByIdQueryHandler(TrainingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Database.Models.Training> Handle(GetTrainingByIdQuery request, CancellationToken cancellationToken)
    {
        var trarining= await _dbContext.Trainings
            .Include(x=>x.Participants)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) 
            ?? throw new TrainingNotFoundException($"Could not find training with id '{request.Id}'");
        
        return trarining;
    }
}