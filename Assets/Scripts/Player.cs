using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public enum DirectionFacing: int
    {
        LEFT = -1,
        RIGHT = 1,
    }

    [Header("Player Setting")]
    [Range(1f, 10f)]
    public float speed = 1;

    [Header("Player Logic")]
    public bool isFalling = false;
    public bool hasTeleported = false;
    public DirectionFacing directionFacing = DirectionFacing.RIGHT;

    [SerializeField] private bool isMovementDisabled = false;

    [Range(0.05f, 1f)]
    [SerializeField] private float waitTime = 0.1f;
    private float linecastMultiplier = 1.0f;
    private Teleportable teleportable;
    private LayerMask wallMask;
    private LayerMask boxMask;
    private BoxCollider2D boxCollider2D;
    private void Awake()
    {

        teleportable = GetComponent<Teleportable>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        wallMask = LayerMask.GetMask("Walls");
        boxMask = LayerMask.GetMask("Boxes");
    }

    void Start()
    {
        GameObject.FindGameObjectWithTag("Gem").GetComponent<Gem>().OnLevelComplete += OnGemContacted;
        StartCoroutine(PlayerMove(NextMove(transform.position)));
    }

    // Assume that the movement is valid
    IEnumerator PlayerMove(Vector3 end)
    {
        yield return new WaitForSeconds(StageManager.instance.movementInterval);
        while (transform.position != end)
        {
            if (!teleportable.HasTeleported())
            {
                transform.position = Vector3.MoveTowards(transform.position, end, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();

            } else
            {
                teleportable.FinishTeleportation();
                break;

            }
        }

        yield return StartCoroutine(Wait());
        yield return StartCoroutine(WaitForFalling());


        if (!isMovementDisabled)
        {
            end = transform.position;
            Vector3 next = NextMove(end);
            Debug.DrawLine(end, next, Color.red, 1);
            StartCoroutine(PlayerMove(next));
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
    }
    // Get the next open position for the player to move to
    public Vector3 NextMove(Vector3 start)
    {
        // Check if the linecast hits the left and/or right wall
        bool hitLeftWall = CheckIfPlayerHitWall(start, DirectionFacing.LEFT);
        bool hitRightWall = CheckIfPlayerHitWall(start, DirectionFacing.RIGHT);

        // Get the left and right position
        Vector3 leftPosition = start + Vector3.left;
        Vector3 rightPosition = start + Vector3.right;

        // Player is in a one hole gap
        if (hitLeftWall && hitRightWall)
        {
            return start;
        } 
        
        // Handle case when the player is facing right
        else if (directionFacing == DirectionFacing.RIGHT)
        {
            if (hitRightWall)
            {
                SwitchDirection();
                return leftPosition;
            }

            else
            {
                return rightPosition;
            }

        }

        // Handle case when the player is facing left
        else if (directionFacing == DirectionFacing.LEFT)
        {
            if (hitLeftWall)
            {
                SwitchDirection();
                return rightPosition;
            }

            else
            {
                return leftPosition;
            }
        }

        throw new Exception("No codepath for movement");
    }

    // Helper function to convert direction to a Vector3
    public Vector3 GetVectorDirection(DirectionFacing directionFacing)
    {
        return new Vector3((int) directionFacing, 0, 0);
    }

    // Checks if the player hits a nearby wall using a linecast and given direction
    public bool CheckIfPlayerHitWall(Vector3 start, DirectionFacing direction)
    {
        Vector3 end = start + GetVectorDirection(direction) * linecastMultiplier;
        RaycastHit2D hit = Physics2D.Linecast(start, end, wallMask);
        return hit ? true : false;
    }

    // Change the direction and flip the sprite
    public void SwitchDirection()
    {
        bool willTurnLeft = directionFacing == DirectionFacing.RIGHT;

        if (willTurnLeft)
        {
            directionFacing = DirectionFacing.LEFT;
            transform.eulerAngles = new Vector3(0, 180, 0);
        } 
        else
        {
            directionFacing = DirectionFacing.RIGHT;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    // Waits for player to finish falling
    public IEnumerator WaitForFalling()
    {
        isFalling = true;
/*        Debug.Log(boxCollider2D.IsTouchingLayers(wallMask));
        Debug.Log(transform.position);*/

        // Check if the player is on top of the wall or box
        while (!boxCollider2D.IsTouchingLayers(wallMask | boxMask))
        {
            yield return new WaitForEndOfFrame();
        }

        isFalling = false;
    }
    

    public void OnGemContacted()
    {
        isMovementDisabled = true;
    }
}
