\ this is a test
\ misc junk code
\ t.brumley 2015

[ifdef] recursion.fs
  recursion.fs
[endif]

marker recursion.fs
vocabulary troy
also troy
troy definitions

\ test recursion:
\ i'm seeing that these can handle a bit less than 1,900
\ calls in my system ... adding a variable allocation
\ try to eat some space and see what that changes

: countDown ( n -- )
  dup 1 = if
    .
  else
    dup .
    1- recurse
  then
;

: countUp ( n -- )
  dup 1 = if
    .
  else
    dup 1- recurse .
  then
;


\ eat some space before testing
\
\ create space-eater
\space-eater 9999 dup allot erase
\
\ and the return stack is elsewhere so this didn't change
\ the behavior


\ some of the classics:

\ fibonacci, 0, 1, 1, 2, 3, 5, 8
\ wikipedia says it is acceptable in modern times to
\ start the sequence from 0, 1 instead of 1, 1

\ simple minded recursion to get the nth fibonacci number
\ this burns up my old macbook pro for values of n over 35
\ but since gforth is 64 bit it can calculate the answers
: simple-fib ( n -- fib )
  dup 1 >
  if
    1- dup 1- recurse swap recurse +
  then
;

\ more complex fib that caches several values as they
\ are calculated, arbitrarily using 100 cells and
\ sloppily using magic numbers since this is just
\ hacking to get used to using forth

create fib-cache
fib-cache 8 100 * dup allot erase

: cached-fib ( n -- fib )
  ." work in progress "
;
