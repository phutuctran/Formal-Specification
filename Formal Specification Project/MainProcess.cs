using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		private string outputCode;

		public string OutputCode {
			get { return outputCode; }
			set { outputCode = value; }
		}

		private string  outputCode_Highlighting;

		public string  OutputCode_Highlighting {
			get { return outputCode_Highlighting; }
			set { outputCode_Highlighting = value; }
		}



		public MainProcess(string inputTextBox = "") {
			this.inputTextBox = inputTextBox;
		}

		public void FSProcess() {
			if (!string.IsNullOrWhiteSpace(inputTextBox)) {
				Cpp_Function CppCode = new Cpp_Function(inputTextBox);
				CppCode.GenerateCodeCpp();
				outputCode = CppCode.CodeOutput;
				
			}
			else {
				MessageBox.Show("Không có Input!!!", "Thông báo!");
			}
		}
	}
}
