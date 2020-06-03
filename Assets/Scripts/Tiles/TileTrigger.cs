using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    private TileManager m_Manager;

    private void Awake()
    {
        m_Manager = GetComponentInParent<TileManager>();
    }

    private void OnMouseOver()
    {
        m_Manager.OnMouseOver.Invoke();
    }

    private void OnMouseExit()
    {
        m_Manager.OnMouseExit.Invoke();
    }
}
