using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deletin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(del());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator del(){
         yield return new WaitForSeconds(10f);
         Destroy(gameObject);
    }
}
