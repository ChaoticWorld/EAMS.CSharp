using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

namespace WebCommon
{
	/// <summary>
	/// 继承自: System.Web.UI.Page
	/// 类全名:  └ WebCommon.Origin
	/// 派生类:    └ Qilu168.bbs.Kernel.Main
	///　　　        └ Qilu168.bbs.UI.Xxx (所有页面的后台代码)
	/// 说　明: 该类继承System.Web.UI.Page并派生所有UI页面，包含了一些极常用的属性和方法，请见以下部分
	///　　　   本类所有内容均无public
	///　　　　 一些不常用的方法和属性未列出
	/// </summary>
	public class Origin : System.Web.UI.Page
	{	
		#region Properties
		/// <summary>
		/// 表前缀
		/// </summary>
 
		private static string mTablePrefix;
        public static string TablePrefix
        {
            get
            {
                return mTablePrefix;
            }
        }


		
		// ---------- Fetch --------------------------
		/// <summary>
		/// Is Robots
		/// </summary>
		protected virtual bool IsRobots
		{
			get
			{
				return Fetch.IsRobots;
			}
		}

		/// <summary>
		/// 当前路径
		/// </summary>
		protected virtual string currentPath
		{
			get
			{
				return Fetch.CurrentPath;
			}
		}
		#endregion

		#region 编码成 sql 文本格式 protected virtual string SqlEncode(string str)
		/// <summary>
		/// 编码成 sql 文本格式
		/// </summary>
		protected virtual string SqlEncode(string str)
		{
			return str.Trim().Replace("'", "''");
		}
		#endregion

		#region 过滤脏词 protected virtual string ShitEncode(string str)
		/// <summary>
		/// 过滤脏词
		/// </summary>
		protected virtual string ShitEncode(string str)
		{
			string bw = ApplicationSettings.Get("BadWords");
			if (bw == null || bw == string.Empty)
			{
				bw = "妈的|你妈|他妈|妈b|妈比|我日|我操|法轮|fuck|shit";
			}
			else
			{
				bw = Regex.Replace(bw, @"\|{2,}", "|");
				bw = Regex.Replace(bw, @"(^\|)|(\|$)", "");
			}
			return Regex.Replace(str, bw, "**", RegexOptions.IgnoreCase);
		}
		#endregion

		#region Url地址编码 protected virtual string UrlEncode(string str)
		/// <summary>
		/// Url地址编码
		/// </summary>
		protected virtual string UrlEncode(string str)
		{
			return HttpContext.Current.Server.UrlEncode(str);
		}
		#endregion
 
		#region 文本编码 protected virtual string TextEncode(string str)
		/// <summary>
		/// 文本编码
		/// </summary>
		protected virtual string TextEncode(string str)
		{
			StringBuilder sb = new StringBuilder(str);
			sb.Replace("&", "&amp;");
			sb.Replace("<", "&lt;");
			sb.Replace(">", "&gt;");
			sb.Replace("\"", "&quot;");
			sb.Replace("\'", "&#39;");
			return ShitEncode(sb.ToString());
		}
		#endregion

		#region HTML 编码 protected virtual string HtmlEncode(string str)
		/// <summary>
		/// HTML 编码
		/// </summary>
		protected virtual string HtmlEncode(string str)
		{
			StringBuilder sb = new StringBuilder(str);
			sb.Replace("&", "&amp;");
			sb.Replace("<", "&lt;");
			sb.Replace(">", "&gt;");
			sb.Replace("\"", "&quot;");
			sb.Replace("\'", "&#39;");
			sb.Replace("\t", "&nbsp; &nbsp; ");
			sb.Replace("\r", "");
			sb.Replace("\n", "<br />");
			return ShitEncode(sb.ToString());
		}
		#endregion

		#region JavaScript 编码 protected virtual string JavaScriptEncode(string str)
		/// <summary>
		/// JavaScript 编码
		/// </summary>
		protected virtual string JavaScriptEncode(string str)
		{
			return Text.JavaScriptEncode(str);
		}
		#endregion
		
		#region 字符串的长度
		/// <summary>
		/// 字符串的长度
		/// </summary>
		protected virtual int Len(string str)
		{
			return Encoding.GetEncoding(936).GetByteCount(str);
		}
		#endregion

