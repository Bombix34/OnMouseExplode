using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGetter : MonoBehaviour
{
    public List<TileManager> tiles;

    public GameObject GetTileAvailable()
    {
        foreach(TileManager tile in tiles)
        {
            if (tile.transform.parent == this.transform)
                return tile.gameObject;
        }
        return null;
    }

    public void ResetTiles()
    {
        foreach (TileManager tile in tiles)
        {
            tile.gameObject.SetActive(false);
            tile.transform.parent = this.transform;
            tile.transform.position = new Vector2(100f, 100f);
        }
    }
}
