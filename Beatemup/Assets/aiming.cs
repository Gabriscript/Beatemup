using System.Collections;
using System.Collections.Generic;
<<<<<<< Updated upstream
using System.Linq;
using Unity.VisualScripting;
=======
>>>>>>> Stashed changes
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class aiming : MonoBehaviour
{
    public Animator animator;
<<<<<<< Updated upstream
    public GameObject player;
    public List<Transform> Enmies;
=======
    GameObject player;
    public GameObject[] Enmies;
>>>>>>> Stashed changes
    public GameObject aimer;
    Transform closest;
    
    // Start is called before the first frame update
    void Start()
    {
<<<<<<< Updated upstream
       
=======
        Enmies = GameObject.FindGameObjectsWithTag("enemie");
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        Enmies = GameObject.FindGameObjectsWithTag("enemie").Select(go => go.transform).ToList();
        if (Enmies.Count()>0 )
        {

            Enmies = Enmies.OrderBy(x => Vector3.Distance(player.transform.position, x.transform.position)).ToList();
            aimer.transform.position = Enmies[0].transform.position;
            if(Input.GetKeyDown(KeyCode.Mouse1))
        {
                aimer.transform.position = Enmies[0].transform.position;
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
            
        
    }
    
=======
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
>>>>>>> Stashed changes

}
