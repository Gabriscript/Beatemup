using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class aiming : MonoBehaviour
{
    public Animator animator;
    public GameObject player;
    public List<Transform> Enmies;
    public GameObject aimer;
    Transform closest;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
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
    

}
