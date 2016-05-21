// JavaScript source code

$(document).ready(function(){
    var idx = 1;
    $('#pageGo').on('click', function () {
        idx = Number($('#pageIndex').val());
        changePageStripIdx(idx);
        alert(idx);
    });
    $('#pageFirst').on('click', function () {
        idx = Number($('#pageFirst').attr('data-Idx'));
        $('#pageIndex').val(idx);
        $('#pageGo').click();
    });
    $('#pageEnd').on('click', function () {
        idx = Number($('#pageEnd').attr('data-Idx'));
        $('#pageIndex').val(idx);
        $('#pageGo').click();
    });
    $('#pagePrev').on('click',function(){
        idx = Number($('#pageIndex').val())-1;
        $('#pageIndex').val(idx);
        $('#pageGo').click();
    });
    $('#pageNext').on('click',function(){
        idx=Number($('#pageIndex').val())+1;
        $('#pageIndex').val(idx);
        $('#pageGo').click();
    });
});
function changePageIdx(clickPage){//点击p0-p5时处理
    var i = $(clickPage).text();
    $('#pageIndex').val(i);
    $('#pageGo').click();
}
function changePageStripIdx(clickIdx) {
    var First = Number($('#pageFirst').attr('data-Idx'));
    var End = Number($('#pageEnd').attr('data-Idx'));
    var pageTxt = 0;
    var pIdx = 0;
    $('#btnStrips').find('button').each(function () { $(this).removeAttr('disabled'); });
    if (clickIdx > First + 3 && clickIdx < End - 3) {
        pIdx = (End - clickIdx) % 7 - 1;
        if (pIdx < 0) for (var i = 0; i < 7; i++) {
            page = "#page" + i.toString();
            pageTxt = clickIdx - 3 + i;
            $(page).text(pageTxt);
            if (pageTxt == clickIdx)
                $(page).attr('disabled', 'disabled');
        }
        else
        for (var i = 0; i < 7; i++) {
            page = "#page" + i.toString();
            pageTxt = clickIdx - pIdx + i;
            $(page).text(pageTxt);
            if (pageTxt == clickIdx)
                $(page).attr('disabled', 'disabled');
        }
    }
    else if (clickIdx <= First + 3) {
        for (var i = 0; i < 7; i++) {
            page = "#page" + i.toString();
            pageTxt = i + First;
            $(page).text(pageTxt);
            if (pageTxt == clickIdx) {
                $(page).attr('disabled', 'disabled');
            }
        } if (clickIdx == First) {
            $('#pagePrev').attr('disabled', 'disabled');
            $('#pageFirst').attr('disabled', 'disabled');
        }
    }
    else if (clickIdx >= End - 3) {
        for (var i = 0; i < 7; i++) {
            page = "#page" + i.toString();
            pageTxt = End - 6 + i;
            $(page).text(pageTxt);
            if (pageTxt == clickIdx) {
                $(page).attr('disabled', 'disabled');
            }
        }
        if (clickIdx == End) {
            $('#pageNext').attr('disabled', 'disabled');
            $('#pageEnd').attr('disabled', 'disabled');
        }
    }
}
