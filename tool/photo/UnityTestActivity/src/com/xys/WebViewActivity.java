package com.xys;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

import com.unity3d.player.UnityPlayer;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.provider.MediaStore;
import android.view.KeyEvent;
import android.widget.ImageView;

public class WebViewActivity extends Activity
{
	

    ImageView imageView = null;  
   
    public static final int NONE = 0;  
    public static final int PHOTOHRAPH = 1;// ����  
    public static final int PHOTOZOOM = 2; // ����  
    public static final int PHOTORESOULT = 3;// ���  
    
    public static final String IMAGE_UNSPECIFIED = "image/*";  
    
    
    public final static String FILE_NAME = "image.png";
    public final static String DATA_URL = "/data/data/";

    
	@Override
	protected void onCreate(Bundle savedInstanceState) {

		super.onCreate(savedInstanceState);
		
		
		setContentView(R.layout.main);

		
		imageView = (ImageView) this.findViewById(R.id.imageID);
		
		String type = this.getIntent().getStringExtra("type");
		
		//�������ж��Ǵ򿪱�����ỹ��ֱ������
		if(type.equals("takePhoto"))
		{
			  Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);  
              intent.putExtra(MediaStore.EXTRA_OUTPUT, Uri.fromFile(new File(Environment.getExternalStorageDirectory(), "temp.jpg")));  
              startActivityForResult(intent, PHOTOHRAPH);  
		}else
		{
		       Intent intent = new Intent(Intent.ACTION_PICK, null);  
               intent.setDataAndType(MediaStore.Images.Media.EXTERNAL_CONTENT_URI, IMAGE_UNSPECIFIED);  
               startActivityForResult(intent, PHOTOZOOM);  
		}

    }  
 
    @Override  
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {  
        if (resultCode == NONE)  
            return;  
        // ����  
        if (requestCode == PHOTOHRAPH) {  
            //�����ļ�����·��������ڸ�Ŀ¼��  
            File picture = new File(Environment.getExternalStorageDirectory() + "/temp.jpg");  
            startPhotoZoom(Uri.fromFile(picture));  
        }  
          
        if (data == null)  
            return;  
          
        // ��ȡ�������ͼƬ  
        if (requestCode == PHOTOZOOM) {  
            startPhotoZoom(data.getData());  
        }  
        // ������  
        if (requestCode == PHOTORESOULT) {  
            Bundle extras = data.getExtras();  
            if (extras != null) {  

                Bitmap photo = extras.getParcelable("data");  
		        imageView.setImageBitmap(photo);  
         	
            	try {
            		SaveBitmap(photo);
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
            }  
  
        }  
  
        super.onActivityResult(requestCode, resultCode, data);  
    }  
  
    public void startPhotoZoom(Uri uri) {  
        Intent intent = new Intent("com.android.camera.action.CROP");  
        intent.setDataAndType(uri, IMAGE_UNSPECIFIED);  
        intent.putExtra("crop", "true");  
        // aspectX aspectY �ǿ�ߵı���  
        intent.putExtra("aspectX", 1);  
        intent.putExtra("aspectY", 1);  
        // outputX outputY �ǲü�ͼƬ���  
        intent.putExtra("outputX", 300);  
        intent.putExtra("outputY", 300);  
        intent.putExtra("return-data", true);  
        startActivityForResult(intent, PHOTORESOULT);  
    }  

    
	public void SaveBitmap(Bitmap bitmap) throws IOException {

		FileOutputStream fOut = null;
		
		//ע��1
		String path = "/mnt/sdcard/Android/data/com.xys/files";
		try {
			  //�鿴���·���Ƿ���ڣ�
			  //�����û�����·����
			  //�������·��
			  File destDir = new File(path);
			  if (!destDir.exists()) 
			  {
			  
				  destDir.mkdirs();
			  
			  }
			
			fOut = new FileOutputStream(path + "/" + FILE_NAME) ;
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}
		//��Bitmap����д�뱾��·���У�Unity��ȥ��ͬ��·������ȡ����ļ�
		bitmap.compress(Bitmap.CompressFormat.PNG, 100, fOut);
		try {
			fOut.flush();
		} catch (IOException e) {
			e.printStackTrace();
		}
		try {
			fOut.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) 
	{
		if (keyCode == KeyEvent.KEYCODE_BACK && event.getRepeatCount() == 0) 
		{
			   //���û�������ؼ��� ֪ͨUnity��ʼ��"/mnt/sdcard/Android/data/com.xys/files";·���ж�ȡͼƬ��Դ������������Unity��
			   UnityPlayer.UnitySendMessage("Main Camera","messgae",FILE_NAME);

		}
		return super.onKeyDown(keyCode, event);
	}
}
