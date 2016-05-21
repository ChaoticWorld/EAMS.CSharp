using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IM
{
    public class yixin : IIMFactory
    {
        string token = "dflYixin";
        public bool checkSignature(string signature, string timestamp, string nonce)
        {
            bool r = false;
            List<string> signs = new List<string>() { token, timestamp, nonce };
            signs.Sort();
            StringBuilder sign = new StringBuilder();
            for (int i = 0; i < signs.Count; i++)
                sign.Append(signs[i]);
            commonCryptography.HashEncrypt hashEncrypt = new commonCryptography.HashEncrypt(false,false);
            r = signature.Equals( hashEncrypt.SHA1Encrypt(sign.ToString()));
            return r;
        }
    }
}
