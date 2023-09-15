using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Logic;
using Base;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
	private List<string> logs = new List<string>();

	public Transform Scene;

	public GameObject MainCanvas;
	public GameObject HudCanvas;
	public GameObject WinCanvas;
	public GameObject SystemCanvas;
	public Hud hud;
    public ShowLocation location;

    [SerializeField]
	public OrbitCamera orbitCamera;
    public GameObject Player;

    public  NetworkConnection network;

	private AssetManager _assetManager { get; set; }
	private Networking _networking { get; set; }
	private ModelController _modelController { get; set; }
	
	private static Main _instance;

	public static Main Instance { get { return _instance; } }
	public static AssetManager AssetManager { get { return _instance._assetManager; } }
	public static Networking Networking { get { return _instance._networking; } }
	public static ModelController ModelController { get { return _instance._modelController; } }

	public static Hud Hud{ get { return _instance.hud; } }
		
	private void Start()
	{
		if (Instance == null)
			_instance = this;
		else
		{
			Destroy(gameObject);
			return;
		}

		_assetManager = new AssetManager(this);
		hud.init();

		showLog("Инициализация....");
		_modelController = GetComponent<ModelController>();
		_modelController.showMesure(false);
		Settings.loadSett();

		var serverurl = Settings.serverurl;
		//_networking = new Networking(serverurl, Settings.entryPoint, "sdf34hfnhv4556");
		createScene();
    }

	void Awake()
	{

	}

	void Update()
	{
		if (WinCanvas == null) 
			WinCanvas = GameObject.Find("WindowsCanvas");
	}

    public void createScene()
    {
		var station = AssetManager.getResPrefab("Electric", Scene);
    }
    
	public void setOrbitCamera(Transform target)
    {
		orbitCamera.target = target;
	}


	public void OnConnect()
    {
		WindowConnect.show(() => {
			_modelController.init();
			network.init();
			//network.send(AllTypes.MESS_COMM + ":" + AllTypes.COMM_INIT);
			//network.send(AllTypes.MESS_COMM + ":" + AllTypes.COMM_VARS);
			ModelController.isConnected = true;

			Delay(1f, () => { network.send(AllTypes.MESS_COMM + ":" + AllTypes.COMM_INIT); });
		});
	}

	public static void showLog(string mess)
	{
		return;

		var syncContext = System.Threading.SynchronizationContext.Current;

		// On your worker thread
		syncContext.Post(_ =>
		{
			// This code here will run on the main thread
			var logs = Main.Instance.logs;
			if (logs.Count > 20) logs.RemoveRange(0, logs.Count - 20);
			logs.Add(mess);
			Hud.log_text.text = "";

			foreach (string log in logs)
				Hud.log_text.text = log + "\n\r" + Hud.log_text.text;

		}, null);	
	}

	public void StopSceneCoroutine(Coroutine corutine)
	{
		if (corutine != null)
			StopCoroutine(corutine);
	}

	public void StopGameCoroutine(List<Coroutine> listCoroutine)
	{
		if (listCoroutine != null)
			foreach (Coroutine cr in listCoroutine)
			{
				StopCoroutine(cr);
			}
	}

	public Coroutine Delay(float timeout, Action after)
	{
		if (timeout == 0)
		{
			if (after != null) after();
		}
		else
		{
			return StartCoroutine(DelayCoroutine(timeout, after));
		}
		return null;
	}

	public IEnumerator DelayCoroutine(float timeout, Action after)
	{
		yield return new WaitForSeconds(timeout);

		if (after != null) after();
	}

	public static Transform WindowParent
	{
		get
		{
			if (_instance.WinCanvas == null)
				return null;

			return _instance.WinCanvas.transform;
		}
	}

	public static Transform SystemWindowParent
	{
		get
		{
			if (_instance.SystemCanvas == null)
				return null;

			return _instance.SystemCanvas.transform;
		}
	}

	public static bool BUILD_WEBGL
	{
		get
		{
#if UNITY_WEBGL
			return true;
#endif
			return false;
		}
	}

	public static bool BUILD_IOS
	{
		get
		{
#if UNITY_IOS
			return true;
#endif
			return false;
		}
	}

	public static bool BUILD_ANDROID
	{
		get
		{
#if UNITY_ANDROID
			return true;
#endif
			return false;
		}
	}

	public static bool BUILD_STANDALONE
	{
		get
		{
#if UNITY_STANDALONE
			return true;
#endif
			return false;
		}
	}

	public static bool BUILD_UNITY_EDITOR
	{
		get
		{
#if UNITY_EDITOR
			return true;
#endif
			return false;
		}
	}

	public static bool BUILD_MOBILE
	{
		get { return BUILD_IOS || BUILD_ANDROID; }
	}
}