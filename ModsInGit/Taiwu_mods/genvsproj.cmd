set STEAMDIR = "D:\Steam\steamapps\common\The Scroll Of Taiwu"
echo STEAMDIR is %STEAMDIR%
@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

ECHO Searching for taiwu install dir...
IF "%PROCESSOR_ARCHITECTURE%"=="AMD64" (
    FOR /F "tokens=2* skip=2" %%a in ('REG QUERY HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam /v InstallPath') do SET "SteamInstallPath=%%b"
) ELSE (
    FOR /F "tokens=2* skip=2" %%a in ('REG QUERY HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam /v InstallPath') do SET "SteamInstallPath=%%b"
)



 SET TAIWUDIR="D:\Steam\steamapps\common\The Scroll Of Taiwu"
                IF EXIST !TAIWUDIR! (
                    ECHO Found taiwu folder: !TAIWUDIR!
                    SET StopLoop=true
                    SET TaiwuFound=true
                    goto :build
                )

IF NOT !TaiwuFound!==true ECHO Did not find taiwu install folder in any steam library
goto :eof

:Build
if not exist "%~dp0\build" (mkdir "%~dp0\build")
set "PWD=%CD%"
cd "%~dp0\build"

if defined VisualStudioVersion goto :RunVCVars

set _VSWHERE="%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if exist %_VSWHERE% (
    for /f "usebackq tokens=*" %%i in (`%_VSWHERE% -latest -property installationPath`) do set _VSCOMNTOOLS=%%i\Common7\Tools
)
if not exist "%_VSCOMNTOOLS%" set _VSCOMNTOOLS=%VS140COMNTOOLS%
if not exist "%_VSCOMNTOOLS%" goto :MissingVersion

call "%_VSCOMNTOOLS%\VsDevCmd.bat" > nul

:RunVCVars
if "%VisualStudioVersion%"=="16.0" (
    echo Visual Studio 2019
    cmake .. -G "Visual Studio 16 2019" -DSTEAMDIR=!TAIWUDIR!
) else if "%VisualStudioVersion%"=="15.0" (
    echo Visual Studio 2017
    cmake .. -G "Visual Studio 15 2017" -DSTEAMDIR=!TAIWUDIR!
) else (
:MissingVersion
    echo "cannot find Visual Studio 2017 or later version"
)

cd "%PWD%"
