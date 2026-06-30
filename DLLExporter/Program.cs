using DllExporter;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;


namespace DLLExporter
{
    internal class Program
    {
        private static bool isDebuggable = false;

        private static string Assemble(string assemblerPath, string assemblyPath, string workDir)
        {
            string str = string.Format(@"{0}\{1}", workDir, Path.GetFileName(assemblyPath));
            string path = string.Format(@"{0}\{1}", workDir, "input.res");
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("\"{0}\\output.il\" /debug /out:\"{1}\"", workDir, str);
            if (Path.GetExtension(assemblyPath) == ".dll")
            {
                builder.Append(" /dll");
            }
            if (File.Exists(path))
            {
                builder.AppendFormat(" /res:\"{0}\"", path);
            }
            ProcessStartInfo startInfo = new ProcessStartInfo(assemblerPath, builder.ToString())
            {
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process process = Process.Start(startInfo);
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception("ilasm.exe has failed assembling generated source.");
            }
            return str;
        }

        private static string Disassemble(string disassemblerPath, string assemblyPath, string workDir)
        {
            string str = string.Format(@"{0}\input.il", workDir);
            ProcessStartInfo startInfo = new ProcessStartInfo(disassemblerPath, string.Format("\"{0}\" /out:\"{1}\" /LINENUM", assemblyPath, str))
            {
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process process = Process.Start(startInfo);
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception(string.Format("ildasm.exe has failed disassembling {0}.", assemblyPath));
            }
            return str;
        }

        private static string GetAssemblerPath()
        {
            string path = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\ilasm.exe");
            if (!File.Exists(path))
            {
                throw new Exception("Cannot locate ilasm.exe.");
            }
            return path;
        }

        private static string GetDisassemblerPath()
        {
            Exception exception = new Exception("Cannot locate ildasm.exe.");
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SDKs\NETFXSDK\4.6.1") ?? Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SDKs\.NETFramework\v2.0");
            if (key == null)
            {
                throw exception;
            }
            string path = (string)key.GetValue("InstallationFolder");
            if (path == null)
            {
                throw exception;
            }
            path = path + @"Bin\NETFX 4.6.1 Tools\ildasm.exe";
            if (!File.Exists(path))
            {
                throw exception;
            }
            return path;
        }

