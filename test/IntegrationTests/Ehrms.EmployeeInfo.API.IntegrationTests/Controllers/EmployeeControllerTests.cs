using System.Net;
using FluentAssertions;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Ehrms.EmployeeInfo.API.Dtos.Employee;
using Ehrms.EmployeeInfo.API.IntegrationTests.Faker.Dto;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers;

public class EmployeeControllerTests
{
    [Fact]
    public async Task Put_ValidEmployeeDetails_ReturnsOkWithEmployeeDto()
    {
        EmployeeInfoWebApplicationFactory application = new();
        CreateEmployeeDto createEmployeeDto = new CreateEmployeeDtoFaker().Generate();

        var client = application.CreateClient();
        var response = await client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeDto);
        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        createEmployeeResponse?.Id.Should().NotBe(Guid.Empty);
        createEmployeeResponse?.FirstName.Should().Be(createEmployeeDto.FirstName);
        createEmployeeResponse?.LastName.Should().Be(createEmployeeDto.LastName);
        createEmployeeResponse?.Qualification.Should().Be(createEmployeeDto.Qualification);
        createEmployeeResponse?.DateOfBirth.Should().Be(createEmployeeDto.DateOfBirth);
    }

    [Fact]
    public async Task Get_ExistingEmployeeId_ReturnsOkWithEmployeeReadDto()
    {
        EmployeeInfoWebApplicationFactory application = new();
        CreateEmployeeDto createEmployeeDto = new CreateEmployeeDtoFaker().Generate();

        var client = application.CreateClient();
        var response = await client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeDto);
        response.EnsureSuccessStatusCode();

        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();
        response = await client.GetAsync($"{Endpoints.EmployeeApi}/{createEmployeeResponse?.Id}");
        var readEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readEmployeeResponse?.Id.Should().NotBe(Guid.Empty);
        readEmployeeResponse?.FirstName.Should().Be(createEmployeeDto.FirstName);
        readEmployeeResponse?.LastName.Should().Be(createEmployeeDto.LastName);
        readEmployeeResponse?.Qualification.Should().Be(createEmployeeDto.Qualification);
        readEmployeeResponse?.DateOfBirth.Should().Be(createEmployeeDto.DateOfBirth);
    }

    [Fact]
    public async Task Delete_ExistingEmployeeId_RemovesEmployee()
    {
        EmployeeInfoWebApplicationFactory application = new();
        CreateEmployeeDto createEmployeeDto = new CreateEmployeeDtoFaker().Generate();

        var client = application.CreateClient();
        var response = await client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeDto);
        response.EnsureSuccessStatusCode();

        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();
        response = await client.DeleteAsync($"{Endpoints.EmployeeApi}/{createEmployeeResponse?.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_NonExistingEmployeeId_ReturnsNotFound()
    {
        EmployeeInfoWebApplicationFactory application = new();
        var client = application.CreateClient();
        var response = await client.DeleteAsync($"Endpoints.EmployeeApi/{Guid.NewGuid}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_ExistingEmployeeId_ReturnsOkWithUpdatedReadEmployeeDto()
    {
        EmployeeInfoWebApplicationFactory application = new();
        CreateEmployeeDto createEmployeeDto = new CreateEmployeeDtoFaker().Generate();

        var client = application.CreateClient();
        var response = await client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeDto);
        response.EnsureSuccessStatusCode();
        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        UpdateEmployeeDto updateEmployeeDto = new()
        {
            Id = createEmployeeResponse!.Id,
            FirstName = "IntegrationTestUpdatedFirstName",
            LastName = "IntegrationTestUpdatedLastName",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(395)))
        };
        response = await client.PostAsJsonAsync(Endpoints.EmployeeApi, updateEmployeeDto);
        var updateEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updateEmployeeResponse?.Id.Should().Be(updateEmployeeDto.Id);
        updateEmployeeResponse?.FirstName.Should().Be(updateEmployeeDto.FirstName);
        updateEmployeeResponse?.LastName.Should().Be(updateEmployeeDto.LastName);
        updateEmployeeResponse?.Qualification.Should().Be(updateEmployeeDto.Qualification);
        updateEmployeeResponse?.DateOfBirth.Should().Be(updateEmployeeDto.DateOfBirth);
    }
}