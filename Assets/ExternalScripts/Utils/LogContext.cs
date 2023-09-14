using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ExternalScripts
{
	public class LogContext
	{
		public static Logger getLogger(Type t)
		{
			var split = t.ToString().Split('.');
			var str = split[split.Length - 1];

			return new Logger(str);
		}
	}

	public class Logger
	{
		public static bool NEED_WRITE_TO_FILE = false;
		public static bool NEED_SHOW_IN_CONSOLE = false;
		public static bool NEED_SHOW_IN_BROWSER = false;

		private string _cls = "";

		public Logger(string cls)
		{
			this._cls = "[" + cls + "] ";
		}


		public void info(object obj)
		{
			DateTime now = DateTime.Now;
			var time = now.ToString("HH:mm:ss")    + " ";
			string result = time + "[INFO]" + _cls + obj;

			if (NEED_WRITE_TO_FILE)
			{
				string path = Application.persistentDataPath + "/debugLog.txt";
				StreamWriter sw = new StreamWriter(path, true);
				sw.WriteLine(result + "\n");
				sw.Close();
			}

			if (NEED_SHOW_IN_CONSOLE)
				Debug.Log(result);

			if (NEED_SHOW_IN_BROWSER)
				ExternalInterface.info(result);
		}

		public void warning(object obj)
		{
			DateTime now = DateTime.Now;
			var time = now.ToString("HH:mm:ss") + " ";

			string result = time + "[WARNING]" + _cls + obj;

			if (NEED_WRITE_TO_FILE)
			{
				string path = Application.persistentDataPath + "/debugLog.txt";
				StreamWriter sw = new StreamWriter(path, true);
				sw.WriteLine(result + "\n");
				sw.Close();
			}

			if (NEED_SHOW_IN_CONSOLE)
				Debug.LogWarning(result);

			if (NEED_SHOW_IN_BROWSER)
				ExternalInterface.warning(result);
		}

		public void error(object obj)
		{
			DateTime now = DateTime.Now;
			var time = now.ToString("HH:mm:ss") + " ";

			string result = time + "[ERROR]" + _cls + obj;

			if (NEED_WRITE_TO_FILE)
			{
				string path = Application.persistentDataPath + "/debugLog.txt";
				StreamWriter sw = new StreamWriter(path, true);
				sw.WriteLine(result + "\n");
				sw.Close();
			}

			if (NEED_SHOW_IN_CONSOLE)
				Debug.LogError(result);

			if (NEED_SHOW_IN_BROWSER)
				ExternalInterface.error(result);
		}
	}
}