        private static List<string> GetMethods(string assemblyPath)
        {
            List<string> list = new List<string>();
            //GTH Test
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            //Assembly assembly = Assembly.ReflectionOnlyLoad(assemblyPath);
            foreach (Type type in assembly.GetTypes())
            {
                foreach (MethodInfo info in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static))
                {
                    object[] customAttributes = info.GetCustomAttributes(typeof(DllExportAttribute), false);
                    if (customAttributes.Length == 1)
                    {
                        DllExportAttribute attribute = (DllExportAttribute)customAttributes[0];
                        list.Add(attribute.EntryPoint ?? info.Name);
                    }
                }
            }
            return list;
        }

        private static DirectoryInfo GetWorkingDirectory()
        {
            DirectoryInfo info = new DirectoryInfo(Environment.ExpandEnvironmentVariables(@"%TEMP%\DllExporter"));
            if (!info.Exists)
            {
                info.Create();
            }
            return info;
        }

        private static bool IsDebuggable(string assemblyPath)
        {
            object[] customAttributes = Assembly.LoadFrom(assemblyPath).GetCustomAttributes(typeof(DebuggableAttribute), true);
            //object[] customAttributes = Assembly.ReflectionOnlyLoad.LoadFrom(assemblyPath).GetCustomAttributes(typeof(DebuggableAttribute), true);
            return ((customAttributes != null) && (customAttributes.Length != 0));
        }

        private static void Main(string[] args)
        {
            if (!((args.Length == 1) && File.Exists(args[0])))
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("1. Add reference to DllExporter.exe to your project;");
                Console.WriteLine("2. Add DllExporter.DllExport attribute to static methods that will be exported;");
                Console.WriteLine("3. Add following post-build command to project properties:");
                Console.WriteLine("    DllExporter.exe $(TargetFileName)");
                Console.WriteLine("    move $(TargetName).Exports$(TargetExt) $(TargetFileName)");
                Console.WriteLine("4. Build project;");
                Console.WriteLine("5. ???");
                Console.WriteLine("6. PROFIT!");
            }
            else
            {
                try
                {
                    DirectoryInfo workingDirectory = GetWorkingDirectory();
                    string assemblerPath = GetAssemblerPath();
                    string disassemblerPath = GetDisassemblerPath();
                    List<string> methods = GetMethods(args[0]);
                    isDebuggable = IsDebuggable(args[0]);
                    string sourcePath = Disassemble(disassemblerPath, args[0], workingDirectory.FullName);
                    string outPath = workingDirectory + @"\output.il";
                    ProcessSource(sourcePath, outPath, methods);
                    string path = Assemble(assemblerPath, args[0], workingDirectory.FullName);
                    string str6 = Path.Combine(Path.GetDirectoryName(args[0]), Path.GetFileNameWithoutExtension(path) + ".Exports" + Path.GetExtension(path));
                    File.Delete(str6);
                    File.Move(path, str6);
                    if (isDebuggable)
                    {
                        //File.Move(Path.Combine(workingDirectory.FullName, Path.GetFileNameWithoutExtension(path) + ".pdb"), Path.Combine(Path.GetDirectoryName(args[0]), Path.GetFileNameWithoutExtension(path) + ".pdb"));
                        string srcFile = Path.Combine(workingDirectory.FullName, Path.GetFileNameWithoutExtension(path) + ".pdb");
                        string dstFile = Path.Combine(Path.GetDirectoryName(args[0]), Path.GetFileNameWithoutExtension(path) + ".pdb1");
                        File.Move(srcFile, dstFile);
                    }
                    workingDirectory.Delete(true);
                }
                catch (Exception exception)
                {
                    Console.Error.WriteLine(exception.Message);
                    Environment.Exit(1);
                }
            }
        }

        private static void ProcessSource(string sourcePath, string outPath, List<string> methods)
        {
            using (StreamWriter writer = new StreamWriter(outPath, false, Encoding.Default))
            {
                int num = 0;
                int num2 = 0;
                int num3 = 0;
                bool flag = false;
                foreach (string str in File.ReadAllLines(sourcePath, Encoding.Default))
                {
                    if (num2 > 0)
                    {
                        num2--;
                    }
                    else if (str.TrimStart(new char[] { ' ' }).StartsWith(".assembly extern DllExporter"))
                    {
                        num2 = 3;
                    }
                    else if (str.TrimStart(new char[] { ' ' }).StartsWith(".corflags"))
                    {
                        writer.WriteLine(".corflags 0x00000002");
                        int num4 = 1;
                        while (num4 <= methods.Count)
                        {
                            writer.WriteLine(".vtfixup [1] int32 fromunmanaged at VT_{0}", num4);
                            num4++;
                        }
                        for (num4 = 1; num4 <= methods.Count; num4++)
                        {
                            writer.WriteLine(".data VT_{0} = int32(0)", num4);
                        }
                    }
                    else
                    {
                        string str2;
                        int num6;
                        if (str.TrimStart(new char[] { ' ' }).StartsWith(".method"))
                        {
                            flag = str.Contains(" static ");
                        }
                        //if (str.TrimStart(new char[] { ' ' }).StartsWith(".custom instance void [DllExporter]DllExporter.DllExportAttribute"))
                        if (str.Contains(".custom instance void [DLLExporter]DllExporter.DllExportAttribute"))
                        {
                            str2 = str;
                            num6 = 0;
                            while (num6 < str2.Length)
                            {
                                switch (str2[num6])
                                {
                                    case '(':
                                        num3++;
                                        break;

                                    case ')':
                                        num3--;
                                        break;
                                }
                                num6++;
                            }
                            if (flag)
                            {
                                writer.WriteLine(".vtentry {0} : 1", num + 1);
                                writer.WriteLine(".export [{0}] as {1}", num + 1, methods[num]);
                                num++;
                            }
                        }
                        else if (num3 > 0)
                        {
                            str2 = str;
                            for (num6 = 0; num6 < str2.Length; num6++)
                            {
                                switch (str2[num6])
                                {
                                    case '(':
                                        num3++;
                                        break;

                                    case ')':
                                        num3--;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            writer.WriteLine(str);
                        }
                    }
                }
            }
        }
    }
}
