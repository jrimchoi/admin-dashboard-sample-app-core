/***************************************************************************************
// kendo ajax transaction 
***************************************************************************************/
var KendoAjaxTran = {

    search: function (params) {
        trace("KendoAjaxTran search ........");
        // parameters 
        var url = params.url; // $("form[name='form_cond']").serialize();
        var inputData = params.data;
        var resultBind = params.resultBind;  // result0, result1, result2  
        var cond_data = null;
        var pageindex = isnull(params.pageindex, 1);
        var funcVar = params.funcVar;
        var svid = isnull(params.svid, "SEARCH");
        var dataType = typeof (inputData);
        var pageSize = 0, serverPaging = false;

        var isExcel = isnull(params.isExcel, false);  // excel down
        if (isExcel) {
            resultBind = false;
        }

        if (dataType == "string") { // form id 
            inputData = inputData.replace("#", ""); 

            if ($("#" + inputData)[0].tagName && $("#" + inputData)[0].tagName.toUpperCase() == "FORM") { // 조회조건 = form
                var obj = Object.keys(resultBind);
 
                for (key in Object.keys(obj)) {
                    var field = obj[key];

                    if (typeof (field) == "string") { // bind result 
                        var bindId = "#" + resultBind[field].replace("#", "");
                        var role = $(bindId).attr("data-role");

                        if (role == "grid" || role == "treelist") { // bind result == kendo grid
                            var grid = $(bindId).data(KendoUtils.getkendoType(role));  // kendoGrid, kendoTreeList

                            pageSize = grid.dataSource._pageSize; //  isnull(grid.dataSource.options.pageSize, 0);  // pageSize vs pageSize()
                            serverPaging = grid.dataSource.options.serverPaging == false ? false : true;
                            //trace("serverPaging >>>> " + pageSize + " / " + serverPaging + " / " + pageindex)

                            // page size == All --> 0 으로 
                            if (role == "grid") { // treelist 는 따로 설정할것. 데이터가 없어짐...
                                if (pageSize == undefined || (String(pageSize).toUpperCase() == "All".toUpperCase())) {
                                    pageSize = 0;
                                }
                            }
 
                            if (pageSize > 0 && serverPaging) {
                                $("#" + inputData).find("input[name='_PAGE_ABLE']").remove();
                                $("#" + inputData).append('<input type="hidden" name="_PAGE_ABLE" value="true" />');
                            } else {
                                $("#" + inputData).find("input[name='_PAGE_ABLE']").remove();
                                $("#" + inputData).append('<input type="hidden" name="_PAGE_ABLE" value="false" />');
                            }

                            $("#" + inputData).find("input[name='_PAGE_SIZE']").remove();
                            $("#" + inputData).append('<input type="hidden" name="_PAGE_SIZE" value="' + isnull(pageSize, 0) + '" />');

                            if (pageindex == 1) grid.dataSource.page(pageindex); // page no 1 --> 3 --> 조회버튼 클릭시 --> 1 page on 

                        }
                    }
                }

                $("#" + inputData).find("input[name='_PROC_TYPE']").remove();
                $("#" + inputData).append('<input type="hidden" name="_PROC_TYPE" value="SEARCH" />');

                $("#" + inputData).find("input[name='_PAGE_INDEX']").remove();
                $("#" + inputData).append('<input type="hidden" name="_PAGE_INDEX" value="' + pageindex + '" />');

                // multi combo 
                var multi = $("#" + inputData).find("[_data-input-type='multicombobox']");
                $(multi).each(function (index, item) {
                    var id = $(item).attr("id");
                    var name = $(item).attr("name");
                    //trace("KendoUtils : " + KendoUtils.get(id) + " / " + _vmCond.get(name) );
                    $(item).val(KendoUtils.get(id)); // 'LC_BASIC,LC_FOS'  넘김... --> IN ('A', 'B')
                });

                if (isExcel) { // excel export all page
                    $("input[name='_PROC_TYPE']").val("EXCEL");
                    $("input[name='_PAGE_SIZE']").val("0");
                    $("input[name='_PAGE_ABLE']").val("false");
                }

                cond_data = $("#" + inputData).serialize(); 

            } else { // 그외 
                alert("not defined...."); return; 
            }
 
            //trace("cond_data : " + cond_data);

        } else { // json object 

            //cond_data = _vmCond;
            cond_data = inputData;  // json {CD_TYP:"C"}  ok
            cond_data["_PROC_TYPE"] = "SEARCH";
            cond_data["_PAGE_INDEX"] = pageindex;

            var obj = Object.keys(resultBind);

            for (key in Object.keys(obj)) {
                var field = obj[key];

                if (typeof (field) == "string") { // bind result 
                    var bindId = "#" + resultBind[field].replace("#", "");
                    var role = $(bindId).attr("data-role");

                    if (role == "grid" || role == "treelist") { // bind result == kendo grid
                        var grid = $(bindId).data(KendoUtils.getkendoType(role));  // kendoGrid, kendoTreeList
 
                        pageSize = grid.dataSource._pageSize; //  isnull(grid.dataSource.options.pageSize, 0);  // pageSize vs pageSize()
                        serverPaging = grid.dataSource.options.serverPaging == false ? false : true;

                        if (pageSize > 0 && serverPaging) {
                            cond_data["_PAGE_ABLE"] = true;
                        } else {
                            cond_data["_PAGE_ABLE"] = false;
                        }

                        cond_data["_PAGE_SIZE"] = pageSize;

                        if (pageindex == 1) grid.dataSource.page(pageindex);

                        //trace("serverPaging 22 >>>> " + pageSize + " / " + serverPaging + " / " + isnull(pageindex, 1))
                    }
                }
            }

            if (isExcel) { // excel export all page
                cond_data["_PROC_TYPE"] = "EXCEL";
                cond_data["_PAGE_SIZE"] = "0";
                cond_data["_PAGE_ABLE"] = false;
            }
        }
 
        // ajax option 
        var dataType = "json";
        var contentType = "application/x-www-form-urlencoded; charset=utf-8";
        var token = $('[name=__RequestVerificationToken]').val();

        //trace("cond_data : " + cond_data);

        $.ajax({
            type: "POST",
            url: url,
            data: cond_data,
            dataType: dataType,
            contentType: contentType,
            //headers: headers,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("AjaxCall", "Y");
                xhr.setRequestHeader("RequestVerificationToken", token);
                WAIT.startLoader();
            }, // 필수로 넘겨줄것
            success: function (data) {

                if (isExcel) { // excel export 
                    KendoAjaxTran.excel_post(params, data);
                    return;
                }

                if (resultBind == false || resultBind == "false") { // no bind 
                    WAIT.stopLoader();
                    funcVar.apply_post(svid, data);
                    return;
                }

                var obj = Object.keys(resultBind);
                
                for (key in Object.keys(obj)) {
                    var keyId = obj[key]; 
                    var bindId = resultBind[keyId].replace("#", "");
                    var role = $("#" + bindId).attr("data-role");

                    if (role == "grid" || role == "treelist") { // kendo grid
                        var grid = $("#" + bindId).data(KendoUtils.getkendoType(role));  // kendoGrid, kendoTreeList

                        grid.checkRows = []; // checked 초기화
                        grid.checkRowsUID = [];
                        $("#" + bindId).find("#_chkAll").prop("checked", false)    // 전체선택 초기화

                        /*******************************************
                        // 데이터 바인딩 
                        *******************************************/
                        if (role == "grid") {
                            var options = grid.dataSource.options;
                            var model = grid.dataSource.options.schema.model;

                            grid.dataSource.data(data[keyId]);  // --> sorting not working..... 
                            grid.dataSource.options.data = data[keyId];
                            grid.dataSource.transport.data = data[keyId];

                        } else if (role == "treelist") {
                            ////grid.dataSource.data(data[keyId]);
                            //grid.setDataSource(data[keyId]);  // ????  --> USE_YN 등 체크컬럼 add 시 오류 

                            grid.dataSource.data(data[keyId]);  // --> sorting not working..... 
                            grid.dataSource.options.data = data[keyId];
                            grid.dataSource.transport.data = data[keyId];

                            grid.dataSource._pageSize = pageSize;  // paging working...
                            grid.dataSource._page = pageindex;
                            grid.dataSource._take = pageSize;
                            grid.dataSource.options.serverPaging = serverPaging;
                        }

                        trace("callback : " + grid.dataSource._view.length + " / " + pageSize + " / " + serverPaging + " / " + pageindex)
  
                        /*******************************************
                        // 페이징 설정
                        *******************************************/
                        ////var pageSize = grid.dataSource._pageSize; //  isnull(grid.dataSource.options.pageSize, 0);  // pageSize vs pageSize()
                        ////var serverPaging = grid.dataSource.options.serverPaging == false ? false : true;
 
                        if (pageSize > 0 && serverPaging) {  
                            var totalrows = 0;
                            if (data[keyId] == undefined) {
                                trace("data[keyId] == undefined !!!! ")
                            } else {
                                if (data[keyId].length > 0) {
                                    totalrows = isnull(data[keyId][0].PAGE_TOT_ROWS, 0);  // total count & paging
                                } else {
                                    trace("data[keyId].length == 0 !!! ")
                                }
                            }
                            //trace("serverPaging totalrows >>>> " + totalrows +  "/ " + pageSize + " / " + serverPaging + " / " + pageindex)
                            grid.dataSource._total = parseInt(totalrows); // important !!!
                            grid.pager.refresh(); //  important !!!
                        }


                        ////grid.setOptions(gridoptions);

                        /*******************************************
                        // 행별 처리 
                        *******************************************/
                        var rows = grid.dataSource.data();

                        // #ERR100 처리에 따라 발생되는 현상 : 2페이지 클릭시 data 존재하나 view length == 0  , 화면에 데이터가 표시되지 않는다. 
                        // 여기서 전체행을 view 에 반영 
                        //grid.dataSource.view(rows);
                        //grid.refresh();
 
                        $(rows).each(function (index, item) {
                            var size = grid.dataSource.pageSize();
                            var page = isnull(grid.dataSource.page(), 1);

                            //trace("callback page..." + size + " / " + page);

                            var rn = index + 1 + (grid.dataSource.pageSize() * (grid.dataSource.page() - 1));
                            var rn = index + 1 + (pageSize * (grid.dataSource.page() - 1));
                            var uid = item.uid;
                            var rowLabel = $("#" + bindId).find("tr[data-uid='" + uid + "']").find(".row-number");
                            $(rowLabel).html(rn);  

                            grid.dataSource.data()[index].dirty = false;
                            ///////grid.dataSource.data()[index].isNew = false;  // important !!! <-- isNew() 함수가 false 가되게 처리해야되는데... 해결책을 찾기전까지..false 로..
                            grid.dataSource.data()[index]._ROW_TYPE = "";
                        }); 

                    } else { // form 
                        var result_this = null;
                        if (data[keyId] == undefined) {
                            //alert("not defined.....");
                            result_this = null; 
                        }else if(data[keyId].length == undefined) { // model
                            result_this = data[keyId];  
                        } else if (data[keyId].length >= 1) { // List<model>
                            result_this = data[keyId][0];
                        } else { // list = 0 

                        }

                        var forms = $("#" + bindId).find(KendoUtils.data_tag);
                        var __viewModel = (new Function('return ' + $("#" + bindId).attr("_data-view-model")))();

                        if (result_this == null) {
                            KendoUtils.dataClear(bindId);
                        } else {
                            // (new Function('return ' + _editortemplate.datasourceStr))
                            $(forms).each(function (index, item) {
                                var id = isnull($(item).attr("id"), "");
                                if (id == "") return true; // continue 

                                if (result_this[id] == undefined) {
                                    // dummy  
                                } else {
                                    KendoUtils.set(id, result_this[id]);
                                }

                                var inputtype = $("#" + id).attr("_data-input-type");
                                var codeDs = $("#" + id).attr("_data-code-ds");

                                if (inputtype.toUpperCase() == "checkbox".toUpperCase()) {
                                    if (result_this[id] == KendoUtils.checkType) { // Y 
                                        $("#" + id).prop("checked", true);
                                    } else {
                                        $("#" + id).prop("checked", false);
                                    }
                                } else if (inputtype.toUpperCase() == "datebox".toUpperCase()) {
                                    ////var dateStr = isnull(result_this[id], "");
                                    ////KendoUtils.set(id, formatDate(dateStr));
                                    //trace("date " + result_this[id])
                                } else if (inputtype.toUpperCase() == "mccombobox".toUpperCase()) { // ##########  #AAA 동기화 할것. 
                                    if (codeDs == "remote") { // 코드도움...
                                        var text = result_this[id + "_NM"];  // text CHARGER_NM
                                        var mcCombo = $("#" + id).data("kendoMultiColumnComboBox");
                                        //mcCombo.text(text);  // not ...
                                        $("#" + bindId).find("input[name='" + id + "_input']").val(text);  // ok...
      
                                        trace("mcc id : " + result_this[id] + " / " + KendoUtils.get(id) + " / " + mcCombo.value() );

                                    } else { // 코드 리스트가 화면에 있다. 
                                        // 자동반영 
                                    }
                                } else if (inputtype.toUpperCase() == "multicombobox".toUpperCase()) { // ##########  #AAA 동기화 할것.
                                    //trace("multicombobox : " + result_this[id])
                                    var arr = isnull(result_this[id], "").split(",");
                                    var multiText = "";
                                    var valueObj = [];
                                    for (var k = 0; k < arr.length; k++) {
                                        var xx = arr[k].trim().replace("[", "").replace("]", "");
                                        if (isnull(xx, "") == "") continue;
                                        var cc = xx;
                                        if (k == 0) {
                                            multiText += cc;
                                            valueObj.push(cc);
                                        } else {
                                            multiText += ", " + cc;
                                            valueObj.push(cc);
                                        }
                                    }
                                    KendoUtils.set(id, valueObj);   // ["A", "B"]  object
                                }

                            });
                        }

                        // form 데이터 변경여부를 판단하기 위해 담아둔다. 
                        var origin = $(forms).serialize();
                        __viewModel.set("_FORM_ORIGIN_DATA", origin);

                    }
                  
                }

                funcVar.apply_post(svid, data);
            },
            error: function (request, status, error) {
                // response.StatusCode --> request.status
                KendoAjaxTran.error_post(request, status, error);
            },
            complete: function (request, status, error) {
                WAIT.stopLoader();
            }
        });
    },
 
    viewJosnStr2Form: function (formId, jsonstr, viewMode) { // json str  --> form 반영 (for view & form )
        jsonstr = isnull(jsonstr, "");

        if (jsonstr == "") return;

        // 개행문자 (textarea)
        jsonstr = replaceAll(jsonstr, "\r", "\\r");
        jsonstr = replaceAll(jsonstr, "\n", "\\n");

        //var arr = ' "PARSING_TYPE": "[LC_BASIC", "LC_FOS]", "LAS_TYPE": "QR", ' ;
        var regExp = /\[[^]+\]/;
        var matches = regExp.exec(jsonstr);
        if (matches != undefined) {
            var pre = matches[0];
            var post = replaceAll(pre, '"', "'");   // 
            jsonstr = replaceAll(jsonstr, pre, post)
            //trace("multicombobox id :" + matches[0])
        }

        var vm = $("#" + formId).attr("_data-view-model");
        var vmObj = (new Function('return ' + vm)());
        vmObj = JSON.parse(jsonstr);

        // span 이라... 안되겟네...
        var origin = ""; //$("#" + formId).serialize();
        vmObj["_FORM_ORIGIN_DATA"] = "";

        var keys = Object.keys(vmObj); //키를 가져옵니다. 이때, keys 는 반복가능한 객체가 됩니다.
        for (var i = 0; i < keys.length; i++) {
            var key = keys[i];

            //trace("key : " + key + ", value : " + vmObj[key])

            var value = vmObj[key];
            var valueObj = [];

            var inputtype = "", codeDs = "";
            if (viewMode == "VIEW") {
                inputtype = isnull($("#" + formId).find("span[name='" + key + "']").attr("_data-input-type"), "");
                codeDs = isnull($("#" + formId).find("span[name='" + key + "']").attr("_data-code-ds"), key) ;
            } else {
                inputtype = isnull($("#" + formId).find("input[name='" + key + "']").attr("_data-input-type"), "");
                codeDs = isnull($("#" + formId).find("input[name='" + key + "']").attr("_data-code-ds"), keys);
            }

            //trace("codeDs : " + inputtype + " / " + codeDs)
            var obj = $("#" + formId).find(":input[name='" + key + "']").data(KendoUtils.getkendoType(inputtype));
 
            if (inputtype.toUpperCase() == "combobox".toUpperCase()) {
                var codeList = _dsCodeList[codeDs];
                vmObj[key] = (viewMode == "VIEW" ? KendoUtils.getText4ddlDs(codeList, value) : value);
                if (viewMode != "VIEW") {
                    obj.value(value); obj.trigger("change");
                }
            } else if (inputtype.toUpperCase() == "mccombobox".toUpperCase()) {

                if (viewMode == "VIEW") { // 쿼리에서 value 대신 text 넘겨줄것.  _NM
                    // mcc 는 공통코드를 사용하지 않고 따로 데이터를 받아오므로 아래 코드를 적용할수 없다. 5/20
                    //var codeList = _dsCodeList[codeDs];
                    //var text = KendoUtils.getText4ddlDs(codeList, value);
                    //vmObj[key] = (text == "" ? value : text);

                    var text = vmObj[key + "_NM"];  // _NM
                    vmObj[key] = text;
                } else {
                    var mcCombo = $("#" + key).data("kendoMultiColumnComboBox");
                    if (codeDs == "remote") { // 코드도움...    // ##########  #AAA 동기화 할것.
                        var text = vmObj[key + "_NM"];  // text CHARGER_NM
                        var options = mcCombo.options;
                        options.text = text;
                        options.autoBind = false;  // important !!!
                        mcCombo.setOptions(options);
                    } else { // 코드 리스트가 화면에 있다. (X)
                        //var codeList = _dsCodeList[codeDs];
                        //trace("key : " + key + "  / " + value)
                        vmObj[key] = value;
                    }

                    obj.value(value); obj.trigger("change");
                }

            } else if (inputtype == "multicombobox") { //  // ##########  #AAA 동기화 할것.  
                value = isnull(value, "");

                value = replaceAll(value, '"', ''); //  ['LC_BASIC','LC_IMO'] -->  ["LC_BASIC","LC_IMO"]
                value = replaceAll(value, "'", '');
                var codeList = _dsCodeList[codeDs];
                var arr = value.split(",");
                var multiText = "";
                for (var k = 0; k < arr.length; k++) {
                    var xx = arr[k].trim().replace("[", "").replace("]", "");
                    if (isnull(xx, "") == "") continue;

                    var cc = (viewMode == "VIEW" ? KendoUtils.getText4ddlDs(codeList, xx) : xx);

                    if (k == 0) {
                        multiText += cc;
                        valueObj.push(cc);
                    } else {
                        multiText += ", " + cc;
                        valueObj.push(cc);
                    }

                }
                //trace("multiText :" + multiText);

                vmObj[key] = (viewMode == "VIEW" ? multiText : valueObj);

                if (viewMode == "VIEW") {
                    // 
                } else {
                    obj.value(valueObj); obj.trigger("change");
                }

            }

        }

        kendo.bind($("#" + formId), vmObj); // ??? 
    },
 
    // inert , update , delete  
    save : function(params){
        trace("Ajax save to json ......... ");

        // parameters 
        var url = params.url; // $("form[name='form_cond']").serialize();
        var inputData = params.data;
        var resultBind = params.resultBind;
        var funcVar = params.funcVar;
        var svid = isnull(params.svid, "SAVE");
        var msg = isnull(params.msg, "");
        var changed_data = null;
        var procType = isnull(params.procType, "");

        var isChange = false; 
        changed_data = {};

        var obj = Object.keys(inputData);

        for (key in Object.keys(obj)) {
            var key = obj[key];
            var dataContainer = "", role = "", tagNm = "";

            if (typeof (inputData[key]) == "string") {
                dataContainer = isnull(inputData[key], "").replace("#", "");
                role = isnull($("#" + dataContainer).attr("data-role"), "");
                if ($("#" + dataContainer)[0] != undefined) {
                    tagNm = isnull($("#" + dataContainer)[0].tagName, "").toUpperCase();
                }
            } else {
                role = "object"; 
            }
            
            if (role == "grid" || role == "treelist") { // kendo grid
                if (!isChange) {
                    isChange = KendoUtils.hasChanges(dataContainer);
                }

                // grid --> changed rows
                var this_data = [];
                var grid = $("#" + dataContainer).data(KendoUtils.getkendoType(role));
                var gridData = grid.dataSource.view();

                for (var i = 0; i < grid.dataSource.view().length; i++) {

                    var isNew = KendoUtils.isNew(gridData[i]); 

                    trace("gridData[i].dirty: " + gridData[i].dirty + " / isNew : " + isNew + " / _ROW_TYPE: " + gridData[i]._ROW_TYPE + " / autoSync : " + grid.dataSource.options.autoSync)

                    // isNew --> _ROW_TYPE 으로 Insert 구분하겠다.
                    if (gridData[i].dirty || isNew || gridData[i]._ROW_TYPE == "I") { // gridData[i].isNew() xxxx
                        var item = gridData[i];
                        item._PROC_TYPE = (procType == "" ? "SAVE" : procType);
                        if (gridData[i]._ROW_TYPE == "I") {
                            item._ROW_TYPE = "I";
                        } else if (gridData[i].dirty) {
                            item._ROW_TYPE = "U";
                        } else {
                            alert("_ROW_TYPE  not defined.......");
                            return;
                        }

                        this_data.push(item);
                    }
                }

                changed_data[key] = JSON.parse(JSON.stringify(this_data));  // kendo dataSource 

            } else if (tagNm == "FORM") {
                // form 
                /////isChange = KendoUtils.hasChanges(dataContainer);
                isChange = true;  // multiselector ... 반영되어야함....

                $("#" + dataContainer).find("input[name='_PROC_TYPE']").remove();
                $("#" + dataContainer).append('<input type="hidden" name="_PROC_TYPE" value="' + (procType == "" ? "SAVE" : procType) + '" />');

                // multi combo 
                var multi = $("#" + dataContainer).find("[_data-input-type='multicombobox']");
                $(multi).each(function (index, item) {
                    var id = $(item).attr("id");
                    var name = $(item).attr("name");
                    //trace("KendoUtils : " + KendoUtils.get(id) + " / " + _vmCond.get(name) );
                    $(item).val(KendoUtils.get(id)); // 'LC_BASIC,LC_FOS'  -->  ["A", "B"]  --> save 
                });

                changed_data[key] = KendoAjaxTran.serializeObject(dataContainer);

            } else if (role == "object") {
                isChange = true;    
                var this_data = inputData[key];
                changed_data[key] = JSON.parse(JSON.stringify(this_data));  // kendo dataSource 
            } else {
                changed_data[key] = inputData[key];
                trace(" inputData[key] : " + inputData[key] + " / " + key)
            }

        }
        trace(url)
        if (url == '/Analysis/ChangeSampleState' || url == '/Analysis/UpdateBrabenderGraphData') {
        }else if (!isChange && procType != "DELETE") {
            alert("변경된 항목이 없습니다.");
            return false;
        }

        //trace("changed_data : " + jsonStr(changed_data) );

        // svid 가 SAVE_autosaveDetail, 2, saveChkRow 일 경우, msg == false 처리
        if (svid == "SAVE_autosaveDetail" || svid == "SAVE_autosaveDetail2" || svid == "saveChkRow") {
            msg = false;
        }

        if (typeof (msg) == "boolean" && msg == false) {
            // 화면단에서 메세지 처리했거나, 메세지가 필요없을 경우 
        } else {
            if (msg != "") {
                if (!confirm(msg + " 하시겠습니까 ? ")) return;
            } else {
                if (!confirm("처리" + " 하시겠습니까 ? ")) return;
            }
        }


        // ajax option 
        var dataType = "json";
        var contentType = "application/x-www-form-urlencoded; charset=utf-8";
        ////var contentType = "application/json" ;
        var token = $('[name=__RequestVerificationToken]').val();

        $.ajax({
            type: "POST",
            url: url,
            ////data: { model: changed_data },
            data: changed_data,
            dataType: dataType,
            contentType: contentType,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("AjaxCall", "Y");
                xhr.setRequestHeader("RequestVerificationToken", token);
                WAIT.startLoader();
            }, // 필수로 넘겨줄것
            success: function (data) {

                if (procType == "DELETE" && params._DEL_GRID_OBJ != undefined) { // grid checked row delete 
                    // 그리드 및 데이터소스 row remove.. 
                    var delrowId = params._DEL_UID
                    var delGrid = params._DEL_GRID_OBJ;
                    // reverse 로 담겼으므로.. forword 로 삭제    
                    for (var i = 0; i < delrowId.length; i++) {
                        var dataRow = delGrid.dataSource.getByUid(delrowId[i]);
                        delGrid.dataSource.remove(dataRow);
                    }
                    trace("datasource length : " + delGrid.dataSource.data().length)
                }

                //if (typeof (msg) == "boolean" && msg == false) {
                //    // 화면단에서 메세지 처리했거나, 메세지가 필요없을 경우
                //} else {
                //    if (msg != "") {
                //        if (!confirm(msg + " 처리되었습니다")) return;
                //    } else {
                //        if (!confirm("처리되었습니다.")) return;
                //    }
                //}

                // svid 가 SAVE_autosaveDetail, 2 - 확인완료 처리 일 경우, alert 제거
                if (svid == "SAVE_autosaveDetail" || svid == "SAVE_autosaveDetail2" ) {
                    //alert("처리가 완료되었습니다.");
                } else {
                    alert("처리가 완료되었습니다.");
                }

                funcVar.apply_post(svid, data);  // 우선 무조건 재 조회로...

            },
            error: function (request, status, error) {
                KendoAjaxTran.error_post(request, status, error);
            },
            complete: function () { 
                WAIT.stopLoader();
            }
        });
    },
 
    del: function (params) { //   delete : form  
        params.procType = "DELETE";
        this.save(params);
    },

    delRow: function (params) { //   grid : one row delete  /  row command button click 
        params.procType = "DELETE";

        // 1건 삭제 ,
        var resultBind = params.resultBind;  // 입력이 [{}] object 라서.. 결과 바인딩 ID 기준으로... grid 찾는다.
        var bindId = this.checkBindOne(resultBind);  // bind 1개만 허용
        if (bindId == "") return false;

        var delData = this.checkBindOne(params.data)[0];  // [{}]
        var role = $("#" + bindId).attr("data-role");
        var grid = $("#" + bindId).data(KendoUtils.getkendoType(role));

        if (delData._ROW_TYPE == "I") {
            var dataSource = grid.dataSource;
            dataSource.remove(delData);
            dataSource.sync();
            //trace("deleted.. after.. : " + dataSource.data().length)

            if (KendoUtils.isExistRowNum(grid)) {
                KendoUtils.setRowNum2(bindId);
            }
            return;
        } else {
            this.save(params); // 1건삭제 Tran 
        }
    },

    delCheckedRow: function (params) { //   grid : checked...rows 
        params.procType = "DELETE";

        var inputData = params.data;
        var bindId = this.checkBindOne(inputData);  // bind 1개만 허용
        if (bindId == "") return false;

        var tr = $("#" + bindId + " tbody tr");
        var checkedData = [];

        var role = $("#" + bindId).attr("data-role");
        var grid = $("#" + bindId).data(KendoUtils.getkendoType(role));

        // I 모드 삭제 (reverse !!!)
        var removeCnt = 0, idx = 0;
        var delrowId = [];
        params._DEL_GRID_OBJ = grid;

        $($(tr).get().reverse()).each(function (index) { // Important !!! reverse 
            var row = $(this);
            //var chk = $(row).find("td._checkAll").find("input").is(":checked");
            var chk = $(row).find("._chkCol").is(":checked");
            if ($(row).find("._chkCol").length > 0) { // KendoUtils.js 에서 완성된 그리드 
                chk = $(row).find("._chkCol").is(":checked");
            } else { // selectable 사용한 그리드 로 간주 
                chk = $(row).find(".k-checkbox[data-role='checkbox']").is(":checked");
            }


            if (chk) {
                var dataItem = grid.dataItem(row);
                delrowId[idx++] = dataItem.uid;

                if (dataItem._ROW_TYPE == "I") {
                    // dummy  --> callback...
                } else {
                    // 실제 삭제할 데이터 
                    removeCnt++;
                }
            }
        });

        if (removeCnt > 0) {
            params._DEL_UID = delrowId;
            this.checkedProc(params);  // bind 1개만 지원한다. 
        } else {
            // I 모드 삭제 , reverse 로 담겼으므로.. forword 로 삭제    
            for (var i = 0; i < delrowId.length; i++) {
                var dataRow = grid.dataSource.getByUid(delrowId[i]);
                grid.dataSource.remove(dataRow);
            }
            //trace("dataSource length : " + grid.dataSource.data().length);
        }

        //trace("delrowId.length : " + delrowId.length)

        if (delrowId.length == 0) {
            alert("체크된 데이터가 없습니다.");
            return false;
        }
    },

    checkedProc: function (params) {
        var inputData = params.data;

        var bindId = this.checkBindOne(inputData);  // bind 1개만 허용
        if (bindId == "") return false;

        /////var tr = $("#" + bindId + " tbody tr");
        var checkedData = [];

        var role = $("#" + bindId).attr("data-role");
        var grid = $("#" + bindId).data(KendoUtils.getkendoType(role));
        var selector = KendoUtils.getCheckRowSelector(bindId);

        var tr = $("#" + bindId).find(".k-grid-content>table>tbody").children(selector);
        $.each(tr, function (e) {
            var row = $(this);
            var chk = $(row).find("._chkCol").is(":checked");
            if ($(row).find("._chkCol").length > 0) { // KendoUtils.js 에서 완성된 그리드 
                chk = $(row).find("._chkCol").is(":checked");
            } else { // selectable 사용한 그리드 로 간주 
                chk = $(row).find(".k-checkbox[data-role='checkbox']").is(":checked");
            }
 
            if (chk) {
                ////var grid = row.closest(".k-grid").data("kendoGrid");
                if (params._DEL_GRID_OBJ != undefined) { // grid checked delete  ... 
                    var dataItem = grid.dataItem(row);
                    if (dataItem._ROW_TYPE == "I") return true;  // continue --> 저장된 데이터만 담는다. I 모드 데이터는 callback 에서 삭제한다.
                    checkedData.push(dataItem);
                } else {
                    var dataItem = grid.dataItem(row);
                    checkedData.push(dataItem);
                }
            }
        });

        //trace("checkedData : " + checkedData.length)
        if (checkedData.length == 0) {
            alert("체크된 데이터가 없습니다.");
            return;
        }

        params.data = { model: checkedData };

        this.save(params);
    },
 
    excel: function (params) {
        params.isExcel = true;  // important !!! 

        var resultBind = params.resultBind;  // 입력이 [{}] object 라서.. 결과 바인딩 ID 기준으로... grid 찾는다.
        var bindId = KendoAjaxTran.checkBindOne(resultBind);  // bind 1개만 허용
        if (bindId == "") return false;

        KendoAjaxTran.search(params);  // 조회와 동일한 로직...
    },

    excel_post: function (params, data) {
        var bindId = KendoAjaxTran.checkBindOne(params.resultBind);  // bind 1개만 허용
        var grid = $("#" + bindId).data("kendoGrid");

        // temp grid 에 데이터를 바인딩한후 엑셀 추출 
        $("body").find("#grid4Excel_temp").remove();
        $("body").append('<div id="grid4Excel_temp"></div>');

        var dsExcel = new kendo.data.DataSource({
            // dummy
        });

        $("#grid4Excel_temp").kendoGrid({
            dataSource: dsExcel,
            columns: grid.options.columns,
            excel: {
                allPages: true,
                fileName: isnull(params.fileNm, "excel") + ".xlsx",
            },
        });
        var grid4Excel = $("#grid4Excel_temp").data("kendoGrid");

        grid4Excel.setDataSource(data.result0);
        grid4Excel.saveAsExcel();
    },

    error_post: function (request, status, error) {
        WAIT.stopLoader();

        if (request.status == "401") {
            alert("처리 권한이 없습니다. [" + request.status + "]");  // Status401Unauthorized   -->  IntegratedExceptionFilterAttribute 안걸린...에러
            return;
        }

        if (request.responseJSON == undefined) {
            alert("error occured....not filtered " + request.status);  // IntegratedExceptionFilterAttribute 안걸린...에러..--> 확인할것
            return;
        }

        if (request.status == "400" || request.status == "440") { // bad request = 400, session time out = 440
            var status = request.responseJSON._status;
            var msg = request.responseJSON._msg;

            //alert(msg + "[" + status + "]" + "\r" + jsonStr(request.responseJSON._errorList));

            var errStr = "";
            var json = request.responseJSON._errorList;

            if (json == null) {
                if (request.responseJSON._msg != undefined) {
                    alert(request.responseJSON._msg);
                }
            } else {
                if (json.length == undefined) {
                    for (key in json) {
                        errStr += json[key] + '  \r';
                    }
                } else {
                    for (var i = 0; i < json.length; i++) {
                        var temp = "";
                        for (key in json[i]) {
                            temp += json[i][key] + '  \r';
                        }

                        errStr += temp + " \r";
                    }
                }

                if (msg.indexOf("ValidateModel") >= 0) {
                    alert("[입력항목 확인]" + "\r" + errStr);
                } else {
                    alert(msg + "[" + status + "]" + "\r" + jsonStr(request.responseJSON._errorList));
                }
            }
 
            if (request.status == "440") { // session timeout 
                window.location.href = "/login/logout";
            }
        }
        else {
            alert("error occured....");
        }
    },

    checkBindOne: function (param_data) {
        var inputData = param_data;

        var bindId = "";
        var obj = Object.keys(inputData);
        var cnt = 0;
        for (key in Object.keys(obj)) {
            var key = obj[key];
            bindId = inputData[key];
            //trace("bindId : " + bindId)
            if (cnt > 0) {
                alert("1개 입력만 지원합니다.");
                return "";
            }
            cnt++;
        }

        return bindId;
    },

    serializeObject: function (id) {
        var obj = null;
        try {
            if ($("#" + id)[0].tagName && $("#" + id)[0].tagName.toUpperCase() == "FORM") {
                var arr = $("#" + id).find(":input").serializeArray();
                if (arr) {
                    obj = {};
                    jQuery.each(arr, function () {
                        obj[this.name] = this.value;
                        //trace("serializeObject : "  + this.name + "/" + this.value )
                    });
                }//if ( arr ) {
            }
        } catch (e) {
            alert(e.message);
        } finally {
        }

        return obj;
    },

    getData4MCC : function (e, id, constId){ // for remote filter
        var keyword = isnull(e.sender._prev, "");
        if (keyword == "") {
            trace("change...... blank..")
            return;
        } else {
            trace("change...... not blank..")
        }
 
        // multi column combo 추가 설정 
        var mcCombo = $("#" + id).data("kendoMultiColumnComboBox");
        var params = { KEYWORD: keyword, CONST: constId };

        $.ajax({
            type: "POST",
            url: "/Common/GetCodeHelp",

            data: { model: params }, 
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("AjaxCall", "Y");
                //xhr.setRequestHeader("RequestVerificationToken", token);
                //$("#div_load_image").show();
            }, // 필수로 넘겨줄것
            success: function (data, status) {
                //trace("data : " + data.length)
                //mcCombo.setDataSource(data);
                mcCombo.dataSource.data(data);  // for ajax 
                //mcCombo.refresh();
            },
            error: function (request, status, error) {
                KendoAjaxTran.error_post(request, status, error);
            },
            complete: function (req, error) {

            }
        });
    },

    // 파일 다운로드 
    fileDownload: function (url, filePath, fileNm, subDir1) {
        //trace("filePath : " +  '{fileNm: "' + fileNm + '", ' + 'filePath:"' + filePath + '"}'  );
        var params = { fileNm: fileNm, filePath: filePath, subDir: subDir1 };

        $.ajax({
            type: "POST",
            url: url,  // "/Equip/DownloadFile",
            data: params,
            ////contentType: "application/json; charset=utf-8",
            dataType: "text",  // 받는데이터 
            beforeSend: function (xhr) {
                xhr.setRequestHeader("AjaxCall", "Y");
                //xhr.setRequestHeader("RequestVerificationToken", token);
                WAIT.startLoader();
            }, // 필수로 넘겨줄것
            success: function (data) {
                //Convert Base64 string to Byte Array.
                var bytes = Base64ToBytes(data);

                //Convert Byte Array to BLOB.
                var blob = new Blob([bytes], { type: "application/octetstream" });

                //Check the Browser type and download the File.
                var isIE = false || !!document.documentMode;
                if (isIE) {
                    window.navigator.msSaveBlob(blob, fileNm);
                } else {
                    var url = window.URL || window.webkitURL;
                    link = url.createObjectURL(blob);
                    var a = $("<a id='filedownloadtemp'/>");
                    a.attr("download", fileNm);
                    a.attr("href", link);
                    $("body").append(a);
                    a[0].click();
                    //$("body").remove(a);
                    $("#filedownloadtemp").remove();
                }
            },
            error: function (request, status, error) {
                KendoAjaxTran.error_post(request, status, error);
            },
            complete: function (req, error) {
                //$("#div_load_image").hide();
                WAIT.stopLoader();
            }
        });
    },

};

