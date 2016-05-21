$(function () {
    var trs = $("table #grid").rows;
    if (trs) {
        for (i = 1; i < trs.length; i++) {
            if (i % 2 == 0)
                tr.className += "alt";
        } 
    }
})
