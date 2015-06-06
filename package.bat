
.nuget\nuget pack "ReallySimpleFeatureToggle\ReallySimpleFeatureToggle.csproj" -Properties Configuration=Release
if %errorlevel% neq 0 exit /b %errorlevel%

.nuget\nuget pack "ReallySimpleFeatureToggle.Web\ReallySimpleFeatureToggle.Web.csproj" -Properties Configuration=Release
if %errorlevel% neq 0 exit /b %errorlevel%

.nuget\nuget pack "ReallySimpleFeatureToggle.Web.Mvc\ReallySimpleFeatureToggle.Web.Mvc.csproj" -Properties Configuration=Release
if %errorlevel% neq 0 exit /b %errorlevel%


mkdir NuGetPackage
copy *.nupkg NuGetPackage\*.nupkg
del *.nupkg