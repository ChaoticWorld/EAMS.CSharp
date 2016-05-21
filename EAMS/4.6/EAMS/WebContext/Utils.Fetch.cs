using System;
using System.Web;
using System.Configuration;


namespace WebCommon
{
	/// <summary>
	/// �Գ��÷������½��з�װ������ȡһЩ���û�������
	/// </summary>
	public class Fetch
	{
		#region Fields
		private static readonly string mTablePrefix;
		public static readonly string SiteMapPath;
		#endregion

		#region ���캯��,��ʼ������ǰ׺�ȡ�
		static Fetch()
		{
			mTablePrefix = ApplicationSettings.Get("TablePrefix");
			if (null == mTablePrefix || string.Empty == mTablePrefix)
			{
				mTablePrefix = "ql_";
			}
			else
			{
				mTablePrefix = mTablePrefix.Replace("'", "''");
			}
			SiteMapPath = Fetch.MapPath(".");
		}
		#endregion

        #region �ж�ҳ�����ֵ������ֵ�ж�
        /// <summary>
        /// ��ȡҳ���ύ�Ĳ���ֵ���൱if else �ж�
        /// </summary>
        public static string GetJudge(string strjudge, string strright, string strerror)
        {
            return (strjudge == "1" )? "<font color=\"red\">"+strright+"</font>" : strerror;
        }
        #endregion
		#region ��ȡҳ���ύ�Ĳ���ֵ���൱�� Request.Form public static string Post(string name)
		/// <summary>
		/// ��ȡҳ���ύ�Ĳ���ֵ���൱�� Request.Form
		/// </summary>
		public static string Post(string name)
		{
			string value = HttpContext.Current.Request.Form[name];
			return value == null ? string.Empty : value.Trim();
		}
		#endregion

		#region ��ȡҳ���ַ�Ĳ���ֵ���൱�� Request.QueryString public static string Get(string name)
		/// <summary>
		/// ��ȡҳ���ַ�Ĳ���ֵ���൱�� Request.QueryString
		/// </summary>
		public static string Get(string name)
		{
			string value = HttpContext.Current.Request.QueryString[name];
			return value == null ? string.Empty : value.Trim();
		}
		#endregion

		#region ��ȡҳ���ַ�Ĳ���ֵ����鰲ȫ�ԣ��൱�� Request.QueryString public static string Get(string name, CheckGetEnum chkType)
		/// <summary>
		/// ��ȡҳ���ַ�Ĳ���ֵ����鰲ȫ�ԣ��൱�� Request.QueryString
		/// chkType �� CheckGetEnum.Int�� CheckGetEnum.Safety�������ͣ�
		/// CheckGetEnum.Int ��֤������������
		/// CheckGetEnum.Safety ��֤�ύ�Ĳ���ֵû�в������ݿ�����
		/// </summary>
		public static string Get(string name, CheckGetEnum chkType)
		{
			string value = Get(name);
			bool isPass  = false;
			switch (chkType)
			{
				default:
					isPass = true;
					break;
				case CheckGetEnum.Int:
				{
					try
					{
						int.Parse(value);
						isPass = RegExp.IsNumeric(value);
					}
					catch
					{
						isPass = false;
					}
					break;
				}
				case CheckGetEnum.Safety:
					isPass = RegExp.IsSafety(value);
					break;
			}
			if (!isPass)
			{
				new Terminator().Throw("��ַ���в�����" + name + "����ֵ������Ҫ������Ǳ����в���벻Ҫ�ֶ��޸�URL��");
				return string.Empty;
			}
			return value;
		}
		#endregion
		
		#region ���ٵ������һ������ public static void  Trace(object obj)
		/// <summary>
		/// ���ٵ������һ������
		/// </summary>
		public static void  Trace(object obj)
		{
			HttpContext.Current.Response.Write(obj.ToString());
		}
		#endregion