		#region 截断字符串 protected virtual string Left(string str, int need, bool encode)
		/// <summary>
		/// 截断字符串，如果str 的长度超过 need，则提取 str 的前 need 个字符，并在尾部加 “...”
		/// </summary>
		protected virtual string Left(string str, int need, bool encode)
		{	
			if (str == null || str == string.Empty)
			{
				return string.Empty;
			}
			int len = str.Length;
			if (len < need / 2)
			{
				return encode ? TextEncode(str) : str;
			}
			int i, j, bytes = 0;
			for (i=0; i<len; i++)
			{
				bytes += RegExp.IsUnicode(str[i].ToString()) ? 2 : 1;
				if (bytes >= need)
				{
					break;
				}
			}
			string result = str.Substring(0, i);
			if (len > i)
			{
				for (j=0; j<5; j++)
				{	
					/*added by caoxin03@gmail.com*/
					if ( i-j >= str.Length || i-j < 0 ) 
					{
						-- j;
						break;
					}
					bytes -= (RegExp.IsUnicode(str[i-j].ToString()) ? 2 : 1);
					if (bytes <= need)
					{
						break;
					}
				}
				result = str.Substring(0, i-j) + "...";//
			}
			return encode ? TextEncode(result) : result;
		}
		#endregion

		#region Email 编码 protected virtual string EmailEncode(string str)
		/// <summary>
		/// Email 编码
		/// </summary>
		protected virtual string EmailEncode(string str)
		{
			string email = (TextEncode(str)).Replace("@", "&#64;").Replace(".", "&#46;");

			return Text.JoinString("<a href='mailto:", email, "'>", email, "</a>");
		}
		#endregion
 
		#region url编码 protected virtual string UrlEncode(object obj)
		/// <summary>
		/// url编码
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual string UrlEncode(object obj)
		{
			return UrlEncode(obj.ToString());
		}
		#endregion

		#region 文本编码 protected virtual string TextEncode(object obj)
		/// <summary>
		/// 文本编码
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual string TextEncode(object obj)
		{
			return TextEncode(obj.ToString());
		}
		#endregion

		#region html编码 protected virtual string HtmlEncode(object obj)
		/// <summary>
		/// html编码
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual string HtmlEncode(object obj)
		{
			return HtmlEncode(obj.ToString());
		}
		#endregion

		#region javascript编码 protected virtual string JavaScriptEncode(object obj)
		/// <summary>
		/// javascript编码
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual string JavaScriptEncode(object obj)
		{
			return JavaScriptEncode(obj.ToString());
		}
		#endregion
		
		#region email编码 protected virtual string EmailEncode(object obj)
		/// <summary>
		/// email编码
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual string EmailEncode(object obj)
		{
			return EmailEncode(obj.ToString());
		}
		#endregion

		#region 切割 protected virtual string Left(string str, int i)
		/// <summary>
		/// 切割
		/// </summary>
		/// <param name="str"></param>
		/// <param name="i"></param>
		/// <returns></returns>
		protected virtual string Left(string str, int i)
		{
			return Left(str, i, true);
		}
		#endregion

		#region 切割 protected virtual string Left(object obj, int i)
		/// <summary> 
		/// 切割
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="i"></param>
		/// <returns></returns>
		protected virtual string Left(object obj, int i)
		{
			return Left(obj.ToString(), i, true);
		}
		#endregion

		#region 切割 protected virtual string Left(object obj, int i, bool encode)
		/// <summary>
		/// 切割
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="i"></param>
		/// <param name="encode"></param>
		/// <returns></returns>
		protected virtual string Left(object obj, int i, bool encode)
		{
			return Left(obj.ToString(), i, encode);
		}
		#endregion

		#region 切割 protected virtual string XLeft(string s, int need )
		/// <summary>
		/// 切割
		/// </summary>
		/// <param name="s"></param>
		/// <param name="need"></param>
		/// <returns></returns>
		protected virtual string XLeft(string s, int need )
		{
			if (s == null || s == "")
			{
				return string.Empty;
			}
			if(s.Length < need)
				return s;
			string result = s.Substring(0, need);			
			return result+"...";
		}
		#endregion

