using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


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

	private Vector3[] path = new Vector3[3];
	public void ShowEdit()
	{
		mDeleteEditorNode.SetActive(true);
		//transform.DOLocalPath(path, 0.5f, DG.Tweening.PathType.Linear, DG.Tweening.PathMode.Full3D);
	}

	public void HidenEdit()
	{
		mDeleteEditorNode.SetActive(false);
		//transform.DOLocalPath(path.Reverse(), 0.5f, DG.Tweening.PathType.Linear, DG.Tweening.PathMode.Full3D);
	}
}