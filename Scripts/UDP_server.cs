using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

using UnityEngine.UI;
using System.Threading;
using System.Text;


public class UDP_server : MonoBehaviour
{

    private static UDP_server instance;
    private UdpClient udpServer;
    private Thread serverThread;
    private bool isServerRunning = false;

    public string messagePack = "Searching For Connection";

    IPAddress remoteAddress = IPAddress.Parse("192.168.2.15");

    public bool dataReceived = false;
    public bool startTimeSync = false;
    public bool beginGame = false;


    // Singleton instance property
    public static UDP_server Instance
    {
        get
        {
            if (instance == null)
            {
                // Create a new GameObject with the UDP_server script attached

                GameObject singletonObject = new GameObject("UDP_server");
                instance = singletonObject.AddComponent<UDP_server>();
                DontDestroyOnLoad(singletonObject);
            }
            return instance;
        }
    }


    private void Start()
    {
        //       StartServer();
    }

    private void OnDestroy()
    {
        if(serverThread != null)
            serverThread.Abort();
        StopServer();
    }

    //private void StartServer()
    public void StartServer()
    {
        //udpServer = new UdpClient(12345);
        udpServer = new UdpClient(8080);
        serverThread = new Thread(ServerThreadFunction);
        serverThread.Start();
        isServerRunning = true;
    }
    private void StopServer()
    {
        if (isServerRunning)
        {
            udpServer.Close();
            serverThread.Join(); // Wait for the server thread to finish
            isServerRunning = false;
        }
    }

    public void transmitData(NetworkDataSend nd)
    {
        udpServer.Send(Encoding.UTF8.GetBytes(nd.ToJsonString()), nd.ToJsonString().Length, GameManger.instance.GetServerIP(), 8080);
        //udpServer.Send(Encoding.UTF8.GetBytes(nd.ToJsonString()), nd.ToJsonString().Length, "192.168.2.137", 8080);
        //udpServer.Send(Encoding.UTF8.GetBytes(nd.ToJsonString()), nd.ToJsonString().Length, "127.0.0.1", 12346);
        //udpServer.Send(Encoding.UTF8.GetBytes(nd.ToJsonString()), nd.ToJsonString().Length, "127.0.0.1", 12345);
        //StartCoroutine(TransmitDataCoroutine(nd));
    }

 public void transmitData(string nd)
    {
        if(nd != null)
        udpServer.Send(Encoding.UTF8.GetBytes(nd), nd.Length, GameManger.instance.GetServerIP(), 8080);
        //udpServer.Send(Encoding.UTF8.GetBytes(nd), nd.Length, "192.168.2.137", sendingPort);
        //udpServer.Send(Encoding.UTF8.GetBytes(nd.ToJsonString()), nd.ToJsonString().Length, "127.0.0.1", 12346);
        //udpServer.Send(Encoding.UTF8.GetBytes(nd.ToJsonString()), nd.ToJsonString().Length, "127.0.0.1", 12345);
        //StartCoroutine(TransmitDataCoroutine(nd));
    }

    private void ServerThreadFunction()
    {
        try
        {
            while (isServerRunning)
            {




                Debug.Log($"Server thread ID: {Thread.CurrentThread.ManagedThreadId}");
                if(!dataReceived)
                {
                    Thread.Sleep(100);
                    transmitData("START");
                    Thread.Sleep(1000);
                    transmitData("START");
                    dataReceived = true;
                }

                //IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpServer.Receive(ref remoteEndPoint);

                //Check message 
                string message = System.Text.Encoding.UTF8.GetString(data);

                Debug.Log($"Received from {remoteEndPoint}: {message}");
               
                //byte[] responseData = Encoding.ASCII.GetBytes(response);
                //udpServer.Send(responseData, responseData.Length, GameManger.instance.GetServerIP(), 8080);





                // string[] times =

                // Debug.Log($"Received from {remoteEndPoint}: {message}");






                if (message.Equals("START"))
                startTimeSync = true;
                else{
                    ProcessJSONData(remoteEndPoint, data);
                }

                //Thread.Sleep(1000);
            }
        }
        catch (Exception e)
        {
            // Debug.LogError($"Error in server thread: {e.Message}");
            Debug.Log($"Failed to parse JSON {e.Message}");
        }
    }


    //private void ProcessReceivedData(IPEndPoint remoteEndPoint, byte[] data)
    private void ProcessJSONData(IPEndPoint remoteEndPoint, byte[] data)
    {
        // Process the received data here
        string message = System.Text.Encoding.UTF8.GetString(data);
        Debug.Log($"Received from {remoteEndPoint}: {message}");

        // Parse the JSON string using a simple parser
        Dictionary<string, object> jsonData = SimpleJsonParse(message);

        if (jsonData != null)
        {

            float player_xpos = float.Parse(jsonData["player_xpos"].ToString());
            float player_ypos = float.Parse(jsonData["player_ypos"].ToString());
            //float laser_xpos = float.Parse(jsonData["laser_xpos"].ToString());
            //float laser_ypos = float.Parse(jsonData["laser_ypos"].ToString());
            bool player_fired = bool.Parse(jsonData["player_fired"].ToString());
            double player_time = double.Parse(jsonData["time"].ToString());
            NetworkDataReceive.Instance.Player_xpos = player_xpos;
            NetworkDataReceive.Instance.Player_ypos = player_ypos;
            //NetworkDataReceive.Instance.Laser_xpos = laser_xpos;
            //NetworkDataReceive.Instance.Laser_ypos = laser_ypos;
            NetworkDataReceive.Instance.Player_fired = player_fired;    
            NetworkDataReceive.Instance.Player_Time = player_time;

            Debug.Log($"Received Data: Player_xpos={player_xpos}, Player_ypos={player_ypos}");
        }
        else
        {
            // Debug.LogError("Failed to parse JSON string.");
            Debug.Log("Failed to parse JSON string.");
        }

        // You can perform further processing or update Unity objects here
        // Note: Ensure that any Unity API calls are made on the main thread
    }

    bool IsJson(string jsonString)
    {
        try
        {
            // Attempt to parse the string using JsonUtility
            JsonUtility.FromJson<NetworkDataReceive>(jsonString);
            return true;
        }
        catch (System.Exception)
        {
            // The string is not valid JSON
            return false;
        }
    }

    // Simple JSON parser for demonstration purposes
    private Dictionary<string, object> SimpleJsonParse(string jsonString)
    {
        try
        {
            // Split the JSON string into key-value pairs
            string[] pairs = jsonString.Trim('{', '}').Split(',');

            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            foreach (string pair in pairs)
            {
                string[] keyValue = pair.Split(':');
                string key = keyValue[0].Trim('\"').Trim();
                string value = keyValue[1].Trim();

                jsonData.Add(key, value);
            }

            return jsonData;
        }
        catch (System.Exception)
        {
            return null;
        }
    }

    public void Update()
    {
        //text.text = client_message;
    }

    //private void OnDestroy()
    //{
    //    udpServer.Close();
    //}

}
