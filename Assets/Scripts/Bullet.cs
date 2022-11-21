using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float Speed = 10.0f;

    private Rigidbody rb = null;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "bullet";
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * Speed, ForceMode.Acceleration);
         
        // destroy after 5 seconds
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
