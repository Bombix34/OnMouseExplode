using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private TileSettings m_Settings;
    private TileEventsDatas[] m_Events;
    private TileRenderer m_Renderer;
    private FaceManager m_FaceManager;
    public TileType Type { get; set; }
    private Vector2 m_GridPosition;

    private bool m_IsActive = true;
    private bool m_IsLock = false;

    public bool IsMouseOver { get; set; } = false;
    public bool IsEventMouseOver { get; set; } = false;

    private UnityEvent m_OnMouseOver, m_OnMouseExit, m_OnLock, m_OnStart;

    public bool test;
    public TileType testType;

    private void Awake()
    {
        m_OnMouseOver = new UnityEvent();
        m_OnMouseExit = new UnityEvent();
        m_OnLock = new UnityEvent();
        m_OnStart = new UnityEvent();
        m_Renderer = GetComponent<TileRenderer>();
        m_Renderer.Settings = m_Settings;
        m_FaceManager = GetComponent<FaceManager>();
        m_Events = GetComponentsInChildren<TileEventsDatas>();
    }

    public void Init(TileDatas datas, Vector2 positionInGrid)
    {
        ResetListeners();
        Type = datas.m_TileType;
        m_Renderer.RendererObject.SetActive(Type != TileType.EMPTY);
        if (Type == TileType.EMPTY)
        {
            IsActive = false;
            m_IsLock = true;
            return;
        }
        BoardManager.Instance.Spawner.TilesContainer.GetEventsDatasOfType(Type).Init(this);
        m_Renderer.Datas = datas;
        m_Renderer.ResetTile();
        m_Renderer.IsBouncing = false;
        m_Renderer.IsGrowing = false;
        m_Renderer.IsShaking = false;
        Face.Init(datas);
        m_GridPosition = positionInGrid;
        IsMouseOver = false;
        IsEventMouseOver = false;
        IsLock = false;
        m_IsActive = false;
        m_OnMouseOver.AddListener(() => IsEventMouseOver = true);
        m_OnMouseExit.AddListener(() => IsEventMouseOver = false);
    }

    private void Update()
    {
        test = m_IsLock;
        testType = Type;
    }

    private void OnDisable()
    {
        ResetListeners();
    }

    private void ResetListeners()
    {
        m_OnMouseOver.RemoveAllListeners();
        m_OnMouseExit.RemoveAllListeners();
        m_OnLock.RemoveAllListeners();
        m_OnStart.RemoveAllListeners();
    }

    public void Lock()
    {
        Camera.main.GetComponent<CameraShaker>().Shake(0.07f);
        float chrono = 0f;
        DOTween.To(() => chrono, x => chrono = x, 1f, 0.3f)
             .OnComplete(() => BoardManager.Instance.OnTileLocked());
        StartCoroutine(LockCoroutine());
    }

    private IEnumerator LockCoroutine()
    {
        m_Renderer.ResetTile();
        yield return new WaitForSeconds(0.1f);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xDist =Mathf.Abs( mousePosition.x - transform.position.x);
        float yDist = Mathf.Abs(mousePosition.y - transform.position.y);
        m_Renderer.Squash(xDist < yDist) ;
        yield return new WaitForSeconds(0.1f);
        m_Renderer.ResetTile();
        m_Renderer.Color = Color.grey;
        m_OnLock.Invoke();
    }

    public void ReceiveEventMessage(EventMessage message)
    {
        switch (message.m_MessageType)
        {
            case EventMessage.MessageType.IS_ACTIVE:
                IsActive = message.m_BoolParam;
                break;
            case EventMessage.MessageType.IS_GROWING:
                m_Renderer.IsGrowing = message.m_BoolParam;
                break;
            case EventMessage.MessageType.IS_SHAKING:
                m_Renderer.IsShaking = message.m_BoolParam;
                break;
            case EventMessage.MessageType.IS_LOCK:
                IsLock = message.m_BoolParam;
                break;
            case EventMessage.MessageType.SPAWN_TILE:
                if (Type==TileType.EMPTY)
                {
                    TileDatas newDatas = BoardManager.Instance.Spawner.GetDataOfType(message.m_TileTypeParam);
                    Init(newDatas, m_GridPosition);
                    Type = newDatas.m_TileType;
                    m_IsActive = true;
                    BoardManager.Instance.Board.SetNewTile(this, (int)m_GridPosition.x, (int)m_GridPosition.y);
                }
                break;
            case EventMessage.MessageType.SWITCH_FACE:
                m_FaceManager.SwitchFace(message.m_FaceTypeParam);
                break;
            case EventMessage.MessageType.ACTIVATE_ON_MOUSE_OVER:
                if (IsActive && !IsEventMouseOver)
                {
                    IsEventMouseOver = true;
                    m_OnMouseOver.Invoke();
                }
                break;
            case EventMessage.MessageType.ACTIVATE_ON_MOUSE_EXIT:
                m_OnMouseExit.Invoke();
                break;
            case EventMessage.MessageType.FLEE:
                break;

        }
    }

    #region GET/SET

    public bool IsPositionCorresponding(Vector2 toTest)
    {
        return m_GridPosition.x == toTest.x && m_GridPosition.y == toTest.y;
    }

    public Vector2 Position { get { return m_GridPosition; } }

    public bool IsActive
    {
        get { return m_IsActive; }
        set
        {
            if(!value)
                ResetListeners();
            m_IsActive = value;
        }
    }

    public TileRenderer Renderer {  get { return m_Renderer; } }

    public FaceManager Face {  get { return m_FaceManager; } }

    public TileSettings Settings {  get { return m_Settings; } }

    public UnityEvent OnMouseOver { get { return m_OnMouseOver; } }

    public UnityEvent OnMouseExit { get { return m_OnMouseExit; } }

    public UnityEvent OnLock { get { return m_OnLock; } }
    public UnityEvent OnStart { get { return m_OnStart; } }

    public bool IsLock
    {
        set
        {
            //m_Renderer.enabled = !value;
            m_IsLock = value;
            if (value)
            {
                Lock();
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
        EMPTY,
        TYPE_01,
        TYPE_02,
        TYPE_03,
        TYPE_04,
        MAX_COUNT
    }
}




