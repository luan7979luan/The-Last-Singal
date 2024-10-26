using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkenScreenAndDisableVFX : MonoBehaviour
{
    public Image darkScreenOverlay; // Overlay Image để làm tối màn hình (cần gán sẵn trên UI Canvas)
    public GameObject[] vfxObjects; // Array các VFX cần tắt
    
    private bool isDarkened = false;

    void Start()
    {
        if (darkScreenOverlay != null)
        {
            darkScreenOverlay.color = new Color(0, 0, 0, 0); // Đảm bảo ban đầu màn hình không bị tối
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleDarkScreenAndVFX();
        }
    }

    void ToggleDarkScreenAndVFX()
    {
        isDarkened = !isDarkened;

        if (darkScreenOverlay != null)
        {
            darkScreenOverlay.color = isDarkened ? new Color(0, 0, 0, 0.5f) : new Color(0, 0, 0, 0);
        }

        foreach (GameObject vfx in vfxObjects)
        {
            if (vfx != null)
            {
                vfx.SetActive(!isDarkened);
            }
        }
    }
}
