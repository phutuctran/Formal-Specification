using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formal_Specification_Project
{
    class JS_Function
    {
        private int currentType;

        private string fsInput;

        public string FsInput
        {
            get { return fsInput; }
            set { fsInput = value; }
        }


        private string codeOutput;

        private struct InputType2
        {
            public string countVar;
            public string arrayName;
            public string arrayType;
        }

        public string CodeOutput
        {
            get { return this.codeOutput; }
        }

        public JS_Function(string fsInput = "")
        {
            this.fsInput = fsInput;
        }

        public void GenerateCodeJS()
        {
            //Xuli input
            InputProcess inputProcess = new InputProcess(fsInput);


            //Xuli dong dau
            inputProcess.SplitInput();
            inputProcess.InitProcess();
            string funcName = inputProcess.FuncName;
            currentType = inputProcess.CurrentType;
            List<(string varName, string varType)> listInput = inputProcess.ListInput;
            (string varName, string varType) result = inputProcess.Result;


            //***Create Code***
            //them ham nhap
            this.codeOutput = InitProgram;
            Dictionary<string, bool> enterFuncCheck = new Dictionary<string, bool>();
            enterFuncCheck.Add("int", false);
            enterFuncCheck.Add("double", false);
            enterFuncCheck.Add("natural", false);
            enterFuncCheck.Add("bool", false);
            enterFuncCheck.Add("string", false);
            foreach ((string varName, string varType) var in listInput)
            {
                //string str = "";
                switch (var.varType)
                {
                    case "Z":
                        if (!enterFuncCheck["int"])
                        {
                            this.codeOutput = this.codeOutput + EnterIntNumberFunction;
                            enterFuncCheck["int"] = true;
                        }
                        break;
                    case "R":
                        if (!enterFuncCheck["double"])
                        {
                            this.codeOutput = this.codeOutput + EnterDoubleNumberFunction;
                            enterFuncCheck["double"] = true;
                        }
                        break;
                    case "R*":
                        if (!enterFuncCheck["double"])
                        {
                            this.codeOutput = this.codeOutput + EnterDoubleNumberFunction;
                            enterFuncCheck["double"] = true;
                        }
                        break;
                    case "N":
                        if (!enterFuncCheck["natural"])
                        {
                            this.codeOutput = this.codeOutput + EnterNaturalNumberFunction;
                            enterFuncCheck["natural"] = true;

                        }
                        break;
                        break;
                    case "N*":
                        if (!enterFuncCheck["natural"])
                        {
                            this.codeOutput = this.codeOutput + EnterNaturalNumberFunction;
                            enterFuncCheck["natural"] = true;

                        }
                        break;
                    case "B":
                        if (!enterFuncCheck["bool"])
                        {
                            this.codeOutput = this.codeOutput + EnterBoolVarFunction;
                            enterFuncCheck["bool"] = true;

                        }
                        break;
                    case "char*":
                        if (!enterFuncCheck["string"])
                        {
                            this.codeOutput = this.codeOutput + EnterStringFunction;
                            enterFuncCheck["string"] = true;

                        }
                        break;
                }
            }

            //tao ham xu li chinh
            string processFunc = "";
            //kieu du lieu cua ham
            processFunc = "function";
            //them ten ham
            processFunc = processFunc + " " + funcName;
            //them input cho ham
            string _strInput = "";
            foreach ((string varName, string varType) var in listInput)
            {
                if (!string.IsNullOrEmpty(_strInput))
                {
                    _strInput = _strInput + ", ";
                }
                _strInput = _strInput + var.varName;
            }
            if (currentType == 1)
            {
                processFunc = processFunc + "(" + _strInput + ")\r\n{\r\n" + AddTabChar(1) + "let " + inputProcess.Result.resultVarName + ";\r\n" + PostProcessType1(inputProcess.PostFunc, result, 1) + AddTabChar(1) + "return " + inputProcess.Result.resultVarName + ";\r\n}\r\n\r\n";
            }
            else
            {
                processFunc = processFunc + "(" + _strInput + ")\r\n{\r\n" + PostProcessType2(inputProcess.PostFunc, result, 1) + "}\r\n\r\n";
            }


            this.codeOutput = this.codeOutput + processFunc;

            string mainFunc = "function main()\r\n{\r\n" + MainFuncProcess(inputProcess.PreFunc, listInput, result, funcName) + "}\r\n\r\n";

            this.codeOutput = this.codeOutput + mainFunc + "main()\r\nconsole.log('Press any key to continue.');\r\nprocess.stdin.once('data', function () {});";


        }

        private string MainFuncProcess(string preStr, List<(string varName, string varType)> listInput, (string varName, string varType) result, string funcName)
        {
            string strMain = "";
            foreach ((string varName, string varType) var in listInput)
            {
                strMain = strMain + AddTabChar(1) + "let " + var.varName + ChangeFSTypetoCppType(var.varType) + ";\r\n";
            }

            if (currentType == 1)
            {
                if (!string.IsNullOrEmpty(preStr))
                {
                    strMain = strMain + "    let check = false;\r\n" + "    do\r\n    {\r\n";
                    strMain = strMain + InputCodeType1(listInput, 2);
                    strMain = strMain + "        if " + preStr + "\r\n        {\r\n            check = true;\r\n        }\r\n        else\r\n        {\r\n            console.log(\"Khong thoa dieu kien nhap!!!\\n\");\r\n        }\r\n";
                    strMain = strMain + "    }while(!check);\r\n";
                }
                else
                {
                    strMain = strMain + InputCodeType1(listInput, 1);
                }
            }
            else
            {
                strMain = strMain + InputCodeType2(listInput);
            }

            string strInput = "";
            foreach ((string varName, string varType) var in listInput)
            {
                if (!string.IsNullOrEmpty(strInput))
                {
                    strInput = strInput + ", " + var.varName;
                }
                else
                {
                    strInput = strInput + var.varName;
                }
            }

            strMain = strMain + "    console.log(\"Ket qua " + result.varName + " la: \" + " + funcName + "(" + strInput + "));\r\n\r\n";
            return strMain;
        }

        private string InputCodeType1(List<(string varName, string varType)> listInput, int Tablev)
        {
            string str = "";
            foreach (var s in listInput)
            {
                switch (s.varType)
                {
                    case "Z":
                        str = str + AddTabChar(Tablev) + s.varName + " = NhapSoNguyen(\"" + s.varName + "\");\r\n";
                        break;
                    case "R":
                        str = str + AddTabChar(Tablev) + s.varName + " = NhapSoThuc(\"" + s.varName + "\");\r\n";
                        break;
                    case "N":
                        str = str + AddTabChar(Tablev) + s.varName + " = NhapSoTuNhien(\"" + s.varName + "\");\r\n";
                        break;
                    case "B":
                        str = str + AddTabChar(Tablev) + s.varName + " = NhapGiaTriDungSai(\"" + s.varName + "\");\r\n";
                        break;
                    case "char*":
                        str = str + AddTabChar(Tablev) + s.varName + " = NhapChuoi(\"" + s.varName + "\");\r\n";
                        break;
                }
            }
            return str;

        }
        private string InputCodeType2(List<(string varName, string varType)> listInput)
        {
            string str = "";

            InputType2 inputType2 = new InputType2();
            int idx;
            if (listInput[0].varType == "N")
            {
                idx = 1;
                inputType2.countVar = listInput[0].varName;
            }
            else
            {
                idx = 0;
                inputType2.countVar = listInput[1].varName;
            }
            inputType2.arrayName = listInput[idx].varName;
            inputType2.arrayType = listInput[idx].varType;

            str = str + AddTabChar(1) + inputType2.countVar + " = NhapSoTuNhien(\"" + inputType2.countVar + "\");\r\n";
            str = str + $"    {inputType2.arrayName}.length = {inputType2.countVar} + 1;\r\n    for (let i = 1; i <= {inputType2.countVar}; i++)\r\n" + "    {\r\n" + $"        let str = \"{inputType2.arrayName}[\" + i + \"]\";\r\n        {inputType2.arrayName}[i] = " + ((inputType2.arrayType == "R*") ? "NhapSoThuc" : "NhapSoTuNhien") + "(str);\r\n" + "    }\r\n";

            return str;

        }


        private string PostProcessType1(string postStr, (string varName, string varType) result, int lev)
        {
            string strTmp = "";
            List<string> AssignmentOperator = new List<string>();
            Stack<int> idxOpenBracket = new Stack<int>();
            if (string.IsNullOrEmpty(postStr))
            {
                return "";
            }
            for (int i = 0; i < postStr.Length; i++)
            {
                if (postStr[i] == '(')
                {
                    idxOpenBracket.Push(i);
                }
                else if (postStr[i] == ')')
                {
                    int idxOpen = idxOpenBracket.Pop();
                    string tmp = postStr.Substring(idxOpen + 1, i - idxOpen - 1);
                    if (IsTheAssignmentOperator(tmp, result))
                    {
                        AssignmentOperator.Add((tmp));
                    }
                    if (IsAOperator(tmp))
                    {
                        if (!IsTheAssignmentOperator(tmp, result))
                        {
                            if (IsTheEqualityOperator(tmp, result))
                            {
                                postStr = postStr.Insert(idxOpen + tmp.IndexOf("=") + 1, "==");
                                i += 2;
                            }
                            if (IsTheNotEqualityOperator(tmp))
                            {
                                postStr = postStr.Insert(idxOpen + tmp.IndexOf("=") + 1, "=");
                                i++;
                            }

                        }
                        //Console.WriteLine(tmp);
                        postStr = postStr.ReplaceAt(idxOpen, ' ');
                        postStr = postStr.ReplaceAt(i, ' ');
                        //i -= 2;

                    }
                    if (idxOpenBracket.Count == 0)
                    {
                        string postStrNonCheck = postStr.Substring(i + 1);
                        postStr = postStr.Substring(0, i + 1);
                        if (AssignmentOperator.Count > 0)
                        {
                            foreach (var s in AssignmentOperator)
                            {
                                int idx = postStr.IndexOf(s);
                                if (postStr.Length >= idx + s.Length + 2 && postStr[idx + s.Length + 1] == '&' && postStr[idx + s.Length + 2] == '&')
                                {
                                    postStr = postStr.Remove(idx - 1, s.Length + 4); //Xoas && o sau
                                }
                                else if (idx > 1 && postStr[idx - 2] == '&' && postStr[idx - 3] == '&')
                                {
                                    postStr = postStr.Remove(idx - 2, s.Length + 4);// Xoa && o truoc
                                }
                                else
                                {
                                    postStr = postStr.Remove(idx, s.Length);

                                }
                            }
                            // truong hop kh co dieu kien so sanh o post
                            if (postStr.Replace(" ", String.Empty).Length == 0)
                            {
                                foreach (var s in AssignmentOperator)
                                {
                                    strTmp = strTmp + AddTabChar(lev) + s + ";\r\n";
                                }
                                break;
                            }
                            strTmp = strTmp + IfFuncTemplate(postStr, AssignmentOperator, lev);
                            if (postStrNonCheck.Length > 0)
                            {
                                if (postStrNonCheck[0] == '|' && postStrNonCheck[1] == '|')
                                {
                                    strTmp = strTmp + AddTabChar(lev) + "else\r\n" + AddTabChar(lev) + "{\r\n" + PostProcessType1(postStrNonCheck.Substring(2), result, lev + 1) + AddTabChar(lev) + "}\r\n";
                                }
                                else if (postStrNonCheck[0] == '&' && postStrNonCheck[1] == '&')
                                {
                                    ///////////////////////////
                                }
                            }
                            break;
                        }
                    }
                    else
                    {
                        ////////////////////
                    }

                }
            }


            return strTmp;
        }

        /// ////////////////////////////////

        private string PostProcessType2(string postStr, (string varName, string varType) result, int lev)
        {

            string str = "";
            int idxFirstOpenBracket = postStr.IndexOf('(');
            int idxLastCloseBracket = postStr.IndexOf('}');
            string strTmp = postStr.Substring(idxFirstOpenBracket + 1, idxLastCloseBracket - idxFirstOpenBracket).Trim();
            var forCode = getLoopCode(strTmp);
            postStr = postStr.Substring(idxLastCloseBracket + 2, postStr.Length - idxLastCloseBracket - 3);

            str = str + AddTabChar(1) + forCode.forStr + AddTabChar(1) + "{\r\n";

            if (postStr.IndexOf('{') == -1 && postStr.IndexOf('}') == -1)
            {
                postStr = CreateCon(postStr, result);
                if (forCode.type == 1)
                {
                    strTmp = $"        if ({postStr})\r\n" + "        {\r\n            //notthing\r\n        }\r\n        else\r\n        {\r\n            return false;\r\n        }\r\n    }\r\n    return true;\r\n";
                }
                else
                {
                    strTmp = $"        if ({postStr})\r\n" + "        {\r\n            return true;\r\n        }\r\n    }\r\n    return false;\r\n";
                }
            }
            else
            {
                idxLastCloseBracket = postStr.IndexOf('}');
                strTmp = postStr.Substring(0, idxLastCloseBracket + 1).Trim();
                postStr = postStr.Substring(idxLastCloseBracket + 2, postStr.Length - idxLastCloseBracket - 2);
                postStr = CreateCon(postStr, result);
                var forCode2 = getLoopCodeLev2(strTmp, postStr);
                if (forCode.type == 1)
                {
                    strTmp = forCode2.forStr + "        if (!flag)\r\n        {\r\n            return false;\r\n        }\r\n\r\n    }\r\n    return true;\r\n";
                }
                else
                {
                    strTmp = forCode2.forStr + "        if (flag)\r\n        {\r\n            return true;\r\n        }\r\n\r\n    }\r\n    return false;\r\n";
                }
            }
            str = str + strTmp;

            return str;
        }

        private string CreateCon(string postStr, (string varName, string varType) result)
        {
            Stack<int> stack = new Stack<int>();
            for (int i = 0; i < postStr.Length; i++)
            {
                if (postStr[i] == '(')
                {
                    stack.Push(i);
                }
                if (postStr[i] == ')')
                {
                    int idx = stack.Pop();
                    if (idx == 0 && i == postStr.Length - 1)
                    {
                        postStr = postStr.Remove(0, 1).Remove(postStr.Length - 2, 1);
                        break;
                    }

                    string str = postStr.Substring(idx + 1, i - idx - 1);
                    if (IsAOperator(str))
                    {
                        if (IsTheEqualityOperator(str, result))
                        {
                            postStr = postStr.Insert(idx + str.IndexOf('=') + 1, "==");
                            i += 2;
                        }
                        if (IsTheNotEqualityOperator(str))
                        {
                            postStr = postStr.Insert(idx + str.IndexOf('=') + 1, "=");
                            i++;
                        }
                    }
                    else
                    {
                        if (!HaveOperator(str))
                        {
                            postStr = postStr.ReplaceAt(i, ']').ReplaceAt(idx, '[');
                        }

                    }
                }
            }
            //truong hop khong co ngoac lon
            for (int i = 0; i < postStr.Length; i++)
            {
                if (postStr[i] == '=' && postStr[i - 1] != '!' && postStr[i - 1] != '<' && postStr[i - 1] != '>' && postStr[i + 1] != '=' && postStr[i - 1] != '=')
                {
                    postStr = postStr.Insert(i, "==");
                    i += 2;
                }

            }
            for (int i = 0; i < postStr.Length; i++)
            {
                if (postStr[i] == '!' && postStr[i + 1] == '=')
                {
                    postStr = postStr.Insert(i + 1, "=");
                    i++;
                }
            }
            return postStr;
        }

        private (string forStr, int type) getLoopCodeLev2(string strLoop, string con)
        {
            var tmp = getLoopCode(strLoop);
            string strTmp = "";
            if (tmp.type == 1)
            {
                strTmp = $"            if ({con})\r\n" + "            {\r\n                //notthing\r\n            }\r\n            else\r\n            {\r\n                flag = false;\r\n                break;\r\n            }\r\n";
                tmp.forStr = "        let flag = true;\r\n        " + tmp.forStr;
            }
            else
            {
                strTmp = $"            if ({con})\r\n" + "            {\r\n                flag = true;\r\n                break;\r\n            }\r\n";
                tmp.forStr = "        let flag = false;\r\n        " + tmp.forStr;
            }
            tmp.forStr = tmp.forStr + "        {\r\n" + strTmp + "        }\r\n";
            return tmp;
        }

        private (string forStr, int type) getLoopCode(string strTmp)
        {

            //Lấy tên biến đếm
            string idxName = (strTmp.IndexOf("VM") != -1 ? strTmp.Substring(strTmp.IndexOf("VM") + 2, strTmp.IndexOf("TH") - strTmp.IndexOf("VM") - 2) : strTmp.Substring(strTmp.IndexOf("TT") + 2, strTmp.IndexOf("TH") - strTmp.IndexOf("TT") - 2)).Trim();

            //Lấy loại trường hợp xét: VM : 1; TT : 2
            int type = strTmp.IndexOf("VM") != -1 ? 1 : 2;

            //Lấy khoảng đếm
            string tmp = strTmp.Substring(strTmp.IndexOf("{") + 1, strTmp.IndexOf("}") - strTmp.IndexOf("{") - 1);
            string startIdx = tmp.Substring(0, tmp.IndexOf("..")).Trim();
            string endIdx = tmp.Substring(tmp.IndexOf("..") + 2).Trim();
            string str = $"for (let {idxName} = {startIdx}; {idxName} <= {endIdx}; {idxName}++)\r\n";
            return (str, type);

        }

        private string IfFuncTemplate(string condition, List<string> AssignmentOperator, int NumTab)
        {
            if (condition[0] == '(' && condition[1] == ' ')
            {
                condition = condition.Remove(1, 1);
            }
            if (condition[condition.Length - 1] == ')' && condition[condition.Length - 2] == ' ')
            {
                condition = condition.Remove(condition.Length - 2, 1);
            }

            string str = AddTabChar(NumTab) + "if " + condition + "\r\n";
            str = str + AddTabChar(NumTab) + "{\r\n";
            foreach (var s in AssignmentOperator)
            {
                str = str + AddTabChar(NumTab + 1) + s + ";\r\n";
            }
            str = str + AddTabChar(NumTab) + "}\r\n";
            return str;

        }

        private string AddTabChar(int num)
        {
            string str = "";
            for (int i = 0; i < num; i++)
            {
                str = str + "    ";
            }
            return str;
        }

        private string ChangeFSTypetoCppType(string type)
        {
            switch (type)
            {
                case "R*":
                    return " = []";
                case "N*":
                    return "= []";
                default:
                    return "";
            }

        }

        private bool HaveOperator(string str)
        {
            return str.IndexOf('=') >= 0
                || str.IndexOf(">") >= 0
                || str.IndexOf('<') >= 0
                || str.IndexOf('!') >= 0
                || str.IndexOf("&&") >= 0
                || str.IndexOf("||") >= 0;
        }

        private bool IsTheNotEqualityOperator(string _operator)
        {
            return _operator.IndexOf("!=") != -1;
        }
        private bool IsTheEqualityOperator(string _operator, (string varName, string varType) result)
        {
            return _operator.IndexOf("=") != -1 &&
               _operator.IndexOf(">") == -1 &&
               _operator.IndexOf("<") == -1 &&
               _operator.IndexOf("!=") == -1 &&
               _operator.IndexOf(">=") == -1 &&
               _operator.IndexOf("<=") == -1 &&
               //_operator.IndexOf("!") == -1 &&
               _operator.IndexOf("&&") == -1 &&
               _operator.IndexOf("||") == -1 &&
               _operator.Substring(0, _operator.IndexOf("=")) != result.varName;
        }

        private bool IsTheAssignmentOperator(string _operator, (string varName, string varType) result)
        {
            return _operator.IndexOf("=") != -1 &&
                _operator.IndexOf(">") == -1 &&
                _operator.IndexOf("<") == -1 &&
                _operator.IndexOf("!=") == -1 &&
                _operator.IndexOf(">=") == -1 &&
                _operator.IndexOf("<=") == -1 &&
                //_operator.IndexOf("!") == -1 &&
                _operator.IndexOf("&&") == -1 &&
                _operator.IndexOf("||") == -1 &&
                _operator.Substring(0, _operator.IndexOf("=")) == result.varName;

        } //Check if a string is a assignment operator

        private bool IsAOperator(string _operator) //Check if a string is a operator
        {
            //Console.WriteLine(_operator);
            int num = ((_operator.IndexOf("=") != -1 &&
                        _operator.IndexOf(">=") == -1 &&
                        _operator.IndexOf("<=") == -1 &&
                        _operator.IndexOf("==") == -1 &&
                        _operator.IndexOf("!=") == -1) ? 1 : 0) +
                ((_operator.IndexOf("<") != -1 &&
                  _operator.IndexOf("<=") == -1) ? 1 : 0) +
                ((_operator.IndexOf(">") != -1 &&
                  _operator.IndexOf(">=") == -1) ? 1 : 0) +
                ((_operator.IndexOf("!=") == -1) ? 0 : 1) +
                ((_operator.IndexOf(">=") == -1) ? 0 : 1) +
                ((_operator.IndexOf("<=") == -1) ? 0 : 1) +
                //_operator.IndexOf("!") == -1 +
                (_operator.IndexOf("&&") == -1 ? 0 : 2) +
                (_operator.IndexOf("||") == -1 ? 0 : 2) +
                ((_operator.IndexOf("==") == -1) ? 0 : 1);
            return (num == 1 || (num == 0 && _operator.IndexOf("!") != -1));
        }

        public void setInput(string fsInput = "")
        {
            this.fsInput = fsInput;
        }

        private string InitProgram = "const prompt = require('prompt-sync')();\r\n\r\n";
        private string EnterIntNumberFunction = "function NhapSoNguyen (varaiableName){\r\n    let num;\r\n    do{\r\n        num = prompt(`Nhap so nguyen ${varaiableName} = `);\r\n        num = Number(num);\r\n    }while(!Number.isInteger(num));\r\n \r\n    return num;\r\n};\r\n\r\n";
        private string EnterDoubleNumberFunction = "function isNumber( num){\r\n    if(typeof num !== 'string'){\r\n        return false;\r\n    }\r\n    if(num.trim() === ''){\r\n        return false;\r\n    }\r\n    return !isNaN(num);\r\n};\r\n\r\n function NhapSoThuc(varaiableName ){\r\n    let num;\r\n    do{\r\n        num = prompt(`Nhap so thuc ${varaiableName} = `);\r\n    }while(!isNumber(num.trim()));\r\n    return parseFloat(num.trim());\r\n };\r\n\r\n";
        private string EnterNaturalNumberFunction = "function NhapSoTuNhien (varaiableName){\r\n    let num;\r\n    do{\r\n        num = prompt(`Nhap so tu nhien ${varaiableName} = `);\r\n        num = Number(num);\r\n    }while(!Number.isInteger(num) || num < 0);\r\n    return num;\r\n};\r\n\r\n";
        private string EnterBoolVarFunction = "function NhapGiaTriDungSai(varaiableName ){\r\n    trueList  = ['T', 'D', 'DUNG', 'TRUE',  '1'];\r\n    falseList = ['S', 'SAI', 'F', 'FALSE', '0'];\r\n    do{\r\n        char = prompt(`Nhap gia tri dung sai (T/F, D/S, True/False, Dung/Sai) ${varaiableName} = `);\r\n        char = char.trim().toUpperCase();\r\n    }while(!trueList.includes(char) && !falseList.includes(char));\r\n    if(trueList.includes(char)){\r\n        return true;\r\n    }\r\n        return false;\r\n\r\n};\r\n\r\n";
        private string EnterStringFunction = "function NhapChuoi(varaiableName){\r\n    var char = prompt(`Nhap chuoi ${varaiableName} = `);\r\n    return char;\r\n};\r\n\r\n";
    }
}