		#region ����Ե�ַת��Ϊ���Ե�ַ����һ����װ���Ż� Server.MapPath public static string MapPath(string path)
		/// <summary>
		/// ����Ե�ַת��Ϊ���Ե�ַ����һ����װ���Ż� Server.MapPath
		/// </summary>
		public static string MapPath(string path)
		{
			if (RegExp.IsPhysicalPath(path))
			{
				return path;
			}

			if (null != HttpContext.Current)
			{
				if (!RegExp.IsRelativePath(path))
				{
					return HttpContext.Current.Server.MapPath(path);
				}
#if DEBUG
				if (null == HttpContext.Current)
				{
					throw new Exception("HttpContext.Current Ϊ������");
				}
#endif
				return HttpContext.Current.Server.MapPath(PathUpSeek + "/" + path);
			}
			else if (SiteMapPath.Length > 0)
			{
				return Text.JoinString(SiteMapPath + "/" + path);
			}
			else
			{
				throw new Exception("HttpContext.Current Ϊ������");
			}
		}
		#endregion

		#region IP ��ַ�ַ�����ʽת���ɳ����� public static long Ip2Int(string ip)
		/// <summary>
		/// IP ��ַ�ַ�����ʽת���ɳ�����
		/// </summary>
		public static long Ip2Int(string ip)
		{
			if (!RegExp.IsIp(ip))
			{
				return -1;
			}
			string[] arr = ip.Split('.');
			long lng = long.Parse(arr[0]) * 16777216;
			lng += int.Parse(arr[1]) * 65536;
			lng += int.Parse(arr[2]) * 256;
			lng += int.Parse(arr[3]);
			return lng;
		}
		#endregion

		#region ��ȡ�ͻ���IP  public static string UserIp
		/// <summary>
		/// ��ȡ�ͻ���IP 
		/// </summary>
		public static string UserIp
		{
			get 
			{
				string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				if (ip == null || ip == string.Empty)
				{
					ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
				}
				if (!RegExp.IsIp(ip))
				{
					return "Unknown";
				}
				return ip;
			}
		}
		#endregion

		#region  ��ȡ��������ʹ�õ�������� public static string UserBrowser
		/// <summary>
		/// ��ȡ��������ʹ�õ��������
		/// </summary>
		public static string UserBrowser
		{
			get 
			{
				string agent = HttpContext.Current.Request.UserAgent;
				if (agent == null || agent == string.Empty)
					return "Unknown";
				agent = agent.ToLower();

				HttpBrowserCapabilities  bc = HttpContext.Current.Request.Browser;
				if (agent.IndexOf("firefox") >= 0 
					|| agent.IndexOf("firebird") >= 0
					|| agent.IndexOf("myie") >= 0
					|| agent.IndexOf("opera") >= 0
					|| agent.IndexOf("netscape") >= 0
					||	agent.IndexOf("msie") >= 0 
					)		
					return bc.Browser + bc.Version;

				return "Unknown";
			}
		}
		#endregion
 
		#region �����վ���� 	public static string ServerDomain
		/// <summary>
		/// �����վ����
		/// </summary>
		public static string ServerDomain
		{
			get 
			{
				string host = HttpContext.Current.Request.Url.Host.ToLower();
				string[] arr = host.Split('.');
				if (arr.Length < 3 || RegExp.IsIp(host))
				{
					return host;
				}
				string domain = host.Remove(0, host.IndexOf(".") + 1);
				if (domain.StartsWith("com.") || domain.StartsWith("net.") || domain.StartsWith("org.") || domain.StartsWith("gov."))
				{
					return host;
				}
				return domain;
			}
		}
		#endregion

		#region ��õ�ǰ·��
		/// <summary>
		/// ��õ�ǰ·��
		/// </summary>
		public static string CurrentPath
		{
			get 
			{
				string path = HttpContext.Current.Request.Path;
				path = path.Substring(0, path.LastIndexOf("/"));
				if (path == "/")
				{
					return string.Empty;
				}
				return path;
			}
		}
		#endregion

		#region �����վ�����Ŀ¼ 	public static string PathUpSeek
		/// <summary>
		/// �����վ�����Ŀ¼
		/// </summary>
		public static string PathUpSeek
		{
			get 
			{
				string currentPath = CurrentPath;
				string lower_current_path = currentPath.ToLower();

				
#if DEBUG
				string[] arr = (Text.JoinString(ApplicationSettings.Get("PathUpSeek"), "|/install|/upgrade|/test")).ToLower().Split('|');
#else
				string[] arr = (Text.JoinString(ApplicationSettings.Get("PathUpSeek"), "|/install|/upgrade")).ToLower().Split('|');
#endif
				

				foreach (string s in arr)
				{
					if (null == s || 0 == s.Length)
					{
						continue;
					}
					if (s[0] != '/')
					{
						continue;
					}
					if (lower_current_path.EndsWith(s))
					{
						return currentPath.Remove(currentPath.Length - s.Length, s.Length);
					}
				}

				int indexof = currentPath.IndexOf("/templates/");
				if (-1 != indexof)
				{
					return currentPath.Substring(0, indexof);
				}
				return currentPath;
			}
		}
		#endregion

