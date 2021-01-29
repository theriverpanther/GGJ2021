using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomba : MonoBehaviour
{
    //Fields
    [SerializeField] GameObject[] waypoints;
    [SerializeField] int target;
    [SerializeField] int roombaState; //1 = Waypoints / 2 = Pursue
    private PlayerControl player;
    [SerializeField] const float speed = 0.2f;
    [SerializeField] Rigidbody[] allForceObjs;
    [SerializeField] List<GameObject> nearbyForceObjs;

    // Start is called before the first frame update
    void Start()
    {
        target = 0;
        roombaState = 0;
        player = FindObjectOfType<PlayerControl>();
        allForceObjs = FindObjectsOfType<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        nearbyForceObjs.Clear();
        foreach(Rigidbody rigid in allForceObjs)
        {
            if(Vector3.Distance(rigid.gameObject.transform.position, transform.position) < 2)
            {
                rigid.gameObject.transform.position += Vector3.Normalize(transform.position - rigid.gameObject.transform.position) * Time.deltaTime;
            }
        }

        switch (roombaState)
        {
            case 0:
                //Move towards current target
                transform.position += Vector3.Normalize((waypoints[target].transform.position - transform.position)) * Time.deltaTime * speed;

                //Swap targets if successfully reached one
                if (Vector3.Distance(waypoints[target].transform.position, transform.position) < 1)
                {
                    if (target < waypoints.Length - 1)
                    {
                        target++;
                    }
                    else
                    {
                        target = 0;
                    }
                }

                //Detect distance to key
                if(Vector3.Distance(player.gameObject.transform.position, transform.position) < 3)
                {
                    roombaState = 1;
                }
                break;
            case 1:
                //Move towards key
                transform.position += Vector3.Normalize((player.gameObject.transform.position - transform.position)) * Time.deltaTime * speed;

                //Give up if the key is far enough away
                if (Vector3.Distance(player.gameObject.transform.position, transform.position) > 5)
                {
                    roombaState = 0;
                }
                break;
        }
    }
}
