using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BattleShip.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BattleShip.Api.Filters
{
    public interface IExceptionHandler : IExceptionBuilder, IAsyncExceptionFilter
    {
        Task OnExceptionAsync(HttpContext httpContext);
    }


    public class ExceptionHandler : IAsyncExceptionFilter, IExceptionHandler
    {
        private readonly ILogger _logger;

        public ExceptionHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ExceptionHandler>();
        }

        public async Task OnExceptionAsync(ExceptionContext expContext)
        {
            var controllerName = expContext.RouteData.Values["controller"];


            // var requestContent = await this._requestSerializer.SerializeAsync(expContext.HttpContext);

            var exp = expContext.Exception;

            var errorCodes = new List<string>();

            int statusCode = 500;


            switch (exp)
            {
                /*badRequest*/
                /*validation*/
                case BadRequestException _:
                    {
                        expContext.Result = new BadRequestResult();
                        statusCode = 400;

                        if (exp.InnerException is ValidationException innerExp)
                            errorCodes.Add(innerExp.Code);
                        else if (exp.InnerException is ValidationAggregateException innerAggExp)
                            errorCodes.AddRange(innerAggExp.InnerExceptions.OfType<ValidationException>().Select(e => e.Code));
                        break;
                    }
                /*unauthorized*/
                case ValidationAggregateException validationAggExp:
                    expContext.Result = new BadRequestResult();
                    statusCode = 400;

                    errorCodes.AddRange(validationAggExp.InnerExceptions.OfType<ValidationException>().Select(e => e.Code));
                    break;
                case UnauthorizedAccessException _:
                /*internalServer*/
                case AuthenticationException _:
                    expContext.Result = new UnauthorizedResult();
                    statusCode = 401;
                    break;
                default:
                    statusCode = 500;
                    break;
            }

          

            errorCodes = errorCodes.Distinct().ToList();

            expContext.Result = new ContentResult
            {
                Content = errorCodes.Any()
                    ? JsonConvert.SerializeObject(new { message = $"{exp.Message}",  errorCodes })
                    : JsonConvert.SerializeObject(new { message = $"{exp.Message}" }),
                ContentType = "application/json",
                StatusCode = statusCode
            };

            await Task.FromResult(true);
        }

        public async Task OnExceptionAsync(HttpContext httpContext)
        {
         

            var exceptionHandlerFeature = httpContext.Features?.Get<IExceptionHandlerPathFeature>();

            var trackId = httpContext.TraceIdentifier;

            var (status, _) = this.BuildAndLogException(exceptionHandlerFeature?.Error, _logger, trackId);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = status;

            var contentBuilder = new StringBuilder();

            contentBuilder.Append(JsonConvert.SerializeObject(new
            { Message = $"An error occurred. Please use trackId {trackId} to review the details." }));

            using (var sw = new StreamWriter(httpContext.Response?.Body))
            {
                await sw.WriteAsync(contentBuilder);
            }
        }

        public (int Status, Exception Error) BuildAndLogException(Exception exp, ILogger logger, string trackId)
        {
            (int Status, Exception Error) context = (-1, null);

            switch (exp)
            {
                case BadRequestException _:
                    context = (400, exp);
                    break;
                case UnauthorizedAccessException _:
                case AuthenticationException _:
                    context = (401, exp);
                    break;
                case null:
                case InternalServerErrorException _:
                    context = (500, exp);
                    break;
                case ValidationAggregateException _:
                case ValidationException _:
                    context = (400, new BadRequestException(
                        $"Input data is not valid. Please use TrackId {trackId} to check the details.",
                        exp, trackId));
                    break;
                default:
                    context = (500, new InternalServerErrorException(
                        $"An error occurred.",
                        exp, trackId));
                    break;
            }

            return context;
        }
    }
}
