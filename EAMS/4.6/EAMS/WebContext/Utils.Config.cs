using System;
using System.Web;
using System.Xml;
using System.Web.Caching;
using System.IO;
using System.Collections;
using System.Configuration;
#if DOTNET2_0
using System.Collections.Generic;
#endif


namespace WebCommon
{
	/// <summary>
	/// 自定义Config类
	/// </summary>
	public class Config
	{
		#region Fields
		private static Config mSettings;
		private static readonly object _LockObject = new object();

		/// <summary>
		/// 将配置项存储在无序列表中
		/// </summary>
		
#if DOTNET2_0
		private System.Collections.Generic.Dictionary<string, string> m_hashtable;
#else
		private Hashtable	m_hashtable;
#endif
	
		private string		m_filename;

		#endregion
		
		#region 构造函数，初始化配置文件路径
		public Config()
		{
			m_filename = ApplicationSettings.Get("CustomConfigFile");
			this.Initialize();
		}
		public Config(string filename)
		{
			m_filename = filename;
			this.Initialize();
		}
		#endregion		

		#region 索引
		public string this[string key]
		{
			get 
			{
				if (m_hashtable.ContainsKey(key))
				{
					return m_hashtable[key].ToString();
				}
				return string.Empty;
			}
			set 
			{
				m_hashtable[key] = value;
			}
		}
		#endregion

		/// <summary>
		/// 私有用属性，配制文件(*.xml)的路径
		/// </summary>
		private string m_configFilePath
		{
			get 
			{
				return Fetch.MapPath(m_filename);
			}
		}

		//--- Methods -------------------------

		#region 读取配置文件 	private void Initialize()
		/// <summary>
		/// 读取配置文件
		/// </summary>
		private void Initialize()
		{
			//若成功从缓存读取到，则直接返回
			object cacheConfig = Caching.Get(m_configFilePath);
			if (cacheConfig != null)
			{
				
#if DOTNET2_0
				m_hashtable = cacheConfig as Dictionary<string, string>;
#else
				m_hashtable = cacheConfig as Hashtable;
#endif
				
				return;
			}

			//检查配置文件是否存在
			if (!File.Exists(m_configFilePath))
			{
#if DEBUG
				new Terminator().Throw("配置文件 " + m_configFilePath + " 不存在或访问被拒绝。");
#else
				new Terminator().Throw("配置文件 " + m_filename + " 不存在或访问被拒绝。");
#endif
				return;
			}

			///从配置文件读入配置
			
#if DOTNET2_0
			m_hashtable = new Dictionary<string, string>();
#else
			m_hashtable = new Hashtable();
#endif

			XmlTextReader reader = null;
			try
			{
				string key = null, value = null;
				reader = new XmlTextReader(m_configFilePath);
				reader.WhitespaceHandling = WhitespaceHandling.None;
				reader.MoveToContent();
				while (reader.Read())
				{
					if (3 == reader.Depth && XmlNodeType.EndElement == reader.NodeType && "Item" == reader.Name)
					{
						if (!m_hashtable.ContainsKey(key))
						{
							m_hashtable.Add(key, value);
						}
					}
					else if (4 == reader.Depth && reader.IsStartElement())
					{
						if ("Name" == reader.Name)
						{
							key = reader.ReadString();
						}
						else if ("Value" == reader.Name)
						{
							value = reader.ReadString();
						}
					}
				}
				key = value = null;
			}
			finally
			{
				if (null != reader)
				{
					reader.Close();
					reader = null;
				}
			}
			

			//保存到缓存项，并将配置文件作为缓存依赖
			
			if (!m_hashtable.ContainsKey("DefaultTemplateSkin") || 0 == m_hashtable["DefaultTemplateSkin"].ToString().Length)
			{
				m_hashtable["DefaultTemplateName"] = "default";
				m_hashtable["DefaultSkinName"] = "default";
			}
			else
			{
				string[] strs = m_hashtable["DefaultTemplateSkin"].ToString().Split(new char[1]{'/'});
				m_hashtable["DefaultTemplateName"] = strs[0];
				m_hashtable["DefaultSkinName"] = strs[1];
			}
			Caching.Set(m_configFilePath, m_hashtable, new CacheDependency(m_configFilePath), DateTime.Now.AddSeconds(12));
			
		}

		
		private static string muploadfaces;
		/// <summary>
		/// 通过静态方法获得自身实例
		/// </summary>
		public static string uploadfaces
		{
			get
			{
				if( null == muploadfaces)
				{
					muploadfaces = mSettings["uploadfaces"].ToString().Replace("\\","/");
				}
				return muploadfaces;
			}
		}

		private static string muploadfiles;
		public static string uploadfiles
		{
			get
			{
				if( null == muploadfiles)
				{	if(null == mSettings) mSettings=new Config();
					muploadfiles = mSettings["uploadfiles"].ToString();
				}
				return muploadfiles;
			}
		}

		private static string mpicuploadfiles;
		public static string picuploadfiles
		{
			get
			{
				if( null == mpicuploadfiles)
				{
					mpicuploadfiles = mSettings["picuploadfiles"].ToString();
				}
				return mpicuploadfiles;
			}
		}
		
		//通过静态方法获得自身实例

		
		public static Config Settings
		{
			get 
			{
				lock (_LockObject)
				{
					if (null == mSettings)
					{
						mSettings = new Config();
						return mSettings;
					}
				}
				mSettings.Initialize();
				return mSettings;
			}
		}
		#endregion

		#region 删除存储配置的缓存 	public void RemoveSelfCache()
		/// <summary>
		/// 删除存储配置的缓存
		/// </summary>
		public void RemoveSelfCache()
		{
			Caching.Remove(m_configFilePath);
		}		
		#endregion
	}
}