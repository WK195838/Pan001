var Grid = null;
var UpperBound = 0;
var LowerBound = 1;
var CollapseImage = '/WebSite/App_Themes/images/minus.gif';
var ExpandImage = '/WebSite/App_Themes/images/plus.gif';
var IsExpanded = true;
var Rows = null;
var ToggleRow = 1;
var TimeSpan = 0;//展開與收合之速度

function Toggle(theGrid, Image, ToExpanded, RowIndex, RowCount) {
    Toggle(theGrid, Image, ToExpanded, RowIndex, RowCount, "", "");
}

function Toggle(theGrid, Image, ToExpanded, RowIndex, RowCount, CImage, EImage) {
    CollapseImage = CImage == '' ? CollapseImage : CImage;
    ExpandImage = EImage == '' ? ExpandImage : EImage;
    Grid = theGrid;
    Rows = Grid.getElementsByTagName('tr');
    ToggleRow = RowIndex + 1;
    LowerBound = ToggleRow;
    UpperBound = ToggleRow + RowCount - 1;
    IsExpanded = Rows[UpperBound].style.display == '' ? false : true;
    ToggleImage(Image);
    ToggleRows(RowIndex);
}

function ToggleImage(Image) {
    if (IsExpanded) {
        Image.src = CollapseImage;
        Image.title = 'Collapse';
        Grid.rules = 'none';
        ToggleRow = LowerBound;

        IsExpanded = false;
    }
    else {
        Image.src = ExpandImage;
        Image.title = 'Expand';
        Grid.rules = 'cols';
        ToggleRow = UpperBound;

        IsExpanded = true;
    }
}

function ToggleRows(RowIndex) {
    if (ToggleRow < LowerBound || ToggleRow > UpperBound) return;

    if (Rows[ToggleRow].style.backgroundRepeat == null || Rows[ToggleRow].style.backgroundRepeat == '') {
        Rows[ToggleRow].style.display = IsExpanded ? 'none' : '';
    }
    else {
        if (Rows[ToggleRow].style.marginRight != '1px') {
            var im = Rows[ToggleRow].getElementsByTagName('img');
            try {
                //因為IE以外的瀏覽器可能認不得,所以要包try catch,才不會造成失效
                im(0).src = IsExpanded ? ExpandImage : CollapseImage;
            }
            catch (e)
                { }             
            Rows[ToggleRow].style.display = IsExpanded ? 'none' : '';
        }
    }
        //Rows[ToggleRow].style.display = Rows[ToggleRow].style.display == '' ? 'none' : '';
    if (IsExpanded) ToggleRow--; else ToggleRow++;
    //有展開收動作特效
    //setTimeout("ToggleRows(" + ToggleRow + ")", TimeSpan);
    //無展開收動作特效
    ToggleRows(ToggleRow);
}

/*---FOR 全部展開/收合----*/
/*注意:收合列背景色需設置為空，否則收合時將會把收合列同明細一起收合至僅剩標題*/
function ToggleAll(theGrid, ToExpanded, RowCount) {
    Grid = theGrid;
    Rows = Grid.getElementsByTagName('tr');
 
    if (ToExpanded) {//展開
        for (i = 1; i <= RowCount; i = i + 1) {
            if (Rows[i].style.backgroundRepeat == '') {
                Rows[i].style.display = '';
            }
            else {
                if (Rows[i].style.marginRight != '1px') Rows[i].style.display = '';
                var im = Rows[i].getElementsByTagName('img');
                try {
                    //因為IE以外的瀏覽器可能認不得,所以要包try catch,才不會造成失效
                    im(0).src = CollapseImage;
                }
                catch (e)
                { } 
            }
        }
    }
    else {//收合
        for (i = 1; i <= RowCount; i = i + 1) {
            if (Rows[i].style.backgroundRepeat == '') {
                if (Rows[i].style.marginRight != '1px') Rows[i].style.display = 'none';
            }
            else {
                if (Rows[i].style.marginRight != '1px') Rows[i].style.display = 'none';
                var im = Rows[i].getElementsByTagName('img');
                try {
                    //因為IE以外的瀏覽器可能認不得,所以要包try catch,才不會造成失效
                    im(0).src = ExpandImage;
                }
                catch (e)
                { }   
                
            }
        }
    }
}