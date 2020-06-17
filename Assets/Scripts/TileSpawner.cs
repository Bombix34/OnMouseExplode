using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileSpawner : MonoBehaviour
{
    private SpawnType m_SpawnType;
    [SerializeField]
    private BoardSettings m_Settings;
    [SerializeField]
    private LevelDataBase m_DataBase;
    [SerializeField]
    private List<TileDatas> m_TilesDatabase;
    [Space]
    private BoardManager m_BoardManager;
    [SerializeField]
    private TileGetter m_TileGetter;

    [SerializeField]
    private Transform m_Level1Container, m_Level2Container;
    private Transform m_CurrentLevelContainer;

    public void InitSpawner()
    {
        DebugDisplay.Instance.Log("INIT SPAWNER");
        m_BoardManager = GetComponent<BoardManager>();
        DebugDisplay.Instance.Log(m_BoardManager.name);
        m_SpawnType = m_DataBase.CurrentSpawnType;
        m_DataBase.InitLevelAvailable();
        m_CurrentLevelContainer = m_Level2Container;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
           // SpawnTileInZone(2, 2, 1, TileManager.TileType.TYPE_01);
        }
    }

    private void ChangeCurrentLevelContainer()
    {
        if (m_CurrentLevelContainer == m_Level1Container)
            m_CurrentLevelContainer = m_Level2Container;
        else
            m_CurrentLevelContainer = m_Level1Container;
    }

    public void LaunchSpawn()
    {
        DebugDisplay.Instance.Log("SPAWNING");
        if (m_SpawnType == SpawnType.TEST_LEVEL)
            SpawnLevel(m_DataBase.LevelToTest);
        else if (m_SpawnType == SpawnType.RANDOM)
            SpawnLevel(m_DataBase.RandomLevel);
        else if(m_SpawnType == SpawnType.CRESCENDO)
            SpawnLevel(m_DataBase.RandomCrescendoLevel);
    }

    private void SpawnLevel(LevelMap newLevel)
    {
        ResetContainer();
        ChangeCurrentLevelContainer();
        for (int i = 0; i < m_Settings.COLUMN; ++i)
        {
            for(int j = 0; j < m_Settings.RAW; ++j)
            {
                SpawnTile(newLevel.levelTile[j].entries[i], i ,j);
            }
        }
        float chrono = 0f;
        DOTween.To(() => chrono, x => chrono = x, 1f, 0.2f)
            .OnComplete(()=>MoveContainers(false));
    }

    public void SpawnTile(TileManager.TileType currentTileType, int posX, int posY)
    {
        TileDatas datas = GetDataOfType(currentTileType);
        TileManager.TileType tileOnBoard = m_BoardManager.Board.GetTileType(posX, posY);
        GameObject tileSpawned = m_TileGetter.GetTileAvailable();
        tileSpawned.SetActive(true);
        tileSpawned.transform.parent = m_CurrentLevelContainer;
        tileSpawned.transform.localPosition = new Vector2(posX * m_Settings.COLUMN_DISPLACEMENT, posY * m_Settings.RAW_DISPLACEMENT);
        TileManager tile = tileSpawned.GetComponent<TileManager>();
        if (datas == null)
            datas = GetDataOfType(TileManager.TileType.EMPTY);
        tile.Init(datas, new Vector2(posX, posY));
        m_BoardManager.Board.InitTile(tile, posX, posY);
    }

    public void MoveContainers(bool isDisappear)
    {
        if(isDisappear)
        {
            float chrono = 0f;
            DOTween.To(() => chrono, x => chrono = x, 1f, 0.3f)
            .OnComplete(() => MoveOut());
        }
        else
        {
            DebugDisplay.Instance.Log("MOVE IN");
            float chrono = 0f;
            DOTween.To(() => chrono, x => chrono = x, 1f, 0.3f)
            .OnComplete(() => m_BoardManager.ActivateBoard(true));
            m_CurrentLevelContainer.DOMoveX(0f, 0.5f);
        }
    }

    private void MoveOut()
    {
        DebugDisplay.Instance.Log("MOVE OUT");
        if (m_CurrentLevelContainer == m_Level1Container)
            m_Level2Container.transform.position = new Vector2(10f, 0f);
        else
            m_Level1Container.transform.position = new Vector2(10f, 0f);
        m_CurrentLevelContainer.DOMoveX(10f, 0.5f).OnComplete(() => LaunchSpawn());
    }

    private void ResetContainer()
    {
        if (m_CurrentLevelContainer.childCount == 0)
            return;
        m_TileGetter.ResetTiles();
    }

    public TileDatas GetDataOfType(TileManager.TileType type)
    {
        foreach(TileDatas data in m_TilesDatabase)
        {
            if (data.m_TileType == type)
                return data;
        }
        return null;
    }

    public TileGetter TilesContainer { get { return m_TileGetter; } }
}

public enum SpawnType
{
    TEST_LEVEL,
    RANDOM,
    CRESCENDO
}
