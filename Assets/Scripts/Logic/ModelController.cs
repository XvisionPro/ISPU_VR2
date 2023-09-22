using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Assets.Scripts.Logic
{
    public class ModelController : MonoBehaviour
	{
		public static Dictionary<string, BaseModel> AllModels = new Dictionary<string, BaseModel>();
		public static string[] AllVars;
		public static List<BaseModel> ShowedModels = new List<BaseModel>();

		private Dictionary<string, string> vars;
		public static bool isConnected = false;
		public static bool modelInited = false;
		public static bool modelNeedUpdate = false;
		
		public static BaseModel curModel;
		public TMP_Text mesU_txt;
		public TMP_Text mesI_txt;
		public TMP_Text mesR_txt;

		public float mesU;
		public float mesI;
		public float mesR;
		
		public float curTime;

		public ShowMesureLine mesure1, mesure2;
		public GameObject multimetr;

		public ModelController()
        {
			init();
		}

		public void init()
		{
			vars = new Dictionary<string, string>();
			AllModels = new Dictionary<string, BaseModel>();
		}

        private void Update()
        {
			curTime += Time.deltaTime;
			showMesure(false);
			
			mesU_txt.text = Mathf.Lerp(0, mesU, curTime * 2f).ToString("0.00");
			mesI_txt.text = Mathf.Lerp(0, mesI, curTime * 1.8f).ToString("0.00");
			mesR_txt.text = Mathf.Lerp(0, mesR, curTime * 2.5f).ToString("0.00");
		}

		public void initModels(string[] mess)
        {
			destroyModel("#ALL");
			AllModels = new Dictionary<string, BaseModel>();

			foreach (var var in mess)
			{
				var tmpvar = var.Split("=");
				if (tmpvar.Length == 2 && tmpvar[0] != "")
				{
					if (AllModels.ContainsKey(tmpvar[0]))
					{
						Debug.Log("Dublicate model name " + tmpvar[0]);
					}
					else
						AllModels.Add(tmpvar[0], new BaseModel() { baseName = tmpvar[0], typeName = tmpvar[1] });
				}
			}
		}

		public void initVars(string[] mess)
		{
			AllVars = new string[mess.Length - 1];

			var k = 0;
			for (var i = 1; i < mess.Length; i++)
			{
				AllVars[k] = mess[i];
				/*if (AllVars.ContainsKey(mess[i]))
					Debug.Log("Dublicate var name " + mess[i]);
				else
					AllModels.Add(mess[i], k);*/

				k++;
			}
		}

		public void updateData(string[] mess)
        {
			Dictionary<string, string> inmess = new Dictionary<string, string>();

			foreach (var var in mess)
			{
				var tmpvar = var.Split("=");
				if (tmpvar.Length == 2 && tmpvar[0] != "")
				{
					if (inmess.ContainsKey(tmpvar[0]))
					{
						Debug.Log("Dublicate send vars " + tmpvar[0]);
					}
					else
						inmess.Add(tmpvar[0], tmpvar[1]);

					Main.ModelController.updateVar(tmpvar[0], tmpvar[1]);
				}
				/*else
					Debug.Log("ERROR  - Invalid recieve data " + var);*/
			}
		}

		public void updateDataBin(float[] floats)
		{
			if (AllVars == null) return;
			for (var i = 2; i < floats.Length; i++)
            {
				Main.ModelController.updateVar(AllVars[i-2], floats[i].ToString("0.00"));
			}
		}

		public void updateVar(string par, string val)
		{
			if (!isConnected) return;

			if (vars.ContainsKey(par))
				vars[par] = val;
			else
				vars.Add(par, val);
		}

		public string getVar(string par)
		{
			if (vars.ContainsKey(par))
				return vars[par];
			else
				return "**";
		}

		public float getFloatVar(string par)
		{
			if (vars.ContainsKey(par))
				return BaseUtils.toFloat(vars[par]);
			else
				return 0;
		}

		public float getVarCalc(string baseName, string expr)
		{
			var tmp = expr.Split(" ");
			float res = getFloatVar(baseName + tmp[0]);
			for (var i = 0; i < tmp.Length - 1; i++)
			{
				if (i + 1 < tmp.Length - 1)
				{
					if (tmp[i] == "+") res += getFloatVar(baseName + tmp[i + 1]);
					if (tmp[i] == "-") res -= getFloatVar(baseName + tmp[i + 1]);
					if (tmp[i] == "*") res *= getFloatVar(baseName + tmp[i + 1]);
					if (tmp[i] == "/") res /= getFloatVar(baseName + tmp[i + 1]);
				}
			}

			return res;
		}

		public void showModel(BaseModel mdl)
		{
			if (curModel != null)
			{
				destroyModel(curModel.baseName);
			}
			curModel = BaseModel.init(mdl.baseName, mdl.typeName, Main.Instance.Scene);
			curModel.setLocation(0);
			Main.Hud.onShowPanel(false);
			ClearMesure();
			//Main.Instance.Player.SetActive(true);
        }

		public static BaseModel isModelOpen(string name)
		{
			ShowedModels.RemoveAll(x => x == null);

			foreach (BaseModel mdl in ShowedModels)
			{
				if (mdl != null && mdl.baseName == name)
				{
					return mdl;
				}
			}

			return null;
		}

		public static void destroyModel(string name)
		{
			ShowedModels.RemoveAll(x => x == null);

			var cnt = ShowedModels.Count;
			for (var i = cnt - 1; i >= 0; i--)
			{
				if (ShowedModels[i] != null && (ShowedModels[i].baseName == name || name == "#ALL"))
				{
					Destroy(ShowedModels[i].gameObject);
					Destroy(ShowedModels[i]);
					ShowedModels.RemoveAt(i);
				}
			}
		}

		public void showMesure(bool contact)
		{
			var cont1 = mesure1.cont;
			var cont2 = mesure2.cont;

			if (contact) curTime = 0;

			if (cont1 != null && cont2 != null)
			{
				if (cont1.baseModel == cont2.baseModel)
				{
					multimetr.SetActive(true);
					mesU = cont1.baseModel.getU(cont1, cont2);
					mesI = 0;
					mesR = cont1.baseModel.getR(cont1.var, cont2.var);
					return;
				}
			}
			
			multimetr.SetActive(false);
			mesU_txt.text = "";
			mesI_txt.text = "";
			mesR_txt.text = "";
		}

		public void ClearMesure()
        {
			mesure1.Clear();
			mesure2.Clear();
		}
	}
}
