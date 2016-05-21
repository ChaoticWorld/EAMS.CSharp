/*
var myDate = new Date();
myDate.getYear(); //��ȡ��ǰ���(2λ)
myDate.getFullYear(); //��ȡ���������(4λ,1970-????)
myDate.getMonth(); //��ȡ��ǰ�·�(0-11,0����1��)
myDate.getDate(); //��ȡ��ǰ��(1-31)
myDate.getDay(); //��ȡ��ǰ����X(0-6,0����������)
myDate.getTime(); //��ȡ��ǰʱ��(��1970.1.1��ʼ�ĺ�����)
myDate.getHours(); //��ȡ��ǰСʱ��(0-23)
myDate.getMinutes(); //��ȡ��ǰ������(0-59)
myDate.getSeconds(); //��ȡ��ǰ����(0-59)
myDate.getMilliseconds(); //��ȡ��ǰ������(0-999)
myDate.toLocaleDateString(); //��ȡ��ǰ����
var mytime=myDate.toLocaleTimeString(); //��ȡ��ǰʱ��
myDate.toLocaleString( ); //��ȡ������ʱ��


����ʱ��ű��ⷽ���б�

Date.prototype.isLeapYear �ж�����
Date.prototype.Format ���ڸ�ʽ��
Date.prototype.DateAdd ���ڼ���
Date.prototype.DateDiff �Ƚ����ڲ�
Date.prototype.toString ����ת�ַ���
Date.prototype.toArray ���ڷָ�Ϊ����
Date.prototype.DatePart ȡ���ڵĲ�����Ϣ
Date.prototype.MaxDayOfDate ȡ���������µ��������
Date.prototype.WeekNumOfYear �ж�����������ĵڼ���
StringToDate �ַ���ת������
IsValidDate ��֤������Ч��
CheckDateTime ��������ʱ����
CheckDate �������ڼ��
daysBetween ����������
*/
//js���룺

//---------------------------------------------------
// �ж�����
//---------------------------------------------------
Date.prototype.isLeapYear = function()
{
return (0==this.getYear()%4&&((this.getYear()%100!=0)||(this.getYear()%400==0)));
}

//---------------------------------------------------
// ���ڸ�ʽ��
// ��ʽ YYYY/yyyy/YY/yy ��ʾ���
// MM/M �·�
// W/w ����
// dd/DD/d/D ����
// hh/HH/h/H ʱ��
// mm/m ����
// ss/SS/s/S ��
//---------------------------------------------------
Date.prototype.Format = function(formatStr)
{
var str = formatStr;
var Week = ['��','һ','��','��','��','��','��'];

str=str.replace(/yyyy|YYYY/,this.getFullYear());
str=str.replace(/yy|YY/,(this.getYear() % 100)>9?(this.getYear() % 100).toString():'0' + (this.getYear() % 100));
var m = this.getMonth()+1;
str=str.replace(/MM/,m>9?m:'0' + m);
str=str.replace(/M/g,m);

str=str.replace(/w|W/g,Week[this.getDay()]);

str=str.replace(/dd|DD/,this.getDate()>9?this.getDate().toString():'0' + this.getDate());
str=str.replace(/d|D/g,this.getDate());

str=str.replace(/hh|HH/,this.getHours()>9?this.getHours().toString():'0' + this.getHours());
str=str.replace(/h|H/g,this.getHours());
str=str.replace(/mm/,this.getMinutes()>9?this.getMinutes().toString():'0' + this.getMinutes());
str=str.replace(/m/g,this.getMinutes());

str=str.replace(/ss|SS/,this.getSeconds()>9?this.getSeconds().toString():'0' + this.getSeconds());
str=str.replace(/s|S/g,this.getSeconds());

return str;
}

//+---------------------------------------------------
//| ������ʱ��������� ���ڸ�ʽΪ YYYY-MM-dd
//+---------------------------------------------------
function daysBetween(DateOne,DateTwo)
{
var OneMonth = DateOne.substring(5,DateOne.lastIndexOf ('-'));
var OneDay = DateOne.substring(DateOne.length,DateOne.lastIndexOf ('-')+1);
var OneYear = DateOne.substring(0,DateOne.indexOf ('-'));

var TwoMonth = DateTwo.substring(5,DateTwo.lastIndexOf ('-'));
var TwoDay = DateTwo.substring(DateTwo.length,DateTwo.lastIndexOf ('-')+1);
var TwoYear = DateTwo.substring(0,DateTwo.indexOf ('-'));

var cha=((Date.parse(OneMonth+'/'+OneDay+'/'+OneYear)- Date.parse(TwoMonth+'/'+TwoDay+'/'+TwoYear))/86400000);
return Math.abs(cha);
}


