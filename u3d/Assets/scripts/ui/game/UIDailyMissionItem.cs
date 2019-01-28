using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


// ui daily mission
public class UIDailyMissionItem : MonoBehaviour
{
	public Mission mMission;
	public UI_Event mBtnBg;
	public UI_Event mBtnCancel;
	public UI_Event mBtnFinish;
	public UI_Event mBtnDelete;
	public UI_Event mBtnEditor;
	public GameObject mDeleteEditorNode;
	public RawImage mImagePic;
	public Text mTextDesc;
	public Text mTextCount;

	private Vector3[] mShowMovePath = null;
	private Vector3[] mShowBackPath = null;

	void Awake()
	{
		//
	}

	public void SetMission(Mission _mis)
	{
		mMission = _mis;
		if(mImagePic != null)
		{
			mImagePic.texture = _mis.texture;
		}
		if(mTextDesc != null)
		{
			mTextDesc.text = _mis.mDesc;
		}
		if(mTextCount != null)
		{
			mTextCount.text = _mis.mCount + " times";
		}

		if(mDeleteEditorNode != null)
		{
			mDeleteEditorNode.SetActive(false);
		}
	}

	public void ShowEdit()
	{
		if(mShowMovePath == null)
		{
			mShowMovePath = new Vector3[2];
			mShowMovePath[0] = new Vector3(0, transform.localPosition.y,0);
			mShowMovePath[1] = new Vector3(-300,transform.localPosition.y,0);
		}
		mDeleteEditorNode.SetActive(true);
		transform.DOLocalPath(mShowMovePath, 0.5f, DG.Tweening.PathType.Linear, DG.Tweening.PathMode.Full3D);
	}

	public void HidenEdit()
	{
		if(mShowBackPath == null)
		{
			mShowBackPath = new Vector3[2];
			mShowBackPath[0] = new Vector3(-300, transform.localPosition.y, 0);
			mShowBackPath[1] = new Vector3(0, transform.localPosition.y, 0);
		}
		mDeleteEditorNode.SetActive(false);
		transform.DOLocalPath(mShowBackPath, 0.5f, DG.Tweening.PathType.Linear, DG.Tweening.PathMode.Full3D);
	}
}