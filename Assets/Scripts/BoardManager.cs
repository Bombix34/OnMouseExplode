using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    [SerializeField]
    private BoardSettings m_Settings;
    private TileSpawner m_Spawner;
    private Board m_CurrentBoard;


    private void Awake()
    {
        m_Spawner = GetComponent<TileSpawner>();
        m_CurrentBoard = new Board(m_Settings.COLUMN, m_Settings.RAW);
    }

    private void Update()
    {
        
    }

    public void OnTileLocked()
    {
        print(m_CurrentBoard.m_InitialTilesAlive);
        if (LockedTilesNumber() == m_CurrentBoard.m_InitialTilesAlive/2)
        {
            for (int i = 0; i < m_Settings.COLUMN; ++i)
            {
                for (int j = 0; j < m_Settings.RAW; ++j)
                {
                    if (m_CurrentBoard.GetTile(i, j) != null && !m_CurrentBoard.GetTile(i, j).IsLock && !m_CurrentBoard.GetTile(i, j).IsMouseOver)
                        m_CurrentBoard.GetTile(i, j).Face.SwitchFace(FaceType.angry);
                }
            }
        }
        else if (LockedTilesNumber() == m_CurrentBoard.m_InitialTilesAlive)
        {
            m_CurrentBoard = new Board(m_Settings.COLUMN, m_Settings.RAW);
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
                if (m_CurrentBoard.GetTile(i, j) != null &&m_CurrentBoard.GetTile(i, j).IsLock)
                    ++count;
            }
        }
        return count;
    }


    #region GET/SET

    public Board Board
    {
        get
        {
            return m_CurrentBoard;
        }
    }

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
