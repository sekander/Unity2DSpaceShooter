using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
       SetupSingleton(); 
    }

    private void SetupSingleton(){
        //Gets the type of the current object
        if(FindObjectsOfType(GetType()).Length > 1 )
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
