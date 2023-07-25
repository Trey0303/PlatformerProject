using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///      made this as a seperate script so that it could be attacked to a seperate empty gameobject. I did it this way so that I could have the empty gameobject scale be set to (1,1,1) 
/// to help prevent player scale from being altered while riding on moving platform.
/// </summary>
public class StickyPlatform : MonoBehaviour
{
    public Transform platform;

    private void FixedUpdate()//needs to be fixed update or it wont work
    {
        transform.position = platform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //originalPlayerScale = other.transform.localScale; // Store the player's original scale
            other.transform.SetParent(transform);
            Debug.Log("player on plaform");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.SetParent(null);
            //other.transform.localScale = originalPlayerScale; // Restore the player's original scale
            Debug.Log("Player left platform");

        }
    }
}
