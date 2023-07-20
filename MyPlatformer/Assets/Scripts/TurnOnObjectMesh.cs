using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// made this script to hopefully help with bug causing some walls mesh textures to be left off when they are not suppost to
/// resulting in the player suddenly finding random walls dissappering for no reason
/// </summary>

public class TurnOnObjectMesh : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Wall") && other.gameObject.GetComponent<MeshRenderer>().shadowCastingMode != UnityEngine.Rendering.ShadowCastingMode.On)
        {
            Debug.Log(other);
            other.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }
}
