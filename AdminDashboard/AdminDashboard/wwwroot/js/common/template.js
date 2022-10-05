let tmplData = [];
var Alphabets = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];

class Formula {
    constructor(sample_id, row, column, formula, attr_name) {
        this.sample_id = sample_id;
        this.row = row;
        this.column = column;
        this.formula = formula;
        this.attr_name = attr_name;
    }
}
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
var shouldMakeFormulas = true;
var rowData = [{ "sample_id": 2676, "seq_line": 1.0, "vial": 1.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2690, "seq_line": 2.0, "vial": 2.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2704, "seq_line": 3.0, "vial": 3.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2718, "seq_line": 4.0, "vial": 4.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2732, "seq_line": 5.0, "vial": 5.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2746, "seq_line": 6.0, "vial": 6.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2760, "seq_line": 7.0, "vial": 7.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2774, "seq_line": 8.0, "vial": 8.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2788, "seq_line": 9.0, "vial": 9.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2802, "seq_line": 10.0, "vial": 10.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2816, "seq_line": 11.0, "vial": 11.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2830, "seq_line": 12.0, "vial": 12.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2844, "seq_line": 13.0, "vial": 13.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2858, "seq_line": 14.0, "vial": 14.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2872, "seq_line": 15.0, "vial": 15.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2886, "seq_line": 16.0, "vial": 16.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2900, "seq_line": 17.0, "vial": 17.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2914, "seq_line": 18.0, "vial": 18.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2928, "seq_line": 19.0, "vial": 19.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2942, "seq_line": 20.0, "vial": 20.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2956, "seq_line": 21.0, "vial": 21.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2970, "seq_line": 22.0, "vial": 22.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2984, "seq_line": 23.0, "vial": 23.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 2998, "seq_line": 24.0, "vial": 24.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 3012, "seq_line": 25.0, "vial": 25.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 3026, "seq_line": 26.0, "vial": 26.0, "sample_name": "FL", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }, { "sample_id": 3040, "seq_line": 27.0, "vial": 100.0, "sample_name": "DW WASH", "ph": 0.0, "od": 0.0, "MK6": 0.0, "MK7": 0.0, "ret_time": 0.0, "evaporation": 0.0, "mk6_percentage": 0.0, "od_average": 0.0, "ph_average": 0.0, "mk7_average": 0.0, "mk6_mk7_percentage": 0.0 }];
function setSheetHeader() {
    var s = $("#spreadsheet").data("kendoSpreadsheet");
    //origin_headers = [];
    for (var i = 0; i < headers.length; i++) {
        console.log(i + ' : ' + headers[i]);
        //origin_headers.push(s.activeSheet().range(0, i).value());
        s.activeSheet().range(0, i).value(headers[i]);
    }
}
function exportSpreadsheet() {
    var spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
    spreadsheet.saveAsExcel();
}
function ToJsonTest() {
    var spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
    var data = spreadsheet.toJSON();

    console.log(JSON.stringify(data));
    console.log(data);

    var dataURL = new kendo.ooxml.Workbook(JSON.parse(JSON.stringify(data))).toDataURL();

    kendo.saveAs({
        dataURI: dataURL,
        fileName: "Test.xlsx"
    });
}
function findGetParameter(parameterName) {
    var result = null,
        tmp = [];
    location.search
        .substr(1)
        .split("&")
        .forEach(function (item) {
            tmp = item.split("=");
            if (tmp[0] === parameterName) result = decodeURIComponent(tmp[1]);
        });
    return result;
}
function getListFromSpreadsheet() {
    var sampleList = [];
    spreadsheet = $('#spreadsheet').data('kendoSpreadsheet');
    //if (spreadsheet.toJSON().sheets[0] == undefined) sleep(1000);
    spreadsheet.toJSON().sheets[0].rows.forEach(row => {
        var sample = {};
        for (var i = 0; i < row.cells.length; i++) {
            var attr_name = origin_headers[i];
            sample[attr_name] = row.cells[i].value;
        }
        if (sample['sample_id'] != undefined && typeof sample['sample_id'] == 'number')
            sampleList.push(sample);
        console.log(typeof sample['sample_id']);
    });
    return sampleList;
}
function setSpreadsheetSize() {
    var s = $('#spreadsheet').data('kendoSpreadsheet');
    var sampleList = getListFromSpreadsheet();
    s.activeSheet().resize(sampleList.length + 1, headers.length);
    s.activeSheet().range('A2:' + Alphabets[headers.length] + sampleList.length + 1).background('');
    s.activeSheet().range('A2:' + Alphabets[headers.length] + sampleList.length + 1).borderTop({ size: 1, color: "black" });
    s.activeSheet().range('A2:' + Alphabets[headers.length] + sampleList.length + 1).borderLeft({ size: 1, color: "black" });
    s.activeSheet().range('A2:' + Alphabets[headers.length] + sampleList.length + 1).borderRight({ size: 1, color: "black" });
    s.activeSheet().range('A2:' + Alphabets[headers.length] + sampleList.length + 1).borderBottom({ size: 1, color: "black" });
    s.activeSheet().range('A2:' + Alphabets[headers.length] + sampleList.length + 1).textAlign('center');
    s.activeSheet().range('A1:' + Alphabets[headers.length] + sampleList.length + 1).verticalAlign('center');
    s.activeSheet().range('A1:' + Alphabets[headers.length] + '1').enable(false);
    s.activeSheet().range('A1:C' + sampleList.length + 1).enable(false);
}
function syncSpreadsheetToDataSource() {
    spreadsheet = $('#spreadsheet').data('kendoSpreadsheet');
    var diff = isSameHeader();
    if (diff != '') {
        alert('Some Header Rows are changed : ' + diff + "restored to original data");
        setTmplDataSource();
        setSheetHeader();
        restoreFormula();
        setSpreadsheetSize();
    }
    alert('엑셀파일을 적용하였으며, 확인 후 저장하시기 바랍니다');
    var sampleList = getListFromSpreadsheet();
    data = spreadsheet.options.sheets[0].dataSource.data();
    for (var i = 0; i < data.length; i++) {
        //            console.log(i, data[i]['MK6']);
        var samples = sampleList.filter(s => data[i]['sample_id'] == s['sample_id']);
        //            console.log(data[i]['sample_id'], samples.length);
        if (samples.length > 0) {
            var sample = samples[0];
            origin_headers.forEach(c => data[i][c] = sample[c]);
            console.log(data[i]['MK6'], sample['MK6']);
        }
    }

    for (var i = 0; i < data.length; i++) {
        if (data[i]['sample_id'] != undefined && typeof data[i]['sample_id'] == 'number')
            data[i].dirty = true;
        else
            data[i].dirty = false;
    }
    $("#btnCancelEdit, #btnSaveTemplate").toggleClass("k-state-disabled", false);
}
function onChange(arg) {
    $("#btnCancelEdit, #btnSaveTemplate").toggleClass("k-state-disabled", false);
    console.log("Spreadsheet change. New value: " + arg.range.value());
    spreadsheet = $('#spreadsheet').data('kendoSpreadsheet');
    data = spreadsheet.options.sheets[0].dataSource.data();
    //syncSpreadsheetToDataSource();
    //data[arg.range._ref.topLeft.row - 1].dirty = true;
}

