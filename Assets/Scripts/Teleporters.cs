using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TeleporterObj
{
    public int id;
    public Vector2 location;
    public Color color;

    public TeleporterObj(int id, Vector2 location, Color color)
    {
        this.location = location;
        this.id = id;
        this.color = color;
    }
}


[CreateAssetMenu(menuName = "TeleportGame/Teleporters/Create Teleporters", fileName = "New Teleporters List.asset")]
public class Teleporters : ScriptableObject
{
   public List<TeleporterObj> teleporters;

   [Tooltip("")]
   [Range(0f, 5f)] [SerializeField] private float _animationTime = 1f;


    public void AddTeleporter(int id, Vector2 location, Color color)
    {
        teleporters.Add(new TeleporterObj(id, location, color));
    }

    public void RemoveTeleporter(int id)
    {
        teleporters.RemoveAll(teleporter => teleporter.id == id);
    }
}
