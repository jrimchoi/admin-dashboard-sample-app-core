/** 
 * Kendo UI v2022.1.412 (http://www.telerik.com/kendo-ui)                                                                                                                                               
 * Copyright 2022 Progress Software Corporation and/or one of its subsidiaries or affiliates. All rights reserved.                                                                                      
 *                                                                                                                                                                                                      
 * Kendo UI commercial licenses may be obtained at                                                                                                                                                      
 * http://www.telerik.com/purchase/license-agreement/kendo-ui-complete                                                                                                                                  
 * If you do not own a commercial license, this file shall be governed by the trial license terms.                                                                                                      
                                                                                                                                                                                                       
                                                                                                                                                                                                       
                                                                                                                                                                                                       
                                                                                                                                                                                                       
                                                                                                                                                                                                       
                                                                                                                                                                                                       
                                                                                                                                                                                                       
                                                                                                                                                                                                       
                                                                                                                                                                                                       
                                                                                                                                                                                                       
                                                                                                                                                                                                       
                                                                                                                                                                                                       
                                                                                                                                                                                                       
                                                                                                                                                                                                       
                                                                                                                                                                                                       

*/
(function(f, define){
    define('kendo.imagebrowser',[ "./kendo.filebrowser" ], f);
})(function(){

var __meta__ = { // jshint ignore:line
    id: "imagebrowser",
    name: "ImageBrowser",
    category: "web",
    description: "",
    hidden: true,
    depends: [ "filebrowser" ]
};

(function($, undefined) {
    var kendo = window.kendo,
        FileBrowser = kendo.ui.FileBrowser,
        isPlainObject = $.isPlainObject,
        extend = $.extend,
        isFunction = kendo.isFunction,
        trimSlashesRegExp = /(^\/|\/$)/g,
        ERROR = "error",
        NS = ".kendoImageBrowser",
        NAMEFIELD = "name",
        SIZEFIELD = "size",
        TYPEFIELD = "type",
        DEFAULTSORTORDER = { field: TYPEFIELD, dir: "asc" },
        EMPTYTILE = kendo.template('<div class="k-listview-item k-listview-item-empty"><span class="k-file-preview"><span class="k-file-icon k-icon k-i-none"></span></span><span class="k-file-name">${text}</span></div>');

    extend(true, kendo.data, {
        schemas: {
            "imagebrowser": {
                data: function(data) {
                    return data.items || data || [];
                },
                model: {
                    id: "name",
                    fields: {
                        name: "name",
                        size: "size",
                        type: "type"
                    }
                }
            }
        }
    });

    extend(true, kendo.data, {
        transports: {
            "imagebrowser": kendo.data.RemoteTransport.extend({
                init: function(options) {
                    kendo.data.RemoteTransport.fn.init.call(this, $.extend(true, {}, this.options, options));
                },
                _call: function(type, options) {
                    options.data = $.extend({}, options.data, { path: this.options.path() });

                    if (isFunction(this.options[type])) {
                        this.options[type].call(this, options);
                    } else {
                        kendo.data.RemoteTransport.fn[type].call(this, options);
                    }
                },
                read: function(options) {
                    this._call("read", options);
                },
                create: function(options) {
                    this._call("create", options);
                },
                destroy: function(options) {
                    this._call("destroy", options);
                },
                update: function() {
                    //updates are handled by the upload
                },
                options: {
                    read: {
                        type: "POST"
                    },
                    update: {
                        type: "POST"
                    },
                    create: {
                        type: "POST"
                    },
                    destroy: {
                        type: "POST"
                    }
                }
            })
        }
    });

    function offsetTop(element) {
        return element.offsetTop - $(element).height();
    }

    function concatPaths(path, name) {
        if(path === undefined || !path.match(/\/$/)) {
            path = (path || "") + "/";
        }
        return path + name;
    }

    function sizeFormatter(value) {
        if(!value) {
            return "";
        }

        var suffix = " bytes";

        if (value >= 1073741824) {
            suffix = " GB";
            value /= 1073741824;
        } else if (value >= 1048576) {
            suffix = " MB";
            value /= 1048576;
        } else  if (value >= 1024) {
            suffix = " KB";
            value /= 1024;
        }

        return Math.round(value * 100) / 100 + suffix;
    }

    var ImageBrowser = FileBrowser.extend({
        init: function(element, options) {
            var that = this;

            options = options || {};

            FileBrowser.fn.init.call(that, element, options);

            that.element.addClass("k-imagebrowser");
        },

        options: {
            name: "ImageBrowser",
            fileTypes: "*.png,*.gif,*.jpg,*.jpeg"
        },

        value: function () {
            var that = this,
                selected = that._selectedItem(),
                path,
                imageUrl = that.options.transport.imageUrl;

            if (selected && selected.get(TYPEFIELD) === "f") {
                path = concatPaths(that.path(), selected.get(NAMEFIELD)).replace(trimSlashesRegExp, "");
                if (imageUrl) {
                    path = isFunction(imageUrl) ? imageUrl(path) : kendo.format(imageUrl, encodeURIComponent(path));
                }
                return path;
            }
        },

        _fileUpload: function(e) {
            var that = this,
                options = that.options,
                fileTypes = options.fileTypes,
                filterRegExp = new RegExp(("(" + fileTypes.split(",").join(")|(") + ")").replace(/\*\./g , ".*."), "i"),
                fileName = e.files[0].name,
                fileSize = e.files[0].size,
                fileNameField = NAMEFIELD,
                sizeField = SIZEFIELD,
                file;

            if (filterRegExp.test(fileName)) {
                e.data = { path: that.path() };

                file = that._createFile(fileName, fileSize);

                if (!file) {
                    e.preventDefault();
                } else {
                    file._uploading = true;

                    that.upload.one("error", function() {
                        file = undefined;
                    });

                    that.upload.one("success", function(e) {
                        if (file) {
                            delete file._uploading;

                            var model = that._insertFileToList(file);

                            model.set(fileNameField, e.response[that._getFieldName(fileNameField)]);
                            model.set(sizeField, e.response[that._getFieldName(sizeField)]);

                            that._tiles = that.listView.items().filter("[" + kendo.attr("type") + "=f]");
                            that._scroll();
                        }
                    });
                }
            } else {
                e.preventDefault();
                that._showMessage(kendo.format(options.messages.invalidFileType, fileName, fileTypes));
            }
        },

        _content: function() {
            var that = this;

            that.list = $('<div class="k-filemanager-listview" />')
                .appendTo(that.element)
                .on("dblclick" + NS, ".k-listview-item", that._dblClick.bind(that));

            that.listView = new kendo.ui.ListView(that.list, {
                layout: "flex",
                flex: {
                    direction: "row",
                    wrap: "wrap"
                },
                dataSource: that.dataSource,
                template: that._itemTmpl(),
                editTemplate: that._editTmpl(),
                selectable: true,
                autoBind: false,
                dataBinding: function(e) {

                    that.toolbar.find(".k-i-close").parent().addClass("k-disabled");

                    if (e.action === "remove" || e.action === "sync") {
                        e.preventDefault();
                        kendo.ui.progress(that.listView.content, false);
                    }
                },
                dataBound: function() {
                    if (that.dataSource.view().length) {
                        that._tiles = this.items().filter("[" + kendo.attr("type") + "=f]");
                        that._scroll();
                    } else {
                        this.content.append(EMPTYTILE({ text: that.options.messages.emptyFolder }));
                    }
                },
                change: that._listViewChange.bind(that)
            });

            that.listView.content.on("scroll" + NS, that._scroll.bind(that));
        },

        _dataSource: function() {
            var that = this,
                options = that.options,
                transport = options.transport,
                typeSortOrder = extend({}, DEFAULTSORTORDER),
                nameSortOrder = { field: NAMEFIELD, dir: "asc" },
                schema,
                dataSource = {
                    type: transport.type || "imagebrowser",
                    sort: [typeSortOrder, nameSortOrder]
                };

            if (isPlainObject(transport)) {
                transport.path = that.path.bind(that);
                dataSource.transport = transport;
            }

            if (isPlainObject(options.schema)) {
                dataSource.schema = options.schema;
            } else if (transport.type && isPlainObject(kendo.data.schemas[transport.type])) {
                schema = kendo.data.schemas[transport.type];
            }

            if (that.dataSource && that._errorHandler) {
                that.dataSource.unbind(ERROR, that._errorHandler);
            } else {
                that._errorHandler = that._error.bind(that);
            }

            that.dataSource = kendo.data.DataSource.create(dataSource)
                .bind(ERROR, that._errorHandler);
        },

        _loadImage: function(li) {
            var that = this,
                element = $(li),
                dataItem = that.dataSource.getByUid(element.attr(kendo.attr("uid"))),
                name = dataItem.get(NAMEFIELD),
                thumbnailUrl = that.options.transport.thumbnailUrl,
                img = $("<img />", { alt: name }),
                urlJoin = "?";

            if (dataItem._uploading) {
                return;
            }

            img.hide()
               .on("load" + NS, function() {
                   $(this).prev().remove().end().addClass("k-image k-file-image").fadeIn();
               });

            element.find(".k-i-loading").after(img);

            if (isFunction(thumbnailUrl)) {
                thumbnailUrl = thumbnailUrl(that.path(), encodeURIComponent(name));
            } else {
                if (thumbnailUrl.indexOf("?") >= 0) {
                    urlJoin = "&";
                }

                thumbnailUrl = thumbnailUrl + urlJoin + "path=" + encodeURIComponent(that.path() + name);
                if (dataItem._override) {
                    thumbnailUrl += "&_=" + new Date().getTime();
                    delete dataItem._override;
                }
            }

            // IE8 will trigger the load event immediately when the src is assigned
            // if the image is loaded from the cache
            img.attr("src", thumbnailUrl);

            li.loaded = true;
        },

        _scroll: function() {
            var that = this;
            if (that.options.transport && that.options.transport.thumbnailUrl) {
                clearTimeout(that._timeout);

                that._timeout = setTimeout(function() {

                    var height = kendo._outerHeight(that.listView.content),
                        viewTop = that.listView.content.scrollTop(),
                        viewBottom = viewTop + height;

                    that._tiles.each(function() {
                        var top = offsetTop(this),
                            bottom = top + this.offsetHeight;

                        if ((top >= viewTop && top < viewBottom) || (bottom >= viewTop && bottom < viewBottom)) {
                            that._loadImage(this);
                        }

                        if (top > viewBottom) {
                            return false;
                        }
                    });

                    that._tiles = that._tiles.filter(function() {
                        return !this.loaded;
                    });

                }, 250);
            }
        },

        _itemTmpl: function() {
            var that = this,
                html = '<div class="k-listview-item" ' + kendo.attr("uid") + '="#=uid#" ';

            html += kendo.attr("type") + '="${' + TYPEFIELD + '}">';
            html += '#if(' + TYPEFIELD + ' == "d") { #';
            html += '<div class="k-file-preview"><span class="k-file-icon k-icon k-i-folder"></span></div>';
            html += "#}else{#";
            if (that.options.transport && that.options.transport.thumbnailUrl) {
                html += '<div class="k-file-preview"><span class="k-file-icon k-icon k-i-loading"></span></div>';
            } else {
                html += '<div class="k-file-preview"><span class="k-file-icon k-icon k-i-file"></span></div>';
            }
            html += "#}#";
            html += '<span class="k-file-name">${' + NAMEFIELD + '}</span>';
            html += '#if(' + TYPEFIELD + ' == "f") { # <span class="k-file-size">${this.sizeFormatter(' + SIZEFIELD + ')}</span> #}#';
            html += '</div>';

            return kendo.template(html).bind({ sizeFormatter: sizeFormatter });
        }
    });

    kendo.ui.plugin(ImageBrowser);
})(window.kendo.jQuery);

return window.kendo;

}, typeof define == 'function' && define.amd ? define : function(a1, a2, a3){ (a3 || a2)(); });

