using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] Behaviour[] componentsToDisable;

    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            for(int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else
        {
            camera = Camera.main;
            if (camera != null)
            {
                camera.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void OnDisable()
    {
        if (camera != null)
        {
            camera.gameObject.SetActive(true);
        }
    }
}
