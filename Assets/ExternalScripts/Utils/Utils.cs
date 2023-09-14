using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using AOT;
using CSharpDeferred;
using RSG;
using SimpleJSON;
using UnityEngine;

namespace ExternalScripts
{
	public class Utils
    {
		private static ExternalScripts.Logger log = LogContext.getLogger(typeof(Utils));
		
		private static MonoBehaviour _mb = null;

		public static void setMainContainer(MonoBehaviour mainContainer)
		{
			_mb = mainContainer;
		}
		
		public static IPromise<object> wait(float time)
		{
			Deferred deferred = new Deferred();

			if (time == 0f)
				deferred.resolve();
			else
			{
				if (_mb != null)
                    _mb.StartCoroutine(timer());
				else
					log.warning("_mb is null");
			}

			return deferred.promise;

			IEnumerator timer()
			{
				yield return new WaitForSeconds(time);
				deferred.resolve(null);
			}
		}

		public static string[] convertToStringArray(JSONNode jsonNode)
		{
			if (jsonNode == null)
				return new string[] { };

            string patten = @"[""\[\]]";
            Regex regex = new Regex(patten);

            return jsonNode.Linq.Select(x => regex.Replace(x.Value.ToString(), "")).ToArray();
		}
		
		public static Dictionary<string, GameObject> getAllObjectsInTransform(Transform trans, string prefix = "_", Dictionary<string, GameObject> contentDictionary = null)
		{
			if (contentDictionary == null)
				contentDictionary = new Dictionary<string, GameObject>();
			
			if (prefix == null || trans.name.StartsWith(prefix))
			{
				contentDictionary.Add(trans.name, trans.gameObject);
			}
        
			foreach (Transform child in trans)
				getAllObjectsInTransform(child, prefix, contentDictionary);

			return contentDictionary;
		}
		
		public static Dictionary<string, string> JsonToDictionary(JSONObject json)
		{
			var res = new Dictionary<string, string>();
			if (json != null)
			{
				foreach (string key in json.Keys)
				{
					res.Add(key, json[key]);
				}
			}
			return res;
		}

		public static void StartCoroutine(IEnumerator routine)
		{
			if (_mb != null)
				_mb.StartCoroutine(routine);
			else
				log.warning("_mb is null");
		}

        public static Transform FindTransform(Transform parent, string name)
        {
            if (parent.name.Equals(name)) return parent;
            foreach (Transform child in parent)
            {
                Transform result = FindTransform(child, name);
                if (result != null) return result;
            }
            return null;
        }

        public static void DestroyChildren(Transform transform)
        {
            if (transform == null) return;

            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
            transform.DetachChildren();
        }

        public static Vector3 emptyVector
        {
            get
            {
                return new Vector3(999, 999, 999);
            }
        }

		public static string TimeToFormat(int tm, string format, bool shorttime)
        {
			var ts = TimeSpan.FromSeconds(tm);
			return  string.Format("{0:00}:{1:00}:{2:00}", (ts.Hours + ts.Days * 24), ts.Minutes, ts.Seconds);
		}
	}
}