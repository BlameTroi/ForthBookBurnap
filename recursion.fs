\ this is a test
\ misc junk code
\ t.brumley 2015

[ifdef] test2.fs
test2.fs
[endif]

marker test2.fs
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
    dup 1 =
    if
	.
    else
	dup 1- recurse .
    then
;

\ eat some space before testing

create space-eater

space-eater 9999 dup allot erase

\ and the return stack is elsewhere so this didn't change
\ the behavior
