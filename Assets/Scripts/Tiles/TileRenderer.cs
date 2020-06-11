using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileRenderer : MonoBehaviour
{
    private TileSettings m_Settings;
    private TileDatas m_Datas;
    private TileManager m_Manager;

    [SerializeField]
    private SpriteRenderer m_Renderer;
    private Vector3 m_BaseScale;
    private Vector3 m_BasePosition;

    public bool IsBouncing { get; set; } = false;
    public bool IsShaking { get; set; } = false;
    public bool IsGrowing { get; set; } = false;
    private float m_GrowingAmount = 0f;

    private void Awake()
    {
        m_Manager = GetComponent<TileManager>();
        m_BaseScale = m_Renderer.transform.localScale;
        m_BasePosition = m_Renderer.transform.position;
    }

    private void Update()
    {
        if (!m_Manager.IsActive)
            return;
        if(IsBouncing)
        {
            float bounceAmount = Mathf.PingPong(Time.time*m_Settings.BounceSpeed, 0.3f);
            Scale = new Vector2(m_BaseScale.x + bounceAmount, m_BaseScale.x + bounceAmount);
        }
        if (IsShaking)
        {
            Position = (Random.insideUnitSphere * m_Settings.ShakeSpeed);
        }
        else
            Position = Vector3.zero;
        if(IsGrowing)
        {
            m_GrowingAmount += (Time.fixedDeltaTime * m_Settings.GrowingSpeed);
            Scale = new Vector2(m_BaseScale.x + m_GrowingAmount, m_BaseScale.x + m_GrowingAmount);
        }
        else
            Scale = m_BaseScale;
    }

    public void ResetTile()
    {
        Color = m_Datas.m_Color;
        Scale = m_BaseScale;
        m_GrowingAmount = 0f;
        Position = Vector3.zero;
    }

    public void Squash(bool isSquashX)
    {
        if(isSquashX)
        {
            m_Renderer.transform.DOScaleY(Scale.y - 0.6f, 0.1f);
            m_Renderer.transform.DOScaleX(Scale.x + 0.3f, 0.1f);
        }
        else
        {
            m_Renderer.transform.DOScaleX(Scale.x - 0.6f, 0.1f);
            m_Renderer.transform.DOScaleY(Scale.y + 0.3f, 0.1f);
        }
        
    }

    #region GET/SET

    public TileSettings Settings { set { m_Settings = value; } }

    public TileDatas Datas { set { m_Datas = value; } }

    public Color Color { set { m_Renderer.color = value; }}

    public Vector2 Scale
    {
        get { return m_Renderer.transform.localScale; }
        set { m_Renderer.transform.localScale = value; }
    }

    public Vector2 Position
    {
        set { m_Renderer.transform.localPosition= value; }
    }

    public GameObject RendererObject { get { return m_Renderer.transform.gameObject; } }

    #endregion
}
