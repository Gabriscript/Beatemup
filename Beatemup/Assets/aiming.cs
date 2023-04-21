using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class aiming : MonoBehaviour
{
    public Animator animator;
    GameObject player;
    public GameObject[] Enmies;
    public GameObject aimer;
    Transform closest;
    
    // Start is called before the first frame update
    void Start()
    {
        Enmies = GameObject.FindGameObjectsWithTag("enemie");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1)) 
        {
            animator.SetBool("aiming", true);
            Debug.Log("i am aiming!");
            //closest=GetClosestEnemy(Enmies);
            //aimer.transform.position= closest;
            
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            animator.SetBool("aiming", false);
            
        }
    }
    Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = player.transform.position;
        foreach (Transform t in enemies)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin.transform;
    }

}
