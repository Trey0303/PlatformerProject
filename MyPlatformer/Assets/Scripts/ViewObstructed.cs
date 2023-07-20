using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this script helps prevent walls from getting in the way of keeping the character in view
/// </summary>

public class ViewObstructed : MonoBehaviour
{
    public Transform[] obstructions;
    public Transform player;
    public Transform playerCamera;

    public int oldHitsNumber;

    // Start is called before the first frame update
    void Start()
    {
        oldHitsNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ObstructedView();
    }

    void ObstructedView()
    {

        float characterDistance = Vector3.Distance(playerCamera.transform.position, player.transform.position);
        int layerNumber = LayerMask.NameToLayer("Wall");
        int layerMask = 1 << layerNumber;
        RaycastHit[] hits = Physics.RaycastAll(playerCamera.transform.position, player.position - playerCamera.transform.position, characterDistance, layerMask);
        if (hits.Length > 0)
        {   // Means that some stuff is blocking the view
            int newHits = hits.Length - oldHitsNumber;

            if (obstructions != null && obstructions.Length > 0 && newHits < 0)
            {
                // Repaint all the previous obstructions. Because some of the stuff might be not blocking anymore
                for (int i = 0; i < obstructions.Length; i++)
                {
                    obstructions[i].gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
            }
            obstructions = new Transform[hits.Length];
            // Hide the current obstructions 
            for (int i = 0; i < hits.Length; i++)
            {
                Transform obstruction = hits[i].transform;
                obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                obstructions[i] = obstruction;
            }
            oldHitsNumber = hits.Length;
        }
        else
        {   // Mean that no more stuff is blocking the view and sometimes all the stuff is not blocking as the same time
            if (obstructions != null && obstructions.Length > 0)
            {
                for (int i = 0; i < obstructions.Length; i++)
                {
                    obstructions[i].gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
                oldHitsNumber = 0;
                obstructions = null;
            }
        }

    }
}
