using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChronoManager : MonoBehaviour
{
    [SerializeField]
    private LevelDataBase m_SpawnerSettings;
    public float m_MaxTime = 60f;
    private float m_Currenttime = 0f;
    [SerializeField]
    private Text m_ChronoDisplay;

    private void Update()
    {
        m_Currenttime += Time.deltaTime;
        DisplayChrono(m_SpawnerSettings.CurrentSpawnType);
    }

    private void DisplayChrono(SpawnType chronoType)
    {
        if(chronoType==SpawnType.TEST_LEVEL)
        {
            m_ChronoDisplay.text = "";
        }
        else if(chronoType==SpawnType.RANDOM)
        {
            int decimalNumber = (int)(m_Currenttime % 1) * 100;
            m_ChronoDisplay.text = ((int)m_Currenttime).ToString() + ":" + decimalNumber.ToString();
        }
        else if(chronoType==SpawnType.CRESCENDO)
        {
            float toDispplay = m_MaxTime - m_Currenttime;
            int decimalNumber = (int)((toDispplay % 1) * 100);
            string decimalDisplay = decimalNumber.ToString();
            if (decimalNumber < 10)
                decimalDisplay = "0" + decimalNumber.ToString();
            m_ChronoDisplay.text = ((int)toDispplay).ToString() + ":" + decimalDisplay;
        }
    }
}
