using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private TileSettings m_Settings;
    private TileRenderer m_Renderer;
    private FaceManager m_FaceManager;
    public TileType Type { get; set; }
    private Vector2 m_GridPosition;

    private bool m_IsLock = false;

    public bool IsMouseOver { get; set; } = false;

    [SerializeField]
    private UnityEvent m_OnMouseOver, m_OnMouseExit;

    private void Awake()
    {
        m_Renderer = GetComponent<TileRenderer>();
        m_Renderer.Settings = m_Settings;
        m_FaceManager = GetComponent<FaceManager>();
    }

    private void Update()
    {
        if (IsMouseOver && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    m_OnMouseExit.Invoke();
                    break;
            }
        }
    }


    public void Init(Vector2 positionInGrid)
    {
        m_GridPosition = positionInGrid;
        m_OnMouseOver.AddListener(() => IsMouseOver = true);
        m_OnMouseExit.AddListener(() => IsMouseOver = false);
    }

    private void OnDisable()
    {
        m_OnMouseOver.RemoveAllListeners();
        m_OnMouseExit.RemoveAllListeners();
    }

    public void Die()
    {
        if (m_IsLock)
            return;
        Camera.main.GetComponent<CameraShaker>().Shake(0.07f);
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        m_Renderer.ResetTile();
        m_Renderer.IsGrowing = true;
        yield return new WaitForSeconds(0.1f);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xDist =Mathf.Abs( mousePosition.x - transform.position.x);
        float yDist = Mathf.Abs(mousePosition.y - transform.position.y);
        m_Renderer.Squash(xDist < yDist) ;
        yield return new WaitForSeconds(0.1f);
        m_FaceManager.SwitchFace(FaceType.dead);
        m_Renderer.ResetTile();
        IsLock = true;
    }

    #region GET/SET

    public bool IsPositionCorresponding(Vector2 toTest)
    {
        return m_GridPosition.x == toTest.x && m_GridPosition.y == toTest.y;
    }

    public Vector2 Position { get { return m_GridPosition; } }

    public TileRenderer Renderer {  get { return m_Renderer; } }

    public FaceManager Face {  get { return m_FaceManager; } }

    public TileSettings Settings {  get { return m_Settings; } }

    public UnityEvent OnMouseOver { get { return m_OnMouseOver; } }

    public UnityEvent OnMouseExit { get { return m_OnMouseExit; } }

    public bool IsLock
    {
        set
        {
            m_Renderer.enabled = !value;
            m_IsLock = value;
            if (value)
            {
                m_Renderer.Color = Color.grey;
                BoardManager.Instance.OnTileLocked();
            }
            else
                m_Renderer.ResetTile();
        }
        get
        {
            return m_IsLock;
        }
    }

    #endregion


    public enum TileType
    {
        empty,
        basic
    }
}

