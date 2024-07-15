
global using Bogus;
global using AutoMapper;
global using FluentAssertions;

global using Ehrms.ProjectManagement.API.Database.Context;
global using Ehrms.ProjectManagement.API.Handlers.Project.Commands;
global using Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker;

global using Ehrms.ProjectManagement.API.Handlers.Project.Queries;
global using Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker.Command;

global using Ehrms.ProjectManagement.API.Profiles;
global using Microsoft.EntityFrameworkCore;
global using Ehrms.ProjectManagement.API.Exceptions;
global using Ehrms.ProjectManagement.API.Consumer;

global using Ehrms.ProjectManagement.API.UnitTests.TestHelpers;
global using Ehrms.Contracts.Employee;
global using MassTransit;
global using Microsoft.Extensions.Logging;
global using Moq;
global using Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker.Event;