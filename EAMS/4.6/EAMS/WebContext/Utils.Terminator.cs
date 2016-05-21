using System.Web;
using System.Text;
using WebCommon;

namespace WebCommon
{
	/// <summary>
	/// 程序终止调度
	/// </summary>
	public class Terminator
	{
		#region Response.Write  private void Echo(string s)
		private void Echo(string s)
		{
			HttpContext.Current.Response.Write(s);
		}
		#endregion

		#region end private void End()
		/// <summary>
		/// end
		/// </summary>
		private void End()
		{
			HttpContext.Current.Response.End();
		}
		#endregion

		#region alert javascript
		/// <summary>
		/// alert javascript
		/// </summary>
		/// <param name="s"></param>
		public virtual void Alert(string s)
		{
			Echo("<script language='javascript'>alert('" + s.Replace("'", @"\'") + "');history.back();</script>");
			End();
		}

		public virtual void Alert(string s,string backurl)
		{
			Echo("<script language='javascript'>alert('" + s.Replace("'", @"\'") + "');location.href='"+backurl+"';</script>");
			End();
		}
		#endregion

		#region stop public virtual void Throw(string message)
		/// <summary>
		/// stop
		/// </summary>
		/// <param name="message"></param>
		public virtual void Throw(string message)
		{
			HttpContext.Current.Response.ContentType = "text/html";
			HttpContext.Current.Response.AddHeader("Content-Type", "text/html");
			Throw(message, null, null, null, true);
		}
		#endregion

		#region 输出指定的提示信息 public virtual void Throw(string message, string title, string links, string autojump, bool showback)
		/// <summary>
		/// 输出指定的提示信息
		/// </summary>
		/// <param name="message">提示内容</param>
		/// <param name="title">标题</param>
		/// <param name="links">链拉</param>
		/// <param name="autojump">自动跳转定向地址</param>
		/// <param name="showback">是否显示返回链接</param>
		public virtual void Throw(string message, string title, string links, string autojump, bool showback)
		{
			HttpContext.Current.Response.ContentType = "text/html";
			HttpContext.Current.Response.AddHeader("Content-Type", "text/html");

			StringBuilder sb = new StringBuilder(template);

			sb.Replace("{$Message}", message);
			sb.Replace("{$Title}", (title == null || title == "") ? "系统提示" : title);

			if (links != null && links != "")
			{
				string[] arr1 = links.Split('|');
				for (int i=0; i<arr1.Length; i++)
				{
					string[] arr2 = arr1[i].Split(',');
					if (arr2.Length > 1)
					{
						if (arr2[1].Trim() == "RefHref")
						{
							arr2[1] = Fetch.Referrer;
						}
						if (arr2[1] == string.Empty || arr2[1] == null)
						{
							continue;
						}

						string s = ("<li><a href='" + arr2[1] + "'");
						if (arr2.Length == 3)
						{
							s += (" target='" + arr2[2].Trim() + "'");
						}

						if (arr2[0].Trim() == "RefText")
						{
							arr2[0] = Text.TextEncode(Fetch.Referrer);
						}
						s += (">" + arr2[0].Trim() + "</a></li>\r\n\t\t\t\t");
						sb.Replace("{$Links}", s + "{$Links}");
					}
				}
			}

			if (autojump != null && autojump != string.Empty)
			{
				string s = autojump == "back" ? "javascript:history.back()" : autojump;
				sb.Replace("{$AutoJump}", "<meta http-equiv='refresh' content='3; url=" + s + "' />");
			}
			else 
			{
				sb.Replace("{$AutoJump}", "<!-- no jump -->");
			}

			if (showback)
			{
				sb.Replace("{$Links}", "<li><a href='javascript:history.back()'>返回前一页</a></li>");
			}
			else 
			{
				sb.Replace("{$Links}", "<!-- no back -->");
			}
			Echo(sb.ToString());
			End();
		}
		#endregion


//------- template -------------------------------------------------------
		#region 页面终止页面模板		public virtual string template
/// <summary>
/// 页面终止页面模板
/// </summary>
public virtual string template
{
	get
	{
		return @"<html xmlns:v>
				<head>
				<title>{$Title}</title>
				<meta http-equiv='Content-Type' content='text/html; charset=" + Encoding.Default.BodyName + @"' />
				<meta name='description' content='.NET类库 WebCommon.dll 页面中止程序' />
				<meta name='copyright' content='http://www.Qilu168.net/' />
				<meta name='generator' content='vs2008' />
				<meta name='usefor' content='application termination' />
				{$AutoJump}
				<style rel='stylesheet'>
				v\:*	{
					behavior:url(#default#vml);
				}
				body, div, span, li, td, a {
					color: #222222;
					font-size: 12px !important;
					font-size: 11px;
					font-family: tahoma, arial, 'courier new', verdana, sans-serif;
					line-height: 19px;
				}
				a {
					color: #2c78c5;
					text-decoration: none;
				}
				a:hover {
					color: red;
					text-decoration: none;
				}
				</style>
				</head>
				<body style='text-align:center;margin:90px 20px 50px 20px'>
				<?xml:namespace prefix='v' />
				<div style='margin:auto; width:450px; text-align:center'>
					<v:roundrect style='text-align:left; display:table; margin:auto; padding:15px; width:450px; height:210px; overflow:hidden; position:relative;' arcsize='3200f' coordsize='21600,21600' fillcolor='#fdfdfd' strokecolor='#e6e6e6' strokeweight='1px'>
						<table width='100%' cellpadding='0' cellspacing='0' border='0' style='padding-bottom:6px; border-bottom:1px #cccccc solid'>
							<tr>
								<td><b>{$Title}</b></td>
								<td align='right' style='color:#f8f8f8'>--- Qilu168 terminator</td>
							</tr>
						</table>
						<table width='100%' cellpadding='0' cellspacing='0' border='0' style='word-break:break-all; overflow:hidden'>
							<tr>
								<td width='80' valign='top' style='padding-top:13px'><span style='font-size:16px; zoom:4; color:#aaaaaa'><font face='webdings'>i</font></span></td>
								<td valign='top' style='padding-top:17px'>
									<p style='margin-bottom:22px'>{$Message}</p>
									{$Links}
								</td>
							</tr>
						</table>
					</v:roundrect>
				</div>
				</body>
				</html>";
			}
		}
		#endregion
	}
	
}