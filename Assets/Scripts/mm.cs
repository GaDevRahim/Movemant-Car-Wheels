using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mm : MonoBehaviour
{
    GameObject car;
    GameObject[] wheel = new GameObject[4];
    GameObject parentFrontLeftWheel;
    GameObject ParentFrontRightWheel;
    Vector3 Distance;

    public float x;

    public float speedToAhead;
    float accelerateAhead;
    float maxAheadSpeed;

    float autoAccelerateStopping;
    float carBrakes;

    public float speedToBack;
    float accelerateBack;
    float maxBackSpeed;

    public float speedOfRotation;

    float maxTurnWheels;
    public float currentTurnWheels;
    Vector3 angles;

    private void Awake()
    {
        car = GameObject.Find("Car");
        parentFrontLeftWheel = GameObject.Find("/Car/Front_Left_Wheel");
        ParentFrontRightWheel = GameObject.Find("/Car/Front_Right_Wheel");
        setWheel();
        Distance = new Vector3();

        speedToAhead = 0.0f;
        accelerateAhead = 0.15f;
        maxAheadSpeed = 50.0f;

        autoAccelerateStopping = 0.2f;
        carBrakes = autoAccelerateStopping * 4.0f;



        speedToBack = 0.0f;
        accelerateBack = 0.15f;
        maxBackSpeed = 15.0f;

        speedOfRotation = 2000.0f;

        maxTurnWheels = 50.0f;
        currentTurnWheels = 0.0f;
        angles = new Vector3();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // see what user input
        Drive(); 
    }

    void LateUpdate()
    {
        Steering();
        TurnWheel();


        
    }

    // create wheels 
    void setWheel()
    {
        wheel[0] = GameObject.Find("/Car/Front_Left_Wheel/wheel_fl");
        wheel[1] = GameObject.Find("/Car/Front_Right_Wheel/wheel_fr");
        wheel[2] = GameObject.Find("/Car/wheel_rl");
        wheel[3] = GameObject.Find("/Car/wheel_rr");
    }


    // condition while press key to drive
    void Drive()
    {

        if (Input.GetKey(KeyCode.D) && speedToAhead > 1.0f) rotationToAheadRight();
        if (Input.GetKey(KeyCode.A) && speedToAhead > 1.0f) rotationToAheadLeft();
        if (Input.GetKey(KeyCode.D) && speedToBack > 1.0f) rotationToBackLeft();
        if (Input.GetKey(KeyCode.A) && speedToBack > 1.0f) rotationToBakRight();

        // while press space when he prees w or s
        if (((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) && Input.GetKey(KeyCode.Space))) PressSpace();

        // while press w and s both as the same time when car is on speed > 0
        else if ((Input.GetKey(KeyCode.W) && speedToBack > 0) || (Input.GetKey(KeyCode.S) && speedToAhead > 0)) PressSpace();

        // while press only w without s
        else if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) OnGasToAhead();
        
         // while press only s without w
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) OnGasToBack();
         
         // if press only space or in any else 
        else if (Input.GetKey(KeyCode.Space)) PressSpace();
         
         // if press w and now not
        else if (!Input.GetKey(KeyCode.W) && speedToAhead > 0) OffGasAhead();
         
         // if press s and now not
        else if (!Input.GetKey(KeyCode.S) && speedToBack > 0) OffGasToBack();


    }

    // لما اكون ماشي لقدام
    void OnGasToAhead()
    {
        if (speedToAhead <= maxAheadSpeed && speedToBack <= 0)
        {
            speedToAhead += accelerateAhead;
            Distance.Set(0, 0, 1 * Time.deltaTime * speedToAhead);
            car.transform.Translate(Distance);
        }
        else car.transform.Translate(Distance);
    }

    // لما اكون ماشي لقدام واشيل دوسة البنزين
    void OffGasAhead()
    {
        speedToAhead -= autoAccelerateStopping;
        Distance.Set(0, 0, 1 * Time.deltaTime * speedToAhead);
        car.transform.Translate(Distance);
    }

    // لما اكون ماشي لورا
    void OnGasToBack()
    {
        if (speedToBack <= maxBackSpeed && speedToAhead <= 0)
        {
            speedToBack += accelerateBack;
            Distance.Set(0, 0, -1 * Time.deltaTime * speedToBack);
            car.transform.Translate(Distance);
        }
        else car.transform.Translate(Distance);
    }

    // لما اكون ماشي لورا واشيل دوسة البنزين
    void OffGasToBack()
    {
        speedToBack -= accelerateBack;
        Distance.Set(0, 0, -1 * Time.deltaTime * speedToBack);
        car.transform.Translate(Distance);
    }

 
    void rotationToAheadRight()
    {
        car.transform.Rotate(Vector3.up, 30 * Time.deltaTime, Space.World);
    }

    void rotationToAheadLeft()
    {
        car.transform.Rotate(Vector3.down, 30 * Time.deltaTime, Space.World);
    }

    void rotationToBakRight()
    {
         car.transform.Rotate(Vector3.up, 30 * Time.deltaTime, Space.World);
    }

    void rotationToBackLeft()
    {
        car.transform.Rotate(Vector3.down, 30 * Time.deltaTime, Space.World);
        // car.transform.Rotate(Vector3.up, currentTurnWheels * speedToAhead * Time.deltaTime, Space.World);
    }

    void PressSpace()
    {
        // break speed
        if (speedToAhead > 0)
        {
            speedToAhead -= carBrakes;
            Distance.Set(0, 0, 1 * Time.deltaTime * speedToAhead);
            car.transform.Translate(Distance);
        }
        else if (speedToBack > 0)
        {
            // سالب 1 عشان همشي العكس
            speedToBack -= carBrakes;
            Distance.Set(0, 0, -1 * Time.deltaTime * speedToBack);
            car.transform.Translate(Distance);
        }
    }

    // to make wheels steering when run
    void Steering()
    {
        

        // if user press space wheels will stop steer
        if (Input.GetKey(KeyCode.Space)) return;

        // if press w and s both
        else if (speedToAhead < 0 && speedToBack < 0 && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) 
        {
            if (speedOfRotation <= 3500.0f) speedOfRotation += accelerateAhead*30;

            wheel[2].transform.Rotate(Vector3.left, speedOfRotation * Time.deltaTime);
            wheel[3].transform.Rotate(Vector3.right, speedOfRotation * Time.deltaTime);
        }

        // وبعدين شال الضغط.. المفروض ان العجل الخلفي كان بيلف حولين نفسه وده هنا هيوقف العجل تدريجي w + s في حالة اذا كان ضاغط 
        // ! W + ! S
        else if (!(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) && speedOfRotation > 2000)
        {
            speedOfRotation -= 10;
            wheel[2].transform.Rotate(Vector3.left, speedOfRotation * Time.deltaTime);
            wheel[3].transform.Rotate(Vector3.right, speedOfRotation * Time.deltaTime);
        }

        // streering wheels .. the left wheels is معكوسين
        if (speedToAhead > 0.0)
        {
            wheel[0].transform.Rotate(Vector3.left, speedToAhead * 60 * Time.deltaTime);
            wheel[1].transform.Rotate(Vector3.right, speedToAhead * 60 * Time.deltaTime);
            wheel[2].transform.Rotate(Vector3.left, speedToAhead * 60 * Time.deltaTime);
            wheel[3].transform.Rotate(Vector3.right, speedToAhead * 60 * Time.deltaTime); 
        }
        else if (speedToBack > 0.0)
        {
            wheel[0].transform.Rotate(Vector3.right, speedToBack * 30 * Time.deltaTime);
            wheel[1].transform.Rotate(Vector3.left, speedToBack * 30 * Time.deltaTime);
            wheel[2].transform.Rotate(Vector3.right, speedToBack * 30 * Time.deltaTime);
            wheel[3].transform.Rotate(Vector3.left, speedToBack * 30 * Time.deltaTime);
        }
    }

    // turn wheels
    void TurnWheel()
    {

        if ((       ((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                    || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) )
                    && currentTurnWheels != 0.0f))
        {
            if (currentTurnWheels < 0.0f)
                currentTurnWheels += 4;
            else if (currentTurnWheels > 0.0f)
                currentTurnWheels -= 4;
            
            parentFrontLeftWheel.transform.localEulerAngles = new Vector3(0, currentTurnWheels, 0);
            ParentFrontRightWheel.transform.localEulerAngles = new Vector3(0, currentTurnWheels, 0);
        }
        else if (Input.GetKey(KeyCode.D) && currentTurnWheels < maxTurnWheels)
        {
            currentTurnWheels += 2;
            parentFrontLeftWheel.transform.localEulerAngles = new Vector3(0, currentTurnWheels, 0);
            ParentFrontRightWheel.transform.localEulerAngles = new Vector3(0, currentTurnWheels, 0);
        }
        else if (Input.GetKey(KeyCode.A) && currentTurnWheels > -maxTurnWheels)
        {
            currentTurnWheels -= 2;
            parentFrontLeftWheel.transform.localEulerAngles = new Vector3(0, currentTurnWheels, 0);
            ParentFrontRightWheel.transform.localEulerAngles = new Vector3(0, currentTurnWheels, 0);
        }
        

    }
}
