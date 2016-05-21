using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

namespace WebCommon
{
	/// <summary>
	/// �̳���: System.Web.UI.Page
	/// ��ȫ��:  �� WebCommon.Origin
	/// ������:    �� Qilu168.bbs.Kernel.Main
	///������        �� Qilu168.bbs.UI.Xxx (����ҳ��ĺ�̨����)
	/// ˵����: ����̳�System.Web.UI.Page����������UIҳ�棬������һЩ�����õ����Ժͷ�����������²���
	///������   �����������ݾ���public
	///�������� һЩ�����õķ���������δ�г�
	/// </summary>
	public class Origin : System.Web.UI.Page
	{	
		#region Properties
		/// <summary>
		/// ��ǰ׺
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
		/// ��ǰ·��
		/// </summary>
		protected virtual string currentPath
		{
			get
			{
				return Fetch.CurrentPath;
			}
		}
		#endregion

		#region ����� sql �ı���ʽ protected virtual string SqlEncode(string str)
		/// <summary>
		/// ����� sql �ı���ʽ
		/// </summary>
		protected virtual string SqlEncode(string str)
		{
			return str.Trim().Replace("'", "''");
		}
		#endregion

		#region ������� protected virtual string ShitEncode(string str)
		/// <summary>
		/// �������
		/// </summary>
		protected virtual string ShitEncode(string str)
		{
			string bw = ApplicationSettings.Get("BadWords");
			if (bw == null || bw == string.Empty)
			{
				bw = "���|����|����|��b|���|����|�Ҳ�|����|fuck|shit";
			}
			else
			{
				bw = Regex.Replace(bw, @"\|{2,}", "|");
				bw = Regex.Replace(bw, @"(^\|)|(\|$)", "");
			}
			return Regex.Replace(str, bw, "**", RegexOptions.IgnoreCase);
		}
		#endregion

		#region Url��ַ���� protected virtual string UrlEncode(string str)
		/// <summary>
		/// Url��ַ����
		/// </summary>
		protected virtual string UrlEncode(string str)
		{
			return HttpContext.Current.Server.UrlEncode(str);
		}
		#endregion
 
		#region �ı����� protected virtual string TextEncode(string str)
		/// <summary>
		/// �ı�����
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

		#region HTML ���� protected virtual string HtmlEncode(string str)
		/// <summary>
		/// HTML ����
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

		#region JavaScript ���� protected virtual string JavaScriptEncode(string str)
		/// <summary>
		/// JavaScript ����
		/// </summary>
		protected virtual string JavaScriptEncode(string str)
		{
			return Text.JavaScriptEncode(str);
		}
		#endregion
		
		#region �ַ����ĳ���
		/// <summary>
		/// �ַ����ĳ���
		/// </summary>
		protected virtual int Len(string str)
		{
			return Encoding.GetEncoding(936).GetByteCount(str);
		}
		#endregion

		#region �ض��ַ��� protected virtual string Left(string str, int need, bool encode)
		/// <summary>
		/// �ض��ַ��������str �ĳ��ȳ��� need������ȡ str ��ǰ need ���ַ�������β���� ��...��
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

		#region Email ���� protected virtual string EmailEncode(string str)
		/// <summary>
		/// Email ����
		/// </summary>
		protected virtual string EmailEncode(string str)
		{
			string email = (TextEncode(str)).Replace("@", "&#64;").Replace(".", "&#46;");

			return Text.JoinString("<a href='mailto:", email, "'>", email, "</a>");
		}
		#endregion
 
		#region url���� protected virtual string UrlEncode(object obj)
		/// <summary>
		/// url����
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual string UrlEncode(object obj)
		{
			return UrlEncode(obj.ToString());
		}
		#endregion

		#region �ı����� protected virtual string TextEncode(object obj)
		/// <summary>
		/// �ı�����
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual string TextEncode(object obj)
		{
			return TextEncode(obj.ToString());
		}
		#endregion

		#region html���� protected virtual string HtmlEncode(object obj)
		/// <summary>
		/// html����
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual string HtmlEncode(object obj)
		{
			return HtmlEncode(obj.ToString());
		}
		#endregion

