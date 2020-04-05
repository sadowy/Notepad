using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Noter
{
    using ViewModel;
    public abstract class FileDialogBox : FrameworkElement, INotifyPropertyChanged
    {
        public bool? FileDialogResult { get; protected set; }
        public ICommand Show
        {
            get
            {
                if (show == null)
                    show = new RelayCommand(execute);
                return show;
            }
        }
        public ICommand CommandFileOk
        {
            get
            {
                return (ICommand)GetValue(commandFileOkProperty);
            }
            set
            {
                SetValue(commandFileOkProperty, value);
            }
        }
        public string Caption
        {
            get
            {
                return (string)GetValue(captionProperty);
            }
            set
            {
                SetValue(captionProperty, value);
            }
        }
        public object CommandParameter
        {
            get { return GetValue(commandParameterProperty); }
            set { SetValue(commandParameterProperty, value); }
        }
        public string FilePath
        {
            get { return (string)GetValue(filePathProperty); }
            set { SetValue(filePathProperty, value); }
        }
        public string Filter
        {
            get { return (string)GetValue(filterProperty); }
            set { SetValue(filterProperty, value); }
        }
        public int FilterIndex
        {
            get { return (int)GetValue(filterIndexProperty); }
            set { SetValue(filterIndexProperty, value); }
        }
        public string DefaultExtension
        {
            get { return (string)GetValue(defaultExtensionProperty); }
            set { SetValue(defaultExtensionProperty, value); }
        }

        protected Action<object> execute = null;
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName]string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
        protected ICommand show;

        protected Microsoft.Win32.FileDialog fileDialog = null;

        protected static readonly DependencyProperty captionProperty = 
            DependencyProperty.Register(nameof(Caption), typeof(string), typeof(FileDialogBox));
        public static DependencyProperty commandParameterProperty = 
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(FileDialogBox));
        protected static readonly DependencyProperty filePathProperty =
            DependencyProperty.Register(nameof(FilePath), typeof(string), typeof(FileDialogBox));
        protected static readonly DependencyProperty filterProperty =
            DependencyProperty.Register(nameof(Filter), typeof(string), typeof(FileDialogBox));
        protected static readonly DependencyProperty filterIndexProperty =
            DependencyProperty.Register(nameof(FilterIndex), typeof(int), typeof(FileDialogBox));
        protected static readonly DependencyProperty defaultExtensionProperty =
            DependencyProperty.Register(nameof(DefaultExtension), typeof(string), typeof(FileDialogBox));
        protected static readonly DependencyProperty commandFileOkProperty =
            DependencyProperty.Register(nameof(CommandFileOk), typeof(ICommand), typeof(FileDialogBox));

        public FileDialogBox()
        {
            execute = o =>
            {
                fileDialog.Title = Caption;
                fileDialog.Filter = Filter;
                fileDialog.FilterIndex = FilterIndex;
                fileDialog.DefaultExt = DefaultExtension;

                string filePath = string.Empty;
                if (FilePath != null) filePath = FilePath;
                else FilePath = string.Empty;
                if (o != null) filePath = (string)o;
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    fileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(filePath);
                    fileDialog.FileName = System.IO.Path.GetFileName(filePath);
                }
                FileDialogResult = fileDialog.ShowDialog();
                NotifyPropertyChanged(nameof(FileDialogResult));
                if (FileDialogResult.HasValue && FileDialogResult.Value)
                {
                    FilePath = fileDialog.FileName;
                    NotifyPropertyChanged(nameof(FilePath));
                    object commandParameter = CommandParameter;
                    if (commandParameter == null) commandParameter = FilePath;
                    if (CommandFileOk != null)
                        if (CommandFileOk.CanExecute(commandParameter))
                            CommandFileOk.Execute(commandParameter);
                }
            };
        }
    }

    public class OpenFileDialog : FileDialogBox
    {
        public OpenFileDialog()
        {
            fileDialog = new Microsoft.Win32.OpenFileDialog();
        }
    }
    
    public class SaveFileDialog : FileDialogBox
    {
        public SaveFileDialog()
        {
            fileDialog = new Microsoft.Win32.SaveFileDialog();
        }
    }
}
