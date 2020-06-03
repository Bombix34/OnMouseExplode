using UnityEngine;
using UnityEditor;


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

        //Draw popups
        for (int i = m.levelTile.Length-1; i >= 0; i--)
        {
            GUILayout.BeginHorizontal();
            for (int j = 0; j < m.levelTile[i].entries.Length; j++)
            {
                m.levelTile[i].entries[j] = (TileManager.TileType)EditorGUILayout.EnumPopup(m.levelTile[i].entries[j]);
            }
            GUILayout.EndHorizontal();
        }
    }

    void CheckArraySizes(Level m)
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
            //resizing number of entries per row
            for (int i = 0; i < m.levelTile.Length; i++)
            {
                System.Array.Resize(ref m.levelTile[i].entries, m.settings.COLUMN);
            }
        }
    }
}