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

    private GameObject my_hatp;
    private bool stay = false;
    private bool fire = false;
    private bool i_see = false;
    private bool spalil = false;
    private bool head_shoot = false;

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

        if(healf<=50){
            Run_rundom();
            }

        if(!stay)
        {
            if ((distance <= DistancetoRun && distance > DistancetoWalk && hit_in ==0) || spalil)
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
                spalil = false;
                agent.speed=4f;
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                anim.SetBool("shoot", false);
                agent.SetDestination(Player.transform.position);
                StartCoroutine(whait_shoot());
            }

            if(!i_see && hit_in ==0 && distance <= DistancetoFire){
                 agent.speed=4f;
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                anim.SetBool("shoot", false);
                agent.SetDestination(Player.transform.position);
                StartCoroutine(whait_shoot());
            }

            if (distance <= DistancetoFire && hit_in ==0 && i_see)
            {
                
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
            chek(distance);
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
            head_shoot = true;
           // hit_in = 0;    
        }

        if (hit_in == 3)
        {
             chek(distance);
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            anim.SetBool("shoot", false);
            agent.SetDestination(transform.position);
            StartCoroutine(whait_hit_low());
            agent.SetDestination(transform.position);
        }
        
    }

    void Run_rundom(){
        //убегает
    }

    void dead(){
        Destroy(mesh);
        Ragdoll.SetActive(true);
        Instantiate(Ragdoll, transform.position,transform.rotation);
        my_weapon.SetActive(true);
        Instantiate(my_weapon, my_weapon_resp_pos.transform.position,my_weapon_resp_pos.transform.rotation);
        my_hat.SetActive(true);
        my_hatp = Instantiate(my_hat, my_hat_resp_pos.transform.position,my_hat_resp_pos.transform.rotation);
        Debug.Log(my_hat.transform.up);
        if(head_shoot)
        my_hatp.GetComponent<Rigidbody>().AddForce(my_hatp.transform.forward * 40f, ForceMode.Impulse);
    }

    void chek(float distance){
        if(distance > DistancetoRun)
            spalil = true;
            stay = false;
       }

      IEnumerator whait_hit_low(){
          stay = true;
        anim.SetBool("Hit_low", true);
        agent.SetDestination(transform.position);
         yield return new WaitForSeconds(2.5f);
         anim.SetBool("Hit_low", false);
         stay = false;
         hit_in = 0;
    }

     IEnumerator whait_hit_medium(){
         stay = true;
        anim.SetBool("Hit_middle", true);
        agent.SetDestination(transform.position);
         yield return new WaitForSeconds(2.5f);
         anim.SetBool("Hit_middle", false);
         stay = false;
         hit_in = 0;
    }

     IEnumerator whait_shoot(){
         yield return new WaitForSeconds(0.5f);
         RaycastHit hit;
         Ray ray = new Ray(my_weapon_resp_pos.transform.position, Player.transform.position - my_weapon_resp_pos.transform.position);
         //пускаем луч
         Physics.Raycast(ray, out hit);
         //если луч с чем-то пересёкся, то..
            if (hit.collider != null){
         //если луч не попал в цель
                if (hit.transform.name != "Player"){
                Debug.Log("Путь к врагу преграждает объект: "+hit.collider.name);
                i_see = false;
                stay = false;

            }   
            //если луч попал в цель
            else{
            Debug.Log("Попадаю во врага!!!");
            i_see = true;
            }
         //просто для наглядности рисуем луч в окне Scene
         Debug.DrawLine(ray.origin, hit.point,Color.red);
        }
     }

}
