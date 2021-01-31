using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public GameObject player;
    public GameObject uiManager;

    // Shows the win state to the player if they enter the trigger
    void OnTriggerEnter(Collider other)
    {
        // If the hitbox is the player, trigger the win (WIP)
        if (other == player.gameObject.GetComponent<BoxCollider>())
        {
            player.gameObject.GetComponent<Rigidbody>().position = new Vector3(3.65f, 1.6f, -2.183f);
            player.gameObject.GetComponent<Rigidbody>().rotation = new Quaternion(0, 0, 0, 0);
            player.gameObject.transform.position = new Vector3(3.65f, 1.6f, -2.183f);
            player.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            player.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            uiManager.GetComponent<UIManager>().Win();
        }
    }
}
