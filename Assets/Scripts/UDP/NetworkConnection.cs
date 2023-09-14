using Assets.Scripts.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkConnection: MonoBehaviour
{
	public bool TCPConnection = true;
	public UDPReceiver UDPReceiver;
	public UDPSender UDPSender;
	//public TCPReciever TCPReceiver;
    public TCPSocket TCPSender;

    public bool isEmulation = false;

    public int InPort = 7000;
    public string InHostName = "127.0.0.1";

    public int OutPort = 5904;
    public string OutHostName = "213.219.228.19";

    public void init()
    {
        if (isEmulation)
        {
            OutPort = 7001;
            OutHostName = "192.168.1.70";
        }

        if (TCPConnection)
        {
            //TCPReceiver.init(InHostName, InPort);
            TCPSender.init(OutHostName, OutPort);
        }
		else
        {
			UDPReceiver.init(InHostName, InPort, "");
			UDPSender.init(OutHostName, OutPort);
		}
	}

	public void send(string mess)
	{
		if (TCPConnection)
		{
            TCPSender.Send(mess);
        }
		else
		{
			UDPSender.Send(mess);
		}
	}

	public void recieve(byte[] data)
    {
        var firstArray = new float[2];
        if (data.Length > 8)
        {
            Buffer.BlockCopy(data, 0, firstArray, 0, 8);
        }

        string mess = Encoding.ASCII.GetString(data);
        string[] tmpmess = mess.Split(":");
        if (ModelController.modelInited)
        {
            if (firstArray[0] == AllTypes.MESS_BINDATA)
            {
                int cnt = (int)firstArray[1];
                if (cnt <= data.Length)
                {
                    var floatArray = new float[cnt / 4];
                    Buffer.BlockCopy(data, 0, floatArray, 0, cnt);
                    Main.ModelController.updateDataBin(floatArray);
                }
                else
                {
                    Debug.Log("Network - not valid packet");
                }
            }
            else if (tmpmess[0] == AllTypes.MESS_DATA)
            {
                Main.ModelController.updateData(tmpmess);
            }
        }
        else if (tmpmess[0] == AllTypes.MESS_MODELS)
        {
            Main.ModelController.initModels(tmpmess);
            Main.showLog("Получили данные по моделям");
            Main.Hud.modelsPanel.show();
            send(AllTypes.MESS_COMM + ":" + AllTypes.COMM_VARS);
        }
        else if (tmpmess[0] == AllTypes.MESS_VARS)
        {
            Main.ModelController.initVars(tmpmess);
            Main.showLog("Получили данные по переменным");
            ModelController.modelInited = true;
        }
    }

    public void disconnect()
    {
        ModelController.isConnected = false;
        ModelController.modelInited = false;
        if (ModelController.curModel != null)
            ModelController.destroyModel(ModelController.curModel.baseName);

        TCPSender.OnDisconnect();
        //TCPReceiver.OnDisconnect();
        UDPSender.OnDisconnect();
        UDPReceiver.OnDisconnect();
    }

    private void OnApplicationQuit()
    {
        disconnect();
    }
}
