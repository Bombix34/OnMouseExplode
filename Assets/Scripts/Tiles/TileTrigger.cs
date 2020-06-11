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

    private void Update()
    {
        if (!m_Manager.IsActive)
            return;
        if (m_Manager.IsMouseOver && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    m_Manager.OnMouseExit.Invoke();
                    break;
            }
        }
    }

    private void OnMouseOver()
    {
        if(m_Manager.IsActive && !m_Manager.IsMouseOver)
        {
            m_Manager.IsMouseOver = true;
            m_Manager.OnMouseOver.Invoke();
        }
    }

    private void OnMouseExit()
    {
        if (m_Manager.IsActive && m_Manager.IsMouseOver)
        {
            m_Manager.IsMouseOver = false;
            m_Manager.OnMouseExit.Invoke();
        }
    }
}
