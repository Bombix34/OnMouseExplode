using UnityEngine;
using UnityEditor;
using JsonParse;


[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //Cast target to Level
        Level m = (Level)target;

        //Never let user go below 1 w/h
        // m.settings.COLUMN = Mathf.Max(1, EditorGUILayout.IntField("settings.COLUMN:", m.settings.COLUMN));
        //   m.settings.RAW = Mathf.Max(1, EditorGUILayout.IntField("settings.RAW:", m.settings.RAW));

        //Check that the array sizes match w/h values
        CheckArraySizes(m);

        GUILayout.Space(10f);
        //Draw popups
        for (int i = m.levelTile.Length - 1; i >= 0; i--)
        {
            GUILayout.BeginHorizontal();
            for (int j = 0; j < m.levelTile[i].entries.Length; j++)
            {

                GUI.backgroundColor = GetCorrespondingColor(m.levelTile[i].entries[j]);
                if (GUILayout.Button(((int)m.levelTile[i].entries[j]).ToString(), GUILayout.Width(50), GUILayout.Height(20)))
                {
                    m.levelTile[i].entries[j]++;
                    if (m.levelTile[i].entries[j] >= TileManager.TileType.MAX_COUNT)
                        m.levelTile[i].entries[j] = 0;
                }
            }
            GUILayout.EndHorizontal();
        }
        GUI.backgroundColor = Color.white;
        GUILayout.Label("OPTIONS");
        if (GUILayout.Button("RESET GRID"))
        {
            for (int i = m.levelTile.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < m.levelTile[i].entries.Length; j++)
                {
                    m.levelTile[i].entries[j] = TileManager.TileType.EMPTY;
                }
            }
        }

        GUILayout.Space(10f);

        GUILayout.Label("SAVE/LOAD");
        if (GUILayout.Button("SAVE LEVEL INTO FILE"))
        {
            if (m.levelFileName == "")
                return;
            LevelMap level = new LevelMap(m);
            JsonParse<LevelMap>.SaveIntoJsonFile("Assets/Resources/Levels/"+m.levelFileName, level);
        }
        if (GUILayout.Button("LOAD FILE"))
        {
            if (m.loadFileLevel == null)
                return;
            LevelMap toLoad = JsonParse<LevelMap>.FromJson(m.loadFileLevel);
            for (int i = m.levelTile.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < m.levelTile[i].entries.Length; j++)
                {
                    m.levelTile[i].entries[j] = toLoad.levelTile[i].entries[j];
                }
            }
        }

    }

    private void CheckArraySizes(Level m)
    {
        if (m.levelTile == null || 
            m.levelTile.Length == 0 ||
            m.levelTile[0] == null ||
            m.levelTile[0].entries.Length == 0)
        {
            //Create/init new array when there isn't one
            m.levelTile = new Level.Row[m.settings.RAW];
            for (int i = 0; i < m.levelTile.Length; i++)
            {
                m.levelTile[i] = new Level.Row();
                m.levelTile[i].entries = new TileManager.TileType[m.settings.COLUMN];
            }
        }
        else if (m.levelTile.Length != m.settings.RAW)
        {
            //resizing number of rows
            int oldRAW = m.levelTile.Length;
            bool growing = m.settings.RAW > m.levelTile.Length;
            System.Array.Resize(ref m.levelTile, m.settings.RAW);
            if (growing)
            {
                //Add new rows to array when growing array
                for (int i = oldRAW; i < m.settings.COLUMN; i++)
                {
                    m.levelTile[i] = new Level.Row();
                    m.levelTile[i].entries = new TileManager.TileType[m.settings.COLUMN];
                }
            }

        }
        else if (m.levelTile[0].entries.Length != m.settings.COLUMN)
        {
            Debug.Log("INIT LEVEL V3");
            //resizing number of entries per row
            for (int i = 0; i < m.levelTile.Length; i++)
            {
                System.Array.Resize(ref m.levelTile[i].entries, m.settings.COLUMN);
            }
        }
    }

    private Color GetCorrespondingColor(TileManager.TileType typeConcerned)
    {
        if (typeConcerned == TileManager.TileType.EMPTY)
            return new Color(0.5f, 0.5f, 0.5f, 0.5f);
        else if (typeConcerned == TileManager.TileType.TYPE_01)
            return new Color(0.6f, 0.6f, 0.9f, 1f);
        else if (typeConcerned == TileManager.TileType.TYPE_02)
            return new Color(0.6f, 0.9f, 0.6f, 1f);
        else if (typeConcerned == TileManager.TileType.TYPE_03)
            return new Color(0.9f, 0.9f, 0.6f, 1f);
        else if (typeConcerned == TileManager.TileType.TYPE_04)
            return new Color(0.6f, 0.9f, 0.9f, 1f);
        else
            return Color.black;
    }
}