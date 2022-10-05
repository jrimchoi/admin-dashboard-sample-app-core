using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Net.Http.Headers;
using DSELN.Cmm.Exceptions;
using DSELN.Cmm.Utils;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace DSELN.Cmm.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string controllerName = context.ActionDescriptor.GetType().GetProperties().FirstOrDefault(p => p.Name == "ControllerName").GetValue(context.ActionDescriptor) as string ?? string.Empty;
            string actionName = context.ActionDescriptor.GetType().GetProperties().FirstOrDefault(p => p.Name == "ActionName").GetValue(context.ActionDescriptor) as string ?? string.Empty;
            bool isLogin = false;
            if ("Login".Equals(controllerName) && "Login".Equals(actionName)) isLogin = true;

            if (!context.ModelState.IsValid)
            {
                // 에러 메세지 및 리스트는 IntegratedExceptionFilterAttribute 에서 처리하겠다. 
                //context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;  // 400 

                Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
                foreach (KeyValuePair<string, ModelStateEntry> kvp in context.ModelState)
                {
                    string key = kvp.Key;
                    ModelStateEntry entry = kvp.Value;

                    if (entry.Errors.Count > 0)
                    {
                        List<string> errorList = new List<string>();
                        foreach (ModelError error in entry.Errors)
                        {
                            errorList.Add(error.ErrorMessage);
                        }

                        errors[key] = errorList;
                    }
                }

                //Console.WriteLine("OnActionExecuting model validation error count ............" + errors.Count  );


                throw new ModelValidationException("ValidateModelAttribute")
                {
                    ModelValidationErrors = errors,
                    ModelValidationErrorsStr = GetModelValidationErrors2Str(errors),
                    IsLogin = isLogin,
                };

            }

        }

        public string GetModelValidationErrors2Str(Dictionary<string, List<string>> modelValidationErrors)
        {
            string errorList2Str = "";  // modelValidationErrors 내용을 문자로 표시 \
            if (modelValidationErrors != null)
            {
                foreach (var item in modelValidationErrors.Values)
                {
                    errorList2Str += (item[0] as string ?? string.Empty) + "\r"; //.Replace("+", "＋") + "\r";
                }

                //errorList2Str = WebUtility.UrlEncode(errorList2Str).Replace("+", " ").Replace("＋", "");

                //Console.WriteLine("errorList2Str : " + errorList2Str);
            }

            return errorList2Str;
        }

    }

}
