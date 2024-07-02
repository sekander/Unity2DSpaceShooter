using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject 
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeed = 2.0f;

    public GameObject GetEnemyPrefab(){return enemyPrefab;}
    public List<Transform> GetWaypoints(){
        var waveWaypoints = new List<Transform>();

        foreach(Transform child in pathPrefab.transform)
        {
            waveWaypoints.Add(child);            
        }

        return waveWaypoints;
    }
    public float GetTimeBetweenSpawns(){return timeBetweenSpawns;}
    public float GetSpawnRandom(){return spawnRandomFactor;}
    public int GetNumberOfEnemies(){return numberOfEnemies;}
    public float getMoveSpeed(){return moveSpeed;}
}
