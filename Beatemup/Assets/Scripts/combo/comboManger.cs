using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class comboManger : MonoBehaviour
{
    public float Combo;
    public float MaxCombo;
    public float MinCombo;
    public float ComboCoolOff;
    
    public Image bar;
    float timepassed = 0;

    public void Start()
    {



        
    }
    public void Update()
    {
        
        timepassed +=Time.deltaTime;
        bar.fillAmount = (ComboCoolOff - timepassed) / ComboCoolOff;

        if(Input.GetKeyUp(KeyCode.Space)) 
        {

            timepassed=0;

        }

    }

    
}
