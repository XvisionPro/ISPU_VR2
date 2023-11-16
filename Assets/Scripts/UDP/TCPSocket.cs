using Assets.Scripts.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPSocket : MonoBehaviour
{
	private Socket socketConnection;

	private Thread clientReceiveThread;

	private int port = 7000;
	private string hostName = "127.0.0.1";

	Queue pQueue = Queue.Synchronized(new Queue()); // holds the packet queue
	
	void Update()
	{
		lock (pQueue.SyncRoot)
		{
			if (pQueue.Count > 0)
			{
				byte[] data = (byte[])pQueue.Dequeue();
				Main.Instance.network.recieve(data);
				pQueue.Clear();
			}
		}

		if (ModelController.isConnected && Main.Instance.network.TCPConnection)
        {
			if (socketConnection != null && !socketConnection.Connected) Main.Instance.network.disconnect();
		}
	}

	public void setConfig(string _hostName, int _port)
	{
		hostName = _hostName;
		port = _port;
	}

	public void init(string _hostName, int _port)
	{
		setConfig(_hostName, _port);

		ConnectToTcpServer();
	}	
	
	private void ConnectToTcpServer()
	{
		int i = 0;
		try
		{
			socketConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socketConnection.Connect(new IPEndPoint(IPAddress.Parse(hostName), port));

			clientReceiveThread = new Thread(new ThreadStart(ListenForData));
			clientReceiveThread.IsBackground = true;
			clientReceiveThread.Start();
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}
	}

	private void ListenForData()
	{
		try
		{
			Byte[] bytes = new Byte[1024];
			socketConnection.NoDelay = true;

			while (socketConnection.Connected)
			{
				int length = socketConnection.Receive(bytes); 
				var incommingData = new byte[length];
				Array.Copy(bytes, 0, incommingData, 0, length);
				pQueue.Enqueue(incommingData);
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket recieve exception: " + socketException);
			socketConnection.Close();
		}
	}

	public void Send(string mess)
	{
		if (socketConnection == null)
		{
			return;
		}
		try
		{
			byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(mess);
			socketConnection.Send(clientMessageAsByteArray);
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket send exception: " + socketException);
			socketConnection.Close();
		}
	}

	public void OnDisconnect()
	{
		if (socketConnection != null)
		{
			socketConnection.Close();
			socketConnection.Dispose();
			socketConnection = null;
		}

		if (clientReceiveThread != null)
		{
			clientReceiveThread.Abort();
		}
	}
}