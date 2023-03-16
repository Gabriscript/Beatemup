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
        combo.text = (comboranks[Combo]);
        if (Input.GetKeyUp(KeyCode.Space))
        {

            timepassed = 0;
            if (Combo<comboranks.Length-1) 
            {
                Combo += 1;
            }
            
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

    public void ComboRank()
    {



    }




    
}
