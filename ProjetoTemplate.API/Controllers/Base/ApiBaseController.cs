using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ProjetoTemplate.API.Controllers.Base
{
    public class ApiBaseController : ControllerBase
    {
        protected TService GetService<TService>() => (TService)HttpContext.RequestServices.GetService(typeof(TService))!;
        public bool IsProfileAdmin => Convert.ToBoolean(User.FindFirst("IsProfileAdmin")?.Value);
        public long UserId => Convert.ToInt64(User.FindFirst("UserId")?.Value);
    }
}
