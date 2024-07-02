using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    // Start is called before the first frame update
    [SerializeField] int startingWave = 0;

    [SerializeField] bool looping = false;
    /*
    void Start()
    {
        StartCoroutine(SpawnAllWaves());
    }
    */
    IEnumerator Start()
    {
        do{
            yield return StartCoroutine(SpawnAllWaves());
        }
        while(looping);
    }

    private IEnumerator SpawnAllWaves()
    {
        for(int i = 0; i < waveConfigs.Count; i++)
        {
            var currentWave = waveConfigs[i];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));

        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig){
        for(int i = 0; i < waveConfig.GetNumberOfEnemies(); i++){

            var newEnemy = 

            Instantiate(
                waveConfig.GetEnemyPrefab(), 
                waveConfig.GetWaypoints()[0].transform.position,
                Quaternion.identity);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);

            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }

}
