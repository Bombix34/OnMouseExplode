using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardManager : Singleton<BoardManager>
{
    [SerializeField]
    private BoardSettings m_Settings;
    private TileSpawner m_Spawner;
    public Board Board { get; private set; }

    private bool m_IsActive = true;

    public void Awake()
    {
        Board = new Board(m_Settings.COLUMN, m_Settings.RAW);
    }

    public void Start()
    {
        m_Spawner = GetComponent<TileSpawner>();
        m_Spawner.InitSpawner();
        float chrono = 0f;
        DOTween.To(() => chrono, x => chrono = x, 1f, 0.2f)
            .OnComplete(() => m_Spawner.LaunchSpawn());
    }

    public void OnTileLocked()
    {
        if (!m_IsActive)
            return;
        Board.CurrentTilesLockedNumber++;
        int lockedNumber = Board.CurrentTilesLockedNumber;
        print(lockedNumber);
        print(Board.m_InitialTilesAlive);
        if (lockedNumber == Board.m_InitialTilesAlive/2)
        {
            for (int i = 0; i < m_Settings.COLUMN; ++i)
            {
                for (int j = 0; j < m_Settings.RAW; ++j)
                {
                    TileManager tile = Board.GetTile(i, j);
                    if (tile!=null)
                    {
                        if (tile.Type != TileManager.TileType.EMPTY && !tile.IsLock && !tile.IsMouseOver)
                              tile.Face.SwitchFace(FaceType.ANGRY);
                    }
                }
            }
        }
        else if (lockedNumber >= Board.m_InitialTilesAlive)
        {
            m_IsActive = false;
            DebugDisplay.Instance.Log("END LEVEL");
            ActivateBoard(false);
            float chrono = 0f;
            DOTween.To(() => chrono, x => chrono = x, 1f, 0.2f)
                .OnComplete(() => LaunchNewLevel());
        }
    }

    private void LaunchNewLevel()
    {
        Board = new Board(m_Settings.COLUMN, m_Settings.RAW);
        m_Spawner.MoveContainers(true);
        m_IsActive = true;
    }

    private int LockedTilesNumber()
    {
        int count = 0;
        foreach(TileManager tile in Board.Tiles)
        { 
            if (tile.Type!=TileManager.TileType.EMPTY && tile.IsLock)
                ++count;
        }
        return count;
    }

    public void ActivateBoard(bool isActive)
    {
        for (int i = 0; i < m_Settings.COLUMN; ++i)
        {
            for (int j = 0; j < m_Settings.RAW; ++j)
            {
                if (Board.GetTile(i, j) != null)
                {
                    Board.GetTile(i, j).IsActive = isActive;
                }
            }
        }
    }


    #region GET/SET

    public bool IsLastOne(TileManager.TileType typeConcerned)
    {
        foreach (TileManager tile in Board.Tiles)
        {
            if (tile != null)
            {
                if (!tile.IsLock && tile.Type != typeConcerned)
                    return false;
            }
        }
        return true;
    }

    public List<TileManager> GeTilesInZone(int posX, int posY, int zone)
    {
        List<TileManager> toReturn = new List<TileManager>();
        for (int i = -zone; i < zone + 1; ++i)
        {
            for (int j = -zone; j < zone + 1; ++j)
            {
                if (posX + i < 0 || posX + i >= Board.Column || posY + j < 0 || posY + j >= Board.Raw || (i==0 && j==0))
                    continue;
                toReturn.Add(Board.GetTile(posX + i, posY + j));
            }
        }
        return toReturn;
    }

    public TileSpawner Spawner { get { return m_Spawner; } }

    #endregion
}

public class Board
{
    private int column, raw;
    private TileManager[,] m_Tiles;
    private TileManager.TileType[,] m_Types;
    public int m_InitialTilesAlive = 0;
    private int m_CurrentLockNumber = 0;

    public Board(int column, int raw)
    {
        this.column = column;
        this.raw = raw;
        m_Tiles = new TileManager[this.column, this.raw];
        m_Types = new TileManager.TileType[this.column, this.raw];
        m_InitialTilesAlive = 0;
        m_CurrentLockNumber = 0;
    }

    public void InitTile(TileManager newTile, int column, int raw)
    {
        if (newTile.Type != TileManager.TileType.EMPTY)
            m_InitialTilesAlive++;
        m_Types[column, raw] = newTile.Type;
        m_Tiles[column, raw] = newTile;
        // m_Tiles[column, raw].IsActive = false;
    }

    public TileManager GetTile(int column, int raw)
    {
        return m_Tiles[column, raw];
    }

    public TileManager.TileType GetTileType(int column, int raw)
    {
        return m_Types[column, raw];
    }

    public void SetNewTile(TileManager value, int column, int raw)
    {
        if (value.Type != TileManager.TileType.EMPTY)
            m_InitialTilesAlive++;
        m_Tiles[column, raw] = value;
        m_Types[column, raw] = value.Type;
    }

    public TileManager[,] Tiles { get => m_Tiles; }

    public int Column { get => column;  }

    public int Raw { get => raw;  }

    public int CurrentTilesLockedNumber { get => m_CurrentLockNumber; set { m_CurrentLockNumber = value; } } 

}
