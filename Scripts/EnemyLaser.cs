using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {}
        
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag ("Player_1"))
          Debug.Log("Collision Enter with: " + other.gameObject.name);

    // Draw a red ray from each contact point along the collision normal
    // foreach (ContactPoint2D contact in other.contacts)
    // {
    //     Debug.DrawRay(contact.point, contact.normal, Color.red);
    // }
      
    }
}

