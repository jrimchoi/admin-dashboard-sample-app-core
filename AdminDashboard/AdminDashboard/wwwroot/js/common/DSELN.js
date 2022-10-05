
/***************************************************************************************
// Chart
***************************************************************************************/
// DashBoard  Chart
var DashBoardChart = {
    color: ["#ff1c1c", "#ffae00", "#73c100", "#007eff", "#32CD32", "#00FFFF", "#00BFFF", "#FF1493",
        "#DA70D6", "#2F4F4F", "#808080", "#663399", "#1E90FF", "#40E0D0", "#008000", "#FFD700",
        "#E6E6FA", "#D8BFD8", "#DDA0DD", "#EE82EE", "#DA70D6", "#FF00FF", "#4B0082", "#483D8B",
    ],

    // Pie chart 
    createPieChart: function (chartId, title, series, enlarge, skipX) {
 
        $("#" + chartId).kendoChart({
            title: {
                text: title
            },
            legend: {
                //position: "bottom"
            },
            seriesDefaults: {
                labels: {
                    template: "#= category #: #= value #",
                    position: "outsideEnd",
                    visible: true,
                    background: "transparent"
                }
            },
            series: [{
                type: "pie",
                data: series,
            }],
            tooltip: {
                visible: true,
                template: "#= category #: #= value #",
            }
        });

        // seriesClick 
        var chart = $("#" + chartId).data("kendoChart");
        if ((new Function(' return  typeof(' + chartId + '_seriesClick) == "function" ? ' + chartId + '_seriesClick : null  '))() != null) { // 사용자 정의 이벤트가 존재하면...
            chart.bind("seriesClick", (new Function(' return  typeof(' + chartId + '_seriesClick) == "function" ? ' + chartId + '_seriesClick : null  '))());
        }

        if (enlarge) {
            if ($("#" + chartId).find("#enlarge").length == 0) {
                $("#" + chartId).prepend('<button type="button" id="enlarge" class="k-i-zoom-in" style="position: absolute; right: 0; margin-top:8px;margin-right:4px;" ></button>');

                $("#enlarge").kendoButton({
                    icon: "zoom-in",
                    click: function (e) {
                        POP.braPopup();
                    }
                });
            }
        }
    }, 

    // bar chart 
    createBarChart: function (chartId, title, chartData, enlarge) {
        //var min = Math.min.apply(null, seriesData);
        //var max = Math.max.apply(null, seriesData);
 
        // 10. 팀별  건수 
        $("#" + chartId).kendoChart({
            dataSource: {
                data: chartData 
            },
            title: {
                text: title
            },
            transitions: false,
            series: [{
                type: "column",
                field: "CNT",
                colorField: "valueColor",
                labels: {
                    visible: true,
                    format: "{0}"
                }
            }],
            categoryAxis: {
                field: "DEPTNAME",
                labels: {
                    color: "#293135",
                    rotation: {
                        angle: (chartData.length == 1 ? 0 : 45)
                    }
                },
            },
            valueAxis: {
                labels: {
                    template: "#= value # "
                },
                title: {
                    //text: "Dollars"
                }
            },
            tooltip: {
                visible: true,
                template: "#=kendo.format('{0:0}', value)# "
            }
        });

        // seriesClick 
        var chart = $("#" + chartId).data("kendoChart");
        if ((new Function(' return  typeof(' + chartId + '_seriesClick) == "function" ? ' + chartId + '_seriesClick : null  '))() != null) { // 사용자 정의 이벤트가 존재하면...
            chart.bind("seriesClick", (new Function(' return  typeof(' + chartId + '_seriesClick) == "function" ? ' + chartId + '_seriesClick : null  '))());
        }

        if (enlarge) {
            if ($("#" + chartId).find("#enlarge").length == 0) {
                $("#" + chartId).prepend('<button type="button" id="enlarge" class="k-i-zoom-in" style="position: absolute; right: 0; margin-top:8px;margin-right:4px;" ></button>');

                $("#enlarge").kendoButton({
                    icon: "zoom-in",
                    click: function (e) {
                        POP.braPopup();
                    }
                });
            }
        }
    },

    // bar chart 4 baseUnit  
    createBarChart4BaseUnit: function (chartId, chartData, baseUnit, title, series, format, enlarge) {

        // 10. 팀별  건수 
        $("#" + chartId).kendoChart({
            dataSource: {
                data: chartData
            },
            title: {
                text: title
            },
            legend: {
                position: "bottom"
            },
            seriesDefaults: {
                type: "bar",
                stack: true,
                //labels: {
                //    visible: true,
                //    format: "{0}"
                //}
            },
            series: series,
            categoryAxis: {
                baseUnit: baseUnit, //"months",
                majorGridLines: {
                    visible: false
                },
                labels: {
                    format: format,
                },
            },
            valueAxis: {
                labels: {
                    format: "{0}",
                },
                line: {
                    visible: false
                },
                axisCrossingValue: 0,
                majorUnit: undefined,
                min: 0,
                //max: 100,
            },
            tooltip: {
                visible: true,
                format: "{0}%",
                template: "#= series.name #: #= value #"
            },
        });

        // seriesClick 
        var chart = $("#" + chartId).data("kendoChart");
        if ((new Function(' return  typeof(' + chartId + '_seriesClick) == "function" ? ' + chartId + '_seriesClick : null  '))() != null) { // 사용자 정의 이벤트가 존재하면...
            chart.bind("seriesClick", (new Function(' return  typeof(' + chartId + '_seriesClick) == "function" ? ' + chartId + '_seriesClick : null  '))());
        }

        if (enlarge) {
            if ($("#" + chartId).find("#enlarge").length == 0) {
                $("#" + chartId).prepend('<button type="button" id="enlarge" class="k-i-zoom-in" style="position: absolute; right: 0; margin-top:8px;margin-right:4px;" ></button>');

                $("#enlarge").kendoButton({
                    icon: "zoom-in",
                    click: function (e) {
                        POP.braPopup();
                    }
                });
            }
        }
    },
};

// BRA Chart 
var BraChart = {
    color : ["#ff1c1c", "#ffae00", "#73c100", "#007eff", "#32CD32", "#00FFFF", "#00BFFF", "#FF1493",
        "#DA70D6", "#2F4F4F", "#808080", "#663399", "#1E90FF", "#40E0D0", "#008000", "#FFD700",
        "#E6E6FA", "#D8BFD8", "#DDA0DD", "#EE82EE", "#DA70D6", "#FF00FF", "#4B0082", "#483D8B",
    ],

    // chart 생성 
    createChart: function (chartId, data, enlarge, skipX, vis_nm, temp_nm, time_nm) {
        vis_nm = isnull(vis_nm, "VISCOSITY");
        temp_nm = isnull(temp_nm, "TEMPERATURE");
        time_nm = isnull(time_nm, "TIME_MS");

        var Viscosity_min = 0, Viscosity_max = 1000;
        var temp_min = 50, temp_max = 100;

        var color = BraChart.color;
 
        var categories = [];
        if (data.length > 0) { 
            categories = JSON.parse("[" + data[0][time_nm] + "]")
        } else {
            for (var i = 0; i <= 90; i++) { // x축 0분~ 90분 (1분단위)
                categories.push(i);
            }
        }
 
        // 10. Viscosity 
        var series = [];
        var ViscosityMaxMin = [];
        $(data).each(function (index, item) {
            var json = {
                type: "line",
                style: "smooth",
                markers: {
                    visible: false
                },
                data: JSON.parse("[" + item[vis_nm] + "]"),
                name: item.SAMPLE_NAME,
                color: (index < color.length ? color[index] : "red"),
                axis: "Viscosity"
            };
            series.push(json);

            // Viscosity 최대 최소값 담기 
            var data2 = json.data;
            ViscosityMaxMin.push(Math.min.apply(null, data2));
            ViscosityMaxMin.push(Math.max.apply(null, data2));
        });

        // Viscosity 최소, 최대값 
        Viscosity_min = Math.min.apply(null, ViscosityMaxMin);
        Viscosity_max = Math.max.apply(null, ViscosityMaxMin);


        // 20. 온도 
        var tempMaxMin = [];
        if (data.length > 0) {
            var json = {
                type: "line",
                style: "smooth",
                markers: {
                    visible: false
                },
                data: JSON.parse("[" + data[0][temp_nm] + "]"),
                name: "온도",
                color: "gray",
                axis: "temp"
            }
            series.push(json);

            // temp 최대 최소값 담기 
            var data2 = json.data;
            tempMaxMin.push(Math.min.apply(null, data2));
            tempMaxMin.push(Math.max.apply(null, data2));
        }

        // temp 최소, 최대값 
        temp_min = Math.min.apply(null, tempMaxMin);
        temp_max = Math.max.apply(null, tempMaxMin);

        //trace("max min : " + temp_min + " / " + temp_max + " / " +  Viscosity_min +  " / " + Viscosity_max)

        // 챠크 설정  
        $("#" + chartId).kendoChart({
            title: {
                text: "Brabender Graph",
            },
            legend: {
                position: "bottom"
            },
            series: series,
            valueAxis: [
                {
                    name: "temp",  // 온도 
                    color: "#007eff",
                    min: Math.floor(temp_min),
                    max: temp_max + Math.floor(temp_max*0.05),
                },
                {
                    name: "Viscosity", // 피크
                    min: 0, //Viscosity_min,
                    max: Viscosity_max + + Math.floor(Viscosity_max * 0.05),
                }
            ],
            categoryAxis: {
                categories: categories, //["0", "10", "20", "30", "40", "50", "60", "70", "80", "90"],
                labels: {
                    step: 5
                },
                // Align the first two value axes to the left
                // and the last two to the right.
                //
                // Right alignment is done by specifying a
                // crossing value greater than or equal to
                // the number of categories.
                axisCrossingValues: [temp_max, 0],
                justified: true
            },
            tooltip: {
                visible: true,
                format: "{0}",
                template: "#= category #/03: #= value #"
            },
            //toolbar: {
            //    buttons: [
            //        { name: "ButtonA", iconClass: "k-icon k-i-gear" },
            //        { name: "ButtonA", iconClass: "k-icon k-i-gear" }
            //    ]
            //}

            //seriesClick: function (e) {
            //    //trace("seriesClick : " + e.originalEvent.type);
            //    //if (e.originalEvent.type === "contextmenu") {
            //    //    // Disable browser context menu
            //    //    e.originalEvent.preventDefault();
            //    //}
            //    trace(e.category + " / " + e.value)
            //},
        });

        // seriesClick 
        var chart = $("#" + chartId).data("kendoChart");
        if ( (new Function(' return  typeof(' + chartId + '_seriesClick) == "function" ? ' + chartId + '_seriesClick : null  ')) () != null) { // 사용자 정의 이벤트가 존재하면...
            chart.bind("seriesClick", (new Function(' return  typeof(' + chartId + '_seriesClick) == "function" ? ' + chartId + '_seriesClick : null  '))());
        }

        if (enlarge) {
            if ($("#" + chartId).find("#enlarge").length == 0) {
                $("#" + chartId).prepend('<button type="button" id="enlarge" class="k-i-zoom-in" style="position: absolute; right: 0; margin-top:8px;margin-right:4px;" ></button>');

                $("#enlarge").kendoButton({
                    icon: "zoom-in",
                    click: function (e) {
                        POP.braPopup();
                    }
                });
            }
        }
 
    },
 
};

