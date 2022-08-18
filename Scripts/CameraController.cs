using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//Structue of MetaData
public struct Parameter{
    public int id;
    public float UpArmpos_x;
    public float UpArmpos_y;
    public float LowArmpos_x;
    public float LowArmpos_y;
    public float Handpos_x;
    public float Handpos_y;
}

//Converter class (between binary bytes and structure)
public class Converter{
    public Byte[] StructToBytes(System.Object structure){
        Int32 size = Marshal.SizeOf(structure);
        Console.WriteLine(size);
        IntPtr buffer = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.StructureToPtr(structure, buffer, false);
            Byte[] bytes = new Byte[size];
            Marshal.Copy(buffer, bytes, 0, size);
            return bytes;
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    public System.Object BytesToStruct(Byte[] bytes, Type strcutType){
        Int32 size = Marshal.SizeOf(strcutType);
        IntPtr buffer = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.Copy(bytes, 0, buffer, size);
            return Marshal.PtrToStructure(buffer, strcutType);
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }
}

public class CameraController : MonoBehaviour
{
    Camera cam;
    public int count = 0;

    public GameObject UpArm_pos;
    public GameObject LowArm_pos;
    public GameObject Hand_pos;

    string folderPath = Directory.GetCurrentDirectory();

    string imgfolderPath;
    string metafolderPath;

    string imgfolderName = "/Arm/ImageData/";
    string metafolderName = "/Arm/MetaData/";

    Converter Convert = new Converter();

    //WorldToScreen Vector3 to Structure
    public Parameter VtoS(Vector3 UpArm, Vector3 LowArm, Vector3 Hand, int count){
        Parameter text = new Parameter();
        text.id = count;

        text.UpArmpos_x = UpArm.x;
        text.UpArmpos_y = UpArm.y;

        text.LowArmpos_x = LowArm.x;
        text.LowArmpos_x = LowArm.y;

        text.Handpos_x = Hand.x;
        text.Handpos_y = Hand.y;

        return text;
    }

    //Save MetaData(Structure convert to Binary bytes)
    public void SaveData(Parameter text, int count){

        string filename = Path.Combine(metafolderPath, (count.ToString()+ ".txt"));

        FileStream fs = File.Create(filename);

        Byte[] data = Convert.StructToBytes(text);

        fs.Write(data, 0, data.Length);
    }

    public void SaveData(Vector2 UpArm, Vector2 LowArm, Vector2 Hand){

        string filename = Path.Combine(metafolderPath, (count.ToString()+ ".txt"));

        string[] lines = {UpArm.ToString(), LowArm.ToString(), Hand.ToString()};

        File.WriteAllLines(filename, lines);

    }

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

        UpArm_pos = GameObject.Find("Right_UpperArm");
        LowArm_pos = GameObject.Find("Right_LowerArm");
        Hand_pos = GameObject.Find("Right_Hand");

        imgfolderPath = folderPath + imgfolderName;
        metafolderPath = folderPath + metafolderName;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 UpArm_screenPos = cam.WorldToScreenPoint(UpArm_pos.transform.position);
        Vector3 LowArm_screenPos = cam.WorldToScreenPoint(LowArm_pos.transform.position);
        Vector3 Hand_screenPos = cam.WorldToScreenPoint(Hand_pos.transform.position);
        */
        
        Vector2 UpArm_screenPos = RectTransformUtility.WorldToScreenPoint (cam, UpArm_pos.transform.position);
        Vector2 LowArm_screenPos = RectTransformUtility.WorldToScreenPoint (cam, LowArm_pos.transform.position);
        Vector2 Hand_screenPos = RectTransformUtility.WorldToScreenPoint (cam, Hand_pos.transform.position);
        
        ScreenCapture.CaptureScreenshot(Path.Combine(imgfolderPath, (count.ToString()+ ".png")));
        //SaveData(VtoS(UpArm_screenPos, LowArm_screenPos, Hand_screenPos, count), count);
        //SaveData(UpArm_screenPos, LowArm_screenPos, Hand_screenPos, count);
        SaveData(UpArm_screenPos, LowArm_screenPos, Hand_screenPos);
        
        count++;
    }
}