using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanEffect : MonoBehaviour
{
    [SerializeField]
    public float maxX = 16f;
    [SerializeField]
    public float maxZ = 10f;
    [SerializeField]
    public bool isActive = false;

    void PacManEffect()
    {

        float currX = transform.position.x;
        float currY = transform.position.y;
        float currZ = transform.position.z;

        if (currX > maxX)
        {
            transform.position = new Vector3(-maxX, currY, -currZ);
        }
        else if (currX < -maxX)
        {
            transform.position = new Vector3(maxX, currY, -currZ);
        }

        if (currZ > maxZ)
        {
            transform.position = new Vector3(-currX, currY, -maxZ);
        }
        else if (currZ < -maxZ)
        {
            transform.position = new Vector3(-currX, currY, maxZ);
        }
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            PacManEffect();
        }
    }
}
