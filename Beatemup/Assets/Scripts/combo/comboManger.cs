using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class comboManger : MonoBehaviour
{
    public float Combo=0;
    public float MaxCombo;
    public float MinCombo;
    public float ComboCoolOff;
    
    
    public Image bar;
    public Text combo;
    public float timepassed = 0;

    public void Start()
    {



        
    }
    public void Update()
    {

        ComboSystem();

    }

    public void ComboSystem()
    {
        timepassed += Time.deltaTime;
        bar.fillAmount = (ComboCoolOff - timepassed) / ComboCoolOff;
        combo.text = ("Combo" + Combo);
        if (Input.GetKeyUp(KeyCode.Space))
        {

            timepassed = 0;
            Combo += 1;
        }

        if (timepassed >= ComboCoolOff)
        {
            Combo -= 1;
            timepassed = 0;
        }
    }
    
}
