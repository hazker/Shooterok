using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public float power = 50f;
    public float distance = 100;
    public Transform firepoint;

    private RaycastHit hit;

    public GameObject shellPref;
    public Transform  shellspawn;
    
    private GameObject shell;
    public float shellpower = 1f;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 forward = firepoint.transform.TransformDirection(Vector3.forward) * distance;
        Debug.DrawRay(firepoint.transform.position, forward, Color.green);
        if (Input.GetMouseButtonDown(0))
        {
            Shot();
            shellout();
        }
    }
    private void Shot()
    {
        if(Physics.Raycast(firepoint.position, -transform.forward, out hit, distance))
        {
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            if(rb!=null && !rb.isKinematic && rb.constraints == RigidbodyConstraints.None)
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
        
    }
    private void shellout()
    {
        shell = Instantiate(shellPref, shellspawn.position, shellspawn.rotation);
        shell.GetComponent<Rigidbody>().AddForce(shell.transform.right * shellpower, ForceMode.Impulse);
        Destroy(shell, 2f);
    }
}
