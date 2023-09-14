using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CSharpDeferred;
using RSG;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

namespace ExternalScripts
{
	public class AbstractNetworking
	{
		private static Logger log = LogContext.getLogger(typeof(AbstractNetworking));
		
		private string _server = "";
		private string _entryPoint = "";
		private string _secret = "";

		public AbstractNetworking(string server, string entryPoint, string secret)
		{
			this._server = server;
			this._entryPoint = entryPoint;
			this._secret = secret;
		}
		
		public void setServer(string server, string entryPoint)
		{
			this._server = server;
			this._entryPoint = entryPoint;
		}

		public string getUrl(Dictionary<string, string> get)
        {
			var query = new Query(null, get);
			var args = query.getParams != null ? query.getParams : new Dictionary<string, string>();

			addBaseParams(args);

			var sign = signature(args);

			if (!query.toSN)
			{
				if (args.ContainsKey("sig"))
					args["sig"] = sign;
				else
					args.Add("sig", sign);
			}

			string gets = args.Select(x => x.Key + "=" + x.Value).Aggregate((s1, s2) => s1 + "&" + s2);
			var toUrl = _server;
			toUrl += query.entryPoint != null ? query.entryPoint : _entryPoint;
			toUrl += "?" + gets;

			return toUrl;
		}

		public virtual void Request(Dictionary<string, string> get, Dictionary<string, string> post, Action<JSONNode> complete, Action<string> error, string entryPoint = null, bool externalRequest = false)
		{
            if (post == null)
                post = new Dictionary<string, string>();

            var query = new Query(entryPoint, get, post, complete, error);

			POSTGET(query)
				.Then(data =>
				      {
					      onCompleteRequest(data);
					      
					      if (query.onComplete != null)
						      query.onComplete((JSONNode)data);
					  },
				      msg =>
				      {
						  if (get.ContainsKey("noerr"))
                          {
							  // не показывать окно ошибки
                          }
						  else
							onRejectedRequest(msg);

					      if (query.onError != null)
						      query.onError(msg.Message);
				      })
				.Done();
		}

		public virtual void uploadFile(Dictionary<string, string> get, byte[] bytes, Action<JSONNode> complete, Action<string> error, string entryPoint = null, bool externalRequest = false)
		{

			BinaryUpLoad(get, bytes)
				.Then(data =>
				{
					onCompleteRequest(data);

					if (complete != null) complete((JSONNode)data);
				},
					  msg =>
					  {
						  if (get.ContainsKey("noerr"))
						  {
							  // не показывать окно ошибки
						  }
						  else
							  onRejectedRequest(msg);

						  if (error != null) error(msg.Message);
					  })
				.Done();
		}

		protected virtual void onCompleteRequest(object data)
		{
			
		}
		
		protected virtual void onRejectedRequest(object data)
		{
			
		}

