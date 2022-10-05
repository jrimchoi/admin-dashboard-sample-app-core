using DSELN.Cmm.Filters;
using System.ComponentModel.DataAnnotations;

namespace DSELN.Models.Analysis
{
    public class ItemModel : BaseModel
    {
        public int ITEM_ID { get; set; }
        public int SAMPLE_ID { get; set; }
        public int ANAL_ID { get; set; }
        [ValidateInputTextAttribute]
        public string? ITEM_NAME { get; set; }
        [ValidateInputTextAttribute]
        public string? USE_DEPT { get; set; }
        public double? RET_TIME { get; set; } = 0;
        public double? MEAS_RET_TIME { get; set; } = 0;
        public double? AREA { get; set; } = 0;

        [ValidateInputTextAttribute]
        public string? CREATOR { get; set; }
        [ValidateInputTextAttribute]
        public string? MODIFIER { get; set; }
        [ValidateInputTextAttribute]
        public string? TITLE { get; set; }

        public double? PCT_AREA { get; set; } = 0; 
        public string? DATA_JSON { get; set; }

        public int ANALYSIS_REM_CNT { get; set; }  //  남은 건수 
    }

    public class ItemSearch : BaseSearchModel
    {
        [ValidateSearchTextAttribute]
        public int ITEM_ID { get; set; }
        [ValidateSearchTextAttribute]
        public int SAMPLE_ID { get; set; }
        [ValidateSearchTextAttribute]
        public int ANAL_ID { get; set; }

    }

    public class FragranceModel : BaseModel
    {
        public int ANAL_ID { get; set; }
        public int SAMPLE_ID { get; set; }

        ////[StringLength(9999, MinimumLength = 3, ErrorMessage = "샘플명은 3자 이상 입력하십시오.")]
        ///// 서비스단에서 체크하겠다. list 개수만큼 메세지 표시되어서...
        [ValidateInputTextAttribute]
        public string? SAMPLE_NAME { get; set; }
        public double PK { get; set; }
        public double RT { get; set; }
        public double AREA_PCT { get; set; }
        [ValidateInputTextAttribute]
        public string? LIBRARY_ID { get; set; }
        [ValidateInputTextAttribute]
        public string? REF { get; set; }
        [ValidateInputTextAttribute]
        public string? CAS { get; set; }
        public double QUAL { get; set; }
        [ValidateInputTextAttribute]
        public string? CREATOR { get; set; }
        [ValidateInputTextAttribute]
        public string? MODIFIER { get; set; }
        [ValidateInputTextAttribute]
        public string? CREATED { get; set; }
        [ValidateInputTextAttribute]
        public string? MODIFIED { get; set; }
    }

    public class FragranceSearch : BaseSearchModel
    {
        [ValidateSearchTextAttribute]
        public int SAMPLE_ID { get; set; }
    }

}