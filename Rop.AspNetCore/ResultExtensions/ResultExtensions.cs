using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rop.Result;

namespace Rop.AspNetCore.ResultExtensions
{
    public static class ResultExtensions
    {
        public static ActionResult MatchToActionResult<TI>(this ResultAction<TI> resultAction)
        {
            if (resultAction.IsSuccess)
                switch (resultAction.StatusCode.Id)
                {
                    case StatusCodes.Status200OK:
                        return new OkObjectResult(resultAction.Value!);
                }
            else
                switch (resultAction.StatusCode.Id)
                {
                    case StatusCodes.Status400BadRequest:
                        return new BadRequestObjectResult(
                            resultAction.ToProblemDetails("https://tools.ietf.org/html/rfc7235#section-3.1"));

                    case StatusCodes.Status401Unauthorized:
                        return new UnauthorizedObjectResult(
                            resultAction.ToProblemDetails("https://tools.ietf.org/html/rfc7235#section-3.1"));

                    case StatusCodes.Status404NotFound:
                        return new NotFoundObjectResult(
                            resultAction.ToProblemDetails("https://tools.ietf.org/html/rfc7231#section-6.5.4"));
                }

            return new StatusCodeResult(ResultCode.InternalServerError.Id);
        }

        public static ActionResult MatchToActionResult(this ResultAction resultAction)
        {
            if (resultAction.IsSuccess)
                switch (resultAction.StatusCode.Id)
                {
                    case StatusCodes.Status200OK:
                        return new OkResult();
                }
            else
                switch (resultAction.StatusCode.Id)
                {
                    case StatusCodes.Status400BadRequest:
                        return new BadRequestObjectResult(resultAction.ToValidationProblemDetails());

                    case StatusCodes.Status401Unauthorized:
                        return new UnauthorizedObjectResult(
                            resultAction.ToProblemDetails("https://tools.ietf.org/html/rfc7235#section-3.1"));

                    case StatusCodes.Status404NotFound:
                        return new NotFoundObjectResult(
                            resultAction.ToProblemDetails("https://tools.ietf.org/html/rfc7231#section-6.5.4"));
                }

            return new StatusCodeResult(ResultCode.InternalServerError.Id);
        }

        private static ValidationProblemDetails ToValidationProblemDetails(this ResultActionBase resultAction)
        {
            var validationErrors = resultAction.ValidationErrors ?? new List<ErrorValidation>();
            var errors = validationErrors
                .GroupBy(e => e.Identifier)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
            var problemDetails = new ValidationProblemDetails(errors)
            {
                Status = resultAction.StatusCode.Id,
                Title = resultAction.StatusCode.Name,
                Detail = resultAction.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };
            return problemDetails;
        }

        private static ProblemDetails ToProblemDetails(this ResultActionBase resultAction, string type)
        {
            return new ProblemDetails
            {
                Status = resultAction.StatusCode.Id,
                Title = resultAction.StatusCode.Name,
                Detail = resultAction.Message,
                Type = type
            };
        }
    }
}
