using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour
{
    public delegate void TeleportAction();
    public event TeleportAction OnTeleport;

    public int counter = 0;

    private Teleporter[] teleporters;
    private List<Teleporter> selections = new List<Teleporter>();
    private Vector3 ZLocation = new Vector3(0, 0, 0.5f);

    public bool canTeleport
    {
        get
        {
            return selections.Count == 2;
        }
    }


    void Start()
    {
        teleporters = GetComponentsInChildren<Teleporter>();
        AssignTeleporterIDs();
    }


    // Assign each teleporter in the stage with a generated id
    private void AssignTeleporterIDs()
    {
        for (int i = 0; i < teleporters.Length; i++)
        {
            teleporters[i].AssignID(i);
        }
    }

    public void ClearSelection()
    {
        if (canTeleport)
            Debug.Log("clear selection called");
        {

            // Deselect all teleporters and remove them from the selection
            foreach (Teleporter selection in selections)
            {
                selection.SetDeselect();
            }

            selections.Clear();
        }
    }

    // Assume that the teleporter is not selected
    public void AddTeleporterToSelection(Teleporter teleporter)
    {
        int n = selections.Count;
        // Replace the last teleporter clicked with another teleporter
        if (n == 2)
        {
            selections[0].SetDeselect();
            selections.RemoveAt(0);
            selections.Add(teleporter);
        }

        else
        {
            // Add the teleporter to the end of the stack if there are currently less than two teleporters selected
            selections.Add(teleporter);

            // There are now two teleporters selected
            if (n == 1)
            {
                // Check inside a box collider of each teleport to see if something can be teleported 
                foreach (Teleporter selection in selections.ToArray())
                {
                    if (selection)
                    {
                        selection.CheckCollider();
                    }
                }
            }
        }
    }

    // Remove a teleporter from the selection
    // Assumes that the teleporter is already selected
    public void RemoveTeleporterFromSelection(Teleporter teleporter)
    {
        (Teleporter tele, int id) = GetTeleporterFromSelection(teleporter.id);
        if (id != -1)
        {
            selections.RemoveAt(id);
        }
    }

    public (Teleporter, int) GetTeleporterFromSelection(int id, bool shouldGetOther = false) {

        System.Predicate<Teleporter> predicate;

        if (shouldGetOther)
        {
            predicate = tele => tele.id != id;
        } 
        
        else
        {
            predicate = tele => tele.id == id;
        }

        Teleporter teleporter = selections.Find(predicate);
        int index = selections.FindIndex(predicate);
        return (teleporter, index);
    }

    public void TeleportCollider(Collider2D collision, Vector3 location)
    {
        Teleportable teleportable = collision.GetComponent<Teleportable>();
        if (teleportable && canTeleport)
        {
            OnTeleport?.Invoke();
            teleportable.Teleport(location);
            ClearSelection();
        }
    }
}
