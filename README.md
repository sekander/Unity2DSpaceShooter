# Unity2DSpaceShooter


This UDP_ACK class in Unity implements a client-server model using UDP (User Datagram Protocol) for network communication. Let's break down its key features and functionality:
Key Features and Functionality

    UDP Server Initialization and Management
        StartServer(), StartHostServer(), StartClientServer(): These methods initialize the UDP server on different ports based on whether it's acting as a host or client. They start a new thread (serverThread) where the server listens for incoming messages.
        StopServer(): Stops the UDP server by closing the UDP client and joining the server thread.

    Client and Host Modes
        StartHostServer(): Sets up the server to host a game, defining specific hosting and sending ports.
        StartClientServer(): Configures the server to act as a client, using different ports for hosting and sending.

    Searching for Host
        searchForHostAsync(): Asynchronously searches for a host using UDP broadcast messages (CONNECT_REQUEST). It iterates through a range of IP addresses derived from the local network to find a host that responds with an acknowledgment (CONNECT_ACK). Once found, it establishes the connection and sets up further communication.

    Network Interaction
        transmitData(string nd): Sends UDP messages (nd) to the host IP address (GameManger.instance.GetServerIP()) on a specified sending port (sendingPort).

    Thread Management
        ServerThreadFunction(): The main server thread function that continuously listens for incoming UDP messages (CONNECT_REQUEST, CONNECT_ACK, READY) and processes them accordingly. It handles communication with clients and hosts.

    UI and Game Interaction
        ProcessReceivedDataUI(IPEndPoint remoteEndPoint, byte[] data): Processes received data specifically for UI interactions, distinguishing between different message types (CONNECT_REQUEST, CONNECT_ACK, READY) and updating UI states accordingly.
        Update() and StartGameAfterDelay(float delayInSeconds): Manages game flow after establishing connections. For instance, it starts the game after a specified delay once all necessary connections are confirmed (READY).

    Network Configuration
        GetLocalIPAddress(): Retrieves the local IP address and base IP address (without the last octet), crucial for determining the network range to search for hosts.

    Error Handling
        The code includes error handling for socket exceptions (SocketException) and timeouts to manage network-related issues gracefully.

Conclusion

This UDP_ACK class forms the backbone of a UDP-based client-server networking system in Unity. It allows for dynamic network setup, asynchronous host discovery, and seamless communication between multiple clients and a host. The usage of threads (serverThread) ensures non-blocking network operations, crucial for real-time applications like games. The class encapsulates both server and client functionalities, making it versatile for different network roles within a game environment.


Your UDP_server class in Unity is designed to act as a UDP server that listens for incoming messages and processes them accordingly. Here’s a breakdown of your code and some recommendations:
Key Components and Functionality:

    Singleton Implementation (Instance Property):
        Ensures there's only one instance of UDP_server in the scene.
        Uses DontDestroyOnLoad to persist across scene changes.

    UDP Server Initialization (StartServer):
        Starts a UDP server on port 8080.
        Creates a separate thread (serverThread) to handle incoming messages asynchronously.

    UDP Server Shutdown (OnDestroy and StopServer):
        Ensures the server thread is properly terminated and resources are released when the object is destroyed.

    Message Transmission (transmitData):
        Sends messages (either NetworkDataSend object serialized to JSON or raw string) to a specified IP address (GameManger.instance.GetServerIP()) and port (8080).

    Server Thread (ServerThreadFunction):
        Continuously listens for incoming UDP messages.
        Upon receiving a message, processes it based on content ("START" or JSON data).
        Parses JSON data into a Dictionary<string, object> and updates NetworkDataReceive.Instance with parsed values.
        Logs received data for debugging purposes.

    JSON Handling (SimpleJsonParse and ProcessJSONData):
        SimpleJsonParse method splits the received JSON string into key-value pairs and returns a Dictionary<string, object>.
        ProcessJSONData method extracts specific fields (player_xpos, player_ypos, player_fired, time) from the JSON and updates the NetworkDataReceive.Instance.


