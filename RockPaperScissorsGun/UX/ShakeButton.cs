using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeButton : MonoBehaviour
{
    private Vector3 initialPosition;
    public float ShakeIntensity;


    void Start()
    {
        initialPosition = this.gameObject.transform.localPosition;
    }


    void Update()
    {
        this.gameObject.transform.localPosition = initialPosition + Random.insideUnitSphere * ShakeIntensity;
    }
}
