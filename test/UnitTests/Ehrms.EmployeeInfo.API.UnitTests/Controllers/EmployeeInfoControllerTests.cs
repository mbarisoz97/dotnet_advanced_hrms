using Ehrms.EmployeeInfo.API.Controllers;
using Ehrms.Shared.TestHepers.Mock;

namespace Ehrms.EmployeeInfo.API.UnitTests.Controllers;

public class EmployeeControllerTests
{
    private readonly MockMediatr _mockMediatr = new();
    private readonly EmployeeController _controller;

    public EmployeeControllerTests()
    {
        var mapper = MapperFactory.CreateWithExistingProfiles();
        _controller = new EmployeeController(mapper, _mockMediatr.Object);
    }
}