using System.Web;
using System.Configuration;


namespace WebCommon
{
	/// <summary>
	/// ��Cookie�������з�װ
	/// </summary>
	public class Cookie
	{
		#region ��ȡCookieֵ public static HttpCookie Get(string name)
		/// <summary>
		/// ��ȡCookieֵ
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static HttpCookie Get(string name)
		{
			string appPrefix = ApplicationSettings.Get("AppPrefix");
			return HttpContext.Current.Request.Cookies[appPrefix + name];
		}
		#endregion

		#region ����Cookieֵ 	public static HttpCookie Set(string name)
		/// <summary>
		/// ����Cookieֵ
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static HttpCookie Set(string name)
		{
			string appPrefix = ApplicationSettings.Get("AppPrefix");
			return new HttpCookie(appPrefix + name);
		}
		#endregion

		#region ����Cookieֵ public static void Save(HttpCookie cookie)
		/// <summary>
		///  ����Cookieֵ
		/// </summary>
		/// <param name="cookie"></param>
		public static void Save(HttpCookie cookie)
		{
			string domain = Fetch.ServerDomain;
			string host   = HttpContext.Current.Request.Url.Host.ToLower();
			if (domain != host)
			{
				cookie.Domain = domain;
			}
			HttpContext.Current.Response.Cookies.Add(cookie);
		}
		#endregion

		#region �Ƴ�Cookieֵ public static void Remove(HttpCookie cookie)
		/// <summary>
		///�Ƴ�Cookieֵ
		/// </summary>
		/// <param name="cookie"></param>
		public static void Remove(HttpCookie cookie)
		{
			if (cookie != null)
			{
				cookie.Expires = new System.DateTime(1983, 5, 21);
				Save(cookie);
			}
		}
		#endregion

		#region �Ƴ�Cookieֵ public static void Remove(string name)
		/// <summary>
		///�Ƴ�Cookieֵ
		/// </summary>
		/// <param name="name"></param>
		public static void Remove(string name)
		{
			Remove(Get(name));
		}
		#endregion
	}
}