Your UDP_client class effectively establishes a UDP connection to a server and verifies the connection by waiting for an acknowledgment message. Ensure thorough testing, especially in varying network conditions, and implement the suggested improvements for enhanced robustness and reliability. This setup should serve as a solid foundation for UDP communication in Unity.

Your GameSession class in Unity manages the game session logic, including scoring, game mode management, and handling multiplayer functionality based on whether the game is played online or offline. Here's a detailed breakdown and some suggestions for improvement:
Key Components and Functionality:

    Enum for Play Mode (PLAY_MODE):
        Defines two modes: SINGLE_PLAYER and MULTI_PLAYER.
        Used to determine the current mode of gameplay.

    Score Tracking:
        Tracks scores for two players (p1_score and p2_score).
        Provides methods to retrieve scores (Get_P1_Score, Get_P2_Score) and update scores (AddTo_P1_Score, AddTo_P2_Score).

    Game State Management:
        Manages the current play mode (state) and allows setting and getting of this mode (GetPlayMode, SetPlayMode).

    Initialization (Awake Method):
        Ensures only one instance of GameSession exists using a singleton pattern (SetupSingleton).
        Disables EnemySpawner if the game mode is online and initializes a fixed random seed for consistency (Random.InitState).

    Update Method:
        Handles game logic updates per frame, especially when in online mode (GameManger.PLAY_MODE.ONLINE).
        Tracks gametime and synchronizes game start (UDP_server.Instance.startTimeSync) based on network data received (NetworkDataReceive.Instance.Player_Time).
        Transmits game time data when synchronization conditions are met (UDP_server.Instance.transmitData).

    Reset Methods:
        ResetScore: Resets scores of both players.
        ResetGame: Destroys the GameSession object, typically used when resetting the entire game state.

The Player class represents a player-controlled character in a 2D game. It handles player movement, shooting mechanics, power-ups, health management, and interactions with other game objects. It includes multiplayer functionality and networking capabilities using Unity's UDP server.
Key Features and Components

    Player Movement and Boundaries
        Handles player movement based on user input (WASD or touch controls).
        Constrains player movement within the game boundaries defined by the camera.

    Shooting Mechanism
        Supports multiple firing modes (Single, Double, Triple, Wide, Fast, Helix) controlled by keyboard inputs or network data in multiplayer mode.
        Instantiates different types of projectiles (laserPrefab) based on the selected firing mode.

    Power-Ups and Upgrades
        Implements various power-up functionalities (Sheild, SpeedUP, SpeedDOWN, Life) that modify player behavior and attributes.
        Power-ups are activated using specific keyboard inputs or triggered by network data in multiplayer mode.

    Health and Damage
        Manages player health (health) and implements damage-taking functionality (OnTriggerEnter2D).
        Implements invincibility period (isInvincible) after taking damage to provide temporary protection.

    Multiplayer and Networking
        Supports multiplayer functionality (player_1, player_2, remote_player) with different control schemes and network data synchronization.
        Uses UDP server (UDP_server) to handle network communication, transmitting and receiving player actions and game data.

    Visual Effects and UI
        Provides visual feedback for power-up activation, invincibility (ChangeOpacity), and damage (SpawnGhostEffect).
        Updates player UI elements (playerLives) based on current health and lives.

    Audio and Game Flow
        Plays audio clips (deathSFX, shootSFX) for player actions (shooting, death).
        Handles player death (Die), resets player position, and manages game flow based on single-player or multiplayer mode.

    Coroutine Management
        Utilizes coroutines (SpawnGhostEffect, StartLoopCoroutine, StopLoopCoroutine) for managing visual effects and continuous actions (e.g., ghost effects, continuous firing).

    Initialization and Setup
        Initializes player attributes (moveSpeed, health, shootDelay) and sets up initial game state (boundaries, power-ups, etc.) in Start and Awake methods.

Conclusion

Overall, the Player class encapsulates the core functionalities of a player character in a 2D game, including movement, shooting, power-ups, health management, and multiplayer capabilities with networking support. It leverages Unity's component-based architecture and scripting capabilities to create an interactive and dynamic player experience.


