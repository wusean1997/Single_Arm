using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour
{
    float threthold = 1.0f;

    public GameObject Inter_pos;
    public GameObject Distal_pos;

    int min_Prox_x_angle = -10;
    int max_Prox_x_angle = 90;

    int min_Inter_x_angle = 0;
    int max_Inter_x_angle = 90;

    int min_Distal_x_angle = 0;
    int max_Distal_x_angle = 45;
    
    float min_angular_speed = 5.0f;
    float max_angular_speed = 10.0f;
    
    float vel = 1.0f;

    int rotate_Prox_angle, rotate_Inter_angle, rotate_Distal_angle;

    public float Velocity(){
        float vel = UnityEngine.Random.Range(min_angular_speed, max_angular_speed);
        return vel;
    }

    public int RotateAngle(string id){
        if (id == "Prox"){
            int angle = UnityEngine.Random.Range(min_Prox_x_angle, max_Prox_x_angle);
            return angle;
        }  
        else if (id == "Inter"){
            int angle = UnityEngine.Random.Range(min_Inter_x_angle, max_Inter_x_angle);
            return angle;
        }
        else if (id == "Distal"){
            int angle = UnityEngine.Random.Range(min_Distal_x_angle, max_Distal_x_angle);
            return angle;
        }   
        else{
            print("Function() RotateAngle Syntax Error");
            int angle=0;
            return angle;
        }
    } 

    public (bool, float) DistAngle(int rotate_angle, float rotation){
        if (rotation > 180){
            rotation -= 360;
        }
        float dist = rotate_angle - rotation;
        if (Math.Abs(dist) < threthold){
            bool get = true;
            return (get, dist);
        }
        else{
            bool get = false;
            return (get, dist);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //rotate_angle_x = RotateAngle("x");
        Inter_pos = GameObject.Find("Right_RingIntermediate");
        Distal_pos = GameObject.Find("Right_RingDistal");
        rotate_Prox_angle = RotateAngle("Prox");
        rotate_Inter_angle = RotateAngle("Inter");
        rotate_Distal_angle = RotateAngle("Distal");
    }

    // Update is called once per frame
    void Update()
    {   
        var Prox_rot = transform.localEulerAngles;
        var Inter_rot = Inter_pos.transform.localEulerAngles;
        var Distal_rot = Distal_pos.transform.localEulerAngles;

        (bool get_Prox, float dist_Prox) = DistAngle(rotate_Prox_angle, Prox_rot[0]);
        (bool get_Inter, float dist_Inter) = DistAngle(rotate_Inter_angle, Inter_rot[0]);
        (bool get_Distal, float dist_Distal) = DistAngle(rotate_Distal_angle, Distal_rot[0]);
        
        if (get_Prox) {
            rotate_Prox_angle = RotateAngle("Prox");
        }
        else{
            if (dist_Prox > 0){
                transform.localEulerAngles += new Vector3(vel, 0, 0) * Time.deltaTime;
            }
            else{
                transform.localEulerAngles -= new Vector3(vel, 0, 0) * Time.deltaTime;
            }
        }

        if (get_Inter) {
            rotate_Inter_angle = RotateAngle("Inter");
        }
        else{
            if (dist_Inter > 0){
                Inter_pos.transform.localEulerAngles += new Vector3(vel, 0, 0) * Time.deltaTime;
            }
            else{
                Inter_pos.transform.localEulerAngles -= new Vector3(vel, 0, 0) * Time.deltaTime;
            }
        }

        if (get_Distal) {
            rotate_Distal_angle = RotateAngle("Distal");
        }
        else{
            if (dist_Distal > 0){
                Distal_pos.transform.localEulerAngles += new Vector3(vel, 0, 0) * Time.deltaTime;
            }
            else{
                Distal_pos.transform.localEulerAngles -= new Vector3(vel, 0, 0) * Time.deltaTime;
            }
        }
    }
}
