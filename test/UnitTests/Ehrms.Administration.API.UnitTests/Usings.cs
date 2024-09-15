global using System;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using System.Collections.Generic;
global using Microsoft.Extensions.Logging;

global using Moq;
global using Ehrms.Administration.API.Consumer;
global using Ehrms.Administration.API.Database.Context;

global using Ehrms.Administration.API.UnitTests.TestHelpers.Configurations;
global using Ehrms.Administration.TestHelpers.Fakers.Employee;
global using Ehrms.Contracts.Employee;
global using FluentAssertions;
global using MassTransit;