using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Net.Http.Headers;
using DSELN.Cmm.Exceptions;
using DSELN.Cmm.Utils;
using DSELN.Cmm.DataBase;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Net;
using System.Text;
using Serilog;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace DSELN.Cmm.Filters
{
    public class IntegratedExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            context.ExceptionHandled = true;    // 필수 

            /******************************************************************
            // reqeust, response 
            ******************************************************************/
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            bool isAjaxRequest = IsAjaxRequest(request);  // ajax 로 request 
            string isAjaxCall = request.Headers["AjaxCall"].ToString(); // ajax beforeSend 에 AjaxCall == "Y" 설정  <-- user defined 
            string dataType = request.Headers["accept"].ToString();   //  리턴할 데이터 타입 ex) application/json
            int respnseCode = response.StatusCode;

            // exception 
            string exceptionName = context.Exception.GetType().Name;

            // session exception 
            string redirectUrl = "Views/Shared/Error.cshtml";

            // model validation exception error list 
            string errorList2Str = "";
            bool isLogin = false;
            Dictionary<string, List<string>> modelValidationErrors = null;

            Log.Debug("request.Headers -->  : " + isAjaxRequest + " / " + isAjaxCall + " / " + dataType + " / " + response.StatusCode);

            /******************************************************************
            // Exception error message 
            ******************************************************************/
            if (respnseCode == 200) respnseCode = (int)HttpStatusCode.BadRequest;  // 400 ;
            string errorMessage = context.Exception.Message;
            switch (context.Exception)
            {
                case TransactionAbortByBizLogException:
                    errorMessage += "\r" + "[현재 처리는 취소되었습니다.]";
                    break;
                case ModelValidationException:
                    if ("ValidateModelAttribute".Equals(context.Exception.Message))  // Model validation 에서 발생   : 사용자정의 exception 으로 바꿀것... 
                    {
                        var errorProp = context.Exception.GetType().GetProperties().FirstOrDefault(p => p.Name == "ModelValidationErrors");
                        modelValidationErrors = (Dictionary<string, List<string>>)errorProp.GetValue(context.Exception);
                        if (modelValidationErrors != null)
                        {
                            var errorStrProp = context.Exception.GetType().GetProperties().FirstOrDefault(p => p.Name == "ModelValidationErrorsStr");
                            errorList2Str = (errorStrProp.GetValue(context.Exception) as string) ?? string.Empty;

                            var isLoginProp = context.Exception.GetType().GetProperties().FirstOrDefault(p => p.Name == "IsLogin");
                            isLogin = (bool)isLoginProp.GetValue(context.Exception);
                            //GetModelValidationErrors2Str(modelValidationErrors);  // ModelValidationErrorsStr
                        }
                        respnseCode = (int)HttpStatusCode.BadRequest;  // 400 ;
                    }
                    break;
                case SessionExpiredException:
                    if ("SessionExpired".Equals(context.Exception.Message))
                    {
                        errorMessage = "세션이 만료되었습니다.";
                        redirectUrl = "Views/Shared//SessionExpired.cshtml";
                    }
                    respnseCode = 440;  // 440 ;
                    break;
                case ArgumentException:
                    errorMessage = "ArgumentException occurred";
                    break;
                case NullReferenceException:
                    errorMessage = "NullReferenceException 이 발생하였습니다.";
                    break;
                case OracleException:
                    //errorMessage = errorMessage;
                    break;
                case AntiforgeryValidationException:
                    errorMessage = "AntiforgeryValidationException " + errorMessage;
                    break;
                case DirectoryNotFoundException:
                    errorMessage = "Directory Not Found  : " + errorMessage;
                    break;
                case CustomValidationException:
                    errorMessage = "Validation  : " + errorMessage;
                    break;
                default:
                    errorMessage = "Some unknown error occurred : " + errorMessage;
                    break;
            }

            // dev console print .....
            Log.Debug("context.Exception.Message : " + context.Exception.Message);
            Log.Debug(context.Exception, errorMessage);  // error log 

            /******************************************************************
            // Exception 처리결과 반영 
            // contentType & dataType 종류에 따른 처리 구분 
            ******************************************************************/
            if (isAjaxRequest)
            {
                if ("Y".Equals(request?.Headers["AjaxCall"].ToString()))  // ajax but header[AjaxCall] == Y
                {
                    if (dataType.IndexOf("text/html") == 0) // text/html 
                    {
                        Log.Debug("Ajax to text/html.........");

                        response.Clear();
                        response.StatusCode = respnseCode;
                        response.ContentType = new MediaTypeHeaderValue("text/html").ToString();
                        response.WriteAsync(errorMessage + "\r" + errorList2Str); // errorList 존재할 경우 메세지와 함께 넘겨줄것...보완 
                    }
                    else // json 으로 간주 
                    {
                        Log.Debug("Ajax to json............");

                        response.Clear();
                        response.StatusCode = respnseCode;
                        response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
                        response.WriteAsync(JsonConvert.SerializeObject(new { _status = Const.FAIL, _msg = errorMessage, _errorList = modelValidationErrors }), Encoding.UTF8);
                    }

                }
                else // ajax but header[AjaxCall] <> Y
                {
                    if (dataType.IndexOf("text/html") == 0) // text/html 
                    {
                        Log.Debug("Ajax to text/html 2 .........");

                        var result = new ViewResult { ViewName = redirectUrl };
                        var modelMetadata = new EmptyModelMetadataProvider();
                        result.ViewData = new ViewDataDictionary(modelMetadata, context.ModelState);
                        result.ViewData.Add("HandleException", context.Exception);
                        result.ViewData.Add("_status", "0");  // 실패 
                        result.ViewData.Add("_msg", errorList2Str == "" ? "Error Occured..." + errorMessage : errorList2Str);
                        context.Result = result;
                    }
                    else // json 으로 간주 
                    {
                        Log.Debug("Ajax to json 2 ............");
                        // to-do....
                    }
                }
            }
            else  // ajax 아닌 경우 
            {
                Log.Debug("No Ajax to text/html ........." + errorMessage);

                var result = new ViewResult { ViewName = redirectUrl };
                var modelMetadata = new EmptyModelMetadataProvider();
                result.ViewData = new ViewDataDictionary(modelMetadata, context.ModelState);
                result.ViewData.Add("HandleException", context.Exception);
                result.ViewData.Add("_status", "0");  // 실패 
                result.ViewData.Add("_msg", string.IsNullOrEmpty(errorList2Str) ? "Error Occured..." + errorMessage : errorList2Str);
                result.ViewData.Add("_isLogin", isLogin);
                //result.ViewData.Add("_errorList", null);

                context.Result = result;

                //context.Result = new RedirectResult("/Login/Login");
                //or context.Result = new RedirectResult("/Home/Error");
            }
        }

        // ajax request 여부 
        public static bool IsAjaxRequest(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Headers != null)
            {
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }

            return false;
        }


    }

}
