using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ExternalScripts;
using JetBrains.Annotations;
using SimpleJSON;
using UnityEngine;

public static class BaseUtils
{
    private static ExternalScripts.Logger log = LogContext.getLogger(typeof(BaseUtils));

    public static int toInt(string val)
    {
        int res = 0;
        try
        {
            res = int.Parse(val);
        }
        catch
        {
        }

        return res;
    }

    public static float toFloat(string val)
    {
        float fval = 0;
        if (val.Contains(",") && "," != CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator) val = val.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
        if (val.Contains(".") && "." != CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator) val = val.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

        //if (val.Contains(".")) val = val.Replace(".", ",");

        try
        {
            fval = float.Parse(val, CultureInfo.InvariantCulture);
            //fval = System.Convert.ToSingle(val);
        }
        catch
        {
        }

        return fval;
    }

    public static string toString(float val)
    {
        string sval = val.ToString();
        if (sval.Contains(",") && "," != CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator) sval = sval.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
        if (sval.Contains(".") && "." != CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator) sval = sval.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
        return sval;
    }

    public static string Decompress(byte[] data)
    {
        using (var compressedStream = new MemoryStream(data))
        {
            System.IO.Compression.GZipStream instream = new System.IO.Compression.GZipStream(compressedStream, System.IO.Compression.CompressionMode.Decompress);

            using (StreamReader reader = new StreamReader(instream))
            {
                return reader.ReadToEnd();
            }
        }
    }

    public static byte[] Compress(string value)
    {
        byte[] byteArray = stringToBytes(value);
        var compressedStream = new System.IO.MemoryStream();
        var instream =
            new System.IO.Compression.GZipStream(compressedStream, System.IO.Compression.CompressionMode.Compress);

        //Compress
        instream.Write(byteArray, 0, byteArray.Length);
        //Close, DO NOT FLUSH cause bytes will go missing...
        instream.Close();

        byteArray = compressedStream.ToArray();

        compressedStream.Close();
        instream.Dispose();
        compressedStream.Dispose();

        return byteArray;
    }

    public static byte[] stringToBytes(string value)
    {
        byte[] byteArray = new byte[value.Length];
        int indexBA = 0;
        foreach (char item in value.ToCharArray())
        {
            byteArray[indexBA++] = (byte)item;
        }
        return byteArray;
    }

    public static JSONNode DictionaryToJson(Dictionary<string, string> data)
    {
        var res = JSONNode.Parse("{}");
        foreach (var d in data)
        {
            res[d.Key] = d.Value;
        }

        return res;
    }

    public static JSONNode DictionaryToJson(Dictionary<string, List<string>> data)
    {
        var res = JSONNode.Parse("{}");
        foreach (var d in data)
        {
            foreach (var l in d.Value)
            {
                res[d.Key].Add(l);
            }
        }

        return res;
    }

    public static JSONNode ArrayToJson(string[] items)
    {
        var res = JSONNode.Parse("{}");
        foreach (var it in items)
        {
            res.Add(it, it);
        }

        return res;
    }

    public static void MemoryLog()
    {
        log.info("Memory total: " + UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() / 1024 / 1024 + " Mb " +
                 " / allocated: " + UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / 1024 / 1024 + " Mb" +
                 " / free: " + UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong() / 1024 / 1024 + " Mb" +
                 " / graphics: " + UnityEngine.Profiling.Profiler.GetAllocatedMemoryForGraphicsDriver() / 1024 / 1024 +
                 " Mb ");
    }

    public static void SaveData(object objectToSave, string fileName, bool convertToJson = true)
    {
        string FullFilePath = Application.persistentDataPath + "/" + fileName + ".bin";
        StreamWriter sw = new StreamWriter(FullFilePath);
        var txt = objectToSave.ToString();

        if (convertToJson)
            txt = JsonUtility.ToJson(objectToSave);

        //sw.Write(EncodeDecrypt(txt));
        sw.Write(txt);
        sw.Close();
    }

    public static string LoadData(string fileName)
    {
        string FullFilePath = Application.persistentDataPath + "/" + fileName + ".bin";
        if (File.Exists(FullFilePath))
        {
            try
            {
                StreamReader sw = new StreamReader(FullFilePath);
                string json = sw.ReadToEnd();

                if (json[0] != '{')
                    json = EncodeDecrypt(json);

                sw.Close();
                return json;
            }
            catch
            {
                return "{}";
            }
            
        }
        else
        {
            return "{}";
        }
    }

    public static void MoveFile(string fileFrom, string fileTo)
    {
        if (File.Exists(fileFrom))
            File.Move(fileFrom, fileTo);
    }

    public static string LoadFile(string FullFilePath)
    {
        string data = "";
        if (File.Exists(FullFilePath))
        {
            StreamReader sw = new StreamReader(FullFilePath);
            data = sw.ReadToEnd();
            sw.Close();
        }

        return data;
    }