//+---------------------------------------------------
//| ���ڼ���
//+---------------------------------------------------
Date.prototype.DateAdd = function(strInterval, Number) {
var dtTmp = this;
switch (strInterval) {
case 's' :return new Date(Date.parse(dtTmp) + (1000 * Number));
case 'n' :return new Date(Date.parse(dtTmp) + (60000 * Number));
case 'h' :return new Date(Date.parse(dtTmp) + (3600000 * Number));
case 'd' :return new Date(Date.parse(dtTmp) + (86400000 * Number));
case 'w' :return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));
case 'q' :return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number*3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
case 'm' :return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
case 'y' :return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
}
}

//+---------------------------------------------------
//| �Ƚ����ڲ� dtEnd ��ʽΪ�����ͻ��� ��Ч���ڸ�ʽ�ַ���
//+---------------------------------------------------
Date.prototype.DateDiff = function(strInterval, dtEnd) {
var dtStart = this;
if (typeof dtEnd == 'string' )//������ַ���ת��Ϊ������
{
dtEnd = StringToDate(dtEnd);
}
switch (strInterval) {
case 's' :return parseInt((dtEnd - dtStart) / 1000);
case 'n' :return parseInt((dtEnd - dtStart) / 60000);
case 'h' :return parseInt((dtEnd - dtStart) / 3600000);
case 'd' :return parseInt((dtEnd - dtStart) / 86400000);
case 'w' :return parseInt((dtEnd - dtStart) / (86400000 * 7));
case 'm' :return (dtEnd.getMonth()+1)+((dtEnd.getFullYear()-dtStart.getFullYear())*12) - (dtStart.getMonth()+1);
case 'y' :return dtEnd.getFullYear() - dtStart.getFullYear();
}
}

//+---------------------------------------------------
//| ��������ַ�����������ϵͳ��toString����
//+---------------------------------------------------
Date.prototype.toString = function(showWeek)
{
var myDate= this;
var str = myDate.toLocaleDateString();
if (showWeek)
{
var Week = ['��','һ','��','��','��','��','��'];
str += ' ����' + Week[myDate.getDay()];
}
return str;
}

//+---------------------------------------------------
//| ���ںϷ�����֤
//| ��ʽΪ��YYYY-MM-DD��YYYY/MM/DD
//+---------------------------------------------------
function IsValidDate(DateStr)
{
var sDate=DateStr.replace(/(^\s+|\s+$)/g,''); //ȥ���߿ո�;
if(sDate=='') return true;
//�����ʽ����YYYY-(/)MM-(/)DD��YYYY-(/)M-(/)DD��YYYY-(/)M-(/)D��YYYY-(/)MM-(/)D���滻Ϊ''
//���ݿ��У��Ϸ����ڿ�����:YYYY-MM/DD(2003-3/21),���ݿ���Զ�ת��ΪYYYY-MM-DD��ʽ
var s = sDate.replace(/[\d]{ 4,4 }[\-/]{ 1 }[\d]{ 1,2 }[\-/]{ 1 }[\d]{ 1,2 }/g,'');
if (s=='') //˵����ʽ����YYYY-MM-DD��YYYY-M-DD��YYYY-M-D��YYYY-MM-D
{
var t=new Date(sDate.replace(/\-/g,'/'));
var ar = sDate.split(/[-/:]/);
if(ar[0] != t.getYear() || ar[1] != t.getMonth()+1 || ar[2] != t.getDate())
{
//alert('��������ڸ�ʽ����ʽΪ��YYYY-MM-DD��YYYY/MM/DD��ע�����ꡣ');
return false;
}
}
else
{
//alert('��������ڸ�ʽ����ʽΪ��YYYY-MM-DD��YYYY/MM/DD��ע�����ꡣ');
return false;
}
return true;
}

