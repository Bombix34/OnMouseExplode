using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventMessage 
{
    public MessageType m_MessageType;
    public bool m_BoolParam;
    public int m_FloatParam;
    public TileManager.TileType m_TileTypeParam;
    public FaceType m_FaceTypeParam;

    public enum MessageType
    {
        IS_SHAKING,
        IS_ACTIVE,
        IS_LOCK,
        IS_GROWING,
        SPAWN_TILE,
        SWITCH_FACE,
        FLEE,
        ACTIVATE_ON_MOUSE_OVER,
        ACTIVATE_ON_MOUSE_EXIT
    }
}
