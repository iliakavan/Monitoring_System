﻿global using notifier.Infrastructure;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using MediatR;
global using notifier.Application;
global using notifier.Middleware;
global using System.Net;
global using System.Text.Json;
global using notifier.Domain.Enum;
global using notifier.Application.Projects.Commads.AddProjectCommand;
global using notifier.Application.Projects.Commads.DeleteProjectCommand;
global using notifier.Application.Projects.Commads.UpdateProjectCommand;
global using notifier.Application.Projects.Queries.GetAllProjects;
global using notifier.Application.Projects.Queries.GetById;
global using notifier.Application.Services.Queries.GetServiceById;
global using notifier.Application.Services.Queries.GetAllService;
global using notifier.Application.Projects.Queries.Search;
global using notifier.Application.Services.Commands.AddService;
global using notifier.Application.Services.Commands.UpdateService;
global using notifier.Application.Services.Commands.DeleteService;
global using notifier.Application.ServiceTests.Queries.GetAllServiceTest;
global using notifier.Application.ServiceTests.Queries.GetServiceTestById;
global using notifier.Application.ServiceTests.Queries.Search;
global using notifier.Application.ServiceTests.Command.AddServiceTest;
global using notifier.Application.ServiceTests.Command.UpdateServiceTest;
global using notifier.Application.ProjectOfficials.Commands.AddProjectOfficial;
global using notifier.Application.ProjectOfficials.Commands.DeleteProjectOffical;
global using notifier.Application.ProjectOfficials.Commands.UpdateProjectOffical;
global using notifier.Application.ProjectOfficials.Quries.GetAllProjectOfficials;
global using notifier.Application.ProjectOfficials.Quries.Search;
global using notifier.Application.ProjectOfficials.Quries.GetProjectOfficialById;
global using notifier.Application.Services.Queries.Search;
global using notifier.Application.ServiceTests.Command.DeleteServiceTest;
global using notifier.Application.ServiceNotifications.Commands.AddServiceNotification;
global using notifier.Application.ServiceNotifications.Commands.DeleteServiceNotification;
global using notifier.Application.ServiceNotifications.Commands.UpdateServiceNotification;
global using notifier.Application.ServiceNotifications.Queries.GetAllServiceNotification;
global using notifier.Application.ServiceNotifications.Queries.GetServiceNotification;
global using notifier.Application.ServiceNotifications.Queries.Search;
global using Hangfire;
global using Hangfire.SqlServer;
global using Microsoft.OpenApi.Models;
global using notifier.Hangfire;
global using wallet_api.Util;
global using Swashbuckle.AspNetCore.SwaggerUI;