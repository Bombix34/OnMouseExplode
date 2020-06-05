using UnityEngine;


//[CreateAssetMenu(fileName = "new level", menuName = "Explofun/new level")]
public class Level : ScriptableObject
{

    //Default control allows negative values = bad

    public BoardSettings settings;
    public TextAsset loadFileLevel;
    public string levelFileName;

    [System.Serializable]
    public class Row
    {
        public TileManager.TileType[] entries;
    }

    //Hide default array drawing = ugly
    [HideInInspector]
    public Row[] levelTile;
}

public class LevelMap
{
    public Level.Row[] levelTile;

    public LevelMap(TextAsset toLoad)
    {
        Level parser = JsonParse.JsonParse<Level>.FromJson(toLoad);
        levelTile=parser.levelTile;
    }

    public LevelMap(Level level)
    {
        levelTile = level.levelTile;
    }
}