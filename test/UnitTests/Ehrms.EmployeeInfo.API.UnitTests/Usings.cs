global using Moq;
global using AutoMapper;
global using MassTransit;
global using FluentAssertions;

global using Microsoft.EntityFrameworkCore;
global using Ehrms.EmployeeInfo.API.Exceptions;
global using Ehrms.EmployeeInfo.API.UnitTests.TestHelpers;

global using Ehrms.Contracts.Title;
global using Ehrms.Contracts.Employee;

global using Ehrms.EmployeeInfo.API.Mapping;
global using Ehrms.EmployeeInfo.API.Database.Context;

global using Ehrms.EmployeeInfo.API.Exceptions.Title;

global using Ehrms.EmployeeInfo.API.Handlers.Employee.Command;
global using Ehrms.EmployeeInfo.API.Handlers.Title.Command;
global using Ehrms.EmployeeInfo.API.Handlers.Title.Query;

global using Ehrms.EmployeeInfo.TestHelpers.Faker.Model;
global using Ehrms.EmployeeInfo.TestHelpers.Faker.Command.Title;
global using Ehrms.EmployeeInfo.TestHelpers.Faker.Command.Skill;
global using Ehrms.EmployeeInfo.TestHelpers.Faker.Command.Employee;