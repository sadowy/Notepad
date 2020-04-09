using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noter.Model
{
    class Text
    {
        public string[] Paragraphs
        {
            get
            {
                return paragraphs.ToArray();
            }
            set
            {
                paragraphs.Clear();
                paragraphs.AddRange(value);
            }
        }

        private List<string> paragraphs = new List<string>();

        public void ReadFromFile(string filePath)
        {
            string[] rows = System.IO.File.ReadAllLines(filePath);
            paragraphs.Clear();
            paragraphs.AddRange(rows);
        }
        public void SaveToFile(string filePath)
        {
            System.IO.File.WriteAllLines(filePath, Paragraphs, Encoding.Default);
        }
        public void Clear()
        {
            paragraphs.Clear();
        }
        public Text Clone()
        {
            List<string> _paragraphs = new List<string>();
            foreach (var paragraph in Paragraphs)
            {
                _paragraphs.Add(paragraph);
            }
            return new Text() { Paragraphs = _paragraphs.ToArray() };
        }
        public override string ToString()
        {
            return string.Concat<string>(Paragraphs);
        }
        public string getString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var paragraph in Paragraphs)
            {
                if (paragraph.EndsWith("\r"))
                {
                    builder.Append(paragraph + "\n");
                    continue;
                }
                builder.Append(paragraph);
            }
            return builder.ToString();
        }
    }
}
