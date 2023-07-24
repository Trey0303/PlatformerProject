using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    public Transform platform;

    private void FixedUpdate()
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
