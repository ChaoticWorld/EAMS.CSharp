

using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Configuration;

namespace WebCommon
{
	/// <summary>
	/// �����ı�������
	/// </summary>
	public class Text 
	{
		#region 32 λ MD5 ���� public static string MD5(string s)
		/// <summary>
		/// 32 λ MD5 ���� 
		/// </summary>
		public static string MD5(string s)
		{
			if (null == System.Web.HttpContext.Current)
			{
				if (null == s || 0 == s.Length)
				{
					s = string.Empty;
				}

				return FormsAuthentication.HashPasswordForStoringInConfigFile(s, "MD5").ToLower();
			}
			else
			{
				System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
				byte[] buffer = System.Web.HttpContext.Current.Request.ContentEncoding.GetBytes(s);
				buffer= md5.ComputeHash(buffer);
				return System.BitConverter.ToString(buffer).Replace("-", "").ToLower();
			}
		}
		
		/// <summary>
		/// ��ȡDVģʽ��MD5����
		/// </summary>
		public static string MD5(string s,int start,int length)
		{
			return Text.MD5(s).Substring(start, length);
		}
		#endregion

		#region �� 32 λ MD5 ���ܼ�CookieToken��׺����ʽ���� cookie ���� public static string GenerateToken(string s)
		/// <summary>
		/// �� 32 λ MD5 ���ܼ�CookieToken��׺����ʽ���� cookie ����
		/// </summary>
		public static string GenerateToken(string s)
		{
			if (null == s || 0 == s.Length)
			{
				s = string.Empty;
			}

			return MD5(s + ApplicationSettings.Get("CookieToken"));
		}
		#endregion

		#region ����Ƚϣ�֧�� 32λ��16λ����Ƚ� public static bool ComparePassword(string pwd1, string pwd2)

		/// <summary>
		/// ����Ƚϣ�֧�� 32λ��16λ����Ƚ�
		/// </summary>
		public static bool ComparePassword(string pwd1, string pwd2)
		{
			
			if (null == pwd1 && null == pwd2)
			{
				return true;
			}
			else if (null == pwd1 || null == pwd2)
			{
				return false;
			}

			int len1 = pwd1.Length, len2 = pwd2.Length;
			if (len1 == len2 && 0 != len1)
			{
				// ִ�в����ִ�Сд�ļ��
				return (0 == string.Compare(pwd1, pwd2, true));
			}
			else if (32 == len1 && 16 == len2)
			{
				// ִ�в����ִ�Сд�ļ��
				return (0 == string.Compare(pwd1.Substring(8, 16), pwd2, true));
			}
			else if (16 == len1 && 32 == len2)
			{
				// ִ�в����ִ�Сд�ļ��
				return (0 == string.Compare(pwd2.Substring(8, 16), pwd1, true));
			}
			

			return false;
		}
		#endregion
 		
		#region Ϊ�ַ�����0  public static string AddZero(int i)
		/// <summary>
		/// add zero
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public static string AddZero(int i)
		{
			return (i > 9 ? string.Empty : "0") + i;
		}
		#endregion

		#region ����� sql �ı����Խ��ܵĸ�ʽ
		/// <summary>
		/// ����� sql �ı����Խ��ܵĸ�ʽ
		/// </summary>
		public static string SqlEncode(string s)
		{
			if (null == s || 0 == s.Length)
			{
				return string.Empty;
			}

			return s.Trim().Replace("'", "''");
		}
		#endregion
		
		#region ������� public static string ShitEncode(string s)
		/// <summary>
		/// ������ʣ�Ĭ����Ϊ�����|����|����|��b|���|fuck|shit|����|����|�Ҳ١�
		/// ������ bbs.config ������ BadWords ��ֵ
		/// </summary>
		public static string ShitEncode(string s)
		{
			string bw = Config.Settings["BadWords"];
			if (bw == null || 0 == bw.Length)
			{
				bw = "���|����|����|��b|���|fuck|shit|����|����|�Ҳ�";
			}
			else
			{
				bw = Regex.Replace(bw, @"\|{2,}", "|");					
				bw = Regex.Replace(bw, @"(^\|)|(\|$)", string.Empty);
			}
			return Regex.Replace(s, bw, "**", RegexOptions.IgnoreCase);
		}
		#endregion

