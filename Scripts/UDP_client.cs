using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

using UnityEngine.UI;
using System.Text;
using System;

public class UDP_client : MonoBehaviour
{
    private UdpClient udpClient;

    private void Start()
    {
        udpClient = new UdpClient();
    }

    private void OnDestroy()
    {
        udpClient.Close();
    }

    public void ConnectToServer(string serverIpAddress, int serverPort)
    {
        try
        {
            // Send a connection request to the server
            string connectRequestMessage = "CONNECT_REQUEST";
            byte[] connectRequestData = Encoding.UTF8.GetBytes(connectRequestMessage);
            udpClient.Send(connectRequestData, connectRequestData.Length, serverIpAddress, serverPort);

            // Wait for the acknowledgment from the server
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIpAddress), serverPort);
            byte[] acknowledgmentData = udpClient.Receive(ref serverEndPoint);
            string acknowledgmentMessage = Encoding.UTF8.GetString(acknowledgmentData);

            if (acknowledgmentMessage == "CONNECT_ACK")
            {
                Debug.Log("Connected to the server");
                // Handle further communication with the server as needed
            }
            else
            {
                Debug.LogError("Failed to establish a connection");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error connecting to the server: {e.Message}");
        }
    }
}
