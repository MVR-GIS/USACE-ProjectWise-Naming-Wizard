using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

public class BPSUtilities
{
    private static readonly object _syncObject = new object();

    public static Hashtable BuildHashTableFromString(string sList, string sDelimiter)
    {
        Hashtable hashtable = new Hashtable();
        string[] strArray = sList.Split(sDelimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strArray.Length; i++)
        {
            if (!hashtable.Contains(strArray[i].ToLower()))
            {
                hashtable.Add(strArray[i].ToLower(), strArray[i].ToLower());
            }
        }
        return hashtable;
    }

    public static SortedList<string, string> BuildListFromString(string sList, string sDelimiter)
    {
        SortedList<string, string> list = new SortedList<string, string>(StringComparer.CurrentCultureIgnoreCase);
        string[] strArray = sList.Split(sDelimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strArray.Length; i++)
        {
            if (!list.ContainsKey(strArray[i]))
            {
                list.Add(strArray[i], strArray[i]);
            }
        }
        return list;
    }

    public static bool GetBooleanSetting(string sSettingName)
    {
        bool result = false;
        if ((ConfigurationManager.AppSettings[sSettingName] != null) && !bool.TryParse(ConfigurationManager.AppSettings[sSettingName], out result))
        {
            if (ConfigurationManager.AppSettings[sSettingName].ToLower() == "true")
            {
                return true;
            }
            if (ConfigurationManager.AppSettings[sSettingName].ToLower() == "false")
            {
                return false;
            }
            if (ConfigurationManager.AppSettings[sSettingName].ToLower() == "yes")
            {
                return true;
            }
            if (ConfigurationManager.AppSettings[sSettingName].ToLower() == "no")
            {
                return false;
            }
            if (ConfigurationManager.AppSettings[sSettingName] == "1")
            {
                return true;
            }
            if (ConfigurationManager.AppSettings[sSettingName] == "0")
            {
                result = false;
            }
        }
        return result;
    }

    public static bool GetBooleanSettingFromDLLConfig(string sSettingName)
    {
        bool result = false;
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if ((configuration.AppSettings.Settings[sSettingName] != null) && !bool.TryParse(configuration.AppSettings.Settings[sSettingName].Value, out result))
        {
            string str = configuration.AppSettings.Settings[sSettingName].Value;
            if (str.ToLower() == "true")
            {
                return true;
            }
            if (str.ToLower() == "false")
            {
                return false;
            }
            if (str == "yes")
            {
                return true;
            }
            if (str == "no")
            {
                return false;
            }
            if (str == "1")
            {
                return true;
            }
            if (str == "0")
            {
                result = false;
            }
        }
        return result;
    }

    public static bool GetEmbeddedResourceFile(string sResourceName, string sTargetFile)
    {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        if (File.Exists(sTargetFile))
        {
            try
            {
                File.Delete(sTargetFile);
            }
            catch
            {
            }
        }
        try
        {
            using (Stream stream = executingAssembly.GetManifestResourceStream(sResourceName))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                File.WriteAllBytes(sTargetFile, buffer);
            }
        }
        catch (Exception exception)
        {
            WriteLog("Error: {0}\n{1}", new object[] { exception.Message, exception.StackTrace });
        }
        return File.Exists(sTargetFile);
    }

    public static int GetIntSetting(string sSettingName)
    {
        int result = 0;
        if (ConfigurationManager.AppSettings[sSettingName] != null)
        {
            int.TryParse(ConfigurationManager.AppSettings[sSettingName], out result);
        }
        return result;
    }

    public static int GetIntSettingFromDLLConfig(string sSettingName)
    {
        int result = 0;
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration.AppSettings.Settings[sSettingName] != null)
        {
            int.TryParse(configuration.AppSettings.Settings[sSettingName].Value, out result);
        }
        return result;
    }

    public static string GetSetting(string sSettingName)
    {
        if (ConfigurationManager.AppSettings[sSettingName] != null)
        {
            return ConfigurationManager.AppSettings[sSettingName];
        }
        return string.Empty;
    }

    public static string GetSettingFromDLLConfig(string sSettingName)
    {
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        if (configuration.AppSettings.Settings[sSettingName] != null)
        {
            return configuration.AppSettings.Settings[sSettingName].Value;
        }
        return string.Empty;
    }

    public static bool IsFileLocked(string sFileName)
    {
        if (File.Exists(sFileName))
        {
            FileInfo info = new FileInfo(sFileName);
            FileStream stream = null;
            try
            {
                stream = info.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
        return false;
    }

    public static void LogError(string sMessage, params object[] args)
    {
        WriteLog("[Error  ] " + sMessage, args);
    }

    public static void LogInfo(string sMessage, params object[] args)
    {
        WriteLog("[Info   ] " + sMessage, args);
    }

    public static void LogWarning(string sMessage, params object[] args)
    {
        WriteLog("[Warning] " + sMessage, args);
    }

    public static void WriteLog(string sMessage)
    {
        Console.WriteLine(sMessage);
        string location = Assembly.GetCallingAssembly().Location;
        string str2 = string.Format("{3}_{0:D4}{1:D2}{2:D2}.log", new object[] { DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Path.GetFileNameWithoutExtension(location) });
        if (!string.IsNullOrEmpty(GetSetting("LogFolder")))
        {
            str2 = Path.Combine(GetSetting("LogFolder"), str2);
        }
        if (!string.IsNullOrEmpty(str2))
        {
            try
            {
                lock (_syncObject)
                {
                    using (StreamWriter writer = new StreamWriter(str2, true))
                    {
                        writer.WriteLine(string.Format("{0:u}\t{1}", DateTime.Now, sMessage));
                    }
                }
            }
            catch (Exception)
            {
                string.Format("{0:u}\t{1}", DateTime.Now, sMessage);
            }
        }
    }

    public static void WriteLog(string sMessage, params object[] args)
    {
        string location = Assembly.GetCallingAssembly().Location;
        string str2 = string.Format("{3}_{0:D4}{1:D2}{2:D2}.log", new object[] { DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Path.GetFileNameWithoutExtension(location) });
        Console.WriteLine(string.Format(sMessage, args));
        if (!string.IsNullOrEmpty(GetSetting("LogFolder")))
        {
            str2 = Path.Combine(GetSetting("LogFolder"), str2);
        }
        if (!string.IsNullOrEmpty(str2))
        {
            try
            {
                lock (_syncObject)
                {
                    using (StreamWriter writer = new StreamWriter(str2, true))
                    {
                        string str3 = string.Format("{0:u}\t{1}", DateTime.Now, string.Format(sMessage, args));
                        writer.WriteLine(str3);
                    }
                }
            }
            catch (Exception)
            {
                try
                {
                    string.Format("{0:u}\t{1}", DateTime.Now, string.Format(sMessage, args));
                }
                catch (Exception)
                {
                }
            }
        }
    }

    public static void WriteLogToFile(string sMessage, string sLogFilePath)
    {
        Console.WriteLine(sMessage);
        string location = Assembly.GetCallingAssembly().Location;
        string str2 = string.Format(@"{4}\{3}_{0:D4}{1:D2}{2:D2}.log", new object[] { DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Path.GetFileNameWithoutExtension(location), sLogFilePath });
        if (!string.IsNullOrEmpty(str2))
        {
            try
            {
                lock (_syncObject)
                {
                    using (StreamWriter writer = new StreamWriter(str2, true))
                    {
                        writer.WriteLine(string.Format("{0:u}\t{1}", DateTime.Now, sMessage));
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
