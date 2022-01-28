using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1._4.Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====First task start====");
            var rusEn = Encoding.GetEncoding(20880); //IBM EBCDIC (Cyrillic Russian)
            var gerEn = Encoding.GetEncoding(20106); // German (IA5)
            var japEn = Encoding.GetEncoding(932); // Japanese (Shift-JIS)
            var defEn = Console.OutputEncoding;
            Console.OutputEncoding = japEn;

            var strEn = ToBase64FromString("Мама мыла раму", rusEn);
            Console.WriteLine(strEn);
            Console.WriteLine(ToStringFromBase64(strEn, rusEn));

            Console.WriteLine(strEn = ToBase64FromString("Mama hat den Rahmen gewaschen", gerEn));
            Console.WriteLine(ToStringFromBase64(strEn, gerEn));

            Console.WriteLine(strEn = ToBase64FromString("ママはフレームを洗った", japEn));
            Console.WriteLine(ToStringFromBase64(strEn, japEn));
            Console.WriteLine("====First task end====\n");

            Console.ReadKey();
            Console.Clear();
            Console.OutputEncoding = defEn;

            Console.WriteLine("====Second task start====");
            var listExceprion = new List<Exception>() { new Exception("Hi there!")};
            if (listExceprion.Count == 1)
            {
                CreateException(ExceptionDispatchInfo.Capture(listExceprion[0]));
            }
            Console.WriteLine("====Second task end====\n");
            Console.ReadKey();
        }

        public static string ToBase64FromString(string convetrString, Encoding encoding)
        {
            return Convert.ToBase64String(encoding.GetBytes(convetrString));
        }

        public static string ToStringFromBase64(string convetedString, Encoding encoding)
        {
            return encoding.GetString(Convert.FromBase64String(convetedString)) + "\n";
        }

        public static string WriteBase64(byte[] bytesToWriting)
        {
            return string.Join("", bytesToWriting);
        }

        public static void CreateException(ExceptionDispatchInfo dispatchInfo)
        {
            dispatchInfo.Throw(); 
        }
    }
}
