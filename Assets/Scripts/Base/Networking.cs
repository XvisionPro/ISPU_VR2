using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using CSharpDeferred;
using ExternalScripts;
using RSG;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

namespace Base
{
	public class Networking : AbstractNetworking
	{
		private static ExternalScripts.Logger log = LogContext.getLogger(typeof(Networking));

		public Networking(string server, string entryPoint, string secret) : base(server, entryPoint, secret)
		{

		}

		override public void Request(Dictionary<string, string> get, Dictionary<string, string> post, Action<JSONNode> complete, Action<string> error, string entryPoint = null, bool externalRequest = false)
		{
			base.Request(get, post, complete, error, entryPoint);
        }
		
		override protected void onCompleteRequest(object data)
		{
			if (data is JSONNode)
			{
				var result = (JSONNode) data;
				/*if (User.fssk > 0 && result != null && result["ssk"] != null && User.fssk != result["ssk"].AsInt)
				{
					WindowInfo.show("information", "sskerror", "", "ok", "", delegate { CommonWindow.show("WindowLoading", true, null); });
					throw new Exception("sskerror");
				}*/
			}
		}
		
		override protected void onRejectedRequest(object data)
		{
			Main.showLog("Server error " + data.ToString());
			WindowTechnicalWork.show();
			Debug.LogError(data.ToString());
		}

        override protected void addBaseParams(Dictionary<string, string> gets)
		{
			if (gets != null)
			{
				/*gets.Add("uid", Main.User.id.ToString());
				gets.Add("tm", Model.getServerTime().ToString());
				gets.Add("lasttm", Main.User.lasttm.ToString());

				System.Random rnd = new System.Random();
				gets.Add("rnd", rnd.Next(1, 100000000).ToString());
                
                gets.Add("auth", Main.User.authKey);*/
            }
		}

		override protected void GetBasePost(WWWForm Data)
		{
			//Data.AddField("auth", Main.User.authKey);
		}
	}
}