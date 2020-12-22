using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance = null;

    public delegate void LevelCompleteAction();
    public event LevelCompleteAction OnLevelComplete;

    public delegate void RestartAction();
    public event RestartAction OnRestart;

    [Header("Stage Settings")]
    public float movementInterval = 0.4f;
    public int playerMovementPerInterval = 1;

    [Header("Stage Logic")]
    public bool allGrounded = true;
    // Start is called before the first frame update
    private void Awake()
    {
        //cCheck if instance already exists
        if (instance == null)
        {

            // If not, set instance to this
            instance = this;
        }

        //If instance already exists and it's not this:

        else if (instance != this)
        {

            Destroy(gameObject);
        }
    }
}
