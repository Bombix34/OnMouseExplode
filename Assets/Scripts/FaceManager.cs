﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceManager : MonoBehaviour
{
    private FaceType m_CurrentFace = FaceType.NEUTRAL;
    [SerializeField]
    private SpriteRenderer m_EyeLeftSR, m_EyeRightSR, m_MouthSR, m_SourcilLeft, m_SourcilRight;
    [SerializeField]
    private Transform m_EyeLeftContainer, m_EyeRightContainer;
    private List<FaceData> m_FaceDatas;

    private bool m_CanMoveEyes = false;

    public void Init(TileDatas datas)
    {
        m_FaceDatas = datas.m_FaceDatas;
        SwitchFace(FaceType.NEUTRAL);
    }

    private void Update()
    {
        if(m_CanMoveEyes)
        {
            Vector3 targ = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targ.z = 0f;
            Vector3 objectPos = transform.position;
            targ.x = targ.x - objectPos.x;
            targ.y = targ.y - objectPos.y;
            float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
            m_EyeLeftContainer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            m_EyeRightContainer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            m_EyeLeftContainer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 130));
            m_EyeRightContainer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 130));
        }
    }

    public void SwitchFace(FaceType newFace)
    {
        FaceData concernedDatas=m_FaceDatas[0];
        foreach (FaceData item in m_FaceDatas)
            if (item.faceType == newFace)
                concernedDatas = item;
        m_CurrentFace = concernedDatas.faceType;
        m_EyeLeftSR.sprite = concernedDatas.eyeLeftSprite;
        m_EyeRightSR.sprite = concernedDatas.eyeRightSprite;
        m_MouthSR.sprite = concernedDatas.mouthSprite;
        m_EyeLeftSR.flipX = concernedDatas.eyeLeftNeedFlip;
        m_CanMoveEyes = concernedDatas.canMoveEye;
        m_SourcilLeft.gameObject.SetActive(concernedDatas.hasSourcil);
        m_SourcilRight.gameObject.SetActive(concernedDatas.hasSourcil);
    }

    public void SwitchFace(int newFace)
    {
        FaceData concernedDatas = m_FaceDatas[0];
        foreach (FaceData item in m_FaceDatas)
            if ((int)item.faceType == newFace)
                concernedDatas = item;
        m_CurrentFace = concernedDatas.faceType;
        m_EyeLeftSR.sprite = concernedDatas.eyeLeftSprite;
        m_EyeRightSR.sprite = concernedDatas.eyeRightSprite;
        m_MouthSR.sprite = concernedDatas.mouthSprite;
        m_EyeLeftSR.flipX = concernedDatas.eyeLeftNeedFlip;
        m_CanMoveEyes = concernedDatas.canMoveEye;
        m_SourcilLeft.gameObject.SetActive(concernedDatas.hasSourcil);
        m_SourcilRight.gameObject.SetActive(concernedDatas.hasSourcil);
    }
}

[System.Serializable]
public struct FaceData
{
    public FaceType faceType;
    public Sprite mouthSprite;
    public Sprite eyeLeftSprite;
    public Sprite eyeRightSprite;
    public bool canMoveEye;
    public bool eyeLeftNeedFlip;
    public bool hasSourcil;
}

[System.Serializable]
public enum FaceType
{
    NEUTRAL =0,
    SCARED  =1,
    LOCK    =2,
    ANGRY   =3
}

