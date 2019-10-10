using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    
    public float power = 50f;
    public float distance = 100;
    public PlayerController Player;
    public float shellpower = 50f;
    public int NumberOfWeapons;
    public Animator[] Animator;
    public GameObject[] hands;
    public int[] MagasinSize;
    public float[] FireRate;
    public float[] PickUpTime;
    public float[] ReloadTime;
    public Transform[] firepoint;
    public GameObject[] shellPref;
    public Transform[] shellspawn;
    public Text Hp;
    public Text Armor;
    public Text AmmoText;

    private int[] currentAmmo;
    private int[] weapons;
    private int currentWeapon;
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
        Player = Player.GetComponent<PlayerController>();
        weapons = new int[NumberOfWeapons];
        currentAmmo = new int[NumberOfWeapons];
        AmmoText.text = "";
        Hp.text = "100";
        Armor.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.currentState == PlayerState.WithWeapon) { 
        //Debug.Log(Player.currentState);
        /*if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(shot(currentFireRate));
        }*/
            if (Input.GetMouseButtonDown(0))
            {
                if(currentAmmo[currentWeapon] > 0)
                {
                    StartCoroutine(shot(currentFireRate));
                }
                else
                {
                    Player.currentState = PlayerState.ReloadWeapon;
                    currentAmmo[currentWeapon] = MagasinSize[currentWeapon];
                    AmmoText.text = currentAmmo[currentWeapon] + "/" + MagasinSize[currentWeapon];
                    StartCoroutine(reload(currentReloadTime));
                }
            }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Player.currentState = PlayerState.ReloadWeapon;
            currentAmmo[currentWeapon] = MagasinSize[currentWeapon];
            AmmoText.text = currentAmmo[currentWeapon] + "/" + MagasinSize[currentWeapon];
            StartCoroutine(reload(currentReloadTime));
        }
            if (Input.GetKeyDown(KeyCode.Alpha1) && weapons[0]==1)
            {
                PmSetup();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && weapons[1] == 1)
            {
                TommySetup();
            }
        }
    }

    private IEnumerator shot(float currentFireRate)
    {
        Player.currentState = PlayerState.Shoot;
        currentAnimator.SetBool("Fire", true);
        //Shot();
        yield return new WaitForSeconds(currentFireRate);
        currentAnimator.SetBool("Fire", false);
        Player.currentState = PlayerState.WithWeapon;
    }

    private IEnumerator PickUp(float currentPickUpTime)
    {
        currentHands.SetActive(true);
        Player.currentState = PlayerState.PickingUp;
        yield return new WaitForSeconds(currentPickUpTime);
        currentAnimator.SetBool("Found", false);
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

    private void PmSetup()
    {
        if (Player.currentState == PlayerState.WithWeapon)
        {
            currentHands.SetActive(false);
        }
        Player.currentState = PlayerState.WithWeapon;
        currentWeapon = 0;
        currentHands = hands[currentWeapon];
        currentAnimator = Animator[currentWeapon];
        currentFireRate = FireRate[currentWeapon];
        currentPickUpTime = PickUpTime[currentWeapon];
        currentReloadTime = ReloadTime[currentWeapon];
        currentfirepoint = firepoint[currentWeapon];
        currentshellPref = shellPref[currentWeapon];
        currentshellspawn = shellspawn[currentWeapon];
        StartCoroutine(PickUp(currentPickUpTime));
        AmmoText.text = currentAmmo[currentWeapon] +"/"+ MagasinSize[currentWeapon];
        weapons[currentWeapon] = 1;
    }

    private void TommySetup()
    {
        if (Player.currentState == PlayerState.WithWeapon)
        {
            currentHands.SetActive(false);
        }
        Player.currentState = PlayerState.WithWeapon;
        currentWeapon = 1;
        currentHands = hands[currentWeapon];
        currentAnimator = Animator[currentWeapon];
        currentFireRate = FireRate[currentWeapon];
        currentPickUpTime = PickUpTime[currentWeapon];
        currentReloadTime = ReloadTime[currentWeapon];
        currentfirepoint = firepoint[currentWeapon];
        currentshellPref = shellPref[currentWeapon];
        currentshellspawn = shellspawn[currentWeapon];
        StartCoroutine(PickUp(currentPickUpTime));
        AmmoText.text = currentAmmo[currentWeapon] + "/" + MagasinSize[currentWeapon];
        weapons[currentWeapon] = 1;
    }

    private void ArSetup()
    {
        if (Player.currentState == PlayerState.WithWeapon)
        {
            currentHands.SetActive(false);
        }
        Player.currentState = PlayerState.WithWeapon;
        currentWeapon = 2;
        currentHands = hands[currentWeapon];
        currentAnimator = Animator[currentWeapon];
        currentFireRate = FireRate[currentWeapon];
        currentPickUpTime = PickUpTime[currentWeapon];
        currentReloadTime = ReloadTime[currentWeapon];
        currentfirepoint = firepoint[currentWeapon];
        currentshellPref = shellPref[currentWeapon];
        currentshellspawn = shellspawn[currentWeapon];
        StartCoroutine(PickUp(currentPickUpTime));
        AmmoText.text = currentAmmo[currentWeapon] + "/" + MagasinSize[currentWeapon];
        weapons[currentWeapon] = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if((Player.currentState == PlayerState.WithoutWeapon || Player.currentState == PlayerState.WithWeapon)) { 
        Debug.Log(other);
        if (other.CompareTag("PM") )
        {
            currentAmmo[0] = 8;
            Destroy(other.gameObject);
            PmSetup();
        }
        if (other.CompareTag("TOMMY"))
        {
            currentAmmo[1] = 50;
            Destroy(other.gameObject);
            TommySetup();
        }
        if (other.CompareTag("Helmet"))
        {
            Destroy(other.gameObject);
            Armor.text="25";
        }
            if (other.CompareTag("Ar"))
            {
                currentAmmo[2] = 30;
                Destroy(other.gameObject);
                ArSetup();
            }
        }
    }

    private void Shot()
    {
        currentAmmo[currentWeapon]--;
        AmmoText.text = currentAmmo[currentWeapon] + "/" + MagasinSize[currentWeapon];
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray,out hit, distance))
        {
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic && rb.constraints == RigidbodyConstraints.None)
            {
                
                rb.AddForceAtPosition(ray.direction * power, hit.point);

            }

            if (hit.transform.tag == "Mafia_Enemy")
            {
                Hit_box coll = hit.transform.GetComponent<Hit_box>();
                //урон от пушки, надо массив уронов добавить
                coll.dmg = 20f;
                coll.hit = true;
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
