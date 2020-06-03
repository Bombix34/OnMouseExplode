using UnityEngine;


[CreateAssetMenu(fileName = "new level", menuName = "Explofun/new level")]
public class Level : ScriptableObject
{

    //Default control allows negative values = bad

    public BoardSettings settings;

    [System.Serializable]
    public class Row
    {
        public TileManager.TileType[] entries;
    }

    //Hide default array drawing = ugly
    [HideInInspector]
    public Row[] levelTile;
}