		protected virtual IPromise<object> POSTGET(Query query)
		{
			var count = 0;
			var deferred = new Deferred();
			WWWForm data = null;

			var args = query.getParams != null ? query.getParams : new Dictionary<string, string>();
			
			addBaseParams(args);

			var sign = signature(args);

			if (!query.toSN)
			{
				if (args.ContainsKey("sig"))
					args["sig"] = sign;
				else
					args.Add("sig", sign);
			}
			
			string gets = args.Select(x => x.Key + "=" + x.Value).Aggregate((s1, s2) => s1 + "&" + s2);

			if (query.postParams != null)
			{
				data = new WWWForm();
				data.AddField("post", 1);

				if (query.postParams != null)
					foreach (var val in query.postParams)
					{
						data.AddField(val.Key, val.Value);
					}

				GetBasePost(data);
			}
			
			var toUrl = _server;
			toUrl += query.entryPoint != null ? query.entryPoint : _entryPoint;
			toUrl += "?" + gets;

			send();

			return deferred.promise;

			void after(WWW www)
			{
				if (!string.IsNullOrEmpty(www.error))
				{
					if (query.iteration < 3)
					{
						log.warning("Server does not respond : iteration=" + query.iteration + " error=" + www.error);

						query.onErrorIteration();
						send();
					}
					else
					{
						log.info("Server request: " + toUrl);
						log.error("Server does not respond : "   + www.error);
						
						deferred.reject(www.error);
						//onRejectedRequest("Сервер не отвечает");
					}
				}
				else
				{
					if (www.text == null || www.text.Length == 0 || !query.toSN && www.text.Substring(0, 1) != "{")
					{
						log.info("Server request: " + toUrl);
						log.error("Server error : "              + www.text);
						
						deferred.reject(www.error + www.text);

						//onRejectedRequest("Ошибка обработки запроса на сервере");
					}
					else
					{
						if (query.toSN)
							deferred.resolve(www.text);
						else
						{
							var result = JSON.Parse(www.text);

							if (result["error"] != null)
							{
								log.info("Server request: "  + toUrl);
								log.error("Server error: " + result["error"]);
								deferred.reject(result["error"].Value);
							}
							else
							{
								deferred.resolve(result);
							}
						}
					}
				}

				www.Dispose();
			}

			void send()
			{
				log.info("Server request: " + toUrl);

				var sendDeferred = new Deferred();

				sendDeferred.promise.Then(
					response =>
					{
						WWW www = (WWW) response;
						after(www);
					});
				
				ExternalScripts.Utils.StartCoroutine(_send());
				
				IEnumerator _send()
				{
					WWW www = null;
					
					if (data != null)
						www = new WWW(toUrl, data.data); // POST
					else
						www = new WWW(toUrl); // GET
					
					yield return www;
					sendDeferred.resolve(www);
				}
			}
		}

        public virtual IPromise<object> LoadFile(string file)
        {
            var deferred = new Deferred();

            var requesturl = Application.streamingAssetsPath + "/" + file;

            send();

            return deferred.promise;

            void after(WWW www)
            {
                string errorString = null;

                if (www.error != null)
                {
                    log.warning("File does not respond : " + www.error);
                    deferred.reject(errorString);
                }
                else
                {
                    deferred.resolve(www.text);
                }

                www.Dispose();
            }

            void send()
            {
                log.info("Server request: " + requesturl);

                var sendDeferred = new Deferred();

                sendDeferred.promise.Then(
                    response =>
                    {
                        WWW www = (WWW)response;
                        after(www);
                    });

                Utils.StartCoroutine(_send());

                IEnumerator _send()
                {
                    var www = new WWW(requesturl);
                    yield return www;
                    sendDeferred.resolve(www);
                }
            }
        }


        public virtual IPromise<object> BinaryLoadFile(string url, Dictionary<string, string> get = null)
		{
			var deferred = new Deferred();

			var query = new Query(url);

			var getString = "";
			
			if (get != null)
				getString = "?" + get.Select(x => x.Key + "=" + x.Value).Aggregate((s1, s2) => s1 + "&" + s2);
			
			var requesturl = _server + query.entryPoint + getString;
			
			send();
			
			return deferred.promise;
			
			void after(WWW www)
			{
				string errorString = null;
				
				if (www.error != null)
				{
					if (query.iteration < 3)
					{
						log.warning("Server does not respond : iteration=" + query.iteration + " error=" + www.error);

						query.onErrorIteration();
						send();
					}
					else
					{
						errorString = www.error;

						log.info("Server request: "              + requesturl);
						log.warning("Server does not respond : " + www.error);
					}
				}
				else
				{
					if (www.bytes == null || www.bytes.Length == 0)
					{
						log.warning("Server error request: " + requesturl);

						errorString = "servererror";
					}
					else
					{
						deferred.resolve(www.bytes);
					}
				}

				www.Dispose();
				
				if (errorString != null)
					deferred.reject(errorString);
			}

			void send()
			{
				log.info("Server request: " + requesturl);

				var sendDeferred = new Deferred();

				sendDeferred.promise.Then(
					response =>
					{
						WWW www = (WWW) response;
						after(www);
					});
				
				Utils.StartCoroutine(_send());
				
				IEnumerator _send()
				{
					var www = new WWW(requesturl);
					yield return www;
					sendDeferred.resolve(www);
				}
			}
		}
        public virtual IPromise<object> BinaryLoadZipFile(string url, Dictionary<string, string> get = null)
        {
            var deferred = new Deferred();

            if (get == null)
            {
                get = new Dictionary<string, string>();
                //get["tm"] = Model.getServerTime().ToString();
            }

            BinaryLoadFile(url, get)
                .Then(data =>
                {
                    if (data is byte[])
                    {
                        log.info(url + " loaded");

                        string text = BaseUtils.Decompress(data as byte[]);
                        deferred.resolve(JSON.Parse(text));
                    }
                    else
                        deferred.reject("no bytes[]");
                },
                      error => { deferred.reject(error.Message); });

            return deferred.promise;
        }

