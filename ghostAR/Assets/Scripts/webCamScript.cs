using System.Collections;
using System.Collections.Generic;

using UnityEngine;
public class webCamScript : MonoBehaviour {

    public GameObject webCameraPlane;

    void Start() {
        if (Application.isMobilePlatform)
        {
            GameObject cameraParent = new GameObject("camParent");
            cameraParent.transform.position = this.transform.position;
            this.transform.parent = cameraParent.transform;
            cameraParent.transform.Rotate(Vector3.right, 90);
        }

        Input.gyro.enabled = true;

        WebCamTexture webCamTexture = new WebCamTexture();
        webCameraPlane.GetComponent<MeshRenderer>().material.mainTexture = webCamTexture;
        webCamTexture.Play();

    }
	
	void Update () {
        Quaternion cameraRotation = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        this.transform.localRotation = cameraRotation;
	}

    /// <summary>
    /// Turns off pretty much everything attached to this script and it's gameObject.
    /// </summary>
    public void TurnOffEverything () {
        WebCamTexture webCamTexture = webCameraPlane.GetComponent<MeshRenderer>().material.mainTexture as WebCamTexture;

        webCamTexture.Stop();
        webCameraPlane.SetActive(false);
    }
}
