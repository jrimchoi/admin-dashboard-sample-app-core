using System;
using System.Collections.Generic;

namespace DSELN.Models.Analysis
{
    public class SpreadsheetSubmitViewModel
    {
        public IList<Dictionary<string, Object>> Created { get; set; }

        public IList<Dictionary<string, Object>> Destroyed { get; set; }

        public IList<Dictionary<string, Object>> Updated { get; set; }
    }
}