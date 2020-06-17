using System.Collections.Generic;
using UnityEngine;
using JsonParse;

[CreateAssetMenu(fileName = "DATABASE", menuName = "Explofun/new level database")]
public class LevelDataBase : ScriptableObject
{
    [SerializeField]
    private SpawnType m_SpawnType;
    [SerializeField]
    private TextAsset TestFileLevel;
    private LevelMap m_LevelToTest;
    [SerializeField]
    private List<TextAsset> m_AllLevelDatabase;
    private List<LevelMap> m_LevelAvailable;
    [SerializeField]
    private List<TextAsset> m_EasyLevels;
    [SerializeField]
    private List<TextAsset> m_MediumLevels;
    [SerializeField]
    private List<TextAsset> m_HardLevels;
    private int m_CurrentDifficulty = 1;
    public int m_LevelNumberToIncreaseDifficulty = 5;
    private int m_LevelCountThisGame = 0;

    public LevelMap RandomLevel
    {
        get
        {
            if (m_LevelAvailable == null || m_LevelAvailable.Count == 0)
                InitLevelAvailable();
            LevelMap toReturn = m_LevelAvailable[Random.Range(0, m_LevelAvailable.Count)];
            m_LevelAvailable.Remove(toReturn);
            return toReturn;
        }
    }

    public LevelMap RandomCrescendoLevel
    {
        get
        {
            if (m_LevelAvailable == null )
                InitLevelAvailable();
            m_LevelCountThisGame++;
            if(m_LevelCountThisGame % m_LevelNumberToIncreaseDifficulty == 0)
                IncreaseDifficulty();
            LevelMap toReturn = m_LevelAvailable[Random.Range(0, m_LevelAvailable.Count)];
            m_LevelAvailable.Remove(toReturn);
            return toReturn;
        }
    }

    public void InitLevelAvailable()
    {
        if (m_LevelAvailable == null)
            m_LevelAvailable = new List<LevelMap>();
        m_LevelAvailable.Clear();
        m_LevelCountThisGame = 0;
        if(m_SpawnType==SpawnType.RANDOM)
        {
            foreach (TextAsset level in m_AllLevelDatabase)
            {
                m_LevelAvailable.Add(JsonParse<LevelMap>.FromJson(level));
            }
        }
        else if(m_SpawnType==SpawnType.CRESCENDO)
        {
            m_CurrentDifficulty = 1;
            foreach (TextAsset level in m_EasyLevels)
            {
                m_LevelAvailable.Add(JsonParse<LevelMap>.FromJson(level));
            }
        }
    }

    private void IncreaseDifficulty()
    {
        if (m_CurrentDifficulty < 3)
            m_CurrentDifficulty++;
        m_LevelAvailable.Clear();
        if (m_CurrentDifficulty==2)
        {
            foreach (TextAsset level in m_MediumLevels)
            {
                m_LevelAvailable.Add(JsonParse<LevelMap>.FromJson(level));
            }
        }
        else if(m_CurrentDifficulty==3)
        {
            foreach (TextAsset level in m_HardLevels)
            {
                m_LevelAvailable.Add(JsonParse<LevelMap>.FromJson(level));
            }
        }
    }

    public LevelMap LevelToTest
    {
        get
        {
            if(m_LevelToTest==null)
            {
                m_LevelToTest = JsonParse<LevelMap>.FromJson(TestFileLevel);
            }
            return m_LevelToTest;
        }
    }

    public SpawnType CurrentSpawnType { get => m_SpawnType; }
}
