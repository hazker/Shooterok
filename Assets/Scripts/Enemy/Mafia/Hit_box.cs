using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_box : MonoBehaviour
{
    public GameObject mesh;
    public bool hit;
    public float dmg;
    public float iam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(hit){
         Mafiozi_AI ai = mesh.transform.GetComponent<Mafiozi_AI>();
         ai.healf -= dmg;
         ai.hit_in = iam;  
         hit = false; 
        }
    }
}
