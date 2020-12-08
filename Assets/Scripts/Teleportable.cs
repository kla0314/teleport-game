using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportable : MonoBehaviour
{
    [Header("Teleportable Logic")]
    [SerializeField] bool hasTeleported = false;
    public void Teleport(Vector3 location)
    {
        transform.position = location;
        hasTeleported = true;
    }

    public bool HasTeleported()
    {
        return hasTeleported;
    }

    public void FinishTeleportation()
    {
        hasTeleported = false;
    }
}
