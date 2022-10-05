/***************************************************************************************
// kendo custom common script  
***************************************************************************************/
var KendoUtils = {
    checkType: "Y",  // Y, true , 1 
    uncheckType: "N", // N, false, 0 
    checkAllCol: "_chkAll",
    checkCol: "_chkCol",

    showHideField: function (gridId, fileds, visible) { // grid fileds show/hide 
        var grid = this.getKendoGrid(gridId);  // grid, treelist 

        $(fileds).each(function (index, value) {
            if (visible) {
                grid.showColumn(value);
            } else {
                grid.hideColumn(value);
            }
        });
    },

    showHideField_LCF: function (gridId, fileds, visible, jsonCompound) { // grid fileds show/hide 
        trace("showHideField_LCF")
        trace(jsonCompound)

        var grid = this.getKendoGrid(gridId);  // grid, treelist 
        

        $(fileds).each(function (index, value) {
            if (visible) {
                grid.showColumn(value);
            } else {
                grid.hideColumn(value);
            }
        });
    },

    getRowIndexByUID: function (gridId, uid) {
        var rowIdx = null;
        if (typeof (gridId) == "string") {
            var grid = this.getKendoGrid(gridId);  // grid, treelist 
            rowIdx = grid.tbody.find("tr[data-uid='" + uid + "']").find(".k-state-selected").prevObject[0].rowIndex;
        } else if (typeof (gridId) == "object") { // kendo grid object 
            rowIdx = gridId.tbody.find("tr[data-uid='" + uid + "']").find(".k-state-selected").prevObject[0].rowIndex;
        }

        return rowIdx;
    },

    getRowDataByUID: function (gridId, uid) {
        var tr = $("#" + gridId).find("tbody>tr[data-uid='" + uid + "']");
        var data = KendoUtils.getKendoGrid(gridId).dataItem(tr);
        return data;
    }, 

    set: function (id, value) {
        id = id.replace("#", "");
        var applyCnt = 0;
        var forms = $("form");  // 현재화면 전체   

        // grid col == form field  id 동일할 경우 셀클릭시 셀내의 id 를 찾아버림.
        // 화면의 form 내에 존재하는 id 를 찾도록한다.  
        $(forms).each(function (index, item) {
            if ($(this).attr("id") == undefined) return true;

            var datainputtype = $("#" + id).attr("_data-input-type");
            var datarole = $("#" + id).attr("data-role");

            if (datainputtype == undefined) return true; // continue 

            var obj = $(this).find("#" + id).data(KendoUtils.getkendoType(datainputtype));

            if (obj == undefined) {
                return true;
            }

            obj.value(value);
            obj.trigger("change");

            applyCnt++;

            return false;
        });

        if (applyCnt == 0) {
            alert("ID 확인....")
        }

    },

    get: function (id) {
        id = id.replace("#", "");

        var forms = $("form");  // 현재화면 전체 
        var value = null;

        // grid col == form field  id 동일할 경우 셀클릭시 셀내의 id 를 찾아버림.
        // 화면의 form 내에 존재하는 id 를 찾도록한다.  
        $(forms).each(function (index, item) {
            var datainputtype = $(item).find("#" + id).attr("_data-input-type");

            if (datainputtype == undefined) return true; // continue 

            var obj = $(item).find("#" + id).data(KendoUtils.getkendoType(datainputtype));
            if (obj == undefined) {
                return true;
            }

            value = obj.value();
            return false;
        });

        return value;
    },

    copyCodeList: function (origin, copyTo, blankOption) {
        blankOption = isnull(blankOption, "");
        _dsCodeList[copyTo] = _dsCodeList[origin];

        if (blankOption == "1") { // 조회항목 
            _dsCodeList[copyTo].push({ VALUE: "", TEXT: "전체" });
        } else if (blankOption == "2") { // 저장 항목 
            _dsCodeList[copyTo].push({ VALUE: "", TEXT: "" });
        }
    },

    addCodeItem: function (codeConst, item, isFirst) {  // CD_TYP , { TEXT: "영업", VALUE: "SA" }
        isFirst = isnull(isFirst, false);
        if (isFirst) {
            _dsCodeList[codeConst].unshift(item);
        } else {
            _dsCodeList[codeConst].push(item);
        }
    },

    addWholeItem: function (codeConst, isFirst) {  // 전체 항목 추가 
        isFirst = isnull(isFirst, false);
        var item = { TEXT: "All", VALUE: "" };
        if (isFirst) {
            _dsCodeList[codeConst].unshift(item);
        } else {
            _dsCodeList[codeConst].push(item);
        }
    },

    getKendoGrid: function (grid) {  // grid or treelist
        if (typeof (grid) == "object") return grid;  // kendo grid object 

        grid = grid.replace("#", "");
        var gridObj = null;

        var role = $("#" + grid).attr("data-role");

        if (role == "grid" || role == "treelist") { // kendo grid
            gridObj = $("#" + grid).data(KendoUtils.getkendoType(role));  // kendoGrid, kendoTreeList
        } else {
            if (role == undefined) return;

            alert("not defined..... getKendoGrid " + role)
            return;
        }

        return gridObj;
    },

    data_tag: "input, select, textarea",

    setInputKendo: function (form_id, viewModel, ctrQFunc) {
        viewModel = isnull(viewModel, "");
        var __viewMode = null;

        //trace("viewModel : " + viewModel);

        form_id = form_id.replace("#", "");
        var forms = $("form");  // 현재화면 전체 

        $(forms).each(function (index, item) {
            if ($(item)[0].tagName && $(item)[0].tagName.toUpperCase() == "FORM") {
                var formName = $(item).attr("name");
                var formId = $(item).attr("id");
                var fields = { "uid": "dummy" };
                if ($(item).attr("name") == "mainForm") return true;  // continue 

                if (form_id != formId) return true;  // continue 

                var that = $("form[name='" + formName + "']");

                //if (viewModel != "") {
                //    $(that).attr("_data-view-model", viewModel);
                //    kendo.bind($("#" + form_id), __viewMode);  // Bind the View to the View-Model  
                //}

                // ### MSMC (multi-selector & multi-column) ###
                $(that).find("[_composite='msmc']").each(function (index, item) {
                    var attr = isnull($(item).attr("_data-input-type"), "");
                    var msmcId = isnull($(item).attr("id"), "") + "_mcc";
                    var codeDs = isnull($(item).attr("_data-code-ds"), "");

                    if (attr == "multicombobox") {
                        trace("msmcId : " + msmcId + " / " + codeDs);
                        var html = '<input id="' + msmcId + '" name="' + msmcId + '" _data-input-type="mccombobox" _data-code-ds="' + codeDs + '" hidden _composite="msmc" />';
                        $(item).parent().append(html);
                    }
                });

                if ($(that)[0].tagName && $(that)[0].tagName.toUpperCase() == "FORM") {

                    var arr = $(that).find(KendoUtils.data_tag).serializeArray();  // name tag required
                    if (arr) {
                        jQuery.each(arr, function () {

                            if (this.name == "__RequestVerificationToken") return true; // continue 
                            var there = $("form[name='" + formName + "']").find("[name='" + this.name + "']");
                            var input_id = isnull($(there).attr("id"), "");

                            if (input_id == "") return true; // continue

                            var datainputtype = $(there).attr("_data-input-type");  // this.name
                            var datacodeds = isnull($(there).attr("_data-code-ds"), "");         // code datasource
                            var format = isnull($(there).attr("_data-format"), "");
                            var required = isnull($(there).attr("required"), "");
                            var labelhtml = isnull($("label[for='" + input_id + "']").html(), "");
                            if (labelhtml != "" && (required == "required" || required)) { // 필수 
                                $("label[for='" + input_id + "']").html('<span class="text-danger">' + '*' + labelhtml + '</span>');
                            }

                            var view_Mode = isnull($(there).attr("_view_mode"), "");
                            if (view_Mode == "VIEW") { // 읽기전용
                                var baseTD = $(there).closest("td");
                                $(baseTD).html("");
                                $(baseTD).html('<span id="' + input_id + '" name="' + isnull($(there).attr("name"), "") + '" data-bind="text: ' + this.name + ' " _data-code-ds="' + datacodeds + '" _data-input-type="' + datainputtype + '"   >');
                                $(baseTD).height(25 * 1.4);

                                return true; // continue 
                            }

                            if (datainputtype.toUpperCase() == "combobox".toUpperCase()) {
                                // config....basic dropdownlist 
                                $("#" + input_id).kendoComboBox({
                                    dataTextField: "TEXT",    // fixed  
                                    dataValueField: "VALUE",  // fixed  
                                    dataSource: (datacodeds == "" ? _dsCodeList[input_id] : _dsCodeList[datacodeds]),  // ViewBag.CodeList 항목명
                                    filter: "contains",
                                    suggest: true,
                                    index: -1,
                                    placeholder: "Select...",

                                    change: KendoUtils.setEvent(input_id, "change"),
                                    filtering: KendoUtils.setEvent(input_id, "filtering"),
                                    cascade: KendoUtils.setEvent(input_id, "cascade"),
                                    close: KendoUtils.setEvent(input_id, "close"),
                                    open: KendoUtils.setEvent(input_id, "open"),
                                    dataBound: KendoUtils.setEvent(input_id, "dataBound"),
                                    select: KendoUtils.setEvent(input_id, "select"),

                                });

                                // _defaultvalue
                                var defaultvalue = isnull($("#" + input_id).attr("_defaultvalue"), "");
                                //trace("defaultvalue : " + defaultvalue)
                                if (defaultvalue.toUpperCase() == "USE_DEPT".toUpperCase()) {
                                    KendoUtils.set(input_id, SESSION_USE_DEPT);
                                } else if (defaultvalue.toUpperCase() == "USER".toUpperCase()) {
                                    KendoUtils.set(input_id, SESSION_USER_ID);
                                }

                            } else if (datainputtype.toUpperCase() == "multicombobox".toUpperCase()) {
                                var composite = isnull($(there).attr("_composite"), "").toLowerCase();

                                $("#" + input_id).kendoMultiSelect({
                                    dataSource: (datacodeds == "" ? _dsCodeList[input_id] : _dsCodeList[datacodeds]),
                                    dataTextField: "TEXT",
                                    dataValueField: "VALUE",
                                    placeholder: "Select items...",
                                    autoClose: false,

                                    change: KendoUtils.setEvent(input_id, "change"),
                                    filtering: KendoUtils.setEvent(input_id, "filtering"),
                                    deselect: KendoUtils.setEvent(input_id, "deselect"),
                                    close: KendoUtils.setEvent(input_id, "close"),
                                    open: composite == "msmc" ? KendoUtils.MSMC_multiSelector_onOpen : KendoUtils.setEvent(input_id, "open"),
                                    dataBound: KendoUtils.setEvent(input_id, "dataBound"),
                                    select: KendoUtils.setEvent(input_id, "select"),
                                });

                                if (composite == "msmc") { // span re-sortable 
                                    KendoUtils.MSMC_sortable(input_id);
                                }

                            } else if (datainputtype.toUpperCase() == "mccombobox".toUpperCase()) {
                                var composite = isnull($(there).attr("_composite"), "").toLowerCase();

                                $("#" + input_id).kendoMultiColumnComboBox({
                                    dataSource: (datacodeds == "" ? _dsCodeList[input_id] : _dsCodeList[datacodeds]),
                                    dataTextField: "TEXT",
                                    dataValueField: "VALUE",
                                    placeholder: "Select...",
                                    height: 400,
                                    autoBind: false,
                                    columns: [
                                        { field: "TEXT", title: "TEXT", width: 100 },
                                        { field: "VALUE", title: "VALUE", width: 100 },
                                        //{ field: "ATT1", title: "직위", width: 100 },
                                        //{ field: "ATT2", title: "전화번호", width: 140},
                                    ],
                                    footerTemplate: 'Total #: instance.dataSource.total() # items found',
                                    filter: "contains",
                                    //filterFields: ["TEXT", "ContactTitle", "CompanyName", "Country"],

                                    select: KendoUtils.setEvent(input_id, "select"),
                                    change: composite == "msmc" ? KendoUtils.MSMC_multiColumn_onChange : KendoUtils.setEvent(input_id, "change"),
                                    close: composite == "msmc" ? KendoUtils.MSMC_multiColumn_onClose : KendoUtils.setEvent(input_id, "close"),
                                    open: KendoUtils.setEvent(input_id, "open"),
                                    filtering: (datacodeds.toUpperCase() == "remote".toUpperCase() ? KendoUtils.preventDefault : KendoUtils.setEvent(input_id, "filtering")),
                                    dataBound: KendoUtils.setEvent(input_id, "dataBound"),
                                });

                                if (composite == "msmc") {
                                    KendoUtils.MSMC_focusout(input_id);
                                    KendoUtils.MSMC_sortable2(input_id);
                                }

                            } else if (datainputtype.toUpperCase() == "datebox".toUpperCase()) {
                                var daetFormat = isnull(format, "") == "" ? "yyyy-MM-dd" : format;
                                var parseFormats = isnull(format, "") == "" ? "yyyyMMdd" : format;

                                // config....basic datebox  
                                $("#" + input_id).kendoDatePicker({
                                    animation: {
                                        close: {
                                            effects: "fadeOut zoom:out",
                                            duration: 300
                                        },
                                        open: {
                                            effects: "fadeIn zoom:in",
                                            duration: 300
                                        }
                                    },
                                    format: daetFormat,
                                    parseFormats: [parseFormats], //format also will be added to parseFormats

                                    change: KendoUtils.setEvent(input_id, "change"),
                                    close: KendoUtils.setEvent(input_id, "close"),
                                    open: KendoUtils.setEvent(input_id, "open"),
                                });

                                // _defaultvalue
                                var defaultvalue = isnull($("#" + input_id).attr("_defaultvalue"), "");
                                if (defaultvalue != "") {
                                    KendoUtils.set(input_id, gfn_getDay(defaultvalue, input_id));
                                }

                            } else if (datainputtype.toUpperCase() == "numberbox".toUpperCase()) {
                                // config....basic number
                                $("#" + input_id).kendoNumericTextBox({
                                    format: "p", // Define the percentage format signified with the letter "p".
                                    value: 0.15, // 15 %

                                    change: KendoUtils.setEvent(input_id, "change"),
                                    spin: KendoUtils.setEvent(input_id, "spin"),
                                });

                            } else if (datainputtype.toUpperCase() == "checkbox".toUpperCase()) {
                                $("#" + input_id).attr("value", KendoUtils.checkType);  // Y 설정
                                $("#" + input_id).css("width", 20);
                                $("#" + input_id).css("height", 20);

                                var labelText = ""; // isnull($("label[for='" + input_id + "']").text(), "");

                                // config....basic checkbox
                                $("#" + input_id).kendoCheckBox({
                                    label: labelText,   //"Luggage compartment cover"
                                    change: KendoUtils.setEvent(input_id, "change"),
                                });
                            } else if (datainputtype.toUpperCase() == "textbox".toUpperCase()) {
                                // config....basic text
                                $("#" + input_id).kendoTextBox({
                                    //enable: false
                                    change: KendoUtils.setEvent(input_id, "change"),
                                });

                                var enterFunc = isnull($(there).attr("_data-enter-func"), "");
                                if (enterFunc != "") {
                                    $("#" + input_id).keyup(function (key) {
                                        if (key.keyCode == 13) {//키가 13이면 실행 (엔터는 13)
                                            var id = key.target.id;
                                            $("#" + input_id).focusout(); // focus out 될때  데이터소스와 연동됨...
                                            return (new Function(' return ' + enterFunc))();
                                        }
                                    });
                                }

                            } else if (datainputtype.toUpperCase() == "textareabox".toUpperCase()) {
                                // config....basic textarea 
                                var rows = isnull($(there).attr("rows"), 4);
                                var maxLength = isnull($(there).attr("maxLength"), 400);

                                var parent = $("#" + input_id).parent();
                                trace("parent: " + $(parent)[0].tagName + "/" + (parent).length);

                                if ($(parent)[0].tagName.toUpperCase() == "textarea".toUpperCase()) {
                                    // ok
                                } else {
                                    if ($(parent)[0].tagName.toUpperCase() == "TD".toUpperCase() && $(parent).length == 1) {
                                        var html = '<textarea id="' + input_id + '" name="' + input_id + '" style="width: 100%;" ' + required + '  ></textarea>';
                                        $(parent).html(html);
                                    } else {
                                        alert("textarea tag 사용할것..... " + input_id);
                                        return;
                                    }
                                }

                                $("#" + input_id).kendoTextArea({
                                    rows: rows,
                                    maxLength: maxLength,
                                    placeholder: "Enter your text here.",
                                    change: KendoUtils.setEvent(input_id, "change"),
                                });
                            }

                            var hidden = isnull($(there).attr("hidden"), "");
                            if (hidden == "hidden" || hidden) {
                                $("#" + input_id).closest("span").hide();  // kendo inpupt 상위에 span 도 hide 
                            }

                            var readonly = isnull($(there).attr("readonly"), "");
                            if (readonly == "readonly" || readonly) {
                                //var kendoui = $("#" + input_id).data(KendoUtils.getkendoType(datainputtype));
                                //kendoui.readonly(true);  // checkbox not working 
                            }

                            if (datainputtype.toUpperCase() != "editor".toUpperCase()) {
                                // viewMode & form  bind 
                                var value = KendoUtils.get(input_id);
                                fields[this.name] = value;
                                $("#" + input_id).attr("data-bind", "value: " + this.name);
                                //trace("fields[this.name] : " + this.name + " : " + value);
                            }

                        });
                    }
                }

                //var grid = $("#grid").data("kendoGrid");
                //grid.bind("beforeEdit", grid_beforeEdit);

                fields["_FORM_ORIGIN_DATA"] = $(that).serialize();

                __viewMode = kendo.observable(
                    fields,
                    /** {
                        firstName: "John",
                        lastName: "Doe",
                        displayGreeting: function () {
                            // Get the current values of "firstName" and "lastName"
                            var firstName = this.get("firstName");
                            this.set("lastName", "aaaa");
                            var lastName = this.get("lastName");
                            alert("Hello, " + firstName + " " + lastName + "!!!");
                        }
                    } **/
                );

                if (viewModel != "") {
                    $(that).attr("_data-view-model", viewModel);
                    kendo.bind($("#" + form_id), __viewMode);  // Bind the View to the View-Model  

                }

                // form >> button 
                $(that).find("button").each(function (index, item) {  // cancel save edit delete
                    if ($(item).attr("id") != undefined) {
                        var classNm = isnull($(item).attr("class"), "");
                        var icon = KendoUtils.getButtonIco(classNm);

                        //  config....basic button
                        $("#" + $(item).attr("id")).kendoButton({
                            icon: icon,
                            themeColor: "primary",
                            click: function (e) {

                            }
                        });
                    }
                });
            }

        });

        // title header buttons 
        KendoUtils.setHeadDiv();

        // Ctr + Q 조회 
        $(document).off("keyup");  // 기존 이벤트 제거 
        if (typeof (ctrQFunc) == "boolean" && ctrQFunc == false) {
            // dummy 
        } else {
            $(document).keyup(function (key) {
                if (key.ctrlKey && key.keyCode == 81) {  // Ctr + Q  
                    $(document).focusout();

                    if (isnull(ctrQFunc, "") == "") ctrQFunc = "TR.search()";
                    var funcNm = ctrQFunc.substr(0, ctrQFunc.indexOf("("));

                    try {
                        return (new Function(' return  typeof(' + funcNm + ') == "function" ? ' + ctrQFunc + ' : KendoUtils.noExistsFunc("' + funcNm + '")  '))();
                    } catch (err) {
                        trace(err.message);
                    }
                }
            });
        }

        return __viewMode;
    },

    noExistsFunc: function (funcNm) {
        alert("함수가 존재하지 않습니다. [" + funcNm + "]");
    }, 

    setHeadDiv: function () {
        // title header buttons 
        $("#pageHeadDiv button").each(function (index, item) {
            var btn = $("#" + $(item).attr("id")).data("kendoButton");

            if (btn == undefined) { // kendo button 생성 
                if ($(item).attr("id") != undefined) {
                    var classNm = isnull($(item).attr("class"), "");
                    var icon = KendoUtils.getButtonIco(classNm);
                    trace("kendo button undefined... ")
                    //  config....basic button
                    $("#" + $(item).attr("id")).kendoButton({
                        icon: icon,
                        themeColor: "primary",
                        click: function (e) {

                        }
                    });
                }
            } else { // button class 만 재지정 
                if ($(item).attr("id") != undefined) {
                    var classNm = isnull($(item).attr("class"), "");
                    var icon = KendoUtils.getButtonIco(classNm);
                    $(item).find(".k-icon").addClass("k-i-" + icon);
                    $(item).addClass("k-button-solid-primary");
                }
            }
        });
    }, 

    setEvent: function (id, ev) {
        var first = ev.substr(0, 1).toUpperCase();
        ev = first + ev.substr(1, ev.length - 1);
        //trace("event... " + ' return  typeof(' + id + '_on' + ev + ') == "function" ? ' + id + '_on' + ev + ' : null  ');
        return (new Function(' return  typeof(' + id + '_on' + ev + ') == "function" ? ' + id + '_on' + ev + ' : null  '))();
    },

    preventDefault: function (e) {
        trace("event ...... preventDefault")
        e.preventDefault();
    },

    setGrid2DataSource: function (grid, keyField, __grid_datasoruce) { // dataSource 직접 넘겨줘야됨.
        grid = grid.replace("#", "");
        var gridObj = KendoUtils.getKendoGrid(grid);
        var fields = {}; // dataSource fields    { "USE_YN": { type: "boolean", editable: "true" } };

        // 1010. grid column 의 _editortemplate (custom) 속성으로 dropDownList, checkbox 의 editor, template 속성 연결 
        var gridOptions = gridObj.options;
        var columns = gridObj.options.columns;
        var gridEditable = gridObj.options.editable;

        if (gridObj.options._checkCol) { // check column
            columns = [{
                title: "<input id='" + KendoUtils.checkAllCol + "', type='checkbox',  class='chkbx k-checkbox k-checkbox-md k-rounded-md', style='width:20px; height:20px;' />",
                width: "28px", attributes: { style: "text-align: center;" },
                template: "<input type=\"checkbox\"  class=\" " + KendoUtils.checkCol + " chkbx k-checkbox k-checkbox-md k-rounded-md\" style=\"width:20px; height:20px;\"  />",

                //selectable: true,
                //width: "24px",
                //attributes: { "class": "checkbox-align _checkAll", },
                //headerAttributes: { "class": "checkbox-align", }
            }
            ].concat(columns);
        }

        var rowNum = isnull(gridObj.options._rowNumCol, "");
        if ((typeof (rowNum) == "boolean" && rowNum == true) || (typeof (rowNum) == "string" && rowNum != "" && rowNum != "0px" && rowNum != "0")) {
            var width = "24px";
            if (typeof (rowNum) == "string") width = rowNum;
            columns = [{ title: "No.", width: width, attributes: { style: "text-align: center;" }, template: "<span class='row-number'></span>", editor: KendoUtils.nonEditor, },].concat(columns);
        }


        if (gridObj.options._checkCol || gridObj.options._rowNumCol) {
            gridObj.options.columns = null;
            gridObj.options.columns = columns;
        }

        if (columns.length > 0) {
            for (var i = 0; i < columns.length; i++) {
                var col = columns[i];
                var fieldNm = isnull(col.field, "").replace(/ /gi, "");

                if (!gridEditable) {
                    /////columns[i].model.editable = false;
                }

                var isRequired = false, editable = false;
                if (col.model != undefined) {
                    if (col.model.validation != undefined) { // 필수 
                        isRequired = isnull(col.model.validation.required, false);
                        if (isRequired) {
                            columns[i].title = "<span , class='text-danger'>" + "*" + col.title + "</span>";
                        }

                        delete columns[i].model.validation.required;
                        columns[i].model.validation.required_site = isRequired; // required...  mouse click... locking...
                    }

                    editable = isnull(col.model.editable, false);
                    if (editable == false) {
                        //trace("col.title : " + col.title)
                    }
                }


                if (col._editortemplate != undefined && typeof (col._editortemplate) == "object" && isnull(col._customSkip, false) != true) {
                    var _editortemplate = col._editortemplate;

                    if (_editortemplate.type != undefined && isnull(_editortemplate.type, "") != "") {

                        if (_editortemplate.type.toUpperCase() == "dropDownList".toUpperCase()) {  // editor & template
                            var datasource1 = (new Function('return ' + _editortemplate.datasourceStr))();
                            //trace("_editortemplate.datasourceStr " + fieldNm  + " / " + editable)
                            columns[i].editor = KendoUtils.ddlEditor
                            columns[i].template = KendoUtils.ddlTemplate(fieldNm, _editortemplate.datasourceStr);

                        } else if (_editortemplate.type.toUpperCase() == "multiSelector".toUpperCase()) {  // template
                            columns[i].editor = KendoUtils.multiSelectorEditor;
                            columns[i].template = KendoUtils.multiTemplate(fieldNm, _editortemplate.datasourceStr);

                        } else if (_editortemplate.type.toUpperCase() == "checkbox".toUpperCase()) {  // template
                            delete columns[i].field; // ### 체크박스의 경우 온전한 기능을 가진 template 이 없다. 여기서 필드명을 삭제하고 template 을 다시 설정해준다. 
                            columns[i].template = KendoUtils.checkTemplate(fieldNm);

                        } else if (_editortemplate.type.toUpperCase() == "date".toUpperCase()) {  // template
                            columns[i].format = _editortemplate.format;
                            ///columns[i].parseFormats = "yyyyMMdd";

                            columns[i].editor = KendoUtils.dateEditor;
                            //columns[i].template = '#= kendo.toString(' + fieldNm + ', "yyyyMMdd HH:mm:ss") #';

                            columns[i].model.type = "date"; // model type --> date 

                        } else if (_editortemplate.type.toUpperCase() == "number".toUpperCase()) {  // template
                            columns[i].format = _editortemplate.format;
                            columns[i].editor = KendoUtils.numericTextBox;
                            columns[i].model.type = "number"; // model type --> number

                        } else {

                        }
                    }
                }

                if (editable == false && columns[i].model != undefined) {
                    if (col._editortemplate != undefined && col._editortemplate.type.toUpperCase() == "dropDownList".toUpperCase()) {
                        // ddl --> ddl editor 에서 disable 
                    } else {
                        columns[i].editor = KendoUtils.nonEditor;   // grid column : editalbe false  /  datasource field : editable true 로 설정하겠다. 
                    }
                    columns[i].model.editable = true;    // dataSource filed == editable 
                    columns[i].model.editable2 = false; // grid column editable 로 사용하겠다. (내부처리용)
                } else if (editable == true && columns[i].model != undefined) {
                    columns[i].model.editable = true;  
                    columns[i].model.editable2 = true;
                }
            }

            fields["_ROW_TYPE"] = { type: "string", editable: "true", defaultValue: "I" }; // row type I/U/D 
            fields["PAGE_TOT_ROWS"] = { type: "number", editable: "true", defaultValue: "0" };

            // ddl, checkbox ... redraw : ####### imnportant 
            gridObj.options.columns = columns;
            gridObj.setOptions(gridObj.options);
        }

        // 2010. checkbox 의 경우 재처리 : important 
        var columns = gridObj.options.columns;
        if (columns.length > 0) {
            for (var i = 0; i < columns.length; i++) {
                var col = columns[i];

                if (col.template != undefined && typeof (col.template) == "string" && isnull(col._customSkip, false) != true) {
                    var template = isnull(col.template, "").replace(/ /gi, ""); // 공백제거 
                    if (col.template.indexOf("KendoUtils.dirtyField") >= 0 && col.template.indexOf("checkbox") >= 0) {
                        var fieldNm = template.replace('#=KendoUtils.dirtyField(data,"', '');
                        fieldNm = fieldNm.split('"')[0]
                        fields[fieldNm] = col.model;  // 1010 에서 field 속성을 삭제해서 dataSource와 매핑이 안되므로  template string 에서 field 명을 추출해서 dataSource model 에 반영한다.

                    }
                }
            }
        }

        // grid field to datasource model
        var columnsNames = "";
        var columns = gridObj.options.columns;
        if (columns.length > 0) {
            for (var i = 0; i < columns.length; i++) {
                var col = columns[i];
                if (col.field != undefined) {
                    if (col.model != undefined) {
                        if (col._editortemplate != undefined && col._editortemplate.type != undefined && col._editortemplate.type.indexOf("checkbox") >= 0) {
                            // 앞 단계에서 담았다.
                        } else {
                            fields[col.field] = col.model // important !!!
                        }

                        //////__grid_datasoruce.options.schema.model.fields[col.field] = col.model;  // 원래 기능이 존재하는듯...
                    }
                }
            }
        }

        // 3010. basic datasource for Grid 
        var pageSize = isnull(gridObj.options.pageable._pageSize, "All");
        if (__grid_datasoruce == null) {

            __grid_datasoruce = new kendo.data.DataSource({
                page: 1,                   //     반환할 페이지
                pageSize: pageSize,   //     반환할 항목 수
                skip: 0,                    //     건너뛸 항목 수
                take: pageSize,       //     반환할 항목 수 (pageSize와 같음)
                data: [],
                autoSync: false,        //  true 일 경우 sync 되어서 dirty == true 되지 않음... (주의)  default = false
                //////transport: {},
                schema: {
                    model: {
                        id: keyField,  // <-- id 필드   
                        //fields: {}
                        fields,  // <-- 1010/2010 에서 재처리된 fields  
                    },
                    //data: "data",
                    total: "PAGE_TOT_ROWS",
                },

                //change: function (e) {
                //    trace(e.action + "/" + e.field + "/" + e.sender)  // itemchange , add , remove , sync
                //    var data = this.data();
                //    console.log(data.length); // displays "77"
                //},

            });
        } else {

        }

        // 4010. grid pager 정보를 datasource 에  설정 
        __grid_datasoruce.options.serverPaging = gridObj.options.serverPaging;
        __grid_datasoruce.options.pageSize = pageSize; //isnull(gridObj.options.pageable._pageSize, 20);

        // 5010. grid dataSource 적용 
        gridObj.setDataSource(__grid_datasoruce);  // 화면에서 grid null 로 설정함. 
        ///////gridObj.dataSource.options = __grid_datasoruce; // important !!! 없으면 신규행추가시 cell locking... ==> #ERR100

        __grid_datasoruce._transport = {};
        //__grid_datasoruce.sync();

        return __grid_datasoruce;
    },

    gridComplete: function () {

        $(".k-grid").each(function (index, item) {  // kendo grid  
            var gridId = $(this).attr("id");

            var pageSize = $('#' + gridId).data('kendoGrid').options.pageable.pageSize;
            if (pageSize == 0) {
                $('#' + gridId).data('kendoGrid').options.pageable = false;
                $('#' + gridId).find(".k-pager-wrap").hide();
            }

            // grid event bind  --> grid input.chkbx 이벤트 안탐...  
            $("#" + gridId).bind("change", function (e) { // grid id 로만 이벤트 발생됨 ?
                var grid_id = $(e.target).closest("div[data-role='grid']").attr("id");
                var grid = $("#" + grid_id).data("kendoGrid");
                var id = $(e.target).attr("id");
                var type = $(e.target).attr("type");
                var fieldNm = $(e.target).attr("name");
                var uid = $(e.target).closest("tr").attr("data-uid");
                var classNm = $(e.target).attr("class");
                var ddl_role = $(e.target).context.dataset.role;

                //trace("classNm : " + type + " / " + classNm + " / " + id + " / " + ddl_role)

                if (type == "checkbox" && id == KendoUtils.checkAllCol) { // check all
                    KendoUtils.checkAll(e, grid_id);

                    // 현재 그리드에 체크된 행을 담아둔다. edit event --> databound --> disappeared
                    KendoUtils.storeCheckRows(grid_id, e.target);

                } else if (type == "checkbox") { // #CHECK_VLUE 와 동기화 
                    if (isnull(fieldNm, "") != "") {
                        dataItem = grid.dataItem($(e.target).closest("tr"));

                        if ($(e.target).is(":checked")) {
                            dataItem.set(fieldNm, KendoUtils.checkType);
                        } else {
                            dataItem.set(fieldNm, KendoUtils.uncheckType);
                        }
                    }

                    // 현재 그리드에 체크된 행을 담아둔다. edit event --> databound --> disappeared
                    KendoUtils.storeCheckRows(grid_id, e.target);

                } else if (type == "date") { // xxx
                    // KendoUtils.dateEditor
                } else if (ddl_role == "dropdownlist") { // dropdownlist

                } else if (ddl_role == "numerictextbox") { // numerictextbox

                }

                if (type == "checkbox") {
                    // dummy 
                } else if (ddl_role == "dropdownlist" || ddl_role == "numerictextbox") {
                    // ddlEditor >> input >> change event 에서 처리 
                } else {
                    KendoUtils.setRowNum2(gridId, e); // grid databound  (현재행의 번호가 없어짐..) >> here  (여기서 다시 )
                }
            });

            $("#" + gridId).bind("click", function (e) { // grid id 로만 이벤트 발생됨 ?
                var grid = $("#" + gridId).data("kendoGrid");
                var tag = isnull($(e.target)[0].tagName, "").toUpperCase();

                //trace("tag : " + tag + " / " + $(e.target).closest("th").attr("aria-sort") + " / " + grid.options.sortable)
                trace("tag : " + tag + " / " + e.shiftKey + " / " + e.ctrlKey)

                if (tag == "TD" && $(e.target).closest("tr").attr("role") == "row") {
                    var uid = $(e.target).closest("tr").attr("data-uid");
                    ////var rowIndex = $("#" + gridId).data("kendoGrid").tbody.find("tr[data-uid='" + uid + "']").find(".k-state-selected").prevObject[0].rowIndex;
                    ////var rowIndex = $(e.target).closest("tr").index();

                    KendoUtils.setCheckRow("ROW_CLICK", $(e.target).closest("tr"), true, true, e.shiftKey, e.ctrlKey); // 현재행 선택

                } else if (tag == "SPAN" && $(e.target).closest("th").attr("role") == "columnheader") { // sorting...
                    var sort = isnull($(e.target).closest("th").attr("aria-sort"), "");
                    var sortable = grid.options.sortable;
                    if (sortable) {
                        KendoUtils.setCheckRow("SORT", $(e.target).closest("tr"), true, true); // sorting...click 
                    }

                } else if (tag == "TBODY") { // 마우스로 행 드래그선택 
                    // 이때 tr 안넘어감, 첫행을 넘겨주겠다. 
                    KendoUtils.setCheckRow("FT", $(e.target).find("tr").eq(0), true, true, e.shiftKey, e.ctrlKey); // from ~ to 선택
                }
 
            });

            // pager select change event 
            $("#" + gridId + " .k-pager-sizes select").bind("change", function (e) {
                var grid = $('#' + gridId).data('kendoGrid');
                if (grid.dataSource.options.serverPaging) { // server paging

                    //trace("page >>> " + this.value + " / " + grid2.options.page + " / " + grid2.dataSource.options.pageSize);

                    if (isnull(this.value, "").toUpperCase() == "All".toUpperCase()) {
                        grid.dataSource._pageSize = 0;
                        grid.dataSource._take = 0;
                        grid.options.pageable._pageSize = this.value;
                    } else {
                        grid.dataSource._pageSize = this.value;
                        grid.dataSource._take = this.value;
                        grid.options.pageable._pageSize = this.value;
                    }
                    grid.trigger("page");
                }

            });

            // grid toolbar event bind 
            var toolbar = KendoUtils.getKendoGrid(gridId).options.toolbar;
            if (toolbar != undefined) {
                for (var i = 0; i < toolbar.length; i++) {
                    var classNm = isnull(toolbar[i].className, "");
                    var btnNm = isnull(toolbar[i].text, "");
                    var clickFunc = isnull(toolbar[i].clickFunc, "");  // k-icon k-i-save k-button-icon
                    var classNm2 = isnull(classNm, "").split(" ")[0];
                    //trace("classNm2 >>>>> " + classNm2)
                    if (classNm2.indexOf("k-grid-custom") >= 0) {
                        jQuery("#" + gridId).find("." + classNm2).bind('click', { "clickFunc": clickFunc }, function (e) {
                            //trace("btnNm...click..." + e.data.clickFunc);
                            //eval(e.data.clickFunc);
                            new Function('return ' + e.data.clickFunc)();
                        });
                    }
                    //trace("classNm2 : " + classNm2)
                    if (isnull(classNm2, "").trim() != "") {
                        var icon = KendoUtils.getButtonIco(classNm2);
                        $("#" + gridId).find("." + classNm2).prepend('<span class="k-icon k-i-' + icon + ' k-button-icon"></span>');
                    }

                }
            }

        });

    },

    getButtonIco: function (classNm) {
        var icon = "save";
        if (classNm.indexOf("search") >= 0) icon = "search";  // 조회   k-i-search
        if (classNm.indexOf("del") >= 0) icon = "delete";       // 삭제    k-i-delete
        if (classNm.indexOf("cancel") >= 0) icon = "cancel";  // 취소 
        if (classNm.indexOf("check") >= 0) icon = "check-circle";  // 선택하여 처리 
        if (classNm.indexOf("print") >= 0) icon = "print";              // 출력 
        if (classNm.indexOf("popup") >= 0) icon = "hyperlink-open"; // 팝업 
        if (classNm.indexOf("add") >= 0) icon = "plus";  // 신규추가 
        if (classNm.indexOf("excel") >= 0) icon = "excel"; 
        return icon;
    },

    setColumn4mcCombo: function (id, columns) {
        // mc combo columns 
        //var columns = [
        //                        { field: "TEXT", title: "성명", width: 100},
        //                        { field: "VALUE", title: "사번", width: 100},
        //                        { field: "ATT1", title: "직위", width: 100 },
        //                        { field: "ATT2", title: "전화번호", width: 140},
        //                      ];
        var mcCombo = $("#" + id).data("kendoMultiColumnComboBox");
        var options = mcCombo.options;
        options.columns = columns;
        mcCombo.setOptions(options);
        mcCombo.refresh();
    },

    checkAll: function (e, grid_id) {
        var that = $("#" + grid_id).find("input[id='" + KendoUtils.checkAllCol + "']");
        var $cb = that;
        var checked = $cb.is(':checked');

        var item = $("#" + grid_id).data("kendoGrid").table.find("tr").find("td ." + KendoUtils.checkCol);

        for (var i = 0; i < item.length; i++) {
            item[i].checked = checked;
        }
    },

    setCodeList: function (jsonCodeList) {  // _dsCodeList 으로 고정
        _dsCodeList = [];
        var codeList = isnull(jsonCodeList, "");  //'@Html.Raw(Json.Serialize(ViewBag.CodeList))';

        if (codeList == "" || codeList == null || codeList == "null") return;

        codeList = JSON.parse(codeList.substr(1, codeList.length - 2)); // "{   }"  <-- 따옴표 제거 
        var obj = Object.keys(codeList);
        for (key in Object.keys(obj)) {
            var field = obj[key];
            _dsCodeList[field] = codeList[field];  // codeList[field];
        }
    },

    _getFields: function (grid) {  // 참고용 소스 
        var gridObj = null;
        if (typeof (grid) == "string") {
            grid = "#" + grid.replace("#", "");
            gridObj = $(grid).data("kendoGrid");
        } else {
            gridObj = grid;
        }

        // 10. datasoruce model 
        var fields = gridObj.dataSource.options.schema.model.fields;
        var obj = Object.keys(fields);
        for (key in Object.keys(obj)) {
            var field = obj[key];
            if (typeof (field) == "string") {
                console.log(field + "/" + fields[field].type);
            }
        }

    },

    nonEditor: function (container, options) {
        container.text(options.model[options.field]);
    },

    numericTextBox: function (container, options) {
        var format, decimal;
        var gridId = container[0].id.replace("_active_cell", "").replace("_active_element", "");

        if (isnull(gridId, "") == "") { // treelist  <-- _active_element
            gridId = container.context.id;
        }

        if (gridId == "") return;

        var grid = KendoUtils.getKendoGrid("#" + gridId);
        var columns = grid.columns; // _editortemplate
        // container, options 외 function 으로 넘겨준 파라미터가 셀 클릭시점에 맞지가 않다. important !!! 
        // 다시 담는다. 
        for (var i = 0; i < columns.length; i++) {
            if (columns[i].field == options.field && columns[i]._editortemplate != undefined) {
                format = columns[i]._editortemplate.format;
                break;
            }
        }

        if (isnull(format, "") != "" && isnull(format, "").trim().split(".").length >= 2) {
            decimal = isnull(format, "").trim().split(".")[1].replace("}", "").length;
        }

        //trace("format : " + format + " / " + decimal);

        $('<input name="' + options.field + '"/>')
            .appendTo(container)
            .kendoNumericTextBox({
                format: isnull(format, "{0:n0}"),
                decimals: isnull(decimal, 0), //4
                spinners: false,
                change: function (e) {
                    trace("grid numericTextBox input ..... change... last event  ... important !!! ")
                    KendoUtils.setRowNum(gridId); // grid databound  (현재행의 번호가 없어짐..) >> here  (여기서 다시 )
                },
                close: function (e) {

                },
            });
    },

    dateEditor: function (container, options) {
        var format;
        var gridId = container[0].id.replace("_active_cell", "");
        if (gridId == "") return;
        var grid = KendoUtils.getKendoGrid("#" + gridId);
        var columns = grid.columns; // _editortemplate
        // container, options 외 function 으로 넘겨준 파라미터가 셀 클릭시점에 맞지가 않다. important !!! 
        // 다시 담는다. 
        for (var i = 0; i < columns.length; i++) {
            if (columns[i].field == options.field && columns[i]._editortemplate != undefined) {
                format = columns[i]._editortemplate.format;
                break;
            }
        }

        //trace("format " + format)
        $('<input ' + (isRequired ? 'required ' : '') + (disable ? 'readonly ' : '') + ' data-text-field="' + options.field + '" data-value-field="' + options.field
            + '" data-bind="value:' + options.field + '" />')
            .appendTo(container)
            .kendoDatePicker({
                format: isnull(format, "{0: yyyy-MM-dd}"),    //{0: yyyy-MM-dd HH:mm:ss}",
                parseFormats: ["yyyyMMdd"],
                value: new Date(options.model.dateTime),
                change: function (e) {
                    options.model[options.field] = e.sender._oldText; // date object --> string 
                }
            });
    },

    checkTemplate: function (fieldName) {   // #CHECK_VLUE 와 동기화 
        var checkOption = fieldName + ' == "' + KendoUtils.checkType + '"';
        if (KendoUtils.checkType == true) {
            checkOption = fieldName;
        }
        return '#=KendoUtils.dirtyField(data,"' + fieldName + '")#<input type="checkbox"  style="width:20px;height:20px;" name= "' + fieldName + '" #= ' + checkOption + ' ? \'checked="checked"\' : "" # class="chkbx k-checkbox k-checkbox-md k-rounded-md" />';
    },

    dirtyField: function (data, fieldNm) {
        var hasClass = $("[data-uid=" + data.uid + "]").find(".k-dirty-cell").length < 1;
        //trace("fieldNm : " + fieldNm)
        if (data.dirty && data.dirtyFields[fieldNm] && hasClass) {
            return "<span class='k-dirty'></span>"
        }
        else {
            return "";
        }
    },
 
    nonCheckEditor: function (container, options) {    //check 컬럼 editor : 체크박스 주위 셀클릭시  체크박스가 값으로 변경되지 못하게 처리함.
        var html = "";
        var disabled = "";
        var gridid = container[0].id.replace("_active_cell", "");   // container[0].cellIndex

        if (isnull(gridid, "") == "") { // treelist
            gridid = container.context.id;
        }

        var grid = KendoUtils.getKendoGrid(gridid);
        if (grid.options.columns[container[0].cellIndex].template.indexOf("disabled") >= 0) {
            disabled = "disabled";
        }

        trace("incellCheckEditor : " + gridid + " / " + options.field + " / " + options.model[options.field].toUpperCase() + " / " + disabled)

        if (options.model[options.field] != undefined) {
            if (typeof options.model[options.field] == "boolean") {
                if (options.model[options.field] == true) {
                    html = "<input type='checkbox' name='" + options.field + "' checked='checked' " + disabled + " style='width: 20px; height: 20px;' class='chkbx k-checkbox k-checkbox-md k-rounded-md' ></input>";
                } else {
                    html = "<input type='checkbox' name='" + options.field + "' " + disabled + " style='width: 20px; height: 20px;' class='chkbx k-checkbox k-checkbox-md k-rounded-md' ></input>";
                }
            } else {
                if (options.model[options.field].toUpperCase().indexOf("Y") >= 0 || options.model[options.field] == "1") {
                    html = "<input type='checkbox' name='" + options.field + "' checked='checked' " + disabled + " style='width: 20px; height: 20px;'  class='chkbx k-checkbox k-checkbox-md k-rounded-md' ></input>";
                } else {  
                    html = "<input type='checkbox' name='" + options.field + "' " + disabled + " style='width: 20px; height: 20px;' class='chkbx k-checkbox k-checkbox-md k-rounded-md' ></input>";
                }
            }

        } else {
            html = "<input type='checkbox' name='" + options.field + "' " + disabled + " style='width: 20px; height: 20px;' class='chkbx k-checkbox k-checkbox-md k-rounded-md'  ></input>";
        }

        //trace("html : " + html);

        container.html(html);
    },
 
    checkTemplate2 : function (field, type, disabled) { //체크컬럼  템플릿  
        if (isnull(disabled, "")) {
            disabled = "disabled";
        } else {
            disabled = "";
        }

        trace("checkTemplate2 : " + field + "/" + type)

        if (type == "1") {
            return "<input type='checkbox' name='" + field + "' #= typeof(" + field + ") != 'undefined' && " + field + "=='1' ? 'checked = checked' : '' # " + disabled + " style='width: 20px; height: 20px;' class='chkbx k-checkbox k-checkbox-md k-rounded-md' ></input>";
        }
        if (type == "Y") {
            return '<input type="checkbox" name="' + field + '" #=' + field + '=="Y" ? "checked=checked" : "" # ' + disabled + ' style="width: 20px; height: 20px;" class="chkbx k-checkbox k-checkbox-md k-rounded-md" ></input>';
        }
        if (type == "true") {
            return "<input type='checkbox' name='" + field + "' #= typeof(" + field + ") != 'undefined' && " + field + "=='true' ? 'checked=checked' : '' # " + disabled + " style='width: 20px; height: 20px;' class='chkbx k-checkbox k-checkbox-md k-rounded-md' ></input>";
        }

    }, 

    ddlEditor: function (container, options) {
        var dataSource, optionlabel, disable, isRequired;
        // ddl 컬럼이 여러개인 경우 dataSource 가 마지막 ddl 의 dataSource 로 무조건 넘어온다. 
        // 컬럼정보를 다시 찾아서 해당 컬럼의 dataSource 를 바인딩해준다. 
        var gridId = container[0].id.replace("_active_cell", "");
        if (gridId == "") return;
        var grid = KendoUtils.getKendoGrid("#" + gridId);
        var columns = grid.columns;
        // container, options 외 function 으로 넘겨준 파라미터가 셀 클릭시점에 맞지가 않다. important !!! 
        // 다시 담는다. 
        for (var i = 0; i < columns.length; i++) {
            //trace(columns[i].field + " / " + options.field)
            if (columns[i].field == options.field && columns[i]._editortemplate != undefined && columns[i]._editortemplate.datasourceStr != undefined) {
                dataSource = (new Function('return ' + columns[i]._editortemplate.datasourceStr))();
                //disable = (!columns[i].model.editable);
                disable = (!columns[i].model.editable2);
                isRequired = columns[i].model.required;
                //trace("ddlEditor : " + disable + " / " + isRequired  + " / " + columns[i].model.editable2 + " / " + columns[i].model.aaa);
                break;
            }
        }

        var rowIdx = $("#" + gridId).data("kendoGrid").tbody.find("tr[data-uid='" + options.model.uid + "']").find(".k-state-selected").prevObject[0].rowIndex;
        //trace("rowIdx >>>>>> " + rowIdx + " / " + disable)

        $('<input ' + (isRequired ? 'required ' : '') + (disable ? 'readonly ' : '') + ' name="' + options.field + '"  data-bind="value:' + options.field + '"  />')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: true,
                optionLabel: KendoUtils.ddlOptionLabel(optionlabel), //gf_optionlabel(optionlabel),
                dataTextField: KendoUtils.textFieldName,
                dataValueField: KendoUtils.valueFieldName,
                dataSource: dataSource,
                //valuePrimitive: true,  // important !!! bind data == null 일때... 셀을 처음 클릭하면 text가 표시되지 않는다. 이 옵션을 true 해줄것.
                change: function (e) {
                    options.model[options.field] = isnull(this.value(), "");

                    trace("grid ddl input ..... change... last event  ... important !!! ")
                    KendoUtils.setRowNum(gridId); // grid databound  (현재행의 번호가 없어짐..) >> here  (여기서 다시 )

                    var func = (new Function('return typeof(' + gridId + '_' + options.field + '_onChange) == "function" ? ' + gridId + '_' + options.field + '_onChange : "" '))();
                    if (func != "") {
                        var value = grid.dataSource.data()[rowIdx][options.field];
                        func(e, rowIdx, value);
                    }
                },
                close: function (e) {

                },
            });

        if (disable) {
            //trace("options.model[options.field] : " + options.model[options.field] + " / " + KendoUtils.getText4ddlDs(dataSource, options.model[options.field]))
            var text = KendoUtils.getText4ddlDs(dataSource, options.model[options.field]);
            container.text(isnull(text, ""));
        }
    },

    ddlOptionLabel: function (optionlabel) {
        if (optionlabel) {
            return { TEXT: "Please Select...", VALUE: null };
        } else {
            return false;
        }
    },

    ddlTemplate: function (fieldName, ddl_datasoruce) {
        //trace("ddlTemplate : " + fieldName + " / " + ddl_datasoruce )
        return "#=KendoUtils.ddlDisplayText(" + fieldName + ", " + ddl_datasoruce + ", '" + fieldName + "')#";
    },

    ddlDisplayText: function (ddlField, dsObj, fieldName) {
        //trace("ddlDisplayText : " + ddlField + " /  " + dsObj + " / " + fieldName)
        if (ddlField == undefined || ddlField == "") return "";
        if (dsObj == undefined) return "";

        if (ddlField.TEXT != undefined) {
            text = ddlField.TEXT;
            value = ddlField.VALUE;
            ddlField = value;
            return isnull(text, "");  // 신규행일때는 ddlField = object
        } else {
            //trace("fieldName : " + ddlField +  " /" + fieldName )
        }

        // 데이터 조회된 상태에서... ddlField == value 
        var rtnVal = "";
        var ddlDataSource = KendoUtils.getData4Ds(dsObj); //(dsObj._data == undefined ? dsObj : dsObj._data);  // kendo datasource bind 한 경우 dsObj._data
        for (var i = 0; i < ddlDataSource.length; i++) {
            //trace("ddlDisplayText 22 : " + ddlDataSource[i][KendoUtils.valueFieldName] + " /  " + ddlField)
            if (ddlDataSource[i][KendoUtils.valueFieldName] == ddlField) {
                rtnVal = ddlDataSource[i][KendoUtils.textFieldName];
                break;
            }
        }
        return isnull(rtnVal, "");
    },

    getData4Ds: function (dsObj) { // dataSource or json Obj 
        var data = (dsObj._data == undefined ? dsObj : dsObj._data);
        return data;
    },

    multiSelectorEditor: function (container, options) {
        // ddl 컬럼이 여러개인 경우 dataSource 가 마지막 ddl 의 dataSource 로 무조건 넘어온다. 
        // 컬럼정보를 다시 찾아서 해당 컬럼의 dataSource 를 바인딩해준다. 
        var gridId = container[0].id.replace("_active_cell", "");
        gridId = gridId.replace("_active_element", "");  // treelist

        if (gridId == "") return;
        var grid = KendoUtils.getKendoGrid("#" + gridId);
        var columns = grid.columns;
        var dataSource = null;
        for (var i = 0; i < columns.length; i++) {
            //trace(columns[i].field + " / " + options.field)
            if (columns[i].field == options.field && columns[i]._editortemplate != undefined && columns[i]._editortemplate.datasourceStr != undefined) {
                dataSource = (new Function('return ' + columns[i]._editortemplate.datasourceStr))();
                break;
            }
        }

        $("<select multiple='multiple' data-bind='value :" + options.field + "'/>")
            .appendTo(container)
            .kendoMultiSelect({
                dataTextField: KendoUtils.textFieldName,
                dataValueField: KendoUtils.valueFieldName,
                dataSource: dataSource,
                autoClose: false,
                change: function (e) {
                    options.model[options.field] = this.value();
                    trace("field 11 >>>> " + this.value() + "/"+ options.model[options.field] )
                },
                close: function (e) {

                },
            });
    },

    multiTemplate: function (fieldName, ddl_datasoruce) {
        return "#=KendoUtils.multiDisplayText(" + fieldName + ", " + ddl_datasoruce + ")#";
        //return "#= PARSING_TYPE.join(', ') #";  //"#= [" + isnull(fieldNm, '') == '' ? '' + "].join(', ') #"; // PARSING_TYPE   ["LC_42A","LC_FOS"]
    },

    multiDisplayText: function (ddlField, dsObj) {
        var fieldValue = ddlField; // '[LC_BASIC, LC_FOS]';
        var fieldText = "";
        var rtnVal = [];

        if (typeof (fieldValue) == "string") { // 조회시 
            if (isnull(fieldValue, "") == "") {
                rtnVal = "[]";
            } else {
                var arr = fieldValue.split(",");
                for (var i = 0; i < arr.length; i++) {
                    var value = arr[i].trim().replace("[", "").replace("]", "");
                    var value2 = value;

                    if (value.indexOf('"') == 0) { // 쌍따옴표 있을경우 ["AAA", "BBB"] or "AAA", "BBB"
                        // 
                    } else {
                        value2 = '"' + KendoUtils.getText4ddlDs(dsObj, value) + '"';
                    }
                    fieldValue = fieldValue.replace(value, value2).trim();
                }

                if (fieldValue.indexOf("[") >= 0 && fieldValue.indexOf("]") >= 0) {
                    rtnVal = fieldValue;
                } else {
                    rtnVal = '[' + fieldValue + ']';
                }
            }

            rtnVal = JSON.parse(rtnVal);

        } else { // ddl select...
            var fieldText = [];
            if (isnull(fieldValue, "") == "") {
                fieldValue = [];
            }
            var keys2 = Object.keys(fieldValue); //키를 가져옵니다. 이때, keys 는 반복가능한 객체가 됩니다.
            for (var xx = 0; xx < keys2.length; xx++) {
                var key2 = fieldValue[xx];
                var text = KendoUtils.getText4ddlDs(dsObj, key2);
                fieldText[xx] = text;
            }

            rtnVal = fieldText;
        }

        return rtnVal.join(', ');
    },

    valueFieldName: "VALUE",
    textFieldName: "TEXT",

    hasChanges: function (container) {
        container = container.replace("#", "'");

        var isChange = false;
        var role = isnull($("#" + container).attr("data-role"), "");
        var tagNm = "";

        if (role != "grid" && $("#" + container)[0] != undefined) {
            tagNm = isnull($("#" + container)[0].tagName, "").toUpperCase();
        }

        if (role == "grid") { // kendo grid
            var grid = container;

            try {
                isChange = $('#' + grid).data('kendoGrid').dataSource.hasChanges();
            } catch (error) {
                console.error(error);
                isChange = true;  // 신규행 --> 저장 --> 서비스단 오류처리 --> 다시 저장시 isNew not working.... --> 변경된 것으로 간주하겠다. 
            } finally {
                trace("isChange : " + isChange)
            }

            // _ROW_TYPE 으로 판단 : add row >> id field 값 입력시 isNew : true --> false 변경되어 버림.... 
            // _ROW_TYPE 초기값을 I 설정해서 신규입력을 판단함. 
            var data = $('#' + grid).data('kendoGrid').dataSource.view();
            for (i = 0; i < data.length; i++) {
                if (data[i]._ROW_TYPE == "I") {
                    /////data[i].isNew = true;
                    isChange = true;
                }
            }

            if (!isChange) {
                //// alert("변경된 데이터가 없습니다.");
                return false;
            }
            return true;

        } else if (tagNm == "FORM") {
            // form 
            var current = $("#" + container).serialize();
            var __viewModel = (new Function('return ' + $("#" + container).attr("_data-view-model")))();
            var before = __viewModel.get("_FORM_ORIGIN_DATA");

            //trace("current : " + current)
            //trace("before  :    " + before)

            if (current == before) {
                isChange = false;
            } else {
                isChange = true;
            }

            return isChange
        } else {
            alert("not defined....");
            return false;
        }
    },

    dataBoundPost: function (e) {
        var gridId = e.sender.element[0].id;
        var role = $("#" + gridId).attr("data-role");

        if (role == "grid" || role == "treelist") { // kendo grid
            var grid = $("#" + gridId).data(KendoUtils.getkendoType(role));

            // 행번호 
            if ((grid.options._rowNumCol != undefined && grid.options._rowNumCol == true) || (grid.options._checkCol != undefined && isnull(grid.options._checkCol, false) != false)) { // _checkCol true, false, null, 24px 
                trace("dataBoundPost...")
                KendoUtils.setRowNum(grid);
            }

            // 페이징 항목수 ddl 클릭시 refresh 
            //trace("grid.dataSource.serverPaging :" + grid.dataSource.options.serverPaging)
            if (grid.dataSource.options.serverPaging) {
                if (grid.dataSource.data() != undefined && grid.dataSource.data().length > 0 && grid.dataSource.data()[0].PAGE_TOT_ROWS != undefined) {
                    var totalrow = isnull(grid.dataSource.data()[0].PAGE_TOT_ROWS, 0);
                    grid.dataSource._total = parseInt(totalrow);
                    if (!(grid.pager == undefined || grid.pager == null)) {  // grid pageable:  false  --> skip
                        grid.pager.refresh();
                    }
                }
            }

        }
    },

    setDefault4New: function (e, jdata) { // 그리드 추가버튼 클릭시 초기값 설정
        var gridid = e.sender.element[0].id;  //e.sender._cellId.replace("_active_cell", "");
        var gridtype = "kendoGrid"; // <--- 확인할것.... // gf_getGridType_by_gridid(gridid);
        var grid = $("#" + gridid).data(gridtype);
        var newRow = 0;
        var isNew = KendoUtils.isNew(e.model);

        if ((isNew && !e.model.dirty) || isnull(grid.dataSource.view()[newRow]["_ROW_TYPE"], "") == "I") {
            for (var i = 0; i < Object.keys(jdata).length; i++) {
                var field = Object.keys(jdata)[i];
                var value = eval("jdata." + field);

                var d1 = $(e.container).find('input[name="' + field + '"]').data("kendoDropDownList");
                var d2 = $(e.container).closest("tr").find('input[name="' + field + '"]');

                if ($(d2).attr("type") == "checkbox") {
                    if (value == "Y") {
                        $(d2).prop("checked", true);
                    } else {
                        $(d2).prop("checked", false); 
                    }
                }

                var editable = grid.dataSource.options.schema.model.fields[field].editable;

                if (d1 == undefined) {
                    if (editable == false) {
                        //grid.dataSource.options.schema.model.fields[field].editable = true;
                    }

                    grid.dataSource._view[newRow][field] = value;  // disable column 을 위해 둘다 해준다.
                    //grid.dataSource.view()[newRow][field] = value;
                    //grid.dataSource.view().at(newRow).set(field, value);

                } else {
                    d1.value(value);
                }
            }
            grid.dataSource.view().at(newRow).set("_ROW_TYPE", "I");
            //grid.dataSource._view[newRow]["_ROW_TYPE"] = "I";
            ////grid.dataSource.sync();  // ---> 행 전체가 lock 걸림...

            KendoUtils.setRowNum(gridid);  // row number reset 
        }
    },

    setRowNum2: function (gridObj, e) {
        trace("setRowNum2... from grid change....");

        var grid = $("#" + gridObj).data("kendoGrid");
        var row = $("#" + gridObj).find("tbody>tr[data-uid='" + grid.checkRowsUID + "']");
        grid.select(row); // 선택 유지

        this.setRowNum(gridObj);
    },

    setRowNum: function (gridObj) { // grid_id or evnet e
        if (typeof (gridObj) == "string") {
            gridObj = gridObj.replace("#", "");
            gridObj = KendoUtils.getKendoGrid(gridObj);
        }

        trace("gridObj.options._checkCol : " + gridObj.options._rowNumCol)

        if (!(KendoUtils.isExistRowNum(gridObj) || KendoUtils.isExistCheckCol(gridObj))) {
            return; // 행번호 or 체크컬럼 없을때..
        }

        var size = gridObj.dataSource._pageSize;
        var page = isnull(gridObj.dataSource._page, 1);

        // trace("callback page..." + size + " / " + page);

        var checkedRow = isnull(gridObj.checkRows, []);
        var checkedRowUID = isnull(gridObj.checkRowsUID, []);

        var rows = gridObj.items();
        $(rows).each(function (index, item) {
            //var rn = $(item).index() + 1;

            var rn = index + 1 + (size * (page - 1));
            //var rn = index + 1 + (pageSize * (gridObj.dataSource.page() - 1));
            var uid = item.uid;

            var rowLabel = $(item).find(".row-number");
            $(rowLabel).html(rn);

            var dataItem = gridObj.dataSource.view()[index]

            var isNew = KendoUtils.isNew(dataItem);
            //trace("row... " + rn + " / " + isNew + " / " + typeof (dataItem.isNew) + " / " + " / " + dataItem.dirty + " / " + isnull(dataItem._ROW_TYPE, ""))

            if ((isNew && !dataItem.dirty) || isnull(dataItem._ROW_TYPE, "") == "I") {
                $(rowLabel).html(KendoUtils.setIcon(rn, true));
            } else {
                //trace("aaaa : " + rn)
                $(rowLabel).html(rn);
            }

            // check 유지 
            if (KendoUtils.isExistCheckCol(gridObj)) { // check 컬럼 있으면..
                var checked = false;
                for (var i = 0; i < checkedRow.length; i++) {
                    if (index == checkedRow[i]) {
                        checked = true;
                        break;
                    }
                }
                //$(item).find("input[data-role='checkbox']").prop("checked", checked);  // kendo check 
                $(item).find("._chkCol").prop("checked", checked); // html check 
                KendoUtils.setCheckRow("ROWNUM", $(item), checked);
            }

        });

    },

    storeCheckRows: function (gridId, checkbox) {
        var fieldNm = $(checkbox).attr("name");
        var classNm = $(checkbox).attr("class");
        var grid = $("#" + gridId).data("kendoGrid");
        var selector = KendoUtils.getCheckRowSelector(gridId);

        // 현재 그리드에 체크된 행을 담아둔다. edit event --> databound --> disappeared
        if (isnull(fieldNm, "") == "" && classNm.indexOf("k-checkbox") >= 0) { // _checkCol
            //var isCheckbox = (isnull($(checkbox).closest("td").attr("class"), "").indexOf("_checkAll") >= 0 ? true : false); // 행별 체크박스
            //var isCheckboxHeader = ($(checkbox).parent()[0].tagName.toUpperCase() == "TH" ? true : false);  // 전체선택 체크박스
            if (1 == 2) { // kendo check 사용시 
                grid.checkRows = [];
                $("#" + gridId).find(".k-grid-content>table>tbody").children(selector).each(function (index, item) {   // aria-checked
                    var checked = $(item).find("input[data-role='checkbox']").is(":checked");
                    if (checked) {
                        grid.checkRows.push(index);
                    }
                });
            }
            if (1 == 1) { // html check 사용시 
                if (grid.checkRows == undefined) grid.checkRows = [];
                if (grid.checkRowsUID == undefined) grid.checkRowsUID = [];
                ////////grid.checkRows = [];
                $("#" + gridId).find(".k-grid-content>table>tbody").children(selector).each(function (index, item) {   // aria-checked
                    var checked = $(item).find("._chkCol").is(":checked");
                    var uid = $(item).attr("data-uid");
                    if (checked) {
                        grid.checkRows.push(index);
                        grid.checkRowsUID.push(uid);
                    }
                    KendoUtils.setCheckRow("storeCheckRows", $(item), checked);
                });
            }
        }

        // trace("grid.checkRows : " + grid.checkRows)
    },

    setCheckRow: function (svid, tr, isSelect, withCheck, shiftKey, ctrlKey) {  // row check  &&  select
        //trace("setCheckRow svid : " + svid )
        ////if (svid == "ROWNUM") return;

        shiftKey = isnull(shiftKey, false);  // 행선택시 shift key 사용여부 
        ctrlKey = isnull(ctrlKey, false);  // 행선택시 ctrl key 사용여부
        var gridId = $(tr).closest(".k-grid").attr("id"); // grid or treelist 
        if (gridId == undefined) {
            trace("setCheckRow gridId == undefined : " + svid);
            return;
        }

        var grid = KendoUtils.getKendoGrid(gridId);
        if (!KendoUtils.isExistCheckCol(grid)) return; // check 컬럼 없으면 스킵 

        if (grid.checkRows == undefined) grid.checkRows = [];
        if (grid.checkRowsUID == undefined) grid.checkRowsUID = [];

        var selector = KendoUtils.getCheckRowSelector(gridId);


        if (svid == "ROW_CLICK") { // 행클릭시는 현재행 외 모두 제거
            $("#" + gridId).find(".k-grid-content>table>tbody").children(selector).each(function (index2, row2) {   // 모든행 선택 해제
                $(row2).find("._chkCol").prop("checked", false);
                $(tr).removeClass('k-state-selected');
                $(tr).attr("aria-selected", "false");
            });
        } else if (svid == "SORT") { // sorting 
            $("#" + gridId).find(".k-grid-content>table>tbody").children(selector).each(function (index2, row2) {   // 모든행 선택 해제
                $(row2).find("._chkCol").prop("checked", false);
                $(tr).removeClass('k-state-selected');
                $(tr).attr("aria-selected", "false");
            });

            // sorting... check clear.... 
        }

        // 현재행에 대한 처리 
        if (svid != "FT") {  // FT 는 무조건 첫행을 넘겨줌. 다음단계에서 처리함. 
            if (isSelect) {
                $(tr).addClass('k-state-selected');
                $(tr).attr("aria-selected", "true");

                if (withCheck) { // 선택행에 대해서 체크도... 
                    $(tr).find("._chkCol").prop("checked", isSelect);
                }
                // 현재 rowIndex 추가 
                //grid.checkRows.push(rowIndex);
            } else {
                $(tr).removeClass('k-state-selected');
                $(tr).attr("aria-selected", "false");

                if (withCheck) { // 선택행에 대해서 체크도... 
                    $(tr).find("._chkCol").prop("checked", isSelect);
                }
                // 현재 rowIndex 제거
                //grid.checkRows = grid.checkRows.splice($.inArray(rowIndex, grid.checkRows), 1);
            }
        }
 
        // ctrl, shift, 마우스드래그 사용시 
        if (shiftKey || ctrlKey || svid == "FT") { // row select 모두 체크로 
            $("#" + gridId).find(".k-grid-content>table>tbody").children(selector).each(function (index2, row2) {
                if ($(row2).attr("class").indexOf('k-state-selected') >= 0) {
                    $(row2).find("._chkCol").prop("checked", true);
                } else {
                    $(row2).find("._chkCol").prop("checked", false);
                }

            });
        }

        grid.checkRows = KendoUtils.getCheckRowIndex(gridId);
        grid.checkRowsUID = KendoUtils.getCheckRowUID(gridId);

        // 사용자 정의 함수 호출
        if (svid != "ROWNUM") {
            return (new Function(' return  typeof(' + gridId + '_check_post) == "function" ? ' + gridId + '_check_post("' + svid + '") : null  '))();
        }

    },

    getCheckRowSelector: function (gridId) {
        var datarole = $("#" + gridId).attr("data-role");
        var selector = "tr.k-master-row";
        if (datarole == "treelist") { //###CHECK_ROW_SELECTOR 동기화할것.
            selector = "tr";
        }

        return selector; 
    }, 

    removeCheck: function (gridId) {
        var selector = KendoUtils.getCheckRowSelector(gridId);

        $("#" + gridId).find(".k-grid-content>table>tbody").children(selector).each(function (index, row) {   // 모든행 선택 해제
            $(row).find("._chkCol").prop("checked", false);
            $(row).removeClass('k-state-selected');
            $(row).attr("aria-selected", "false");
        });

        var grid = KendoUtils.getKendoGrid(gridId);
        grid.checkRows = KendoUtils.getCheckRowIndex(gridId);
        grid.checkRowsUID = KendoUtils.getCheckRowUID(gridId);
    }, 

    getCheckRowIndex: function (gridId) { // 체크행 _checkCol 
        //var grid = KendoUtils.getKendoGrid(gridId);  //$("#" + gridId).data("kendoGrid");

        // $("#" + gridId).find(".k-grid-content>table>tbody").children("tr.k-master-row")  :  important !!! --> sub grid 지원하지 않는다.
        var checked = [];
        var selector = KendoUtils.getCheckRowSelector(gridId);

        if ($("#" + gridId).find(".k-grid-content>table>tbody").children(selector).find("._chkCol").length > 0) {  // 공통을 사용한 그리드
            $("#" + gridId).find(".k-grid-content>table>tbody").children(selector).each(function (index, row) { // jquery 화면에 보이는 순서이므로 view 기준임 important !!!
                var chk = $(row).find("._chkCol").is(":checked");
                if (chk) {
                    checked.push(index);
                }
            });
        } else {  // selectable 컬럼 사용한 경우로 간주 
            $("#" + gridId).find(".k-grid-content>table>tbody").children(selector).each(function (index, row) {
                var chk = $(row).find(".k-checkbox[data-role='checkbox']").is(":checked");
                if (chk) {
                    checked.push(index);
                }
            });
        }
        //trace("bbb : " + checked)
        return checked;
    },

    getCheckRowUID: function (gridId) { // 체크행 _checkCol
        var checked = [];
        var selector = KendoUtils.getCheckRowSelector(gridId);

        if ($("#" + gridId).find(".k-grid-content>table>tbody").children(selector).find("._chkCol").length > 0) {  // 공통을 사용한 그리드
            $("#" + gridId).find(".k-grid-content>table>tbody").children(selector).each(function (index, row) {
                var chk = $(row).find("._chkCol").is(":checked");
                var uid = $(row).attr("data-uid");
                if (chk) {
                    checked.push(uid);
                }
            });
        } else {  // selectable 컬럼 사용한 경우로 간주 
            $("#" + gridId).find(".k-grid-content>table>tbody").children(selector).each(function (index, row) {
                var chk = $(row).find(".k-checkbox[data-role='checkbox']").is(":checked");
                var uid = $(row).attr("data-uid");
                if (chk) {
                    checked.push(uid);
                }
            });
        }

        return checked;
    },

    getSelectRowIndex: function (gridId, allRows) { // 선택행 
        allRows = isnull(allRows, false);  // allRows  선택된 전체행 리턴   ex) [1,3,6] 

        var selectedRow = [];
        var grid = KendoUtils.getKendoGrid(gridId);
        var row = grid.select();
        var rowIndex = -1;

        if (row.length > 0) {
            selectedRow = grid.select();
            ////rowIndex = selectedRow[0].rowIndex;

            if (allRows) {
                return selectedRow;  // [1,3,6] 
            } else {
                return selectedRow[0].rowIndex;  // 선택행중 첫행 
            }

        } else {
            if (allRows) {
                return [];  // [1,3,6] 
            } else {
                return -1;  // 선택행중 첫행 
            }
        }

    },

    isExistRowNum: function (gridObj) { // row num 컬럼여부 
        if (typeof (gridObj) == "string") {
            gridObj = gridObj.replace("#", "");
            gridObj = KendoUtils.getKendoGrid(gridObj);
        }

        var isNum = false;
        if (gridObj.options._rowNumCol != undefined) {
            if (gridObj.options._rowNumCol == true) {
                isNum = true;
            } else if (typeof (gridObj.options._rowNumCol) == "string") {
                isNum = true;
            }
        }

        return isNum;
    },

    isExistCheckCol: function (gridObj) { // check 컬럼여부 
        if (typeof (gridObj) == "string") {
            gridObj = gridObj.replace("#", "");
            gridObj = KendoUtils.getKendoGrid(gridObj);
        }

        var isCheck = false;
        if (gridObj.options != undefined && gridObj.options._checkCol != undefined) {
            if (gridObj.options._checkCol == true) {
                isCheck = true;
            }
        }

        return isCheck;
    },

    setIcon: function (text, isnew, isreply) {
        var rtnVal = text;

        if (isreply == "Y" || isreply == true) {
            rtnVal = '<img src="/images/common/re.gif" border="0" />' + " " + rtnVal;
        }

        if (isnew == "Y" || isnew == true) {
            rtnVal = '<img src="/images/common/icon_new_01.png" border="0" />' + " " + rtnVal;
        }

        if (isreply == "Y" || isreply == true) {
            rtnVal = '&nbsp;&nbsp;' + rtnVal;
        }

        return rtnVal;
    },

    editable4New: function (that, e, field) {  // 신규입력시에만 셀 활성화
        var fieldName = e.container.find("input").attr("name");
        //alert( e.model.isNew() ); 
        if (fieldName == field && !e.model.isNew) {
            that.closeCell(); // prevent editing
        }
    },

    isNew: function (dataItem) { // 신규행여부 
        var isNew = false;
        if (typeof (dataItem.isNew) == "function") {
            isNew = dataItem.isNew();
        } else {
            isNew = dataItem.isNew;
        }

        return isNew;
    },

    getkendoType: function (datainputtype) {
        var type = "";
        if (datainputtype == "combobox") {
            type = "kendoComboBox";
        } else if (datainputtype == "datebox") {
            type = "kendoDatePicker";
        } else if (datainputtype == "numberbox") {
            type = "kendoNumericTextBox";
        } else if (datainputtype == "checkbox") {
            type = "kendoCheckBox";
        } else if (datainputtype == "textbox") {
            type = "kendoTextBox";
        } else if (datainputtype == "textareabox") {
            type = "kendoTextArea";
        } else if (datainputtype == "multicombobox") {
            type = "kendoMultiSelect";
        } else if (datainputtype == "mccombobox") {
            type = "kendoMultiColumnComboBox";

        } else if (datainputtype == "grid") {  // data-role 
            type = "kendoGrid";

        } else if (datainputtype == "treelist") { // data-role
            type = "kendoTreeList";

        } else {
            return "not defined....";
        }

        return type;
    },

    dataClear: function (containerId) {
        var role = $("#" + containerId).attr("data-role");
        var tagNm = $("#" + containerId)[0].tagName.toUpperCase();   //attr("data-role"); if ($(item)[0].tagName && $(item)[0].tagName.toUpperCase() == "FORM")

        if (role == "grid") {
            var gridObj = KendoUtils.getKendoGrid(containerId);
            gridObj.dataSource.data([]);

        } else if (tagNm == "FORM") {
            var forms = $("#" + containerId).find(KendoUtils.data_tag);
            $(forms).each(function (index, item) {
                var id = isnull($(item).attr("id"), "");
                if (id == "") return true; // continue 
                KendoUtils.set(id, "");
                //_FORM_ORIGIN_DATA
            });

            var __viewModel = (new Function('return ' + $("#" + containerId).attr("_data-view-model")))();
            __viewModel.set("_FORM_ORIGIN_DATA", $("#" + containerId).serialize());

        } else {
            alert("not defined.....");
        }

    },

    getValue4ddlDs: function (dsObj, text) {  // code list --> text  --> value
        text = isnull(text, "").trim();  // dsObj == [{}, {}... ]  <> dataSource 

        if (dsObj == undefined) return "";

        var codeList = KendoUtils.getData4Ds(dsObj);  //dsObj; // _dsCodeList[codeDs];
        var keys2 = Object.keys(codeList); //키를 가져옵니다. 이때, keys 는 반복가능한 객체가 됩니다.

        if (keys2 == undefined) return "";

        for (var xx = 0; xx < keys2.length; xx++) {
            var key2 = keys2[xx];
            //trace("getValue4ddlDs: " + text + " / " + key2  )
            if (codeList[key2].TEXT == text) {
                return isnull(codeList[key2].VALUE, "");
            }
        }
    },

    getText4ddlDs: function (dsObj, value, fieldNm) {  // code list --> value --> text
        value = isnull(value, "").trim();   // dsObj == [{}, {}... ]  <> dataSource 
        fieldNm = isnull(fieldNm, "").trim();

        if (dsObj == undefined) return "";

        var codeList = KendoUtils.getData4Ds(dsObj); //dsObj; // _dsCodeList[codeDs];
        var keys2 = Object.keys(codeList); //키를 가져옵니다. 이때, keys 는 반복가능한 객체가 됩니다.

        if (keys2 == undefined) return "";

        for (var xx = 0; xx < keys2.length; xx++) {
            var key2 = keys2[xx];
            if (codeList[key2].VALUE == value) {
                if (fieldNm == "") { // TEXT 
                    return isnull(codeList[key2].TEXT, "");
                } else { // 특정필드값 
                    return isnull(codeList[key2][fieldNm], "");
                }
            }
        }
    },

    getValueCheckedOne: function (gridId, key) {  // grid checked only one 
        // 1건 선택 체크 
        var rtnValue = "";
        var tr = $("#" + gridId + " tbody tr");
        var cnt = 0, isbreak = false;

        var role = $("#" + gridId).attr("data-role");
        var grid = $("#" + gridId).data(KendoUtils.getkendoType(role));

        $(tr).each(function (index) { // Important !!! reverse 
            var row = $(this);
            //var chk = $(row).find("td._checkAll").find("input").is(":checked");
            var chk = $(row).find("._chkCol").is(":checked");

            if (chk) {
                var dataItem = grid.dataItem(row);
                rtnValue = dataItem[key];

                if (cnt > 0) {
                    alert("처리할 데이터 1건을 선택하십시오.");
                    isbreak = true;
                    return false; // break 
                }

                cnt++;
            }
        });

        if (cnt == 0) {
            alert("처리할 데이터 1건을 선택하십시오.");
            return "";
        }

        if (isbreak) {
            return "";
        } else {
            //trace("rtnValue : " + rtnValue)
            return rtnValue;
        }
    },

    multiItemDisp: function (gridId, field, maxLength, delimiter) {
        delimiter = isnull(delimiter, ",");
        maxLength = isnull(maxLength, 10);
        var role = $("#" + gridId).attr("data-role");
        var grid = $("#" + gridId).data(KendoUtils.getkendoType(role));
        var fiedArr = [];
        if (typeof (field) == "string") {
            fiedArr[0] = field;
        } else {
            fiedArr = field;
        }

        var chgCnt = 0;
        for (var i = 0; i < grid.dataSource.data().length; i++) {

            for (var kk = 0; kk < fiedArr.length; kk++) {
                var items = isnull(grid.dataSource.data()[i][fiedArr[kk]], "");
                var itemsArr = items.split(",");
                if (itemsArr.length > maxLength) {
                    items = "";
                    for (var x = 0; x < maxLength; x++) {
                        items += isnull(itemsArr[x], "").trim() + (x == maxLength - 1 ? "" : ", ");
                    }
                    //trace("items : " + items)
                    grid.dataSource.data()[i].ANAL_ITEMS_NAME = items + " ...";
                    chgCnt++;
                }
            }
        }

        if (chgCnt > 0) {
            grid.dataSource.sync();
        }
    },

    setComboFilter41Lvl: function (currentId, lowerId, baseField, operator) { // lowerId baseField value = currentId value 인 코드만...
        operator = isnull(operator, "eq");  // eq neq startswith
        var currentValue = KendoUtils.get(currentId);
        var dsId = $("#" + lowerId).attr("_data-code-ds");
        var inputtype = $("#" + lowerId).attr("_data-input-type");
        var composite = isnull($("#" + lowerId).attr("_composite"), "").toLowerCase();

        // combo, multiselect
        var ddl = $("#" + lowerId).data(KendoUtils.getkendoType(inputtype));
        if (ddl == undefined) return; // 초기화시 컴포넌트 생성 순서에 따라 ddl 컴포넌트가 생성이 안될 경우가 있음. 

        if (composite == "msmc") {
            //trace("currentValue : " + currentValue)
            var lowerId2 = lowerId + "_mcc";
            var inputtype2 = $("#" + lowerId2).attr("_data-input-type");
            var dsId2 = $("#" + lowerId2).attr("_data-code-ds");

            var ddl2 = $("#" + lowerId2).data(KendoUtils.getkendoType(inputtype2));
            if (ddl2 == undefined) return; // 초기화시 컴포넌트 생성 순서에 따라 ddl 컴포넌트가 생성이 안될 경우가 있음.

            var dataSource2 = new kendo.data.DataSource({
                data: _dsCodeList[dsId2],
                filter: (isnull(currentValue, "") == "" ? "" : { field: baseField, operator: operator, value: currentValue })  // 일치
            });

            ddl2.setDataSource(dataSource2);
            ddl2.dataSource.read();

            KendoUtils.set(lowerId2, "");
            KendoUtils.set(lowerId, "");  // 둘다 clear 
        } else {
            var dataSource = new kendo.data.DataSource({
                data: _dsCodeList[dsId],
                filter: (isnull(currentValue, "") == "" ? "" : { field: baseField, operator: operator, value: currentValue })  // 일치
            });

            ddl.setDataSource(dataSource);
            ddl.dataSource.read();

            KendoUtils.set(lowerId, "");
        }

    },

    resizeOneGrid: function (spilitter01, form1, form2) {
        var cwH = $(".content-wrapper").height();
        var chH = $("#pageHeadDiv").height();
        var footH = isnull($(".border-top.footer.text-muted").height(), 0);
        var form1H = isnull($("#" + form1).height(), 0);
        var form2H = isnull($("#" + form2).height(), 0);

        //trace("left : " + cwH + " /  " + footH + " /  " + chH + " / " + form1H);

        // 10. splitter 
        var splitter1 = $("#" + spilitter01).data("kendoSplitter");
        var splitter1H = cwH - form1H - form2H - chH - footH - KendoUtils.topHeadH - KendoUtils.footerH;

        var popup = $("div[data-role='window']", parent.document);

        //trace("popup : " + $("#modalWindow").css('display') + " / " + $(popup).css('display'))

        if ($(popup).find("iframe").length > 0) { // 팝업화면
            if ($(popup).height() == 0) {
                trace("popup height .... 0 ")
            }

            var popupH = $(popup).height();
            var popupW = $(popup).width();

            splitter1 = $("#" + spilitter01).data("kendoSplitter");
            splitter1H = popupH - 46 - form1H - form2H;  // pager 46
            splitter1.wrapper.width(popupW - 14);
        }

        splitter1.wrapper.height(splitter1H);
        splitter1.resize();

        // treelist --> treelist resizing... 
        var gridId = $("#" + spilitter01).find("[role='group']").attr("id");
        var role = $("#" + gridId).attr("data-role");
        if (role == "treelist" && $(popup).find("iframe").length == 0) {
            var grid = $("#" + gridId).data(KendoUtils.getkendoType(role));  // kendoGrid, kendoTreeList
            grid.options.height = splitter1H;  // treelist ... 
        }

    },

    resize2GridH: function (spilitter01, rightPane, form1) {
        var cwH = $(".content-wrapper").height();
        var chH = $("#pageHeadDiv").height();
        var footH = isnull($(".border-top.footer.text-muted").height(), 0);
        var form1H = isnull($("#" + form1).height(), 0);

        // 10. splitter 
        var splitter1 = $("#" + spilitter01).data("kendoSplitter");
        var splitter1H = cwH - form1H - chH - footH - KendoUtils.topHeadH - KendoUtils.footerH;

        var gridId = $("#" + rightPane).find("div").attr("id");
        var role = $("#" + gridId).attr("data-role");
        var grid = $("#" + gridId).data(KendoUtils.getkendoType(role));
        if (role == "treelist") {
            grid.options.height = splitter1H;  // treelist ... 
        } else {
            $("#" + gridId).height(splitter1H);
        }

        splitter1.wrapper.height(splitter1H);
        splitter1.resize(); 
    },

    resizeLMD: function (spilitter01, spilitter02, form1, form2, bottomPane) {  // list master detail
        var cwH = $(".content-wrapper").height();
        var chH = $("#pageHeadDiv").height();
        var footH = $(".border-top.footer.text-muted").height();
        var form1H = isnull($("#" + form1).height(), 0);
        var form2H = isnull($("#" + form2).height(), 0);

        //trace("left : " + cwH + " /  " + footH + " /  " + chH + " / " + form1H);

        // 1010. splitter1 
        var splitter1 = $("#" + spilitter01).data("kendoSplitter");
        var splitter1H = cwH - form1H - chH - footH - KendoUtils.topHeadH - KendoUtils.footerH;

        splitter1.wrapper.height(splitter1H);
        splitter1.resize();

        // 1020. pager bar not existis....
        var pagerH = 0;
        var pager = $("#" + spilitter01).find(".k-pager-wrap.k-grid-pager");
        if (pager.length == 0) {
            pagerH = 36;
            var grid = $("#" + spilitter01).find("[role='group']");
            var dataArea = $(grid).find(".k-grid-content");

            //trace("dataArea.height() >>>> " + dataArea.height() + " / " + ($(dataArea).height() + pagerH) + "/" + grid.attr("id"))
            $(grid).height($(grid).height() + pagerH);

            $(dataArea).height($(dataArea).height() + pagerH);
        }

        // 2010. splitter2 
        var splitter2 = $("#" + spilitter02).data("kendoSplitter");
        splitter2.wrapper.height(splitter1H);
        splitter2.resize();

        var dataArea = $("#" + bottomPane).find(".k-grid-content");
        var pager = $("#" + spilitter02).find(".k-pager-wrap.k-grid-pager");
        if (pager.length == 0) {
            //pagerH = 36;
            //var grid = $("#" + spilitter02).find("[role='group']");
            //var dataArea = $(grid).find(".k-grid-content");
        }

        var grid = $("#" + bottomPane).find(":first");
        $(grid).height($("#" + bottomPane).height() - 2);
        var dataArea = $(grid).find(".k-grid-content");
        $(dataArea).height($("#" + bottomPane).height() - 100);  // 100??
    },

    topHeadH: 60,
    footerH: 30,

    isCheckCol: function (e) {  // grid change event 
        var idx = isnull(e.sender._current[0].className, "").indexOf("_checkAll"); // old..version
        var idx2 = isnull(e.sender._current[0].className, "").indexOf("checkbox-align k-header k-state-focused");// old..version
        var idx3 = isnull(e.sender._current[0].className, "").indexOf("_chkCol");
        //var idx4 = isnull(e.sender._current[0].className, "").indexOf("#_chkAll"); <-- 차후 확인할것.

        if (idx >= 0 || idx2 >= 0 || idx3 >= 0) {
            return true;
        }
        return false;
    },

    MSMC_multiSelector_onOpen: function (e) {  // ### MSMC (multi-selector & multi-column) ###  >> multiSelector Open envent
        var id = e.sender.element[0].id;   //"ANAL_ITEMS";
        trace("MSMC multi-selector onOpen  : " + id);

        // mcc display : toggle
        var mccSpan = $("#" + id + "_mcc").closest("span");
        if ($(mccSpan).css('display') === 'none') {

            $("#" + id + "_mcc" + "-list").find(".k-table-th").css("text-align", "center"); // columns > attribute not working
            $("#" + id + "_mcc" + "-list").find(".k-table-td").css("text-align", "center");

            // mcc list 에 표시 checked..
            var multiSel = $("#" + id).data("kendoMultiSelect");
            //var arr = isnull(multiSel.value(), "").split(",");
            //for (var k = 0; k < arr.length; k++) {
            //    var value = isnull(arr[k], "").trim();
            //    if (value != "") {
            //        // 
            //    }
            //}
            //trace("multiSel : " + multiSel.value())


            $(mccSpan).show();
            $(mccSpan).css('backgroundColor', '#FEF9E7');
            setTimeout(function () {
                $("input[name='" + id + "_mcc_input']").filter(':visible').focus(); // mcc focus-in
            }, 200);
        } else {
            $(mccSpan).hide();
        }

        e.preventDefault(); // prevent multi-selector list 
    },

    MSMC_multiColumn_onClose: function (e) {
        trace("MSMC_multiColumn_onClose....")
        //////e.preventDefault(); // prevent close... 
    },

    MSMC_Close: function (thatButton) {
        $(thatButton).closest(".k-animation-container").hide();
    },

    MSMC_sortable: function (input_id) {
        var multiSelector = $("#" + input_id).data("kendoMultiSelect");
        var dataSource = multiSelector.options.dataSource;
        var dsCode = null;
        if (dataSource.options == undefined) { // json 
            dsCode = dataSource;
        } else { // kendo dataSource 
            dsCode = dataSource.options.data;
        }

        $("#" + input_id + "_taglist").sortable({
            opacity: 0.8,
            revert: true,
            //axis: 'x',
            placeholder: "sortable-placeholder",
            forceHelperSize: true,
            forcePlaceholderSize: true,
            //tolerance: 'pointer',
            items: "> span",   // important !!!

            start: function (event, ui) {
                var mccSpan = $("#" + input_id + "_mcc").closest("span");  // 드래그시 mcc 숨김
                $(mccSpan).hide();
            },
            stop: function (event, ui) {
                var multi = [];
                $("#" + input_id + "_taglist").find("span .k-chip-label").each(function (index, item) {
                    var html = $(item).html();
                    multi.push(KendoUtils.getValue4ddlDs(dsCode, html))
                    //trace("html >>>>>> " + html)
                });
                KendoUtils.set(input_id, multi);
            }
        });
        $("#" + input_id + "_taglist").disableSelection();
    },

    MSMC_sortable2: function (input_id) {  // mcc list 
        $("#" + input_id + "_listbox").sortable({
            opacity: 0.8,
            revert: true,
            axis: 'y',
            placeholder: "sortable-placeholder",
            forceHelperSize: true,
            forcePlaceholderSize: true,
            //tolerance: 'pointer',
            items: "> li",   // important !!!

            start: function (event, ui) {
                // 
            },
            stop: function (event, ui) {
                // k-selected   k-focus   data-offset-index
                $(this).find("li").each(function (index, item) {
                    $(item).removeClass("k-selected");
                    $(item).removeClass("k-focus");
                    $(item).attr("data-offset-index", index);
                    $(item).attr("aria-selected", false);
                });
            }
        });
        $("#" + input_id + "_listbox").disableSelection();

        //trace("input_id : " + input_id)
        //$("#" + input_id + "_listbox").find("li").bind("onclick", function (e) {  //  
        //    trace(999999999)

        //});

        $("#" + input_id + "_listbox").on("click", "li", function () {
            console.log($(this).text());
        });

    },

    MSMC_focusout: function (input_id) {
        $("input[name='" + input_id + "_input']").bind("focusout", function (e) {  // composite mcc 선택 후 hide
            // mcc focus-out --> mcc hidden & text clear 
            var mcc = $("#" + input_id).data("kendoMultiColumnComboBox");
            $("input[name='" + input_id + "_input']").val("");
            $("#" + input_id).closest("span").hide();

            $("#" + input_id + "-list").closest(".k-animation-container").hide();  // mcc table 숨김... 

        });
    },

    MSMC_Checked: function (thatCheck) {
        //var checked = $(thatCheck).is(':checked'); 
        var mccId = $(thatCheck).closest("ul").attr("id").replace("_listbox", "");  // ANAL_ITEMS_mcc_listbox -->  // ANAL_ITEMS_mcc
        var id = mccId.replace("_mcc", "");  // ANAL_ITEMS 

        //var mcc = $("#" + id + "_mcc").data("kendoMultiColumnComboBox");  // ANAL_ITEMS_mcc-list  k-table th  k-table-th
        //var dataSource = mcc.options.dataSource;

        var multi = [];
        $(thatCheck).closest("ul").find("li").each(function (index, item) {
            var chk = $(item).find("span").find(".msmc_check").is(':checked');

            if (chk) {
                var value = $(item).find("span").html();
                //multi.push(KendoUtils.getValue4ddlDs(dataSource.options.data, text));  // _dsCodeList[dsNm]
                multi.push(value);
                trace("chk : " + id + " / " + chk + " / " + value)
            }
        });

        KendoUtils.set(id, multi); // multi-selector 값 새로 반영   
    },

    MSMC_multiColumn_onChange: function (e) { // ### MSMC (multi-selector & multi-column) ###  multi-selector & multi-column >> multiColumn change envent
        var index = e.sender.selectedIndex == undefined ? -1 : e.sender.selectedIndex;
        if (index < 0) return;
        var id = e.sender.element[0].id.replace("_mcc", "");  //"ANAL_ITEMS";

        trace("MSMC_multiColumn_onChange  : " + id);

        /*****  MSMC_Checked 에서 처리한다.   **/
        var mcc = $("#" + id + "_mcc").data("kendoMultiColumnComboBox");  // ANAL_ITEMS_mcc-list  k-table th  k-table-th
        var dataSource = mcc.options.dataSource;
        if (dataSource.options == undefined) { // json
            dsCode = dataSource;
        } else { // kendo dataSource
            dsCode = dataSource.options.data;
        }

        var value = KendoUtils.get(id + "_mcc");  // 현재 선택한 값  value (O) 
        var multi = [];
        $("#" + id + "_taglist").find("span .k-chip-label").each(function (index, item) { // 기존 선택된 값 
            multi.push(KendoUtils.getValue4ddlDs(dataSource.options.data, $(item).html()));  // _dsCodeList[dsNm]
        });
        multi.push(value);  // 현재선택된값 반영   ["A", "B, "C"]
        KendoUtils.set(id, multi); // multi-selector 값 새로 반영 
        /*****/

        $("input[name='" + id + "_mcc_input']").select(); // mcc 검색항목 text 전체선택 

        //msmc_check  e.sender.selectedIndex     ANAL_ITEMS_mcc_listbox ul >>  il > data-offset-index = 2
        //var li = $("#" + id + "_mcc" + "-list").find(".k-table-body").find("li[data-offset-index='" + index + "']");
        //var checked = $(li).find("span").find(".msmc_check").is(':checked');
        //trace("index : " + index + " / " + checked)
        //trace("MSMC_multiColumn_onChange  : " + id);

    },

};

