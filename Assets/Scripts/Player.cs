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
    public float speed = 1;
    public float groundDistance = 0.55f;

    [Header("Player Logic")]
    public bool isFalling = false;
    public bool hasTeleported = false;
    public DirectionFacing directionFacing = DirectionFacing.RIGHT;

    private float linecastMultiplier = 1.0f;
    private Teleportable teleportable;
    private LayerMask mask;
    void Start()
    {
        teleportable = GetComponent<Teleportable>();
        mask = LayerMask.GetMask("Walls");
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

        yield return StartCoroutine(WaitForFalling());

        end = transform.position;
        Vector3 next = NextMove(end);
        Debug.DrawLine(end, next, Color.red, 1);
        StartCoroutine(PlayerMove(next));
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
        RaycastHit2D hit = Physics2D.Linecast(start, end, mask);
        return hit ? true : false;
    }

    // Occurs when the player runs into a wall
    public void SwitchDirection()
    {
        directionFacing = directionFacing == DirectionFacing.RIGHT ? DirectionFacing.LEFT : DirectionFacing.RIGHT;
    }

    // Checks if player is falling by casting a raycast and checking its distance from the ground
    public bool IsFalling(Vector3 start)
    {
        RaycastHit2D hit = Physics2D.Raycast(start, Vector2.down, Mathf.Infinity, mask);
        Debug.DrawRay(start, Vector3.down, Color.blue, 1);
        return hit.distance >= groundDistance;
    }

    // Waits for player to finish falling
    public IEnumerator WaitForFalling()
    {
        isFalling = true;
        while (IsFalling(transform.position))
        {
            yield return new WaitForEndOfFrame();
        }
        isFalling = false;
    }
}
