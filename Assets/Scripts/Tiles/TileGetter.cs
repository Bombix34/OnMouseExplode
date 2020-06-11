using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGetter : MonoBehaviour
{
    public List<TileManager> tiles;
    private TileEventsDatas[] m_TilesEventDatas;

    private void Awake()
    {
        m_TilesEventDatas = GetComponents<TileEventsDatas>();
        foreach(TileEventsDatas data in m_TilesEventDatas)
        {
            data.PrepareEventsDatas();
        }
    }

    public GameObject GetTileAvailable()
    {
        foreach (TileManager tile in tiles)
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

    public void ResetTile(TileManager tile)
    {
        tile.gameObject.SetActive(false);
        tile.transform.parent = this.transform;
        tile.transform.position = new Vector2(100f, 100f);
    }


    public TileEventsDatas GetEventsDatasOfType(TileManager.TileType type)
    {
        foreach (TileEventsDatas datas in m_TilesEventDatas)
        {
            if (datas.TileConcerned == type)
                return datas;
        }
        return m_TilesEventDatas[0];
    }
}
