@echo off
echo wait for saving ...
:loop
    inotifywait *.cs -qr -e modify .\
    msbuild -verbosity:quiet cpp_serial_test.vcxproj /p:configuration=debug /p:platform=x86
    rem echo %ERRORLEVEL%
    if %ERRORLEVEL% EQU 0 (
        echo ---------------------------------------------
        echo run
        echo ---------------------------------------------

        Debug\cpp_serial_test.exe COM3

        echo wait for saving ...
    )

    goto loop

