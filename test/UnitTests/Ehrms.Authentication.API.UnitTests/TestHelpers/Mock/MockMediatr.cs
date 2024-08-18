using MediatR;

namespace Ehrms.Authentication.API.UnitTests.TestHelpers.Mock;

internal class MockMediatr : Mock<IMediator>
{
    internal void SetupSend<TRequest, TResponse>(TResponse response)
        where TRequest : IRequest<TResponse>
    {
        Setup(x => x.Send(It.IsAny<TRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
    }

    internal void SetupSend<TResponse>(IRequest<TResponse> request, TResponse response)
    {
        Setup(x => x.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
    }
}