using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance = null;

    [Header("Stage Settings")]
    public float movementInterval = 0.4f;
    public int playerMovementPerInterval = 1;

    [Header("Stage Logic")]
    public bool allGrounded = true;

    private Teleporter[] teleporters;
    private List<Teleporter> selections = new List<Teleporter>();


    public bool canTeleport
    {
        get
        {
            return selections.Count == 2;
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {

        //Check if instance already exists
        if (instance == null)
        {

            //if not, set instance to this
            instance = this;
        }

        //If instance already exists and it's not this:

        else if (instance != this)
        {

            Destroy(gameObject);
        }
    }

    void Start()
    {
        teleporters = FindObjectsOfType<Teleporter>();
        AssignTeleporterIDs();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        {
            // Teleport the player or object 

            // Deselect all teleporters and remove them from the selection
            for (int i = 0; i < selections.Count; i++)
            {
                selections[i].SetDeselect();   
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
                foreach (Teleporter selection in selections)
                {
                    selection.CheckCollider();
                }

            }
        }

    }

    // Remove a teleporter from the selection
    // Assumes that the teleporter is already selected
    public void RemoveTeleporterFromSelection(Teleporter teleporter)
    {
        (Teleporter tele, int id) = GetTeleporterFromSelection(teleporter.id);
         tele.SetDeselect();
         selections.RemoveAt(id);
    }

    public (Teleporter, int) GetTeleporterFromSelection(int id)
    {
        for (int i = 0; i < selections.Count; i++)
        {
            if (selections[i].id == id)
            {
                return (selections[i], i);
            }
        }
        return (null, -1);
    }

    public (Teleporter, int) GetOtherTeleporterFromSelection(int id)
    {
        for (int i = 0; i < selections.Count; i++)
        {
            if (selections[i].id != id)
            {
                return (selections[i], i);
            }
        }
        return (null, -1);
    }
}
