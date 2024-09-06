namespace Ehrms.TrainingManagement.API.MessageQueue.StateMachine;

public class TrainingRecommendationSagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public Guid RequestId { get; set; }
    public string? CurrentState { get; set; }
    public bool RequestPending { get; set; }
    public bool RequestFinalized { get; set; }
}

public class TrainingRecommendationSaga : MassTransitStateMachine<TrainingRecommendationSagaData>
{
    public required State Pending { get; set; }
    public required State Finalized { get; set; }

    public required Event<TrainingRecommendationRequestAcceptedEvent> RecommendationRequestAccepted { get; set; }
    public required Event<TrainingRecommendationCompletedEvent> RecommendationCompleted { get; set; }
    public required Event<TrainingRecommendationCancelledEvent> RecommendationCancelled { get; set; }

    public TrainingRecommendationSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => RecommendationCancelled, e => e.CorrelateById(m => m.Message.RequestId));
        Event(() => RecommendationCompleted, e => e.CorrelateById(m => m.Message.RequestId));
        Event(() => RecommendationRequestAccepted, e => e.CorrelateById(m => m.Message.RequestId));

        Initially(
            When(RecommendationRequestAccepted)
                .Then(context =>
                {
                    context.Saga.RequestId = context.Message.RequestId;
                    context.Saga.RequestPending = true;
                })
                .TransitionTo(Pending));

        During(Pending,
            When(RecommendationCancelled)
                .Then(context =>
                {
                    context.Saga.RequestFinalized = true;
                    context.Saga.RequestId = context.Message.RequestId;
                })
                .TransitionTo(Finalized)
                .Finalize());

        During(Pending,
            When(RecommendationCompleted)
                .Then(context =>
                {
                    context.Saga.RequestFinalized = true;
                })
                .TransitionTo(Finalized)
                .Finalize());
    }
}