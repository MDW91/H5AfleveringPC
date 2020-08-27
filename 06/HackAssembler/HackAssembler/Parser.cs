using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace HackAssembler
{
    public class Parser
    {
        public string line;
        public string Filename;
        public string WriteFilename = "TestFileWrite.txt";
        public string NewFilename;
        public string instruction;
        public int lineNumber = -1;
        public int Symbol_Value = 16;
        public int iterations = 0;
        public string labelToSave;
        public string ACompBit;
        public string ACompBits;
        public string BinaryCode;

        public string Filepath = "C:/Users/mathi/OneDrive/Skrivebord/nand2tetris/projects/06/pong/";

        public Parser(string FileName)
        {
            Filename = FileName;
        }

        public void FileReader()
        {
        
            FileInfo file = new FileInfo(Filename);
            StreamReader sr = new StreamReader(Filepath + Filename);
            StreamWriter sw = new StreamWriter(Filepath + WriteFilename);


            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                HandleLine(line, sw);
                lineNumber++;
                
            }
            sw.Close();
            sr.Close();

        }

        public void LabelReader()
        {
            FileInfo file = new FileInfo(Filename);
            StreamReader sr = new StreamReader(Filepath + Filename);
            StreamWriter sw = new StreamWriter(Filepath + WriteFilename);

            //line = sr.ReadLine();

            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                checkLabel(line);
                lineNumber++;

            }
            sw.Close();
            sr.Close();
            lineNumber = 0;
        }

        private void HandleLine(string line, StreamWriter sw)
        {
            // remove whitespace's and whitespace between characters

            line = line.Trim().Replace(" ", "");

            if (checkSkipLine(line))
            {
                return;
            }
            //checkLabel(line);

            string A_value = checkIfA_Command(line);
            if (A_value != null)
            {
                HandleAInstruction(A_value, sw);
            }
            else
            {
                HandleCInstruction(line, sw);
            }
        }

        public void AddtoSymbolTable(string value)
        {
            bool isNumber = int.TryParse(value, out int numericValue);

            bool isSymbol = SymbolTable.symbols.TryGetValue(value, out numericValue);

            if (!isSymbol)
            {
                numericValue = Symbol_Value;
                SymbolTable.symbols.Add(value, numericValue);
                Symbol_Value++;
            }
        }

        private void HandleAInstruction(string value, StreamWriter sw)
        {

            //AddtoSymbolTable(value);

            //get numbers/ chars of A-instruction after @ symbol
            bool isNumber = int.TryParse(value, out int numericValue);

            if (!isNumber)
            {
                bool isSymbol = SymbolTable.symbols.TryGetValue(value, out numericValue);
                // if symbol dont exist add to table
                string checkLabel = Regex.Replace(line, @"[^a-zA-Z]", "");
                bool islabel = SymbolTable.labelTable.ContainsKey(checkLabel);
                if (!isSymbol && !islabel)
                {
                    numericValue = Symbol_Value;
                    SymbolTable.symbols.Add(value, numericValue);
                    Symbol_Value++;
                }
            }
            string binaryCode = "0";
            for (int i = 1; i <= 15; i++)
            {
                int bit = (int)Math.Pow(2, 15 - i);
                if (bit <= numericValue)

                {
                    binaryCode += "1";
                    numericValue -= bit;
                }
                else
                {
                    binaryCode += "0";
                }
            }

            value = value.Replace("_", "");
            bool test = SymbolTable.labelTable.ContainsKey(value);
            SymbolTable.labelTable.TryGetValue(value, out int labelNr);
            if (SymbolTable.labelTable.ContainsKey(value))
            {
                binaryCode = Convert.ToString(labelNr, 2);
            }
            if (binaryCode.Length < 16)
            {
                //int Length = binaryCode.Length;
                for (int i = binaryCode.Length; i < 16; i++)
                {
                    binaryCode = "0" + binaryCode;
                }
            }
            sw.WriteLine(binaryCode);
        }

        private void HandleCInstruction(string value, StreamWriter sw)
        {
            string destination1;
            string jump;

            int index = line.LastIndexOf("/");
            if (index > 0)
                line = line.Substring(0, index); // or index + 1 to keep slash
            
            string opcode = "111";

            string jmpBits = "000";
            
            string[] destBits = { "0", "0", "0" };
            string compchars = "";
            string compBits = "";
            

            if (line.Contains("="))
            {


     
                    Regex splitEquals = new Regex(@"^([^=]+)=([^=].*)");
                    //Regex splitEquals = new Regex(@"^([^=]+)");
                    var match = splitEquals.Match(line);
                    
                    if (match.Success)
                    {
                        destination1 = match.Groups[1].Value;
                        compchars = match.Groups[2].Value;
                        compchars = compchars.Replace("= ", "");
                        compchars = compchars.Replace(" ", "");
                        compchars = compchars.Replace("/", "");
                        compchars = compchars.Replace("\t", "");

                        jump = match.Groups[3].Value;
                    }
                    else
                    {
                        destination1 = null;
                        jump = null;
                    }

                    if (destination1 != null && jump != null )
                    {
                        if (destination1.Contains("A"))
                        {
                            destBits[0] = "1";
                        }
                        if (destination1.Contains("D"))
                        {
                            destBits[1] = "1";
                        }
                        if (destination1.Contains("M"))
                        {
                            destBits[2] = "1";
                        }
                    }

                //compchars = line.ToString();
                compBits = SymbolTable.compTable[compchars];
            }
            else if(line.Contains(";"))
            {
                line = line.Replace(" ", "");
                Regex splitEquals = new Regex(@"^([^;]+);([^;]+)");
                //Regex splitEquals = new Regex(@"^([^;]+)");
                var match = splitEquals.Match(line);

                if (match.Success)
                {
                    destination1 = match.Groups[1].Value;
                    //compchars = match.Groups[2].Value;

                    jump = match.Groups[2].Value;
                    jump = jump.Replace("= ", "");
                    jump = jump.Replace(" ", "");
                    jump = jump.Replace("/", "");
                    jump = jump.Replace("\t", "");
                }
                else
                {
                    destination1 = null;
                    jump = null;
                }


                jmpBits = SymbolTable.JmpTable[jump];
                compBits = SymbolTable.compTable[destination1];
            }
            else
            {
                if (line.Contains("-"))
                {
                        
                    compchars = line;
                    compchars = compchars.Replace(" ", "");

                    compBits = SymbolTable.compTable[compchars];

                }

            }
            
            if (!line.Contains("(") && !line.Contains(")"))
            {

                 string tempDestBits = String.Join("", destBits);
                 BinaryCode = opcode + compBits + tempDestBits + jmpBits;
                 sw.WriteLine(BinaryCode);
            }


        }


        private string checkIfA_Command(string line)
        {
            // regex for a-instructions of form @value
            Regex a_instruction = new Regex(@"^(@)([^/\s]+)(.*)$");
            var match = a_instruction.Match(line);
            if (match.Success)
            {
                // get the value symbol or number
                return match.Groups[2].Value;
            }
            return null;
        }

        private bool checkLabel(string line)
        {
            // regex for assembly labels e.g. (LOOP)
            int index1 = line.LastIndexOf("/");
            if (index1 > 0)
                line = line.Substring(0, index1); // or index + 1 to keep slash


            Regex label = new Regex(@"^\(.+\)");
            if (line == "" || line == "/")
            {
                lineNumber = lineNumber - 1;
            }

            if (label.IsMatch(line))
            {
                int index = line.LastIndexOf(")");
                if (index > 0)
                    line = line.Substring(0, index); // or index + 1 to keep slash

                labelToSave = line;
                labelToSave = Regex.Replace(line, @"[^a-zA-Z]", "");

                if (!SymbolTable.labelTable.ContainsKey(labelToSave))
                {
                    int labelLineNr = lineNumber;
                    SymbolTable.labelTable.Add(labelToSave, labelLineNr);
                }

                lineNumber = lineNumber - 1;
                return true;
            }
            return false;
        }

        // regex conditional, check if we should skip line of code
        private bool checkSkipLine(string line)
        {
            Regex blank = new Regex(@"^\s?\r?\n?$");
            Regex comment = new Regex(@"^//.*$");
            // if whitespace or comment, return without writing to file
            if (comment.IsMatch(line) || blank.IsMatch(line))
            {
                return true;
            }
            return false;
        }
    }
}
