using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new tile datas", menuName = "Explofun/Tiles Datas")]
public class TileDatas : ScriptableObject
{
    public TileManager.TileType m_TileType;
    public Color m_Color;
    public List<FaceData> m_FaceDatas;
}
