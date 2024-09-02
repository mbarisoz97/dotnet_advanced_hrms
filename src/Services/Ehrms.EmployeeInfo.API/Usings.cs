﻿global using MediatR;
global using AutoMapper;
global using MassTransit;
global using Microsoft.EntityFrameworkCore;
global using LanguageExt.Common;

global using Ehrms.Contracts.Employee;
global using Microsoft.AspNetCore.Mvc;

global using Ehrms.Shared.Exceptions;
global using Ehrms.EmployeeInfo.API.Exceptions;

global using Ehrms.Shared.Common;
global using Ehrms.EmployeeInfo.API;
global using Ehrms.EmployeeInfo.API.Dtos.Skill;
global using Ehrms.EmployeeInfo.API.Dtos.Employee;

global using Ehrms.EmployeeInfo.API.Handlers.Skill.Command;
global using Ehrms.EmployeeInfo.API.Handlers.Skill.Query;

global using Ehrms.EmployeeInfo.API.Handlers.Employee.Query;
global using Ehrms.EmployeeInfo.API.Handlers.Employee.Command;