		#region ��õ�ǰURL public static string CurrentUrl
		/// <summary> 
		/// ��õ�ǰURL
		/// </summary>
		public static string CurrentUrl
		{
			get 
			{
				return HttpContext.Current.Request.Url.ToString();
			}
		}
		#endregion

		#region ��ȡ��ǰ�����ԭʼURL 
		/// <summary>
		/// ��ȡ��ǰ�����ԭʼ URL
		/// </summary>
		public static string webCurrentUrl
		{
			get
			{
				return HttpContext.Current.Request.RawUrl.ToString();
			}
		}
		#endregion

		#region �����ԴURL public static string Referrer
		/// <summary>
		/// �����ԴURL
		/// </summary>
		public static string Referrer
		{
			get 
			{
				Uri uri = HttpContext.Current.Request.UrlReferrer;
				if (uri == null)
				{
					return string.Empty;
				}
				return Convert.ToString(uri);
			}
		}
		#endregion

		#region �Ƿ�������������������� 	public static bool IsPostFromAnotherDomain
		/// <summary>
		/// �Ƿ��������������������
		/// </summary>
		public static bool IsPostFromAnotherDomain
		{
			get 
			{
				if (HttpContext.Current.Request.HttpMethod == "GET")
				{
					return false;
				}
				return Referrer.IndexOf(ServerDomain) == -1;
			}
		}
		#endregion

		#region �Ƿ������������������POST��ʽ�ύ�� 	public static bool IsGetFromAnotherDomain
		/// <summary>
		/// �Ƿ������������������POST��ʽ�ύ��
		/// </summary>
		public static bool IsGetFromAnotherDomain
		{
			get 
			{
				if (HttpContext.Current.Request.HttpMethod == "POST")
				{
					return false;
				}
				return Referrer.IndexOf(ServerDomain) == -1;
			}
		}
		#endregion
 
		#region �Ƿ��ж�Ϊ������ public static bool IsRobots
		/// <summary>
		/// �Ƿ��ж�Ϊ������ 
		/// </summary>
		public static bool IsRobots
		{
			get 
			{
				return IsWebSearch();
			}
		}
		#endregion

        #region ����������Դ�ж� public static bool IsWebSearch()
		
		/// <summary>
		/// ����������Դ�ж�
		/// </summary>
		private static string[] _WebSearchList = new string[]{"google", "isaac", "surveybot", "baiduspider", "yahoo", "yisou", "3721", "qihoo", "daqi", "ia_archiver", "p.arthur", "fast-webcrawler", "java", "microsoft-atl-native", "turnitinbot", "webgather", "sleipnir", "msn"};
		public static bool IsWebSearch()
		{
			string user_agent = HttpContext.Current.Request.UserAgent;
			if (null == user_agent || string.Empty == user_agent)
			{
				return true;
			}
			else
			{
				user_agent = user_agent.ToLower();
			}

			for (int i = 0; i < _WebSearchList.Length; i++)
			{
				if (-1 != user_agent.IndexOf(_WebSearchList[i]))
				{
					return true;
				}
			}
			return UserBrowser.Equals("Unknown");
		}
		#endregion
		
		#region Url���룬�൱�� Server.UrlEncode public static string UrlEncode(string url)
		/// <summary>
		/// Url���룬�൱�� Server.UrlEncode
		/// </summary>
		public static string UrlEncode(string url)
		{
			return HttpContext.Current.Server.UrlEncode(url);
		}
		#endregion

		#region ����
		/// <summary>
		/// ����ǰ׺
		/// </summary>
		public static string TablePrefix
		{
			get
			{
				return mTablePrefix;
			}
		}

		
		

	
		/// <summary>
		/// ��ȡ��֤���ļ��ĵ�ַ
		/// </summary>
		public static string ValidateCodeFileName
		{
			get
			{
				string file_name = Config.Settings["ValidateCodeFileName"];
				if (null == file_name || 0 == file_name.Length)
				{
					file_name = "image.aspx";
				}
				return file_name;
			}
		}
		#endregion
	
	}
}