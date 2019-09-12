using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mafiozi_AI : MonoBehaviour
{
    public float DistancetoFire;
    public float DistancetoWalk;
    public float DistancetoRun;
    public float rasstoyanie;

    public GameObject head_collider;
    public GameObject body_collider;
    public GameObject legs_collider;

    public GameObject Player;
    public GameObject Ragdoll;
    public GameObject mesh;
    public Animator anim;

    public bool hit_inleg;
    public bool hit_inbody;
    public bool hit_inhead;

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
        if(distance > DistancetoRun) { return; }

        if (distance <= DistancetoRun && distance > DistancetoWalk)
        {
            //подходит
            agent.speed=8f;
            anim.SetBool("Run", true);
            anim.SetBool("Walk", false);
            anim.SetBool("shoot", false);
            agent.SetDestination(Player.transform.position);

        }

        if (distance < DistancetoWalk && distance > DistancetoFire)
        {
            agent.speed=3f;
            anim.SetBool("Walk", true);
            anim.SetBool("Run", false);
            anim.SetBool("shoot", false);
            agent.SetDestination(Player.transform.position);

        }


            if (distance < DistancetoFire)
        {
            //стреляет
            dead();
            transform.LookAt(Player.transform.position);
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            anim.SetBool("shoot", true);
            agent.SetDestination(transform.position);

        }

          if (hit_inbody)
        {
            anim.SetBool("Hit_middle", true);
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            anim.SetBool("shoot", false);
            agent.SetDestination(transform.position);

        }

        if (hit_inhead)
        {
            return;
        }

        if (hit_inleg)
        {
            anim.SetBool("Hit_low", true);
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            anim.SetBool("shoot", false);
            agent.SetDestination(transform.position);

        }

    }

    void dead(){
        mesh.SetActive(false);
        Ragdoll.SetActive(true);
        Instantiate(Ragdoll, transform.position,transform.rotation);
    }
}