var ALLOWED_EXTENSIONS = [".xlsx"];

let excelData = {};
let spreadsheet;
let data;
function saveFormula() {
    formulas = [];
    spreadsheet = $('#spreadsheet').data('kendoSpreadsheet');
    data = spreadsheet.options.sheets[0].dataSource.data();
    excelData = JSON.parse(JSON.stringify(spreadsheet.toJSON().sheets[0].rows));

    for (let i = 0; i < excelData.length; i++) {
        for (let j = 0; j < excelData[i].cells.length; j++) {
            //console.log(i, j, excelData[i].cells[j].formula);
            if (excelData[i].cells[j].formula != undefined) {
                // sample_id, row, column, formula, attr_name
                formulas.push(new Formula(excelData[i].cells[0].value, i, j, excelData[i].cells[j].formula, origin_headers[j]));
                console.log(excelData[i].cells[0].value, i, j, excelData[i].cells[j].formula);
            }
        }
    }
}
function restoreFormula() {
    spreadsheet = $('#spreadsheet').data('kendoSpreadsheet');
    formulas.forEach(formula => {
        console.log(formula.row, formula.column, formula.formula);
        spreadsheet.activeSheet().range(getCellIdByIndex(formula.row, formula.column)).formula(formula.formula);
    });
    setDirtyFlag(false);
}
function setDirtyFlag(flag) {
    var spreadsheet = $('#spreadsheet').data('kendoSpreadsheet');
    var data = spreadsheet.options.sheets[0].dataSource.data();
    data.forEach(row => {
        row.dirty = flag;
    });
}
function getCellIdByIndex(r, c) {
    console.log(r, c, Alphabets[c] + r);
    return Alphabets[c] + (r + 1);
}
function isSameHeader() {
    var spreadsheet = $('#spreadsheet').data('kendoSpreadsheet');
    var sheet = spreadsheet.activeSheet();
    var isSame = true;
    var different = "";
    for (var i = 0; i < headers.length; i++) {
        if (headers[i] != sheet.range(Alphabets[i] + 1).value()) {
            console.log(i, headers[i], sheet.range(Alphabets[i] + 1).value());
            different += i + " : " + headers[i] + " : " + sheet.range(Alphabets[i] + 1).value() + "\n";
            isSame = false;
        }
    }
    return different;
}
function getExcelFileFromServer() {
    $.ajax(
        {
            type: 'POST',
            dataType: 'text',
            url: '/Analysis/GetExcelFile',
            data: { fileName: uploadFileName },
            success:
                function (response) {
                    var spread = $("#spreadsheet").data('kendoSpreadsheet');
                    spread.fromFile(b64ToBlob(response));

                    setTimeout(() => syncSpreadsheetToDataSource(), 2000);
                },
            error:
                function (response) {
                    alert("Error: " + response);
                }
        });
}
function b64ToBlob(data) {
    var byteString = atob(data.split(',')[1]);
    var ab = new ArrayBuffer(byteString.length);
    var ia = new Uint8Array(ab);

    for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }

    return new Blob([ab], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
}
var uploadFileName = "";
function setTmplDataSource(e) {
    console.log('ANAL_ID = ' + ANAL_ID);
    $.ajax({
        url: "/Analysis/GetAnalTmpl?ANAL_ID=" + ANAL_ID,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log(result);
            //e.success(result);
            tmplData = result.result0;
            var spreadsheet = $('#spreadsheet').data('kendoSpreadsheet');
            spreadsheet.options.sheets[0].dataSource.data(tmplData);
            spreadsheet.options.sheets[0].dataSource.options.data = tmplData;
            spreadsheet.options.sheets[0].dataSource.transport.data = tmplData;
        }
    });
}


