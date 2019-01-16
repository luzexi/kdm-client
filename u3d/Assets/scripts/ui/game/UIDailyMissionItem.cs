using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// ui daily mission
public class UIDailyMissionItem : MonoBehaviour
{
	public Mission mMission;
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
}