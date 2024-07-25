global using AutoMapper;
global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;

global using Ehrms.Shared.Common;
global using Ehrms.Shared.Exceptions;

global using Ehrms.TrainingManagement.API.Exceptions;
global using Ehrms.TrainingManagement.API.Database.Context;
global using Ehrms.TrainingManagement.API;
global using Ehrms.TrainingManagement.API.Dtos.Training;
global using Ehrms.TrainingManagement.API.Database.Models;
global using Ehrms.TrainingManagement.API.Handlers.Training.Commands;

global using MassTransit;
global using Ehrms.Shared;
global using Ehrms.Contracts.Project;
global using Ehrms.TrainingManagement.API.Consumers.EmployeeEvent;

global using Ehrms.TrainingManagement.API.Consumer.SkillEvents;
global using Ehrms.TrainingManagement.API.Consumers.ProjectEvent;