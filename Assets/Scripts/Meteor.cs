using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private float index = 0f;

    private Rigidbody rb = null;

    public delegate void OnMeteorDestroyed(Meteor destroyedMeteor);
    public static OnMeteorDestroyed onMeteorDestroyed;

    void Start()
    {
        gameObject.tag = "meteor";
        rb = GetComponent<Rigidbody>();
    }

    public void setIndex(float index)
    {
        this.index = index;
    }

    public float getIndex()
    {
        return this.index;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            StartCoroutine(onDestroyAnimation(collision.gameObject.GetComponent<Bullet>()));
        }
        if (collision.gameObject.tag == "player")
        {
            Player.onHitByMeteor?.Invoke(this);
        }
            
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 4, Color.red, 0f, true);
    }

    IEnumerator onDestroyAnimation(Bullet bullet)
    {
        Destroy(bullet.gameObject);
        Destroy(gameObject.transform.GetChild(0).gameObject);
        ParticleSystem ps = gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        ps?.Play();
        rb.detectCollisions = false;
        yield return new WaitForSeconds(0.1f);
        onMeteorDestroyed?.Invoke(this);
        yield return new WaitForSeconds(0.5f);
        Destroy(ps);
        Destroy(gameObject);
    }
}
