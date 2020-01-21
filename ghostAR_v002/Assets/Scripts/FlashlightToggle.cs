using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FlashlightToggle : MonoBehaviour {
    private bool Active;
    private AndroidJavaObject camera1;

    void FL_Start () {
        AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");
        WebCamDevice[] devices = WebCamTexture.devices;

        int camID = 0;
        camera1 = cameraClass.CallStatic<AndroidJavaObject>("open", camID);

        if (camera1 != null) {
            AndroidJavaObject cameraParametes = camera1.Call<AndroidJavaObject>("getParameters");
            cameraParametes.Call("setFlashMode", "torch");
            camera1.Call("setParameters", cameraParametes);
            camera1.Call("startPreview");
            Active = true;
        } else {
            Debug.LogError("[CameraParametersAndroid] Camera not available");
        }
    }

    private void OnDestroy() {
        FL_Stop();
    }

    void FL_Stop () {
        if (camera1 != null) {
            camera1.Call("stopPreview");
            camera1.Call("release");
            Active = false;
        } else {
            Debug.LogError("[CameraParametersAndroid] Camera not available");
        }
    }

    private void OnGUI() {
        GUILayout.BeginArea(new Rect(Screen.width * 0.1f, Screen.height * 0.1f, Screen.width * 0.3f, Screen.height * 0.1f));
        if (!Active) {
            if (GUILayout.Button("ENABLE FLASHLIGHT"))
                FL_Start();
        } else {
            if (GUILayout.Button("DISABLE FLASHLIGHT"))
                FL_Stop();
        }
        GUILayout.EndArea();
    }
}