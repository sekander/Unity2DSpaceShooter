using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLogic : MonoBehaviour
{
    // Start is called before the first frame update

    // [System.Serializable]
    // public struct PickupData
    // {
    //     public Sprite sprite;
    //     public string tag;
    // }
    // public PickupData[] pickupDataArray; // Array to hold your sprite-tag associations

    public Sprite[] pickupSprites; // Array to hold your 10 different sprites
    public float lifetime = 2.5f; // Lifetime of the pickup object in seconds
    int randomIndex;
    private int fixedSeed = 12345; // Choose a fixed seed
    void Start()
    {
        // Check if pickupSprites array is not null and has at least one sprite
        if (pickupSprites != null && pickupSprites.Length > 0)
        {
            
            // Randomly choose an index for the sprite array
            randomIndex = Random.Range(1, pickupSprites.Length + 1);
            // int randomIndex = Random.Range(0, pickupSprites.Length);

            // Get the SpriteRenderer component of this object
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            // Assign the randomly chosen sprite to the SpriteRenderer
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = pickupSprites[randomIndex - 1];
                // spriteRenderer.sprite = pickupDataArray[randomIndex].sprite;
            }
            // gameObject.tag = pickupDataArray[randomIndex].tag;
        }
        else
        {
            Debug.LogError("PickUpLogic: pickupSprites array is not assigned or is empty.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Countdown the timer
        lifetime -= Time.deltaTime;

        // Check if the countdown has reached zero or below
        if (lifetime <= 0f)
        {
            // Destroy this pickup object
            Debug.Log("PowerUp Destroyed");
            Destroy(gameObject);
        }

        transform.position += Vector3.down * Time.deltaTime * 10;
    }
     private void OnTriggerEnter2D(Collider2D other){

        if (other.CompareTag("Player_1"))
        {
            Debug.Log("Player 1 obtained PickUp : " + randomIndex);
            // if(CompareTag("PowerUP"))
            if (randomIndex <= 5)
            {
                 Debug.Log("PowerUp Pickup");
                GameManger.instance.Set_P1_FiringMode(randomIndex);
            }
            else{
                GameManger.instance.Set_P1_ShipUpgrade(randomIndex - 5);
            }
            // if (GameManger.instance.GetPLAY_MODE() == GameManger.PLAY_MODE.ONLINE)
            // {
            //     NetworkDataSend.Instance.Player_Upgrade = randomIndex;
            // }
            
        }
        else if (other.CompareTag("Player_2"))
        {
            Debug.Log("Player 2 obtained PickUp : " + randomIndex);
            // if(CompareTag("PowerUP"))
            if (randomIndex <= 5)
            {
                // Debug.Log("PowerUp Pickup");
                GameManger.instance.Set_P2_FiringMode(randomIndex);
            }
            else{
                GameManger.instance.Set_P2_ShipUpgrade(randomIndex - 5);
            }
        }

        //Check for null
        // Destroy(other.gameObject);
    }

}
