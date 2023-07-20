using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// was suppost to help the player push objects around but that doesnt seem necessary at the moment b/c its seems that unitys player controller already covers that
/// </summary>

public class ObstaclePush : MonoBehaviour
{
    [SerializeField]
    private float forcemagnitude;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;
        if(rigidbody != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rigidbody.AddForceAtPosition(forceDirection * forcemagnitude, transform.position, ForceMode.Impulse);
        }
    }
}
