using Microsoft.AspNetCore.Mvc;

namespace BBKBootcampSocial.Core.Utilities
{
    public static class JsonResponseStatus
        {
            public static JsonResult Success()
            {
                return new JsonResult(new { status = "Success" });
            }

            public static JsonResult Success(object returnData)
            {
                return new JsonResult(new { status = "Success", data = returnData });
            }

            public static JsonResult NotFound()
            {
                return new JsonResult(new { status = "NotFound" });
            }

            public static JsonResult NotFound(object returnData)
            {
                return new JsonResult(new { status = "NotFound", data = returnData });
            }

            public static JsonResult Error()
            {
                return new JsonResult(new { status = "Error" });
            }

            public static JsonResult Error(object returnData)
            {
                return new JsonResult(new { status = "Error", data = returnData });
            }
        }
    

}
