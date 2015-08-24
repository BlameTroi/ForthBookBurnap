\ array and data structures from library book
\ FORTH the 4th Generation Language, Steve Burnap
\ t.brumley 2015

[ifdef] test1.fs
    test1.fs
[endif]

marker test1.fs

\ *** get into a vocabulary (namespace) of my own
vocabulary troy
also troy
troy definitions


\ constants and variables

10 constant num-records
20 constant name-length
2 constant score-length

name-length score-length + constant record-length

create test-scores

test-scores record-length num-records *
dup allot erase

num-records 1 - constant max-record-number


\ find record/entry by number, checks for validity

: >score-record ( n -- addr )
  dup dup 0 < swap max-record-number > or if
	  ." invalid record number: " . quit
  else
	  record-length * test-scores +
  then ;

\ find name for record by number, rely on >score-record for
\ error checking

: >name-for-record ( n -- addr )
  >score-record
;

\ find score for record by number, rely on >score-record for
\ error checking

: >score-for-record ( n -- addr )
  >score-record
  name-length +
;

\ display name for record ...
: >print-name ( n -- )
    >name-for-record name-length type
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

\ enter name for record ...
: >enter-name ( n -- )
    dup >clear-name
    >name-for-record name-length expect
;

\ display score for record ...
: >print-score ( n -- )
    >score-for-record score-length type
;

\ enter score for record ..
: >enter-score ( n -- )
  dup
  >clear-score
  >score-for-record score-length expect
;


\ display whole record
: >print-record ( n -- )
    dup
    >print-name
    space
    >print-score
;

: >enter-record ( n -- )
  dup
  >
;
