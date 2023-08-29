using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Microsoft.VisualBasic;

namespace Multi_Request.Services.PayloadFilterService
{

    public static class Constants
    {
        public const long MaxPayloadSize = 10 * 1024 * 1024; // 10 MB
    }
    public class MaxPayloadSizeFilterAttribute : ActionFilterAttribute
    {
        private readonly long _maxPayloadSize;

        public MaxPayloadSizeFilterAttribute()
        {
            _maxPayloadSize = Constants.MaxPayloadSize;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Body == null)
            {
                context.Result = new BadRequestObjectResult("Invalid request body.");
                return;
            }

            var jsonPayload = JsonSerializer.Serialize(context.ActionArguments.Values);
            var payloadSize = Encoding.UTF8.GetByteCount(jsonPayload);

            if (payloadSize > _maxPayloadSize)
            {
                context.Result = new BadRequestObjectResult("Payload size exceeds the allowed limit.");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
