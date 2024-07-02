using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class NetworkDataSend : MonoBehaviour
{
    // Singleton instance
    private static NetworkDataSend instance;

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

    // Singleton instance property
    public static NetworkDataSend Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("NetworkDataSend").AddComponent<NetworkDataSend>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;

            
        }
    }

    // Private constructor to enforce singleton pattern
    private NetworkDataSend() { }
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


    // Serialize NetworkData object to byte array
    public byte[] Serialize()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream())
        {
            formatter.Serialize(stream, this);
            return stream.ToArray();
        }
    }

    // Deserialize byte array to NetworkData object
    public static NetworkDataSend Deserialize(byte[] data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream(data))
        {
            return formatter.Deserialize(stream) as NetworkDataSend;
        }
    }







}
