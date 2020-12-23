#load "nuget:Dotnet.Build, 0.12.1"
#load "nuget:dotnet-steps, 0.0.2"

BuildContext.CodeCoverageThreshold = 70;

[StepDescription("Runs the tests with test coverage")]
Step codecoverage = () => DotNet.TestWithCodeCoverage();

[StepDescription("Runs all the tests for all target frameworks")]
Step test = () => DotNet.Test();

[StepDescription("Creates all artifacts")]
Step pack = () =>
{
    test();
    codecoverage();
    DotNet.Pack();
    DotNet.Publish();
};

[DefaultStep]
[StepDescription("Ships all artifacts")]
AsyncStep ship = async () =>
{
    pack();
    await Artifacts.Deploy();
};

await StepRunner.Execute(Args);
return 0;
