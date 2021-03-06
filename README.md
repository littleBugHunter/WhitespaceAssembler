# WhitespaceAssembler
A human readable assembler format and compiler for the whitespace programming language

This Assembler was written at 1:00 in the morning as a proof of concept, because I was bored. It probably features bugs. Feel free to report them as issues.

There are probably a bunch of better assemblers/compilers outputting whitespace out there, I'd highly reccomend looking around for them.

Usage
-----
Compile the C# Project

Run WhitespaceAssembler.exe with the input and output file destinations as Parameters

Variables and Values
--------------------
Numbers can be input as simple decimal integers, #hexadecimal values (prefixed with #) or as 'A' Ascii characters (' ' is currently not supported, use Ascii code 32 instead.

Variables have two prefixes depending on if you are talking about their Value or their Address. They do not need to be declared. \*variableName retrieves the value at the given variable (Like a Pointer dereference) &variableName gets the address of the Variable

Labels are prefixed with a . and can be defined with the lbl command

Instruction Set
---------------

The instructions are followed by any of the following symbols, describing the parameter type. (parameters in code are seperated by spaces or tabs)

\# Means a Number Value or an Address (&variableName)

\* Means a Variable Value (\*variableName)

. Means a Label

>Stack Maniupulation
>===================
>Push #
>------
>Pushes the Value or Address to the Stack
>
>Push *
>------
>Pushes the Variable Value to the Stack
>
>Dup
>----
>Duplicates the top Value of the Stack
>
>Pop
>----
>Discards the top Value of the Stack
>
>Aritmethic
>==========
>
>Add
>---
>Adds the two Values on top of the Stack and stores the result on the Stack
>
>Add #
>-----
>Adds the Values on top of the Stack to the Value or Address in Param0 and stores the result on the Stack
>
>Add *
>-----
>Adds the Values on top of the Stack to the Variable in Param0 and stores the result on the Stack
>
>Add *#
>-----
>Adds the Variable in Param0 to the Value or Address in Param1 and stores the result on the Stack
>
>Add #*
>-----
>Adds the Value or Address in Param1 to the Variable in Param0 and stores the result on the Stack
>
>Add **
>-----
>Adds the Variable in Param1 to the Variable in Param0 and stores the result on the Stack
>
>Sub
>---
>Subtracts the two Values on top of the Stack and stores the result on the Stack
>
>Sub #
>-----
>Subtracts the Values on top of the Stack from the Value or Address in Param0 and stores the result on the Stack
>
>Sub *
>-----
>Subtracts the Values on top of the Stack from the Variable in Param0 and stores the result on the Stack
>
>Sub *#
>-----
>Subtracts the Variable in Param0 from the Value or Address in Param1 and stores the result on the Stack
>
>Sub #*
>-----
>Subtracts the Value or Address in Param1 from the Variable in Param0 and stores the result on the Stack
>
>Sub **
>-----
>Subtracts the Variable in Param1 from the Variable in Param0 and stores the result on the Stack
>
>Mul
>---
>Multiplies the two Values on top of the Stack and stores the result on the Stack
>
>Mul #
>-----
>Multiplies the Values on top of the Stack with the Value or Address in Param0 and stores the result on the Stack
>
>Mul *
>-----
>Multiplies the Values on top of the Stack with the Variable in Param0 and stores the result on the Stack
>
>Mul *#
>-----
>Multiplies the Variable in Param0 with the Value or Address in Param1 and stores the result on the Stack
>
>Mul #*
>-----
>Multiplies the Value or Address in Param1 with the Variable in Param0 and stores the result on the Stack
>
>Mul **
>-----
>Multiplies the Variable in Param1 with the Variable in Param0 and stores the result on the Stack
>
>Div
>---
>Divides the two Values on top of the Stack and stores the result on the Stack
>
>Div #
>-----
>Divides the Values on top of the Stack with the Value or Address in Param0 and stores the result on the Stack
>
>Div *
>-----
>Divides the Values on top of the Stack with the Variable in Param0 and stores the result on the Stack
>
>Div *#
>-----
>Divides the Variable in Param0 with the Value or Address in Param1 and stores the result on the Stack
>
>Div #*
>-----
>Divides the Value or Address in Param1 with the Variable in Param0 and stores the result on the Stack
>
>Div **
>-----
>Divides the Variable in Param1 with the Variable in Param0 and stores the result on the Stack
>
>Mod
>---
>Gets the Modulo of the two Values on top of the Stack and stores the result on the Stack
>
>Mod #
>-----
>Gets the Modulo of the Values on top of the Stack and the Value or Address in Param0 and stores the result on the Stack
>
>Mod *
>-----
>Gets the Modulo of the Values on top of the Stack and the Variable in Param0 and stores the result on the Stack
>
>Mod *#
>-----
>Gets the Modulo of the Variable in Param0 and the Value or Address in Param1 and stores the result on the Stack
>
>Mod #*
>-----
>Gets the Modulo of the Value or Address in Param1 and the Variable in Param0 and stores the result on the Stack
>
>Mod **
>-----
>Gets the Modulo of the Variable in Param1 and the Variable in Param0 and stores the result on the Stack
>
>Heap Control
>============
>
>Store
>-----
>Stores the Value at the second topmost Stack at the Address on top of the Stack
>
>Store #
>-----
>Stores the Value on top of the Stack at the Address in Param0 (This is kind of the reverse behaviour of the Store instruction but Store at Address is pretty handy)
>
>Store ##
>-----
>Stores the Value Or Address in Param0 at the Address in Param1
>
>Retrieve
>-------
>Replaces the Address on top of the Stack with the value at said Address
>
>Retrieve #
>Pushes the Value at the Address in Param0 on top of the Stack
>
>Flow Control
>============
>
>Lbl .
>-----
>Defines a Label as param0
>
>Call .
>------
>Calls the Label specified in Param0
>
>Jmp .
>-----
>Jumps to the Label specified in Param0
>
>Jpz .
>-----
>Jumps to the Label specified in Param0 if the value on top of the Stack is zero (This pops the top of the stack so call Dup before, if you want to keep the Value)
>
>Jpn .
>-----
>Jumps to the Label specified in Param0 if the value on top of the Stack is negative (This pops the top of the stack so call Dup before, if you want to keep the Value)
>
>Ret
>----
>Returns from a Call
>
>Exit
>----
>Stops the Program
>
>I/O
>====
>
>Print_char
>----------
>Prints the character on top of the Stack to the Console
>
>Print_number
>-----------
>Prints the number on top of the Stack to the Console
>
>Read_char
>---------
>Reads a character from the Console and puts it on top of the Stack
>
>Read_number
>-----------
>Reads a number from the Console and puts it on top of the Stack



