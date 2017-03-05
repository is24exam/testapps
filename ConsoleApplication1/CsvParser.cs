using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class CsvParser
    {
        private const string Prefix = "#";
        private const string PrefixFields = "#Fields:";
        private const char DefaultDelimiter = ' ';
        private readonly int _lengthOfPrefixFields;
        private string _filePath = string.Empty;
        private string[] _headers = { };
        private int[] _headerFilterIndexes = { };
        private List<int> _selectedColumnIndexes = new List<int>();
        private int _selectedColumnIndex = -1;
        private Dictionary<string, string> _filters=new Dictionary<string, string>();
        public CsvParser(string filePath)
        {
            this._filePath = filePath;
            this._lengthOfPrefixFields = PrefixFields.Length;
        }

        public IEnumerable<IEnumerable<string>> Select(string selectedColumn,Dictionary<string,string> filters=null)
        {
            this._filters.Clear();
            this._filters = filters??new Dictionary<string, string>();

            using (FileStream fs = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                bool isParseNewLine = false;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith(Prefix))
                    {
                        if (line.StartsWith(PrefixFields))
                        {
                            this._headers = this.Split(line, DefaultDelimiter,true).ToArray();
                            this._headerFilterIndexes = this._filters.Select(filter => Array.IndexOf(_headers, filter.Key)).ToArray();
                            this._selectedColumnIndex = Array.IndexOf(this._headers, selectedColumn);

                            isParseNewLine = true;
                            continue;
                        }
                        else
                        {
                            isParseNewLine = false;
                        }
                    }
                    if (isParseNewLine)
                    {
                        var data = SplitWithFilters(line);
                        if (data.Any())
                        {
                            yield return data;
                        }
                    }        
                }
            }
        }
        private IEnumerable<string> SplitWithFilters(string line)
        {
            var results = new List<string>();
            var index = 0;

            var items = this.Split(line, DefaultDelimiter);
            foreach (var item in items)
            {
                if (!_filters.Any() || (this._headerFilterIndexes.Contains(index) && this._filters[_headers[index]] == item))
                {
                    results.AddRange(items);
                }
                index++;
            }
            return results;
        }
        private IEnumerable<string> Split(string input,char delimiter,bool isHeader=false)
        {
            var results = new List<string>();
            var length = input.Length;
            var startIndex = isHeader ? PrefixFields.Length + 1 : 0;
            var begining = startIndex;
            var columnIndex = 0;
            for (var index= startIndex; index < length; index++)
            {
                if (input[index] == delimiter)
                {
                    var subString = input.Substring(begining, index - begining);
                    if (isHeader || 
                        (this._selectedColumnIndex==-1 || columnIndex == this._selectedColumnIndex))
                    {
                        results.Add(subString);
                    }
                    columnIndex++;
                    begining = index + 1;
                }
            }
            if (isHeader || this._selectedColumnIndex == -1 || columnIndex == this._selectedColumnIndex)
            {
                results.Add(input.Substring(begining, length - begining));
            }
            return results;
        }
    }
}
