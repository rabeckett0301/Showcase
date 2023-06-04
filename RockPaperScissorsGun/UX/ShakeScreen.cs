using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeScreen : MonoBehaviour
{
    public float ShakeDuration;
    public float ShakeIntensity;
    public float DampingSpeed;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = this.gameObject.transform.localPosition;
    }

    void Update()
    {
        if (ShakeDuration > 0)
        {
            //Debug.Log("SHAKING");

            this.gameObject.transform.localPosition = initialPosition + Random.insideUnitSphere * ShakeIntensity;

            ShakeDuration -= Time.deltaTime * DampingSpeed;
        }
        else
        {
            ShakeDuration = 0f;
            this.gameObject.transform.localPosition = initialPosition;
        }
    }

    public void TriggerCameraShake(float duration, float intensity)
    {
        ShakeDuration = duration;
        ShakeIntensity = intensity;
    }
}
