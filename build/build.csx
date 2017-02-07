
#load "common.csx"

Execute(() => DotNet.Build("../src/LightMock.sln"), "Building");
Execute(() => DotNet.Test("../src/LightMock.Tests/LightMock.Tests.csproj"), "Testing");
Execute(() => DotNet.Pack("../src/LightMock/LightMock.csproj"), "Packing");