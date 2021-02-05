
function extend(default_item, new_item) {
    if (new_item) {
        for (var i in new_item) {
            if (new_item[i]) {
                default_item[i] = new_item[i]
            }
        }
    }
    return default_item;
}
function extend_data(data) {
    extend(vue_option.data, data);
}
function extend_filter(filters) {
    extend(vue_option.filters, filters);
}
function extend_methods(methods) {
    extend(vue_option.methods, methods);
}

ws_state = function (active) {
    vue_option.data.ws_active = active;
};


var common = {
    /**
     * 用户确认
     * @param {string} title 标题
     * @param {string} message 消息
     * @param {Function} callback 确认后的回调
     * @returns {void}
     */
    confirm(title, message, callback) {
        vueObject.$confirm(message, title, {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning'
        }).then(() => {
            callback();
        });
    },
    /**
     * 显示提示
     * @param {string} title 标题
     * @param {string} message 消息
     * @returns {void} 
     */
    showTip(title, message) {
        vueObject.$notify({
            title: title,
            message: message,
            duration: 0
        });
    },
    /**
     * 显示提示
     * @param {string} title 标题
     * @param {string} message 消息
     * @param {string} type 类型
     * @returns {void} 
     */
    showMessage(title, message, type) {
        if (!type) {
            type = 'success';
        }
        vue_option.data.message = message;
        vueObject.$notify({
            title: title,
            message: message,
            type: type,
            duration: 2000
        });
    },
    /**
     * 显示提示
     * @param {string} title 标题
     * @param {string} message 消息
     * @param {string} type 类型
     * @returns {void} 
     */
    showError(title, message) {
        this.showMessage(title, message, 'error');
    },
    /**
     * 显示提示
     * @param {object} res 返回值 标题
     * @returns {void} 
     */
    showStatus(res) {
        if (res.success) {
            this.showMessage(null, res.status && res.status.msg
                ? res.status.msg
                : "请求成功");
        }
        else {
            this.showMessage(null, res.status && res.status.msg
                ? res.status.msg
                : "网络错误", "error");
        }
    },
    loading: null,
    busyNum: 0,
    isBusy: false,
    showBusy(title) {
        if (this.isBusy || this.loading) {
            ++this.busyNum;
            return;
        }
        this.busyNum = 1;
        this.isBusy = true;
        var that = this;
        setTimeout(function () {
            if (!that.isBusy || this.busyNum === 0)
                return;
            that.loading = vueObject.$loading({
                lock: true,
                text: `正在执行【${title}】...`,
                spinner: 'el-icon-loading',
                background: 'rgba(0, 0, 0, 0.7)'
            });
            if (!that.isBusy || this.busyNum === 0) {
                this.loading.close();
                this.loading = null;
            }
        }, 300);
    },
    hideBusy() {
        if (--this.busyNum === 0) {
            this.isBusy = false;
            if (this.loading) {
                this.loading.close();
                this.loading = null;
            }
        }
    }
};

var ajax_ex = {
    config(page, action) {
        return {
            //baseURL: "http://192.168.132.240",
            baseURL: "",
            timeout: 3000,
            headers: {
                "x_zmvc_action": action,
                "x_zmvc_page": page
            }
        };
    },
    /**
     * ajax 操作
     * @param {string} title 标题
     * @param {string} url 地址
     * @param {function} args 参数
     * @param {function} callback 成功回调
     * @param {function} failed 失败回调
     * @param {any} tip 是否显示提示
     */
    post(title, url, args, callback, failed, tip) {
        this.doAjax(title, url, args, callback, failed, tip);
    },
    doAjax(title, url, args, callback, failed, tip) {

        var that = this;
        var redo = function () {
            that.doAjax(title, url, args, callback, failed, tip);
        }
        common.showBusy(title);
        axios.post(url, args, that.config()).then(resp => {
            common.hideBusy();
            var res = that.evalResult(resp.data);
            if (!that.checkApiStatus(title, url, res, resp.status, tip, redo))
                return;
            try {
                if (callback)
                    callback(res);
            } catch (ex) {
                console.error(`${url} : ${ex}`);
                common.showError(title, "结果处理失败!");
            }
        }).catch(err => {
            common.hideBusy();
            console.log(`${url} : ${err}`);
            if (!err.response) {
                common.showError(title, "网络错误");
                return;
            }
            if (err.response.data) {
                var res = that.evalResult(err.response.data);
                if (!that.checkApiStatus(title, url, res, err.response.status, tip, redo))
                    return;
                try {
                    if (failed)
                        failed(res);
                } catch (ex) {
                    console.error(`${url} : ${ex}`);
                    common.showError(title, "结果处理失败!");
                }
                return;
            }
            if (!that.checkApiStatus(title, url, null, err.response.status, tip, redo))
                return;
            if (failed)
                failed();
        });
    },
    /**
     * 校验API返回的标准状态
     * @param {string} title 任务标题
     * @param {string} url 原始URL
     * @param {string} result 返回值
     * @param {string} tip 是否显示提示
     * @param {Function} callback 令牌恢复时的回调
     * @returns {boolean} true表示可以继续后续操作,false表示应中断操作
     */
    checkApiStatus(title, url, result, status, tip, callback) {
        if (result)
            switch (result.code) {
                case OperatorStatusCode.Queue:
                case OperatorStatusCode.Success:
                    return true;
            }

        switch (status) {
            case 403:
                console.log(`${url} : 拒绝访问`);
                if (tip)
                    common.showError(title, "拒绝访问!");
                return true;
            case 404:
                console.log(`${url} : 页面不存在`);
                if (tip)
                    common.showError(title, "页面不存在!");
                return true;
            case 503:
                console.log(`${url} : 服务器拒绝操作`);
                if (tip)
                    common.showError(title, "服务器拒绝操作!");
                return true;
        }
        if (!result) {
            console.log(`${url} : 无返回内容`);
            if (tip)
                common.showError(title, "发生未知错误，操作失败!");
            return true;
        }

        if (result.message) {
            console.log(`${url} : ${result.message}`);
            if (tip)
                common.showError(title, result.message);
        }
        else if (result.code == 404) {
            console.log(`${url} : ${result.message}`);
            if (tip)
                common.showError(title, `接口(${url})不通!`);
        } else {
            console.log(`${url} : "发生未知错误，操作失败!"`);
            if (tip)
                common.showError(title, "发生未知错误，操作失败!");
        }
        return true;
    },
    /**
     * 转换返回值
     * @param {any} result 返回值对象或文本
     * @returns {object} 对象
     */
    evalResult(result) {
        if (!result)
            return null;
        try {
            if (typeof result === "string") {
                console.log(result);
                return eval("(" + result + ")");
            }
        } catch (ex) {
            console.log(ex);
            return null;
        }
        return result;
    }
};


