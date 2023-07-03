global using System.Text;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using System.ComponentModel.DataAnnotations;

global using Microsoft.Extensions.Options;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Ocelot.Logging;
global using Ocelot.Middleware;

global using Furion.DataEncryption;
global using Furion.JsonSerialization;
global using Furion.FriendlyException;
global using Furion;
global using Furion.RemoteRequest.Extensions;
global using Furion.DependencyInjection;
global using Furion.DynamicApiController;

global using Mapster;
global using SqlSugar;
global using Hx.Core;
global using Hx.Gateway.Application.Entities;
global using Hx.Gateway.Application.Enum;
global using Hx.Gateway.Application.Const;