using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Player PlayerClass;

    void Update()
    {
        GetComponent<Slider>().value = PlayerClass.getHealth();
    }
}
