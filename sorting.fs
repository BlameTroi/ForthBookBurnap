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

\ load test data, 10 records

s" one"                  s" 99" 0 >batch-enter-record
s" two"                  s" 76" 1 >batch-enter-record
s" three"                s" 88" 2 >batch-enter-record
s" four"                 s" 92" 3 >batch-enter-record
s" five"                 s" 83" 4 >batch-enter-record
s" six"                  s" 79" 5 >batch-enter-record
s" seven"                s" 60" 6 >batch-enter-record
s" eight"                s" 81" 7 >batch-enter-record
s" nine"                 s" 93" 8 >batch-enter-record
s" ten"                  s" 85" 9 >batch-enter-record

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
: old-swap-entries { addr1 addr2 length -- }
  addr1 pad length cmove
  addr2 addr1 length cmove
  pad addr2 length cmove
;

: swap-entries { n1 n2 -- }
  n1 >score-record pad record-length cmove
  n2 >score-record n1 >score-record record-length cmove
  pad n2 >score-record record-length cmove
;

: compare-names-s> ( n1 n2 -- f )
  >name-for-record swap >name-for-record swap name-length s>
;


\ buble sort single pass for names from book
: bubble ( -- f )
  true
  0 max-record-number ?do
    i 1 - i compare-names-s> if
      i dup 1 - swap-entries
      false and ( i think this is backwards )
    else
      true and
    endif
    -1
  +loop
;
