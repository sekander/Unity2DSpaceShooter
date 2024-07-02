using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class NetworkDataReceive : MonoBehaviour
{
    // Singleton instance
    private static NetworkDataReceive instance;

    [SerializeField]
    private float player_xpos;

    [SerializeField]
    private float player_ypos;
    
    [SerializeField]
    private bool player_fired;

    [SerializeField]
    private double time;

    // [SerializeField]
    // private int playerUpgrade;

    public double Player_Time
    {
        get { return time; }
        set { time = value; }
    }

    public float Player_xpos
    {
        get { return player_xpos; }
        set { player_xpos = value; }
    }
    public float Player_ypos
    {
        get { return player_ypos; }
        set { player_ypos = value; }
    }

    public bool Player_fired
    {
        get { return player_fired; }
        set { player_fired = value; }
    }
    // public int Player_Upgrade
    // {
    //     get { return playerUpgrade; }
    //     set { playerUpgrade = value; }
    // }

    // Singleton inst
    // ance property

    public static NetworkDataReceive Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("NetworkDataReceive").AddComponent<NetworkDataReceive>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    

    // Private constructor to enforce singleton pattern
    private NetworkDataReceive() { }
    //public NetworkData() { }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string ToJsonString()
    {
        return JsonUtility.ToJson(this);
    }



}
