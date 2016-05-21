using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace IPRegister
{
    class Program
    {
        static string Key;
        static string Url;
        static int counter = 0;
        static void Main(string[] args)
        {
            Key = System.Configuration.ConfigurationManager.AppSettings["Key"].ToString();
            Url = System.Configuration.ConfigurationManager.AppSettings["Url"].ToString();
            int interval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["interval"].ToString());
            
            Console.WriteLine("Start!");
            
            do
            {
                doRegister();
                System.Threading.Thread.Sleep(interval);
            }
            while (true);
        }

        static void doRegister()
        {
            Console.WriteLine("第" + (counter++) + "次触发!" + DateTime.Now.ToString());
            string getIPUrl = "http://tools.2345.com/api/getip.php?act=getips";
            string getIpStr = HttpGetPost.RequestGet(getIPUrl);
            Regex regex = new Regex(@"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}");
            CaptureCollection gc = regex.Match(getIpStr).Captures;
            if (gc.Count > 0)
            {
                string IPStr = gc[gc.Count - 1].Value;

                Console.WriteLine("当前外网IP:" + IPStr);
                string data = "Key=" + Key + "&ip=" + IPStr;
                //throw new NotImplementedException();
                string result = string.Empty;
                try
                {
                    result = HttpGetPost.PostDataToUrl(data, Url);
                }
                catch (Exception e) { result = e.Message; }
                
                Console.WriteLine("更新结果:" + result);
            }
            else
            {
                Console.WriteLine("未获得地址:" + getIpStr);
            }
        }
    }
}
