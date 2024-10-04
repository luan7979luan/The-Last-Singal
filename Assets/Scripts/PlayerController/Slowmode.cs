using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowmode : MonoBehaviour
{

    public float slowdownto = 0.5f;
    public float Slowdowntime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale += (1f / Slowdowntime) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, slowdownto, 1f);
        if( Input.GetKeyDown(KeyCode.E))
        {
                MakeSlowMotionEffect() ;
            
        } 


    }

    public void MakeSlowMotionEffect()
    {
        Time.timeScale = slowdownto;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

  
}
