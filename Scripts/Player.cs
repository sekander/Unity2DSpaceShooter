using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour
{
    //public SimpleTouchController leftController;


    //Allow private variables to be sceen in the unity editor
    [Header("Player")]
    [SerializeField] private float moveSpeed = 1.0f;
    // [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float padding = 1.0f;
    [SerializeField] private int health = 200;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSFXVolume = 0.7f;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] [Range(0, 1)] float shootSFXVolume = 0.7f;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10.0f;

    Coroutine fireingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    public SimpleTouchController leftController;

    public bool player_1;
    public bool player_2;
    public bool remote_player;
    float gametime = 0;
    float p1_captureFire;
    float p2_captureFire;
    float shootDelay = 0.5f;

    public int life = 3;
    private Vector2 startPosition;

    private bool isInvincible = false;
    private bool sheildOn = false;
    private int sheildCapacity = 1000;
    private float invincibilityTimer = 3f;

    private SpriteRenderer playerRenderer;
    private Transform myTransform;
    private Collider2D myCollider; // Adjust the type based on your collider type

    public Image[] playerLives;

    


    public enum FiringMode
    {
        Single,
        Double,
        Triple,
        Wide,
        Fast,
        Helix,
    
    }

    private FiringMode currentFiringMode = FiringMode.Single; // Default firing mode
    // public FiringMode currentFiringMode = FiringMode.Single; // Default firing mode

    public enum ShipMode
    {
        Normal,
        Sheild,
        SpeedUP,
        SpeedDOWN,
        Life
    }
    private ShipMode shipMode = ShipMode.Sheild; // Default firing mode


    public GameObject sheildPrefab;
    GameObject sheild;

    private bool slowMode = false;

    // public Material colourControl;
    private Coroutine loopCoroutine;


     // Reference to the CameraShake script attached to the main camera

    void Awake()
    {
        //_rigidbody = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        gametime = 0.0f;
        GameBoundary();
        // Get the SpriteRenderer component attached to the player GameObject
        playerRenderer = GetComponent<SpriteRenderer>();
        // Get the Collider2D component attached to this GameObject
        myCollider = GetComponent<Collider2D>();


        startPosition = transform.position; 
        // Get the Transform component of this GameObject
        myTransform = transform;

        // sheild = Instantiate(sheildPrefab, transform.position, Quaternion.identity);
        // sheild.SetActive(false);

//        StartCoroutine(printWait());
        //Change this to Lan
        if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.ONLINE)
        {
            Debug.Log("ONLINE PLAY ENABLED");

            

            //Data to populate player position and send to connected player
            //networkData = new NetworkData();
            //networkData = gameObject.AddComponent<NetworkData>();
            //UDP_server.Instance.StartUDPServer(NetworkData.Instance);
            //UDP_server.Instance.StartUDPServer();
            //UDP_server.Instance.transmitData(NetworkData.Instance);
            UDP_server.Instance.StartServer();
            //UDP_server.Instance.transmitData("START");
        }

        // colourControl.SetColor("_Color", Color.red);  // Change the color to red 
        // playerRenderer.material = colourControl;
          // Find the CameraShake script attached to the main camera
    }



    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        gametime += delta;

        if(slowMode){
            float frequency = 1f; // Frequency of the sine wave
            float amplitude = 0.5f; // Amplitude of the sine wave
             // Calculate normalized time-dependent sine wave value
            float t = Mathf.PingPong(Time.time * frequency, 1f); // PingPong to keep within [0, 1]
            float sineValue = Mathf.Sin(2 * Mathf.PI * t) * amplitude + amplitude;



              // Get current color
            Color currentColor = playerRenderer.color;

            // Modify green component (setting it to 0.5 for example)
            currentColor.g = sineValue; // Set green component to 0.5 (between 0 and 1)

            // Assign the modified color back to the SpriteRenderer
            playerRenderer.color = currentColor;
        }

        // Check if the player is currently invincible
        if (isInvincible)
        {

            ChangeOpacity(Mathf.Sin(Time.time * 25f) * 1.0f);

            // Update the invincibility timer
            invincibilityTimer -= Time.deltaTime;

            // Check if the invincibility period has ended
            if (invincibilityTimer <= 0)
            {
                // Disable invincibility
                isInvincible = false;
                ChangeOpacity(1.0f);
                invincibilityTimer = 3.0f;

                // Enable collision with other objects on the "Default" layer
                //gameObject.layer = LayerMask.NameToLayer("Default");

                // Additional logic, if needed, when invincibility ends
            }
        }



        //Check Game Type
        //Debug.Log("Play Mode: " + GameManger.instance.GetPLAY_MODE());
        //Debug.Log(NetworkDataSend.Instance.ToJsonString());


        //Player 1 Controls 
        Move();
        if (player_1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // if(GameManger.instance.Get_P1_FiringMode() >= 1 && GameManger.instance.Get_P1_FiringMode() <= 5)
                currentFiringMode = (FiringMode)GameManger.instance.Get_P1_FiringMode();

                if(gametime - p1_captureFire > shootDelay)
                {
                    Fire();
                    NetworkDataSend.Instance.Player_fired = true;
                    p1_captureFire = gametime;
                }
                //else
                //    NetworkDataSend.Instance.Player_fired = false;
            }else if (Input.GetKeyUp(KeyCode.Space))
            {
                NetworkDataSend.Instance.Player_fired = false;
            }
        }
        else if (player_2)
        {
            // if (Input.GetKey(KeyCode.RightShift))
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                // if(GameManger.instance.Get_P1_FiringMode() >= 1 && GameManger.instance.Get_P1_FiringMode() <= 5)
                currentFiringMode = (FiringMode)GameManger.instance.Get_P2_FiringMode();
                if(gametime - p2_captureFire > shootDelay)
                {
                    Fire();
                    p2_captureFire = gametime;
                }
            }

            //Get network data
            //For Online only
        }
        if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.ONLINE)
        {
            if(UDP_server.Instance.beginGame)
                {UDP_server.Instance.transmitData("START");
            UDP_server.Instance.transmitData(NetworkDataSend.Instance);
            }else{
                // if (gametime <= 0.5f)
                UDP_server.Instance.transmitData("START");
            }

            if(remote_player)
            {
                if(NetworkDataReceive.Instance.Player_fired)
                {
                    Debug.Log("BOOM BOOM");
                    // int playerUpgrade = NetworkDataReceive.Instance.Player_Upgrade;
                    // if(playerUpgrade <= 5)
                    //     currentFiringMode = (FiringMode)playerUpgrade;
                        // currentFiringMode = (FiringMode)playerUpgrade;
                    currentFiringMode = (FiringMode)GameManger.instance.Get_P2_FiringMode();
                    if(gametime - p2_captureFire > shootDelay)
                    {
                        Fire();
                        p2_captureFire = gametime;
                    }
                }
            }
        }

       //Check if player object is not null
       if(!this.isActiveAndEnabled)
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);

    }

    private void Move(){
            //Use Time.deltatime to make gaeme frame rate independent

            // Get the horizontal and vertical input
            if(player_1)
            {
                //Ship upgrades
                   // Example: Scale the sprite to double its original size when space key is pressed
                if(GameManger.instance.Get_P1_ShipMode() == 1)
                {
                    Debug.Log("SHEILD ACTIVATED");
                    // sheildCapacity = 1000;
                    if(!sheildOn)
                        sheild = Instantiate(sheildPrefab, transform.position, Quaternion.identity);
                    else{
                        Debug.Log("Regen SHEILD");
                    }

                    sheild.SetActive(true);
                    sheildOn = true;
                    GameManger.instance.Set_P1_ShipUpgrade(0);
                }
                // if (Input.GetKeyDown(KeyCode.J))
                if(GameManger.instance.Get_P1_ShipMode() == 2)
                {
                    Debug.Log("SPEED ACTIVATED");
                    if(slowMode)slowMode = false;
                    moveSpeed = 2.0f;
                    StartLoopCoroutine();
                    GameManger.instance.Set_P1_ShipUpgrade(0);
                }
                // if (Input.GetKeyDown(KeyCode.K))
                if(GameManger.instance.Get_P1_ShipMode() == 3)
                {
                    Debug.Log("SPEED DEACTIVATED");
                    moveSpeed = 0.5f;
                    slowMode = true;
                    StopLoopCoroutine();
                    GameManger.instance.Set_P1_ShipUpgrade(0);
                }
                if(GameManger.instance.Get_P1_ShipMode() == 4)
                {
                    if (life < 4)
                    {
                        Debug.Log("LIFE UP");
                        GameManger.instance.SetTotalLife(GameManger.instance.GetTotalLife() + 1);
                        playerLives[life].enabled = true;
                        life += 1;
                        // playerLives[life].enabled = true;
                        GameManger.instance.Set_P1_ShipUpgrade(0);
                    }
                }




                if(sheild)
                {
                    sheild.transform.position = transform.position;
                }
                else
                    sheildOn = false;





                float horizontalInput = Input.GetKey(KeyCode.D) ? (0.1f * moveSpeed) : (Input.GetKey(KeyCode.A) ? (-0.1f * moveSpeed) : 0f);
                float verticalInput = Input.GetKey(KeyCode.W) ? (0.1f * moveSpeed) : (Input.GetKey(KeyCode.S) ? (-0.1f * moveSpeed) : 0f);

                float touchX = leftController.GetTouchPosition.x * Time.deltaTime * moveSpeed;    
                float touchY = leftController.GetTouchPosition.y * Time.deltaTime * moveSpeed;    
                
                float xpos = Mathf.Clamp( transform.position.x + horizontalInput + touchX , xMin, xMax);
                float ypos = Mathf.Clamp( transform.position.y + verticalInput + touchY , yMin, yMax);

                if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.ONLINE)
                {
                //NetworkData.Instance.Player_xpos = xpos; 
                //NetworkData.Instance.Player_ypos= ypos; 
                    
                    NetworkDataSend.Instance.Player_Time = gametime;
                    NetworkDataSend.Instance.Player_xpos = (float)System.Math.Round(xpos, 2); 
                    NetworkDataSend.Instance.Player_ypos = (float)System.Math.Round(ypos, 2); 
                   // .Player_ypos = ypos;
                    //UDP_server.Instance.transmitData(NetworkData.Instance);
                    //UDP_server.Instance.transmitData(NetworkDataSend.Instance);
                }

                transform.position = new Vector2(xpos, ypos);
            }
            else if(player_2)
            {
                if(GameManger.instance.Get_P2_ShipMode() == 1)
                {
                    Debug.Log("SHEILD ACTIVATED");
                    // // sheildCapacity = 1000;
                    // sheild = Instantiate(sheildPrefab, transform.position, Quaternion.identity);
                    // sheild.SetActive(true);

                    if(!sheildOn)
                        sheild = Instantiate(sheildPrefab, transform.position, Quaternion.identity);
                    else{
                        Debug.Log("Regen SHEILD");
                    }
                    sheildOn = true;
                    GameManger.instance.Set_P2_ShipUpgrade(0);
                }
                // if (Input.GetKeyDown(KeyCode.J))
                if(GameManger.instance.Get_P2_ShipMode() == 2)
                {
                    Debug.Log("SPEED ACTIVATED");
                    if(slowMode)slowMode = false;
                    moveSpeed = 2.0f;
                    StartLoopCoroutine();
                    GameManger.instance.Set_P2_ShipUpgrade(0);
                }
                // if (Input.GetKeyDown(KeyCode.K))
                if(GameManger.instance.Get_P2_ShipMode() == 3)
                {
                    Debug.Log("SPEED DEACTIVATED");
                    moveSpeed = 0.5f;
                    slowMode = true;
                    StopLoopCoroutine();
                    GameManger.instance.Set_P2_ShipUpgrade(0);
                }
                if(GameManger.instance.Get_P2_ShipMode() == 4)
                {
                    if (life < 4)
                    {
                        Debug.Log("LIFE UP");
                        // GameManger.instance.SetTotalLife(GameManger.instance.GetTotalLife() + 1);
                        GameManger.instance.SetTotalLife(GameManger.instance.GetTotalLife() + 1);
                        playerLives[life].enabled = true;
                        life += 1;
                        GameManger.instance.Set_P2_ShipUpgrade(0);
                    }
                }
                if(sheild)
                {
                    sheild.transform.position = transform.position;
                }
                else
                    sheildOn = false;

                float horizontalInput = Input.GetKey(KeyCode.RightArrow) ? 0.1f * moveSpeed: (Input.GetKey(KeyCode.LeftArrow) ? -0.1f * moveSpeed: 0f);
                float verticalInput = Input.GetKey(KeyCode.UpArrow) ? 0.1f * moveSpeed: (Input.GetKey(KeyCode.DownArrow) ? -0.1f * moveSpeed: 0f);

                float touchX = leftController.GetTouchPosition.x * Time.deltaTime * moveSpeed;    
                float touchY = leftController.GetTouchPosition.y * Time.deltaTime * moveSpeed;    
                
                float xpos = Mathf.Clamp( transform.position.x + horizontalInput + touchX , xMin, xMax);
                float ypos = Mathf.Clamp( transform.position.y + verticalInput + touchY , yMin, yMax);

                transform.position = new Vector2(xpos, ypos);

            }
            else if (remote_player)
            {
            //Create new script for remote player
                //transform.position = new Vector2( UDP_server.Instance.receivedData.Player_xpos, 
                //                                  UDP_server.Instance.receivedData.Player_ypos );
                // int playerUpgrade = NetworkDataReceive.Instance.Player_Upgrade;
                // if(playerUpgrade >= 5)
                // {
                //     playerUpgrade -= 5;
                //     if(playerUpgrade == 1)
                //     {
                //         Debug.Log("SHEILD ACTIVATED");
                //         // // sheildCapacity = 1000;
                //         // sheild = Instantiate(sheildPrefab, transform.position, Quaternion.identity);
                //         // sheild.SetActive(true);

                //         if(!sheildOn)
                //             sheild = Instantiate(sheildPrefab, transform.position, Quaternion.identity);
                //         else{
                //             Debug.Log("Regen SHEILD");
                //         }
                //         sheildOn = true;
                //         GameManger.instance.Set_P2_ShipUpgrade(0);
                //     }
                //     // if (Input.GetKeyDown(KeyCode.J))
                //     if(GameManger.instance.Get_P2_ShipMode() == 2)
                //     {
                //         Debug.Log("SPEED ACTIVATED");
                //         if(slowMode)slowMode = false;
                //         moveSpeed = 2.0f;
                //         StartLoopCoroutine();
                //         GameManger.instance.Set_P2_ShipUpgrade(0);
                //     }
                //     // if (Input.GetKeyDown(KeyCode.K))
                //     if(GameManger.instance.Get_P2_ShipMode() == 3)
                //     {
                //         Debug.Log("SPEED DEACTIVATED");
                //         moveSpeed = 0.5f;
                //         slowMode = true;
                //         StopLoopCoroutine();
                //         GameManger.instance.Set_P2_ShipUpgrade(0);
                //     }
                //     if(GameManger.instance.Get_P2_ShipMode() == 4)
                //     {
                //         if (life < 4)
                //         {
                //             Debug.Log("LIFE UP");
                //             // GameManger.instance.SetTotalLife(GameManger.instance.GetTotalLife() + 1);
                //             GameManger.instance.SetTotalLife(GameManger.instance.GetTotalLife() + 1);
                //             playerLives[life].enabled = true;
                //             life += 1;
                //             GameManger.instance.Set_P2_ShipUpgrade(0);
                //         }
                //     }
                // }
                if(GameManger.instance.Get_P2_ShipMode() == 1)
                {
                    Debug.Log("SHEILD ACTIVATED");
                    // // sheildCapacity = 1000;
                    // sheild = Instantiate(sheildPrefab, transform.position, Quaternion.identity);
                    // sheild.SetActive(true);

                    if(!sheildOn)
                        sheild = Instantiate(sheildPrefab, transform.position, Quaternion.identity);
                    else{
                        Debug.Log("Regen SHEILD");
                    }
                    sheildOn = true;
                    GameManger.instance.Set_P2_ShipUpgrade(0);
                }
                // if (Input.GetKeyDown(KeyCode.J))
                if(GameManger.instance.Get_P2_ShipMode() == 2)
                {
                    Debug.Log("SPEED ACTIVATED");
                    if(slowMode)slowMode = false;
                    moveSpeed = 2.0f;
                    StartLoopCoroutine();
                    GameManger.instance.Set_P2_ShipUpgrade(0);
                }
                // if (Input.GetKeyDown(KeyCode.K))
                if(GameManger.instance.Get_P2_ShipMode() == 3)
                {
                    Debug.Log("SPEED DEACTIVATED");
                    moveSpeed = 0.5f;
                    slowMode = true;
                    StopLoopCoroutine();
                    GameManger.instance.Set_P2_ShipUpgrade(0);
                }
                if(GameManger.instance.Get_P2_ShipMode() == 4)
                {
                    if (life < 4)
                    {
                        Debug.Log("LIFE UP");
                        // GameManger.instance.SetTotalLife(GameManger.instance.GetTotalLife() + 1);
                        GameManger.instance.SetTotalLife(GameManger.instance.GetTotalLife() + 1);
                        playerLives[life].enabled = true;
                        life += 1;
                        GameManger.instance.Set_P2_ShipUpgrade(0);
                    }
                }
                
                if(sheild)
                {
                    sheild.transform.position = transform.position;
                }
                else
                    sheildOn = false;


                double offset = gametime - NetworkDataReceive.Instance.Player_Time;
                Debug.Log("Time Offset: " + offset);



                transform.position = new Vector2( NetworkDataReceive.Instance.Player_xpos, 
                                                  NetworkDataReceive.Instance.Player_ypos );
            }
    }
    
    private void GameBoundary(){
        Camera gameCamera = Camera.main;
        //World space value
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0.1f, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).y - padding;

    }

    void ChangeOpacity(float opacity)
    {
        // Make sure the SpriteRenderer is not null
        if (playerRenderer != null)
        {
            // Get the current color
            Color currentColor = playerRenderer.color;

            // Set the new alpha value (opacity)
            currentColor.a = opacity;

            // Apply the new color to the material of the SpriteRenderer
            playerRenderer.color = currentColor;
        }
    }
    IEnumerator SpawnGhostEffect()
    {
        while (true)
        {
            createGhostEffect(); // Invoke the function to create ghost effect

            yield return new WaitForSeconds(0.01f); // Wait for half a second before spawning the next ghost effect
        }
    }
    void createGhostEffect()
    {
         // Instantiate a new GameObject with the same SpriteRenderer component as the player
        GameObject ghostObject = new GameObject("Ghost");  // Create an empty GameObject
        SpriteRenderer ghostRenderer = ghostObject.AddComponent<SpriteRenderer>();  // Add SpriteRenderer component

        // Assign the sprite from the playerRenderer to the ghostRenderer
        ghostRenderer.sprite = playerRenderer.sprite;

        // Set position and rotation to match the player's current transform
        ghostObject.transform.position = transform.position;
        ghostObject.transform.rotation = Quaternion.identity;
        

        // Adjust transparency of the ghost sprite
        Color ghostColor = ghostRenderer.color;
        ghostColor.a = 0.1f;  // Adjust alpha (transparency) as desired
        ghostRenderer.color = ghostColor;

        // Schedule the ghost object for destruction after 1 second
        Destroy(ghostObject, 0.5f);
    }

    public void Fire(){

        float lastSpawnTime; // Track the time of the last laser instantiation
        float helixDelay = 0.0f;
        lastSpawnTime = Time.time;

        switch (currentFiringMode)
        {
            case FiringMode.Single:
                InstantiateLaser(transform.position);
                break;
            case FiringMode.Double:
                // Debug.Log("DOUBLE SHOT");
                InstantiateLaser(transform.position - Vector3.right * 0.5f); // Offset to the left
                InstantiateLaser(transform.position + Vector3.right * 0.5f); // Offset to the right
                break;
            case FiringMode.Triple:
                // Debug.Log("TRIPE SHOT");
                InstantiateLaser(transform.position - Vector3.right * 1.0f); // Offset to the left
                InstantiateLaser(transform.position);
                InstantiateLaser(transform.position + Vector3.right * 1.0f); // Offset to the right
                break;
            case FiringMode.Wide:
                // Debug.Log("WIDE SHOT");
                InstantiateAngledLaser(transform.position - Vector3.right * 1.0f, 115); // Offset to the left
                InstantiateLaser(transform.position);
                InstantiateAngledLaser(transform.position + Vector3.right * 1.0f, 65); // Offset to the left
                break;
            case FiringMode.Helix:
                // Debug.Log("HELIX SHOT");
                for(int i = 0; i < 3; i++)
                {
                    if (Time.time - lastSpawnTime >= helixDelay)
                    {
                        InstantiateHelixLaser(transform.position - Vector3.right * 0.5f, false);
                        InstantiateHelixLaser(transform.position + Vector3.right * 0.5f, true);
                        helixDelay = 0.01f;
                    }
                }
                break;
            case FiringMode.Fast:
                InstantiateLaser(transform.position);
                // Debug.Log("FAST SHOT");
                shootDelay = 0.25f;
                break;
            default:
                Debug.LogError("Unsupported firing mode");
                break;
        }
        AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSFXVolume);
    }

    // IEnumerator fireContinously(){
    //     while(true){
    //         GameObject laser = Instantiate(laserPrefab, new Vector2(transform.position.x, transform.position.y + 1.0f), Quaternion.identity) as GameObject;
    //         laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
    //         AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSFXVolume);
            
    //         if(player_1)
    //             yield return new WaitForSeconds(0.1f);
    //         else if(player_2)
    //             yield return new WaitForSeconds(projectileRate);

    //         //if(laser.transform.position.y > Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y)

    //     }

    // }

    //Example of coroutine
    // IEnumerator printWait(){
    //     Debug.Log("First message sent, boos");
    //     yield return new WaitForSeconds(3.0f);
    //     Debug.Log("Second message sent, boos");
    // }

    private void InstantiateLaser(Vector3 position)
    {
        GameObject laserObject = Instantiate(laserPrefab, new Vector2(position.x, position.y + 1.0f), Quaternion.identity) as GameObject;
        // Laser laser = new Laser();
        // laser = laserObject.GetComponent<Laser>();
        // laser.helixMode = false;
        // laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        laserObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
    }
    void InstantiateAngledLaser(Vector3 position, float angle)
    {
        // Instantiate your laser prefab or object
        // GameObject laserObject = Instantiate(laserPrefab, position, Quaternion.identity);
        GameObject laser= Instantiate(laserPrefab, position, Quaternion.identity);

        // Laser laser = new Laser();
        // laser = laser.GetComponent<Laser>();
        // Laser laser = laserObject.GetComponent<Laser>();
        // laser.helixMode = false;

        // Calculate the velocity vector based on angle and projectile speed
        float angleInRadians = angle * Mathf.Deg2Rad; // Convert angle to radians
        Vector2 velocity = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)) * projectileSpeed;
            // Set the velocity to the Rigidbody2D component of the laser
        laser.GetComponent<Rigidbody2D>().velocity = velocity;



        // Apply rotation to the laser based on the specified angle
        laser.transform.rotation = Quaternion.Euler(0f, 0f, -angle);

        // Additional configuration or logic can go here...
    }


    void InstantiateLasersWithDelay()
    {
        for (int i = 0; i < 2; i++)
        {
            // Use a helper function to calculate the delay
            float delay = i * 0.01f;

            // Invoke the instantiation with the calculated delay
            // Invoke(nameof(InstantiateHelixLaser), delay);

            // Invoke the instantiation with the calculated delay
            Invoke(nameof(InstantiateFirstHelixLaser), delay);
            Invoke(nameof(InstantiateSecondHelixLaser), delay + 0.25f); // Adjust delay for the second laser
        }
    }
    void InstantiateFirstHelixLaser()
    {
        // Instantiate the first helix laser with a slight delay
        InstantiateHelixLaser(transform.position - Vector3.right * 0.5f, false);
    }

    void InstantiateSecondHelixLaser()
    {
        // Instantiate the second helix laser with a slight delay
        InstantiateHelixLaser(transform.position + Vector3.right * 0.5f, true);
    }


    private void InstantiateHelixLaser(Vector3 position, bool rightLaser)
    {
        // // Laser laser = (Laser)Instantiate(laserPrefab, new Vector2(position.x, position.y + 1.0f), Quaternion.identity);
        // GameObject laserObject = Instantiate(laserPrefab, new Vector2(position.x, position.y + 1.0f), Quaternion.identity);
        // // Laser laser = new Laser();

        // // Get the Laser component attached to the instantiated laserObject
        // // Laser laser = laserObject.GetComponent<Laser>();
        // // laser = laserObject.GetComponent<Laser>();
        // Laser laser = laserObject.GetComponent<Laser>();
        // laser.helixMode = true;
        // if(rightLaser)
        //     laser.rightLaser = true;
        // else
        //     laser.rightLaser = false;
        


        if (laserPrefab != null)
        {
            GameObject laserObject = Instantiate(laserPrefab, position, Quaternion.identity);
            Laser laser = laserObject.GetComponent<Laser>();

            if (laser != null)
            {
                laser.helixMode = true;
                laser.rightLaser = rightLaser;
                if(rightLaser)
                    laser.rightLaser = true;
                else
                    laser.rightLaser = false;
            }
            else
            {
                Debug.LogError("Laser component not found on instantiated laserPrefab.");
            }
        }
        else
        {
            Debug.LogError("laserPrefab is not assigned in the Inspector.");
        }
    }
    

     private void ScaleSprite(float scaleFactor)
    {
        // Scale the transform of the GameObject
        myTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
         CircleCollider2D circleCollider = myCollider as CircleCollider2D;
        if (circleCollider != null)
        {
              // Get the current radius
            float currentRadius = circleCollider.radius;
            circleCollider.radius = currentRadius * scaleFactor;
        }
    }

    private void ResetPlayerUpgrades()
    {
        if(player_1)
        {
            GameManger.instance.Set_P1_FiringMode(0);
            ScaleSprite(1.0f);
            moveSpeed = 1.0f;
            shootDelay = 1.0f;
            sheildCapacity = 1000;
            slowMode = false;
            StopLoopCoroutine();
        }
        else{
            GameManger.instance.Set_P2_FiringMode(0);
            ScaleSprite(1.0f);
            moveSpeed = 1.0f;
            shootDelay = 1.0f;
            sheildCapacity = 1000;
            slowMode = false;
            StopLoopCoroutine();
        }
    }

    void StartLoopCoroutine()
    {
        loopCoroutine = StartCoroutine(SpawnGhostEffect());
    }

    void StopLoopCoroutine()
    {
        if (loopCoroutine != null)
        {
            StopCoroutine(loopCoroutine);
            loopCoroutine = null;
        }
    }



    private void OnTriggerEnter2D(Collider2D other){
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        //Check for null
        if(!damageDealer) return;

        if (isInvincible)
            Debug.Log("INVINCIBLE");
        else
            hit(damageDealer);

    }

    private void hit(DamageDealer damageDealer){
        if(!sheildOn){
            // Debug.Log("PLAYER GOT HIT: " + health);
            health -= damageDealer.GetDamage();
              // Trigger camera shake when player gets hit
            // Debug.Log("PLAYER H: " + health);
            
            if(health <= 0)
                Die();
        }
        // else{
        //     Debug.Log("Sheild ON");
        //     Debug.Log("Sheild Capacity : " + sheildCapacity);
        //     sheildCapacity -= damageDealer.GetDamage();
        //     if(sheildCapacity <= 0)
        //     {
        //         sheildOn = false;
        //         sheild.SetActive(false);

        //     }
        // }
    }

    private void Die(){
        //Destroy(gameObject);
        //Reset player position create invulnerable effect 

        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
        
        ResetPlayerUpgrades();
        

        // playerLives[life].enabled = false;
        life -= 1;
        playerLives[life].enabled = false;
        
        if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.MULTI_PLAYER || 
            GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.ONLINE )
        {
            GameManger.instance.SetTotalLife(GameManger.instance.GetTotalLife() - 1);
        }
        
        transform.position = startPosition;
        isInvincible = true;
        // if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.SINGLE_PLAYER)

        //Make player invincible for 3 seconds 

        if (life <= 0)
        {
            Destroy(gameObject);

            if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.SINGLE_PLAYER)
            {
                FindObjectOfType<Level>().LoadGameOver();
            }
        }
        
        if (GameManger.instance.GetTotalLife()  <= 0)
        {
                FindObjectOfType<Level>().LoadGameOver();

        }
    }


}
