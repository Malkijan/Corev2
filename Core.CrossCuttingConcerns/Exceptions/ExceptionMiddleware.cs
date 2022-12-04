using Core.CrossCuttingConcerns.Exceptions.Handlers;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Core.CrossCuttingConcerns.Exceptions
{
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate _next;
        private readonly HttpExceptionHandler _httpExceptionHandler = new();
        private readonly IHttpContextAccessor _contextAccesor;
        private readonly LoggerServiceBase _loggerService;

        public ExceptionMiddleware(RequestDelegate next,
            IHttpContextAccessor contextAccesor, LoggerServiceBase loggerService)
        {
            _next = next;
            _contextAccesor = contextAccesor;
            _loggerService = loggerService;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await LogException(context, exception);
                await HandleExceptionAsync(context.Response, exception);
            }
        }
        private Task HandleExceptionAsync(HttpResponse response, Exception exception)
        {
            response.ContentType = "application/json";
            _httpExceptionHandler.Response = response;
            return _httpExceptionHandler.HandleExceptionAsync(exception);
        }
        private Task LogException(HttpContext context, Exception exception)
        {
            List<LogParameter> logParameters = new()
            {
                new LogParameter
                {
                    Type = context.GetType().Name,
                    Value = context
                }
            };
            LogDetail logDetail = new()
            {
                MethodName = _next.Method.Name,
                Parameters = logParameters,
                User = _contextAccesor.HttpContext?.User.Identity?.Name ?? "?"
            };
            _loggerService.Info(JsonConvert.SerializeObject(logDetail));
            return Task.CompletedTask;
        }
    }
}