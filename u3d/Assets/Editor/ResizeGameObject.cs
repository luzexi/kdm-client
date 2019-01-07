using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResizeGameObject{
	static float x_series = 1920f/1344f;
	static float y_series = 1080f/766f;
	static Vector3 resizeSeries = new Vector3(x_series,y_series,1f);

	[MenuItem("GameObject/Resize GameObject",false,100)]
	static private void Resize()
	{
		GameObject obj = Selection.activeGameObject;
		if(obj == null)
			return;

		ResizeChild(obj.transform);
	}

	static private void ResizeChild(Transform transform)
	{
		ResizeTransform(transform.gameObject.GetComponent<RectTransform>());
		if(transform.childCount == 0)
		{
			return;
		}
		
		for(int i=0;i<transform.childCount;++i)
		{
			ResizeChild(transform.GetChild(i));
		}
	}

	static private void ResizeTransform(RectTransform rect)
	{
		if(rect == null)
			return;

		rect.localPosition = MulVec(rect.localPosition,resizeSeries);
		rect.sizeDelta = MulVec(rect.sizeDelta,resizeSeries);
	}

	static private Vector3 MulVec(Vector3 v1,Vector3 v2)
	{
		return new Vector3(v1.x*v2.x,v1.y*v2.y,v1.z*v2.z);
	}

	[MenuItem("GameObject/Resize GameObject",true)]
	static private bool VResize()
	{
		GameObject obj = Selection.activeGameObject;
		return obj !=null;
	}

}
