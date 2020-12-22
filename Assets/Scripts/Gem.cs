using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public delegate void LevelCompleteAction();
    public event LevelCompleteAction OnLevelComplete;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            OnLevelComplete?.Invoke();
        }
    }
}
