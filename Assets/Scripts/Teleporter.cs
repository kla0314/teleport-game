using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    [Header("Teleporter Info")]
    [Tooltip("Automatically assigned when the stage starts")]
    public int id;

    [Tooltip("Is currently selected by the player")]
    public bool isSelected = false;

    public float transitionTime = 0.5f;
    public Color gizmoColor = new Color(255, 0, 0, 0.5f); // Red

    private BoxCollider2D boxCollider;
    private TeleporterManager teleporterManager;
    private Vector2 boxLength = new Vector2(0.5f, 0.5f);
    private Vector3 initialScale;

    [SerializeField] private float scaleIncrease = 1.1f;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        initialScale = transform.localScale;
        teleporterManager = transform.parent.GetComponent<TeleporterManager>();
    }


    private void OnMouseDown()
    {
        if (isSelected)
        {
            teleporterManager.RemoveTeleporterFromSelection(this);
            SetDeselect();
            
        } 
        else {
            teleporterManager.AddTeleporterToSelection(this);
            SetSelect();
        }
    }

    public void SetDeselect()
    {
        transform.localScale = initialScale;
        isSelected = false;
    }

    public void SetSelect()
    {
        transform.localScale = initialScale * scaleIncrease;
        isSelected = true;
    }

    public void AssignID(int id)
    {
        this.id = id;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (teleporterManager.canTeleport && isSelected) {
            (Teleporter other, int otherID) = teleporterManager.GetTeleporterFromSelection(id, true);
            Vector3 location  = other.transform.position;
            teleporterManager.TeleportCollider(collision, location);
        }
    }

    public void CheckCollider()
    {
        boxCollider.enabled = false;
        Collider2D collision = Physics2D.OverlapBox(transform.position, boxLength, 0);
        boxCollider.enabled = true;

        if (collision && teleporterManager.canTeleport && isSelected)
        {
            (Teleporter other, int otherID) = teleporterManager.GetTeleporterFromSelection(id, true);
            Vector3 location = other.transform.position;
            teleporterManager.TeleportCollider(collision, location);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, boxLength);
    }
}
