@echo off
echo ================================================
echo  Building LiF Damage Meter Release v1.0
echo ================================================

REM Set MSBuild path (Visual Studio 2019/2022)
set MSBUILD_PATH=""
if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" (
    set MSBUILD_PATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
) else if exist "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" (
    set MSBUILD_PATH="C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
) else if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" (
    set MSBUILD_PATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
) else if exist "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" (
    set MSBUILD_PATH="C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
) else (
    echo ERROR: MSBuild not found! Please install Visual Studio 2019/2022
    pause
    exit /b 1
)

echo Using MSBuild: %MSBUILD_PATH%

REM Clean previous build
echo Cleaning previous build...
if exist "LiFDamageMeter\bin\Release" rmdir /s /q "LiFDamageMeter\bin\Release"
if exist "LiFDamageMeter\obj\Release" rmdir /s /q "LiFDamageMeter\obj\Release"

REM Restore NuGet packages
echo Restoring NuGet packages...
nuget restore LiFDamageMeter.sln

REM Build Release configuration
echo Building Release configuration...
%MSBUILD_PATH% LiFDamageMeter.sln /p:Configuration=Release /p:Platform="Any CPU" /p:OutputPath=bin\Release /verbosity:minimal

if %ERRORLEVEL% NEQ 0 (
    echo BUILD FAILED!
    pause
    exit /b %ERRORLEVEL%
)

REM Create release directory
echo Creating release package...
if not exist "Release" mkdir "Release"

REM Copy main executable
copy "LiFDamageMeter\bin\Release\LiFDamageMeter.exe" "Release\"

REM Copy dependencies
copy "LiFDamageMeter\bin\Release\MaterialSkin.dll" "Release\"
copy "LiFDamageMeter\bin\Release\*.config" "Release\"

REM Copy documentation
copy "README.md" "Release\"
copy "LICENSE" "Release\" 2>nul

REM Create version file
echo LiF Damage Meter v1.0 > "Release\VERSION.txt"
echo Built on %date% %time% >> "Release\VERSION.txt"
echo. >> "Release\VERSION.txt"
echo Features: >> "Release\VERSION.txt"
echo - Complete damage type support >> "Release\VERSION.txt"
echo - Full date timestamps >> "Release\VERSION.txt"
echo - Auto-resizing columns >> "Release\VERSION.txt"
echo - Material Design UI >> "Release\VERSION.txt"
echo - Enhanced log parsing >> "Release\VERSION.txt"

echo ================================================
echo  BUILD COMPLETED SUCCESSFULLY!
echo ================================================
echo.
echo Release files created in: Release\
echo Main executable: LiFDamageMeter.exe
echo.
echo Ready for distribution!
echo ================================================

pause