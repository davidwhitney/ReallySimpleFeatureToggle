MSBuild.exe /m:8 /p:Configuration=Release "ReallySimpleFeatureToggle.sln"
if %errorlevel% neq 0 exit /b %errorlevel%

MSBuild.exe /m:8 /p:Configuration=Release40 "ReallySimpleFeatureToggle.sln"
if %errorlevel% neq 0 exit /b %errorlevel%
