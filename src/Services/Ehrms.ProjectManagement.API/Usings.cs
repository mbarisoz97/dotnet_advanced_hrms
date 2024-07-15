global using MediatR;
global using AutoMapper;
global using MassTransit;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Ehrms.ProjectManagement.API.Consumer;
global using Ehrms.ProjectManagement.API.Middleware;

global using Ehrms.Shared.Common;
global using Ehrms.Shared.Exceptions;
global using Ehrms.ProjectManagement.API.Exceptions;

global using Ehrms.Contracts.Employee;

global using Ehrms.ProjectManagement.API;
global using Ehrms.ProjectManagement.API.Database.Context;

global using Ehrms.ProjectManagement.API.Dtos.Project;
global using Ehrms.ProjectManagement.API.Database.Models;
global using Ehrms.ProjectManagement.API.Handlers.Project.Commands;