using System;

using System.Text;
using System.Collections;
namespace WebCommon
{
    public class Json
    {
        //��Ӧ��JSON��success��Ա
        public bool success
        {
            get
            {
                return _success;
            }
            set
            {
                //������Ϊtrue�����error
                if (success) _error = string.Empty;
                _success = value;
            }
        }

        //��Ӧ��JSON��error��Ա
        public string error
        {
            get
            {
                return _error;
            }
            set
            {
                //������error�����Զ�����successΪfalse
                if (value != "") _success = false;
                _error = value;
            }
        }

        //��ӦJSON��singleInfo��Ա
        public string singleInfo = string.Empty;

        protected string _error = string.Empty;
        protected bool _success = true;
        public System.Collections.ArrayList arrData = new ArrayList();
        public System.Collections.ArrayList arrDataItem = new ArrayList();

        public Json()
        {
        }

        //���ã�ÿ��������һ��json����ʱ����ִ�и÷���
        public void Reset()
        {
            _success = true;
            _error = string.Empty;
            singleInfo = string.Empty;
            arrData.Clear();
            arrDataItem.Clear();
        }

        ///���data������һ��Ԫ�أ�js���󣩵�һ����ֵ�ԣ�����
        ///����һ������Ԫ�أ�{userName:"supNate",userId:"1"}
        ///��ִ������AddItem��
        ///AddItem("userName","supNate");
        ///AddItem("userId","1");
        ///���ִ��
        ///ItemOk();
        ///��ʾ����Ԫ�������ϣ����µ�AddItem��ʾ��һ������Ԫ�صĿ�ʼ
        public void AddItem(string name, string _value)
        {
            arrDataItem.Add(name);
            arrDataItem.Add(_value);
        }

        //һ������Ԫ�������ϣ�data���飩
        public void ItemOk()
        {
            arrData.Add(arrDataItem);
            arrDataItem = new ArrayList();
        }

        //���л�JSON���󣬵õ����ص�JSON����
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
