using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HorseApi.Helpers
{
    public class ExceptionActionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ExceptionActionFilter(
            IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region Overrides of ExceptionFilterAttribute

        public override void OnException(ExceptionContext context)
        {
            Logging.LogServerError(context.Exception);

            base.OnException(context);
        }

        #endregion
    }
}
