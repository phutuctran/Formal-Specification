
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Formal_Specification_Project {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {


        IOFunctions IOFunc = new IOFunctions();
        BuildFunctions buildFunc;

        int currentLang = 1;

        public MainWindow() {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) {
            System.Windows.Application.Current.Shutdown();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e) {
            GenerateCode();

        }

        private void textblock_generate_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            GenerateCode();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e) {
            IOFunc.saveFileInput("File input chưa được lưu, lưu fule?", textEditorInput.Text);
            IOFunc.saveFileOutputCpp("File output C++ chưa được lưu, lưu file?", textEditorOutput.Text);
            ResetAll();
            
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog opFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (opFileDialog.ShowDialog() == true) {
                string str = File.ReadAllText(opFileDialog.FileName);
                IOFunc.saveFileInput("File input cũ chưa được lưu, lưu file?", textEditorInput.Text);
                if (currentLang == 1)
                {
                    IOFunc.saveFileOutputCpp("File output C++ chưa được lưu, lưu file?", textEditorOutput.Text);
                }
                else
                {
                    IOFunc.saveFileOutputJS("File output JavaScript chưa được lưu, lưu file?", textEditorOutput.Text);
                }
                
                ResetAll();
                textEditorInput.Text = str;
                IOFunc.inputInFile = opFileDialog.FileName;
                IOFunc.lastInput = str;
                tblInputFile.Text = Path.GetFileName(IOFunc.inputInFile);
            }
        }

        private void btnRedo_Click(object sender, RoutedEventArgs e) {
            textEditorInput.Redo();
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e) {
            textEditorInput.Undo();
        }


        void GenerateCode() {
            //MessageBox.Show(currentLang.ToString());
            MainProcess mainProcess = new MainProcess();
            mainProcess.InputTextBox = textEditorInput.Text;
            mainProcess.FSProcess();
            if (currentLang == 1)
            {
                textEditorOutput.Text = mainProcess.OutputCodeCpp;
            }
            else
            {
                textEditorOutput.Text = mainProcess.OutputCodeJS;
            }
            
        }

        private void btnCut_Click(object sender, RoutedEventArgs e) {
            if (textEditorInput.SelectedText != "") {
                textEditorInput.Cut();
            }
        }

        private void btnCoppy_Click(object sender, RoutedEventArgs e) {
            if (textEditorInput.SelectionLength > 0) {
                textEditorInput.Copy();
            }
            if (textEditorOutput.SelectionLength > 0) {
                textEditorOutput.Copy();
            }
        }

        private void btnPaste_Click(object sender, RoutedEventArgs e) {
            textEditorInput.Paste();
        }

        private void textEditorInput_GotFocus(object sender, RoutedEventArgs e) {
            textEditorOutput.SelectionLength = 0;
        }

        private void textEditorOutput_GotFocus(object sender, RoutedEventArgs e) {
            textEditorInput.SelectionLength = 0;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            if (IOFunc.saveFileInput("Lưu file Input?", textEditorInput.Text))
            {
                MessageBox.Show("Đã lưu!", "Thông báo!");
                tblInputFile.Text = Path.GetFileName(IOFunc.inputInFile);
            }
            if (currentLang == 1)
            {
                if (IOFunc.saveFileOutputCpp("Lưu file Output?", textEditorOutput.Text))
                {
                    MessageBox.Show("Đã lưu!", "Thông báo!");
                    tblInputFile.Text = Path.GetFileName(IOFunc.outputSavePathCpp);
                }
            }
            else
            {
                if (IOFunc.saveFileOutputJS("Lưu file Output?", textEditorOutput.Text))
                {
                    MessageBox.Show("Đã lưu!", "Thông báo!");
                    tblInputFile.Text = Path.GetFileName(IOFunc.outputSavePathJS);
                }
            }

        }
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.ShowDialog();
        }

        void ResetAll()
        {
            IOFunc = new IOFunctions();
            textEditorInput.Text = "";
            textEditorOutput.Text = "";
            tblInputFile.Text = "Unsaved!";
            tblSourceFile.Text = "Unsaved!";
        }

        private void btnBuild_Click(object sender, RoutedEventArgs e)
        {
            Build();

        }

        private void tblInputFileName_TargetUpdated(object sender, DataTransferEventArgs e)
        {

            TextBlock tb = (TextBlock)sender;
            if (string.IsNullOrEmpty(tb.Text))
            {
                tb.Text = "Unsaved!";
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Build();
        }

        void Build()
        {
            if (string.IsNullOrEmpty(textEditorOutput.Text))
            {
                return;

            }
            string savePath = "";
            if (currentLang == 1)
            {
                if (IOFunc.saveFileOutputCpp("File output C++ chưa được lưu, lưu file?", textEditorOutput.Text))
                {
                    tblInputFile.Text = Path.GetFileName(IOFunc.outputSavePathCpp);
                    savePath = IOFunc.outputSavePathCpp;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (IOFunc.saveFileOutputJS("File output JS chưa được lưu, lưu file?", textEditorOutput.Text))
                {
                    tblInputFile.Text = Path.GetFileName(IOFunc.outputSavePathJS);
                    savePath = IOFunc.outputSavePathJS;
                }
                else
                {
                    return;
                }
            }
            buildFunc = new BuildFunctions();
            buildFunc.CurrentLanguage = currentLang;
            buildFunc.SetSavePath(savePath);
            buildFunc.Build();
            
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove(); 
        }


        private void ToggleButton_Language_Click(object sender, RoutedEventArgs e)
        {
            if (ToggleButton_Language.IsChecked == true)
            {
                textEditorOutput.SyntaxHighlighting = (IHighlightingDefinition)(new HighlightingDefinitionTypeConverter().ConvertFrom("JavaScript"));
                currentLang = 2;
                GenerateCode();
            }
            else
            {
                textEditorOutput.SyntaxHighlighting = (IHighlightingDefinition)(new HighlightingDefinitionTypeConverter().ConvertFrom("C++"));
                currentLang = 1;
                GenerateCode();
            }
        }
    }
}
