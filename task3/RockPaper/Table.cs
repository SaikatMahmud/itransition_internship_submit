using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaper
{
    public class Table
    {
        ConsoleTable table;
        public Table(string[] arr)
        {
            table = new ConsoleTable();
            var headers = new[] { "v PC\\User >" }.Concat(arr);
            table.AddColumn(headers);
            int n = arr.Length;
            for(int i=0; i<n; i++)
            {
                var list = new List<string> { arr[i] };
                for(int j=0; j<n; j++)
                {
                    list.Add(Rule.GetResultString(j, i, n).ToString());
                }
                table.AddRow(list.ToArray());
            }
        }
        public void ShowTable()
        {
            table.Write(Format.Alternative);
            Console.WriteLine();
        }
    }
}
