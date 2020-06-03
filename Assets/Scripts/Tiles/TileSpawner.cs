using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileSpawner : MonoBehaviour
{
    [SerializeField]
    private SpawnType m_SpawnType;
    [SerializeField]
    private BoardSettings m_Settings;
    [SerializeField]
    private LevelDataBase m_DataBase;
    private BoardManager m_BoardManager;

    [SerializeField]
    private Transform m_Level1Container, m_Level2Container;
    private Transform m_CurrentLevelContainer;

    private void Awake()
    {
        m_BoardManager = GetComponent<BoardManager>();
        m_DataBase.ResetLevelAvailable();
        m_CurrentLevelContainer = m_Level2Container;
    }

    private void Start()
    {
        SpawnLevel();
    }

    private void ChangeCurrentLevelContainer()
    {
        if (m_CurrentLevelContainer == m_Level1Container)
            m_CurrentLevelContainer = m_Level2Container;
        else
            m_CurrentLevelContainer = m_Level1Container;
    }

    public void SpawnLevel()
    {
        if (m_SpawnType == SpawnType.TEST_LEVEL)
            SpawnLevel(m_DataBase.m_LevelToTest);
        else if (m_SpawnType == SpawnType.RANDOM)
            SpawnLevel(m_DataBase.RandomLevel);

    }

    public void SpawnLevel(Level newLevel)
    {
        ResetContainer();
        ChangeCurrentLevelContainer();
        for (int i = 0; i < m_Settings.COLUMN; ++i)
        {
            for(int j = 0; j < m_Settings.RAW; ++j)
            {
                TileManager.TileType currentTyleType = newLevel.levelTile[j].entries[i];
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
            m_CurrentLevelContainer.DOMoveX(0f, 0.5f).OnComplete(() => SpawnLevel());
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

public enum SpawnType
{
    TEST_LEVEL,
    RANDOM
}