        public virtual IPromise<object> TextLoadFile(string url, Dictionary<string, string> get = null)
        {
            var deferred = new Deferred();

            var query = new Query(url);

            var getString = "";

            if (get != null)
                getString = "?" + get.Select(x => x.Key + "=" + x.Value).Aggregate((s1, s2) => s1 + "&" + s2);

            var requesturl = _server + query.entryPoint + getString;

            send();

            return deferred.promise;

            void after(WWW www)
            {
                string errorString = null;

                if (www.error != null)
                {
                    if (query.iteration < 3)
                    {
                        log.warning("Server does not respond : iteration=" + query.iteration + " error=" + www.error);

                        query.onErrorIteration();
                        send();
                    }
                    else
                    {
                        errorString = www.error;

                        log.info("Server request: " + requesturl);
                        log.warning("Server does not respond : " + www.error);
                    }
                }
                else
                {
                    if (www.text == null || www.text.Length == 0)
                    {
                        log.warning("Server error request: " + requesturl);
                        errorString = "servererror";
                    }
                    else
                    {
                        deferred.resolve(www.text);
                    }
                }

                www.Dispose();

                if (errorString != null)
                    deferred.reject(errorString);
            }

            void send()
            {
                log.info("Server request: " + requesturl);

                var sendDeferred = new Deferred();

                sendDeferred.promise.Then(
                    response =>
                    {
                        WWW www = (WWW)response;
                        after(www);
                    });

                Utils.StartCoroutine(_send());

                IEnumerator _send()
                {
                    var www = new WWW(requesturl);
                    yield return www;
                    sendDeferred.resolve(www);
                }
            }
        }

        public virtual IPromise<object> TextLoadJson(string url, Dictionary<string, string> get = null)
        {
            var deferred = new Deferred();

            if (get == null)
            {
                get = new Dictionary<string, string>();
                //get["tm"] = Model.getServerTime().ToString();
            }

            TextLoadFile(url, get)
                .Then(data =>
                {
                    deferred.resolve(data);
                },
                error => { deferred.reject(error.Message); });

            return deferred.promise;
        }

        public virtual IPromise<object> BinaryLoad(Dictionary<string, string> get)
		{
			var deferred = new Deferred();

			var args = get != null ? get : new Dictionary<string, string>();
			
			addBaseParams(args);
			
			System.Random rnd = new System.Random();
			if (!args.ContainsKey("rnd")) args.Add("rnd", rnd.Next(1, 100000000).ToString());
			args.Add("sig", signature(args));
			
			string gets = args.Select(x => x.Key + "=" + x.Value).Aggregate((s1, s2) => s1 + "&" + s2);

			var url = _server + _entryPoint;
			url += "?" + gets;

			send();
			
			return deferred.promise;

			void after(UnityWebRequest unityWebRequest)
			{
				if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
				{
					deferred.reject(unityWebRequest.error);
				}
				else
				{
					var res = unityWebRequest.downloadHandler.data;

					if (res == null || res.Length == 0)
					{
						log.info("Server request: " + url);
						log.warning("Server error : "            + res);

						deferred.reject(unityWebRequest.error);
					}
					else
					{
						deferred.resolve(res);
					}
				}
				
				unityWebRequest.Dispose();
			}

			void send()
			{
				var sendDeferred = new Deferred();

				sendDeferred.promise.Then(
					response =>
					{
						UnityWebRequest unityWebRequest = (UnityWebRequest) response;
						after(unityWebRequest);
					});
				
				Utils.StartCoroutine(_send());
				
				IEnumerator _send()
				{
					UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
					yield return unityWebRequest.SendWebRequest();
					sendDeferred.resolve(unityWebRequest);
				}
			}
		}

