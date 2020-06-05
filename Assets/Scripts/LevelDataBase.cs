using System.Collections.Generic;
using UnityEngine;
using JsonParse;

[CreateAssetMenu(fileName = "DATABASE", menuName = "Explofun/new level database")]
public class LevelDataBase : ScriptableObject
{
    [SerializeField]
    private TextAsset TestFileLevel;
    private LevelMap m_LevelToTest;
    [SerializeField]
    private List<TextAsset> m_LevelDatabase;
    private List<LevelMap> m_LevelAvailable;

    public LevelMap RandomLevel
    {
        get
        {
            if (m_LevelAvailable == null || m_LevelAvailable.Count == 0)
                ResetLevelAvailable();
            LevelMap toReturn = m_LevelAvailable[Random.Range(0, m_LevelAvailable.Count)];
            m_LevelAvailable.Remove(toReturn);
            return toReturn;
        }
    }

    public void ResetLevelAvailable()
    {
        if (m_LevelAvailable == null)
            m_LevelAvailable = new List<LevelMap>();
        m_LevelAvailable.Clear();
        foreach(TextAsset level in m_LevelDatabase)
        {
            m_LevelAvailable.Add(JsonParse<LevelMap>.FromJson(level));
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
}
