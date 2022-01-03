@echo off
echo wait for saving ...
:loop
    inotifywait *.cs -qr -e modify .\
    msbuild -verbosity:quiet
    rem echo %ERRORLEVEL%
    if %ERRORLEVEL% EQU 0 (
        echo ---------------------------------------------
        echo run
        echo ---------------------------------------------

        bin\Debug\sender.exe

        echo wait for saving ...
    )

    goto loop

