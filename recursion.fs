\ recursion in forth, some simple counting tests and
\ then the fibonacci sequence.
\
\ t.brumley 2015

[ifdef] recursion.fs
  recursion.fs
[endif]

marker recursion.fs
vocabulary troy
also troy
troy definitions

\ -------------------------------------------------------
\ i'm seeing that these can handle a bit less than 1,900
\ calls in my system. i added a variable allocation to my
\ system and this produced no change in behavior. i think
\ that makes sense, the return stack would be the limiting
\ factor.

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

\ ---------------------------------------------------------------------
\ fibonacci series
\
\ fibonacci, 0, 1, 1, 2, 3, 5, 8
\ wikipedia says it is acceptable in modern times to
\ start the sequence from 0, 1 instead of 1, 1
\
\ just creating a listing of the sequence can be done with a loop, but
\ to return the n-th number of the series we use recursion. the first
\ simple implementation starts to be noticably slow around the 35th
\ number in the sequence. a version that caches calculated values is
\ much faster.
\
\ in the single precision (8 byte cells on my macbook pro) implementation,
\ treating the numbers as signed breaks at the 93rd number in the sequence.
\ treating the numbers as unsigned only gets a couple of more numbers before
\ the cell overflows.
\
\ after some more testing, a double precision version will be created.
\

\ *** single precision ***

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
fib-cache 8 200 * dup allot 255 fill

: can-cache-fib ( n -- f )
  dup -1 > swap 200 < and
;

: >fib-cache-entry ( n -- address-of-entry )
  \ counts on outer code to protect cache and
  \ not use this address if it is bad
  8 * fib-cache +
;

\ test if this fib has been cached
\ return false if not, non-zero
\ value if it has
: @fib-in-cache ( n -- f or cached-fib-entry )
  \ drop false exit ( do nothing as yet )
  dup can-cache-fib if >fib-cache-entry @ else drop -1 then
;

\ store value in cache if it will fit
: !fib-in-cache ( fibn n -- )
  dup can-cache-fib if >fib-cache-entry ! else 2drop then
;

\ preload initial cache values for 0, 1, and 2 to
\ simplify coding

0 0 !fib-in-cache
1 1 !fib-in-cache
1 2 !fib-in-cache

\ i could add more but i'm only adding the head of
\ the series to seed the algorithm

\ calculate
: cached-fib ( n -- fib )
  \ i have confirmed that this works correctly
  \ if all the cache support is stubbed out, so
  \ the failure is in dealing with the data
  \ returned from the cache

  dup @fib-in-cache dup -1 <> if
    \ value returned from cache
    swap drop
  else
    \ value not returned
    \ simplistically just calculate and store
    \ the answer, counting on the cache handlers
    \ to protect the cache and leave the stack
    \ in an understandable state even when the
    \ value can't be cached
    \ break" blarg"
    drop dup 1- dup 1- recurse swap recurse +
    dup rot !fib-in-cache
  then
;


\ *** double precision ***

\ simple minded recursion to get the nth fibonacci number
\ this burns up my old macbook pro for values of n over 35
\ but since gforth is 64 bit it can calculate the answers
: simple-fib-double ( d -- dfib )
  2dup 1. d>
  if
    1. d- 2dup 1. d- recurse 2swap recurse d+
  then
;


\ more complex fib that caches several values as they
\ are calculated, arbitrarily using 100 cells and
\ sloppily using magic numbers since this is just
\ hacking to get used to using forth

create fib-cache-double
fib-cache-double 16 200 * dup allot 255 fill

: can-cache-fib-double ( n -- f )
  dup -1 > swap 200 < and
;

: >fib-cache-entry-double ( n -- address-of-entry )
  \ counts on outer code to protect cache and
  \ not use this address if it is bad
  16 * fib-cache-double +
;

\ test if this fib has been cached
\ return false if not, non-zero
\ value if it has
: @fib-in-cache-double ( n -- f or cached-fib-entry )
  \ drop false exit ( do nothing as yet )
  dup can-cache-fib-double if >fib-cache-entry-double 2@ else drop -1. then
;

\ store value in cache if it will fit
: !fib-in-cache-double ( fibn n -- )
  dup can-cache-fib-double if >fib-cache-entry-double 2! else drop 2drop then
;

\ preload initial cache values for 0, 1, and 2 to
\ simplify coding

0. 0 !fib-in-cache-double
1. 1 !fib-in-cache-double
1. 2 !fib-in-cache-double

\ i could add more but i'm only adding the head of
\ the series to seed the algorithm

\ calculate
: cached-fib-double ( n -- fib )
  \ i have confirmed that this works correctly
  \ if all the cache support is stubbed out, so
  \ the failure is in dealing with the data
  \ returned from the cache

  dup @fib-in-cache-double 2dup -1. d<> if
    \ value returned from cache
    rot drop
  else
    \ value not returned
    \ simplistically just calculate and store
    \ the answer, counting on the cache handlers
    \ to protect the cache and leave the stack
    \ in an understandable state even when the
    \ value can't be cached
    2drop dup 0 swap 1- dup 1- recurse rot recurse d+
    2dup 2rot drop !fib-in-cache-double
  then
;
