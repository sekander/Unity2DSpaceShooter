using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Debug.Log (Input.GetTouch(0).position);

            if(Input.GetTouch(0).phase == TouchPhase.Began)
                Debug.Log ("Touch Begin");
            if(Input.GetTouch(0).phase == TouchPhase.Moved)
                Debug.Log ("Touch Moved");
            if(Input.GetTouch(0).phase == TouchPhase.Ended)
                Debug.Log ("Touch Ended");


        }
        
    }
}
