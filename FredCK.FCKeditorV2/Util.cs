using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

namespace FredCK.FCKeditorV2
{
    internal sealed class Util
    {
        private Util()
        {
        }

        [DllImport("msvcrt.dll", SetLastError = true)]
        private static extern int _mkdir(string path);
        public static bool ArrayContains(Array array, object value, IComparer comparer)
        {
            foreach (object obj2 in array)
            {
                if (comparer.Compare(obj2, value) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static DirectoryInfo CreateDirectory(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(path));
            try
            {
                if (!dir.Exists)
                {
                    dir.Create();
                }
                return dir;
            }
            catch
            {
                CreateDirectoryUsingDll(dir);
                return new DirectoryInfo(path);
            }
        }

        private static void CreateDirectoryUsingDll(DirectoryInfo dir)
        {
            ArrayList list = new ArrayList();
            while ((dir != null) && !dir.Exists)
            {
                list.Add(dir.FullName);
                dir = dir.Parent;
            }
            if (dir == null)
            {
                throw new DirectoryNotFoundException("Directory \"" + list[list.Count - 1] + "\" not found.");
            }
            for (int i = list.Count - 1; i >= 0; i--)
            {
                string path = (string)list[i];
                int num2 = _mkdir(path);
                if (num2 != 0)
                {
                    throw new ApplicationException(string.Concat(new object[] { "Error calling [msvcrt.dll]:_wmkdir(", path, "), error code: ", num2 }));
                }
            }
        }
    }
}
