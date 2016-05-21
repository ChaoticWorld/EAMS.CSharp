// JavaScript source code
$(document).ready(function(){
    var idx = 1;
    $('#pageGo').on('click', function () {
        idx = Number($('#pageIndex').val());
        changePageStripIdx(idx);
        $('#pageIndexp').val($('#pageIndex').val());
        goPage(idx);
    });
    $('#pageFirst').on('click', function () {
        idx = Number($('#pageFirst').attr('data-Idx'));
        $('#pageIndex').val(idx);
        $('#pageIndexp').val($('#pageIndex').val());
        $('#pageGo').click();
    });
    $('#pageEnd').on('click', function () {
        idx = Number($('#pageEnd').attr('data-Idx'));
        $('#pageIndex').val(idx);
        $('#pageIndexp').val($('#pageIndex').val());
        $('#pageGo').click();
    });
    $('#pagePrev').on('click',function(){
        idx = Number($('#pageIndex').val())-1;
        $('#pageIndex').val(idx < 1 ? 1 : idx);
        $('#pageIndexp').val($('#pageIndex').val());
        $('#pageGo').click();
    });
    $('#pageNext').on('click',function(){
        idx=Number($('#pageIndex').val())+1;
        max = Number($('#pageEnd').attr('data-Idx'));
        $('#pageIndex').val(idx>max?max:idx);
        $('#pageIndexp').val($('#pageIndex').val());
        $('#pageGo').click();
    });
});
function changePageIdx(clickPage){//点击p0-p5时处理
    var i = $(clickPage).text();
    $('#pageIndex').val(i);
    $('#pageIndexp').val(i);
    $('#pageGo').click();
}
function changePageStripIdx(clickIdx) {
    var First = Number($('#pageFirst').attr('data-Idx'));
    var End = Number($('#pageEnd').attr('data-Idx'));
    var pageTxt = 0;
    var pIdx = 0;
    $('#btnStrips').find('button').each(function () { $(this).removeAttr('disabled'); });
    if (clickIdx < 3) {
        for (var i = 0; i < 5; i++) {
            page = '#page' + i.toString();
            pageTxt = i + 1;
            $(page).text(pageTxt);
            if (pageTxt == clickIdx)
                $(page).attr('disabled', 'disabled');
            if (clickIdx == First) {
                $('#pagePrev').attr('disabled', 'disabled');
                $('#pageFirst').attr('disabled', 'disabled');
            }
        }
    }
    else if (clickIdx > End - 2) {
        for (var i = 0; i < 5; i++) {
            page = '#page' + i.toString();
            pageTxt = End - 4 + i;
            $(page).text(pageTxt);
            if (pageTxt == clickIdx)
                $(page).attr('disabled', 'disabled');
            if (clickIdx == End) {
                $('#pageNext').attr('disabled', 'disabled');
                $('#pageEnd').attr('disabled', 'disabled');
            }
        }
    }
    else {
        for (var i = 0; i < 5; i++) {
            page = '#page' + i.toString();
            pageTxt = clickIdx - 2 + i;
            $(page).text(pageTxt);
            if (pageTxt == clickIdx)
                $(page).attr('disabled', 'disabled');
        }
    }
}
function goPage(idx) {
    alert(idx);
    var url = '';
    $.ajax({
        url: url,
        data: { pageIdx: idx },type: 'POST',
        success: function () { }
    });
}
