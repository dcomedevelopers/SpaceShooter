using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float Speed = 5f;

    [SerializeField]
    private float AngSpeed = 20.0f;

    [SerializeField]
    private float maxSpeed = 5f;

    private float health = 5f;

    private bool canGetDamage = true;

    public GameObject BulletClass;

    private Rigidbody rb = null;

    private Color InitColor;
    
    public delegate void OnHitByMeteor(Meteor meteor);
    public static OnHitByMeteor onHitByMeteor;


    void Start()
    {
        gameObject.tag = "player";
        rb = GetComponent<Rigidbody>();
        InvokeRepeating("Shoot", 0.1F, 0.1F);
        InitColor = GetComponent<Renderer>().material.color;
        onHitByMeteor += HittedByMeteor;

    }
    public float getHealth()
    {
        return health;
    }
    void HittedByMeteor(Meteor meteor)
    {
        if(canGetDamage)
        {
            rb.detectCollisions = false;
            canGetDamage = false;
            health -= meteor.getIndex();

            if (health <= 0)
            {
                Destroy(gameObject);
                Game.onGameEnd?.Invoke();
            } 
            else
            {
                StartCoroutine(BlinkGameObject());
                StartCoroutine(HittedByMeteorCallback());
            }
        }
    }

    public IEnumerator HittedByMeteorCallback()
    {
        yield return new WaitForSeconds(3);
        rb.detectCollisions = true;
        canGetDamage = true;
    }

    public IEnumerator BlinkGameObject()
    {
        // (0.1s+0.1s) * 15 = 3 sec
        for (int i = 0; i < 15; i++)
        {
            GetComponent<Renderer>().material.color = new Color(255, 0, 0);
            yield return new WaitForSeconds(0.1f);
            GetComponent<Renderer>().material.color = InitColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Rotate()
    {
        float hAxis = Input.GetAxis("Horizontal");
        transform.Rotate(0, hAxis * AngSpeed * Time.deltaTime, 0);
    }

    void Acceleration()
    {
       float vAxis = Input.GetAxis("Vertical");
       rb.AddForce(transform.forward * Speed * vAxis, ForceMode.Acceleration);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(BulletClass, transform.position + transform.forward, transform.rotation);
    }


    // Update is called once per frame
    void Update()
    {
        Rotate();
        Acceleration();

        Debug.DrawRay(transform.position, transform.forward * 2, Color.red, 0f, true);

    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
