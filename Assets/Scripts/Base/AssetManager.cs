using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Base
{
	public class AssetManager
	{
		private static string PREFABS_FOLDER = "Prefabs";

		private static string STANDALONE_POSTFIX = "_st";

        public static Dictionary<string, string> Headers { get; } = new Dictionary<string, string>
        {
            {"Cache-Control", "max-age=86400" },
            {"Content-Version", Settings.versionAssets.ToString() }
        };

		private Main main = null;
		private AssetBundle gameBundle = null;
		private AssetBundle assBundle = null;

		private bool WWWRequest = true;

		public static bool isLocalAssets()
		{
			return false;
		}

		private static string WINDOW_FOLDER
		{
			get
			{
				if (Main.BUILD_STANDALONE)
					return "Windows_ST";

				return "Windows";
			}
		}

		public AssetManager(Main main)
		{
			this.main = main;
		}

		public void onLoadingGame()
		{
			if (assBundle != null)
				assBundle.Unload(true);
		}

		private Dictionary<string, int> assetPairs
		{
			get
			{
				return new Dictionary<string, int>
				{ 
					{"data/main", Settings.versionAssets}
				};
			}
		}

		private static Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle>();

		public IEnumerator LoadAssets(Action callback = null)
		{
			if (isLocalAssets())
			{
				foreach (var assetPair in assetPairs)
					yield return DownloadAssetLocal(assetPair.Key, assetPair.Value);
			}
			else
			{
				foreach (var assetPair in assetPairs)
					yield return DownloadAsset(assetPair.Key, assetPair.Value);
			}
			
			if (callback != null)
				callback();
		}

        public IEnumerator LoadAsset(string assetName, int assetVer, Action callback = null)
        {
            if (isLocalAssets())
            {
                yield return DownloadAssetLocal(assetName, assetVer);
            }
            else
            {
                yield return DownloadAsset(assetName, assetVer);
            }
			    
            if (callback != null) 
                callback();
        }

        public void UnloadAsset(string assetName, bool unloadAllLoadedObjects = false)
        {
            if (assetBundles.TryGetValue(assetName, out var assetBundle))
            {
                assetBundle.Unload(unloadAllLoadedObjects);
				assetBundles.Remove(assetName);
            }
        }

        // Загрузка  ассетов локальная

        public IEnumerator DownloadAssetLocal(string asset, int version = -1)
		{
			var loadingPath = GetAssetsPath(asset, version);
			var localRequest = AssetBundle.LoadFromFileAsync(loadingPath);
			yield return localRequest;

			assetBundles[asset] = localRequest.assetBundle;
		}

		// Загрузка ассетов с сервера
		public IEnumerator DownloadAsset(string asset, int version)
		{
			if (!assetBundles.ContainsKey(asset))
            {
				var loadingPath = GetAssetsPath(asset, version);

				using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(loadingPath)) // Caching
				{
					yield return request.SendWebRequest();

					if (!string.IsNullOrEmpty(request.error))
					{
						Debug.LogError($"Can't download asset : {asset} , reason : {request.error}");
						yield break;
					}

					if (!assetBundles.ContainsKey(asset))
						assetBundles[asset] = DownloadHandlerAssetBundle.GetContent(request);
				}
			}
		}

		// Путь к загружаемым ассетам
		private string GetAssetsPath(string asset, int version)
		{
#if UNITY_EDITOR || !UNITY_WEBGL
			return Path.Combine(Settings.assetUrl(), asset + ".unity3d");
#endif
			return Path.Combine(Settings.assetUrl(), asset + ".unity3d?v=" + version);
		}

		public IEnumerator DownloadBundle(string bundleName, int version, Action after)
		{

#if !UNITY_WEBGL
			while (!Caching.ready)
				yield return null;
#endif
			WWW www = null;
			UnityWebRequest uwr = null;
			
			AssetBundle newBundle;
			if (isLocalAssets())
			{
				var bundleLoadRequest = AssetBundle.LoadFromFileAsync(Settings.assetUrl() + "data/" + bundleName + ".unity3d");
				while (!bundleLoadRequest.isDone)
				{
					yield return new WaitForFixedUpdate();
				}

				newBundle = bundleLoadRequest.assetBundle;
				if (gameBundle == null)
				{
					yield break;
				}
			}
			else
			{
				if (WWWRequest)
				{
					uwr = UnityWebRequestAssetBundle.GetAssetBundle(Settings.assetUrl() + "data/" + bundleName + ".unity3d?v=" + version.ToString());
					yield return uwr.SendWebRequest();

					if (uwr.isNetworkError || uwr.isHttpError)
					{
						yield break;
					}
					else
					{
						newBundle = DownloadHandlerAssetBundle.GetContent(uwr);
					}
				}
				else
				{
					var cachever = PlayerPrefs.GetString("data/" + bundleName);
#if !UNITY_WEBGL
					if (cachever != version.ToString())
						Caching.ClearAllCachedVersions("data/" + bundleName);
#endif

					www = WWW.LoadFromCacheOrDownload(Settings.assetUrl() + "data/" + bundleName + ".unity3d?v=" + version.ToString(), version);
					while (!www.isDone)
					{
						yield return new WaitForFixedUpdate();
					}

					if (!string.IsNullOrEmpty(www.error))
					{
						main.StartCoroutine(DownloadBundle(bundleName, version, after));
						yield break;
					}

					PlayerPrefs.SetString("data/" + bundleName, version.ToString());

					newBundle = www.assetBundle;
				}
			}

			if (www != null) www.Dispose();
			if (uwr != null) uwr.Dispose();

			newBundle.LoadAllAssets();

			if (after != null) after();
		}

		private static Dictionary<string, GameObject> loadedGameObjects = new Dictionary<string, GameObject>();
		private static Dictionary<string, Sprite> loadedSprites = new Dictionary<string, Sprite>();

		public GameObject getPrefabFromBundle(string objName)
		{
			if (string.IsNullOrEmpty(objName)) 
            {
				return null;
			}

			var name = objName.Split('/').Last();

            if (loadedGameObjects.TryGetValue(name, out var value) && value != null)
            {
                return loadedGameObjects[name];
			}
			
			foreach (var assetBundle in assetBundles.Values)
			{
				if (assetBundle.Contains(name))
				{
					var gameObject = assetBundle.LoadAsset<GameObject>(name);
					if (gameObject == null) continue;
					loadedGameObjects[name] = gameObject;
					return loadedGameObjects[name];
				}
			}

			return null;
		}

		public Sprite getSpriteFromBundle(string objName)
		{
			if (string.IsNullOrEmpty(objName)) return null;

			var name = objName.Split('/').Last();

            if (loadedSprites.TryGetValue(name, out var value) && value != null)
            {
                return loadedSprites[name];
            }

			foreach (var assetBundle in assetBundles.Values)
			{
				if (assetBundle!= null && assetBundle.Contains(name))
				{
					var sprite = assetBundle.LoadAsset<Sprite>(name);
					loadedSprites[name] = sprite;
					return loadedSprites[name];
				}
			}

			return Resources.Load<Sprite>(objName);
		}

		public static GameObject getBundlePrefab(string name, Transform parent = null)
		{
			name = PREFABS_FOLDER + "/" + name;
			GameObject prefab = null;

			if (Main.BUILD_UNITY_EDITOR)
				prefab = Resources.Load(name, typeof(GameObject)) as GameObject;

			if (prefab == null)
				prefab = Main.AssetManager.getPrefabFromBundle(name);

			if (prefab == null)
				prefab = Resources.Load(name, typeof(GameObject)) as GameObject;

			if (prefab == null)
			{
				//name = PREFABS_FOLDER + "/" + name;
				prefab = Resources.Load<GameObject>(name);
			}

			if (prefab == null)
			{
				return null;
			}

			GameObject prefabInstance = null;

			prefabInstance = Main.Instantiate(prefab, parent);
			return prefabInstance;
		}

		public static GameObject getResPrefab(string name, Transform parent = null)
		{
            name = PREFABS_FOLDER + "/" + name;
            GameObject prefab = null;

            //if (Main.BUILD_UNITY_EDITOR)
            prefab = Resources.Load(name, typeof(GameObject)) as GameObject;

			if (prefab == null)
				prefab = Resources.Load(name, typeof(GameObject)) as GameObject;

			if (prefab == null)
                prefab = Main.AssetManager.getPrefabFromBundle(name);

            if (prefab == null)
            {
                //name = PREFABS_FOLDER + "/" + name;
				prefab = Resources.Load<GameObject>(name);
            }

            if (prefab == null)
            {
                return null;
            }

            GameObject prefabInstance = null;

            prefabInstance = Main.Instantiate(prefab, parent);

            return prefabInstance;
        }

		public void getImgStreamingAssets(string path, Image image)
		{
			var fullPath = Application.streamingAssetsPath + path + ".png";

			if (!File.Exists(fullPath))
				fullPath = Application.streamingAssetsPath + path + ".jpg";

			//Converts desired path into byte array
			byte[] pngBytes = System.IO.File.ReadAllBytes(fullPath);

			//Creates texture and loads byte array data to create image
			Texture2D tex = new Texture2D(2, 2);
			tex.LoadImage(pngBytes);

			//Creates a new Sprite based on the Texture2D
			image.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f, 0, SpriteMeshType.FullRect);
		}

		public IEnumerator loadImgstreamingAssets(string path, Image image)
        {
            if (string.IsNullOrEmpty(path)) yield return null;
			if (image == null) yield return null;

			path = TrimExtensionFromPath(path);

			var fullPath = Application.streamingAssetsPath + path + ".png";

			if (!File.Exists(fullPath))
				fullPath = Application.streamingAssetsPath + path + ".jpg";

			if (File.Exists(fullPath))
			{
#if !UNITY_EDITOR && UNITY_WEBGL
                WWW www = new WWW(fullPath, null, AssetManager.Headers);
#else
                WWW www = new WWW(fullPath);
#endif
				yield return www;

				if (www.error != null)
				{
					//image.enabled = false;
				}
				else
				{
					var tex = www.texture;
					image.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f, 0, SpriteMeshType.FullRect);
				}
			}
		}

        private static string TrimExtensionFromPath(string path)
        {
            if (path.Contains(".png"))
                path = path.Replace(".png", "");

            if (path.Contains(".jpg"))
                path = path.Replace(".jpg", "");

			return path;
		}

		public static GameObject getPrefab(string name, Transform parent = null)
		{
			name = PREFABS_FOLDER + "/" + name;
			GameObject prefab = null;

			//if (Main.BUILD_UNITY_EDITOR)
				prefab = Resources.Load(name, typeof(GameObject)) as GameObject;

			if (prefab == null)
				prefab = Main.AssetManager.getPrefabFromBundle(name);

			if (prefab == null)
				prefab = Resources.Load(name, typeof(GameObject)) as GameObject;

			if (prefab == null)
			{
				//name = PREFABS_FOLDER + "/" + name;
				prefab = Resources.Load<GameObject>(name);
			}

			if (prefab == null)
			{
				return null;
			}

			GameObject prefabInstance = null;

			prefabInstance = Main.Instantiate(prefab, parent);

			return prefabInstance;
		}

		public static GameObject getWindowPrefab(string name, Transform parent = null, bool checkPlatform = true)
		{
			/*if (checkPlatform)
			{
				if (Main.BUILD_STANDALONE)
					name = WINDOW_FOLDER + "/" + name + STANDALONE_POSTFIX;
				else
					name = "Windows" + "/" + name;
			}

			else*/
			
			name = "Windows" + "/" + name;
			return getPrefab(name, parent);
		}
	}
}