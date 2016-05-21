using System.Web;
using System.Configuration;

namespace WebCommon
{
	///< summary>
	/// 对ConfigurationSettings.AppSettings操作进行封装
	/// </summary>
	public class ApplicationSettings
	{
		/// <summary>
		/// 获取web.config的配置项
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