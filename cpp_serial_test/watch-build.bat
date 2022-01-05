@echo off
echo wait for saving ...
:loop
    inotifywait *.cs -qr -e modify .\
    msbuild -verbosity:quiet cpp_serial_test.vcxproj /p:configuration=debug /p:platform=x86
    echo ---------------------------------------------
    echo done.
    echo ---------------------------------------------
    goto loop

