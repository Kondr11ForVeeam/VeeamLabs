using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _1._3.Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<ElementsWithNames>()
            { new ElementsWithNames() { Value = 01, Name = "01" },
              new ElementsWithNames() { Value = 02, Name = "2" },
              new ElementsWithNames() { Value = 03, Name = "03" },
              new ElementsWithNames() { Value = 1, Name = "First" },
              new ElementsWithNames() { Value = 2, Name = "Second" },
              new ElementsWithNames() { Value = 3, Name = "Third" }};

            Console.WriteLine("====First task start====");
            Console.WriteLine(LinqTasks<ElementsWithNames>.ConcatenationNames(list, ", "));
            Console.WriteLine("====First task end====\n");

            Console.WriteLine("====Second task start====");
            list = LinqTasks<ElementsWithNames>.FindElementsWithLengthMoreThanCount(list).ToList();
            LinqTasks<ElementsWithNames>.WriteCollention(list);
            Console.WriteLine("====Second task end====\n");

            Console.WriteLine("====Third task start====");
            LinqTasks<ElementsWithNames>.WordsGroups("Это что же получается: ходишь, ходишь в школу, а потом бац - вторая смена");
            Console.WriteLine("====Third task end====\n");

            Console.WriteLine("====Fourth task start====");
            var dic = new Dictionary<string, string>() { {"THIS", "ЭТА" }, { "DOG", "СОБАКА" }, { "EATS", "ЕСТ" } ,
                { "TOO", "СЛИШКОМ" }, { "MUCH", "МНОГО" }, {"VEGETABLES", "ОВОЩЕЙ" }, {"AFTER", "ПОСЛЕ" }, {"LUNCH", "ОБЕДА" } };
            Console.WriteLine(LinqTasks<ElementsWithNames>.GetBook("This dog eats too much vegetables after lunch", dic, 3));
            Console.WriteLine("====Fourth task end====\n");
            Console.ReadKey();
        }


    }

    public static class LinqTasks<T> where T: ElementsWithNames
    {
        public static string ConcatenationNames(IEnumerable<T> collection, string delimeter)
        {
            return string.Join(delimeter, collection.Skip(3).Select(col => col.Name));
        }
        public static IEnumerable<T> FindElementsWithLengthMoreThanCount(IEnumerable<T> collection)
        {
            int index = 0;
            return from el in collection
                   let counter = index++
                   where el.Name.Length > counter
                   select el;
        }

        public static void WriteCollention(IEnumerable<T> collection)
        {
            foreach (var el in collection)
                Console.WriteLine($"Name: {el.Name}, Value: {el.Value}");
        }

        public static void WordsGroups(string str)
        {
            StringBuilder builder = new StringBuilder(str);
            char[] replaceChar = new char[] { '.', ':', ';', ',', '!', '?', '-' };
            foreach (var item in replaceChar)
                builder = builder.Replace(item.ToString(), "");
            str = builder.ToString();
            var strArr = new Regex("\\s+").Split(str);
            var dic = new Dictionary<int, List<string>>();
            foreach (var s in strArr)
            {
                if (!dic.ContainsKey(s.Length))
                    dic.Add(s.Length, new List<string>() { s });
                else
                    dic[s.Length].Add(s);
            }
            var index = 1;
            var result = from d in dic
                         let counter = index++
                         orderby d.Value.Count descending, d.Key descending
                         let promRes = $"Группа {counter}. Длинна {d.Key}. Количество {d.Value.Count}\n" + string.Join("\n", d.Value)
                         select promRes;
            foreach (var r in result)
            {
                Console.WriteLine(r + "\n");
            }
        }

        public static string GetBook(string str, Dictionary<string, string> dic, int countRowElements)
        {
            var strArr = new Regex("\\s+").Split(str.ToUpper());
            int index = 0;
            return string.Join("\n", from s in strArr
                              let counter = index++
                              group Sub(s, dic) by counter / countRowElements into row
                              let promRes = string.Join(" ", row)
                              select promRes);
        }

        public static string Sub(string str, Dictionary<string, string> dic)
        {
            return dic[str];
        }
    }

    public class ElementsWithNames
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }
}

