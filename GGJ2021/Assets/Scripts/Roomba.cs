﻿using System.Collections;
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
    [SerializeField] List<Rigidbody> allForceObjs;
    [SerializeField] List<GameObject> nearbyForceObjs;
    Vector3 playerpos;
    public GameObject uiManager;

    // Start is called before the first frame update
    void Start()
    {
        target = 0;
        roombaState = 0;
        player = FindObjectOfType<PlayerControl>();
        Rigidbody[] allForceObjsArray = FindObjectsOfType<Rigidbody>();
        foreach(Rigidbody rigid in allForceObjsArray)
        {
            if(rigid != gameObject.GetComponent<Rigidbody>())
            {
                allForceObjs.Add(rigid);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Key position
        playerpos = player.gameObject.transform.position;

        //Loop through all rigidbodies, aka objects with force
        foreach (Rigidbody rigid in allForceObjs)
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
                transform.position += Vector3.Normalize(new Vector3(waypoints[target].transform.position.x, 0, waypoints[target].transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z)) * Time.deltaTime * speed;

                //Rotate towards direction
                Quaternion rotation = Quaternion.LookRotation(waypoints[target].transform.position - transform.position, Vector3.up);
                transform.rotation = rotation;

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

                //Raycast
                Ray ray = new Ray(playerpos, Vector3.Normalize(new Vector3(playerpos.x - transform.position.x, 0, playerpos.z - transform.position.z)));
                RaycastHit hit;

                //Detect distance to key
                //Takes vertical and horizontal distance into account
                if (Mathf.Abs(playerpos.y - transform.position.y) < 0.5f && SurfaceDist(playerpos, transform.position) < 1)
                {
                    /*
                    if(Physics.Raycast(ray, out hit, 10))
                    {
                        if(hit.collider.tag != "House")
                        {*/
                            roombaState = 1;
                        /*}
                    }*/

                }
                break;
            case 1:
                //Move towards key
                transform.position += Vector3.Normalize(new Vector3(playerpos.x - transform.position.x, 0, playerpos.z - transform.position.z)) * Time.deltaTime * speed;

                //Rotate towards direction
                rotation = Quaternion.LookRotation(new Vector3(playerpos.x, 0, playerpos.z) - new Vector3(transform.position.x, 0, transform.position.z), Vector3.up);
                transform.rotation = rotation;

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
        // If the hitbox is the player, trigger the kill (WIP)
        if(other == player.gameObject.GetComponent<BoxCollider>())
        {
            player.gameObject.GetComponent<Rigidbody>().position = new Vector3(3.65f,1.6f,-2.183f);
            player.gameObject.GetComponent<Rigidbody>().rotation = new Quaternion(0,0,0,0);
            player.gameObject.transform.position = new Vector3(3.65f,1.6f,-2.183f);
            player.gameObject.transform.rotation = new Quaternion(0,0,0,0);
            player.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            uiManager.GetComponent<UIManager>().Death();
        }
    }
}
