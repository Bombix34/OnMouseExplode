using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DATABASE", menuName = "Explofun/new level database")]
public class LevelDataBase : ScriptableObject
{
    public Level m_LevelToTest;
    [SerializeField]
    private List<Level> m_LevelDatabase;
    private List<Level> m_LevelAvailable;

    public Level RandomLevel
    {
        get
        {
            if (m_LevelAvailable == null || m_LevelAvailable.Count == 0)
                ResetLevelAvailable();

            Level toReturn = m_LevelAvailable[Random.Range(0, m_LevelAvailable.Count)];
            m_LevelAvailable.Remove(toReturn);
            return toReturn;
        }
    }

    public void ResetLevelAvailable()
    {
        m_LevelAvailable.Clear();
        foreach(Level level in m_LevelDatabase)
        {
            m_LevelAvailable.Add(level);
        }
    }
}
