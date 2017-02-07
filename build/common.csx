using System.Diagnostics;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Threading;

private static int depth = 0;

private static string lastWriteOperation;






public static class DotNet
{
    public static void Build(string pathToSolutionFile)
    {
        Command.Execute("dotnet.exe","--version", ".");
        Command.Execute("dotnet.exe","restore " + pathToSolutionFile, ".");        
        Command.Execute("dotnet.exe","build " + pathToSolutionFile +  " --configuration Release" , ".");   
    }

    public static void Pack(string pathToProjectFile)
    {
        Command.Execute("dotnet.exe","pack " + pathToProjectFile +  " --configuration Release" , ".");   
    }

    public static void Test(string pathToProjectFile)
    {
        Command.Execute("dotnet.exe","test " + pathToProjectFile +  " --configuration Release" , ".");   
    }

}

private static void WriteStart(string message, params object[] arguments)
{
    StringBuilder sb = new StringBuilder();    
    sb.Append("\n");
    sb.Append(' ', depth);
    sb.Append(string.Format(message, arguments));            
    Console.Out.Flush();
    Console.Write(sb.ToString());
    Console.Out.Flush();
    lastWriteOperation = "WriteStart";
}

private static void WriteLine(string message,  params object[] arguments)
{
    if (message == null)
    {
        return;
    }
    StringBuilder sb = new StringBuilder();    
    if (lastWriteOperation == "WriteStart")
    {
        sb.Append("\n");
    }
    sb.Append(' ', depth);
    sb.Append(string.Format(message, arguments));            
    Console.WriteLine(sb.ToString());
    lastWriteOperation = "WriteLine";
}

private static void WriteEnd(string message, params object[] arguments)
{
    if (lastWriteOperation == "WriteStart")
    {
        Console.WriteLine(string.Format(message,arguments));
    }
    else
    {
        StringBuilder sb = new StringBuilder();    
        sb.Append(' ', depth);
        sb.Append(string.Format(message, arguments));            
        Console.WriteLine(sb.ToString());
        lastWriteOperation = "WriteLine";
    }
}

public static string ResolveDirectory(string path, string filePattern)
{
    string pathToFile = Directory.GetFiles(path, "xunit.runner.visualstudio.testadapter.dll", SearchOption.AllDirectories).Single();
    return Path.GetDirectoryName(pathToFile);
}

public static string GetFile(string path, string filePattern)
{
    WriteLine("Looking for {0} in {1}", filePattern, path);
    string[] pathsToFile = Directory.GetFiles(path, filePattern, SearchOption.AllDirectories).ToArray();
    if (pathsToFile.Length > 1)
    {
        WriteLine("Found multiple files");
        var files = pathsToFile.Select(p => new FileInfo(p));
        var file = files.OrderBy(f => f.LastWriteTime).Last();
        WriteLine("Choosing {0}",file.FullName);
        return file.FullName;
    }
    WriteLine("Found {0}", pathsToFile[0]);    
    return pathsToFile[0];    
}



public static string GetTemporaryDirectory()
{
   string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
   Directory.CreateDirectory(tempDirectory);
   return tempDirectory;
}

public static string RemoveInternalsVisibleToAttribute(string assemblyInfo)
{    
    return Regex.Replace(assemblyInfo, @"(\[assembly: InternalsVisibleTo.+\])", string.Empty); 
}

public static void Execute(Action action, string description)
{
    int currentIndentation = depth;    
    WriteStart(description);
    depth++;              
    Stopwatch watch = new Stopwatch();
    watch.Start();
    action();
    watch.Stop();    
    depth--; 
    WriteEnd("...Done! ({0} ms)", watch.ElapsedMilliseconds);                      
}














public static class MsBuild
{
    public static void Build(string pathToSolutionFile)
    {
        string pathToMsBuild = ResolvePathToMsBuild();        
        Command.Execute(pathToMsBuild, pathToSolutionFile + " /property:Configuration=Release /p:VisualStudioVersion=12.0 /verbosity:minimal",".");     
    }
    
    private static string ResolvePathToMsBuild()
    {                        
        return @"C:/Program Files (x86)/Microsoft Visual Studio/2017/Enterprise/MSBuild/15.0/Bin/MSBuild.exe";
    }
}

public static class PathResolver
{
    public static string GetPathToMsBuildTools()
    {
        string keyName = @"SOFTWARE\Wow6432Node\Microsoft\MSBuild\ToolsVersions";
        string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator; 
        
        RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName);
        string[] subKeynames = key.GetSubKeyNames().Select(n => n.Replace(".", decimalSeparator)).ToArray();        
        Collection<decimal> versionNumbers = new Collection<decimal>();
        
        for (int i = 0; i < subKeynames.Length; i++)
        {
            decimal versionNumber;
            if (decimal.TryParse(subKeynames[i], out versionNumber))
            {
                versionNumbers.Add(versionNumber);
            }                        
        }
        
        decimal latestVersionNumber = versionNumbers.OrderByDescending(n => n).First();
        RegistryKey latestVersionSubKey = key.OpenSubKey(latestVersionNumber.ToString().Replace(decimalSeparator, "."));
        string pathToMsBuildTools = (string)latestVersionSubKey.GetValue("MSBuildToolsPath");
        return pathToMsBuildTools;
    }
}










public static class Command
{
    private static StringBuilder lastProcessOutput = new StringBuilder();
    
    private static StringBuilder lastStandardErrorOutput = new StringBuilder();    
          
    public static string Execute(string commandPath, string arguments, string capture = null)
    {
        lastProcessOutput.Clear();
        lastStandardErrorOutput.Clear();
        var startInformation = CreateProcessStartInfo(commandPath, arguments);
        var process = CreateProcess(startInformation);
        SetVerbosityLevel(process, capture);        
        process.Start();        
        RunAndWait(process);                
               
        if (process.ExitCode != 0 && commandPath != "robocopy")
        {                      
            WriteLine(lastStandardErrorOutput.ToString());
            throw new InvalidOperationException("Command failed");
        }   
        
        return lastProcessOutput.ToString();
    }

    private static ProcessStartInfo CreateProcessStartInfo(string commandPath, string arguments)
    {
        var startInformation = new ProcessStartInfo(StringUtils.Quote(commandPath));
        startInformation.CreateNoWindow = true;
        startInformation.Arguments =  arguments;
        startInformation.RedirectStandardOutput = true;
        startInformation.RedirectStandardError = true;
        startInformation.UseShellExecute = false;
        
        return startInformation;
    }

    private static void RunAndWait(Process process)
    {        
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();         
        process.WaitForExit();                
    }

    private static void SetVerbosityLevel(Process process, string capture = null)
    {
        if(capture != null)
        {
            process.OutputDataReceived += (s, e) => 
            {
                if (e.Data == null)
                {
                    return;
                }
                              
                if (Regex.Matches(e.Data, capture,RegexOptions.Multiline).Count > 0)
                {
                    lastProcessOutput.AppendLine(e.Data);
                    WriteLine(e.Data);        
                }                                             
            };                        
        }
        process.ErrorDataReceived += (s, e) => 
        {
            lastStandardErrorOutput.AppendLine();
            lastStandardErrorOutput.AppendLine(e.Data);                        
        };
    }

    private static Process CreateProcess(ProcessStartInfo startInformation)
    {
        var process = new Process();
        process.StartInfo = startInformation;       
        //process.EnableRaisingEvents = true;
        return process;
    }
}

public static class StringUtils
{
    public static string Quote(string value)
    {
        return "\"" + value + "\"";
    }
}






    
   

   



   
