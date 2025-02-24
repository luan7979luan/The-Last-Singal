using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killCam : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCameraBase killcam;
    public void EnableKillCam()
    {
        killcam.Priority = 20;
    }
}
