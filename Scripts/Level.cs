using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{

    [SerializeField] float gameOverDelay = 2.0f;
   // public GameObject subMenu;

    private void Start()
    {

        //Debug.Log("Singleton Test P1: " + GameManger.instance.GetPlayeOneScore());
        //Debug.Log("Singleton Test P2: " + GameManger.instance.GetPlayeTwoScore());
        //GameManger.instance.IncreasePlayerOneScore(10);
        //GameManger.instance.IncreasePlayerTwoScore(10);
        //Debug.Log("Singleton Test P1: " + GameManger.instance.GetPlayeOneScore());
        //Debug.Log("Singleton Test P2: " + GameManger.instance.GetPlayeTwoScore());

        //subMenu = GameObject.FindWithTag("SubMenu");
    }

    /*
    public void showSubMenu()
    {
        if (subMenu != null)
        {
            Debug.Log("Sub menu active");
            subMenu.SetActive(!subMenu.activeSelf);
        }
            

        //subMenu.isActiveAndEnabled = true;
    }

    public void hideSubMenu()
    {
        if (subMenu != null)
        {
            Debug.Log("Sub menu active");
        }

        Debug.Log("Hide Sub Menu");
          
        subMenu.active = false;
        //subMenu.SetActive(false);

        //subMenu.isActiveAndEnabled = true;
    }
    */


    public void LoadStartMenu(){
        SceneManager.LoadScene(0);
    }

    public void Single_Player(){
        //FindObjectOfType<GameSession>().SetPlayMode(GameSession.PLAY_MODE.SINGLE_PLAYER);
        GameManger.instance.ResetPlayerOneScore();
        GameManger.instance.SetPlayMode(GameManger.PLAY_MODE.SINGLE_PLAYER);
        SceneManager.LoadScene("Single_Player-Game");
        //FindObjectOfType<GameSession>().ResetGame();
    }
    public void Multi_Player(){
        //FindObjectOfType<GameSession>().SetPlayMode(GameSession.PLAY_MODE.MULTI_PLAYER);
        GameManger.instance.ResetPlayerOneScore();
        GameManger.instance.ResetPlayerTwoScore();
        GameManger.instance.SetTotalLife(6);
        GameManger.instance.SetPlayMode(GameManger.PLAY_MODE.MULTI_PLAYER);
        SceneManager.LoadScene("Multi_Player-Game");
        //FindObjectOfType<GameSession>().ResetGame();
    }
    public void Online_Player(){
        //FindObjectOfType<GameSession>().SetPlayMode(GameSession.PLAY_MODE.MULTI_PLAYER);
        GameManger.instance.ResetPlayerOneScore();
        GameManger.instance.ResetPlayerTwoScore();
        GameManger.instance.SetTotalLife(6);
        GameManger.instance.SetPlayMode(GameManger.PLAY_MODE.ONLINE);
        SceneManager.LoadScene("Online_Player-Game");
        NetworkDataSend.Instance.Player_Time = 0.0f;
        UDP_server.Instance.transmitData("START");
        //FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGame(){

        if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.SINGLE_PLAYER)
        {
            Single_Player();
        }
        else if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.MULTI_PLAYER)
        {
            Multi_Player();
        }
        else if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.ONLINE)
        {
            Online_Player();
        }

        //SceneManager.LoadScene(1);
        //FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameOver(){
        StartCoroutine(WaitAndLoad());
        //FindObjectOfType<GameSession>().ResetScore();

    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(gameOverDelay);
        if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.SINGLE_PLAYER)
        {
            SceneManager.LoadScene("Single_Player_GameOver");
        }
        else
            SceneManager.LoadScene("Multi_Player_GameOver");
    }

    public void QuitGame(){
        Application.Quit();
    }

}
