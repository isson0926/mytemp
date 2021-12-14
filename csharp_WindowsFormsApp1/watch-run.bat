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

        bin\Debug\csharp_WindowsFormsApp1.exe

        echo wait for saving ...
    )

    goto loop

