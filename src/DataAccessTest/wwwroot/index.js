
var vue_option = {
    el: '#work_space',
    data: {
        isCollapse: true,
        cwid: '',
        ctxt: "",
        user: ''
    },
    methods: {
        collapse() {
            vue_option.data.isCollapse = !vue_option.data.isCollapse;
            vue_option.data.cwid = vue_option.data.isCollapse ? '' : '260px';
            vue_option.data.ctxt = vue_option.data.isCollapse ? '' : vue_option.data.user;
        },
        menu_select(index) {
            switch (index) {
                case '_event_def':
                    showIframe('/Management/EventDefault/index.htm');
                    break;
            }
        }
    }
};


function ready() {
    vue_option.data.user = "Tester";
    vueObject = new Vue(vue_option);
}
var version = 1;
function showIframe(url) {
    if (!url)
        return;
    try {
        console.log(url);
        var ws = document.getElementById("work_frame");
        if (url.indexOf("?") > 0)
            ws.src = url + "&__t=" + version;
        else
            ws.src = url + "?__t=" + version;
    } catch (e) {
        console.error(e);
    }
}
ready();