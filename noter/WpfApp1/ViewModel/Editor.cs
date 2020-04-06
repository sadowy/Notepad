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
    using System.Windows.Input;

    public class Editor : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void onPropertyChanged(params string[] propertiesNames)
        {
            if (PropertyChanged != null)
                foreach (var propertyName in propertiesNames)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
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

        #region File handling
        public string FilePath { get; set; }
        public ICommand New
        {
            get
            {
                if (_new == null)
                    _new = new RelayCommand(
                        (object param) =>
                        {
                            text.clear();
                            FilePath = null;
                            onPropertyChanged(nameof(Text), nameof(FilePath));
                        }, (object param) =>
                        {
                            return Text.Length != 0;
                        });
                return _new;
            }
        }
        public ICommand ReadTextFromFile
        {
            get
            {
                if (readTextFromFile == null)
                    readTextFromFile = new RelayCommand(
                        (object param) =>
                        {
                            try
                            {
                                if (!(param is string))
                                    throw new Exception("Invalid command parameter type");
                                string filePath = (string)param;
                                text.readFromFile(filePath);
                                FilePath = filePath;
                                onPropertyChanged(nameof(FilePath), nameof(Text));
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Error reading text from file", ex);
                            }
                        });
                return readTextFromFile;
            }
        }
        public ICommand SaveTextToFile
        {
            get
            {
                if (saveTextToFile == null)
                    saveTextToFile = new RelayCommand(
                        (object param) =>
                        {
                            try
                            {
                                if(param != null)
                                {
                                    if (!(param is string))
                                        throw new Exception("Invalid command parameter type");
                                    string filePath = (string)param;
                                    FilePath = filePath;
                                }
                                text.saveToFile(FilePath);
                                onPropertyChanged(nameof(FilePath));
                            }catch(Exception ex)
                            {
                                throw new Exception("Error saving to file", ex);
                            }
                        },
                        (object param) =>
                        {
                            return (param != null && param is string) || FilePath != null;
                        });
                return saveTextToFile;
            }
        }

        private ICommand _new;
        private ICommand readTextFromFile;
        private ICommand saveTextToFile;
        #endregion
    }
}
