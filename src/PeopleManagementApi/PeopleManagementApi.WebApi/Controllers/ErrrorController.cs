﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MainHub.Internal.PeopleAndCulture.PeopleManagement.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public async Task<IActionResult> Error()
        {
            Exception? exception = HttpContext?.Features?.Get<IExceptionHandlerFeature>()?.Error;
            await Task.CompletedTask;
            return Problem(detail: exception?.Message);
        }
    }
}
