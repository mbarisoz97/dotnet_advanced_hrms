﻿using System.Net;
using Ehrms.Web.Models;

namespace Ehrms.Web.Client;

internal sealed class EmployeeInfoServiceClient : IEmployeeInfoServiceClient
{
	private const string EmployeeInfoServiceEndpoint = "/api/Employee";
	private readonly IHttpClientFactory _httpClientFactory;

	public EmployeeInfoServiceClient(IHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory;
	}

	public async Task CreateEmployee(Employee employee)
	{
		var client = _httpClientFactory.CreateClient("ApiGateway");
		await client.PutAsJsonAsync(EmployeeInfoServiceEndpoint, employee);
	}

	public async Task<IEnumerable<Employee>> GetEmployees(int page, int pageSize = 20)
	{
		var client = _httpClientFactory.CreateClient("ApiGateway");
		var response = await client.GetAsync(EmployeeInfoServiceEndpoint);

		IEnumerable<Employee> employees = [];
		if (response.StatusCode == HttpStatusCode.OK)
		{
			employees = await response.Content.ReadFromJsonAsync<IEnumerable<Employee>>() ?? [];
		}
		return employees.Take(pageSize);
	}
}