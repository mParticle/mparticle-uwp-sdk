MSBuild ..\Src\mParticle.Sdk.sln /t:restore /p:Configuration=Release
MSBuild ..\Src\mParticle.Sdk.sln /t:Rebuild /p:Platform="Any CPU" /p:Configuration=Release
MSBuild ..\Src\mParticle.Sdk.Core/mParticle.Sdk.Core.csproj /t:pack /p:Configuration=Release /p:IncludeSource=true /p:Authors="mParticle Inc"
MSBuild ..\Src\mParticle.Sdk.UWP/mParticle.Sdk.UWP.csproj /t:pack /p:Configuration=Release /p:IncludeSource=true /p:Authors="mParticle Inc" /p:Description="mParticle SDK for UWP apps"

