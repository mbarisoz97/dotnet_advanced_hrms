using Moq;
using MediatR;

namespace Ehrms.Shared.TestHepers.Mock;

public class MockMediatr : Mock<IMediator>
{
    public void SetupSend<TRequest, TResponse>(TResponse response)
        where TRequest : IRequest<TResponse>
    {
        Setup(x => x.Send(It.IsAny<TRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
    }
    
    public void SetupSend<TResponse>(IRequest<TResponse> request, TResponse response)
    {
        Setup(x => x.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
    }
}