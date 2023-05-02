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
    
    
  
    public float timepassed = 0;
    public bool comboInitiate = false;
    public GameObject[] comboImages;
    public Image[] fill;
    

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
        fill[Combo].fillAmount = (ComboCoolOff - timepassed) / ComboCoolOff;
        comboImages[Combo].SetActive(true);
        if (comboInitiate)
        {

            timepassed = 0;
            if (Combo<comboranks.Length-1) 
            {
                comboImages[Combo].SetActive(false);
                Combo += 1;
            }
            comboInitiate= false;
        }

        if (timepassed >= ComboCoolOff)
        {
            if (Combo>0)
            {
                comboImages[Combo].SetActive(false);
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