// LetAreaChart
var LetAreaChart = {
    color: ["#ff1c1c", "#ffae00", "#73c100", "#007eff", "#32CD32", "#00FFFF", "#00BFFF", "#FF1493",
        "#DA70D6", "#2F4F4F", "#808080", "#663399", "#1E90FF", "#40E0D0", "#008000", "#FFD700",
        "#E6E6FA", "#D8BFD8", "#DDA0DD", "#EE82EE", "#DA70D6", "#FF00FF", "#4B0082", "#483D8B",
    ],

    // chart 생성 
    createChart: function (chartId, data, enlarge, skipX, vis_nm, time_nm) {
        vis_nm = isnull(vis_nm, "AREA");
        time_nm = isnull(time_nm, "MEAS_RET_TIME");

        var Area_min = 0, Area_max = 1000;
        var color = LetAreaChart.color;

        // 팝업 시, 데이터 바인딩 시 _tmplType 조회 되지 않는 현상 Pass
        if (typeof (_tmplType) != "undefined") {
            if (_tmplType == 'LC_COMMON') {

                var categories = [];

                var time_max = 20; // sec
                var interval_time = 1 // sec

                var Time_Array = [];
                var CustomTime_Array = Array.from(Array(time_max * 10).keys()).map(value => { return value / 10; });
                var Area_Array = Array.from({ length: time_max * 10 }, () => 0);

                if (data.length > 0) {
                    $(data).each(function (index, item) {
                        Time_Array.push(item.MEAS_RET_TIME);
                        CustomTime_Array[Math.round(item.MEAS_RET_TIME * 10)] = item.MEAS_RET_TIME; // 반올림 값을 기준으로 입력
                        Area_Array[Math.round(item.MEAS_RET_TIME * 10)] = item.AREA;
                    });
                } else {
                    if (!skipX) {
                        for (var i = 0; i <= 20; i++) { // x축 0분~ 90분 (1분단위)
                            Time_Array.push(i);
                        }
                    }
                }

                // categories 세팅
                if (Math.max.apply(null, Time_Array) > time_max) {
                    categories = Time_Array;
                } else {
                    categories = CustomTime_Array;
                }

                // 10. Areas  
                var series = [];

                var json = {
                    type: "line",
                    style: "smooth",
                    markers: {
                        visible: false
                    },
                    data: Area_Array,
                    axis: "Area"
                };
                series.push(json);

                // Viscosity 최소, 최대값 
                Area_min = Math.min.apply(null, Area_Array);
                Area_max = Math.max.apply(null, Area_Array);

                // 챠트 설정  
                $("#" + chartId).kendoChart({
                    title: {
                        text: "Peak Graph",
                    },
                    legend: {
                        position: "bottom"
                    },
                    series: series,
                    valueAxis: [
                        {
                            title: {
                                text: "Area",
                                color: "#8E8E8E",
                            },
                            name: "Area", // 피크
                            min: 0, //Viscosity_min,
                            max: Area_max + Math.floor(Area_max * 0.05),
                        }
                    ],
                    categoryAxis: {
                        title: {
                            text: "RetTime",
                            color: "#8E8E8E",
                        },
                        categories: categories, // [0, 0.1, 0.2, ... , 19.8, 19.9, 20],
                        labels: {
                            step: interval_time * 10,
                            format: "{0:0}{1}",
                        },
                        axisCrossingValues: [0, 0],
                        justified: true,
                    },
                    tooltip: {
                        visible: true,
                        format: "{0}",
                        template: "RetTime: #=category# </br>Area: #=value#"
                    },

                });

            } else {
                var categories = [];
                var areas = [];
                if (data.length > 0) {
                    $(data).each(function (index, item) {
                        var ret = isnull(item.MEAS_RET_TIME, "0");
                        if (ret != "0") {
                            categories.push(item.MEAS_RET_TIME);
                            areas.push(item.AREA);
                        }
                    });
                } else {
                    if (!skipX) {
                        for (var i = 0; i <= 20; i++) { // x축 0분~ 90분 (1분단위)
                            categories.push(i);
                        }
                    }
                }

                // 10. Areas  
                var series = [];
                var json = {
                    type: "line",
                    style: "smooth",
                    markers: {
                        visible: false
                    },
                    data: areas,
                    //name: "",
                    axis: "Area"
                };
                series.push(json);

                // Viscosity 최소, 최대값 
                Area_min = Math.min.apply(null, areas);
                Area_max = Math.max.apply(null, areas);

                // 챠트 설정  
                $("#" + chartId).kendoChart({
                    title: {
                        text: "Peak Graph",
                    },
                    legend: {
                        position: "bottom"
                    },
                    series: series,
                    valueAxis: [
                        {
                            name: "Area", // 피크
                            min: 0, //Viscosity_min,
                            max: Area_max + Math.floor(Area_max * 0.05),
                        }
                    ],
                    categoryAxis: {
                        categories: categories, //["0", "10", "20", "30", "40", "50", "60", "70", "80", "90"],
                        labels: {
                            step: 4,
                            format: "{0:0}{1}",
                        },
                        axisCrossingValues: [0, 0],
                        justified: true,
                    },
                    tooltip: {
                        visible: true,
                        format: "{0}",
                        template: "RetTime: #=category# </br>Area: #=value#"
                    },


                    //seriesClick: function (e) {
                    //    //trace("seriesClick : " + e.originalEvent.type);
                    //    //if (e.originalEvent.type === "contextmenu") {
                    //    //    // Disable browser context menu
                    //    //    e.originalEvent.preventDefault();
                    //    //}
                    //    trace(e.category + " / " + e.value)
                    //},
                });
            }
        } else {
            var categories = [];
            var areas = [];
            if (data.length > 0) {
                $(data).each(function (index, item) {
                    var ret = isnull(item.MEAS_RET_TIME, "0");
                    if (ret != "0") {
                        categories.push(item.MEAS_RET_TIME);
                        areas.push(item.AREA);
                    }
                });
            } else {
                if (!skipX) {
                    for (var i = 0; i <= 20; i++) { // x축 0분~ 90분 (1분단위)
                        categories.push(i);
                    }
                }
            }

            // 10. Areas  
            var series = [];
            var json = {
                type: "line",
                style: "smooth",
                markers: {
                    visible: false
                },
                data: areas,
                //name: "",
                axis: "Area"
            };
            series.push(json);

            // Viscosity 최소, 최대값 
            Area_min = Math.min.apply(null, areas);
            Area_max = Math.max.apply(null, areas);

            // 챠트 설정  
            $("#" + chartId).kendoChart({
                title: {
                    text: "Peak Graph",
                },
                legend: {
                    position: "bottom"
                },
                series: series,
                valueAxis: [
                    {
                        name: "Area", // 피크
                        min: 0, //Viscosity_min,
                        max: Area_max + Math.floor(Area_max * 0.05),
                    }
                ],
                categoryAxis: {
                    categories: categories, //["0", "10", "20", "30", "40", "50", "60", "70", "80", "90"],
                    labels: {
                        step: 4,
                        format: "{0:0}{1}",
                    },
                    axisCrossingValues: [0, 0],
                    justified: true,
                },
                tooltip: {
                    visible: true,
                    format: "{0}",
                    template: "RetTime: #=category# </br>Area: #=value#"
                },


                //seriesClick: function (e) {
                //    //trace("seriesClick : " + e.originalEvent.type);
                //    //if (e.originalEvent.type === "contextmenu") {
                //    //    // Disable browser context menu
                //    //    e.originalEvent.preventDefault();
                //    //}
                //    trace(e.category + " / " + e.value)
                //},
            });
        }

        // seriesClick 
        var chart = $("#" + chartId).data("kendoChart");
        if ((new Function(' return  typeof(' + chartId + '_seriesClick) == "function" ? ' + chartId + '_seriesClick : null  '))() != null) { // 사용자 정의 이벤트가 존재하면...
            chart.bind("seriesClick", (new Function(' return  typeof(' + chartId + '_seriesClick) == "function" ? ' + chartId + '_seriesClick : null  '))());
        }

        if (enlarge) {
            if ($("#" + chartId).find("#enlarge").length == 0) {
                $("#" + chartId).prepend('<button type="button" id="enlarge" class="k-i-zoom-in" style="position: absolute; right: 0; margin-top:8px;margin-right:4px;" ></button>');

                $("#enlarge").kendoButton({
                    icon: "zoom-in",
                    click: function (e) {
                        POP.braPopup();
                    }
                });
            }
        }

    },

};

