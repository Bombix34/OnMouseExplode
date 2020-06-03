using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField]
    private BoardSettings m_Settings;
    private BoardManager m_BoardManager;

    public Level m_TestLevel;

    private void Awake()
    {
        m_BoardManager = GetComponent<BoardManager>();
    }

    private void Start()
    {
        InitSpawnTest();
    }

    public void InitSpawnTest()
    {
        for(int i = 0; i < m_Settings.COLUMN; ++i)
        {
            for(int j = 0; j < m_Settings.RAW; ++j)
            {
                TileManager.TileType currentTyleType = m_TestLevel.levelTile[j].entries[i];
                if (currentTyleType != TileManager.TileType.empty)
                {
                    GameObject tileSpawned = Instantiate(m_Settings.TILE_PREFAB, this.transform) as GameObject;
                    tileSpawned.transform.position = new Vector2(i * m_Settings.COLUMN_DISPLACEMENT, j * m_Settings.RAW_DISPLACEMENT);
                    TileManager tile = tileSpawned.GetComponent<TileManager>();
                    tile.Init(new Vector2(i, j));
                    tile.Type = TileManager.TileType.basic;
                    m_BoardManager.Board.InitTile(tile, i, j);
                }
                else
                    m_BoardManager.Board.InitTile(null, i, j);

                switch (currentTyleType)
                {

                }
            }
        }
    }
}
