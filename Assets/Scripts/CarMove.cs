using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{

    public float vertical = Input.GetAxis("Vertical");
    float horizontal = Input.GetAxis("Horizontal");

    public float currentSpeed;

    float accelerationToForward;
    float accelerationToBack;

    float maxSpeedToForward;
    float maxSpeedToBack;


    float autoStopping;

    private void Awake()
    {
        currentSpeed = 0.0f;

        accelerationToForward = 2.0f;
        accelerationToBack = -1.0f;

        maxSpeedToForward = 65.0f;
        maxSpeedToBack = -20.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Drive();
    }

    void Drive()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");


        if (vertical > 0.0f) GoForward();
        if (vertical < 0.0f) GoBack();
        if (vertical == 0 && (currentSpeed <= maxSpeedToForward || currentSpeed >= maxSpeedToBack)) { }
    }


    void GoForward()
    {
        if (currentSpeed <= maxSpeedToForward) { 
            currentSpeed += accelerationToForward;
            transform.Translate(Vector3.forward * vertical * Time.deltaTime * currentSpeed);
        }
        else
            transform.Translate(Vector3.forward * vertical * Time.deltaTime );

    }

    void GoBack()
    {

    }

}
