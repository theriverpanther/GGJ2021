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
    Vector3 playerpos;

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
        //Key position
        playerpos = player.gameObject.transform.position;

        //Loop through all rigidbodies, aka objects with force
        foreach(Rigidbody rigid in allForceObjs)
        {
            Vector3 rigidpos = rigid.gameObject.transform.position;
            if (Mathf.Abs(rigidpos.y - transform.position.y) < 0.05f && SurfaceDist(rigidpos, transform.position) < 1.5f)
            {
                Vector3 direction = new Vector3(transform.position.x - rigidpos.x, 0, transform.position.z - rigidpos.z);
                rigid.gameObject.transform.position += Vector3.Normalize(direction) * Time.deltaTime * 0.10f/SurfaceDist(rigidpos, transform.position);
            }
        }

        //Two GameStates: Following the path and Chasing the player (If close enough)
        switch (roombaState)
        {
            case 0:
                //Move towards current target
                transform.position += Vector3.Normalize((waypoints[target].transform.position - transform.position)) * Time.deltaTime * speed;

                //Swap targets if successfully reached one
                if (Vector3.Distance(waypoints[target].transform.position, transform.position) < 0.1f)
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
                //Takes vertical and horizontal distance into account
                if (Mathf.Abs(playerpos.y - transform.position.y) < 0.5f && SurfaceDist(playerpos, transform.position) < 1)
                {
                    roombaState = 1;
                }
                break;
            case 1:
                //Move towards key
                transform.position += Vector3.Normalize(new Vector3(playerpos.x - transform.position.x, 0, playerpos.z - transform.position.z)) * Time.deltaTime * speed;

                //Give up if the key is far enough away
                //Takes vertical and horizontal distance into account
                if (SurfaceDist(playerpos, transform.position) > 1.5f || Mathf.Abs(playerpos.y - transform.position.y) > 1)
                {
                    roombaState = 0;
                }
                break;
        }
    }

    //Calculates horizontal distance in a 3D space
    private float SurfaceDist(Vector3 a, Vector3 b)
    {
        float distance = Vector3.Distance(new Vector3(a.x, 0, a.z), new Vector3(b.x, 0, b.z));
        return Mathf.Abs(distance);
    }

    // Kills the player if they are too close to the roomba
    void OnTriggerEnter(Collider other)
    {
        if(other == player.gameObject.GetComponent<BoxCollider>())
        {
            player.gameObject.GetComponent<Rigidbody>().position = new Vector3(3.65f,1.6f,-2.183f);
            player.gameObject.GetComponent<Rigidbody>().rotation = new Quaternion(0,0,0,0);
            player.gameObject.transform.position = new Vector3(3.65f,1.6f,-2.183f);
            player.gameObject.transform.rotation = new Quaternion(0,0,0,0);
            player.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
