using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPClient : MonoBehaviour
{
	private TcpClient socketConnectionRead;

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
		try
		{
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
			socketConnectionRead = new TcpClient(hostName, port);
			Byte[] bytes = new Byte[1024];
			socketConnectionRead.GetStream().Flush();

			while (socketConnectionRead.Connected)
			{
				using (NetworkStream stream = socketConnectionRead.GetStream())
				{
					int length;
					while (stream != null)
					{
						if (stream.DataAvailable)
						{
							length = stream.Read(bytes, 0, bytes.Length);
							var incommingData = new byte[length];
							Array.Copy(bytes, 0, incommingData, 0, length);
							stream.Flush();
							pQueue.Enqueue(incommingData);
							//Main.Instance.network.recieve(incommingData);
							//string serverMessage = Encoding.ASCII.GetString(incommingData);
						}
						else
							Thread.Sleep(TimeSpan.FromMilliseconds(100));
					}
				}
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket recieve exception: " + socketException);
		}
	}

	public void Send(string mess)
	{
		if (socketConnectionRead == null)
		{
			return;
		}
		try
		{
			NetworkStream stream = socketConnectionRead.GetStream();
			if (stream.CanWrite)
			{
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(mess);
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket send exception: " + socketException);
		}
	}

	public void OnDisconnect()
	{
		if (socketConnectionRead != null)
		{
			socketConnectionRead.GetStream().Flush();
			socketConnectionRead.GetStream().Close();
			socketConnectionRead.Close();
			socketConnectionRead.Dispose();
		}

		if (clientReceiveThread != null)
		{
			clientReceiveThread.Abort();
		}
	}
}