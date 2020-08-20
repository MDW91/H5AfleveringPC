﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    public class SymbolTable
    {

        public static Dictionary<string, int> symbols = new Dictionary<string, int>
        {
            { "R0", 0},
            { "R1", 1},
            { "R2", 2},
            { "R3", 3},
            { "R4", 4},
            { "R5", 5},
            { "R6", 6},
            { "R7", 7},
            { "R8", 8},
            { "R9", 9},
            { "R10", 10},
            { "R11", 11},
            { "R12", 12},
            { "R13", 13},
            { "R14", 14},
            { "R15", 15},
            { "SCREEN", 16384},
            { "KBD", 24576},
            { "SP", 0},
            { "LCL", 1},
            { "ARG", 2},
            { "THIS", 3},
            { "THAT", 4}
        };


        public static Dictionary<string, string> JmpTable = new Dictionary<string, string>
        {
            {"JMP", "111"},
            {"JLE", "110"},
            {"JNE", "101"},
            {"JLT", "100"},
            {"JGE", "011"},
            {"JEQ", "010"},
            {"JGT", "001"},

        };

    // binary codes for each different computation
    public static Dictionary<string, string> compTable = new Dictionary<string, string>
        {
             {"0", "0101010"},
             {"1", "0111111"},
             {"-1", "0111010"},
             {"A", "0110000"},
             {"D", "0001100"},
             {"M", "1110000"},
             {"!D", "0001101"},
             {"!A", "0110001"},
             {"!M", "1110001"},
             {"-D", "0001111"},
             {"-A", "0110011"},
             {"-M", "1110011" },
             {"D+1", "0011111"},
             {"1+D", "0011111"},
             {"A+1", "0110111"},
             {"1+A", "0110111"},
             {"M+1", "1110111"},
             {"1+M", "1110111"},
             {"D-1", "0001110"},
             {"A-1", "0110010"},
             {"M-1", "1110010"},
             {"D+A", "0000010"},
             {"A+D", "0000010"},
             {"D+M", "1000010"},
             {"M+D", "1000010"},
             {"D-A", "0010011"},
             {"A-D", "0000111"},
             {"D-M", "1010011"},
             {"M-D", "1000111"},
             {"D&A", "0000000"},
             {"A&D", "0000000"},
             {"D&M", "1000000"},
             {"M&D", "1000000"},
             {"D|A", "0010101"},
             {"A|D", "0010101"},
             {"D|M", "1010101"},
             {"M|D", "1010101"},
        };

        public static Dictionary<string, int> labelTable = new Dictionary<string, int>
        {
            
        };

    }
}