    public static long FileSize(string file)
    {
        string FullFilePath = Application.persistentDataPath + "/" + file;
        if (File.Exists(FullFilePath))
        {
            var info = new System.IO.FileInfo(FullFilePath);
            return info.Length;
        }

        return 0;
    }

    public static void AddToFile(string data, string file)
    {
        string path = Application.persistentDataPath + "/" + file;
        StreamWriter sw = new StreamWriter(path, true);
        sw.WriteLine(data);
        sw.Close();
    }

    public static void DeleteData(string fileName)
    {
        File.Delete(Application.persistentDataPath + "/" + fileName + ".bin");
    }

    public static void DeleteFile(string fileName)
    {
        File.Delete(Application.persistentDataPath + "/" + fileName);
    }

    public static void DeleteDirectory(string name)
    {
        if (Directory.Exists(Application.persistentDataPath + "/" + name))
            Directory.Delete(Application.persistentDataPath + "/" + name, true);
    }

    public static void CheckDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public static string EncodeDecrypt(string str, ushort secretKey = 0x0777)
    {
        var ch = str.ToArray(); //преобразуем строку в символы
        string newStr = ""; //переменная которая будет содержать зашифрованную строку
        foreach (var c in ch) //выбираем каждый элемент из массива символов нашей строки
            newStr += TopSecret(c,
                secretKey); //производим шифрование каждого отдельного элемента и сохраняем его в строку
        return newStr;
    }

    public static char TopSecret(char character, ushort secretKey)
    {
        character = (char) (character ^ secretKey); //Производим XOR операцию
        return character;
    }

    public static float osVersion()
    {
        string ver = "0";

        if (Main.BUILD_ANDROID)
        {
            var osText = SystemInfo.operatingSystem;
            var ind = osText.IndexOf("OS") + 3;
            var os = osText.Substring(ind);
            ver = os.Substring(0, 2);
            ver = clearString(ver);
        }

        return BaseUtils.toFloat(ver);
    }

    public static string clearString(string str)
    {
        var charsToRemove = new string[] {"@", ",", ".", ";", "'", " "};
        foreach (var c in charsToRemove)
        {
            str = str.Replace(c, string.Empty);
        }

        return str;
    }
    
    [Obsolete("This method is very expensive! Try to avoid usage. Use Animator.SetTrigger() as much as possible.")]
    public static void SetSingleTrigger(this Animator animator, string trigger)
    {
        var triggers = animator.parameters.Where(parameter => parameter.type == AnimatorControllerParameterType.Trigger).ToArray();

        if (triggers.Length > 0)
        {
            for (int i = 0; i < triggers.Length; i++)
            {
                animator.ResetTrigger(triggers[i].name);
            }

            animator.SetTrigger(trigger);
        }
    }

    public static float ClampRotation(float value, float minValue, float maxValue)
    {
        if (value < 180 && value > maxValue)
            value = maxValue;

        if (minValue < 0)
        {
            if (value > 180 && (value - 360) < minValue)
                value = 360 + minValue;
        }
        else
        {
            if (value < minValue) value = minValue;
        }
        return value;
    }

    public static DateTime TimestampToDate(long unixTimeSeconds)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds).UtcDateTime.ToLocalTime();
    }
    public static int DateToTimestamp(DateTime dateTime)
    {
        return (int)new DateTimeOffset(dateTime).ToUnixTimeSeconds();
    }

    public static string TimeToString(long unixTimeSeconds, string pattern = "")
    {
        if (pattern == "") pattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        TimeZoneInfo localZone = TimeZoneInfo.Local;
        int currentOffset = (int)localZone.BaseUtcOffset.TotalSeconds;
        DateTime yesterday = DateTime.Today.AddDays(-1).AddHours(24);
        DateTime mDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTimeSeconds + currentOffset);
        int delta = (mDate.Date - yesterday.Date).Days;
        
        if (delta >= 0)
            pattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        else
            pattern = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;

        return mDate.ToString(pattern);
    }

    public static string DateTimeToString(DateTime date, string pattern = "")
    {
        if (pattern == "") pattern = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
        return date.ToString(pattern);
    }

    public static DateTime TimeFromString(string day, string time = "")
    {
        //"2020-5-6 12:10:15"
        //12.01.2022
        //8:00 - 9:35
        // "dd:MM:yyyy HH:mm:ss"
        string date = day;

        if (time != "")
        {
            var tmp = time.Split('-');
            date += " " + tmp[0];
        }

        return Convert.ToDateTime(date);
    }

    public static string TimeSecondsToString(int time)
    {
        int hours = time / 3600;
        int minutes = (time % 3600) / 60;
        int seconds = (time % 3600) % 60;

        return hours + ":" + minutes + ":" + seconds;
    }

    public static void RemoveAllChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++) { GameObject.Destroy(parent.GetChild(i).gameObject); }
    }
}