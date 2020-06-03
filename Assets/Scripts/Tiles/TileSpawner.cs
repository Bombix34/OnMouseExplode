using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileSpawner : MonoBehaviour
{
    [SerializeField]
    private BoardSettings m_Settings;
    private BoardManager m_BoardManager;

    [SerializeField]
    private Transform m_Level1Container, m_Level2Container;
    private Transform m_CurrentLevelContainer;

    public Level m_TestLevel;

    private void Awake()
    {
        m_BoardManager = GetComponent<BoardManager>();
        m_CurrentLevelContainer = m_Level2Container;
    }

    private void Start()
    {
        InitSpawnTest();
    }

    private void ChangeCurrentLevelContainer()
    {
        if (m_CurrentLevelContainer == m_Level1Container)
            m_CurrentLevelContainer = m_Level2Container;
        else
            m_CurrentLevelContainer = m_Level1Container;
    }

    public void InitSpawnTest()
    {
        ResetContainer();
        ChangeCurrentLevelContainer();
        for (int i = 0; i < m_Settings.COLUMN; ++i)
        {
            for(int j = 0; j < m_Settings.RAW; ++j)
            {
                TileManager.TileType currentTyleType = m_TestLevel.levelTile[j].entries[i];
                if (currentTyleType != TileManager.TileType.empty)
                {
                    GameObject tileSpawned = Instantiate(m_Settings.TILE_PREFAB, m_CurrentLevelContainer) as GameObject;
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
        MoveContainers(false);
    }

    public void MoveContainers(bool isDisappear)
    {
        if(isDisappear)
        {
            if (m_CurrentLevelContainer == m_Level1Container)
                m_Level2Container.transform.position = new Vector2(0f, 0f);
            else
                m_Level1Container.transform.position = new Vector2(0f, 0f);
            m_CurrentLevelContainer.DOMoveX(0f, 0.5f).OnComplete(() => InitSpawnTest());
        }
        else
        {
            m_CurrentLevelContainer.DOMoveX(-10f, 0.5f);
        }
    }

    private void ResetContainer()
    {
        for (int i = 0; i < m_CurrentLevelContainer.childCount; ++i)
        {
            Destroy(m_CurrentLevelContainer.GetChild(i).gameObject);
        }
    }
}
