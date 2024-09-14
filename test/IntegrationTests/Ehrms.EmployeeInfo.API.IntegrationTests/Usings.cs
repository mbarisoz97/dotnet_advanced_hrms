global using System.Net;
global using System.Net.Http.Json;
global using Microsoft.AspNetCore.Http;

global using FluentAssertions;

global using Ehrms.EmployeeInfo.API.Dtos.Skill;
global using Ehrms.EmployeeInfo.API.Dtos.Employee;
global using Ehrms.EmployeeInfo.API.Database.Context;

global using Ehrms.EmployeeInfo.TestHelpers.Faker.Model;
global using Ehrms.EmployeeInfo.TestHelpers.Faker.Command.Title;
global using Ehrms.EmployeeInfo.TestHelpers.Faker.Command.Skill;
global using Ehrms.EmployeeInfo.TestHelpers.Faker.Command.Employee;