// 사용자 메뉴  
var UserMenu = {
    getMenu: function (menuId, menuJsonStr) {
        // 사용자별 메뉴 
        if (isnull(menuJsonStr, "") == "") { // 10. 로컬스토리지 null 일 경우 
            var dataSource = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "/Common/GetUserMenu",
                        dataType: "json",
                    }
                },
                schema: {
                    model: {
                        id: "MENU_CD",
                        fields: {
                            MENU_CD: { field: "MENU_CD" },
                            UP_MENU_CD: { field: "UP_MENU_CD" },
                            MENU_NM: { field: "MENU_NM" },
                            LINK_URL: { field: "LINK_URL" },
                            LVL: { field: "LVL" },
                            DIR_YN: { field: "DIR_YN" },
                        },
                    }
                }
            });

            dataSource.fetch(function () {
                var menuJson = dataSource.view();
                trace("menuJson length1 : " + menuJson.length);

                UserMenu.setMenu(menuId, UserMenu.setRouteData(menuJson));
            });

        } else { // 20. 로컬스토리지 != null 일 경우 
            var menuJson = JSON.parse(menuJsonStr)
            trace("menuJson length2 : " + menuJson.length);

            // menuJson : 쿼리에서 하위 메뉴가 존재하는것만 가져온다. 
            UserMenu.setMenu(menuId, UserMenu.setRouteData(menuJson));
        }
 
    },

    setRouteData: function (menuJson) {
        if (isnull(_ROUTE_DATA, "") != "") {
            var routeData = JSON.parse(_ROUTE_DATA);
            var obj = Object.keys(routeData);

            for (var k = 0; k < menuJson.length; k++) {
                var linkUrl = isnull(menuJson[k].LINK_URL, "");
                if (linkUrl != "") {
                    for (key in Object.keys(obj)) {
                        var field = obj[key];
                        var value = isnull(routeData[field], "");
                        if (value == "") continue;

                        if (linkUrl.indexOf("?" < 0)) { // 연구노트에서 넘어온 파라미터를 메뉴 Url 에 추가해준다. 
                            linkUrl += "?" + field + "=" + routeData[field];
                        } else {
                            linkUrl += "&" + field + "=" + routeData[field];
                        }
                    }
                    //trace("final link : " + field + " / " + routeData[field] + " / " + linkUrl);
                    menuJson[k].LINK_URL = linkUrl;
                }
            }
        }

        return menuJson;
    }, 

    setMenu: function (menuId, viewData) {
        $("#" + menuId).kendoMenu({
            animation: {
                open: { effects: "fadeIn" },
                duration: 500
            },
            openOnClick: true,
        });

        var menu = $("#" + menuId).data("kendoMenu");
        var menuData = [];

        
 
        // json to JSONTree
        var map = {};
        for (var i = 0; i < viewData.length; i++) {

            var obj = {
                //"id": viewData[i]['MENU_CD'], "text": viewData[i]['MENU_NM'], "url": viewData[i]['LINK_URL'], "LVL": viewData[i]['LVL'], "imageAttr": { alt: 'Image', height: '25px', width: '25px' }, "imageUrl": "https://demos.telerik.com/kendo-ui/content/shared/icons/sports/swimming.png" };
                "id": viewData[i]['MENU_CD'], "text": viewData[i]['MENU_NM'], "url": viewData[i]['LINK_URL'], "LVL": viewData[i]['LVL'] };
            var objOrign = obj;

            obj.items = [];
            map[obj.id] = obj;
            var parent = viewData[i]['UP_MENU_CD'];

            if (!map[parent]) {
                map[parent] = {
                    items: []
                };
            }

            map[parent].items.push(obj);
        }

        // JSONTree 에서 1 level 만 추출,  하위까지 다 포함되어 있음. 
        $.each(map, function (index, value) {
            if (value.LVL == "1") {
                menuData.push(value);
            }
        });
 
        menu.append(menuData);
 
        // JSONTree 생성시 sub item 없을 경우도 [] 로 생성되어서 화살표가 생긴다. 여기서 제거 
        $("#" + menuId).find(".k-menu-item").find("a").find(".k-menu-expand-arrow").each(function (index, item) {
            $(item).text(""); // 화살표 제거 
        });

    },
};

