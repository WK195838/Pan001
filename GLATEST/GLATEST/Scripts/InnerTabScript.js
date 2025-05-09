﻿//$(function () {
//    // 預設顯示第一個頁籤
//    // 並先把 .tabs, .tabs li 及 .tab_content, .tab_content li 等元素取出
//    // 同時也要取得 .tab_content 的寬
//    var _default = 0,
//			$block = $('#abgne-block-20120327'),
//			$tabs = $block.find('.tabs'),
//			$tabsLi = $tabs.find('li'),
//			$tab_content = $block.find('.tab_content'),
//			$tab_contentLi = $tab_content.find('li'),
//			_width = $tab_content.width();

//    // 當滑鼠移到 .tabs li 上時要套用 .hover 樣式
//    // 移出時要移除 .hover 樣式
//    $tabsLi.hover(function () {
//        var $this = $(this);

//        // 若被滑鼠移上去的 li 是目前顯示的頁籤就不做任何動作
//        if ($this.hasClass('active')) return;

//        $this.toggleClass('hover');
//    }).click(function () {	// 當滑鼠點擊 .tabs li 時
//        // 先取出被點擊及目前顯示的頁籤與索引
//        var $active = $tabsLi.filter('.active').removeClass('active'),
//				_activeIndex = $active.index(),
//				$this = $(this).addClass('active').removeClass('hover'),
//				_index = $this.index(),
//				isNext = _index > _activeIndex;

//        // 如果被點擊的頁籤就是目前已顯示的頁籤, 則不做任何動作
//        if (_activeIndex == _index) return;

//        // 依索引大或小來決定移動的位置
//        $tab_contentLi.eq(_activeIndex).stop().animate({
//            left: isNext ? -_width : _width
//        }).end().eq(_index).css('left', isNext ? _width : -_width).stop().animate({
//            left: 0
//        });
//    });

//    // 先把預設要顯示的頁籤加上 .active 樣式及顯示相對應的內容
//    $tabsLi.eq(_default).addClass('active');
//    $tab_contentLi.eq(_default).siblings().css({
//        left: _width
//    });
//});
$(document).ready(function () {
    // 預設顯示第一個 Tab
    var _showTab = parseInt($("input[id$=hfTabIndex]").val());
    var $defaultLi = $('ul.tabs li').eq(_showTab).addClass('active');
    $($defaultLi.find('a').attr('href')).siblings().hide();
    // 當 li 頁籤被點擊時...
    // 若要改成滑鼠移到 li 頁籤就切換時, 把 click 改成 mouseover
    //$('ul.tabs li').mouseover(function() {
    $('ul.tabs li').click(function () {
        //alert('AA');
        // 找出 li 中的超連結 href(#id)
        var $this = $(this),
			                _clickTab = $this.find('a').attr('href');
        // 把目前點擊到的 li 頁籤加上 .active
        // 並把兄弟元素中有 .active 的都移除 class
        $this.addClass('active').siblings('.active').removeClass('active');
        // 淡入相對應的內容並隱藏兄弟元素
        $(_clickTab).stop(false, true).fadeIn().siblings().hide();
        return false;
    }).find('a').focus(function () {
        this.blur();
    });
});

//$("input[id$=hfTabKey]").val((parseInt($this.find('a').attr('href').substring(4, 1)) - 1).toString());