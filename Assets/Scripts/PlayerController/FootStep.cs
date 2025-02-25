using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    public AudioSource footStepsSound, jumpSound, sprintsound, fallLandSound;

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                footStepsSound.enabled = false;
                sprintsound.enabled = true; 
            }
            else
            {
                footStepsSound.enabled = true;
                sprintsound.enabled = false;
            }
        }
        
        else
        {
            footStepsSound.enabled=false;
            sprintsound.enabled = false;
        }
    }
}
