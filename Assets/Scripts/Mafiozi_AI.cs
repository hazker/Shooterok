using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Mafiozi_AI : MonoBehaviour
{
    public float DistancetoFire;
    public float DistancetoWalk;
    public float DistancetoRun;
    public float rasstoyanie;

    public float healf = 100;
    //public GameObject head_collider;
    //public GameObject body_collider;
    //public GameObject legs_collider;

    public GameObject Player;
    public GameObject Ragdoll;
    public GameObject mesh;
    public GameObject my_weapon;
    public GameObject my_hat;
    public GameObject my_hat_resp_pos;
    public GameObject my_weapon_resp_pos;
    public Animator anim;

    public float hit_in;

    private bool stay = false;
    private bool fire = false;

    private UnityEngine.AI.NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(Player.transform.position, transform.position);
        rasstoyanie = distance;

        if(healf<=0){
            dead();
            }

        if(!stay)
        {
            if (distance <= DistancetoRun && distance > DistancetoWalk && hit_in ==0)
            {
                //подходит
                agent.speed=8f;
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
                anim.SetBool("shoot", false);
                agent.SetDestination(Player.transform.position);

            }

            if (distance < DistancetoWalk && distance > DistancetoFire && hit_in ==0)
            {
                agent.speed=4f;
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                anim.SetBool("shoot", false);
                agent.SetDestination(Player.transform.position);

            }


            if (distance <= DistancetoFire && hit_in ==0)
            {
                //стреляет
                stay = true;
                fire = true;
                transform.LookAt(Player.transform.position);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y,0);
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
                anim.SetBool("shoot", true);
                agent.SetDestination(transform.position);
            
            }

        }else
            {
            if (distance > DistancetoRun && hit_in ==0)
                {
                    //стреляет
                    stay = false;
                    transform.LookAt(Player.transform.position);
                    anim.SetBool("Walk", true);
                    anim.SetBool("Run", false);
                    anim.SetBool("shoot", false);
                    agent.SetDestination(Player.transform.position);
            
                }

            if(fire)
                {
                    anim.SetBool("shoot", true);
                    transform.LookAt(Player.transform.position);
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y,0);
                    StartCoroutine(whait_shoot());
                } 
            }
 
          if (hit_in == 2)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            anim.SetBool("shoot", false);
            Debug.Log("Hit_body");
            agent.SetDestination(transform.position);
            StartCoroutine(whait_hit_medium());
            agent.SetDestination(transform.position);
           // hit_in = 0;
        }

        if (hit_in == 1)
        {
            healf -=100;
           // hit_in = 0;    
        }

        if (hit_in == 3)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            anim.SetBool("shoot", false);
            agent.SetDestination(transform.position);
            StartCoroutine(whait_hit_low());
            agent.SetDestination(transform.position);
        }
        
    }

    void dead(){
        mesh.SetActive(false);
        Ragdoll.SetActive(true);
        Instantiate(Ragdoll, transform.position,transform.rotation);
        my_weapon.SetActive(true);
        Instantiate(my_weapon, my_weapon_resp_pos.transform.position,my_weapon_resp_pos.transform.rotation);
        my_hat.SetActive(true);
        Instantiate(my_hat, my_hat_resp_pos.transform.position,my_hat_resp_pos.transform.rotation);
    }

      IEnumerator whait_hit_low(){
        anim.SetBool("Hit_low", true);
        agent.SetDestination(transform.position);
         yield return new WaitForSeconds(2.5f);
         anim.SetBool("Hit_low", false);
         hit_in = 0;
    }

     IEnumerator whait_hit_medium(){
        anim.SetBool("Hit_middle", true);
        agent.SetDestination(transform.position);
         yield return new WaitForSeconds(2.5f);
         anim.SetBool("Hit_middle", false);
         hit_in = 0;
    }

     IEnumerator whait_shoot(){
         yield return new WaitForSeconds(0.5f);
     
     }

}
