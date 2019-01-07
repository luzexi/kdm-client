using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//message box waiting
public class UIMessageBoxWaiting : ScreenBaseHandler
{
    public GameObject ImgWaitting;
    public const float fShowWatting = 1.0f;
    public const float fCloseWnd = 3.0f;
    private float fTime = 0.0f;
    private bool bClosed = false;

    public override void Init()
	{
		base.Init();
	}

	public override void CloseScreen()
    {
        base.CloseScreen();
    }

    public override void OpenScreen()
    {
    	base.OpenScreen();
        InitData();
    }

    public void InitData()
    {
        fTime = 0.0f;
        bClosed = false;

        if (ImgWaitting != null)
            ImgWaitting.SetActive(false);
    }

    public void Update()
    {
        if (bClosed)
            return;

        if (fTime < fShowWatting)
        {
        }
        else if (fTime < fCloseWnd)
        {
            if (ImgWaitting!=null && ImgWaitting.activeSelf == false)
                ImgWaitting.SetActive(true);
        }
        else
        {
            bClosed = true;
            CloseScreen();
        }

        fTime += Time.unscaledDeltaTime;
    }
}