using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class comboManger : MonoBehaviour
{
    [Header("comboSettings")]
    public int Combo=0;
    public float ComboCoolOff;
    public string[] comboranks;
    public int hitsNeeded;
    public int currenthits=0;
    
    
    public Image bar;
    public Text combo;
    public float timepassed = 0;
    public bool comboInitiate = false;

    public void Start()
    {



        
    }
    public void Update()
    {

        ComboCalculations();
        ComboSystem();

    }

    public void ComboSystem()
    {
        timepassed += Time.deltaTime;
        bar.fillAmount = (ComboCoolOff - timepassed) / ComboCoolOff;
        combo.text = (comboranks[Combo]);
        if (comboInitiate)
        {

            timepassed = 0;
            if (Combo<comboranks.Length-1) 
            {
                Combo += 1;
            }
            comboInitiate= false;
        }

        if (timepassed >= ComboCoolOff)
        {
            if (Combo>0)
            {
                Combo -= 1;
            }
            timepassed = 0;
        }
    }

    public void ComboCalculations()
    {
        if (hitsNeeded<=currenthits) 
        {
            comboInitiate= true;
            currenthits= 0;
        }


    }

    public void comboup()
    {
        
    }


    
}
