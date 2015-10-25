﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using log4net;

namespace Java2Dotnet.Spider.Core.Proxy
{
	public class ProxyUtil
	{
		// TODO 改为单例
		private static IPAddress _localAddr;
		private static readonly ILog Logger = LogManager.GetLogger(typeof(ProxyUtil));

		static ProxyUtil()
		{
			Init();
		}

		private static void Init()
		{
			try
			{
				string hostName = Dns.GetHostName();//本机名   

				IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   
				foreach (IPAddress ip in addressList)
				{
					if (ip.AddressFamily == AddressFamily.InterNetwork)
					{
						_localAddr = ip;
						break;
					}
				}
			}
			catch (Exception e)
			{
				Logger.Error("Failure when init ProxyUtil", e);
				//logger.Error("choose NetworkInterface\n" + getNetworkInterface());
			}
		}

		public static bool ValidateProxy(HttpHost p)
		{
			if (_localAddr == null)
			{
				Logger.Error("cannot get local ip");
				return false;
			}
			bool isReachable = false;
			Socket socket = null;
			try
			{
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IPv4);

				socket.Bind(new IPEndPoint(_localAddr, 0));
				IPAddress address = IPAddress.Parse(p.Host);
				socket.SendTimeout = 3000;
				socket.ReceiveTimeout = 3000;
				socket.Connect(address, p.Port);

				Logger.Debug("SUCCESS - connection established! Local: " + _localAddr + " remote: " + p);
				isReachable = true;
			}
			catch (IOException)
			{
				Logger.Warn("FAILRE - CAN not connect! Local: " + _localAddr + " remote: " + p);
			}
			finally
			{
				if (socket != null)
				{
					try
					{
						socket.Close();
					}
					catch (IOException e)
					{
						Logger.Warn("Error occurred while closing socket of validating proxy", e);
					}
				}
			}
			return isReachable;
		}

		//private static string GetNetworkInterface()
		//{
		//	string networkInterfaceName = "";
		//	Enumeration<NetworkInterface> enumeration = null;
		//	try
		//	{
		//		enumeration = NetworkInterface.getNetworkInterfaces();
		//	}
		//	catch (SocketException e1)
		//	{
		//		e1.printStackTrace();
		//	}
		//	while (enumeration.hasMoreElements())
		//	{
		//		NetworkInterface networkInterface = enumeration.nextElement();
		//		networkInterfaceName += networkInterface.toString() + '\n';
		//		Enumeration<InetAddress> addr = networkInterface.getInetAddresses();
		//		while (addr.hasMoreElements())
		//		{
		//			networkInterfaceName += "\tip:" + addr.nextElement().getHostAddress() + "\n";
		//		}
		//	}
		//	return networkInterfaceName;
		//}
	}
}