var dataSource = new kendo.data.DataSource({
    transport: {
        parameterMap: function (options, operation) {
            if (operation !== "read" && options.models) {
                return { models: kendo.stringify(options.models) };
            }
        },
        //submit: onSubmit,
        read: function (e) {
            var data = [];
            setTmplDataSource();
        },
        update: function (e) {
            console.log('update : ' + e.data.id);
            // On success.
            e.success();
            // On failure.
            // e.error("XHR response", "status code", "error message");
        },
        save: function (e) {
            console.log('update : ' + e.data.id);
            // On success.
            // detailDataSource.sync();
        },
        saveChanges: function (e) {
            if (!confirm("Are you sure you want to save all changes?")) {
                e.preventDefault();
            }
        }
    },
    batch: true,
    change: function () {
        $("#btnCancelEdit, #btnSaveTemplate").toggleClass("k-state-disabled", !this.hasChanges());
    },
    schema: {
        model: {
            id: "sample_id",
            fields: schemaModelFields
        },
        data: function (response) {
            return response.result0;
        }
    }
});

$(function () {
    $('#btnExcelExport').kendoButton({
        themeColor: "primary",
        click: exportSpreadsheet
    });
    $("#tmplFile").kendoUpload({
        async: {
            saveUrl: "/Analysis/UploadTemplate",
            autoUpload: true
        },
        multiple: false,
        localization: {
            "select": "Select file to import..."
        },
        select: function (e) {
            var extension = e.files[0].extension.toLowerCase();
            uploadFileName = e.files[0].name;
            if (ALLOWED_EXTENSIONS.indexOf(extension) == -1) {
                alert("Please, select a supported file format");
                e.preventDefault();
            }
        },
        success: function (e) {
            getExcelFileFromServer();
            // Load the converted document into the spreadsheet
            //var spread = $("#spreadsheet").data('kendoSpreadsheet');
            //spread.fromFile(b64ToBlob(e.XMLHttpRequest.responseText));
            //spread.fromJSON(e.response);
        },
        error: function (e) {
            //var spread = $("#spreadsheet").data('kendoSpreadsheet');
            //spread.fromFile(b64ToBlob(e.XMLHttpRequest.responseText));
            //e.preventDefault();
            //var err = $.parseJSON(e.XMLHttpRequest.responseText);

            //alert(err.Message);

            //$.map(e.files, function (file) {
            //    alert("Could not upload " + file.name);
            //});
        }
    });


    $("#spreadsheet").kendoSpreadsheet({
        columns: 20,
        rows: 100,
        toolbar: { home: false, insert: false, data: false },
        sheetsbar: false,
        "render": onRender,
        dataBinding: function (e) {
            console.log('Data is about to be bound to sheet "' + e.sheet.name() + '".');
        },
        dataBound: function (e) {
            console.log('Data has been bound to sheet "' + e.sheet.name() + '".');
            //setSheetHeader();
            restoreFormula();
            setSpreadsheetSize();
            spreadsheet = $("#spreadsheet").data("kendoSpreadsheet");
            var sheet = spreadsheet.activeSheet();
            sheet.frozenRows(1);
            //sheet.resize(rows, cols);
        },
        change: onChange,
        sheets: [
            {
                name: "ExperimentAnalysis",
                dataSource: dataSource,
                rows: [
                    {
                        height: 30,
                        cells: [
                            {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white",
                                width: 50
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white",
                                width: 50
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white",
                                width: 50
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white"
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white"
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white"
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white"
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white"
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white"
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white"
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white"
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white"
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white"
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white"
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white"
                            }, {
                                bold: "true",
                                background: "#9c27b0",
                                textAlign: "center",
                                color: "white"
                            }
                        ]
                    }
                ],
                columns: [
                    { width: 80 },
                    { width: 50 },
                    { width: 50 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 },
                    { width: 100 }
                ]
            }
        ],
        //excelExport: function (e) {
        //    // Prevent the default behavior which will prompt the user to save the generated file.
        //    e.preventDefault();
        //    e.workbook.fileName = EXCEL_FILE_NAME;
        // Get the Excel file as a data URL.
        //var dataURL = new kendo.ooxml.Workbook(e.workbook).toDataURL();

        //// Strip the data URL prologue.
        //var base64 = dataURL.split(";base64,")[1];
        //var contentType = dataURL.split(";base64,")[0];
        //var expNo = findGetParameter('EXP_NO');
        //var analId = findGetParameter('ANAL_ID');

        //// Post the base64 encoded content to the server which can save it.
        //$.post("ExpInsert", {
        //    fileName: EXCEL_FILE_NAME,
        //    base64: base64,
        //    contentType: contentType,
        //    expNo: expNo,
        //    analId: analId
        //}).done(function (msg) {
        //    alert(msg);
        //});
        //},
        excelExport: function (e) {
            e.workbook.fileName = EXCEL_FILE_NAME;
        }
        //,
        //excel: {
        //    proxyURL: '/Analysis/ExpInsert',
        //    forceProxy: true
        //}
    });

    function onRender(e) {
        // do custom height calculations to determine desired height
        sheetResize();
        // var height = window.innerHeight;
        // e.sender.element.innerHeight(height-20);
    }
    function getUpdated() {
        var spreadsheet = $('#spreadsheet').data('kendoSpreadsheet');
        var data = spreadsheet.options.sheets[0].dataSource.data();
        var updated = [];
        data.forEach(row => {
            if (row.dirty) updated.push(row);
        });
        return updated;
    }
    function sheetResize() {
        var height = window.innerHeight
            - 185                                             // border of the Spreadsheet Div.
            ;
        var spreadsheet = $('body').find($("div[data-role='spreadsheet']"));
        spreadsheet.css({ height: height });
        spreadsheet.data("kendoSpreadsheet").resize();
    };
    $(window).resize(sheetResize);
    $('#btnSaveTemplate').kendoButton().click(function (event) {
        saveFormula();
        var updated = getUpdated();
        if (updated.length == 0) {
            console.log('Updated data size : ' + updated.length);
            console.log('returning');
            return "";
        }
        var data = updated.filter(obj => {
            return !(obj['sample_id'] == undefined || obj['sample_id'] == 0 || obj['sample_id'] == null);
        });
        console.log(data);
        console.log(JSON.stringify(data));
        //$.post("/Analysis/UpdateAnalTmpl", JSON.stringify(data), function(data, status){
        //        alert("Data: " + data + "\nStatus: " + status);
        //});
        $.ajax({
            url: "/Analysis/UpdateAnalTmpl",
            type: "POST",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                updated.forEach(row => row.dirty = false);
                console.log(result);
                restoreFormula();
                setSpreadsheetSize();
                $.ajax({
                    url: "/Analysis/UpdateAnalTmplFormula",
                    type: "POST",
                    data: JSON.stringify(formulas),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        console.log("Formula Saved : " + result);
                        alert("저장하였습니다");
                    },
                    error: function (xhr, httpStatusMessage, customErrorMessage) {
                        alert(xhr.responseText);
                    }
                });

            },
            error: function (xhr, httpStatusMessage, customErrorMessage) {
                alert(xhr.responseText);
            }
        });
        event.preventDefault();

    });

    function onRead(options) {
        $.ajax({
            url: "/Analysis/GetAnalTmpl?ANAL_ID=" + ANAL_ID,
            //data: "result0",
            success: function (result) {
                options.success(result);
            },
            error: function (result) {
                options.error(result);
            }
        });
    }

    $("#btnExpInsert").kendoButton().click(function () {
        var expNo = findGetParameter('EXP_NO');
        if (expNo == null || expNo == '') {
            alert('연구노트 번호가 없습니다.');
            return;
        }
        var s = $('#spreadsheet').data('kendoSpreadsheet');
        if (confirm('연구노트에 추가하시겠습니까?')) {
            //s.saveAsExcel();
            var dataURL = new kendo.ooxml.Workbook(s.activeSheet).toDataURL();

            // Strip the data URL prologue.
            var base64 = dataURL.split(";base64,")[1];
            var contentType = dataURL.split(";base64,")[0];
            var expNo = findGetParameter('EXP_NO');
            var analId = findGetParameter('ANAL_ID');

            // Post the base64 encoded content to the server which can save it.
            $.post("ExpInsert",
                {
                    fileName: EXCEL_FILE_NAME,
                    base64: base64,
                    contentType: contentType,
                    expNo: expNo,
                    analId: analId
                }).done(function (msg) {
                    alert(msg);
                });
        }
    });

    $("#btnCancelEdit").kendoButton().click(function () {
        if (!$(this).hasClass("k-state-disabled")) {
            dataSource.cancelChanges();
        }
    });
});