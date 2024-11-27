using DarazApp.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DarazApp.Controllers
{
    public class BaseController : ControllerBase
    {

        // Override BadRequest to return a generic ApiResponse<T> with the error message and errors list
        protected ActionResult BadRequest<T>(string message, List<string> errors = null)
        {
            return base.BadRequest(new ApiResponse<T>(message, errors));  // new List<string> { message }
        }


        // Override OK to return a generic ApiResponse<T> with success data and message
        protected ActionResult Ok<T>(T data, string message = null)
        {
            return base.Ok(new ApiResponse<T>(data, message));
        }


        // Override NotFound to return a generic ApiResponse<T> with a message indicating not found
        protected ActionResult NotFound<T>(string message)
        {
            return base.NotFound(new ApiResponse<T>(message));
        }


        protected ActionResult ValidateModel<T>(T model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage)
                                        .ToList();
                return BadRequest(new ApiResponse<T>("Invalid data provided", errors));
            }
            return null;

        }
    }
}
