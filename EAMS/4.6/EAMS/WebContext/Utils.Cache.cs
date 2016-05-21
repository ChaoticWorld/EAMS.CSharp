using System;
using System.Web;
using System.Configuration;
using System.Web.Caching;


namespace WebCommon
{
	/// <summary>
	/// �Ի���������з�װ
	/// </summary>
	public class Caching
	{
		#region  ��ȡ�������� public static object Get(string name)
		/// <summary>
		/// ��ȡ��������
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static object Get(string name)
		{
			string appPrefix = ApplicationSettings.Get("AppPrefix");
			if (null != HttpContext.Current)
			{
				return HttpContext.Current.Cache.Get(appPrefix + name);
			}
			else
			{
				return HttpRuntime.Cache.Get(appPrefix + name);
			}
		}
		#endregion

		#region ���û��� 	public static void Set(string name, object value, CacheDependency cacheDependency)
		/// <summary>
		/// ���û���
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="cacheDependency"></param>
		public static void Set(string name, object value, CacheDependency cacheDependency)
		{
			
			Set(name, value, cacheDependency, DateTime.Now.AddSeconds(20), TimeSpan.Zero);
			
		}
		#endregion

		#region ���û��� public static void Set(string name, object value, CacheDependency cacheDependency, DateTime dt)
		
		/// <summary>
		/// ���û���
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="cacheDependency"></param>
		/// <param name="dt"></param>
		public static void Set(string name, object value, CacheDependency cacheDependency, DateTime dt)
		{
			Set(name, value, cacheDependency, dt, TimeSpan.Zero);
		}
		#endregion

		#region ���û��� public static void Set(string name, object value, CacheDependency cacheDependency, TimeSpan ts)
		/// <summary>
		/// ���û���
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="cacheDependency"></param>
		/// <param name="ts"></param>
		public static void Set(string name, object value, CacheDependency cacheDependency, TimeSpan ts)
		{
			Set(name, value, cacheDependency, Cache.NoAbsoluteExpiration, ts);
		}
		#endregion
		#region ���û��� public static void Set(string name, object value, CacheDependency cacheDependency, DateTime dt, TimeSpan ts)
		/// <summary>
		/// ���û���
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="cacheDependency"></param>
		/// <param name="dt"></param>
		/// <param name="ts"></param>
		public static void Set(string name, object value, CacheDependency cacheDependency, DateTime dt, TimeSpan ts)
		{
			string appPrefix = ApplicationSettings.Get("AppPrefix");
#if DEBUG
			HttpRuntime.Cache.Insert(appPrefix + name, value, cacheDependency, DateTime.Now.AddSeconds(10), Cache.NoSlidingExpiration);
#else
//			HttpContext.Current.Cache.Insert(appPrefix + name, value, cacheDependency, dt, ts);
			HttpRuntime.Cache.Insert(appPrefix + name, value, cacheDependency, dt, ts);
#endif
		}
		#endregion
	
		#region ������� public static void Remove(string name)
		/// <summary>
		/// �������
		/// </summary>
		/// <param name="name"></param>
		public static void Remove(string name)
		{
			string appPrefix = ApplicationSettings.Get("AppPrefix");
			object obj = HttpContext.Current.Cache[appPrefix + name];
			if (obj != null)
			{
				HttpContext.Current.Cache.Remove(appPrefix + name);
			}
		}
		#endregion
	}
}