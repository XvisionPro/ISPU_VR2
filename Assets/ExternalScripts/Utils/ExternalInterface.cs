using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using SimpleJSON;
using UnityEngine;

namespace ExternalScripts
{
    public class ExternalInterface : MonoBehaviour
    {
        protected static Logger log = LogContext.getLogger(typeof(ExternalInterface));

        private static Dictionary<string, Func<object, object>> callbacks =
            new Dictionary<string, Func<object, object>>();

        public static bool available
        {
            get { return true; }
        }

        /**
		 * Вызывается из iframe
		 * unityInstance.SendMessage("ExternalInterface", "callback", '{"name" : "testFunc", "data": 1}');
		 * "ExternalInterface" - Название объекта на сцене.
		 * "callback" - Нзвание функции которую вызвать. Оставить так.
		 * data - JSON; name - название функции, data - что передать
		 * Предварительно нужно добавить callback ExternalInterface.addCallback("testFunc", testFunction);
		 */
        public void callback(object obj)
        {
            var callbackObject = JSON.Parse((string) obj);

            string name = callbackObject["name"];
            object data = callbackObject["data"];
            bool needResponse = callbackObject["needResponse"];

            log.info("callback " + obj);

            if (callbacks.ContainsKey(name))
            {
                var result = callbacks[name](data);

                if (result != null)
                {
                    if (needResponse)
                    {
                        call("Response_" + name, data);
                    }
                }
            }
        }

        /**Добавить callback с названием name, для доступа из iframe**/
        public static void addCallback(string name, Func<object, object> callback)
        {
            callbacks.Add(name, callback);
        }

        /**Вызвать функцию из iframe**/
        public static void call(string functionName, params object[] args)
        {
            if (available)
            {
                Application.ExternalCall(functionName, args);
            }
        }
        /**Вывести лог в консоль браузера**/
        public static void info(string str)
        {
            if (available)
                Application.ExternalCall("console.log", str);
        }

        /**Вывести варнинг в консоль браузера**/
        public static void warning(string str)
        {
            if (available)
                Application.ExternalCall("console.warn", str);
        }

        /**Вывести эррор в консоль браузера**/
        public static void error(string str)
        {
            if (available)
                Application.ExternalCall("console.error", str);
        }

        public static void onGameLoaded(Func<object, object> callback)
        {
            if (available)
            {
                addCallback("onGameLoaded", callback);
                call("onGameLoaded");
            }
        }

        public static void StartLoadTexture(string uid, string url)
        {
            if (available)
            {
                addCallback("startLoadTextureCallback", StartLoadTextureCallback);
                call("startLoadTexture", uid, url);
            }
        }

        public static void OpenLink(string link)
        {
            if (available)
            {
                //addCallback("startLoadTextureCallback", StartLoadTextureCallback);
                call("openLink", link);
            }
        }

        #region LoadTexture

        public static Dictionary<string, Texture2D> jsLoadedTextures = new Dictionary<string, Texture2D>();

        private static object StartLoadTextureCallback(object obj)
        {
            JSONObject data = (JSONObject)obj;

            if (data == null)
            {
                log.error("Data is null!");
                return null;
            }

            var uid = data["uid"];
            var dataUrl = data["dataUrl"];

            if (jsLoadedTextures.ContainsKey(uid) && jsLoadedTextures[uid] != null) return null;

            var base64 = DataUrlToBase64(dataUrl);

            log.info("base64 = " + base64);

            var texture = Base64ToTexture2D(base64);

            jsLoadedTextures[uid] = texture;

            return null;
        }

        private static string DataUrlToBase64(string data)
        {
            return data.Split(',')[1];
        }

        private static Texture2D Base64ToTexture2D(string base64)
        {
            Texture2D texture = new Texture2D(100, 100);

            byte[] bytes = Convert.FromBase64String(base64);

            texture.LoadImage(bytes);

            log.info("Texture was loaded!");

            return texture;
        }

        #endregion
    }
}