// 사이트 종속된 함수 
var DSELN = {

    setMultiColumnOptions: function (inputId, costId, dataSource) { // mcc column 정의 
        var composite = isnull($("#" + inputId).attr("_composite"), "").toLowerCase();
        inputId = (composite == "msmc" ? inputId + "_mcc" : inputId);

        var mcCombo = $("#" + inputId).data("kendoMultiColumnComboBox");
        var options = mcCombo.options;
        options.dataTextField = "TEXT";
        options.dataValueField = "VALUE";
        //options.autoBind = false;

        var columns = [];

        if (costId == "ANAL_ITEMS") {  // 분석항목
            columns = [
                { field: "VALUE", title: "ID", width: 100, attributes: { style: "text-align: center;" }, },
                { field: "ATT3", title: "분석항목ID", width: 160, headerAttributes: { style: "text-align: center;" }, attributes: { style: "text-align: left;" }, },
                { field: "TEXT", title: "분석항목설명", width: 200, attributes: { style: "text-align: left;" }, },
                { field: "ATT2", title: "Retention Time", width: 120, attributes: { style: "text-align: center;" }, },
                { field: "ATT1_NM", title: "부서", width: 100, attributes: { style: "text-align: center;" }, },
                { field: "ATT1", title: "부서코드", width: 80, attributes: { style: "text-align: center;" }, },  // attribute , hidden not working...
                //{
                //    field: "DUMMY", title: "", width: 100,
                //    template: '<input type="checkbox" class="msmc_check" style="width:20px;height:20px;"></input>',
                //    headerTemplate: '<button type="button" onclick="KendoUtils.MSMC_Close(this)"><span class="k-icon k-i-x"></span></button>', 
                //},
            ];
        } else if (costId == "ANAL_ATTR" || costId == "SMPL_ATTR" || costId == "SMPL_ANAL_ATTR") {  // 속성
            columns = [
                { field: "VALUE", title: "ID", width: "100px", attributes: { style: "text-align: center;" }, },
                { field: "ATT4", title: "속성ID", width: 120, attributes: { style: "text-align: center;" }, },
                { field: "TEXT", title: "속성명", width: 120, attributes: { style: "text-align: center;" }, },
                { field: "ATT3", title: "필드유형", width: 80, attributes: { style: "text-align: center;" }, },
                { field: "ATT1_NM", title: "부서", width: 100, attributes: { style: "text-align: center;" }, },
                { field: "ATT1", title: "부서코드", width: 80, attributes: { style: "text-align: center;" }, },  // attribute , hidden not working...
                //{
                //    field: "DUMMY", title: "", width: 100,
                //    template: '<input type="checkbox" class="msmc_check" style="width:20px;height:20px;"></input>',
                //    headerTemplate: '<button type="button" onclick="KendoUtils.MSMC_Close(this)"><span class="k-icon k-i-x"></span></button>',
                //},
            ];
        } else {
            columns = [
                { field: "VALUE", title: "VALUE", width: "100px", attributes: { style: "text-align: center;" }, },
                { field: "TEXT", title: "TEXT", width: 160, attributes: { style: "text-align: center;" }, },
            ];
        }

        if (composite == "msmc1111") {
            columns[columns.length] = {
                field: "DUMMY", title: "", width: 100,
                template: '<input type="checkbox" class="msmc_check" style="width:20px;height:20px;" onclick="KendoUtils.MSMC_Checked(this)"></input>',
                headerTemplate: '<button type="button" onclick="KendoUtils.MSMC_Close(this)"><span class="k-icon k-i-x"></span></button>',
            };
        }

        options.columns = columns;

        //// options.dataSource =  {type: "json", transport: { read: "/Common/GetCodeHelp?CONST=USER" },};  // when not remote
        mcCombo.setOptions(options);
        ////mcCombo.dataSource.read();  // 10. read   // when not remote
        ////mcCombo.setDataSource(options.dataSource); // 20. setDataSource  // when not remote

        //itemDataSource.data(ddlDs);

    },

};