/***************************************************************************************
// analysis template 
***************************************************************************************/
var Templ = {
    config: { // important !!! 템플릿별 레이아웃 및 분석로직 설정

        // important !!!
        // 주의) LAYOUT 동일한 유형이라도, 각 Pane 별 데이터가 다를수 있으므로, 템플릿유형으로 로직을 구분하고 있음. 
        // 1. config.ANALYSYS 항목은 화면단 apply_post >> Templ.setItemGrid 를 호출할때 사용된다.
        // 2. Templ.setItemGrid@@@ 를 호출할 경우는 템플릿유형(_tmplType) 으로 구분한다.
        // ### 템플릿유형을 추가할 경우  
        // ==> 1. 템플릿유형으로 AnalExpResult.cshtml / DSELN.js 에서 검색하여 하드코딩된 부분을 확인할것. 
        // ==> 2. VISCO_RVA / VISCO_BRA_COMPETITOR 의 챠트데이터는 ELN_IF.VW_BRA_PEAK (DB View Object) 에서 미리 가공해서 가져온다. 여기도 참조할것.

        // A : grid(measeRetTime) 
        LC_42A: { LAYOUT: "A", ANALYSYS: "LC_42A" },  
        LC_NH2: { LAYOUT: "A", ANALYSYS: "LC_NH2" },
        LC_87C: { LAYOUT: "A", ANALYSYS: "LC_87C" },
 
        // C : grid(measeRetTime) / chart 
        LC_MK6: { LAYOUT: "C", ANALYSYS: "BASIC" },   
        LC_MK7: { LAYOUT: "C", ANALYSYS: "BASIC" },
        LC_COMMON: { LAYOUT: "C", ANALYSYS: "BASIC" },

        // D : grid(number of cycle)  
        LC_TASTER: { LAYOUT: "D", ANALYSYS: "LC_TASTER" },  

        // E : grid(PK)  
        GC_FRAGRANCE: { LAYOUT: "E", ANALYSYS: "GC_FRAGRANCE" },

        // B : grid(호화개시온도) / bra chart / grid (peak data)
        VISCO_BRA: { LAYOUT: "B", ANALYSYS: "VISCO_BRA" },
        VISCO_BRA_COMPETITOR: { LAYOUT: "B", ANALYSYS: "VISCO_BRA_COMPETITOR" },   

        // F : grid(호화개시온도) 
        VISCO_RVA: { LAYOUT: "F", ANALYSYS: "VISCO_RVA" },
        VISCO_RVA_COMPETITOR: { LAYOUT: "F", ANALYSYS: "VISCO_RVA_COMPETITOR" },

        // G : grid(LC_F - Compound) / grid(measeRetTime) / chart
        LC_F_SUGAR: { LAYOUT: "G", ANALYSYS: "BASIC" },
        LC_F_ORGANIC_ACID: { LAYOUT: "G", ANALYSYS: "BASIC" },
        LC_F_CAPSAICIN: { LAYOUT: "G", ANALYSYS: "BASIC" },
        LC_F_AMINO: { LAYOUT: "G", ANALYSYS: "BASIC" },
        
    },

    floor: function (value, digit) {
        digit = Math.pow(10, digit);
        return Math.floor(value * digit) / digit;
    },

    keyReplace: function (key) {
        key = key.replace(/-/gi, "_");  // dash --> under score
        key = key.replace(/ /gi, "_");  // blank --> under score
        key = key.replace(/\//gi, "_");  // slash --> under score
        return key;
    },
 
    getLayOutType: function (tmplType) {
        //trace("tmplType : " + tmplType);
        var config = Templ.config[tmplType];  //(tmplType == "VISCO_BRA" ? "B" : (tmplType == "LC_SUGAR" ? "C" : "A"));
        if (config == undefined) {
            alert(tmplType + " 템플릿에 대한 환경설정 정보가 없습니다.");
            return null;
        }
        return config.LAYOUT;
    }, 

    findBRA: function (gridId, x, y) {
        var grid = $('#' + gridId).data('kendoGrid');
 
        var dsCopied = new kendo.data.DataSource({
            data: grid.dataSource._data,
            filter: {
                // leave data items which are "Food" or "Tea"
                logic: "and",
                filters: [
                    { field: "Minute", operator: "eq", value: x },
                    { field: "Viscosity", operator: "startswith", value: y},
                ]
            },
        });
        dsCopied.read();
        //trace("findBRA : " + x + "/" + y + " / " + dsCopied._view.length)
    },

    // for GC_FRAGRANCE (향기)
    setItemGrid4FRAG: function (svid, tmplType, gridId, frangRow, peakJson, dsItemNm, sample_Id, analysis_Id, jsonVisc, jsonTemp, jsonTime, _sampleName) {
        // 1010. 분석 Item 
        var grid = $('#' + gridId).data('kendoGrid');
        var detailData = [];
        var dataJson = []; 
        var idx = 0;

        // 저장여부 확인 
        var isSave = false;
        if (typeof (frangRow) == "object" && frangRow.length > 0) {
            isSave = true;
            dataJson = frangRow;
        }

        // 자동설정 호출시 
        if (svid == "R") isSave = false;

        if (!isSave) { // 저장 전이면 Peak json 참조 
            dataJson = [];
            if (typeof (peakJson) == "object" && peakJson.length == undefined) {  // peakJson  --> {} 로 들어오면...
                dataJson.push(peakJson);
            } else {
                dataJson = peakJson;
            }
        }

        // 10. peak json 으로 row data 만들기 
        // Templ.keyReplace(item.ITEM_NAME) 기준으로 키값을 변경할것.  ex) Number of Cycle --> Number_of_Cycle
        $.each(dataJson, function (key, value) {
            detailData.push({
                PK: (isSave ? value["PK"] : value["PK"]),
                RT: (isSave ? value["RT"] : value["RT"]),
                AREA_PCT: (isSave ? value["AREA_PCT"] : value["Area Pct"]),
                LIBRARY_ID: (isSave ? isnull(value["LIBRARY_ID"], "") : isnull(value["Library/ID"], "")),  
                REF: (isSave ? isnull(value["REF"], "") : isnull(value["Ref"], "")),
                CAS: (isSave ? isnull(value["CAS"], "") : isnull(value["CAS"], "")),
                QUAL: (isSave ? value["QUAL"] : value["Qual"]),

                SAMPLE_ID: sample_Id,
                SAMPLE_NAME: _sampleName,
                ANAL_ID: analysis_Id,
                ITEM_ID: 0,
            });
        });
        
        grid.dataSource.data(detailData);
        grid.dataSource.options.data = detailData;  // for sorting ... 
        grid.dataSource.transport.data = detailData;
    },

    // for LC_TASTER
    setItemGrid4TASTER: function (svid, tmplType, gridId, result0, peakJson, dsItemNm, sample_Id, analysis_Id, jsonVisc, jsonTemp, jsonTime, _sampleName) {
        // 1010. 분석 Item 
        var grid = $('#' + gridId).data('kendoGrid');
        var detailData = [];
        var idx = 0;

        // 10. peak json 으로 row data 만들기 
        // Templ.keyReplace(item.ITEM_NAME) 기준으로 키값을 변경할것.  ex) Number of Cycle --> Number_of_Cycle
        $.each(peakJson, function (key, value) {
            // 주의) 필드id 대소문자 구분할것. 
            detailData.push({
                Sample: value.Sample,
                Number_of_Cycle: value["Number of Cycle"],  // json key : Number of Cycle   
                Sourness: value.Sourness,
                Bitterness: value.Bitterness,
                Astringency: value.Astringency,

                Aftertaste_B: value["Aftertaste-B"],  // json key : Aftertaste-B
                Aftertaste_A: value["Aftertaste-A"], // json key : Aftertaste-A
                Umami: value.Umami,
                Richness: value.Richness,
                Saltiness: value.Saltiness,

                ITEM_ID: "DUMMY" + (idx++),
                TASTER_ITEM_NAME: value["Number of Cycle"],
            });
        });
        
        grid.dataSource.data(detailData);
        grid.dataSource.options.data = detailData;  // for sorting ... 
        grid.dataSource.transport.data = detailData;
 
        // 30. avg --> area  저장 여부 
        var isSave = false; 
        $.each(result0, function (index, item) {
            var value = isnull(item.AREA, 0);
            if (value != 0) {
                isSave = true; 
                return false; 
            }
        });

        // 40. avg row 
        if (isSave) {
            // 저장된 값 표시 
            Templ.setautoAnalysis_LC_TASTER(svid, gridId, result0);  // 자동설정
        } else {
            Templ.setautoAnalysis_LC_TASTER("R", gridId, null);  // 자동설정 
        }
 
    },

    setautoAnalysis_LC_TASTER: function (svid, gridId, detailRow) { // 자동설정
        var grid = $('#' + gridId).data('kendoGrid');

        // 10. 기존 평균행이 존재하면 삭제 
        var dsMatched = new kendo.data.DataSource({
            data: grid.dataSource._view,
            filter: {
                // leave data items which are "Food" or "Tea"
                logic: "or",
                filters: [
                    { field: "Number_of_Cycle", operator: "eq", value: 0 },
                    { field: "Number_of_Cycle", operator: "eq", value: "0" },
                ]
            },
        });
        dsMatched.read(); // important !!!

        if (dsMatched._view.length > 0) {
            var uid = dsMatched._view[0].uid;
            trace("delete uid :" + uid + " /  " + dsMatched._view.length)
            var dataRow = grid.dataSource.getByUid(uid);
            grid.dataSource.remove(dataRow);
        }

         // 20. 평균 집계행 추가 (단, 1행은 제외하고 집계)
        var dsCopied = null; 
        if (svid == "R") { // 자동설정 
            dsCopied = new kendo.data.DataSource({
                data: grid.dataSource._view,
                filter: {
                    // leave data items which are "Food" or "Tea"
                    logic: "and",
                    filters: [
                        { field: "Number_of_Cycle", operator: "neq", value: 1 },
                        { field: "Number_of_Cycle", operator: "neq", value: "1" },
                    ]
                },
                aggregate: [
                    { field: "Sourness", aggregate: "average" },
                    { field: "Bitterness", aggregate: "average" },
                    { field: "Astringency", aggregate: "average" },
                    { field: "Aftertaste_B", aggregate: "average" },
                    { field: "Aftertaste_A", aggregate: "average" },
                    { field: "Umami", aggregate: "average" },
                    { field: "Richness", aggregate: "average" },
                    { field: "Saltiness", aggregate: "average" },
                ],
            });
            dsCopied.read(); // important !!!

            trace("dsCopied : " + dsCopied._view.length)

        } else { // 저장된 데이터 표시 
            dsCopied = new kendo.data.DataSource({  // 스키마 복사용 
                data: [],
                aggregate: [
                    { field: "Sourness", aggregate: "average" },
                    { field: "Bitterness", aggregate: "average" },
                    { field: "Astringency", aggregate: "average" },
                    { field: "Aftertaste_B", aggregate: "average" },
                    { field: "Aftertaste_A", aggregate: "average" },
                    { field: "Umami", aggregate: "average" },
                    { field: "Richness", aggregate: "average" },
                    { field: "Saltiness", aggregate: "average" },
                ],
            });
            dsCopied.read(); // important !!!

            var json = { Sample: "", Number_of_Cycle: 0, ITEM_ID: "", TASTER_ITEM_NAME:""};
            $.each(detailRow, function (index, item) {
                var itemName = Templ.keyReplace(item.ITEM_NAME);  
                var value = isnull(item.AREA, 0);
                //trace("itemName : " + itemName + "/" + value)
                json[itemName] = value;
            });

            dsCopied.add(json);
            dsCopied.sync(); // important !!!
        }
 
        // 30. 평균값을 grid 에 추가 
        grid.dataSource.add(
            {
                Sample: dsCopied._view[0].Sample,
                Number_of_Cycle: 0,  // 0 == AVG 
                ITEM_ID: "DUMMY" + (grid.dataSource._view.length),
                TASTER_ITEM_NAME: "평균 (1행제외)",

                Sourness: Templ.floor(dsCopied.aggregates().Sourness.average, 2),
                Bitterness: Templ.floor(dsCopied.aggregates().Bitterness.average, 2),
                Astringency: Templ.floor(dsCopied.aggregates().Astringency.average, 2),
                Aftertaste_B: Templ.floor(dsCopied.aggregates().Aftertaste_B.average, 2),
                Aftertaste_A: Templ.floor(dsCopied.aggregates().Aftertaste_A.average, 2),
                Umami: Templ.floor(dsCopied.aggregates().Umami.average, 2),
                Richness: Templ.floor(dsCopied.aggregates().Richness.average, 2),
                Saltiness: Templ.floor(dsCopied.aggregates().Saltiness.average, 2),
            }
        );
        ////grid.dataSource.sync();
    },

    // for LC_F
    setItemGrid4LC_F: function (svid, tmplType, gridId, result0, peakJson, dsItemNm, sample_Id, analysis_Id, compoundjson, Compound_Peak) {
        var grid = $('#' + gridId).data('kendoGrid');

        trace("defined....OK : " + tmplType + " / " + sample_Id + " / " + analysis_Id)

        var tmplTypeRedefined = Templ.config[tmplType].ANALYSYS;
        if (tmplTypeRedefined == "NOT_YET" || tmplTypeRedefined == "") {
            trace("defined....not yet : " + tmplType + " / " + sample_Id + " / " + analysis_Id)
            alert("not...yet......");

            grid.dataSource.data([]); // detail clear
            grid.dataSource.sync();
            return;
        }

        if (svid == "R" && tmplType == "LC_NH2") { // 자동설정 
            Templ.setautoAnalysis_LC_NH2(gridId, tmplType, result0, (new Function(' return ' + dsItemNm))())
            return;
        }

        // 1000. peak data 
        if (peakJson == undefined || peakJson.length == 0) {
            trace("peakJson >>>>> not existed...");
            grid.dataSource.data([]); // detail clear
            grid.dataSource.sync();
            return;
        }

        // 1010. 분석항목 콤보 구성 (분석결과 샘플분석항목 row 데이터 기준)
        _dsItem = new kendo.data.DataSource({ data: [], });
        var ddlDs = [{ TEXT: "", VALUE: "", ITEM_NAME: "" }];  // empty row 

        $.each(result0, function (key, value) {
            ddlDs.push({
                //TEXT: value.ITEM_NAME,
                TEXT: value.TITLE,
                VALUE: value.ITEM_ID,
                ITEM_NAME: value.ITEM_NAME.toLowerCase(),
                TITLE: value.TITLE,
            });
        });
        _dsItem.data(ddlDs);

        // LC_F 분석 항목 자동 설정
        // Compound_Peak 연관 관계 정의
        if (Compound_Peak) {
            var Arr_Compound_Peak = Object.entries(Compound_Peak[0])

            $.each(Arr_Compound_Peak, function (key, value) {
                if (value[0].includes(' ')) { // 'aspartic acid' 와 'glutamic acid' 같이 스페이스 들어간 것 ' ' > '' 처리
                    value[0] = value[0].replace(' ', '');
                } else if (value[0] == 'gaba') { // gaba 항목은 gammaaminoacid 로 변경하여 비교
                    value[0] = "gammaaminoacid"
                }

                var compound_ddl = ddlDs.find(v => v.ITEM_NAME === value[0]) // 분석항목 ddlDs 에서 ITEM_NAME 이 value[0] (Compound 시트의 Name) 와 같은 ddl 조회 (compound_ddl)

                if (compound_ddl) { // compound_ddl 이 존재할 경우, peakJson에 Compound DDL value 와 text 저장
                    $.each(peakJson, function (pk_key, pk_value) {
                        if (pk_value.Compound == value[1]) { // Peak Json 의 Compound 값과 value[1] (Compound 시트의 FirstPeak) 와 같을 경우
                            peakJson[pk_key].Compound_DDLValue = compound_ddl.VALUE
                            peakJson[pk_key].Compound_DDLTEXT = compound_ddl.ITEM_NAME
                        }
                    });

                }

            });
        };

        var ddlDataSource = (new Function(' return ' + dsItemNm))();
        ddlDataSource = _dsItem;

        // 2010. Sample PEAKS josn to gridDetail  
        var detailData = [];

        $.each(peakJson, function (key, value) {
            detailData.push({  // ###SAVE_ITEMS : 저장항목 동기화 할것. 
                ITEM_ID: value.Compound_DDLValue, // LC_F 분석항목 커스텀 자동 설정
                ITEM_NAME: value.Compound_DDLTEXT, // LC_F 분석항목 커스텀 자동 설정
                TITLE: "",
                MEAS_RET_TIME: value.MeasRetTime,
                AREA: value.Area,
                INT_PEAK_TYPE: value.IntPeakType,
                SAMPLE_ID: sample_Id,
                ANAL_ID: analysis_Id,
                PCT_AREA: 0,
                DATA_JSON: "",
            });
        });

        grid.dataSource.data(detailData);
        grid.dataSource.options.data = detailData;  // for sorting ... 
        grid.dataSource.transport.data = detailData;
        ////grid.setDataSource(detailData);

        //3010. 분석항목 설정 
        if (tmplTypeRedefined == "BASIC") {
            Templ.setItemGrid4Basic(svid, grid, result0, tmplType, null, null); // 근사치 자동 매칭
        } else if (tmplTypeRedefined == "LC_42A") {
            Templ.setItemGrid4LC_42A(svid, grid, result0, tmplType); // DP10 ~~ 역순으로 자동 매칭
        } else if (tmplTypeRedefined == "LC_87C") {
            Templ.setItemGrid4LC_87C(svid, grid, result0, tmplType); // 사용자가 선택
        } else if (tmplTypeRedefined == "LC_NH2") {
            Templ.setItemGrid4LC_NH2(svid, grid, result0, tmplType);  // 저장된 값을 표시
            Templ.setautoAnalysis_LC_NH2(gridId, tmplType, result0, (new Function(' return ' + dsItemNm))()) // 자동설정으로 ISO , DP8_plus 다시 설정
        }

        // 4010. Area Rate(%)
        if (grid.dataSource.data().length > 0) {
            Templ.setAreaRate(svid, gridId, tmplType, result0);  // svid == "C", "R 자동설정 " 
        }

        grid.refresh(); // refresh ... 

        // LayOut G : 챠트  
        var layoutType = Templ.config[tmplType].LAYOUT;

        if (layoutType == "G") {
            LetAreaChart.createChart("chart", grid.dataSource._view, true);
        }

        // 3010. Compund data 
        var grid = $('#' + "gridLC_F").data('kendoGrid');

        // Compound grid data 전처리
        var detailData = [];
        var detailValue = {};
        var edit_value = {};

        $.each(compoundjson, function (key, value) {
            var ITEM_ID_obj = { 'ITEM_ID': key + 1 };
            
            // column field 에 스페이스가 있을 경우, 오류 발생으로 인한 Object key 값을 변경하여 object 에 추가
            // gridLC_F 그리드 행열에 뿌리는 데이터
            $.each(value, function (key2, value2) {
                if (key2.includes(' ')) { // 'aspartic acid' 와 'glutamic acid' 같이 스페이스 들어간 것 ' ' > '_' 처리 
                    edit_value[key2.replace(' ', '_')] = value2;
                } else {
                    edit_value[key2] = value2;
                }
            });

            detailValue = Object.assign(ITEM_ID_obj, edit_value);

            // detailValue 를 detailData 담기
            detailData.push(detailValue);
        });

        grid.dataSource.data(detailData);
        grid.dataSource.options.data = detailData;  // for sorting ...
        grid.dataSource.transport.data = detailData;

        grid.dataSource.sync();
    },

    // for VISCO_BRA
    setItemGrid4BRA: function (svid, tmplType, gridId, result0, peakJson, dsItemNm, sample_Id, analysis_Id, jsonVisc, jsonTemp, jsonTime, _sampleName) {
        // 1010. 분석 Item 
        var grid = $('#' + gridId).data('kendoGrid');
        var detailData = []; 
        var dataJson = isnull(result0[0].DATA_JSON, "");
 
        if (dataJson == "") {
            dataJson = []; 
        } else {
            dataJson = JSON.parse(dataJson);
        }
 
        if (typeof (dataJson) == "object" && dataJson.length == undefined) { // dataJson  --> {} 로 들어오면...
            dataJson.push(dataJson);
        } else {
            ////dataJson = dataJson;
        }

        $.each(dataJson, function (index, item) {
            // key 대소문자 구분할것 
            detailData.push({  
                Peak: item.Peak,
                BoG: item.BoG,
                Final: item.Final,
                BD: item.BD,
                SB: item.SB,

                // 업데이트용 정보 담기  : // ###SAVE_ITEMS : 저장항목 동기화 할것. 
                ITEM_ID: result0[0].ITEM_ID,
                SAMPLE_ID: sample_Id,
                ANAL_ID: analysis_Id,
                MEAS_RET_TIME: result0[0].MEAS_RET_TIME,
                AREA: result0[0].AREA,
                TITLE: result0[0].TITLE,
                PCT_AREA: result0[0].PCT_AREA,
                DATA_JSON: result0[0].DATA_JSON,
            });
        });

        grid.dataSource.data(detailData);
        grid.dataSource.options.data = detailData;  // for sorting ... 
        grid.dataSource.transport.data = detailData;

        // 2010. chart 
        var seriesJson = [];
        seriesJson.push({
            VISCOSITY: jsonVisc,
            TEMPERATURE: jsonTemp,
            TIME_MS: jsonTime,
            SAMPLE_NAME: _sampleName,
        });

        var chartId = "chart";
        BraChart.createChart(chartId, seriesJson, true);


        // 3010. BRA data 
        var grid = $('#' + "gridBRA").data('kendoGrid');
        grid.dataSource.page(1);  // new data 

        grid.dataSource.data(peakJson);
        grid.dataSource.options.data = peakJson;  // for sorting ...
        grid.dataSource.transport.data = peakJson;

        var rows = grid.dataSource.data();
        $(rows).each(function (index, item) {
            if (peakJson[0].ID == undefined) {
                grid.dataSource.data()[index].ID = (index + 1);
            }
            grid.dataSource.data()[index].SAMPLE_NAME = _sampleName;
        });
 
        grid.dataSource.sync();
 
    },

    // for VISCO_RVA
    setItemGrid4RVA: function (svid, tmplType, gridId, result0, peakJson, dsItemNm, sample_Id, analysis_Id, jsonVisc, jsonTemp, jsonTime, _sampleName) {
        // 1010. 분석 Item 
        var grid = $('#' + gridId).data('kendoGrid');
        var detailData = [];
        var dataJson = isnull(result0[0].DATA_JSON, "");
        var isSave = false;

        if (dataJson == "") {
            isSave = false; 
        } else {
            dataJson = JSON.parse(dataJson);  // isSave
            if (dataJson[0].Peak == null || dataJson[0].Peak == "null") {
                isSave = false; 
            } else { 
                isSave = true; 
            }
        }

        if (svid == "R") isSave = false;  // 자동설정 호출시 

        // DATA_JSON: result0[0].DATA_JSON 에 값이 들어 있으면, 분석항목 저장건으로 간주해서 DATA_JSON 값을 그리드에 표시한다. 
        if (!isSave) { // 저장 전이면 Peak json 참조 
            dataJson = [];
            if (typeof (peakJson) == "object" && peakJson.length == undefined) {  // peakJson  --> {} 로 들어오면...
                dataJson.push(peakJson);
            } else {
                dataJson = peakJson;
            }
        }
 
        $.each(dataJson, function (index, item) {
            trace("item.Peak :  " + item.Peak);
            var DATA_JSON = [{
                Peak: (isSave ? item["Peak"] : item["Peak 1"]),
                BoG: (isSave ? item["BoG"] : item["Pasting Temp"]),
                Final: (isSave ? item["Final"] : item["Final Visc"]),
                BD: (isSave ? item["BD"] : item["Breakdown"]),
                SB: (isSave ? item["SB"] : item["Setback"]),
            }];

            // key 대소문자 구분할것 
            detailData.push({
                Peak: (isSave ? item["Peak"] : item["Peak 1"]),
                BoG: (isSave ? item["BoG"] : item["Pasting Temp"]),
                Final: (isSave ? item["Final"] : item["Final Visc"]),
                BD: (isSave ? item["BD"] : item["Breakdown"]),
                SB: (isSave ? item["SB"] : item["Setback"]),

                // 업데이트용 정보 담기  : // ###SAVE_ITEMS : 저장항목 동기화 할것. 
                ITEM_ID: result0[0].ITEM_ID,
                SAMPLE_ID: sample_Id,
                ANAL_ID: analysis_Id,
                MEAS_RET_TIME: result0[0].MEAS_RET_TIME,
                AREA: result0[0].AREA,
                TITLE: result0[0].TITLE,
                PCT_AREA: result0[0].PCT_AREA,
                DATA_JSON: JSON.stringify(DATA_JSON),
            });
        });
 
        grid.dataSource.data(detailData);
        grid.dataSource.options.data = detailData;  // for sorting ... 
        grid.dataSource.transport.data = detailData;
    },

    // template 
    setItemGrid: function (svid, tmplType, gridId, result0, peakJson, dsItemNm, sample_Id, analysis_Id) {
        var grid = $('#' + gridId).data('kendoGrid');

        trace("defined....OK : " + tmplType + " / " + sample_Id + " / " + analysis_Id)

        var tmplTypeRedefined = Templ.config[tmplType].ANALYSYS;
        if (tmplTypeRedefined == "NOT_YET" || tmplTypeRedefined == "") {
            trace("defined....not yet : " + tmplType + " / " + sample_Id + " / " + analysis_Id)
            alert("not...yet......");

            grid.dataSource.data([]); // detail clear
            grid.dataSource.sync();
            return;
        }

        if (svid == "R" && tmplType == "LC_NH2") { // 자동설정 
            Templ.setautoAnalysis_LC_NH2(gridId, tmplType, result0, (new Function(' return ' + dsItemNm))())
            return;
        } 

        // 1000. peak data 
        if (peakJson == undefined || peakJson.length == 0) {
            trace("peakJson >>>>> not existed...");
            grid.dataSource.data([]); // detail clear
            grid.dataSource.sync();
            return;
        }

        // 1010. 분석항목 콤보 구성 (분석결과 샘플분석항목 row 데이터 기준)
        _dsItem = new kendo.data.DataSource({ data: [], });
        var ddlDs = [{ TEXT: "", VALUE: "", ITEM_NAME: "" }];  // empty row 

        $.each(result0, function (key, value) {
            ddlDs.push({
                //TEXT: value.ITEM_NAME,
                TEXT: value.TITLE,
                VALUE: value.ITEM_ID,
                ITEM_NAME: value.ITEM_NAME,
                TITLE: value.TITLE,
            });
        });

        _dsItem.data(ddlDs);

        var ddlDataSource = (new Function(' return ' + dsItemNm))();
        ddlDataSource = _dsItem;

        // 2010. Sample PEAKS josn to gridDetail  
        var detailData = [];
        $.each(peakJson, function (key, value) {
            detailData.push({  // ###SAVE_ITEMS : 저장항목 동기화 할것. 
                ITEM_ID: "",
                ITEM_NAME: "",
                TITLE: "",
                MEAS_RET_TIME: value.MeasRetTime,
                AREA: value.Area,
                INT_PEAK_TYPE: value.IntPeakType,
                SAMPLE_ID: sample_Id,
                ANAL_ID: analysis_Id,
                PCT_AREA: 0,
                DATA_JSON: "",
            });
        });
        grid.dataSource.data(detailData);
        grid.dataSource.options.data = detailData;  // for sorting ... 
        grid.dataSource.transport.data = detailData;
        ////grid.setDataSource(detailData);

        //3010. 분석항목 설정 
        if (tmplTypeRedefined == "BASIC") {
            Templ.setItemGrid4Basic(svid, grid, result0, tmplType, null, null); // 근사치 자동 매칭
        } else if (tmplTypeRedefined == "LC_42A") {
            Templ.setItemGrid4LC_42A(svid, grid, result0, tmplType); // DP10 ~~ 역순으로 자동 매칭
        } else if (tmplTypeRedefined == "LC_87C") {
            Templ.setItemGrid4LC_87C(svid, grid, result0, tmplType); // 사용자가 선택
        } else if (tmplTypeRedefined == "LC_NH2") {
            Templ.setItemGrid4LC_NH2(svid, grid, result0, tmplType);  // 저장된 값을 표시
            Templ.setautoAnalysis_LC_NH2(gridId, tmplType, result0, (new Function(' return ' + dsItemNm))()) // 자동설정으로 ISO , DP8_plus 다시 설정
        }

        // 4010. Area Rate(%)
        if (grid.dataSource.data().length > 0) {
            Templ.setAreaRate(svid, gridId, tmplType, result0);  // svid == "C", "R 자동설정 " 
        }

        grid.refresh(); // refresh ... 

        // LayOut C : 챠트  
        var layoutType = Templ.config[tmplType].LAYOUT;
        if (layoutType == "C") {
            LetAreaChart.createChart("chart", grid.dataSource._view, true);
        }

    },

    isBeforeSave: function (result0) {
        // 데이터 저정 전 여부 
        var beforeSave = true;
        if (result0.length == 0) {
            return null;
        } else {
            for (var i = 0; i < result0.length; i++) {
                var value = isnull(result0[i].MEAS_RET_TIME, 0);  // 화면에서 모두 clear 하고 저장하면??? 이걸 저장전으로 간주????
                if (value > 0) {
                    beforeSave = false;
                    break;
                }
            }
        }

        return beforeSave;
    },

    isBeforeSave4Rate: function (result0) { // 비율을 db 에 저장하기 이전  데이터를 위해 ... 
        // 데이터 저정 전 여부 
        var beforeSave = true;
        if (result0.length == 0) {
            return null;
        } else {
            for (var i = 0; i < result0.length; i++) {
                var value = isnull(result0[i].PCT_AREA, 0);  // 비율 저장여부 
                if (value > 0) {
                    beforeSave = false;
                    break;
                }
            }
        }

        return beforeSave;
    },

    setItemGrid4Basic: function (svid, grid, result0, tmplType, matchConst, leakSkip) {
        leakSkip = isnull(leakSkip, false);

        var beforeSave = Templ.isBeforeSave(result0);  // 저장전 여부 
        if (beforeSave == null) return;

        // json --> list 중 테이블에 저장된 또는 근사치값을 기준으로  분석항목 설정 
        var dsCopied = new kendo.data.DataSource({ data: grid.dataSource._view, });
        $.each(result0, function (key, value) {
            var ret = isnull(value.RET_TIME, 0);  // ITEM > RET_TIME   
            var retMeas = isnull(value.MEAS_RET_TIME, 0);
            var area = isnull(value.AREA, 0);
            var itemId = isnull(value.ITEM_ID, 0);
            var itemName = isnull(value.ITEM_NAME, "");
            var title = isnull(value.TITLE, "");
            var dataJson = isnull(value.DATA_JSON, "").trim();

            //trace("setItemGrid4Basic >>>> " + svid + " / " + beforeSave + " / " + itemName + " / " + retMeas)

            if (tmplType == "LC_87C") {  // 저장시 집계 --> 조회시 DATA_JSON 에 저장된 리스트로 데이터를 분리해서 설정해준다. 
                if (dataJson == "") {
                    Templ.setItemGrid4BasicStep2(svid, beforeSave, matchConst, ret, retMeas, dsCopied, grid, leakSkip, itemId, itemName, title, dataJson);
                } else {
                    var jsonObj = JSON.parse(dataJson);
                    $.each(jsonObj, function (key2, value2) {
                        var retMeas2 = isnull(value2.MEAS_RET_TIME, 0);
                        var area2 = isnull(value2.AREA, 0);

                        Templ.setItemGrid4BasicStep2(svid, beforeSave, matchConst, ret, retMeas2, dsCopied, grid, leakSkip, itemId, itemName, title, dataJson);
                    });
                }
            } else {
                Templ.setItemGrid4BasicStep2(svid, beforeSave, matchConst, ret, retMeas, dsCopied, grid, leakSkip, itemId, itemName, title, dataJson);
            }
        });
    },

    setItemGrid4BasicStep2: function (svid, beforeSave, matchConst, ret, retMeas, dsCopied, grid, leakSkip, itemId, itemName, title, dataJson) {
       
        if (svid == "R" || beforeSave == true) { // 자동설정 클릭시
            // RET_TIME 기준으로 자동 분석 
            trace("setItemGrid4BasicStep2 >>>>> " + ret + " / " + retMeas)
            if (ret == "0") return;
        } else {
            ret = retMeas;  // 저장된 MEAS_RET_TIME 값 기준으로 ..
            if (ret == "0") return;
        }

        dsCopied.filter({ field: "MEAS_RET_TIME", operator: "eq", value: ret }); // 일치

        var dsMatched = new kendo.data.DataSource({
            data: [],
            aggregate: [
                { field: "DIFF", aggregate: "min" },
                { field: "DIFF", aggregate: "max" }
            ]
        });

        //trace("setItemGrid4Basic2 >>>> " + dsCopied.view().length + " / " + itemName + " / " + retMeas)

        if (dsCopied.view().length != 1) {  // 일치하는 값 1개가 아니면 근사값 찾기 , view 로 비교할것. 
            var ret2 = ret;  //Math.floor(ret * 100)/100; 
            for (var x = 0; x < grid.dataSource._view.length; x++) {
                var ret3 = isnull(grid.dataSource._view[x].MEAS_RET_TIME, 0);

                matchConst = isnull(matchConst, "PM0.5");
                if (Templ.isMatched(matchConst, ret2, ret3)) { // +- 0.5
                    dsMatched.add({ INDEX: x, RET_TIME: ret3, DIFF: Math.abs(ret3 - ret2) });
                }
            }

            //trace("dsMatched.data.length : " + dsMatched.data().length)

            // dsMatched  :  data & view 기준 
            // grid.dataSource : view 기준으로 
            if (dsMatched.data().length == 1) {
                var rowIdx = dsMatched.view()[0].INDEX;
                grid.dataSource._view[rowIdx].ITEM_ID = itemId;
                grid.dataSource._view[rowIdx].ITEM_NAME = itemName;
                grid.dataSource._view[rowIdx].TITLE = title;
                //grid.dataSource._view[rowIdx].DATA_JSON = dataJson;
            } else {
                var aggregates = dsMatched.aggregates().DIFF;
                dsMatched.filter({ field: "DIFF", operator: "eq", value: aggregates.min }); // min diff row

                if (dsMatched.view().length == 1) { // 필터 결과 1건인 경우  view 로 비교할것. 
                    var rowIdx = dsMatched.view()[0].INDEX;
                    grid.dataSource._view[rowIdx].ITEM_ID = itemId;
                    grid.dataSource._view[rowIdx].ITEM_NAME = itemName;
                    grid.dataSource._view[rowIdx].TITLE = title;
                    //grid.dataSource._view[rowIdx].DATA_JSON = dataJson;
                } else {
                    if (!leakSkip) { // LC_87C skip 
                        if (dsMatched.view().length == 0) {
                            //alert("자동설정항목 0건 존재합니다. [" + title + "]"); // not used...
                            //return false;
                        } else {
                            //alert("자동설정항목 2건 이상 존재합니다. [" + title + "]"); // not used...
                            //return false;
                        }
                    }
                }
            }

        } else { // matched one   8.50272274017334  "ad634646-899d-4957-ba64-64a71ced0116"
            var uid = dsCopied.view()[0].uid;
            var rowIdx = KendoUtils.getRowIndexByUID(grid, uid);// grid.tbody.find("tr[data-uid='" + uid + "']").find(".k-state-selected").prevObject[0].rowIndex;

            //trace("matched one : " + itemId + " / " + itemName + " / " + title + " / " + rowIdx + " / " + uid + " / " + dataJson)

            grid.dataSource._view[rowIdx].ITEM_ID = itemId;
            grid.dataSource._view[rowIdx].ITEM_NAME = itemName;
            grid.dataSource._view[rowIdx].TITLE = title;
           //grid.dataSource._view[rowIdx].DATA_JSON = dataJson;
        }
    }, 

    setItemGrid4LC_87C: function (svid, grid, result0, tmplType) {
        // 사용자가 분석항목 지정 
        // 10. 분석항목을 사용자가 직접선택한다. 
        // 20. 동일 분석항목을 2개이상 선택할수 있다. 
        // 30. 2개이상 선택한 경우 table 데이터는 1건이므로 area, pct_area 를 sum 해서 저장하고, 2건이상 선택한 정보를 DATA_JSON 컬럼에 저장해준다. 
        // 40. 조회시 2건이상 선택했던 항목에 대해서 DATA_JSON 에 저장된 정보를 적용하여 사용자가 선택한 상태대로 표시해준다. 


        var beforeSave = Templ.isBeforeSave(result0);  // 저장전 여부 
        if (beforeSave == null) return;

        if (svid == "R" || beforeSave == true) {  // 자동설정 클릭 or 저장전 조회시
            var rowIdx = 0;
            $.each(result0, function (key, value) {
                var itemId = isnull(value.ITEM_ID, 0);
                var itemName = isnull(value.ITEM_NAME, "");
                var title = isnull(value.TITLE, "");

                if (grid.dataSource._view.length > rowIdx) {
                    grid.dataSource._view[rowIdx].ITEM_ID = "";
                    grid.dataSource._view[rowIdx].ITEM_NAME = "";
                    grid.dataSource._view[rowIdx].TITLE = "";
                    rowIdx++;
                } else {
                    trace("peak data length < item data length .....")
                }

            });
        } else { // 저장된 값을 표시 
            Templ.setItemGrid4Basic(svid, grid, result0, tmplType, "PM0", true);
        }

    },

    setItemGrid4LC_42A: function (svid, grid, result0, tmplType) {
        // peak josn list 에 분석항목을 역순으로 설정 

        var beforeSave = Templ.isBeforeSave(result0);  // 저장전 여부 
        if (beforeSave == null) return;

        if (svid == "R" || beforeSave == true) {  // 자동설정 클릭 or 저장전 조회시
            // DP10 , DP9 ~~ DP1

            // 순서를 위해 숫자 컬럼 추가 
            for (var i = 0; i < result0.length; i++) {
                var testString = isnull(result0[i].TITLE, "0");	// 원래 문자열
                var regex = /[^0-9]/g;				                // 숫자가 아닌 문자열을 선택하는 정규식
                var result = testString.replace(regex, "");	    // 원래 문자열에서 숫자가 아닌 모든 문자열을 빈 문자로 변경
                result0[i].SORT_ORD = Number(result);
            }

            result0.sort(function (a, b) {
                return (a.SORT_ORD > b.SORT_ORD) ? -1 : (a.SORT_ORD > b.SORT_ORD) ? 1 : 0;
            });

            var rowIdx = 0;
            $.each(result0, function (key, value) {
                var itemId = isnull(value.ITEM_ID, 0);
                var itemName = isnull(value.ITEM_NAME, "");
                var title = isnull(value.TITLE, "");

                //trace("itemName >>>>> " + itemName);
                if (grid.dataSource._view.length > rowIdx) {
                    grid.dataSource._view[rowIdx].ITEM_ID = itemId;
                    grid.dataSource._view[rowIdx].ITEM_NAME = itemName;
                    grid.dataSource._view[rowIdx].TITLE = title;
                    rowIdx++;
                } else {
                    trace("peak data length < item data length .....")
                }

            });
        } else { // 저장된 값을 표시 
            Templ.setItemGrid4Basic(svid, grid, result0, tmplType, "PM0", true);
        }
    },

    setItemGrid4LC_NH2: function (svid, grid, result0, tmplType) {
        // 10. DP1, DP2, DP2_ISO 고정 
        // 20. DP3 ~ DP7,  DP8+ 을 사용자가 선택
        // 30. #20의 각 항목 아래  DPx_ISO를 자동으로 채운다. (자동설정 클릭시)
        // 40. 단, DP8+ 아래는 자동 채우지 않는다.  

        var beforeSave = Templ.isBeforeSave(result0);  // 저장전 여부 
        if (beforeSave == null) return;

        if (svid == "R" || beforeSave == true) {  // 자동설정 클릭 or 저장전 조회시
            var rowIdx = 0;
            $.each(result0, function (key, value) {
                var itemId = isnull(value.ITEM_ID, 0);
                var itemName = isnull(value.ITEM_NAME, "");
                var title = isnull(value.TITLE, "");

                if (title.indexOf("DP1") == 0 || title.indexOf("DP2") == 0) { // D1, D2(D2 iso) 고정
                    grid.dataSource._view[rowIdx].ITEM_ID = itemId;
                    grid.dataSource._view[rowIdx].ITEM_NAME = itemName;
                    grid.dataSource._view[rowIdx].TITLE = title;
                    rowIdx++;
                }
            });
        } else { // 저장된 값을 표시  
            Templ.setItemGrid4Basic(svid, grid, result0, tmplType, "PM0", true);
        }

    },

    setAreaRate: function (svid, gridId, tmplType, detailRow) {  // detailRow == result0
        var grid = $('#' + gridId).data('kendoGrid');
        var beforeSave = this.isBeforeSave4Rate(detailRow);  // 비율 저장 전 여부 

        // svid == "R" 자동설정, "C" 조회 
        // 데이터 저장전 여부 :  beforeSave  

        trace("setAreaRate 000 >>>>>> " + svid + " / " + detailRow.length + " /  " + beforeSave + " / " + tmplType)

        // 1010. 저장된 Area Rate (PCT_AREA)  
        if (svid == "C" && beforeSave == false) {  // 비율이 저장된 상태이면 (LC_NH2 는 비율 로직을 태운다.)
            if (tmplType == "LC_NH2") { // LC_NH2 는 비율 로직을 태운다.
                // ### Area Rate 자동 계산 로직을 적용한다.
                trace("setAreaRate 1010 >>>>> " + rowIdx + " / " + areaRate + " / " + tmplType)
            } else {

                trace("setAreaRate 저장된 비율 >>>>>> " + svid + " / " + detailRow.length + " /  " + beforeSave + " / " + tmplType)

                for (var i = 0; i < detailRow.length; i++) {
                    var dataJson = isnull(detailRow[i].DATA_JSON, "").trim();
                    var measRetTime = isnull(detailRow[i].MEAS_RET_TIME, "0");
                    var areaRate = isnull(detailRow[i].PCT_AREA, "0");
                    var itemName = isnull(detailRow[i].ITEM_NAME, "");

                    //trace("agggg : " + itemName + " / " + measRetTime + " / " + areaRate)

                    if (tmplType == "LC_87C") {  // 저장시 집계 --> 조회시 DATA_JSON 에 저장된 리스트로 데이터를 분리해서 설정해준다. 
                        if (dataJson == "") {
                            Templ.setAreaRateStep2(svid, grid, tmplType, measRetTime, areaRate, itemName);
                        } else {
                            var jsonObj = JSON.parse(dataJson);
                            $.each(jsonObj, function (key2, value2) {
                                var retMeas2 = isnull(value2.MEAS_RET_TIME, 0);
                                var area2 = isnull(value2.AREA, 0);
                                var areaRate2 = isnull(value2.PCT_AREA, 0);
                                Templ.setAreaRateStep2(svid, grid, tmplType, retMeas2, areaRate2, itemName);
                            });
                        }
                    } else {
                        Templ.setAreaRateStep2(svid, grid, tmplType, measRetTime, areaRate, itemName);
                    }
                }

                return;  // 저장된 비율을 그대로 표시하고 리턴. 
            }
        }

        trace("setAreaRate 자동 비율 >>>>>> " + svid + " / " + detailRow.length + " /  " + beforeSave + " / " + tmplType)

        // 2010. Area Rate (PCT_AREA)  자동 계산 
        for (var i = 0; i < grid.dataSource.view().length; i++) {
            grid.dataSource._view[i].PCT_AREA = "0";
        }
        grid.dataSource.sync(); // important !!!

        var dsArea = new kendo.data.DataSource({
            data: grid.dataSource.data(),
            aggregate: [
                { field: "AREA", aggregate: "sum" },
                { field: "PCT_AREA", aggregate: "sum" },
            ],
            filter: {
                // leave data items which are "Food" or "Tea"
                logic: "and",
                filters: [
                    { field: "ITEM_ID", operator: "neq", value: "" },
                    { field: "ITEM_ID", operator: "neq", value: "0" },
                    { field: "ITEM_ID", operator: "neq", value: null },
                ]
            }
        });
        dsArea.read();

        var areaSum = 0; //dsArea.aggregates().AREA.sum;  // 필터된 결과 sum <-- ddl 선택후 값이 변경되어도 aggregates 변경이 안된다. 
        for (var x = 0; x < dsArea.view().length; x++) {
            areaSum += Number(isnull(dsArea.view()[x].AREA, 0));
        }

        var firstRowIdx = -1;
        for (var x = 0; x < dsArea.view().length; x++) {
            var uid = dsArea.view()[x].uid;
            //var row = grid.tbody.find("tr[data-uid='" + uid + "']");  
            //var dataItem = grid.dataSource.view()[row.index()];
            var rowIdx = KendoUtils.getRowIndexByUID(grid, uid); // grid.tbody.find("tr[data-uid='" + uid + "']").find(".k-state-selected").prevObject[0].rowIndex;

            if (firstRowIdx < 0) firstRowIdx = rowIdx;

            var area = isnull(dsArea.view()[x].AREA, 0);
            var areaRate = 100 * Math.floor(10000 * (area / areaSum)) / 10000;  // 소수 두자리 #Math_DIGIT

            //dataItem.set("PCT_AREA", areaRate);  // editable false ... not working ...
            grid.dataSource._view[rowIdx].PCT_AREA = areaRate;
            grid.dataSource.sync();

            dsArea.view().at(x).set("PCT_AREA", areaRate);
            dsArea.read();
        }

        // 비율 보정처리 : 첫행에 대해서 처리한다. 
        var rem = 100 - isnull(dsArea.aggregates().PCT_AREA.sum, 0);
        if (rem != 0 && firstRowIdx >= 0) {
            var rate = grid.dataSource._view[firstRowIdx].PCT_AREA;
            var adj = Number((rate + rem).toFixed(2));
            grid.dataSource._view[firstRowIdx].PCT_AREA = adj;
            grid.dataSource.sync();
        }

    },

    setAreaRateStep2: function (svid, grid, tmplType, measRetTime, areaRate, itemName) {
        if (measRetTime == "0" || measRetTime == "") return;

        //trace("setAreaRateStep2 : " + itemName + " / " + measRetTime + " / " + areaRate)

        var dsCopiedStep2 = new kendo.data.DataSource({
            data: grid.dataSource._view,
            filter: {
                logic: "and",
                filters: [
                    { field: "ITEM_NAME", operator: "eq", value: itemName},
                    { field: "MEAS_RET_TIME", operator: "startswith", value: Math.floor(measRetTime*100000)/100000},  // important !!! 소수 이하 14자리까지 저장되므로 소수이하 5자리까지 끊어서 비교 startswith 
                ]
            },

        });
        dsCopiedStep2.read();

        if (dsCopiedStep2._view.length == 1) {
            var uid = dsCopiedStep2._view[0].uid;
            var rowIdx = KendoUtils.getRowIndexByUID(grid, uid);
            grid.dataSource._view[rowIdx].PCT_AREA = areaRate;
            //trace("setAreaRate 2010 ... saved value >>>>> " + rowIdx + " / " + areaRate + " / " + tmplType)
        } else if (dsCopiedStep2._view.length > 1) {
            alert("setAreaRate 2 rows .... area rate ..... 2건 이상 존재...확인할것." + " / " + itemName + " / " + measRetTime + " / " + grid.dataSource._view.length)
        } else {
            alert("setAreaRate not found .... area rate ..0건 존재 ... 확인할것." + " / " + itemName + " / " + measRetTime + " / " + grid.dataSource._view.length)
        }
    }, 

    isMatched: function (type, ret, measRet) { // Ret time & Measur Ret time 기준으로 Item 찾기
        if (type == "PM0.5") { //  RET_TIME - 0.5 <=  Peak Josn MeasRetTime  <=  RET_TIME + 0.5   내에서 근사값으로 Item 찾기
            // ret : 1.7 /   measRet : 1.701010101234
            if (measRet >= ret - 0.5 && measRet <= ret + 0.5) {
                //trace("PM0.5 : " + ret + " / " + measRet + " / " + Math.abs(measRet - ret))
                return true;
            }
        } else if (type == "PM0") {  // 일치 (DB에 소수이하 14자리까지 저장됨. 16자리 존재해서 0.00001 로 근사값 찾기함.)
            // ret : 1.7 /   measRet : 1.701010101234
            if (measRet >= ret - 0.00001 && measRet <= ret + 0.00001) {
                //trace("PM0 : " + ret + " / " + measRet + " / " + Math.abs(measRet - ret))
                return true;
            }
        }

        return false;
    },

    edit: function (gridId, e, that, tmplType, dsItem) { 
        var inputId = e.container.find("input").attr("name");  // ddl 일때... 
        //trace("edit :  " + e.container.find("input").attr("name") + "/" + that);

        // PCT_AREA 입력불가 
        //if (tmplType != "LC_87C") {  // LC_87C 아닐때 area비율 입력불가
        //    var numeric = e.container.find("input[name='PCT_AREA']").data("kendoNumericTextBox");
        //    if (numeric != undefined) {
        //        numeric.enable(false);
        //    }
        //}

        var rowIdx = KendoUtils.getRowIndexByUID(gridId, e.model.uid);   //$("#" + gridId).data("kendoGrid").tbody.find("tr[data-uid='" + e.model.uid + "']").find(".k-state-selected").prevObject[0].rowIndex;
        var measRetTime = isnull($("#" + gridId).data("kendoGrid").dataSource._view[rowIdx].MEAS_RET_TIME, "0");

        if (tmplType == "LC_NH2") { // 0, 1, 2 cell lock 
            if (e.container.find("input").attr("name") == "ITEM_ID") {

                if (rowIdx >= 0 && rowIdx <= 2) {
                    that.closeCell(); // prevent editing   that == editor 
                }

                // grid ddl field datasource  
                dsItem.filter({
                    logic: "or",
                    filters: [
                        { field: "ITEM_NAME", operator: "eq", value: "" },
                        { field: "ITEM_NAME", operator: "startswith", value: "DP3" },
                        { field: "ITEM_NAME", operator: "startswith", value: "DP4" },
                        { field: "ITEM_NAME", operator: "startswith", value: "DP5" },
                        { field: "ITEM_NAME", operator: "startswith", value: "DP6" },
                        { field: "ITEM_NAME", operator: "startswith", value: "DP7" },
                        { field: "ITEM_NAME", operator: "startswith", value: "DP8" },
                    ]
                });
                trace("dsItem length : " + dsItem._view.length)
            }
        }
 
        if (measRetTime == "0") { // measRetTime == 0 이면.. 일단 lock ... 걸어두겠다. 
            that.closeCell(); // prevent editing   that == editor 
        }

    },

    itemOnChange: function (gridId, e, rowIdx, value, tmplType, detailRow, dsItem) {
        trace("itemOnChange :  " + gridId + "/ " + rowIdx + "/" + value);

        var grid = $("#" + gridId).data("kendoGrid");

        // 00. 선택한 분석항목에 대한 분석항목명(ITEM_NAME) 설정 
        var itemName = KendoUtils.getText4ddlDs(dsItem, value, "ITEM_NAME");
        var title = KendoUtils.getText4ddlDs(dsItem, value, "TITLE");

        grid.dataSource._view[rowIdx].ITEM_NAME = itemName;  // filed > editable false --> _view or _data 
        grid.dataSource._view[rowIdx].TITLE = title;
        //grid.dataSource.sync();

        // 10. 현재 선택된 행의 값이 다른 행이 존재하는지 체크하고 존재하면 clear 
        if (tmplType != "LC_87C") {
            Templ.setItemPost(gridId, e, rowIdx++, value, tmplType);
        }
 
        grid.dataSource.sync();

        Templ.setAreaRate("CHG", gridId, tmplType, detailRow);

        dsItem.filter({});  // filter 해제 
    },

    setautoAnalysis_LC_NH2: function (gridId, tmplType, detailRow, dsItem) {
        trace("setautoAnalysis_LC_NH2 :  " + gridId + "/" + tmplType);

        var grid = $("#" + gridId).data("kendoGrid");

        // 자동설정되는 항목 초기화 
        for (var rowIdx = 0; rowIdx < grid.dataSource._view.length; rowIdx++) {
            var itemName = grid.dataSource._view[rowIdx].ITEM_NAME;
            if (!(itemName.indexOf("DP1") == 0 || itemName.indexOf("DP2") == 0)) { // DP1~2 제외 
                //if (itemName.toUpperCase().indexOf("ISO".toUpperCase()) >= 0 || itemName.indexOf("DP8") >= 0) { // 나머지는 clear 상태로 
                if (itemName.toUpperCase().indexOf("ISO".toUpperCase()) >= 0 && itemName.indexOf("DP8") < 0) { // DP8 제외 하고 나머지 
                    grid.dataSource._view[rowIdx].ITEM_ID = "";
                    grid.dataSource._view[rowIdx].ITEM_NAME = "";
                    grid.dataSource._view[rowIdx].TITLE = "";
                }
            }
        }

        var auto_iso = "";
        var auto_iso_name = "";
        var auto_iso_title = "";
        var auto_item_name = "";
        var dp7_iso_set_yn = false;
        for (var rowIdx = 0; rowIdx < grid.dataSource._view.length; rowIdx++) {
            var itemId = grid.dataSource._view[rowIdx].ITEM_ID;
            var itemName = isnull(grid.dataSource._view[rowIdx].ITEM_NAME, "");
            var title = grid.dataSource._view[rowIdx].TITLE;
            var measRetTime = isnull(grid.dataSource._view[rowIdx].MEAS_RET_TIME, "0");

            //trace("dsCopied 00 >>>>> " + itemName + " / " + rowIdx);

            // DP3 , DP4 ~~~  DP7 행...
            //if (!(itemName == "" || itemName.indexOf("DP1") == 0 || itemName.indexOf("DP2") == 0 || itemName.toUpperCase().indexOf("ISO".toUpperCase()) >= 0)) {
            if (!(itemName == "" || itemName.indexOf("DP1") == 0 || itemName.indexOf("DP2") == 0 || itemName.indexOf("DP8") == 0 || itemName.toUpperCase().indexOf("ISO".toUpperCase()) >= 0)) {
                auto_item_name = itemName;

                var dsCopied = new kendo.data.DataSource({
                    data: detailRow,
                    filter: {
                        // leave data items which are "Food" or "Tea"
                        logic: "and",
                        filters: [
                            { field: "ITEM_NAME", operator: "neq", value: itemName },
                            { field: "ITEM_NAME", operator: "startswith", value: itemName },
                        ]
                    }
                });

                if (itemName == "DP7") { // DP7 일때.. 
                    dsCopied.filter(
                        {
                            logic: "or",
                            filters: [{
                                logic: "and",
                                filters: [
                                    { field: "ITEM_NAME", operator: "neq", value: itemName },
                                    { field: "ITEM_NAME", operator: "startswith", value: itemName },
                                ]
                            },
                            ////{ field: "ITEM_NAME", operator: "startswith", value: "DP8" },
                            ]
                        }
                    )
                }

                dsCopied.read();

                if (dsCopied._view.length > 0) {
                    auto_iso = dsCopied._view[0].ITEM_ID;
                    auto_iso_name = dsCopied._view[0].ITEM_NAME;
                    auto_iso_title = dsCopied._view[0].TITLE;
                }

            } else if (itemName == "" && measRetTime != "0") { // 직전에서 DP3 ~ DP7 외 하위항목에 대해서 clear 했음.
                if (auto_item_name == "DP7") {
                    // DP7 일 경우 
                    // 1. DP7_ISO 설정 
                    // 2. 나머지행에 대해서 DP8_plus 설정 --> 자동설정 안함. 

                    if (dp7_iso_set_yn) {
                        /***** DP8+ 자동 설정 안한다. 
                        var dsCopied2 = new kendo.data.DataSource({
                            data: dsCopied._view,
                            filter: { field: "ITEM_NAME", operator: "startswith", value: "DP8" }, // DP8+
                        });
                        dsCopied2.read();

                        if (dsCopied2._view.length > 0) {
                            grid.dataSource._view[rowIdx].ITEM_ID = dsCopied2._view[0].ITEM_ID;
                            grid.dataSource._view[rowIdx].ITEM_NAME = dsCopied2._view[0].ITEM_NAME;
                            grid.dataSource._view[rowIdx].TITLE = dsCopied2._view[0].TITLE;
                        }
                        ****/
                    } else {
                        var dsCopied2 = new kendo.data.DataSource({
                            data: dsCopied._view,
                            filter: { field: "ITEM_NAME", operator: "startswith", value: auto_item_name }, // DP7_ISO 
                        });
                        dsCopied2.read();

                        if (dsCopied2._view.length > 0) {
                            grid.dataSource._view[rowIdx].ITEM_ID = auto_iso;
                            grid.dataSource._view[rowIdx].ITEM_NAME = auto_iso_name;
                            grid.dataSource._view[rowIdx].TITLE = auto_iso_title;
                            /////// dp7_iso_set_yn = true;  // DP7_ISO 1개 설정완료 
                        }
                    }

                } else {
                    grid.dataSource._view[rowIdx].ITEM_ID = auto_iso;
                    grid.dataSource._view[rowIdx].ITEM_NAME = auto_iso_name;
                    grid.dataSource._view[rowIdx].TITLE = auto_iso_title;
                }
            }
        }


        grid.dataSource.sync();

        Templ.setAreaRate("CHG", gridId, tmplType, detailRow);
    },

    setItemPost: function (gridId, e, rowIdx, value, tmplType) {
        var grid = $("#" + gridId).data("kendoGrid");
        var data = grid.dataSource.view();

        for (var i = 0; i < data.length; i++) {
            var item = data[i].ITEM_ID;

            if (i != rowIdx && item == value) {
                //trace("item :  " + item + "/" + value);
                grid.dataSource._view[i].ITEM_ID = "";
                grid.dataSource._view[i].ITEM_NAME = "";
                grid.dataSource._view[i].TITLE = "";
            }
        }
    },

    getDetailData4TASTER: function (gridId, tmplType, detailRow) {
        var grid = $("#" + gridId).data("kendoGrid");

        var dsCopied = new kendo.data.DataSource({
            data: grid.dataSource._view,
            filter: {
                logic: "or",
                filters: [
                    { field: "Number_of_Cycle", operator: "eq", value: 0 },
                    { field: "Number_of_Cycle", operator: "eq", value: "0" },
                ]
            },
        });
        dsCopied.read();

        if (dsCopied._view.length == 0) {
            alert("평균값이 존재하지 않습니다.");
            return "";
        }

        // 저장 데이터 
        $.each(detailRow, function (index, item) {
            var itemName = Templ.keyReplace(item.ITEM_NAME);  // item table 저장된 값 
            item.AREA = dsCopied._view[0][itemName];
        });

        return detailRow;
    },

    getDetailData: function (gridId, tmplType, detailRow) {
        if (tmplType == "LC_TASTER") {
            var data = Templ.getDetailData4TASTER(gridId, tmplType, detailRow); 
            return data;
        }

        if (tmplType == "GC_FRAGRANCE") {
            var ds4Frag = $("#" + gridId).data("kendoGrid").dataSource; 
            return ds4Frag.view();
        }

        var leakCheck = true; // 분석항목 누락 체크 
        if (tmplType == "LC_87C" || tmplType == "LC_NH2") {
            leakCheck = false;  // 미입력한 분석항목은 clear update 
        }
 
        // 데이터수집 (분석항목이 존재하는것만 )
        var data = [];
        var ds = $("#" + gridId).data("kendoGrid").dataSource;

        if (tmplType == "VISCO_BRA" || tmplType == "VISCO_RVA" || tmplType == "VISCO_BRA_COMPETITOR" || tmplType == "VISCO_RVA_COMPETITOR") {
            for (var i = 0; i < ds.view().length; i++) {
                var itemId = isnull(ds.view()[i].ITEM_ID, "");
                if (itemId != "" && itemId != "0") {
                    data.push(ds.view()[i]);
                }
            }
            return data;
        }


        if (tmplType == "LC_NH2") { // 분석항목 ISO 가 여러개 화면에 표시된다. 합해서 한건으로 처리 
            // 10. DP1, DP2, DP2_ISO, DP3,4,5,6,7 
            for (var i = 0; i < ds.view().length; i++) {
                var itemId = isnull(ds.view()[i].ITEM_ID, "");
                var itemName = isnull(ds.view()[i].ITEM_NAME, "");
                if (itemId != "" && itemId != "0") {
                    if (itemName.indexOf("DP1") >= 0 || itemName.indexOf("DP2") >= 0) {
                        data.push(ds.view()[i]);
                    } else if (itemName.indexOf("ISO") >= 0) {  // itemName.indexOf("DP8") < 0
                        // dummy   ISO는 집계해서 담는다. 
                    } else {
                        data.push(ds.view()[i]);
                    }
                }
            }

            // 20. DP3_ISO, DP4_ISO ~~ DP7_ISO, DP8_plus 각각 한건으로 집계해서 저장
            var dataItem = Templ.getItemSum(ds, "DP3_ISO");
            if (dataItem != undefined && dataItem != null) data.push(dataItem);

            var dataItem = Templ.getItemSum(ds, "DP4_ISO");
            if (dataItem != undefined && dataItem != null) data.push(dataItem);

            var dataItem = Templ.getItemSum(ds, "DP5_ISO");
            if (dataItem != undefined && dataItem != null) data.push(dataItem);

            var dataItem = Templ.getItemSum(ds, "DP6_ISO");
            if (dataItem != undefined && dataItem != null) data.push(dataItem);

            var dataItem = Templ.getItemSum(ds, "DP7_ISO");
            if (dataItem != undefined && dataItem != null) data.push(dataItem);

            ////var dataItem = Templ.getItemSum(ds, "DP8_plus");
            ////if (dataItem != undefined) data.push(dataItem);

        } else if (tmplType == "LC_87C") { // 중복 입력한 분석항목 합쳐서 저장, 이때 중복된 건수 만큼 json data를 저장해준다. (조회시 필요)

            for (var i = 0; i < detailRow.length; i++) { // 분석항목 개수만큼 
                var itemName = isnull(detailRow[i].ITEM_NAME, "");
                var dataItem = Templ.getItemSum(ds, itemName);
                if (dataItem != undefined && dataItem != null) {
                    data.push(dataItem);
                }
            }

        } else {
            for (var i = 0; i < ds.view().length; i++) {
                var itemId = isnull(ds.view()[i].ITEM_ID, "");
                if (itemId != "" && itemId != "0") {
                    data.push(ds.view()[i]);
                }
            }
        }

        //trace(data);

        // 분석항목 개수 > Peak 건수 
        //if (detailRow.length > ds.view().length) {
        //    leakCheck = false;  // 로직 확인 할것.
        //}

        leakCheck = false;  //  전부 누락 체크하지 않는다. !!! 
        for (var i = 0; i < detailRow.length; i++) { // 분석항목 중복 및 누락 체크
            var itemId = detailRow[i].ITEM_ID;
            var itemName = detailRow[i].ITEM_NAME;
            var title = detailRow[i].TITLE;

            var dsTemp = new kendo.data.DataSource({
                data: data,
            });

            dsTemp.filter({ field: "ITEM_ID", operator: "eq", value: itemId }); // 일치

            if (leakCheck) {
                if (dsTemp.view().length == 0) {
                    //alert("분석항목을 선택하십시오. [" + itemName + "]");  // not used...
                    //return false;
                } else if (dsTemp.view().length > 1) {
                    //alert("분석항목이 2개 이상 선택되었습니다. [" + itemName + "]");  // not used...
                    //return false;
                } else {
                    //trace("OK : " + itemId + " / " + itemName)
                }
            } else { // 분석항목 선택한것만 저장할 경우 
                if (dsTemp.view().length == 0) { // 선택하지 않은것은 0 으로 담아준다. update 
                    data.push({ ITEM_ID: itemId, ITEM_NAME: itemName, TITLE: title, MEAS_RET_TIME: 0, AREA: 0, PCT_AREA: 0, SAMPLE_ID: _sampleId, ANAL_ID: _analysisId });
                } else if (dsTemp.view().length > 1 && tmplType != "LC_NH2") { // LC_NH2 는 스킵 
                    //alert("분석항목이 2개 이상 선택되었습니다. [" + itemName + "]");  // not used...
                    //return false;
                } else {
                    //trace("OK : " + itemId + " / " + itemName)
                }
            }
        }
        
        return data;
    },
 
    getItemSum: function (ds, iso) {
        // ITEM_NAME 이 동일한 행을 합쳐서 리턴해준다.
        var dsCopied = new kendo.data.DataSource({
            data: ds._view,
            filter: { field: "ITEM_NAME", operator: "eq", value: iso},
            aggregate: [
                { field: "AREA", aggregate: "sum" },
                { field: "PCT_AREA", aggregate: "sum" },
                { field: "ITEM_NAME", aggregate: "count" },
            ],
        });
        dsCopied.read();

        if (dsCopied._view.length > 0) {
            var areaSum = dsCopied.aggregates().AREA;
            var areaReatSum = dsCopied.aggregates().PCT_AREA;
            areaReatSum.sum = Math.round(1000 * areaReatSum.sum) / 1000  // 소수 3자리에서 반올림 처리하겠다. 각각의 값은 소수 2자리까지이지만,  sum 결과가 소수 3자리 이상 나올때가 있다.

             //var dataItem = dsCopied._view[0];  //  --> dsCopied --> grid.dataSource 가 변경됨.... 복사해서 처리할것.
            var dataItem = {
                ITEM_ID: dsCopied._view[0].ITEM_ID,
                ITEM_NAME: dsCopied._view[0].ITEM_NAME,
                TITLE: dsCopied._view[0].TITLE,
                MEAS_RET_TIME: dsCopied._view[0].MEAS_RET_TIME,
                AREA: areaSum.sum,  // 합계
                PCT_AREA: areaReatSum.sum,  // 합계
                DATA_JSON: "", 
                SAMPLE_ID: dsCopied._view[0].SAMPLE_ID,
                ANAL_ID: dsCopied._view[0].ANAL_ID,
            };

            var itemCount = dsCopied.aggregates().ITEM_NAME.count;
            if (itemCount > 1) { // 중복 선택한건인 경우 
                var dsCopied2 = new kendo.data.DataSource({
                    data: ds._view,
                    filter: { field: "ITEM_NAME", operator: "eq", value: dsCopied._view[0].ITEM_NAME },
                });
                dsCopied2.read();
 
                var json = [];
                for (var k = 0; k < dsCopied2._view.length; k++) {
                    var item = { AREA: dsCopied2._view[k].AREA, PCT_AREA: dsCopied2._view[k].PCT_AREA, MEAS_RET_TIME: dsCopied2._view[k].MEAS_RET_TIME};
                    json.push(item);
                }

                dataItem.DATA_JSON = JSON.stringify(json);
            }

            trace("aggregates : " + iso + "  / " + dataItem.AREA + " / " + dsCopied._view.length + " / " + dsCopied.aggregates().ITEM_NAME.count)

            return dataItem; 
        }

        return null; 
    },

};

var SetGrid = {
    Title: function (grid, toolbar, text) {
        $("#" + grid + " > " + toolbar).prepend("<span id='" + grid + "_title' style='font-weight:bold; color:#1a1a1a'>" + text + "</span>");

        // Grid 에 해당 toolbar 버튼이 존재할 경우
        if (document.getElementById(grid).querySelector(toolbar).getElementsByTagName('Button')) {
            document.getElementById(grid).querySelector(toolbar).getElementsByTagName('Button')[0].style.marginLeft = 'auto';
        }
        
    }
}
