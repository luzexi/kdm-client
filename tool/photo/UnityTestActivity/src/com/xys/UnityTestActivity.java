package com.xys;


import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import com.unity3d.player.UnityPlayerActivity;

public class UnityTestActivity extends UnityPlayerActivity {
//public class UnityTestActivity extends Activity {
	
	Context mContext = null;

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		mContext = this;
	}

	//Unity�л��������������������ִ������ ��ʼ�������
	 public void TakePhoto(String str)
	 {
	         Intent intent = new Intent(mContext,WebViewActivity.class);
	         intent.putExtra("type", str);
	         this.startActivity(intent);
	 }	
}