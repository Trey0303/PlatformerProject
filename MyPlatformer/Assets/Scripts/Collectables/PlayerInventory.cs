using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfStandardPickups { get; private set; }

    public UnityEvent<PlayerInventory> OnStandardPickupCollected;

    public void standardCollected()
    {
        NumberOfStandardPickups++;
        OnStandardPickupCollected.Invoke(this);
    }
}
