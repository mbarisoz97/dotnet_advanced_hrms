global using Moq;
global using Bogus;
global using AutoMapper;
global using MassTransit;
global using FluentAssertions;

global using Microsoft.EntityFrameworkCore;
global using Ehrms.EmployeeInfo.API.Exceptions;
global using Ehrms.EmployeeInfo.API.UnitTests.TestHelpers;

global using Ehrms.Contracts.Employee;
global using Ehrms.EmployeeInfo.API.Mapping;
global using Ehrms.EmployeeInfo.API.Handlers.Employee.Command;
global using Ehrms.EmployeeInfo.API.UnitTests.TestHelpers.Fakers.Models;
global using Ehrms.EmployeeInfo.API.UnitTests.TestHelpers.Fakers.Command;