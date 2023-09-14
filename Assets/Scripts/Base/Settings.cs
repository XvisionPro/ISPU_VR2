using Base;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public static string version = "1.2.4";
    public static int versionAssets = 1;

    public static bool SHOW_LOG = true;

    public static string serverurl  = "https://abiturreg.ispu.ru/0.mess/";
    public static string asseturl = "https://abiturreg.ispu.ru/0.mess/assets/";
    public static string fileurl = "https://abiturreg.ispu.ru/0.mess/data/";
    public static string settFile = "sett.dat";

    public static JSONNode localSett;

    //**********************************************************************************************
    public static bool useZip = true;

    //**********************************************************************************************
    public static string entryPoint { get { return "api.php"; } }

    public static string serverurlMobile
    {
        get
        {
            return serverurl ;
        }
    }

    public static string getBuildPrefix()
    {
        var prefix = "";
        if (Main.BUILD_WEBGL && !Main.BUILD_UNITY_EDITOR)
            prefix = "webgl";
        else if (Main.BUILD_ANDROID)
            prefix = "and";
        else if (Main.BUILD_IOS)
            prefix = "ios";
        else if (Main.BUILD_STANDALONE || Main.BUILD_UNITY_EDITOR)
            prefix = "win";

        return prefix;
    }

    public static string assetUrl(bool needPrefix = false)
    {
        string prefix = "";
        if (needPrefix) prefix = getBuildPrefix();
        return asseturl + prefix + "/";
    }

    public static void setServerUrl(string url, string assets = "")
    {
        serverurl = url;

        if (assets != null && assets != "")
            asseturl = assets;

        ExternalScripts.Logger.NEED_WRITE_TO_FILE = false;
        ExternalScripts.Logger.NEED_SHOW_IN_BROWSER = false;
        ExternalScripts.Logger.NEED_SHOW_IN_CONSOLE = SHOW_LOG;
        Main.Networking.setServer(serverurl, entryPoint);
    }

    public static void loadSett()
    {
        localSett = JSON.Parse(BaseUtils.LoadData(Settings.settFile));
    }
    public static void saveSett()
    {
        BaseUtils.SaveData(localSett, Settings.settFile, false);
    }

    public static void setSettings()
    {
        if (Settings.localSett != null)
        {
        }
    }
}
