
using ICSharpCode.AvalonEdit;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

        string lastInput = "";
        string lastOutput = "";
        string inputInFile = "";
        string outputSavePath = "";

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
            if (string.Compare(lastInput, textEditorInput.Text) != 0)
            {
                if (saveFileInput("File input cũ chưa được lưu, lưu file?"))
                {
                    MessageBox.Show("Đã lưu!", "Thông báo!");
                }    
            }
            if (string.Compare(lastOutput, textEditorOutput.Text) != 0)
            {
                if (saveFileOutput("File output cũ chưa được lưu, lưu file?"))
                {
                    MessageBox.Show("Đã lưu!", "Thông báo!");
                }
            }
            ResetAll();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog opFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (opFileDialog.ShowDialog() == true) {
                string str = File.ReadAllText(opFileDialog.FileName);
                if (string.Compare(lastInput, textEditorInput.Text) != 0)
                {
                    saveFileInput("File input cũ chưa được lưu, lưu file?");
                }
                if (string.Compare(lastOutput, textEditorOutput.Text) != 0)
                {
                    saveFileOutput("File output cũ chưa được lưu, lưu file?");
                }
                ResetAll();
                textEditorInput.Text = str;
                inputInFile = opFileDialog.FileName;
                lastInput = str;
                tblInputFileName.Text = Path.GetFileName(inputInFile);
            }
        }

        private void btnRedo_Click(object sender, RoutedEventArgs e) {
            textEditorInput.Redo();
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e) {
            textEditorInput.Undo();
        }


        void GenerateCode() {
            MainProcess mainProcess = new MainProcess();
            mainProcess.InputTextBox = textEditorInput.Text;
            mainProcess.FSProcess();
            textEditorOutput.Text = mainProcess.OutputCode;
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
            if (saveFileInput("Lưu file Input?"))
            {
                MessageBox.Show("Đã lưu!", "Thông báo!");
                tblInputFileName.Text = Path.GetFileName(inputInFile);
            }
            if (saveFileOutput("Lưu file Output?"))
            {
                MessageBox.Show("Đã lưu!", "Thông báo!");
            }      
        }
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.ShowDialog();
        }

        void ResetAll()
        {
            textEditorInput.Text = "";
            textEditorOutput.Text = "";
            inputInFile = "";
            outputSavePath = "";
            lastInput = "";
            lastOutput = "";
            tblInputFileName.Text = "Unsaved!";
        }

        bool saveFileOutput(string mess)
        {
            if (string.Compare(textEditorOutput.Text, lastOutput) != 0)
            {
                if (!string.IsNullOrEmpty(outputSavePath))
                {
                    File.WriteAllText(outputSavePath, textEditorOutput.Text);
                    lastOutput = textEditorOutput.Text;
                    return true;
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show(mess, "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = ("C++ file|*.cpp");

                        if (saveFileDialog.ShowDialog() == true)
                        {
                            outputSavePath = saveFileDialog.FileName;
                            File.WriteAllText(saveFileDialog.FileName, textEditorOutput.Text);
                            lastOutput = textEditorOutput.Text;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        bool saveFileInput(string mess)
        {
            if (string.Compare(textEditorInput.Text, lastInput) != 0)
            {
                if (!string.IsNullOrEmpty(inputInFile))
                {
                    File.WriteAllText(inputInFile, textEditorInput.Text);
                    lastInput = textEditorInput.Text;
                    return true;
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show(mess, "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = ("Text file|*.txt");

                        if (saveFileDialog.ShowDialog() == true)
                        {
                            inputInFile = saveFileDialog.FileName;
                            File.WriteAllText(saveFileDialog.FileName, textEditorInput.Text);
                            lastInput = textEditorInput.Text;
                            return true;
                        }

                    }
                }

            }
            return false;
        }

        private void btnBuild_Click(object sender, RoutedEventArgs e)
        {
            Build();

        }

        private void tblInputFileName_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (string.IsNullOrEmpty(tblInputFileName.Text))
            {
                tblInputFileName.Text = "Unsaved!";
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
            if (string.Compare(textEditorOutput.Text, lastOutput) != 0)
            {
                if (saveFileOutput("Output chưa được lưu, lưu file?")) { }
                else
                {
                    return;
                }
            }

            FileInfo fileInfo = new FileInfo(outputSavePath);
            string directoryFullPath = fileInfo.DirectoryName;
            string fileName = Path.GetFileNameWithoutExtension(outputSavePath);
            string extentionFile = fileInfo.Extension;
            string batFileName = directoryFullPath + @"\" + fileName + ".bat";

            File.WriteAllText(batFileName, $"cd /d {directoryFullPath} \ng++ {fileName + extentionFile} -o {fileName + ".exe"} \n");

            string batPathWithoutSpace = batFileName.Replace(" ", "^ ");
            //MessageBox.Show(batFileName);
            Process proc = Process.Start("cmd.exe", "/c" + batPathWithoutSpace);
            proc.WaitForExit();
            File.Delete(batFileName);
            Process.Start(directoryFullPath + "\\" + fileName + ".exe");
        }

    }
}
