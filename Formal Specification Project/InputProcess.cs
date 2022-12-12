using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formal_Specification_Project
{
    internal class InputProcess {

        private int currentType;

        public int CurrentType {
            get { return currentType; }
            set { currentType = value; }
        }


        private List<(string varName, string varType)> listInput = new List<(string varName, string varType)>();
        public List<(string varName, string varType)> ListInput {
            get { return listInput; }
            set { listInput = value; }
        }

        private string fsInput = "";

        private string initFunc;
        public string InitFunc {
            get { return initFunc; }
            set { initFunc = value; }
        }

        private string preFunc;
        public string PreFunc {
            get { return preFunc; }
            set { preFunc = value; }
        }

        private string postFunc;
        public string PostFunc {
            get { return postFunc; }
            set { postFunc = value; }
        }

        private string funcName;
        public string FuncName {
            get { return funcName; }
            set { funcName = value; }
        }


        private (string resultVarName, string resultVarType) result;

        public (string resultVarName, string resultVarType) Result {
            get { return result; }
            set { result = value; }
        }


        public InputProcess(string input = "") {
            this.fsInput = input;
        }

        public void InitProcess() {
            this.funcName = this.initFunc.Substring(0, this.initFunc.IndexOf('('));
            string inputType_str = this.initFunc.Substring(this.initFunc.IndexOf('(') + 1, this.initFunc.IndexOf(')') - this.initFunc.IndexOf('(') - 1);
            //Console.WriteLine(inputType_str);
            string[] _listInput = inputType_str.Split(',');
            foreach (var str in _listInput) {
                string[] _tmp = str.Split(':');
                listInput.Add((_tmp[0], _tmp[1]));
            }
            string _result = this.initFunc.Substring(this.initFunc.IndexOf(')') + 1);
            string[] __tmp = _result.Split(':');
            this.Result = (__tmp[0], __tmp[1]);
        }

        public void SplitInput() {

            if (this.fsInput.IndexOf('{') != -1) {
                currentType = 2;
            }
            else {

                currentType = 1;
            }
            //Format
            this.fsInput = this.fsInput.Replace(" ", String.Empty);
            this.fsInput = this.fsInput.Replace("\t", String.Empty);
            this.fsInput = this.fsInput.Trim('\r');
            this.fsInput = this.fsInput.Trim('\n');
            this.fsInput = this.fsInput.Trim('\n');
            this.fsInput = this.fsInput.Trim('\r');
            int idxPre = 0, idxPost = 0;

            //Get initFunc
            do {
                idxPre = this.fsInput.IndexOf("pre", idxPre + 1);
                if (idxPre == -1) {
                    return;
                }
                if (idxPre == 0 || (this.fsInput[idxPre - 1] != '\n' || this.fsInput[idxPre - 2] != '\r')) {
                    continue;
                }
                else {
                    break;
                }
            } while (true);
            this.initFunc = this.fsInput.Substring(0, idxPre - 2);
            this.initFunc = this.initFunc.Replace("\r\n", String.Empty);

            //Console.WriteLine(this.initFunc);
            //Get pre Func
            do {
                idxPost = this.fsInput.IndexOf("post", idxPost + 1);
                if (idxPost == 0 || (this.fsInput[idxPost - 1] != '\n' || this.fsInput[idxPost - 2] != '\r')) {
                    continue;
                }
                else {
                    break;
                }
            } while (true);
            this.preFunc = this.fsInput.Substring(idxPre, idxPost - idxPre - 2);
            this.preFunc = this.preFunc.Replace("\r\n", String.Empty);
            if (this.preFunc.IndexOf("pre") == 0) {
                this.preFunc = this.preFunc.Remove(0, 3);
            }
            Stack<int> openBacket;
            bool flag;
            if (!string.IsNullOrWhiteSpace(this.preFunc))
            {
                openBacket = new Stack<int>();
                flag = false;
                for (int i = 0; i < this.preFunc.Length; i++)
                {
                    if (this.preFunc[i] == '(')
                    {
                        openBacket.Push(i);
                    }
                    if (this.preFunc[i] == ')')
                    {
                        int idx = openBacket.Pop();
                        if (idx == 0 && i == this.preFunc.Length - 1)
                        {
                            flag = true;
                        }
                    }
                }
                if (!flag)
                {
                    this.preFunc = '(' + this.preFunc + ')';
                }
            }
            

            //Console.WriteLine(this.preFunc);

            //Get post Func
            this.postFunc = this.fsInput.Substring(idxPost);
            this.postFunc = this.postFunc.Replace("\r\n", String.Empty);
            this.postFunc = this.postFunc.Replace("TRUE", "true");
            this.postFunc = this.postFunc.Replace("FALSE", "false");
            if (this.postFunc.IndexOf("post") == 0) {
                this.postFunc = this.postFunc.Remove(0, 4);
            }
            if (currentType == 1)
            {
                openBacket = new Stack<int>();
                flag = false;
                for (int i = 0; i < this.postFunc.Length; i++)
                {
                    if (this.postFunc[i] == '(')
                    {
                        openBacket.Push(i);
                    }
                    if (this.postFunc[i] == ')')
                    {
                        int idx = openBacket.Pop();
                        if (idx == 0 && i == this.postFunc.Length - 1)
                        {
                            flag = true;
                        }
                    }
                }
                if (!flag && this.postFunc.IndexOf("||") < 0)
                {
                    this.postFunc = '(' + this.postFunc + ')';
                }
            }
            

            //Console.WriteLine(this.postFunc);

        }
    }
}
