del ReallySimpleFeatureToggle.*.nupkg

call create-package.bat
if %errorlevel% neq 0 exit /b %errorlevel%

.nuget\nuget push ReallySimpleFeatureToggle.*.nupkg
if %errorlevel% neq 0 exit /b %errorlevel%