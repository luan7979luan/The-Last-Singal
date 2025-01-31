using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    private Transform _playerCamera;

    void Start()
    {
        // Lấy transform của camera chính (camera người chơi)
        _playerCamera = Camera.main.transform;
    }

    void Update()
    {
        // Xoay thanh máu để luôn hướng về camera
        transform.LookAt(transform.position + _playerCamera.forward);
    }
}
