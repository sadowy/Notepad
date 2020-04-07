using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Noter
{
    public class SelectionTextBox : TextBox
    {
        #region SelectedTextDP
        public string SelectedTextDP
        {
            get { return (string)GetValue(SelectedTextDPProperty); }
            set { SetValue(SelectedTextDPProperty, value); }
        }

        public static readonly DependencyProperty SelectedTextDPProperty =
            DependencyProperty.RegisterAttached("SelectedTextDP", typeof(string), typeof(SelectionTextBox), 
                new FrameworkPropertyMetadata(null, 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    SelectedTextChanged));

        private static void SelectedTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = obj as TextBox;
            if (textBox != null)
            {
                string newValue = e.NewValue as string;
                if (!string.IsNullOrEmpty(newValue) && newValue != textBox.SelectedText)
                {
                    textBox.SelectedText = newValue;
                    textBox.Focus();
                }
            }
        }
        #endregion
        #region SelectionStartDP
        public int SelectionStartDP
        {
            get { return (int)GetValue(SelectionStartDPProperty); }
            set { SetValue(SelectionStartDPProperty, value); }
        }

        public static readonly DependencyProperty SelectionStartDPProperty =
            DependencyProperty.RegisterAttached("SelectionStartDP", typeof(int), typeof(SelectionTextBox), 
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                    SelectionStartChanged));

        private static void SelectionStartChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = obj as TextBox;
            if(textBox != null)
            {
                int newValue = (int)e.NewValue;
                if(newValue != textBox.SelectionStart)
                {
                    textBox.SelectionStart = newValue;
                    textBox.Focus();
                }
            }
        }
        #endregion
        #region SelectionLenghthDP
        public int SelectionLengthDP
        {
            get { return (int)GetValue(SelectionLengthDPProperty); }
            set { SetValue(SelectionLengthDPProperty, value); }
        }
        public static readonly DependencyProperty SelectionLengthDPProperty =
            DependencyProperty.RegisterAttached("SelectionLengthDP", typeof(int), typeof(SelectionTextBox), 
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    SelectionLengthDPChanged));

        private static void SelectionLengthDPChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = obj as TextBox;
            if(textBox != null)
            {
                int newValue = (int)e.NewValue;
                if(newValue != textBox.SelectionLength)
                {
                    textBox.SelectionLength = newValue;
                    textBox.Focus();
                }
            }
        }
        #endregion
        public SelectionTextBox() : base()
        {
            this.SelectionChanged += selectionTextBoxSelectionChanged;
        }

        private void selectionTextBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            SelectedTextDP = SelectedText;
            SelectionStartDP = SelectionStart;
            SelectionLengthDP = SelectionLength;
        }
        

    }
}
