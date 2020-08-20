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
        public string A_Command;
        public string C_Command;
        public string L_Command;
        public int lineNumber;
        public int Symbol_Value = 16;
        public int iterations = 0;
        public string labelToSave;
        


        public string Filepath = "C:/Users/mathi/OneDrive/Skrivebord/";

        public Parser(string FileName)
        {
            Filename = FileName;
        }

        public void FileReader()
        {
        
            FileInfo file = new FileInfo(Filename);
            StreamReader sr = new StreamReader(Filepath + Filename);
            StreamWriter sw = new StreamWriter(Filepath + WriteFilename);

            line = sr.ReadLine();

            while (!sr.EndOfStream)
            {
                lineNumber++;
                line = sr.ReadLine();
                HandleLine(line, sw);
                
            }
            sw.Close();
            sr.Close();

            //iterations = 1;


        }

        private void HandleLabel()
        {

        }

        private void HandleLine(string line, StreamWriter sw)
        {
            // remove whitespace's and whitespace between characters

            line = line.Trim().Replace(" ", "");

            if (checkSkipLine(line))
            {
                return;
            }
            checkLabel(line);

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

        private void HandleAInstruction(string value, StreamWriter sw)
        {

            // get numbers/chars of A-instruction after @ symbol
            bool isNumber = int.TryParse(value, out int numericValue);

            if (!isNumber)
            {
                bool isSymbol = SymbolTable.symbols.TryGetValue(value, out numericValue);
                // if symbol dont exist add to table

                if (!isSymbol)
                {
                    numericValue = Symbol_Value;
                    SymbolTable.symbols.Add( value, numericValue);
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
                //Regex splitEquals = new Regex(@"^([^=]+)([^=].*)");
                Regex splitEquals = new Regex(@"^([^=-]+)");
                var match = splitEquals.Match(line);
                
                if (match.Success)
                {
                    destination1 = match.Groups[1].Value;
                    compchars = match.Groups[2].Value;
                    compchars = compchars.Replace("= ", "");
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
            }
            else
            {
                compchars = line.ToString();
            }

            compBits = SymbolTable.compTable[compchars];

        }

        private (string dest1, string dest2) SplitCInstruction(string line, string splitter)
        {
            Regex splitEquals = new Regex(@"^([^=]+)(" + splitter + ")(.*)$");
            var match = splitEquals.Match(line);
            if (!match.Success)
            {
                return (dest1: null, dest2: null);
            }
            return (dest1: match.Groups[1].Value, dest2: match.Groups[3].Value);
        }

        private string StripCommentsWS(string line)
        {
            Regex trailingComment = new Regex(@"^(//)(.*)$");
            var matchTrailComment = trailingComment.Match(line);
            if (matchTrailComment.Success)
            {
                line = matchTrailComment.Groups[2].Value;
            }

            Regex LeadingWSComment = new Regex(@"^([^/\s]+)(.*)$");
            var matchLeadWSComment = LeadingWSComment.Match(line);
            // remove leading WS/comment, if exist
            if (matchLeadWSComment.Success)
            {
                line = matchLeadWSComment.Groups[1].Value;
            }
            return line;

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
            Regex label = new Regex(@"^\(.+\)");

            if (label.IsMatch(line))
            {
                int index = line.LastIndexOf(")");
                if (index > 0)
                    line = line.Substring(0, index); // or index + 1 to keep slash

                labelToSave = line;
                labelToSave = Regex.Replace(line, @"[^a-zA-Z]", "");
                SymbolTable.labelTable.Add(labelToSave, lineNumber + 1);
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
