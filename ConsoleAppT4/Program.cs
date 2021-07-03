using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleAppT4
{
    class Program
    {
        public static string PATH_DIR = @"\Project\Insurance\Alborz-coreinsurance\Core\Insurance\Insurance.Core\Domain\";
        public static string PATH_FOLDER = @"Account\";
        public static string PATH_FILE = @"AccountSetting.cs";

        public class TableObject
        {
            public TableObject()
            {
                ListNode = new List<Node>();
            }
            public string name { get; set; }
            public string desc { get; set; }
            public List<Node> ListNode;
        }

        public class Node
        {
            public string type { get; set; }
            public string desc { get; set; }
            public string name { get; set; }
        }
        static void Main(string[] args)
        {
            SHA1CryptoServiceProvider sha1Hasher = new SHA1CryptoServiceProvider();
            byte[] hashedDataBytes = sha1Hasher.ComputeHash(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
            DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fffZ");
            var nonce = Convert.ToBase64String(hashedDataBytes);
            var now = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fffZ");
            var random = RandomKey();
            m();
            Console.WriteLine("Hello World!");
        }

        private static string RandomKey() => Guid.NewGuid().ToString().Replace("-", "");
        static string m()
        {
            var contents = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, PATH_DIR + PATH_FOLDER + PATH_FILE));
            var newLines = contents
                                .Where(x => !x.Contains("summ"))
                                .Where(x => !x.Contains("namespace"))
                                .Where(x => !x.Contains("using"));
            var result = string.Join(',', newLines).Replace("}","").Replace("{", "").Replace("get; set;","").Replace(",,","").Split("///");

            var tableObject = new TableObject();
            foreach (var item in result)
            {
                if (item.Contains("class"))
                {
                    var objclass = item.Split(",");
                    tableObject.desc = objclass[0];
                    tableObject.name = objclass[1].Split("class")[1].Split(":")[0] ;
                }
                else if(item.Contains("public"))
                {
                    var objprop = item.Split(",");
                    tableObject.ListNode.Add(new Node() 
                    { 
                        desc = objprop[0],
                        name = objprop[1].Split("public ")[1].Split(" ")[1],
                        type = objprop[1].Split("public ")[1].Split(" ")[0]
                    });
                }
            }
            return contents.ToString();
        }
    }
}
