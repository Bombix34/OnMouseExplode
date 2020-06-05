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
    private TileGetter m_TileGetter;

    [SerializeField]
    private Transform m_Level1Container, m_Level2Container;
    private Transform m_CurrentLevelContainer;

    public void InitSpawner()
    {
        m_BoardManager = GetComponent<BoardManager>();
        DebugDisplay.Instance.Log(m_BoardManager.name);
        m_DataBase.ResetLevelAvailable();
        m_CurrentLevelContainer = m_Level2Container;
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
    }

    private void SpawnLevel(LevelMap newLevel)
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
                    GameObject tileSpawned = m_TileGetter.GetTileAvailable();
                    tileSpawned.SetActive(true);
                    tileSpawned.transform.parent = m_CurrentLevelContainer;
                    tileSpawned.transform.localPosition =new Vector2(i * m_Settings.COLUMN_DISPLACEMENT, j * m_Settings.RAW_DISPLACEMENT);
                    TileManager tile = tileSpawned.GetComponent<TileManager>();
                    tile.Init(currentTyleType, new Vector2(i, j));
                    m_BoardManager.Board.InitTile(tile, i, j);
                }
                else
                    m_BoardManager.Board.InitTile(null, i, j);
            }
        }
        float chrono = 0f;
        DOTween.To(() => chrono, x => chrono = x, 1f, 0.2f)
            .OnComplete(()=>MoveContainers(false));
    }

    public void MoveContainers(bool isDisappear)
    {
        if(isDisappear)
        {
            if (m_CurrentLevelContainer == m_Level1Container)
                m_Level2Container.transform.position = new Vector2(10f, 0f);
            else
                m_Level1Container.transform.position = new Vector2(10f, 0f);
            m_CurrentLevelContainer.DOMoveX(10f, 0.5f).OnComplete(() =>LaunchSpawn());
        }
        else
        {
            m_CurrentLevelContainer.DOMoveX(0f, 0.5f);
        }
    }

    private void ResetContainer()
    {
        if (m_CurrentLevelContainer.childCount == 0)
            return;
        m_TileGetter.ResetTiles();
    }
}

public enum SpawnType
{
    TEST_LEVEL,
    RANDOM
}
