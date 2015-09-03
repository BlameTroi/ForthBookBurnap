\ set up for my environment
\ this should be in siteforth\ but i'm not
\ using that yet.


\ gdb broken with 0.7.x releases of gforth
\ use this until i figure out how to use
\ the built in disassembler
' dump is discode


\ my utilities
: cls 27 emit ." [2J" 27 emit ." [;H" ;


\ done
