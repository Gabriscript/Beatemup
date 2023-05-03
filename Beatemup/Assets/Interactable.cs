using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.SceneManagement;


public class Interactable : MonoBehaviour
{
    public bool IsInrange=false;
    public GameObject text;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
           IsInrange= true;
            text.SetActive(true);
            Debug.Log("i am in");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
           IsInrange= false;
            text.SetActive(false);
            Debug.Log("i left");
        }
    }

    public void Update()
    {
        if (IsInrange) 
        {
            if(Input.GetKeyUp(KeyCode.F)) 
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            }
        }
        
    }

    

}