		#region javascript���� protected virtual string JavaScriptEncode(object obj)
		/// <summary>
		/// javascript����
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual string JavaScriptEncode(object obj)
		{
			return JavaScriptEncode(obj.ToString());
		}
		#endregion
		
		#region email���� protected virtual string EmailEncode(object obj)
		/// <summary>
		/// email����
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual string EmailEncode(object obj)
		{
			return EmailEncode(obj.ToString());
		}
		#endregion

		#region �и� protected virtual string Left(string str, int i)
		/// <summary>
		/// �и�
		/// </summary>
		/// <param name="str"></param>
		/// <param name="i"></param>
		/// <returns></returns>
		protected virtual string Left(string str, int i)
		{
			return Left(str, i, true);
		}
		#endregion

		#region �и� protected virtual string Left(object obj, int i)
		/// <summary> 
		/// �и�
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="i"></param>
		/// <returns></returns>
		protected virtual string Left(object obj, int i)
		{
			return Left(obj.ToString(), i, true);
		}
		#endregion

		#region �и� protected virtual string Left(object obj, int i, bool encode)
		/// <summary>
		/// �и�
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

		#region �и� protected virtual string XLeft(string s, int need )
		/// <summary>
		/// �и�
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

		#region �и� protected virtual string XLeft(object s, int need )
		/// <summary>
		/// �и�
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

		#region �ж��ַ����Ƿ���������� protected virtual bool IsNumeric(string str)
		/// <summary>
		/// �ж��ַ����Ƿ����������
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

		#region �Ƿ������� protected virtual bool IsNumeric(object obj)
		/// <summary>
		/// �Ƿ�������
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual bool IsNumeric(object obj)
		{
			return IsNumeric(obj.ToString());
		}
		#endregion
		
		#region �ж��ַ����Ƿ�Ϸ������ڸ�ʽ protected virtual bool IsDate(string value)
		/// <summary>
		/// �ж��ַ����Ƿ�Ϸ������ڸ�ʽ
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

		#region �Ƿ������� protected virtual bool IsDate(object obj)
		/// <summary>
		/// �Ƿ�������
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected virtual bool IsDate(object obj)
		{
			return IsDate(obj.ToString());
		}
		#endregion
	
        #region ��ȡҳ���ύ�Ĳ���ֵ���൱�� Request.Form 	protected virtual string Post(string name)
		/// </summary>
		/// ��ȡҳ���ύ�Ĳ���ֵ���൱�� Request.Form
		/// </summary>
		protected virtual string Post(string name)
		{
			string str = HttpContext.Current.Request.Form[name];
			return str == null ? string.Empty : str.Trim();
		}
		#endregion
 
		#region  ��ȡҳ���ַ�Ĳ���ֵ���൱�� Request.QueryString protected virtual string Get(string name)
		/// </summary>
		/// ��ȡҳ���ַ�Ĳ���ֵ���൱�� Request.QueryString
		/// </summary>
		protected virtual string Get(string name)
		{
			return Fetch.Get(name);
		}
		#endregion

		#region ��ȡҳ���ַ�Ĳ���ֵ����鰲ȫ�� protected virtual string Get(string name, CheckGetEnum type)
		/// </summary>
		/// ��ȡҳ���ַ�Ĳ���ֵ����鰲ȫ�ԣ��൱�� Request.QueryString
		/// chkType �� CheckGetEnum.Int�� CheckGetEnum.Safety�������ͣ�
		/// CheckGetEnum.Int ��֤������������
		/// CheckGetEnum.Safety ��֤�ύ�Ĳ���ֵû�в������ݿ�����
		/// </summary>
		protected virtual string Get(string name, CheckGetEnum type)
		{
			return Fetch.Get(name, type);
		}
		#endregion

		#region �ظ�һ���ַ��� protected virtual string RepeatString(string input, int multiplier)
		/// </summary>
		/// �ظ�һ���ַ���
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
		
		#region ������ֹҳ�����
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