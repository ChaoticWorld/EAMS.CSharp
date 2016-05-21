using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
namespace WebCommon
{
   public abstract class WebControlsUI
	{
        #region 设置或者得到CheckBoxList选中了的值
        /// <summary>
        /// 初始化CheckBoxList中哪些是选中了的         /// </summary>
        /// <param name="checkList">CheckBoxList</param>
        /// <param name="selval">选中了的值串例如："0,1,1,2,1"</param>
        /// <param name="separator">值串中使用的分割符例如"0,1,1,2,1"中的逗号</param>
        public static string SetChecked(CheckBoxList checkList,string selval,string separator)
        {
            selval = separator + selval + separator;        //例如："0,1,1,2,1"->",0,1,1,2,1,"
            for(int i=0; i<checkList.Items.Count; i++)
            {
                checkList.Items[i].Selected = false;
                string val = separator + checkList.Items[i].Value + separator;
                if(selval.IndexOf(val)!=-1)
                {
                    checkList.Items[i].Selected = true;
                    selval = selval.Replace(val,separator);        //然后从原来的值串中删除已经选中了的
                    if(selval == separator)        //selval的最后一项也被选中的话，此时经过Replace后，只会剩下一个分隔符
                    {        
                        selval += separator;        //添加一个分隔符
                    }
                }
            }
            selval = selval.Substring(1,selval.Length-2);        //除去前后加的分割符号
            return selval;
        }

        /// <summary>
        /// 得到CheckBoxList中选中了的值
        /// </summary>
        /// <param name="checkList">CheckBoxList</param>
        /// <param name="separator">分割符号</param>
        /// <returns></returns>
        public static string GetChecked(CheckBoxList checkList, string separator)
        {
            string selval = "";
            for(int i=0;i<checkList.Items.Count;i++)
            {
                if(checkList.Items[i].Selected)
                {
                    selval += checkList.Items[i].Value + separator;
                }
            }
            return selval;
        }
        #endregion 
	}
}