		#region 切割 protected virtual string XLeft(object s, int need )
		/// <summary>
		/// 切割
		/// </summary>
		/// <param name="s"></param>
		/// <param name="need"></param>
		/// <returns></returns>
		protected virtual string XLeft(object s, int need )
		{
			return XLeft(s.ToString(),need );
		}
		#endregion

		// ---------- RegExp --------------------------

		#region 判断字符串是否由数字组成 protected virtual bool IsNumeric(string str)
		/// <summary>
		/// 判断字符串是否由数字组成
		/// </summary>
		protected virtual bool IsNumeric(string str)
		{
			
			if (null == str || 0 == str.Length)
			{
				return false;
			}
			if ('-' != str[0] && !char.IsNumber(str[0]))
			{
				return false;
			}

			int len = str.Length;
			for (int i = 1; i < len; i++)
			{
				if (!char.IsNumber(str[i]))
				{
					return false;
				}
			}
			return true;
		}
		#endregion

		#region 是否是数字 protected virtual bool IsNumeric(object obj)
		/// <summary>
		/// 是否是数字
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual bool IsNumeric(object obj)
		{
			return IsNumeric(obj.ToString());
		}
		#endregion
		
		#region 判断字符串是否合法的日期格式 protected virtual bool IsDate(string value)
		/// <summary>
		/// 判断字符串是否合法的日期格式
		/// </summary>
		protected virtual bool IsDate(string value)
		{
			try
			{
				System.DateTime.Parse(value);
			}
			catch
			{
				return false;
			}
			return true;
		}
		#endregion

		#region 是否是日期 protected virtual bool IsDate(object obj)
		/// <summary>
		/// 是否是日期
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual bool IsDate(object obj)
		{
			return IsDate(obj.ToString());
		}
		#endregion
	
        #region 获取页面提交的参数值，相当于 Request.Form 	protected virtual string Post(string name)
		/// </summary>
		/// 获取页面提交的参数值，相当于 Request.Form
		/// </summary>
		protected virtual string Post(string name)
		{
			string str = HttpContext.Current.Request.Form[name];
			return str == null ? string.Empty : str.Trim();
		}
		#endregion
 
		#region  获取页面地址的参数值，相当于 Request.QueryString protected virtual string Get(string name)
		/// </summary>
		/// 获取页面地址的参数值，相当于 Request.QueryString
		/// </summary>
		protected virtual string Get(string name)
		{
			return Fetch.Get(name);
		}
		#endregion

		#region 获取页面地址的参数值并检查安全性 protected virtual string Get(string name, CheckGetEnum type)
		/// </summary>
		/// 获取页面地址的参数值并检查安全性，相当于 Request.QueryString
		/// chkType 有 CheckGetEnum.Int， CheckGetEnum.Safety两种类型，
		/// CheckGetEnum.Int 保证参数是数字型
		/// CheckGetEnum.Safety 保证提交的参数值没有操作数据库的语句
		/// </summary>
		protected virtual string Get(string name, CheckGetEnum type)
		{
			return Fetch.Get(name, type);
		}
		#endregion

		#region 重复一个字符串 protected virtual string RepeatString(string input, int multiplier)
		/// </summary>
		/// 重复一个字符串
		/// </summary>
		protected virtual string RepeatString(string input, int multiplier)
		{
			if (multiplier <= 0)
			{
				return "";
			}
			if (null == input || string.Empty == input)
			{
				return "";
			}
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < multiplier; i++)
			{
				sb.Append(input);
			}
			return sb.ToString();
		}
		#endregion
		
		#region Validate RepeaterEventArgs
		/// <summary>
		/// Validate RepeaterEventArgs
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		protected virtual DataRowView GetDataRowViewFromRepeaterItemEventArgs(RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
			{
				return null;
			}
			return e.Item.DataItem as DataRowView;
		}

		protected virtual DataRow GetDataRowFromRepeaterItemEventArgs(RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
			{
				return null;
			}
			if (e.Item.DataItem is DataRow)
			{
				return e.Item.DataItem as DataRow;
			}
			else
			{
				return null;
			}
		}
		#endregion
		
		#region 正常终止页面程序
		private void Origin_Error(object sender, System.EventArgs e)
		{
			Dispose();
		}

		public override void Dispose()
		{
            base.Dispose();
		}
		#endregion
		
	}
}