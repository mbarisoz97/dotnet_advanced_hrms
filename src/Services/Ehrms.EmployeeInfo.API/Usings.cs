global using Microsoft.AspNetCore.Mvc;

global using Microsoft.EntityFrameworkCore;
global using AutoMapper;
global using MediatR;

global using Ehrms.Shared.Common;
global using Ehrms.EmployeeInfo.API;
global using Ehrms.EmployeeInfo.API.Models;
global using Ehrms.EmployeeInfo.API.Context;
global using Ehrms.EmployeeInfo.API.Dtos.Skill;
global using Ehrms.EmployeeInfo.API.Dtos.Employee;

global using Ehrms.EmployeeInfo.API.Handlers.Skill.Command;
global using Ehrms.EmployeeInfo.API.Handlers.Skill.Query;

global using Ehrms.EmployeeInfo.API.Handlers.Employee.Query;
global using Ehrms.EmployeeInfo.API.Handlers.Employee.Command;