/**
 * 原始AJAX
 * @param {string} title 任务标题
 * @param {string} api 调用的API
 * @param {string} args 参数
 * @param {Function} onSucceed 成功回调
 * @param {Function} onFailed 失败回调
 */
function ajax_post(title, api, args, onSucceed, onFailed,) {
    ajax_ex.post(title, api, args, onSucceed, onFailed, true);
}

/**
 * 远程调用(无提示)
 * @param {string} title 任务标题
 * @param {string} url 调用的API
 * @param {string} args 参数
 * @param {Function} onSucceed 成功回调
 * @param {Function} onFailed 失败回调
 */
function silentCall(title, url, args, onSucceed, onFailed) {
    ajax_ex.post(title, url, args, onSucceed, onFailed, false);
}

/**
 * 执行远程操作,操作显示一个确认框
 * @public 
 * @param {string} title 操作标题
 * @param {string} url 远程URL
 * @param {object} args 参数
 * @param {function} onSucceed 执行完成后的回调方法
 * @param {string} confirmMessage 确认操作的消息
 * @returns {void} 
 */
function confirmCall(title, url, args, onSucceed, confirmMessage) {
    if (!confirmMessage)
        confirmMessage = "确定要执行操作吗?";
    common.confirm(title, confirmMessage, () => {
        ajax_ex.post(title, url, args, onSucceed, null, true);
    });
}


/** 操作状态码
*/
var OperatorStatusCode = {
    /// <summary>
    ///     正在排队
    /// </summary>
    Queue: 1,

    /// <summary>
    ///     正确
    /// </summary>
    Success: 0,

    /// <summary>
    ///     参数错误
    /// </summary>
    ArgumentError: -1,

    /// <summary>
    ///     发生处理业务错误
    /// </summary>
    BusinessError: -2,

    /// <summary>
    ///     发生未处理业务异常
    /// </summary>
    BusinessException: -3,

    /// <summary>
    ///     发生未处理系统异常
    /// </summary>
    UnhandleException: -4,

    /// <summary>
    ///     网络错误
    /// </summary>
    NetworkError: -5,

    /// <summary>
    ///     执行超时
    /// </summary>
    TimeOut: -6,

    /// <summary>
    ///     拒绝访问
    /// </summary>
    DenyAccess: -7,

    /// <summary>
    ///     未知的Token
    /// </summary>
    TokenUnknow: -8,

    /// <summary>
    ///     令牌过期
    /// </summary>
    TokenTimeOut: -9,

    /// <summary>
    ///     系统未就绪
    /// </summary>
    NoReady: -0xA,

    /// <summary>
    ///     异常中止
    /// </summary>
    Ignore: -0xB,

    /// <summary>
    ///     重试
    /// </summary>
    ReTry: -0xC,

    /// <summary>
    ///     方法不存在
    /// </summary>
    NoFind: -0xD,

    /// <summary>
    ///     服务不可用
    /// </summary>
    Unavailable: -0xE,

    /// <summary>
    ///     未知结果
    /// </summary>
    Unknow: 0xF
};