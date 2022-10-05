using System;
using System.Collections.Generic;

namespace DSELN.Models.Common
{
    public partial class DownloadHistory
    {
        public string UserName { get; set; } = null!;
        public DateTime DlDate { get; set; }
        public string DlListType { get; set; } = null!;
        public decimal? DlParagraphId { get; set; }
        public decimal DlDocumentid { get; set; }
        public string? DlFileName { get; set; }
    }
}
