using UnityEngine;

public class GameManger : MonoBehaviour
{
    public static GameManger instance;

    private int p1_score;
    private int p2_score;

    private int total_life = 6;
    private int killCount;

    private string server_ip;
    private bool player_is_host = false;

    private int p1_firingMode = 0;
    private int p2_firingMode = 0;
    private int p1_shipUpgrade = 0;
    private int p2_shipUpgrade = 0;

    public enum PLAY_MODE
    {
        SINGLE_PLAYER,
        MULTI_PLAYER,
        ONLINE
    }

    private PLAY_MODE playMode = PLAY_MODE.SINGLE_PLAYER; 

    public PLAY_MODE GetPLAY_MODE() { return playMode; }   

    public void SetPlayMode(PLAY_MODE mode)
    {
        playMode = mode;
    }

    public void SetTotalLife(int _total_life) { total_life = _total_life;}
    public int GetTotalLife() { return total_life;} 


    private bool connection_found = false;

      public bool Connection_Found 
    {
        get { return connection_found; }
        set { connection_found = value; }   
    }

    private bool connection_accept = false;
    public bool Connection_Accepted
    {
        get { return connection_accept; }
        set { connection_accept = value; }   
    }



    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public int GetPlayeOneScore()
    {
        return p1_score;
    }
    
    public int GetPlayeTwoScore()
    {
        return p2_score;
    }
    public void ResetPlayerOneScore()
    {
        p1_score = 0;
    }
    public void ResetPlayerTwoScore()
    {
        p2_score = 0;
    }

    public int GetKillCount()
    {
        return killCount;
    }

    public void IncreasePlayerOneScore(int amount)
    {
        p1_score += amount;
    }
    public void IncreasePlayerTwoScore(int amount)
    {
        p2_score += amount;
    }

    public void IncreaseKillCount(int amount)
    {
        killCount += amount;
    }

      // Getter for server_ip
    public string GetServerIP()
    {
        return server_ip;
    }

    // Setter for server_ip
    public void SetServerIP(string ip)
    {
        server_ip = ip;
    }

    // Getter for player_is_host
    public bool IsPlayerHost()
    {
        return player_is_host;
    }

    // Setter for player_is_host
    public void SetPlayerHost(bool isHost)
    {
        player_is_host = isHost;
    }



    public int Get_P1_FiringMode()
    {
        return p1_firingMode;
    }
    public int Get_P2_FiringMode()
    {
        return p2_firingMode;
    }

    public void Set_P1_FiringMode(int mode)
    {
        p1_firingMode = mode;
    }
    public void Set_P2_FiringMode(int mode)
    {
        p2_firingMode = mode;
    }
    


    public int Get_P1_ShipMode()
    {
        return p1_shipUpgrade;
    }
    public int Get_P2_ShipMode()
    {
        return p2_shipUpgrade;
    }
    public void Set_P1_ShipUpgrade (int mode)
    {
        p1_shipUpgrade = mode;
    }
    public void Set_P2_ShipUpgrade (int mode)
    {
        p2_shipUpgrade = mode;
    }
}
