using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public enum PLAY_MODE
    {
        SINGLE_PLAYER,
        MULTI_PLAYER       
    }
    
    
    int p1_score = 0;
    int p2_score = 0;

    PLAY_MODE state = PLAY_MODE.SINGLE_PLAYER;
    EnemySpawner enemySpawner;
    private float gametime;



    private void Awake(){
        SetupSingleton();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.ONLINE)
        {
            enemySpawner.enabled = false;

            //Need to create new seed every game session 
            //Transmit seed to remote player to genearate the random sequence 
            int fixedSeed = 12345; // Choose a fixed seed
            Random.InitState(fixedSeed);

        }
        else
        enemySpawner.enabled = true;
        Debug.Log("Session Awake");
        //if(!GameManger.instance.IsPlayerHost()){
        //UDP_server.Instance.transmitData("CLient Ready");
        //UDP_server.Instance.transmitData("START");
        //UDP_server.Instance.dataReceived = true;
        //}
    }

    private void SetupSingleton(){
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numberGameSessions > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    public PLAY_MODE GetPlayMode()
    {
        return state;
    }

    public void SetPlayMode(PLAY_MODE mode)
    {
        state = mode;
    }

    public int Get_P1_Score(){
        return p1_score;
    }

    public void AddTo_P1_Score(int scoreValue){
        p1_score += scoreValue;
    }
    public int Get_P2_Score(){
        return p2_score;
    }

    public void AddTo_P2_Score(int scoreValue){
        p2_score += scoreValue;
    }

    public void ResetScore(){
        p1_score = 0;
        p2_score = 0;
    }

    public void ResetGame(){
        Destroy(gameObject);
    }

    public void Update()
    {
        float delta = Time.deltaTime;
        if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.ONLINE)
        {
            if(enemySpawner != null)
            {
                //if(UDP_server.Instance.dataReceived)
                gametime += delta;
                
                if(UDP_server.Instance.startTimeSync)
                {
                    UDP_server.Instance.beginGame = true;
                    // if(GameManger.instance.IsPlayerHost())
                    {
                        double offset = gametime - NetworkDataReceive.Instance.Player_Time;
                        Debug.Log("Game Session Offset: " + offset);

                        // if(gametime >= 0.5 && gametime <= 5.0f- NetworkDataReceive.Instance.Player_Time)
                        // if(gametime >= 0.5 && gametime <= 5.0f- offset)
                        // if(gametime >= 0.5 && (gametime <= 5.0f && NetworkDataReceive.Instance.Player_Time <= 5.0f))
                        if(gametime <= 5.0f && NetworkDataReceive.Instance.Player_Time <= 5.0f)
                            UDP_server.Instance.transmitData(gametime.ToString());
                        else
                        {
                            enemySpawner.enabled = true;
                        }
                    }
                }
                else{
                    //UDP_server.Instance.transmitData("START");
                }
            }
            else {
                Debug.Log("NULL");

                gametime = 0.0f;
                UDP_server.Instance.startTimeSync = false;
                UDP_server.Instance.beginGame = false;
                UDP_server.Instance.dataReceived = false;
                enemySpawner = FindAnyObjectByType<EnemySpawner>();
            }
        }
    }
}
