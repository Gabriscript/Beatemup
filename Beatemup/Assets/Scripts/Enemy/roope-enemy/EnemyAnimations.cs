using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    // Access the animation through the triggers set in the animator

    Animator anim;
    public float timer = 0f;
    public float aimTime = 2f;
    public bool coolDown = false;

    public GameObject projectile;
    public GameObject firestart;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            anim.SetTrigger("EnemyAttack");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetTrigger("EnemyShoot");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            anim.speed = 0;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            anim.speed = 1;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger("EnemyGetHit");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            anim.SetTrigger("EnemyFall");
        }

        if (coolDown == true)
        {
            CooldownTimer();
        }
    }


    public void StartSlowdown()
    {
        anim.speed = 0f;
        coolDown = true;
    }

    public void CooldownTimer()
    {
        timer += Time.deltaTime;
        if(timer >= aimTime)
        {
            Instantiate(projectile, firestart.transform.position, firestart.transform.rotation);
            anim.speed = 1f;
            coolDown = false;
            timer = 0f;
        }
        
    }
}
