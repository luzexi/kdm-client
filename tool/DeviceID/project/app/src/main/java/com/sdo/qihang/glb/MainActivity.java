package com.sdo.qihang.glb;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;

import com.sdo.qihang.lib.DeviceUtils;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        DeviceUtils.getUniqueId();
    }
}
