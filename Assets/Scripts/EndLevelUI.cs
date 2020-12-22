using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelUI : MonoBehaviour
{
    int timesTeleported = 0;
    int timePassed = 0;
    private TeleporterManager teleporterManager;
    private Gem gem;
    [SerializeField] private Text timesTeleportedText;

    void Start()
    {
        teleporterManager = GameObject.FindGameObjectWithTag("Teleporters").GetComponent<TeleporterManager>();
        gem = GameObject.FindGameObjectWithTag("Gem").GetComponent<Gem>();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
       gem.OnLevelComplete += CompleteLevel;
       teleporterManager.OnTeleport += IncrementTimesTeleported;
    }

    public void IncrementTimesTeleported()
    {
        timesTeleported++;
    }

    public void CompleteLevel()
    {
        gameObject.SetActive(true);
        timesTeleportedText.text = $"Times Teleported: {timesTeleported}";
    }

}
