using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KomiGroundCheckScript: MonoBehaviour
{

    public Action<Collision> CheckAction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(CheckAction != null)
        {
            this.CheckAction(collision);
        }
    }
}
