using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Net.NetworkInformation;
using Assets.Scripts.Logic;

public class UDPReceiver : MonoBehaviour
{
    UdpClient client;
    IPEndPoint endPoint;
    Thread listener;

    private int port = 7000;
    private string hostName = "127.0.0.1";
    private string ethernetName = "Ethernet 2";
    byte[] data;

    private bool ReceiveThreadIsRunning;
    Queue pQueue = Queue.Synchronized(new Queue()); // holds the packet queue

    void Start()
    {
       
    }

    public void setConfig(string _hostName, int _port)
    {
        hostName = _hostName;
        port = _port;
    }

    public async void init(string _hostName, int _port, string _ethernetName)
    {
        setConfig(_hostName, _port);
        ethernetName = _ethernetName;

        //List<string> ipAddrList = new List<string>();
        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (item.Name == ethernetName && item.NetworkInterfaceType == NetworkInterfaceType.Ethernet && item.OperationalStatus == OperationalStatus.Up)
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        hostName = ip.Address.ToString();
                        //ipAddrList.Add(ip.Address.ToString());
                    }
                }
            }
        }

        OnDisconnect();

        client = new UdpClient();
        client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        
        Main.showLog("Подключение к приему данных.... адрес " + hostName + " порт " + port);
        endPoint = new IPEndPoint(IPAddress.Parse(hostName), port);
        client.Client.Bind(endPoint);
        ReceiveThreadIsRunning = true;
        //GetData();
        //UDPData data = new UDPData(endPoint, client);

        //client.BeginReceive(CallBackRecvive, data);

        if (listener != null) listener.Abort();
        listener = new Thread(new ThreadStart(GetData));
        listener.IsBackground = true;
        listener.Start();
    }

    /*private void CallBackRecvive(IAsyncResult ar)
    {
        if (ar.AsyncState is UDPData state)
        {
            IPEndPoint ipEndPoint = state.EndPoint;
            byte[] data = state.UDPClient.EndReceive(ar, ref ipEndPoint);
            string receiveData = Encoding.Default.GetString(data);
            Debug.Log(receiveData);
            state.UDPClient.BeginReceive(CallBackRecvive, state);
        }
    }*/

    void Update()
    {
        lock (pQueue.SyncRoot)
        {
            if (pQueue.Count > 0)
            {
                byte[] data = (byte[]) pQueue.Dequeue();
                Main.Instance.network.recieve(data);
                pQueue.Clear();
            }
        }
    }

    private void GetData()
    {
        while (ReceiveThreadIsRunning)
        {
            //try
            //{
                data = client.Receive(ref endPoint);
                pQueue.Enqueue(data);
            /*}
            catch (Exception e)
            {
                Debug.Log("ERROR  - Invalid recieve data " + e.ToString());
            }*/
        }
        Debug.Log("Close connection");
    }

    public void OnDisconnect()
    {
        ReceiveThreadIsRunning = false;
        if (listener != null) listener.Abort();
        if (client != null) client.Close();
    }
}
