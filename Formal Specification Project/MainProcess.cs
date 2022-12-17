using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Formal_Specification_Project
{
    class MainProcess
    {
		private string inputTextBox;

		public string InputTextBox {
			get { return inputTextBox; }
			set { inputTextBox = value; }
		}

		private string outputCodeCpp;

		public string OutputCodeCpp {
			get { return outputCodeCpp; }
			set { outputCodeCpp = value; }
		}

        private string  outputCodeJS;
                            
        public string  OutputCodeJS
        {
            get { return outputCodeJS; }
            set { outputCodeJS = value; }
        }





		public MainProcess(string inputTextBox = "") {
			this.inputTextBox = inputTextBox;
		}

		public void FSProcess() {
			if (!string.IsNullOrWhiteSpace(inputTextBox)) {
				Cpp_Function CppCode = new Cpp_Function(inputTextBox);
				CppCode.GenerateCodeCpp();
				outputCodeCpp = CppCode.CodeOutput;

                JS_Function JSCode = new JS_Function(inputTextBox);
                JSCode.GenerateCodeJS();
                outputCodeJS = JSCode.CodeOutput;
				
			}
			else {
				MessageBox.Show("Không có Input!!!", "Thông báo!");
			}
		}
	}

	class IOFunctions
	{
		public string lastInput;
		public string lastOutputCpp;
		public string lastOutputJS;
		public string inputInFile;
		public string outputSavePathCpp;
		public string outputSavePathJS;

		public IOFunctions()
		{
			lastInput = "";
			lastOutputCpp = "";
			lastOutputJS = "";
			inputInFile = "";
			outputSavePathCpp = "";
			outputSavePathJS = "";
		}

        public bool saveFileInput(string mess, string textInput)
        {
            if (string.Compare(textInput, lastInput) != 0)
            {
                if (!string.IsNullOrEmpty(inputInFile))
                {
                    File.WriteAllText(inputInFile, textInput);
                    lastInput = textInput;
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
                            File.WriteAllText(saveFileDialog.FileName, textInput);
                            lastInput = textInput;
                            return true;
                        }

                    }
                }

            }
            return false;
        }
        public bool saveFileOutputJS(string mess, string textEditorOutput)
        {
            if (string.Compare(textEditorOutput, lastOutputJS) != 0)
            {
                if (!string.IsNullOrEmpty(outputSavePathJS))
                {
                    File.WriteAllText(outputSavePathJS, textEditorOutput);
                    lastOutputJS = textEditorOutput;
                    return true;
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show(mess, "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = ("JavaScript file|*.js");

                        if (saveFileDialog.ShowDialog() == true)
                        {
                            outputSavePathJS = saveFileDialog.FileName;
                            File.WriteAllText(saveFileDialog.FileName, textEditorOutput);
                            lastOutputJS = textEditorOutput;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool saveFileOutputCpp(string mess, string textEditorOutput)
        {
            if (string.Compare(textEditorOutput, lastOutputCpp) != 0)
            {
                if (!string.IsNullOrEmpty(outputSavePathCpp))
                {
                    File.WriteAllText(outputSavePathCpp, textEditorOutput);
                    lastOutputCpp = textEditorOutput;
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
                            outputSavePathCpp = saveFileDialog.FileName;
                            File.WriteAllText(saveFileDialog.FileName, textEditorOutput);
                            lastOutputCpp = textEditorOutput;
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
            }
            return true;
        }
    }

    class BuildFunctions
    {


        private int currentLang; // 1 : C++, 2 : JS

        private string code;

        private string  outputSavePath;

        public string  OutputSavePath
        {
            get { return outputSavePath; }
            set { outputSavePath = value; }
        }


        public string Code
        {
            get { return code; }
            set { code = value; }
        }


        public int CurrentLanguage
        {
            get { return currentLang; }
            set { currentLang = value; }
        }

        public BuildFunctions()
        {
            currentLang = -1;
        }

        public void SetCurrentLanguage(int s)
        {
            currentLang = s;
        }

        public void  SetCurrentCode(string c)
        {
            code = c;
        }

        public void SetSavePath(string path)
        {
            outputSavePath = path;
        }

        public bool Build()
        {
            if (currentLang == 1)
            {
                return BuildCpp();
            }
            if (currentLang == 2)
            {
                return BuildJS();
            }
            return false;
        }

        private bool BuildCpp()
        {
            FileInfo fileInfo = new FileInfo(outputSavePath);
            string directoryFullPath = fileInfo.DirectoryName;
            string fileName = Path.GetFileNameWithoutExtension(outputSavePath);
            string extentionFile = fileInfo.Extension;
            string batFileName = directoryFullPath + @"\" + fileName + ".bat";
            string exeFilePath = directoryFullPath + @"\" + fileName + ".exe";
            if (File.Exists(exeFilePath))
            {
                try
                {
                    File.Delete(exeFilePath);
                }
                catch
                {
                    MessageBox.Show("Không thể build file!", "Thông báo!");
                    return false;
                }

            }

            File.WriteAllText(batFileName, $"cd /d {directoryFullPath} \ng++ {fileName + extentionFile} -o {fileName + ".exe"} \n");

            string batPathWithoutSpace = batFileName.Replace(" ", "^ ");
            //MessageBox.Show(batFileName);
            Process proc = Process.Start("cmd.exe", "/c" + batPathWithoutSpace);
            proc.WaitForExit();
            //proc.Close();
            File.Delete(batFileName);
            try
            {
                Process.Start(directoryFullPath + "\\" + fileName + ".exe");
            }
            catch
            {
                MessageBox.Show("Không thể chạy file .exe", "Thông báo!");
                return false;
            }
            return true;
        }

        private bool BuildJS()
        {
            //MessageBox.Show(outputSavePath);
            if (File.Exists(outputSavePath))
            {
                try
                {
                    //MessageBox.Show(outputSavePath);
                    Process.Start("cmd.exe", "/c node " + outputSavePath.Replace(" ", "^ ")) ;
                    return true;
                }
                catch
                {
                    MessageBox.Show("Không thể thực thi!", "Thông báo!");
                    return false;
                }
            }
            MessageBox.Show("Không thể thực thi!", "Thông báo!");
            return false;
        }



    }
}
