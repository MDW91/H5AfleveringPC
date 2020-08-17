// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Mult.asm

// Multiplies R0 and R1 and stores the result in R2.
// (R0, R1, R2 refer to RAM[0], RAM[1], and RAM[2], respectively.)

// Put your code here.

// Sætter R2 = 0
@R2
M = 0

// i = 0
@i
M = 0

(LOOP)		//startlabel af LOOP løkke
  @i 		// index = 0
  D = M 	//gem i M-register
  @R1
  D = D - M	// D Reg = D Reg - index
  @END
  D;JEQ		// hvis D == 0 hop til (END) label

  @R0
  D = M		//gem resultatet i R0's M register
  @R2		
  M = M + D	// 

  @i		// i++
  M = M + 1

  @LOOP
  0;JMP		// Hop ubetinget til (LOOP) label

(END)
  @END		//uendelig løkke (script færdigt)
  0;JMP