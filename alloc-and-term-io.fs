\ array and data structures from library book
\ FORTH the 4th Generation Language, Steve Burnap
\ t.brumley 2015

[ifdef] alloc-and-term-io.fs
  alloc-and-term-io.fs
[endif]

marker alloc-and-term-io.fs

\ *** get into a vocabulary (namespace) of my own
vocabulary troy
also troy
troy definitions


\ constants and variables

10 constant number-of-records
20 constant name-length
 4 constant score-length ( 32 bits stored in binary )

name-length score-length + constant record-length

create test-scores

test-scores record-length number-of-records *
dup allot erase

number-of-records 1 - constant max-record-number


\ find record/entry by number, checking for validity
\ dependent words count on >score-record to do range
\ checking and abort if appropriate. the book i'm
\ working through is old enough that it doesn't deal
\ with real exception handling.

: >score-record ( n -- addr )
  dup dup 0 < swap max-record-number > or
  if
	  ." invalid record number: " . quit
  else
	  record-length * test-scores +
  endif ;

: >name-for-record ( n -- addr )
  >score-record
;

: >score-for-record ( n -- addr )
  >score-record name-length +
;


\ scrub an entry, or subfields within the entry

: >clear-name ( n -- )
  >name-for-record name-length erase
;

: >clear-score ( n -- )
  >score-for-record score-length erase
;

: >clear-record ( n -- )
  dup >clear-name >clear-score
;


\ enter fields or the whole record

: >enter-name ( n -- )
  dup >clear-name
  >name-for-record name-length expect
;

: >enter-score ( n -- )
  \ this is rather simple minded, but it is good
  \ enough for this exercise
  dup >clear-score
  pad 10 accept
  pad swap s>number?
  if
    drop swap >score-for-record l!
  else
    ." error on input conversion" quit
  endif
;


: >enter-record ( n -- )
  dup >enter-name >enter-score
;


: >batch-enter-score ( addr count n -- )
  dup >clear-score >r s>number?
  if
    drop r> >score-for-record l!
  else
    ." error on input conversion" quit
  endif
;

: >batch-enter-name ( addr count n -- )
  dup >clear-name >name-for-record swap cmove
;


\ 1 = name, 2 = score, n = entry number
: >batch-enter-record ( addr1 count1 addr2 count2 n -- )
  dup >r >batch-enter-score r> >batch-enter-name
;

\ display fields or the whole record

: >print-name ( n -- )
  >name-for-record name-length type
;

: >print-score ( n -- )
  >score-for-record l@ .
;

: >print-record ( n -- )
  dup >print-name space >print-score
;


\ more of the higher level application

: enter-records
  \ loop to load the score-record
  number-of-records 0 ?do
    i dup cr . ." :"
    >enter-record
  loop
;


: print-records
  number-of-records 0 ?do
    i dup cr . ." :" >print-record
  loop
;

: average-records ( -- n )
  0
  number-of-records 0 ?do
    i >score-for-record l@ +
  loop
  number-of-records /
;
