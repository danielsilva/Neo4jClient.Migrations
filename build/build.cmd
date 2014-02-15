@echo off
setlocal

goto Start

:Start
set TARGET=%1
if "%TARGET%"=="" goto SetDefaultTarget
goto Build

:SetDefaultTarget
echo No build target provided. REBUILD will be used
set TARGET=Rebuild
goto Build

:Build
echo Build started
echo.
%WinDir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe build.proj /target:%TARGET% %2 %3 %4 %5 %6 %7 %8 %9

if %ERRORLEVEL%==0 goto Success
if %ERRORLEVEL%==1 goto Failure

:Success
echo.
echo Build succeeded :)
goto End

:Failure
echo.
echo Build failed :(
goto End

:End

endlocal