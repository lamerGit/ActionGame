using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSync : MonoBehaviour
{
    private Camera mycamera;
    public Camera cameraToSync;

    private void Awake()
    {
        mycamera = GetComponent<Camera>();
    }

    private void Update()
    {
        mycamera.fieldOfView= cameraToSync.fieldOfView;
        mycamera.transform.position = cameraToSync.transform.position;
        mycamera.transform.rotation = cameraToSync.transform.rotation;
    }

}
