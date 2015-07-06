using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace node_flat
{
  public delegate void Command(DirectoryInfo[] dirs);
  public class DirUtils
  {
    public void Walk(DirectoryInfo rootDir, Command cmd, string filter)
    {
      var dirs = rootDir.GetDirectories(filter);

      if(dirs.Count() == 1)
      {
        var filteredDirectory = dirs[0];
        var filteredDirectoryInnerDirs = filteredDirectory.GetDirectories();
        cmd(filteredDirectoryInnerDirs);
        foreach(var d in filteredDirectoryInnerDirs)
        {
          Walk(d, cmd, filter);
        }
      }
    }

    public void CopyFromTo(string sourceDirName, string destDirName, string filter)
    {
      DirectoryInfo dir = new DirectoryInfo(sourceDirName);
      DirectoryInfo[] dirs = dir.GetDirectories();
      if(!Directory.Exists(destDirName))
      {
        Directory.CreateDirectory(destDirName);
      }

      FileInfo[] files = dir.GetFiles();
      foreach(var file in files)
      {
        var temppath = Path.Combine(destDirName, file.Name);
        file.CopyTo(temppath, true);
      }
    }
  }
}
