using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    float threthold = 1.0f;

    int min_x_angle = -15;
    int max_x_angle = 15;

    int min_y_angle = 0;
    int max_y_angle = 360;

    int min_z_angle = -30;
    int max_z_angle = 30;
    
    float min_angular_speed = 5.0f;
    float max_angular_speed = 10.0f;

    float vel = 1.0f;

    int rotate_angle_x, rotate_angle_y, rotate_angle_z;

    public float Velocity(){
        float vel = UnityEngine.Random.Range(min_angular_speed, max_angular_speed);
        return vel;
    }

    public int RotateAngle(string id){
        if (id == "x" || id == "X"){
            int angle = UnityEngine.Random.Range(min_x_angle, max_x_angle);
            return angle;
        }  
        else if (id == "y" || id == "Y"){
            int angle = UnityEngine.Random.Range(min_y_angle, max_y_angle);
            return angle;
        }
        else if (id == "z" || id == "Z"){
            int angle = UnityEngine.Random.Range(min_z_angle, max_z_angle);
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
        rotate_angle_y = RotateAngle("y");
        rotate_angle_z = RotateAngle("z");
    }

    // Update is called once per frame
    void Update()
    {   
        var rot = transform.localEulerAngles;
        (bool get_x, float dist_x) = DistAngle(rotate_angle_x, rot[0]);
        (bool get_y, float dist_y) = DistAngle(rotate_angle_y, rot[1]);
        (bool get_z, float dist_z) = DistAngle(rotate_angle_z, rot[2]);

        if (get_x) {
            rotate_angle_x = RotateAngle("x");
        }
        else{
            if (dist_x > 0){
                transform.localEulerAngles += new Vector3(vel, 0, 0) * Time.deltaTime;
            }
            else{
                transform.localEulerAngles -= new Vector3(vel, 0, 0) * Time.deltaTime;
            }
        }
        
        if (get_y) {
            rotate_angle_y = RotateAngle("y");
        }
        else{
            if (dist_y > 0){
                transform.localEulerAngles += new Vector3(0, vel, 0) * Time.deltaTime;
            }
            else{
                transform.localEulerAngles -= new Vector3(0, vel, 0) * Time.deltaTime;
            }
        }
        
        if (get_z) {
            rotate_angle_z = RotateAngle("z");
        }
        else{
            if (dist_z > 0){
                transform.localEulerAngles += new Vector3(0, 0, vel) * Time.deltaTime;
            }
            else{
                transform.localEulerAngles -= new Vector3(0, 0, vel) * Time.deltaTime;
            }
        }
        
    }
}