		#region �ı����� 	public static string TextEncode(string s)
		/// <summary>
		/// �ı�����
		///</summary>
		public static string TextEncode(string s)
		{
			if (null == s || 0 == s.Length)
			{
				return string.Empty;
			}

			StringBuilder sb = new StringBuilder(s);
			sb.Replace("&", "&amp;");
			sb.Replace("<", "&lt;");
			sb.Replace(">", "&gt;");
			sb.Replace("\"", "&quot;");
			sb.Replace("\'", "&#39;");
			return ShitEncode(sb.ToString());
		}
		#endregion

		#region HTML ���� public static string HtmlEncode(string s)
		/// <summary>
		/// HTML ����
		///</summary>
		public static string HtmlEncode(string s)
		{
			return HtmlEncode(s, false);
		}

		public static string HtmlEncode(string s, bool bln)
		{
			if (null == s || 0 == s.Length)
			{
				return s;
			}

			StringBuilder sb = new StringBuilder(s);
			sb.Replace("&", "&amp;");
			sb.Replace("<", "&lt;");
			sb.Replace(">", "&gt;");
			sb.Replace("\"", "&quot;");
			sb.Replace("\'", "&#39;");

			if (bln)
			{
				return ShitEncode(sb.ToString());
			}
			else
			{
				return sb.ToString();
			}
		}
		#endregion

		#region HTML ���� public static string HtmlDecode(string s)
		/// <summary>
		/// HTML ����
		///</summary>
		public static string HtmlDecode(string s)
		{
			StringBuilder sb = new StringBuilder(s);
			sb.Replace("&amp;", "&");
			sb.Replace("&lt;", "<");
			sb.Replace("&gt;", ">");
			sb.Replace("&quot;", "\"");
			sb.Replace("&#39;", "'");

			return sb.ToString();
		}
		#endregion

		#region TEXT����		public static string TextDecode(string s)
		/// <summary>
		/// TEXT����
		/// </summary>
		public static string TextDecode(string s)
		{
			StringBuilder sb=new StringBuilder(s);
			sb.Replace("<br/><br/>","\r\n");
			sb.Replace("<br/>","\r");
			sb.Replace("<p></p>","\r\n\r\n");
			return sb.ToString();
		}
		#endregion

		#region �ַ����ĳ��� public static int Len(string s)
		/// <summary>
		/// �ַ����ĳ���
		/// </summary>
		public static int Len(string s)
		{
			return HttpContext.Current.Request.ContentEncoding.GetByteCount(s);
		}
		#endregion

		#region �ض��ַ��� public static string Left(string s, int need, bool encode)
		/// <summary>
		/// �ض��ַ��������str �ĳ��ȳ��� need������ȡ str ��ǰ need ���ַ�������β���� ��...��
		/// </summary>
		public static string Left(string s, int need, bool encode)
		{
			if (s == null || s == "")
			{
				return string.Empty;
			}

			int len = s.Length;
			if (len < need / 2)
			{
				return encode ? TextEncode(s) : s;
			}

			int i, j, bytes = 0;
			for (i=0; i<len; i++)
			{
				bytes += RegExp.IsUnicode(s[i].ToString()) ? 2 : 1;
				if (bytes >= need)
				{
					break;
				}
			}

			string result = s.Substring(0, i);
			if (len > i)
			{
				for (j=0; j<5; j++)
				{
					bytes -= RegExp.IsUnicode(s[i-j].ToString()) ? 2 : 1;
					if (bytes <= need)
					{
						break;
					}
				}
				result = s.Substring(0, i-j) + "...";
			}
			return encode ? TextEncode(result) : result;
		}
		#endregion

		#region Email ���� public static string EmailEncode(string s)
		/// <summary>
		/// Email ����
		/// </summary>
		public static string EmailEncode(string s)
		{
			string email = (TextEncode(s)).Replace("@", "&#64;").Replace(".", "&#46;");

			return JoinString("<a href='mailto:", email, "'>", email, "</a>");
		}
		#endregion

		#region ��Ч�ַ������Ӳ��� public static string JoinString(params string[] value)
		/// <summary>
		/// ��Ч�ַ������Ӳ�����
		/// </summary>
		/// <param name="value">Ҫ���ӵ��ַ���</param>
		/// <returns>���Ӻ���ַ���</returns>
		public static string JoinString(params string[] value)
		{
			if (null == value)
			{
				throw new System.ArgumentNullException("value");
			}
			if (0 == value.Length)
			{
				return string.Empty;
			}
			return string.Join(string.Empty, value);
		}
		#endregion

