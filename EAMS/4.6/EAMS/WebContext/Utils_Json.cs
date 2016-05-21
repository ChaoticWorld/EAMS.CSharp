using System;

using System.Text;
using System.Collections;
namespace WebCommon
{
    public class Json
    {
        //对应于JSON的success成员
        public bool success
        {
            get
            {
                return _success;
            }
            set
            {
                //如设置为true则清空error
                if (success) _error = string.Empty;
                _success = value;
            }
        }

        //对应于JSON的error成员
        public string error
        {
            get
            {
                return _error;
            }
            set
            {
                //如设置error，则自动设置success为false
                if (value != "") _success = false;
                _error = value;
            }
        }

        //对应JSON的singleInfo成员
        public string singleInfo = string.Empty;

        protected string _error = string.Empty;
        protected bool _success = true;
        public System.Collections.ArrayList arrData = new ArrayList();
        public System.Collections.ArrayList arrDataItem = new ArrayList();

        public Json()
        {
        }

        //重置，每次新生成一个json对象时必须执行该方法
        public void Reset()
        {
            _success = true;
            _error = string.Empty;
            singleInfo = string.Empty;
            arrData.Clear();
            arrDataItem.Clear();
        }

        ///添加data数组中一个元素（js对象）的一个名值对，例如
        ///对于一个数组元素：{userName:"supNate",userId:"1"}
        ///需执行两次AddItem：
        ///AddItem("userName","supNate");
        ///AddItem("userId","1");
        ///最后执行
        ///ItemOk();
        ///表示数组元素添加完毕，底下的AddItem表示另一个数组元素的开始
        public void AddItem(string name, string _value)
        {
            arrDataItem.Add(name);
            arrDataItem.Add(_value);
        }

        //一个数组元素添加完毕（data数组）
        public void ItemOk()
        {
            arrData.Add(arrDataItem);
            arrDataItem = new ArrayList();
        }

        //序列化JSON对象，得到返回的JSON代码
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("success:" + _success.ToString().ToLower() + ",");
            sb.Append("error:unescape(\"" + escape(_error) + "\"),");
            sb.Append("singleInfo:unescape(\"" + escape(singleInfo) + "\"),");
            sb.Append("data:[");

            for (int i = 0; i < arrData.Count; i++)
            {
                ArrayList arr = (ArrayList)arrData[i];
                sb.Append("{");
                for (int j = 0; j < arr.Count; j += 2)
                {
                    if (j == arr.Count) break;
                    sb.Append((string)arr[j]);
                    sb.Append(":");
                    sb.Append("unescape(\"");
                    sb.Append(escape(arr[j + 1].ToString()));
                    sb.Append("\")");
                    if (j < arr.Count - 2) sb.Append(",");
                }
                sb.Append("}");
                if (i < arrData.Count - 1) sb.Append(",");
            }
            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }

        public string ToString(int encode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("success:" + _success.ToString().ToLower() + ",");
            sb.Append("error:\"" + _error + "\",");
            sb.Append("singleInfo:\""+ singleInfo + "\",");
            sb.Append("data:[");

            for (int i = 0; i < arrData.Count; i++)
            {
                ArrayList arr = (ArrayList)arrData[i];
                sb.Append("{");
                for (int j = 0; j < arr.Count; j += 2)
                {
                    if (j == arr.Count) break;
                    sb.Append((string)arr[j]);
                    sb.Append(":");
                    sb.Append("\"");
                    sb.Append(arr[j + 1].ToString());
                    sb.Append("\"");
                    if (j < arr.Count - 2) sb.Append(",");
                }
                sb.Append("}");
                if (i < arrData.Count - 1) sb.Append(",");
            }
            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }

        private string escape(string s)
        {
            StringBuilder sb = new StringBuilder();
            byte[] ba = System.Text.Encoding.Unicode.GetBytes(s);
            for (int i = 0; i < ba.Length; i += 2)
            {
                sb.Append("%u");
                sb.Append(ba[i + 1].ToString("X2"));
                sb.Append(ba[i].ToString("X2"));
            }
            return sb.ToString();
        }

    }

}
