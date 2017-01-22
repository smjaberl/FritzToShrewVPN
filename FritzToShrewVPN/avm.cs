/*Copyright 2016 Jan Hendrik Berlin,  mail: Jan-Hendrik.Berlin@uni-dortmund.de

  This file is part of FritzToShrewVPN. https://github.com/smjaberl/FritzToShrewVPN/

  FritzToShrewVPN is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or(at your option) any later version.

  FritzToShrewVPN is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU General Public License for more details.

  You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
  */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Windows;
using System.IO;

namespace FritzToShrewVPN
{
    class avm
    {
        DataVPN thedata;

        public avm(DataVPN data)
        {
            thedata = data;
        }

        public string GetPage(string url)

        {

            Uri uri = new Uri(url);

            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            StreamReader reader = new StreamReader(response.GetResponseStream());

            return reader.ReadToEnd();

        }

        public string GetSessionId()
        {

            XDocument doc = XDocument.Load(@"http://" + thedata.LocalBox + "/login_sid.lua");

            string sid = GetValue(doc, "SID");

            if (sid == "0000000000000000")

            {

                string challenge = GetValue(doc, "Challenge");

                string uri = @"http://" + thedata.LocalBox + "/login_sid.lua?username=" +

               thedata.FBUser + @"&response=" + GetResponse(challenge, thedata.FBPasswd);

                doc = XDocument.Load(uri);

                sid = GetValue(doc, "SID");

            }
            return sid;
        }

        private string GetResponse(string challenge, string kennwort)

        {

            return challenge + "-" + GetMD5Hash(challenge + "-" + kennwort);

        }

        private string GetMD5Hash(string input)

        {

            MD5 md5Hasher = MD5.Create();

            byte[] data =

           md5Hasher.ComputeHash(Encoding.Unicode.GetBytes(input));

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)

            {

                sb.Append(data[i].ToString("x2"));

            }

            return sb.ToString();

        }

        private string GetValue(XDocument doc, string name)

        {

            XElement info = doc.FirstNode as XElement;

            return info.Element(name).Value;



        }

    }
}