		#region �ж�һ���ַ����Ƿ���һ���ɶ��ŷָ��������б� public static bool IsNumberList(string str)
		/// <summary>
		/// �ж�һ���ַ����Ƿ���һ���ɶ��ŷָ��������б�
		/// </summary>
		/// <param name="str">��Ҫ�жϵ��ַ���</param>
		/// <returns></returns>
		public static bool IsNumberList(string str)
		{
			return IsNumberList(str, ',');
		}

		/// <summary>
		/// �ж�һ���ַ����Ƿ���һ����ָ�����ַ��ָ��������б�
		/// </summary>
		/// <param name="str"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static bool IsNumberList(string str, char separator)
		{
			if (null == str)
			{
				return false;
			}
			int len = str.Length;
			if (0 == len)
			{
				return false;
			}
			if (!char.IsNumber(str[0]) || !char.IsNumber(str[len - 1]))
			{
				return false;
			}
			len--;
			for (int i = 1; i < len; i++)
			{
				if (separator == str[i])
				{
					if (!char.IsNumber(str[i-1]) || !char.IsNumber(str[i+1]))
					{
						return false;
					}
				}
				else if (!char.IsNumber(str[i]))
				{
					return false;
				}
			}
			return true;
		}
		#endregion

		#region �ж�Ŀ������Ƿ���������� public static bool IsNumeric(object value)
		/// <summary>
		/// �ж�Ŀ������Ƿ����������
		/// </summary>
		/// <param name="value">Ҫ�жϵ�Ŀ�����</param>
		/// <returns>�жϳɹ����� true �� ���򷵻� false ��</returns>
		public static bool IsNumeric(object value)
		{
			if (null == value)
			{
				return false;
			}
			return IsNumeric(value.ToString());
		}
		#endregion
 
		#region �ж��ַ����Ƿ���������� 	public static bool IsNumeric(string value)
		/// <summary>
		/// �ж��ַ����Ƿ����������
		/// </summary>
		/// <param name="value">Ҫ�жϵ��ַ�����</param>
		/// <returns>�жϳɹ����� true �� ���򷵻� false ��</returns>
		public static bool IsNumeric(string value)
		{
			if (null == value)
			{
				return false;
			}
			int len = value.Length;
			if (0 == len)
			{
				return false;
			}
			if ('-' != value[0] && !char.IsNumber(value[0]))
			{
				return false;
			}
			for (int i = 1; i < len;i ++)
			{
				if (!char.IsNumber(value[i]))
				{
					return false;
				}
			}
			return true;
		}
		#endregion
 
		#region JavaScript ���� public static string JavaScriptEncode(string str)
		/// <summary>
		/// JavaScript ����
		/// </summary>
		public static string JavaScriptEncode(string str)
		{
			StringBuilder sb = new StringBuilder(str);
			sb.Replace("\\", "\\\\");
			sb.Replace("\r", "\\0Dx");
			sb.Replace("\n", "\\x0A");
			sb.Replace("\"", "\\x22");
			sb.Replace("\'", "\\x27");
			return sb.ToString();
		}
		#endregion

		#region JavaScript ���� public static string JavaScriptEncode(object obj)
		/// <summary>
		/// JavaScript ����
		/// </summary>
		public static string JavaScriptEncode(object obj)
		{
			if (null == obj)
			{
				return string.Empty;
			}
			return JavaScriptEncode(obj.ToString());
		}
		#endregion

		#region ȥ�� htmlCode �����е�HTML��ǩ(������ǩ�е�����) public static string StripHtml(string htmlCode)
		/// <summary>
		/// ȥ�� htmlCode �����е�HTML��ǩ(������ǩ�е�����)
		/// </summary>
		/// <param name="htmlCode">���� HTML ������ַ�����</param>
		/// <returns>����һ�������� HTML ������ַ���</returns>
		public static string StripHtml(string htmlCode)
		{
			if (null == htmlCode || 0 == htmlCode.Length)
			{
				return string.Empty;
			}
			return Regex.Replace(htmlCode, @"<[^>]+>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);
		}
		#endregion
		

	}
}