// 기타 함수 
function traceKeyValue(fields) {
    var keys = Object.keys(fields); //키를 가져옵니다. 이때, keys 는 반복가능한 객체가 됩니다.
    for (var i = 0; i < keys.length; i++) {
        var key = keys[i];
        trace("key : " + key + ", value : " + fields[key])
    }
};

function replaceAll(str, searchStr, replaceStr) {

    return str.split(searchStr).join(replaceStr);
};

function gfn_getDay(svid, dateid) {
    svid = isnull(svid, "");   // data-format
    var today = "";
    var logindate = isnull(SESSION_LOGIN_DATE, "");  // 세션정보

    if (logindate == "") {

    } else {
        var logindate = logindate.replace(/[^0-9]/g, "");
        logindate = logindate.substr(0, 8);
    }

    if (isnull(logindate, "") != "") {
        today = logindate.substr(0, 4) + "-" + logindate.substr(4, 2) + "-" + logindate.substr(6, 2); //"2016-11-01" ;   // loginDate....서버에서 받아서 처리할 것... 
    } else {// 로컬 PC 날짜..
        var tmpdate = new Date();
        today = gf_format(tmpdate, "yyyy-MM-dd");
    }

    //alert("today:"+today);
    var format = "yyyyMMdd";
    if ($("#" + dateid).length > 0) {
        format = $("#" + dateid).attr("data-format");
        if (format == undefined) format = "yyyy-MM-dd";
    }
    //trace("today >>>> " + logindate + " / " + today)

    var mydate = new Date(today);

    switch (svid) {
        case "ymd":
        case "":
            // 오늘날짜...
            break;
        case "ymfd": //today --> 해당월 1일 
            mydate = new Date(mydate.getFullYear(), mydate.getMonth(), 1);
            break;
        case "ymld": //today --> 해당월 마지악일 
            mydate = new Date(mydate.getFullYear(), mydate.getMonth() + 1, 0);
            break;
        default:
            break;
    }

    var isAdd = false;
    var adddays = 0;
    if (svid.indexOf("d+") >= 0) {
        adddays = Number(svid.substr(svid.indexOf("d+") + 2, 10));
        isAdd = true;
        mydate.addDays(adddays);
    }

    if (svid.indexOf("d-") >= 0) {
        adddays = (-1) * Number(svid.substr(svid.indexOf("d-") + 2, 10));  // alert( adddays ); 
        isAdd = true;
        mydate.addDays(adddays);
    }

    return gf_format(mydate, format);
}

// date to string 
gf_format = function date2str(x, y) {
    var z = {
        M: x.getMonth() + 1,
        d: x.getDate(),
        h: x.getHours(),
        m: x.getMinutes(),
        s: x.getSeconds()
    };


    y = y.replace(/(M+|d+|h+|m+|s+)/g, function (v) {
        return ((v.length > 1 ? "0" : "") + eval('z.' + v.slice(-1))).slice(-2)
    });

    return y.replace(/(y+)/g, function (v) {
        return x.getFullYear().toString().slice(-v.length)
    });
}

// add days 
Date.prototype.addDays = function (days) {
    this.setDate(this.getDate() + parseInt(days));
    return this;
};
