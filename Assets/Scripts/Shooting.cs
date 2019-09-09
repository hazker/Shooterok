using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public float power = 50f;
    public float distance = 100;
    public CharacterController Player;
    public float shellpower = 50f;
    public Animator[] Animator;
    public GameObject[] hands;
    public float[] FireRate;
    public float[] PickUpTime;
    public float[] ReloadTime;
    public Transform[] firepoint;
    public GameObject[] shellPref;
    public Transform[] shellspawn;

    private Transform currentfirepoint;
    private GameObject currentshellPref;
    private Transform currentshellspawn;
    private float currentPickUpTime;
    private float currentReloadTime;
    private float currentFireRate;
    private Animator currentAnimator;
    private GameObject currentHands;
    private RaycastHit hit;
    private GameObject shell;

    // Start is called before the first frame update
    void Start()
    {
        Player = Player.GetComponent<CharacterController>();

        //hands.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.currentState == PlayerState.WithWeapon) { 
        Vector3 forward = currentfirepoint.transform.TransformDirection(Vector3.forward) * distance;
        Debug.DrawRay(currentfirepoint.transform.position, forward, Color.green);
        //Debug.Log(Player.currentState);
        if (Input.GetMouseButtonDown(0) && Player.currentState == PlayerState.WithWeapon)
        {
            StartCoroutine(shot(currentFireRate));
        }
        if (Input.GetKeyDown(KeyCode.R) && Player.currentState == PlayerState.WithWeapon)
        {
            Player.currentState = PlayerState.ReloadWeapon;
            StartCoroutine(reload(currentReloadTime));
        }
        }
    }

    private IEnumerator shot(float currentFireRate)
    {
        Player.currentState = PlayerState.Shoot;
        currentAnimator.SetBool("Fire", true);
        Shot();
        yield return new WaitForSeconds(currentFireRate);
        currentAnimator.SetBool("Fire", false);
        Player.currentState = PlayerState.WithWeapon;
    }

    private IEnumerator PickUp(float currentPickUpTime)
    {
        currentHands.SetActive(true);
        currentAnimator.SetBool("Found", true);
        Player.currentState = PlayerState.PickingUp;
        yield return new WaitForSeconds(currentPickUpTime);
        Player.currentState = PlayerState.WithWeapon;

    }

    private IEnumerator reload(float currentReloadTime)
    {
        currentAnimator.SetBool("Reload", true);
        Player.currentState = PlayerState.ReloadWeapon;
        yield return new WaitForSeconds(currentReloadTime);
        currentAnimator.SetBool("Reload", false);
        Player.currentState = PlayerState.WithWeapon;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.CompareTag("PM"))
        {
            //Debug.Log(hands.Length);
            currentHands = hands[0];
            currentAnimator = Animator[0];
            currentFireRate = FireRate[0];
            currentPickUpTime = PickUpTime[0];
            currentReloadTime = ReloadTime[0];
            currentfirepoint = firepoint[0];
            currentshellPref = shellPref[0];
            currentshellspawn = shellspawn[0];
            Destroy(other.gameObject);
            StartCoroutine(PickUp(currentPickUpTime));
        }
        if (other.CompareTag("TOMMY"))
        {
            currentHands = hands[1];
            currentAnimator = Animator[1];
            currentFireRate = FireRate[1];
            currentReloadTime = ReloadTime[1];
            currentPickUpTime = PickUpTime[1];
            currentfirepoint = firepoint[1];
            currentshellPref = shellPref[1];
            currentshellspawn = shellspawn[1];
            Destroy(other.gameObject);
            StartCoroutine(PickUp(currentPickUpTime));
        }

    }

    private void Shot()
    {
        if (Physics.Raycast(currentfirepoint.position, -transform.forward, out hit, distance))
        {
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic && rb.constraints == RigidbodyConstraints.None)
            {
                Vector3 direction = (hit.point - transform.position).normalized;
                rb.AddForceAtPosition(direction * power, hit.point);

            }
            Debug.Log("Shoot: " + hit.transform.name);
        }
        else
        {
            Debug.Log("Miss");
        }
        shell = Instantiate(currentshellPref, currentshellspawn.position, currentshellspawn.rotation);
        shell.GetComponent<Rigidbody>().AddForce(shell.transform.right * shellpower, ForceMode.Impulse);
        Destroy(shell, 2f);

    }

}
