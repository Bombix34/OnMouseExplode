using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileRenderer : MonoBehaviour
{
    private TileSettings m_Settings;

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
        m_BaseScale = m_Renderer.transform.localScale;
    }

    private void Start()
    {
        m_BasePosition = this.transform.position;
    }

    private void Update()
    {
        if(IsBouncing)
        {
            float bounceAmount = Mathf.PingPong(Time.time*m_Settings.BounceSpeed, 0.3f);
            Scale = new Vector2(m_BaseScale.x + bounceAmount, m_BaseScale.x + bounceAmount);
        }
        if(IsShaking)
        {
            Position = transform.parent.position + m_BasePosition + (Random.insideUnitSphere*m_Settings.ShakeSpeed);
        }
        if(IsGrowing)
        {
            m_GrowingAmount += (Time.fixedDeltaTime * m_Settings.GrowingSpeed);
            Scale = new Vector2(m_BaseScale.x + m_GrowingAmount, m_BaseScale.x + m_GrowingAmount);
        }
    }

    public void ResetTile()
    {
        Color = Color.white;
        Scale = m_BaseScale;
        m_GrowingAmount = 0f;
        Position = transform.parent.position+m_BasePosition;
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

    public Color Color { set { m_Renderer.color = value; }}

    public Vector2 Scale
    {
        get { return m_Renderer.transform.localScale; }
        set { m_Renderer.transform.localScale = value; }
    }

    public Vector2 Position
    {
        set { m_Renderer.transform.position = value; }
    }

    #endregion
}
