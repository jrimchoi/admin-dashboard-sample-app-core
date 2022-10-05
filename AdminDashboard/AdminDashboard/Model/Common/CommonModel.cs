namespace DSELN.Models.Common
{
    public class CommonModel  : BaseModel // 화면에 필요한 코드값 담는 모델 
    {
        // 공통 코드 조회  
        public string? GRP_CD { get; set; }  // 코드그룹 (코드CD)

        // 공통코드 외 코드성 항목 조회 
        public string? CONST { get; set; }
        public string? KEYWORD { get; set; }

        // 중복체크 
        public string? TABLE_NM { get; set; }
        public string? KEY_ID { get; set; }
        public string? KEY_VALUE { get; set; }
        public string? ORIGIN_KEY_VALUE { get; set; }
        public string? KEY_ID2 { get; set; }
        public string? KEY_VALUE2 { get; set; }
        public string? ORIGIN_KEY_VALUE2 { get; set; }


        // 추가 파라미터 
        public string? PARAM1 { get; set; }

        public string? PARAM2 { get; set; }

    }
}
