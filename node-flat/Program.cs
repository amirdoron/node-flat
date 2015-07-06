using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace node_flat
{
  class Program
  {
    static void Main(string[] args)
    {

      if(!Directory.Exists(args[1]))
      {
        Console.WriteLine("error: bad source path {0}", args[1]);
        return;
      }
      if(!Directory.Exists(args[2]))
      {
        Console.WriteLine("error: bad dest path {0}", args[2]);
        return;
      }

      string nodeModulesSrc = args[1];
      mPackModulesDest = args[2];
      Command cmd = new Command(Pack);

      try
      {
        ExecuteCommand(nodeModulesSrc, cmd);
      }
      catch(Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }

    private static void ExecuteCommand(string nodeModulesSrc, Command cmd)
    {
      mNumOfPackedPackages = 0;
      var myDirUtil = new DirUtils();
      DirectoryInfo di = new DirectoryInfo(nodeModulesSrc);

      Console.WriteLine("start packing ...");

      myDirUtil.Walk(di, cmd, "node_modules");

      Console.WriteLine("packing ended, packed " + mNumOfPackedPackages + " packages");
    }

    private static void Pack(DirectoryInfo[] dirs)
    {
      int numOfPackagesToPack = dirs.Count();
      if(numOfPackagesToPack == 0)
      {
        return;
      }
      mNumOfPackedPackages += numOfPackagesToPack;

      var myDirUtils = new DirUtils();
      System.Diagnostics.Process process = new System.Diagnostics.Process();
      System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
      startInfo.WorkingDirectory = mPackModulesDest;
      startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
      startInfo.FileName = "cmd.exe";

      StringBuilder listOfModules = new StringBuilder();
      foreach(var d in dirs)
      {
        listOfModules.Append(d.FullName + " ");
        Console.WriteLine("packing " + d.FullName);
      }

      startInfo.Arguments = "/C npm pack " + listOfModules;
      process.StartInfo = startInfo;
      process.Start();
      process.WaitForExit();
    }

    private static string mPackModulesDest;
    private static int mNumOfPackedPackages = 0;
  }
}
