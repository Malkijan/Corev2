using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails
{
	public class ValidationProblemDetails : ProblemDetails
	{
		public object Failures { get; init; }

		public ValidationProblemDetails(object failures)
		{
			Title = "Validation error(s)";
			Failures = failures;
			Status = StatusCodes.Status400BadRequest;
			Type = "https://example/probs/validation";
			Detail = "";
			Instance = "";
		}
	}
}

