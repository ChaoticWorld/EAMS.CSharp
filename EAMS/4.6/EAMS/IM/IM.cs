using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IM
{
    public interface IIMFactory
    {
        /// <summary>
        /// 应用程序与IM验证
        /// 加密/校验流程如下：
        ///1. 将token、timestamp、nonce三个参数进行字典序排序;token为在IM网站填写的生成签名用的
        ///2. 将三个参数字符串拼接成一个字符串进行sha1加密
        ///3. 开发者获得加密后的字符串可与signature对比
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="signature">加密签名</param>
        /// <returns>是否验证成功</returns>
        bool checkSignature(string signature,string timestamp,string nonce);



    }
}
