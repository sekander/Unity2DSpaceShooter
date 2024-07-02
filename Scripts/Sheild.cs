using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Sheild : MonoBehaviour
{
    private SpriteRenderer renderer;

    private Coroutine toggleCoroutine;

    private int sheildCapacity = 1000;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        // whiteMat = renderer.material;

        // renderer.material = spriteMat;
        renderer.material.SetFloat("_ShowWhite", 0.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyLaser"))
        {
            if (toggleCoroutine != null)
                StopCoroutine(toggleCoroutine);

            toggleCoroutine = StartCoroutine(ToggleShaderValue());


            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
            sheildCapacity -= damageDealer.GetDamage();
            Destroy(other.gameObject);
            Debug.Log("Sheild Capacity : " + sheildCapacity);
            if(sheildCapacity <= 0)
            {
                // sheildOn = false;
                // SetActive(false);
                // enabled = false;
                // sheildCapacity = 1000;
                // StopCoroutine(toggleCoroutine);
                Destroy(gameObject);

            }




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
