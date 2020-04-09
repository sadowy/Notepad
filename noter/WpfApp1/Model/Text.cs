using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noter.Model
{
    class Text
    {
        public string Words { get; set; } = string.Empty;

        public void ReadFromFile(string filePath)
        {
            Words = System.IO.File.ReadAllText(filePath);
        }
        public void SaveToFile(string filePath)
        {
            System.IO.File.WriteAllText(filePath, Words, Encoding.Default);
        }
        public void Clear()
        {
            Words = string.Empty;
        }
        public Text Clone()
        {
            return new Text() { Words = Words };
        }
        public override string ToString()
        {
            return Words;
        }
    }
}