/************************************************************
// loading 처리 
*************************************************************/
var WAIT = {
    startLoader: function () {
        $("#div_load_image").show();
    },

    stopLoader: function () {
        $("#div_load_image").hide();
    },

    // ajax 처리중 여부 체크
    ajax_isRun: function (svid, menu) {
        //if (global_ajax_isRun) {
            //alert("처리중...." + menu);
            //return false; 
        //}
        //return true;
    },

    // ajax 처리 중... 
    ajax_runniung: function (svid) {
        //global_ajax_isRun = true;
    },

    // ajax 처리 완료 
    ajax_completed: function (svid) {
        //global_ajax_isRun = false;
    },
};
 
function Base64ToBytes(base64) {
    var s = window.atob(base64);
    var bytes = new Uint8Array(s.length);
    for (var i = 0; i < s.length; i++) {
        bytes[i] = s.charCodeAt(i);
    }
    return bytes;
};

/***************************************************************************************
// frequently used script 
***************************************************************************************/
var trace = function (str) {
    console.log(str);
};

var isnull = function (value, value2) {
    if (value == '' || value == null || value == undefined || value == NaN) {
        value = value2;
    }
    return value;
};

var jsonStr = function (json) { // jsonStr 값
    var str = "";
    if (json == null) return "";

    if (json.length == undefined) {
        for (key in json) {
            str += key + ':' + json[key] + '  \r';
        }
    } else {
        for (var i = 0; i < json.length; i++) {
            var temp = "";
            for (key in json[i]) {
                temp += key + ':' + json[i][key] + '  \r';
            }

            str += temp + " \r";
        }
    }

    return str;
};

