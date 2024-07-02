using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100.0f;
    [SerializeField] int scoreValue = 150;


    [Header("Enemy Shooting")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3.0f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10.0f;
    
    
    
    [Header("Enemy Effects")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationVFX = 1.0f;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSFXVolume = 0.7f;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] [Range(0, 1)] float shootSFXVolume = 0.7f;

    [SerializeField] GameObject enemyDrop;


    private bool player_1_kill;
    private bool player_2_kill;

    // private FlashOnHit flashOnHit;

    private SpriteRenderer renderer;

    private Coroutine toggleCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
                // flashOnHit = GetComponent<FlashOnHit>();

        renderer = GetComponent<SpriteRenderer>();
        // whiteMat = renderer.material;

        // renderer.material = spriteMat;
        renderer.material.SetFloat("_ShowWhite", 0.0f);
                

    }

    // Update is called once per frame
    void Update()
    {

        CountDownShot();



    }

    private void OnTriggerEnter2D(Collider2D other){

                    // flashOnHit.FlashOnHitEffect();
                    // renderer.material = spriteMat;   
        // whiteValue = 1.0f;
        if (toggleCoroutine != null)
            StopCoroutine(toggleCoroutine);

        toggleCoroutine = StartCoroutine(ToggleShaderValue());

        if (other.CompareTag("Player 1 laser"))
        {
            Debug.Log("Player 1 laser hit target");
            player_1_kill = true;
        }
        else if (other.CompareTag("Player 2 laser"))
        {
            Debug.Log("Player 2 laser hit target");
            player_2_kill = true;
        }

        //Check for layers and update code to match either player 1 or player 2
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        //Check for null
        if(!damageDealer) return;
        health -= damageDealer.GetDamage();
        
        if(health <= 0)
            Die();

        Destroy(other.gameObject);
        // renderer.material.SetFloat("_ShowWhite", 0.0f);
        // renderer.material = spriteMat;
    }

    private void CountDownShot(){

        //Decrease shot counter for however the frame takes        
        shotCounter -= Time.deltaTime;

        //When shot counter reaches 0 
        //Fire
        if(shotCounter <= 0.0f){
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire(){
        GameObject laser = Instantiate(
            projectile, 
            new Vector2(transform.position.x, transform.position.y - 1.0f),
            Quaternion.identity
        ) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSFXVolume);
    }

    private void Die(){

            //Destroy obect
            Destroy(gameObject);
            
            //Play sound
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
            
            //Create death effect
            GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);

            // GameObject pickUp = Instantiate(enemyDrop, transform.position, Quaternion.identity);
             // Random chance to spawn enemyDrop
            float spawnChance = Random.value; // Random value between 0 and 1
            if (spawnChance <= 0.5f) // 25% chance (0.25f is 25% as a float)
            // if (spawnChance <= 0.85f) // 25% chance (0.25f is 25% as a float)
            // if (spawnChance <= 1.0f) // 25% chance (0.25f is 25% as a float)
            {
                GameObject pickUp = Instantiate(enemyDrop, transform.position, Quaternion.identity);
            }
            
            Destroy(explosion, durationVFX);

            //Add score
            if(player_1_kill)
            { 
                //FindObjectOfType<GameSession>().AddTo_P1_Score(scoreValue);
                GameManger.instance.IncreasePlayerOneScore(10);
                player_1_kill = false;
            }
            else if(player_2_kill) { 
                //FindObjectOfType<GameSession>().AddTo_P2_Score(scoreValue);
                GameManger.instance.IncreasePlayerTwoScore(10);
                player_2_kill = false;
            }
    }

     IEnumerator ToggleShaderValue()
    {
        // Set the shader property to 1.0f
        renderer.material.SetFloat("_ShowWhite", 1.0f);

        // Wait for 0.01 seconds
        yield return new WaitForSeconds(0.01f);

        // Set the shader property back to 0.0f
        renderer.material.SetFloat("_ShowWhite", 0.0f);

        // Reset coroutine reference
        toggleCoroutine = null;
    }
}
