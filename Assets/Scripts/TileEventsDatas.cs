using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEventsDatas : MonoBehaviour
{
    [SerializeField]
    private TileManager.TileType m_TileConcerned;
    private List<TileEvents> m_OnStartActions,m_OnMouseOverActions, m_OnMouseExitActions, m_OnLockActions;
    private bool isInit = false;

    public void PrepareEventsDatas()
    {
        m_OnStartActions = new List<TileEvents>();
        m_OnMouseOverActions = new List<TileEvents>();
        m_OnMouseExitActions = new List<TileEvents>();
        m_OnLockActions = new List<TileEvents>();
        TileEvents[] events = Resources.LoadAll<TileEvents>("Tiles/" + m_TileConcerned + "_EVENTS");
        foreach (TileEvents evnt in events)
        {
            if (evnt.EventType == TileEvents.EventConcerned.ON_START)
                m_OnStartActions.Add(evnt);
            else if (evnt.EventType == TileEvents.EventConcerned.ON_MOUSE_OVER)
                m_OnMouseOverActions.Add(evnt);
            else if (evnt.EventType == TileEvents.EventConcerned.ON_MOUSE_EXIT)
                m_OnMouseExitActions.Add(evnt);
            else if (evnt.EventType == TileEvents.EventConcerned.ON_LOCK)
                m_OnLockActions.Add(evnt);
        }
        isInit = true;
    }

    public void Init(TileManager tileManager)
    {
        tileManager.OnMouseOver.AddListener(() => TriggerEvent(tileManager,m_OnMouseOverActions));
        tileManager.OnMouseExit.AddListener(() => TriggerEvent(tileManager, m_OnMouseExitActions));
        tileManager.OnLock.AddListener(() => TriggerEvent(tileManager, m_OnLockActions));
        tileManager.OnStart.AddListener(() => TriggerEvent(tileManager, m_OnStartActions));
    }

    private void TriggerEvent(TileManager tile, List<TileEvents> events)
    {
        foreach(TileEvents evt in events)
        {
            evt.InvokeEvent(tile);
        }
    }

    public TileManager.TileType TileConcerned
    {
        get
        {
            return m_TileConcerned;
        }
    }
}
