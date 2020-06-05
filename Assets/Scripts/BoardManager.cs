using UnityEngine;
using DG.Tweening;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private BoardSettings m_Settings;
    private TileSpawner m_Spawner;
    public Board Board { get; private set; }

    public void Awake()
    {
        DebugDisplay.Instance.Log("BOARD AWAKE");
        Board = new Board(m_Settings.COLUMN, m_Settings.RAW);
    }

    public void Start()
    {
        DebugDisplay.Instance.Log("INIT SPAWNER");
        m_Spawner = GetComponent<TileSpawner>();
        m_Spawner.InitSpawner();
        float chrono = 0f;
        DOTween.To(() => chrono, x => chrono = x, 1f, 0.2f)
            .OnComplete(() => m_Spawner.LaunchSpawn());
    }


    public void OnTileLocked()
    {
        if (LockedTilesNumber() == Board.m_InitialTilesAlive/2)
        {
            for (int i = 0; i < m_Settings.COLUMN; ++i)
            {
                for (int j = 0; j < m_Settings.RAW; ++j)
                {
                    if (Board.GetTile(i, j) != null && !Board.GetTile(i, j).IsLock && !Board.GetTile(i, j).IsMouseOver)
                        Board.GetTile(i, j).Face.SwitchFace(FaceType.angry);
                }
            }
        }
        else if (LockedTilesNumber() == Board.m_InitialTilesAlive)
        {
          //  DebugDisplay.Instance.Log("END LEVEL");
            Board = new Board(m_Settings.COLUMN, m_Settings.RAW);
            m_Spawner.MoveContainers(true);
        }
    }

    private int LockedTilesNumber()
    {
        int count = 0;
        for (int i = 0; i < m_Settings.COLUMN; ++i)
        {
            for (int j = 0; j < m_Settings.RAW; ++j)
            {
                if (Board.GetTile(i, j) != null &&Board.GetTile(i, j).IsLock)
                    ++count;
            }
        }
        return count;
    }


    #region GET/SET

    #endregion
}

public class Board
{
    private TileManager[,] m_Tiles;
    public int m_InitialTilesAlive=0;

    public Board(int column, int raw)
    {
        m_Tiles = new TileManager[column,raw];
    }

    public void InitTile(TileManager newTile, int column, int raw)
    {
        if (newTile != null)
            m_InitialTilesAlive++;
        m_Tiles[column, raw] = newTile;
    }

    public TileManager GetTile(int column, int raw)
    {
        return m_Tiles[column, raw];
    }
}
