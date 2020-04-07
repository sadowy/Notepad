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
    using System.Windows;
    using System.Windows.Input;

    public class Editor : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                OnPropertyChanged(Text);
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
                            OnPropertyChanged(nameof(Text));
                            FilePath = null;
                            OnPropertyChanged(nameof(FilePath));
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
                                OnPropertyChanged(nameof(Text));
                                FilePath = filePath;
                                OnPropertyChanged(nameof(FilePath));
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
                                OnPropertyChanged(nameof(FilePath));
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
        #region Editing
        public int TextSelectionStart { get; set; }
        public int TextSelectionLength { get; set; }
        public string SelectedText { get; set; }
        
        
        public ICommand CopySelectedText
        {
            get
            {
                if (copySelectedText == null)
                    copySelectedText = new RelayCommand(
                        (object param) =>
                        {
                            copyDeleteSelectedTex(true, false);
                        },
                        (object param) =>
                        {
                            return TextSelectionLength > 0;
                        });
                return copySelectedText;
            }
        }
        public ICommand CutSelectedText
        {
            get
            {
                if (cutSelectedText == null)
                    cutSelectedText = new RelayCommand(
                        (object param) =>
                        {
                            copyDeleteSelectedTex(true, true);
                        },
                        (object param) =>
                        {
                            return TextSelectionLength > 0;
                        });
                return cutSelectedText;
            }
        }
        public ICommand DeleteSelectedText
        {
            get
            {
                if (deleteSelectedText == null)
                    deleteSelectedText = new RelayCommand(
                        (object param) =>
                        {
                            copyDeleteSelectedTex(false, true);
                        },
                        (object param) =>
                        {
                            return TextSelectionLength > 0;
                        });
                return deleteSelectedText;
            }
        }
        public ICommand PasteTextFromClipboard
        {
            get
            {
                if (pasteTextFromClipboard == null)
                    pasteTextFromClipboard = new RelayCommand(
                        (object param) =>
                        {
                            pasteText();
                        },
                        (object param) =>
                        {
                            return Clipboard.ContainsText();
                        });
                return pasteTextFromClipboard;
            }
        }
        public ICommand InsertDateTime
        {
            get
            {
                if (insertDateTime == null)
                    insertDateTime = new RelayCommand(
                        (object param) =>
                        {
                            pasteText(DateTime.Now.ToString());
                        });
                return insertDateTime;
            }
        }
        public ICommand SelectAll
        {
            get
            {
                if (selectAll == null)
                    selectAll = new RelayCommand(
                        (object param) =>
                        {
                            TextSelectionStart = 0;
                            OnPropertyChanged(nameof(TextSelectionStart));
                            TextSelectionLength = Text.Length;
                            OnPropertyChanged(nameof(TextSelectionLength));
                        },
                        (object param) =>
                        {
                            return SelectedText != Text;
                        });
                return selectAll;
            }
        }

        private ICommand copySelectedText;
        private ICommand cutSelectedText;
        private ICommand deleteSelectedText;
        private ICommand pasteTextFromClipboard;
        private ICommand insertDateTime;
        private ICommand selectAll;
        private void copyDeleteSelectedTex(bool copy, bool delete)
        {
            if (copy) Clipboard.SetText(SelectedText);
            if (delete)
                Text = Text.Substring(0, TextSelectionStart) +
                    Text.Substring(TextSelectionStart + TextSelectionLength);

            OnPropertyChanged(nameof(Text));
        }
        private void pasteText(string textToPaste = null)
        {
            if (textToPaste == null) 
                textToPaste = Clipboard.GetText();

            Text = Text.Substring(0, TextSelectionStart) + 
                textToPaste +
                Text.Substring(TextSelectionStart + TextSelectionLength);
            OnPropertyChanged(nameof(Text));

            TextSelectionStart = Text.Length;
            OnPropertyChanged(nameof(TextSelectionStart));
        }
        #endregion
    }
}
