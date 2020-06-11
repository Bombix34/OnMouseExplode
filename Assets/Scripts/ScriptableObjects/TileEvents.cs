using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new tile event", menuName = "Explofun/Tile Event")]
public class TileEvents : ScriptableObject
{
    [SerializeField]
    private EventConcerned m_EventType;
    [SerializeField]
    private TriggerCondition m_TriggerCondition = TriggerCondition.NONE;
    [SerializeField]
    private TriggerZone m_ZoneTriggerEffect;

    [SerializeField]
    private List<EventMessage> m_EventMessages;

    public void InvokeEvent(TileManager concernedTile)
    {
        if(m_TriggerCondition!= TriggerCondition.NONE)
        {
            if (m_TriggerCondition == TriggerCondition.NOT_LOCK)
            {
                if (concernedTile.IsLock)
                    return;
            }
            else if (m_TriggerCondition == TriggerCondition.IS_LAST_ONE)
            {
                if (!BoardManager.Instance.IsLastOne(concernedTile.Type))
                    return;
            }
            else if (m_TriggerCondition == TriggerCondition.IS_NOT_LAST_ONE)
            {
                if (BoardManager.Instance.IsLastOne(concernedTile.Type))
                    return;
            }
        }
        SendEventMessages(concernedTile);
    }

    private void SendEventMessages(TileManager concernedTile)
    {
        if(m_ZoneTriggerEffect == TriggerZone.SELF)
        {
            foreach(EventMessage evt in m_EventMessages)
            {
                concernedTile.ReceiveEventMessage(evt);
            }
        }
        else if(m_ZoneTriggerEffect==TriggerZone.ZONE)
        {
            List<TileManager> zoneTiles = BoardManager.Instance.GeTilesInZone((int)concernedTile.Position.x,(int)concernedTile.Position.y,1);
            foreach(TileManager tile in zoneTiles)
            {
                foreach (EventMessage evt in m_EventMessages)
                {
                    if(m_EventType==EventConcerned.ON_MOUSE_OVER && tile.IsEventMouseOver){}
                    else
                        tile.ReceiveEventMessage(evt);
                }
            }
        }
    }

    public EventConcerned EventType { get { return m_EventType; } }
   // public TriggerZone EventTriggerZone { get { return m_ZoneTriggerEffect; } }
  //  public List<EventMessage> EventMessages { get { return m_EventMessages; } }

    public enum TriggerCondition
    {
        NONE,
        NOT_LOCK,
        IS_LAST_ONE,
        IS_NOT_LAST_ONE
    }

    public enum TriggerZone
    {
        SELF,
        ZONE,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    public enum EventConcerned
    {
        ON_START,
        ON_MOUSE_OVER,
        ON_MOUSE_EXIT,
        ON_LOCK
    }
}