//+---------------------------------------------------
//| ����ʱ����
//| ��ʽΪ��YYYY-MM-DD HH:MM:SS
//+---------------------------------------------------
function CheckDateTime(str)
{
var reg = /^(\d{4,4})-(\d{1,2})-(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
var r = str.match(reg); 
if(r==null)return false;
r[2]=r[2]-1;
var d= new Date(r[1],r[2],r[3],r[4],r[5],r[6]);
if(d.getFullYear()!=r[1])return false;
if(d.getMonth()!=r[2])return false;
if(d.getDate()!=r[3])return false;
if(d.getHours()!=r[4])return false;
if(d.getMinutes()!=r[5])return false;
if(d.getSeconds()!=r[6])return false;
return true;
}
function CheckDate(str)
{
var reg = /^\d{2,4}-\d{1,2}-\d{1,2}$/; 
var rreg = str.match(reg); 
if(rreg==null) return false;
var r = str.split('-');
r[1]=r[1]-1;
var d= new Date(r[0],r[1],r[2]);
if(d.getFullYear()!=r[0])return false;
if(d.getMonth()!=r[1])return false;
if(d.getDate()!=r[2])return false;
return true;
}
//+---------------------------------------------------
//| �����ڷָ������
//+---------------------------------------------------
Date.prototype.toArray = function()
{
var myDate = this;
var myArray = Array();
myArray[0] = myDate.getFullYear();
myArray[1] = myDate.getMonth();
myArray[2] = myDate.getDate();
myArray[3] = myDate.getHours();
myArray[4] = myDate.getMinutes();
myArray[5] = myDate.getSeconds();
return myArray;
}

//+---------------------------------------------------
//| ȡ������������Ϣ
//| ���� interval ��ʾ��������
//| y �� m�� d�� w���� ww�� hʱ n�� s��
//+---------------------------------------------------
Date.prototype.DatePart = function(interval)
{
var myDate = this;
var partStr='';
var Week = ['��','һ','��','��','��','��','��'];
switch (interval)
{
case 'y' :partStr = myDate.getFullYear();break;
case 'm' :partStr = myDate.getMonth()+1;break;
case 'd' :partStr = myDate.getDate();break;
case 'w' :partStr = Week[myDate.getDay()];break;
case 'ww' :partStr = myDate.WeekNumOfYear();break;
case 'h' :partStr = myDate.getHours();break;
case 'n' :partStr = myDate.getMinutes();break;
case 's' :partStr = myDate.getSeconds();break;
}
return partStr;
}

//+---------------------------------------------------
//| ȡ�õ�ǰ���������µ��������
//+---------------------------------------------------
Date.prototype.MaxDayOfDate = function()
{
var myDate = this;
var ary = myDate.toArray();
var date1 = (new Date(ary[0],ary[1]+1,1));
var date2 = date1.DateAdd('m',1);
var result = myDate.DateDiff(date1.Format('yyyy-MM-dd'),date2.Format('yyyy-MM-dd'));
return result;
}

//+---------------------------------------------------
//| ȡ�õ�ǰ������������һ���еĵڼ���
//+---------------------------------------------------
Date.prototype.WeekNumOfYear = function()
{
var myDate = this;
var ary = myDate.toArray();
var year = ary[0];
var month = ary[1]+1;
var day = ary[2];
document.write('< script language=VBScript\> \n');
document.write('myDate = DateValue('+month+'-'+day+'-'+year+') \n');
document.write('result = DatePart('+ww+', myDate) \n');
document.write(' \n');
return result;
}

//+---------------------------------------------------
//| �ַ���ת����������
//| ��ʽ MM/dd/YYYY MM-dd-YYYY YYYY/MM/dd YYYY-MM-dd
//+---------------------------------------------------
function StringToDate(DateStr)
{

var converted = Date.parse(DateStr);
var myDate = new Date(converted);
if (isNaN(myDate))
{
//var delimCahar = DateStr.indexOf('/')!=-1?'/':'-';
var arys= DateStr.split('-');
myDate = new Date(arys[0],--arys[1],arys[2]);
}
return myDate;
}

function utcToDate(utcCurrTime) {   
//Fri Oct 31 18:00:00 UTC+0800 2008   
utcCurrTime = utcCurrTime + "";  
var date="";  
var month=new Array();  
month["Jan"]=1;
month["Feb"]=2;
month["Mar"]=3;
month["Apr"]=4;
month["May"]=5;
month["Jun"]=6;  
month["Jul"]=7;
month["Aug"]=8;
month["Sep"]=9;
month["Oct"]=10;
month["Nov"]=11;
month["Dec"]=12; 
var week=new Array();  
week["Mon"]="һ";
week["Tue"]="��";
week["Wed"]="��";
week["Thu"]="��";
week["Fri"]="��";
week["Sat"]="��";
week["Sun"]="��";
str = utcCurrTime.split(" ");  
date = str[5]+"-";  
date = date + month[str[1]] + "-" + str[2] + "-" + str[3];  
//date=date+" ��"+week[str[0]];   
return date;  
}