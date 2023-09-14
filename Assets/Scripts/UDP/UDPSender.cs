using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class UDPSender : MonoBehaviour
{
    UdpClient client;
    IPEndPoint endPoint;
    
    private int port = 7000;
    private string hostName = "127.0.0.1";

    byte[] data;

    void Start()
    {

    }

    public void setConfig(string _hostName, int _port)
    {
        hostName = _hostName;
        port = _port;
    }

    public void init(string _hostName, int _port)
    {
        setConfig(_hostName, _port);

        OnDisconnect();

        Main.showLog("Подключение к отправке команд.... адрес " + hostName + " порт " + port);
        client = new UdpClient();
        endPoint = new IPEndPoint(IPAddress.Parse(hostName), port);
    }

    public void Send(string mess)
    {
        data = Encoding.ASCII.GetBytes(mess);
        Debug.Log("send = " + mess);
        //BitConverter.GetBytes(mess);
        client.Send(data, data.Length, endPoint);
    }

    public void OnDisconnect()
    {
        if (client != null) client.Close();
    }
}