var formatDate = function(dateStr) {
    if (!dateStr) return "";
        var formatNum = '';
        // 공백제거
        dateStr = dateStr.replace(/\s/gi, "");
        try {
            if (dateStr.length == 8) {
                formatNum = dateStr.replace(/(\d{4})(\d{2})(\d{2})/, '$1-$2-$3');
            }
        } catch (e) {
            formatNum = dateStr;
        }
        return formatNum;
};

var addDays = function (date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}

var uuidv4 = function () {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
};

// 3자리 콤마 
var numberWithCommas = function (x) {
    x = String(x);
    var xArr = x.split(".");
    var x = xArr[0].replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",");
    var y = "";
    if (xArr.length > 1) {
        y = String(Number(xArr[1]));
        if (!(y == "" || y == "0")) {
            y = "." + y;
        } else {
            y = "";
        }
    } else {
        y = "";
    }

    return x + y;
};

// Post 전송 
var Trans = {
    postForm : function (path, params, method) {
        method = method || 'post';

        var form = document.createElement('form');
        form.setAttribute('method', method);
        form.setAttribute('action', path);
        form.setAttribute('name', "form4postForm");

        for (var key in params) {
            if (params.hasOwnProperty(key)) {
                var hiddenField = document.createElement('input');
                hiddenField.setAttribute('type', 'hidden');
                hiddenField.setAttribute('name', key);
                hiddenField.setAttribute('value', params[key]);

                form.appendChild(hiddenField);
            }
        }

        document.body.appendChild(form);
        form.submit();
    }, 
}; 



 

 