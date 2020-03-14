using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noter.ViewModel
{
    using Model;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class Editor : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void onPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private Text text = new Text();

        public string Text
        {
            get
            {
                StringBuilder result = new StringBuilder();
                foreach (string paragraph in text.Paragraphs)
                    result.Append(paragraph);
                return result.ToString();
            }
            set
            {
                text.Paragraphs = value.Split('\n');
                onPropertyChanged();
            }
        }

    }
}
