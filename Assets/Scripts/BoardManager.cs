using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    [SerializeField]
    private BoardSettings m_Settings;
    private Board m_Board;

    private void Awake()
    {
        m_Board = new Board(m_Settings.COLUMN, m_Settings.RAW);
    }

    private void Update()
    {
        
    }

    public void OnTileLocked()
    {
        if(LockedTilesNumber()==20)
        {
            for (int i = 0; i < m_Settings.COLUMN; ++i)
            {
                for (int j = 0; j < m_Settings.RAW; ++j)
                {
                    if (m_Board.GetTile(i, j) !=null && !m_Board.GetTile(i, j).IsLock && !m_Board.GetTile(i, j).IsMouseOver )
                        m_Board.GetTile(i, j).Face.SwitchFace(FaceType.angry);
                }
            }
        }
    }

    private int LockedTilesNumber()
    {
        int count = 0;
        for (int i = 0; i < m_Settings.COLUMN; ++i)
        {
            for (int j = 0; j < m_Settings.RAW; ++j)
            {
                if (m_Board.GetTile(i, j) != null &&m_Board.GetTile(i, j).IsLock)
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
            return m_Board;
        }
    }

    #endregion
}

public class Board
{
    private TileManager[,] m_Tiles;

    public Board(int column, int raw)
    {
        m_Tiles = new TileManager[column,raw];
    }

    public void InitTile(TileManager newTile, int column, int raw)
    {
        m_Tiles[column, raw] = newTile;
    }

    public TileManager GetTile(int column, int raw)
    {
        return m_Tiles[column, raw];
    }
}
