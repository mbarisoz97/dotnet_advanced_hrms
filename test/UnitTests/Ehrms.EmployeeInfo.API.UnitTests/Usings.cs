global using Moq;
global using Bogus;
global using AutoMapper;
global using MassTransit;
global using FluentAssertions;

global using Microsoft.EntityFrameworkCore;

global using Ehrms.Contracts.Employee;
global using Ehrms.EmployeeInfo.API.Models;
global using Ehrms.EmployeeInfo.API.Mapping;
global using Ehrms.EmployeeInfo.API.Context;
global using Ehrms.EmployeeInfo.API.UnitTests.TestHelpers.Faker;
global using Ehrms.EmployeeInfo.API.Handlers.Employee.Command;