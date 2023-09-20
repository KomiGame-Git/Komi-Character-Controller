using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KomiGroundCheckScript: MonoBehaviour
{
    public delegate void OnCollisionEventHandler<T>(T args);
    public OnCollisionEventHandler<Collision> OnCollisionEvent;

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
        
        if (this.OnCollisionEvent != null)
        {
            this.OnCollisionEvent(collision);
        }
    }
}
