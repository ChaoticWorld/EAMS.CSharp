using System.Web;
using System.Configuration;

namespace WebCommon
{
	///< summary>
	/// ��ConfigurationSettings.AppSettings�������з�װ
	/// </summary>
	public class ApplicationSettings
	{
		/// <summary>
		/// ��ȡweb.config��������
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string Get(string key)
		{
			
#if DOTNET2_0
			return System.Configuration.ConfigurationManager.AppSettings[key];		// for .net 2.0
#else
			return System.Configuration.ConfigurationManager.AppSettings[key];		// for .net 1.1
#endif
			
		}
	}
}