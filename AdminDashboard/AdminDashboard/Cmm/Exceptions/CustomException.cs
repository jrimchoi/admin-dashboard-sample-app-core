using DSELN.Cmm.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace DSELN.Cmm.Exceptions
{
    public class ModelValidationException : Exception
    {
        public ModelValidationException() { }
        public ModelValidationException(string message) : base(message) { }
        public Dictionary<string, List<string>> ModelValidationErrors { get; set; }

        public string ModelValidationErrorsStr { get; set; }

        public bool IsLogin { get; set; }
    }

    public class SessionExpiredException : Exception
    {
        public SessionExpiredException() { }
        public SessionExpiredException(string message) : base(message) { }
    }

    public class TransactionAbortByBizLogException : Exception
    {
        public TransactionAbortByBizLogException() { }
        public TransactionAbortByBizLogException(string message) : base(message) { }
    }

    public class CustomValidationException : Exception
    {
        public CustomValidationException() { }
        public CustomValidationException(string message) : base(message) { }
    }

}


