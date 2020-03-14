using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noter.Model
{
    class Text
    {
        private List<string> paragraphs = new List<string>();
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

        public void readFromFile(string filePath)
        {
            string[] rows = System.IO.File.ReadAllLines(filePath);
            paragraphs.Clear();
            paragraphs.AddRange(rows);
        }
        public void saveToFile(string filePath)
        {
            System.IO.File.WriteAllLines(filePath, Paragraphs, Encoding.Default);
        }
        public void clear()
        {
            paragraphs.Clear();
        }
        public Text clone()
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
    }
}
