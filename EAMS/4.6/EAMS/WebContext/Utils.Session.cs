using System.Web;
using System.Configuration;


namespace WebCommon
{
	/// <summary>
	/// ��Session�������з�װ
	/// </summary>
	public class SessionState
	{

		#region �� Session ��ȡ ��Ϊ name ��ֵ public static object Get(string name)
		/// <summary>
		/// �� Session ��ȡ ��Ϊ name ��ֵ
		/// </summary>
		public static object Get(string name)
		{
			string appPrefix = ApplicationSettings.Get("AppPrefix");
			return (object)HttpContext.Current.Session[appPrefix + name];
		}
		#endregion

		#region �� Session ���� ��Ϊ name �ģ� ֵΪ value public static void Set(string name, object value)
		/// <summary>
		/// �� Session ���� ��Ϊ name �ģ� ֵΪ value
		/// </summary>
		public static void Set(string name, object value)
		{
			string appPrefix = ApplicationSettings.Get("AppPrefix");
			HttpContext.Current.Session.Add(appPrefix + name, value);
		}
		#endregion

		#region �� Session ɾ�� ��Ϊ name session �� public static void Remove(string name)
		/// <summary>
		/// �� Session ɾ�� ��Ϊ name session ��
		/// </summary>
		public static void Remove(string name)
		{
			string appPrefix = ApplicationSettings.Get("AppPrefix");
			if (HttpContext.Current.Session[appPrefix + name] != null)
			{
				HttpContext.Current.Session.Remove(appPrefix + name);
			}
		}
		#endregion

		#region ɾ������ session ��
		/// <summary>
		/// ɾ������ session ��
		/// </summary>
		public static void RemoveAll()
		{
			HttpContext.Current.Session.RemoveAll();
		}
		#endregion
	}
}