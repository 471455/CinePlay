@echo off
if "%~1"=="" (
    echo Usage: %0 input_file
    exit /b 1
)

set "input_file=%~1"

if not exist "%input_file%" (
    echo File "%input_file%" does not exist.
    exit /b 1
)

:: Extracts name and extension
set "input_dir=%~dp1"
set "input_name=%~n1"

:: Temp file path
set "output_file=%input_dir%%input_name%_temp.mp4"

:: Convert audio codec to aac
ffmpeg -i "%input_file%" -c:v copy -c:a aac -b:a 192k "%output_file%"
if errorlevel 1 (
    echo Error converting the file.
    exit /b 1
)
echo File converted successfully: %output_file%

:: Deletes original file
del /F "%input_file%" >nul 2>&1
if errorlevel 1 (
    echo Error deleting the original file. It might be in use or you might not have permission.
    exit /b 1
)

:: Replace with updated version
move /Y "%output_file%" "%input_dir%%input_name%.mp4"
if errorlevel 1 (
    echo Error renaming the output file. The file might be in use or you might not have permission.
    exit /b 1
)

echo File replaced successfully: %input_file%   
