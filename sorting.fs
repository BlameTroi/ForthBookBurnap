\ sorting in various ways from library book
\ FORTH the 4th Generation Language, Steve Burnap
\ t.brumley 2015

\ depends upon prior work
include alloc-and-term-io.fs

[ifdef] sorting.fs
sorting.fs
[endif]

marker sorting.fs


\ vocabulary set in prior include
\ vocabulary troy
\ also troy
\ troy definitions

\ string comparison, strings have a maximum
\ length and are stored in a block of memory
\ padded out to the maximum with nulls.
\
\ i'm not hugely happy with this implementation
\ but stack management is taking some getting
\ used to. here i put a presumptive result on
\ the stck and then i drop and re-add it each
\ time through the loop. i'm not finding this
\ very clear, but practice will get me more
\ used to the stack -and- will help me learn
\ more efficient stack idioms.
: s> ( addr1 addr2 length -- f )
  false swap 0
  ?do
    drop
    2dup
    i + c@ swap i + c@
    < dup if
      leave
    endif
  loop
  -rot
  2drop
;

\ in the book, Burnap gives us a swap-name word
\ as an intro to show the need for swap-record. i
\ am just writing my own swap fields word.
\
\ no error checking for overlap or overflow of
\ the pad is done.
\
\ he also avoids passing length as a parameter
\ to avoid stack confusion. now is the time
\ to try variables
: swap-entries { addr1 addr2 length -- }
  addr1 pad length cmove
  addr2 addr1 length cmove
  pad addr2 length cmove
;


\ bubble sort single pass
\ oddly returning false if an exchange occurred
\ and true if not
: bubble ( -- f )
  true
  number-of-records 0 ?do
    i dup >name-for-record
    swap 1- >name-for-record
    s> if
      i dup >name-for-record
      swap 1- >name-for-record
      and false
    else
      and true
    endif
    -1
  +loop
;