		public virtual IPromise<object> BinaryUpLoad(Dictionary<string, string> get, byte[] bytes)
		{
			var deferred = new Deferred();
			
			var args = get != null ? get : new Dictionary<string, string>();
			
			addBaseParams(args);

            System.Random rnd = new System.Random();
            if (!args.ContainsKey("rnd")) args.Add("rnd", rnd.Next(1, 100000000).ToString());
            args.Add("sig", signature(args));
			
			string gets = args.Select(x => x.Key + "=" + x.Value).Aggregate((s1, s2) => s1 + "&" + s2);

			var url = _server + _entryPoint;
			url += "?" + gets;

			send();
			
			return deferred.promise;
			
			void after(UnityWebRequest unityWebRequest)
			{
				if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
				{
					WindowTechnicalWork.show();
					deferred.reject(unityWebRequest.error);
				}
				else
				{
					var res = unityWebRequest.downloadHandler.text;

					if (res == null || res.Length == 0 || res.Substring(0, 1) != "{")
					{
						log.info("Server request: " + url);
						log.warning("Server error : " + res);

						deferred.reject(res);
					}
					else
					{
						var result = JSON.Parse(res);

						if (result["error"] != null)
						{
							log.info("Server request: " + url);
							log.warning("Server error: " + result["error"]);
							deferred.reject(result);
						}
						else
						{
							deferred.resolve(result);
						}
					}
				}
				
				unityWebRequest.Dispose();
			}

			void send()
			{
				var sendDeferred = new Deferred();

				sendDeferred.promise.Then(
					response =>
					{
						UnityWebRequest unityWebRequest = (UnityWebRequest) response;
						after(unityWebRequest);
					});
				
				Utils.StartCoroutine(_send());
				
				IEnumerator _send()
				{
					UnityWebRequest unityWebRequest = UnityWebRequest.Put(url, bytes);
					yield return unityWebRequest.SendWebRequest();
					sendDeferred.resolve(unityWebRequest);
				}
			}
		}
		
		public virtual IPromise<object> sendToSN(string method, JSONObject gets)
		{
			Deferred deferred = new Deferred();

			this.addSNArguments(gets);

			var dict = Utils.JsonToDictionary(gets);
			var query = new Query(method, dict);
			query.setToSN(true);

			POSTGET(query)
				.Then(
					data =>
					{
						try
						{
							deferred.resolve(JSON.Parse(data as String));
						}
						catch (Exception e)
						{
							deferred.resolve(new Exception("SN error", e));
						}
					},
					error =>
					{
						deferred.resolve(error.Message);
					}
				)
				.Done();

			return deferred.promise;
		}
		
		protected virtual void addBaseParams(Dictionary<string, string> gets)
		{
			
		}

		protected virtual void GetBasePost(WWWForm Data)
		{

		}
		
		protected virtual void addSNArguments(JSONObject getArgs)
		{

		}

		protected virtual string SignQuery(string par)
		{
			var data = par.Split('&');
			Dictionary<string, string> query = new Dictionary<string, string>();
			foreach (string val in data)
			{
				string[] p = val.Split('=');
				query[p[0]] = p[1];
			}

			var sortedDict = new SortedDictionary<string, string>(query);
			var outstring = "";
			foreach (var kvp in sortedDict)
				outstring += kvp.Key + "=" + kvp.Value;

			outstring += _secret;

			MD5 md5Hasher = MD5.Create();

			byte[] md5data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(outstring));
			StringBuilder sBuilder = new StringBuilder();
			for (int i = 0; i < md5data.Length; i++)
			{
				sBuilder.Append(md5data[i].ToString("x2"));
			}

			return par + "&sig=" + sBuilder.ToString();
		}
		
		protected virtual string signature(Dictionary<string, string> dict)
		{
			var sortedDict = new SortedDictionary<string, string>(dict);
			var outstring = "";
			foreach (var kvp in sortedDict)
				outstring += kvp.Key + "=" + kvp.Value;

			outstring += _secret;

			MD5 md5Hasher = MD5.Create();

			byte[] md5data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(outstring));
			StringBuilder sBuilder = new StringBuilder();
			for (int i = 0; i < md5data.Length; i++)
			{
				sBuilder.Append(md5data[i].ToString("x2"));
			}

			return sBuilder.ToString();
		}
	}
}