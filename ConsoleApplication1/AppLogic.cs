using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class AppLogic
    {
        private const string Operation1 = "totalcount";
        private const string Operation2 = "search";
        public void ExecuteTask(string[] args)
        {
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            var csvParser = new CsvParser("Testdata/is24_iis.log");
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == Operation1)
                {
                    var results = (from item in (from line in csvParser.Select("s-ip", null)
                                                 from item in line
                                                 select line.First())
                                   group item by item into g
                                   select new { ip = g.Key, count = g.Count() }).ToList();
                    foreach (var result in results)
                    {
                        var output = string.Format("{0} {1}", result.ip, result.count);
                        Console.WriteLine(output);
                        log.Debug(output);
                    }
                }
                else if (args[i] == Operation2)
                {
                    Console.Write("Please enter the filter value : ");
                    var filterValue = Console.ReadLine();
                    var results = (from line in csvParser.Select(null, new Dictionary<string, string> { { "s-ip", filterValue } })
                                select line).ToList();
                    foreach (var result in results)
                    {
                        var sb = new StringBuilder();
                        foreach (var item in result)
                        {
                            sb.Append(item + " ");
                        }                     
                        Console.WriteLine(sb.ToString());
                        log.Debug(sb.ToString());
                    }
                }
            }
            var data = (from line in csvParser.Select("time-taken")
                        from item in line
                        select double.Parse(item)).Average();
            log.Debug(data);
            Console.WriteLine(data);
        }
    }
}
