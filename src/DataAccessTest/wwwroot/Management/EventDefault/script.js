/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/7 1:07:41*/
function createEntity() {
    return {
        selected: false,
        id : 0,
        eventName : '',
        eventCode : '',
        version : '',
        region : 'SystemEvent',
        eventType : 'Broadcast',
        resultOption : 'None',
        successOption : 'None',
        app : '',
        classify : '',
        tag : '',
        memo : '',
        targetType : '',
        targetName : '',
        targetDescription : ''
    };
}

function checkEntity(row) {
        if (typeof row.id === 'undefined')
            row.id = 0;
        if (typeof row.eventName === 'undefined')
            row.eventName = '';
        if (typeof row.eventCode === 'undefined')
            row.eventCode = '';
        if (typeof row.version === 'undefined')
            row.version = '';
        if (typeof row.region === 'undefined')
            row.region = 'SystemEvent';
        if (typeof row.eventType === 'undefined')
            row.eventType = 'Broadcast';
        if (typeof row.resultOption === 'undefined')
            row.resultOption = 'None';
        if (typeof row.successOption === 'undefined')
            row.successOption = 'None';
        if (typeof row.app === 'undefined')
            row.app = '';
        if (typeof row.classify === 'undefined')
            row.classify = '';
        if (typeof row.tag === 'undefined')
            row.tag = '';
        if (typeof row.memo === 'undefined')
            row.memo = '';
        if (typeof row.targetType === 'undefined')
            row.targetType = '';
        if (typeof row.targetName === 'undefined')
            row.targetName = '';
        if (typeof row.targetDescription === 'undefined')
            row.targetDescription = '';
        if (typeof row.isFreeze === 'undefined')
            row.isFreeze = false;
        if (typeof row.dataState === 'undefined')
            row.dataState = 'None';
        if (typeof row.lastModifyDate === 'undefined')
            row.lastModifyDate = '';
        if (typeof row.lastReviserId === 'undefined')
            row.lastReviserId = '';
        if (typeof row.lastReviser === 'undefined')
            row.lastReviser = '';
        if (typeof row.authorId === 'undefined')
            row.authorId = '';
        if (typeof row.author === 'undefined')
            row.author = '';
        if (typeof row.addDate === 'undefined')
            row.addDate = '';
}
extend_methods({
    getDef() {
        return createEntity();
    },
    checkListData(row) {
        checkEntity(row);
    }
});



/**
*   枚举列表
*/
extend_data({
    types : {

        /**
        *   事件区域类型
        */
        regionType : [
            {
                key: '0',
                value: 'SystemEvent',
                label: '系统事件'
            },
            {
                key: '1',
                value: 'DataEvent',
                label: '数据事件'
            },
            {
                key: '2',
                value: 'BusinessEvent',
                label: '业务事件'
            },
            {
                key: '3',
                value: 'FlowEvent',
                label: '流程事件'
            }
        ],
        /**
        *   事件类型类型
        */
        eventType : [
            {
                key: '0',
                value: 'Broadcast',
                label: '广播'
            },
            {
                key: '1',
                value: 'Log',
                label: '日志'
            },
            {
                key: '2',
                value: 'SequenceAggregation',
                label: '顺序聚合'
            },
            {
                key: '3',
                value: 'ParallelAggregation',
                label: '并行聚合'
            }
        ],
        /**
        *   处理结果设置类型
        */
        resultOptionType : [
            {
                key: '0',
                value: 'None',
                label: '不需要'
            },
            {
                key: '1',
                value: 'Result',
                label: '记录结果'
            },
            {
                key: '2',
                value: 'BagPack',
                label: '背包传递'
            }
        ],
        /**
        *   判断成功配置类型
        */
        successOptionType : [
            {
                key: '0',
                value: 'None',
                label: '不判断'
            },
            {
                key: '1',
                value: 'Once',
                label: '部分成功'
            },
            {
                key: '2',
                value: 'All',
                label: '所有成功'
            }
        ]
    }
});
extend_filter({
    /**
    *   事件区域类型枚举转文本
    */
    regionTypeFormater(val) {
        switch (val) {
            case 'SystemEvent': return '系统事件';
            case 'DataEvent': return '数据事件';
            case 'BusinessEvent': return '业务事件';
            case 'FlowEvent': return '流程事件';
            default:
                return '系统事件';
        }
    },

    /**
    *   事件类型类型枚举转文本
    */
    eventTypeFormater(val) {
        switch (val) {
            case 'Broadcast': return '广播';
            case 'Log': return '日志';
            case 'SequenceAggregation': return '顺序聚合';
            case 'ParallelAggregation': return '并行聚合';
            default:
                return '广播';
        }
    },

    /**
    *   处理结果设置类型枚举转文本
    */
    resultOptionTypeFormater(val) {
        switch (val) {
            case 'None': return '不需要';
            case 'Result': return '记录结果';
            case 'BagPack': return '背包传递';
            default:
                return '不需要';
        }
    },

    /**
    *   判断成功配置类型枚举转文本
    */
    successOptionTypeFormater(val) {
        switch (val) {
            case 'None': return '不判断';
            case 'Once': return '部分成功';
            case 'All': return '所有成功';
            default:
                return '不判断';
        }
    }
});


function doReady() {
    try {
        vue_option.data.apiPrefix = '/eventManagement/eventDefault/v1';
        vue_option.data.idField = 'id';
        var data = createEntity();
        vue_option.data.form.def = data
        vue_option.data.form.data = data;
        

        vueObject = new Vue(vue_option);
        vue_option.methods.loadList();
        
    } catch (e) {
        console.error(e);
    }
}
doReady();