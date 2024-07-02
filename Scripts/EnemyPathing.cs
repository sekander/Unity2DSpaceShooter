using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{

    WaveConfig waveConfig;
    //[SerializeField] WaveConfig waveConfig;
    List<Transform> wayPoints;
    int wayPointsIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        wayPoints = waveConfig.GetWaypoints();
        /*
        foreach (var v in wayPoints){
            Debug.Log("X old: " + v.transform.position.x );
            Debug.Log("X : " + v.position.x );
            Debug.Log("Player : " + transform.position.x );
        }
        */
            //transform.position = wayPoints[wayPointsIndex].transform.position;
            transform.position = new Vector2(10, -10);
            
    }

    public void SetWaveConfig(WaveConfig waveConfig){
            this.waveConfig = waveConfig;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Player : " + transform.position.x );
        //Debug.Log("Index : " + wayPointsIndex );

        if(wayPointsIndex >= wayPoints.Count )
            wayPointsIndex = 0;

        
        if(transform.position.x == wayPoints[wayPointsIndex].transform.position.x)
            wayPointsIndex++;
        
        if(wayPointsIndex <= wayPoints.Count - 1){
            var targetPositon = wayPoints[wayPointsIndex].position;
            var movementThisFrame = waveConfig.getMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(
                transform.position, targetPositon, movementThisFrame);

        }
        




    }



}
