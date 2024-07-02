using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using TMPro;

public class UDP_ACK : MonoBehaviour
{

    private static UDP_server instance;
    private UdpClient udpServer;
    private Thread serverThread;
    private bool isServerRunning = false;
    //public int hostingPort = 8080;
    private int hostingPort;
    //public int sendingPort = 8081;
    private int sendingPort;

    public Text text;

    public string messagePack = "Searching For Connection";

    public string sendMessage;
    IPAddress remoteAddress = IPAddress.Parse("192.168.2.15");

    string receivedMessage;

    bool startTransmission = false;
    public Level level;

    private string local_IP;
    private string base_IP;
    private object lockObject = new object(); // Lock object for thread synchronization
    private string host_IP;

    //Client Controls
    private bool user_ready = false;
    private bool user_waiting = false;
    private CancellationTokenSource cancellationTokenSource;


    //Host Controls
    private bool accept_connection = false;
    private bool host_ready = false;

    private string remote_time = "";

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
    }

    private void OnDestroy()
    {
        StopServer();
    }

    //private void StartServer()
    public void StartServer()
    {
        //udpServer = new UdpClient(12345);
        udpServer = new UdpClient(hostingPort);
        serverThread = new Thread(ServerThreadFunction);
        serverThread.Start();

        isServerRunning = true;
    }
    public void StartHostServer()
    {
        hostingPort = 8080;
        sendingPort = 8081;
        //udpServer = new UdpClient(12345);
        udpServer = new UdpClient(hostingPort);
        serverThread = new Thread(ServerThreadFunction);
        serverThread.Start();

        isServerRunning = true;
    }
    public void StartClientServer()
    {
        hostingPort = 8081;
        sendingPort = 8080;
        //udpServer = new UdpClient(12345);
        udpServer = new UdpClient(hostingPort);
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

    public void searchForHost(){
        searchForHostAsync();
    }

     // Function to get local IP address and base IP address without the last octet
//     public async Task searchForHostAsync()
//     {
//           // Get local IP address and base IP address without the last octet
//         (local_IP, base_IP) = GetLocalIPAddress();
//         Debug.Log("Local IP address: " + local_IP);
//         Debug.Log("Base IP address: " + base_IP);



//         string message = "CONNECT_REQUEST";
//         // const int sendingPort = 12345; // Example sending port
//         const int sendingPort = 8085; // Example sending port
//         const int max_timeout = 2;
//         bool connectionFound = false;
//         int counter = 0;
//             // udpServer = new UdpClient(8080);
//             // udpServer.Client.ReceiveTimeout = 5000; // Set a receive timeout (milliseconds)

//         string[] excludedEndings = new string[] { ".1", ".255", "." + local_IP.Split('.')[3] };
//         CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
//         CancellationToken cancellationToken = cancellationTokenSource.Token;


//         try
//         {
//         udpServer = new UdpClient(8080);
//         udpServer.Client.ReceiveTimeout = 5000;

//         // while (!connectionFound && max_timeout > 0)
//         {
//             // for (int i = 90; i < 254 || connectionFound; i++)
//             {
//                 // string currentIP = base_IP + i.ToString();
//                 try
//                 {
//                     int result = await Task.Run(() =>
//                     {
//                         if (cancellationToken.IsCancellationRequested)
//                             return 0;

//                         for (int i = 90; i < 254 || connectionFound; i++)
//                         {

//                             string currentIP = base_IP + i.ToString();
//                             int sendResult = udpServer.Send(Encoding.UTF8.GetBytes(message), message.Length, currentIP, sendingPort);
//                             Debug.Log($"Sent to {currentIP}");

//                             IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
//                             byte[] data = udpServer.Receive(ref remoteEndPoint);
//                             string returnMessage = System.Text.Encoding.UTF8.GetString(data);
//                             Debug.Log($"Received from {remoteEndPoint}: {returnMessage}");

//                             // ProcessReceivedData(remoteEndPoint, data);
//                             // ProcessReceivedDataUI(remoteEndPoint, data);
//                             if (returnMessage.Equals("CONNECT_ACKNOWLEDGED"))
//                             {
//                                 Debug.Log("Connection found at IP: " + currentIP);
//                                 host_IP = currentIP;
//                                 connectionFound = true;
//                                 // break;
//                                 // cancellationTokenSource.Cancel(); // Cancel other tasks
//                             }
//                         }
//                         // int sendResult = udpServer.Send(Encoding.UTF8.GetBytes(message), message.Length, currentIP, sendingPort);
//                         // Debug.Log($"Sent to {currentIP}");

//                         // IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
//                         // byte[] data = udpServer.Receive(ref remoteEndPoint);
//                         // string returnMessage = System.Text.Encoding.UTF8.GetString(data);
//                         // Debug.Log($"Received from {remoteEndPoint}: {returnMessage}");

//                         // // ProcessReceivedData(remoteEndPoint, data);
//                         // // ProcessReceivedDataUI(remoteEndPoint, data);
//                         // if (returnMessage.Equals("CONNECT_ACKNOWLEDGED"))
//                         // {
//                         //     Debug.Log("Connection found at IP: " + currentIP);
//                         //     host_IP = currentIP;
//                         //     connectionFound = true;
//                         //     // break;
//                         //     // cancellationTokenSource.Cancel(); // Cancel other tasks
//                         // }
//                         //
//                         Debug.Log("WAITING....");








//                         return sendResult;
//                     }, cancellationToken);

//                     // Handle result if needed
//                 }
//                 catch (OperationCanceledException)
//                 {
//                     // Task was cancelled due to connection found
//                     Debug.Log("Task cancelled due to connection found.");
//                     break; // Exit the loop
//                 }
//                 catch (SocketException)
//                 {
//                     Debug.Log("Connection attempt timed out for IP: " + currentIP);
//                 }
//                 Thread.Sleep(10); // Optional delay between iterations
//             }

//             // max_timeout--; // Decrement timeout counter
//         }
//     }
//     finally
//     {
//         // Clean up resources
//         udpServer.Close();
//         udpServer = null;
//     }

//     Debug.Log("Search completed. Host IP: " + host_IP);





//         // {
// //         // Continue with your original logic
// //         //while (counter <= max_timeout)
// //         // Continue with your original logic
// //         // while (counter <= max_timeout && !connectionFound)
// //         {
// //             // for (int i = 2; i < 254; i++)
// //             {
// //                 // string currentIP = base_IP + i.ToString();

// //                 // Skip excluded IP addresses
// //                 // bool skip = false;
// //                 // foreach (string ending in excludedEndings)
// //                 // {
// //                 //     if (currentIP.EndsWith(ending))
// //                 //     {
// //                 //         skip = true;
// //                 //         break;
// //                 //     }
// //                 // }

// //                 // if (skip)
// //                 //     continue;



// //                 if(!connectionFound)
// //                 {
// //                     // Execute in a lambda thread
// //                     Task.Run(() =>
// //                     {
// //                         for (int i = 90; i < 254 ; i++)
// //                         {
// //                             string currentIP = base_IP + i.ToString();
// //                             {
// //                                 try
// //                                 {
// //                                     // Send a connect request
// //                                     int result = udpServer.Send(Encoding.UTF8.GetBytes(message), message.Length, currentIP, sendingPort);
// //                                     // int result = udpServer.Send(Encoding.UTF8.GetBytes(message), message.Length, "192.168.4.93", sendingPort);
// //                                     Debug.Log($"Sent to {currentIP}");

// //                                     // Wait for a response (acknowledgement)
// //                                     IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
// //                                     byte[] data = udpServer.Receive(ref remoteEndPoint);

// //                         // ProcessReceivedData(remoteEndPoint, data);
// //                                     // Check message 
// //                                     string returnMessage = System.Text.Encoding.UTF8.GetString(data);
// //                                     Debug.Log($"Received from {remoteEndPoint}: {returnMessage}");

// //                                     // Handle the received message as needed
// //                                     if (returnMessage.Equals("CONNECT_ACKNOWLEDGED"))
// //                                     {
// //                                         Debug.Log("Connection found at IP: " + currentIP);
                                        
// //                                         // Set connectionFound flag to true to stop further iterations
// //                                         // lock (lockObject)
// //                                         {
// //                                             //Save IP address
// //                                             host_IP = currentIP;
// //                 // Debug.Log("FOUND CONNECTION : " + host_IP);

// //                                             connectionFound = true;
// //                 // Debug.Log("FOUND CONNECTION : " + host_IP);
// //                                             // i = 255;
// //                                         }
// //                                         // Perform further actions upon successful connection
// //                                         break;
// //                                     }
// //                                 }
// //                                 catch (SocketException)
// //                                 {
// //                                     // Handle socket exceptions or timeout
// //                                     Debug.Log("Connection attempt timed out for IP: " + currentIP);
// //                                 }
// //                             }
// //                             Thread.Sleep(10);
// //                         }

// //                     });

// //                 Debug.Log("FOUND CONNECTION : " + host_IP);
// //                 }


// //                 {
// //                 // // Send a connect request
// //                 // int result = udpServer.Send(Encoding.UTF8.GetBytes(message), message.Length, currentIP, sendingPort);
// //                 // // Wait for a response (acknowledgement)
// //                 // // byte[] receiveBuffer = new byte[1024];
// //                 // // EndPoint remoteEP = new IPEndPoint(IPAddress.Any, sendingPort);
// //                 //     IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
// //                 //     byte[] data = udpServer.Receive(ref remoteEndPoint);
// //                 //     //Check message 
// //                 //     string returnMessage = System.Text.Encoding.UTF8.GetString(data);
// //                 //     Debug.Log($"Received from {remoteEndPoint}: {message}");
// //                 // try
// //                 // {
// //                 //     int bytesReceived = udpServer.ReceiveFrom(receiveBuffer, ref remoteEP);
// //                 //     string response = Encoding.UTF8.GetString(receiveBuffer, 0, bytesReceived);
// //                 //     // IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
// //                 //     // byte[] data = udpServer.Receive(ref remoteEndPoint);
// //                 //     // Check if the response indicates successful connection
// //                 //     if (response.Equals("CONNECT_ACKNOWLEDGED"))
// //                 //     {
// //                 //         Debug.Log("Connection found at IP: " + currentIP);
// //                 //         // Perform further actions upon successful connection
// //                 //     }
// //                 // }
// //                 // catch (SocketException)
// //                 // {
// //                 //     // Handle socket exceptions or timeout
// //                 //     Debug.Log("Connection attempt timed out for IP: " + currentIP);
// //                 //
// //                     // Perform connection attempt or send message
// //                     // int result = udpServer.Send(Encoding.UTF8.GetBytes(message), message.Length, currentIP, sendingPort);
// //                     // //if (result >= 0)
// //                     // //    break;
// //                     // Thread.Sleep(10);
// //                     // Debug.Log("IP: " + currentIP);
// //                     // Debug.Log("Result: " + result);



// //             // const string baseIP = "192.168.4."; // Example base IP range
// //             // string message = "CONNECT_REQUEST";
// //             // const int max_timeout = 5;
// //             // int coutner = 0;
// //             // //while(coutner <= max_timeout)
// //             // {
// //             //     for(int i = 2; i < 254; i++)
// //             //     {
// //             //         int result = udpServer.Send(Encoding.UTF8.GetBytes(message), message.Length, baseIP + i.ToString(), sendingPort);
// //             //         //if (result >= 0)
// //             //         //    break;
// //             //         Thread.Sleep(10);
// //             //         Debug.Log("IP: " + baseIP + i);
// //             //         Debug.Log("Result: " + result);
// //             //     }
// //             //     coutner++;
// //             // }
// //                 } 
// //             }
// //             counter++;
// //         } 


// //                 Debug.Log("FOUND CONNECTION : " + host_IP);
// //     }

//     }

    public async Task searchForHostAsync()
    {
        // Get local IP address and base IP address without the last octet
        (local_IP, base_IP) = GetLocalIPAddress();
        Debug.Log("Local IP address: " + local_IP);
        Debug.Log("Base IP address: " + base_IP);

        string message = "CONNECT_REQUEST";
        // const int sendingPort = 8085;
        const int sendingPort = 8080;
        // const int max_timeout = 2;
        bool connectionFound = false;
        // int counter = 0;

        string[] excludedEndings = new string[] { ".1", ".255", "." + local_IP.Split('.')[3] };

        try
        {
            udpServer = new UdpClient(8081);
            udpServer.Client.ReceiveTimeout = 5000;

            cancellationTokenSource = new CancellationTokenSource();

            await Task.Run(async () =>
            {
                for (int i = 90; i < 254 || connectionFound; i++)
                {
                    string currentIP = base_IP + i.ToString();

                    // Skip excluded IP addresses
                    bool skip = false;
                    foreach (string ending in excludedEndings)
                    {
                        if (currentIP.EndsWith(ending))
                        {
                            skip = true;
                            break;
                        }
                    }

                    if (skip)
                        continue;

                    try
                    {
                        // Send a connect request
                        int result = await Task.Run(() =>
                        {
                            try
                            {
                                int sendResult = udpServer.Send(Encoding.UTF8.GetBytes(message), message.Length, currentIP, sendingPort);
                                Debug.Log($"Sent to {currentIP}");

                                // Wait for a response (acknowledgement)
                                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                                byte[] data = udpServer.Receive(ref remoteEndPoint);
                                string returnMessage = Encoding.UTF8.GetString(data);
                                Debug.Log($"Received from {remoteEndPoint}: {returnMessage}");

                                // Check if the received message indicates successful connection
                                // if (returnMessage.Equals("CONNECT_ACKNOWLEDGED"))
                                if (returnMessage.Equals("CONNECT_ACK"))
                                {
                                    Debug.Log("Connection found at IP: " + currentIP);
                                    host_IP = currentIP;
                                    connectionFound = true;
                                }

                                return sendResult;
                            }
                            catch (SocketException)
                            {
                                Debug.Log("Connection attempt timed out for IP: " + currentIP);
                                return -1; // Timeout or error
                            }
                        });

                        // If connection found, break the loop
                        if (connectionFound)
                            break;

                        // Optionally add a delay between iterations
                        await Task.Delay(10); // 10 milliseconds delay
                    }
                    catch (OperationCanceledException)
                    {
                        // Task was cancelled due to connection found
                        Debug.Log("Task cancelled due to connection found.");
                        break; // Exit the loop
                    }
                }

                // After completing the search, proceed with the second loop
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    string startMessage = "READY";
                    string readyMessage = "CONNECT_ACK";
                    Debug.Log("Search completed. Host IP: " + host_IP);
                    GameManger.instance.SetServerIP(host_IP);
                    GameManger.instance.Connection_Found = true;
                    udpServer.Client.ReceiveTimeout = 60000 * 5;
                    Thread.Sleep(1000);

                    udpServer.Send(Encoding.UTF8.GetBytes(readyMessage), readyMessage.Length, host_IP, sendingPort);

                    
                    Debug.Log("Waiting for host...");
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(host_IP), 0);
                    byte[] data = udpServer.Receive(ref remoteEndPoint);

                    string receivedMessage = Encoding.UTF8.GetString(data);
                    Debug.Log($"Received from {remoteEndPoint}: {receivedMessage}");

                    // Check if the received message indicates host readiness
                    if (receivedMessage.Equals("READY"))
                    {
                        // udpServer.Send(Encoding.UTF8.GetBytes(startMessage), startMessage.Length, host_IP, sendingPort);
                        
                        GameManger.instance.Connection_Accepted = true;
                        if (user_ready)
                        {
                            udpServer.Send(Encoding.UTF8.GetBytes(startMessage), startMessage.Length, host_IP, sendingPort);
                            user_waiting = true;
                            Debug.Log("Host is READY. Closing the thread.");
                            Thread.Sleep(1000);
                            startTransmission = true;
                            break; // Exit the loop to end the task
                        }
                        // Debug.Log("Host is READY. Closing the thread.");
                        // break; // Exit the loop to end the task
                    }
                    //Dont need this
                    else{
                        remote_time = receivedMessage;
                    }

                }
            });
            Debug.Log("Thread Closed  Closed");
        }
        finally
        {
            // Clean up resources
            if (udpServer != null)
            {
                udpServer.Close();
                udpServer = null;

                Debug.Log("Sock Closed");
            }
        }

        Debug.Log("Thread Closed Sock Closed");
        // startTransmission = true;
    }

    public void SetUserReady(){
        user_ready = true;
    }



    private (string, string) GetLocalIPAddress()
    {
        string localIP = "";
        string baseIP = "";

        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            // Consider only operational network interfaces
            if (ni.OperationalStatus == OperationalStatus.Up)
            {
                foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                {
                    // Check for IPv4 addresses and ignore loopback and link-local addresses
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip.Address) && !ip.Address.ToString().StartsWith("169.254"))
                    {
                        localIP = ip.Address.ToString();
                        
                        // Remove the last octet to get base IP address
                        string[] octets = localIP.Split('.');
                        if (octets.Length == 4)
                        {
                            baseIP = octets[0] + "." + octets[1] + "." + octets[2] + ".";
                        }

                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(localIP) && !string.IsNullOrEmpty(baseIP))
                break;
        }

        return (localIP, baseIP);
    }

    public void transmitData(string nd)
    {
        udpServer.Send(Encoding.UTF8.GetBytes(nd), nd.Length, GameManger.instance.GetServerIP(), sendingPort);
        // udpServer.Send(Encoding.UTF8.GetBytes(nd), nd.Length, "192.168.2.18", sendingPort);
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

                //IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpServer.Receive(ref remoteEndPoint);

                //Check message 
                string message = System.Text.Encoding.UTF8.GetString(data);
                Debug.Log($"Received from {remoteEndPoint}: {message}");

                receivedMessage = message;

                // ProcessReceivedData(remoteEndPoint, data);
                ProcessReceivedDataUI(remoteEndPoint, data);
                //ProcessJSONData(remoteEndPoint, data);

                //Thread.Sleep(1000);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in server thread: {e.Message}");
        }
    }
    private void ProcessReceivedData(IPEndPoint remoteEndPoint, byte[] data)
    {
        string message = System.Text.Encoding.UTF8.GetString(data);
        Debug.Log($"Received from {remoteEndPoint}: {message}");


        if (message == "CONNECT_REQUEST")
        {
            string acknowledgmentMessage = "CONNECT_ACK";
            receivedMessage = acknowledgmentMessage;
            sendMessage = acknowledgmentMessage;
            Thread.Sleep(1000);
            // GameManger.instance.SetServerIP(remoteAddress.ToString());
            SendAcknowledgment(remoteEndPoint, acknowledgmentMessage);
        }
        else if (message == "CONNECT_ACK")
        {
            Debug.Log($"Connection established with {remoteEndPoint}");
            receivedMessage = message;
            sendMessage = "READY";
            Thread.Sleep(1000);
            //Save Ip 
            //t GameManger.instance.SetServerIP(remoteAddress.ToString());
            SendAcknowledgment(remoteEndPoint, "READY");
            // Handle further communication with the client as needed
        }
        else if (message == "READY")
        {
            if(startTransmission)
            {
                receivedMessage = "Begin Transmission";

                //ProcessJSONData(remoteEndPoint, data);
            }
            else
            {
                sendMessage = "CONNECT_REQUEST";
                Thread.Sleep(1000);
                SendAcknowledgment(remoteEndPoint, "READY");
                startTransmission = true;
            }
                //transmitData("CONNECTION_REQUEST");
        }
    }


      private void ProcessReceivedDataUI(IPEndPoint remoteEndPoint, byte[] data)
    {
        string message = System.Text.Encoding.UTF8.GetString(data);
        Debug.Log($"Received from {remoteEndPoint}: {message}");


        if (message == "CONNECT_REQUEST")
        {
            string acknowledgmentMessage = "CONNECT_ACK";
            receivedMessage = acknowledgmentMessage;
            sendMessage = acknowledgmentMessage;
            Thread.Sleep(1000);
            // GameManger.instance.SetServerIP(remoteAddress.ToString());
            
            //Create a button to accept connection
                    //|\\
            GameManger.instance.Connection_Found = true;
            GameManger.instance.SetServerIP(remoteEndPoint.Address.ToString());
            // if(accept_connection)
                SendAcknowledgment(remoteEndPoint, acknowledgmentMessage);
        }
        else if (message == "CONNECT_ACK")
        {
            Debug.Log($"Connection established with {remoteEndPoint}");
            receivedMessage = message;
            sendMessage = "READY";
            Thread.Sleep(1000);
            //Save Ip 
            //t GameManger.instance.SetServerIP(remoteAddress.ToString());
            //Create a button to accept connection
                    //|\\
            GameManger.instance.Connection_Accepted = true;
            // if(host_ready)
                SendAcknowledgment(remoteEndPoint, "READY");
            // Handle further communication with the client as needed
        }
        else if (message == "READY")
        {
            if(startTransmission)
            {
                receivedMessage = "Begin Transmission";

                //ProcessJSONData(remoteEndPoint, data);
            }
            else
            {
                sendMessage = "CONNECT_REQUEST";
                SendAcknowledgment(remoteEndPoint, "READY");
                Thread.Sleep(1000);
                startTransmission = true;
            }
                //transmitData("CONNECTION_REQUEST");
        }
    }


    private void SendAcknowledgment(IPEndPoint clientEndPoint, string acknowledgmentMessage)
    {
        try
        {
            // Convert acknowledgment message to bytes
            byte[] acknowledgmentData = Encoding.UTF8.GetBytes(acknowledgmentMessage);


            // Send acknowledgment back to the client
            udpServer.Send(acknowledgmentData, acknowledgmentData.Length, clientEndPoint);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error sending acknowledgment: {e.Message}");
        }
    }

        public void Update()
    {

        if (startTransmission)
        {
            StartCoroutine(StartGameAfterDelay(3f)); // Start game after 3 seconds delay
            startTransmission = false; // Set startTransmission to false so this only runs once
        }

    }


     IEnumerator StartGameAfterDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);

        // This code runs after the delay
        Debug.Log("Starting game after 3 seconds delay");

        // Call your method to initiate the game or whatever action you need
        level.Online_Player();
    }

}
