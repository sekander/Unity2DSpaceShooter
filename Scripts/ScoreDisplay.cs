using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{

    //[SerializeField] TMP_Text scoreText;
    public TMP_Text score_p1_Text;
    public TMP_Text score_p2_Text;

    //GameObject target_p1_Object;
    //GameObject target_p2_Object;


    // Start is called before the first frame update
    void Start()
    {
        //target_p1_Object = GameObject.FindWithTag("Player 1 Score");
        //target_p2_Object = GameObject.FindWithTag("Player 2 Score");

        //score_p1_Text = GetComponent<TMP_Text>();
        //score_p1_Text = target_p1_Object.GetComponent<TMP_Text>();
        //score_p2_Text = target_p2_Object.GetComponent<TMP_Text>();


        //gameSession = FindObjectOfType<GameSession>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.SINGLE_PLAYER)
            score_p1_Text.text = GameManger.instance.GetPlayeOneScore().ToString();
        // else if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.MULTI_PLAYER)
        else
        {
            score_p1_Text.text = GameManger.instance.GetPlayeOneScore().ToString();
            score_p2_Text.text = GameManger.instance.GetPlayeTwoScore().ToString();
        }

        /*
        if(gameSession.GetPlayMode() == GameSession.PLAY_MODE.SINGLE_PLAYER) { 
            score_p1_Text.text = gameSession.Get_P1_Score().ToString();
        }
        else if(gameSession.GetPlayMode() == GameSession.PLAY_MODE.MULTI_PLAYER) { 
            score_p1_Text.text = gameSession.Get_P1_Score().ToString();
            score_p2_Text.text = gameSession.Get_P2_Score().ToString();
        }
        */
        
    }
}
