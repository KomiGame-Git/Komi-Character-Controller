using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlockScript : MonoBehaviour
{

    private float SumTime { set; get; }
    [SerializeField]
    private Vector3 SetMoveVector;
    private Vector3 vector;
    private Rigidbody MoveRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            this.MoveRigidbody = GetComponent<Rigidbody>();
        }
        else
        {
            this.MoveRigidbody = gameObject.AddComponent<Rigidbody>() as Rigidbody;
            this.MoveRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void FixedUpdate()
    {
        this.SumTime += Time.fixedDeltaTime;
        //Debug.Log($"Sin:{Mathf.Sin(2 * this.SumTime)} Cos:{Mathf.Sin(this.SumTime)}");
        this.vector = this.SetMoveVector * (Mathf.Sin(this.SumTime) >= 0 ? 1 : -1);
        
        this.MoveRigidbody.AddForce(this.vector * Mathf.Sin(2 * this.SumTime));
    }
}
