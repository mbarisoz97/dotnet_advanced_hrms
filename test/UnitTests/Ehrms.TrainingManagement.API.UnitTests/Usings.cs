global using AutoMapper;
global using FluentAssertions;

global using Ehrms.TrainingManagement.API.Exceptions;
global using Ehrms.TrainingManagement.API.Handlers.Training.Queries;

global using Ehrms.TrainingManagement.API.Database.Models;
global using Ehrms.TrainingManagement.API.Handlers.Training.Commands;

global using Ehrms.TrainingManagement.API.Database.Context;
global using Ehrms.TrainingManagement.API.MessageQueue.Events;
global using Ehrms.TrainingManagement.API.UnitTests.TestHelpers;
global using Ehrms.TrainingManagement.API.MessageQueue.Consumers.TrainingEvents;

global using Ehrms.Training.TestHelpers.Fakers.Models;
global using Ehrms.Training.TestHelpers.Fakers.Events;
global using Ehrms.Training.TestHelpers.Fakers.Commands;