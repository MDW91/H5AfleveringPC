// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Fill.asm

// Runs an infinite loop that listens to the keyboard input.
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel;
// the screen should remain fully black as long as the key is pressed. 
// When no key is pressed, the program clears the screen, i.e. writes
// "white" in every pixel;
// the screen should remain fully clear as long as no key is pressed.

// Put your code here.

@8192   // (512 * 32) / 16 (alle pixels af skærmen) gemmer i D Registeret
D=A

@pixelcounter
M=D     // gemmer total pixels af skærmen (8192) i M Register

(LOOP)
@i
M=0     // sætter index til 0



(KBDinput)
@KBD
D=M

@WHITE
D;JEQ   // hop til WHITE hvis KBDinput (tastatur) = 0 (intet input)


(BLACK)
@i
D=M

@SCREEN
A=A+D   // Beregn skærm adresse
M=-1    // gør skærmen sort (-1)

@END
0;JMP   // goto END



(WHITE)
@i
D=M

@SCREEN
A=A+D   // Beregn skærm adresse
M=0     // gør skærmen hvid (0)



(END)
@i
MD=M+1  // i++ (M+D Register +1)

@pixelcounter
D=D-M

@LOOP
D;JEQ   // hop til LOOP hvis pixelcounter - index = 0

@KBDinput
0;JMP   // hop til KBDinput label

