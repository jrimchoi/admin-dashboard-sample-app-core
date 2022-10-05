using DSELN.Cmm.Filters;
namespace DSELN.Models.Common
{

    public class CodeModel  // 화면에 필요한 코드값 담는 모델 
    {
        public string? TEXT { get; set; }  // 값 
        public string? VALUE { get; set; } // 코드 
        public string? ATT1 { get; set; } = "";
        public string? ATT2 { get; set; } = "";
        public string? ATT3 { get; set; } = "";
        public string? ATT4 { get; set; } = "";
        public string? ATT5 { get; set; } = "";

        public string? ATT1_NM { get; set; } = "";
        public string? ATT2_NM { get; set; } = "";
        public string? ATT3_NM { get; set; } = "";
        public string? ATT4_NM { get; set; } = "";
        public string? ATT5_NM { get; set; } = "";

    }

    public class CodeCondition  // 코드값 조회 조건 
    {
        [ValidateSearchTextAttribute]
        public string? ID { get; set; }
        [ValidateSearchTextAttribute]
        public string? SUB_ID { get; set; }

        [ValidateSearchTextAttribute]
        public string? PARAM1 { get; set; }
        [ValidateSearchTextAttribute]
        public string? PARAM1_VALUE { get; set; }

        [ValidateSearchTextAttribute]
        public string? PARAM2 { get; set; }
        [ValidateSearchTextAttribute]
        public string? PARAM2_VALUE { get; set; }

        [ValidateSearchTextAttribute]
        public string? PARAM3 { get; set; }
        [ValidateSearchTextAttribute]
        public string? PARAM3_VALUE { get; set; }

        [ValidateSearchTextAttribute]
        public string? KEYWORD { get; set; }
        [ValidateSearchTextAttribute]
        public string? CONST { get; set; }  // 이전 버전용 
    }
    
}