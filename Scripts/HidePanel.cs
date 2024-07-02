using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HidePanel : MonoBehaviour
{

    //public GameObject panel;

    public TMP_Text client_connection_status;
    public TMP_Text client_message_status;
    public Button   readyButton;
    
    public TMP_Text host_connection_status;
    public TMP_Text host_message_status;
    public Button   host_readyButton;
    public Button   host_acceptButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //public void ToggleSubMenu() {
    public void ToggleSubMenu(Canvas canvas) {

        if (canvas != null)
        {
            canvas.enabled = !canvas.enabled;
        }
        else
        {
            Debug.Log("Canvas referenced not assigned");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(client_connection_status != null || readyButton != null ||
          host_connection_status != null || host_readyButton != null)
        {
        // if(GameManger.instance.Connection_Found == true)
        // {
        //     connection_status.text = "Connection Found IP: " + GameManger.instance.GetServerIP();
        // }
        if (GameManger.instance != null && GameManger.instance.Connection_Found)
        {
            string serverIP = GameManger.instance.GetServerIP();
            if (serverIP != null)
            {
                client_connection_status.text = "Connection Found IP: " + serverIP;
                // readyButton.enabled = true;
                host_connection_status.text = "Connection Found IP: " + serverIP;
                // host_readyButton.enabled = true;
                if(GameManger.instance.Connection_Accepted)
                {
                    // if()
                    client_message_status.text = "Accepted";
                    host_message_status.text = "Accepted";
                        readyButton.enabled = true;
                    host_readyButton.enabled = true;
                    //Make button rady 
                }
                else{
                    client_message_status.text = "Waiting For Host To Accept";
                    host_message_status.text = "Waiting For Client To Accept";

                }
            }
            else
            {
                client_connection_status.text = "Connection Found, but IP is null";
            }
        }
        

        }
    }
}
