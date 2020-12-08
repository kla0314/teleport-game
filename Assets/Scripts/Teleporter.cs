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

    private BoxCollider2D boxCollider;

    private Vector2 boxLength = new Vector2(0.4f, 0.4f);
    private Vector3 ZLocation = new Vector3(0, 0, 0.5f);
    private Vector3 initialScale;
    [SerializeField] private float scaleExpand = 1.1f;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        initialScale = transform.localScale;
        Debug.Log(initialScale);
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    private void OnMouseDown()
    {
        if (isSelected)
        {
            StageManager.instance.RemoveTeleporterFromSelection(this);
            SetDeselect();
            
        } 
        else {
            StageManager.instance.AddTeleporterToSelection(this);
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
        transform.localScale = initialScale * scaleExpand;
        isSelected = true;
    }

    public void AssignID(int id)
    {
        this.id = id;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (StageManager.instance.canTeleport && isSelected) {
            (Teleporter other, int otherID) = StageManager.instance.GetOtherTeleporterFromSelection(id);
            Vector3 location  = other.transform.position;
            TeleportCollider(collision, location);
            StageManager.instance.ClearSelection();
        }
    }

    public void CheckCollider()
    {
        boxCollider.enabled = false;
        Collider2D collision = Physics2D.OverlapBox(transform.position, boxLength, 0);
        boxCollider.enabled = true;
        Debug.Log(collision);

        if (collision && StageManager.instance.canTeleport && isSelected)
        {
            (Teleporter other, int otherID) = StageManager.instance.GetOtherTeleporterFromSelection(id);
            Vector3 location = other.transform.position;
            TeleportCollider(collision, location);
            StageManager.instance.ClearSelection();
        }
    }

    private void TeleportCollider(Collider2D collision, Vector3 location)
    {
        Teleportable teleportable = collision.GetComponent<Teleportable>();
        if (teleportable)
        {
            teleportable.Teleport(location + ZLocation);
        }
    }

}
