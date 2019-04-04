using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CamShakeDto
{
    public float intensity;
    public float length;

    public CamShakeDto(float _intensity, float _length)
    {
        intensity = _intensity;
        length = _length;
    }
}

public class Constants 
{
    public static int MapWidth = 7;  
    public static int MapHeight = 11; 
    public static float CameraX = (MapWidth / 2.0f) - 0.5f;

    public static float BattyMoveSpeed = 15f;
    public static float ObjectMoveSpeed = 7.5f;

    public static CamShakeDto LightCamShake = new CamShakeDto(0.035f, 0.004f);
    public static CamShakeDto LightCamShakeLong = new CamShakeDto(0.03f, 0.002f);
    public static CamShakeDto MediumCamShake = new CamShakeDto(0.045f, 0.0035f);
    public static CamShakeDto HeavyCamShake = new CamShakeDto(0.1f, 0.0025f);

}

public class Offsets
{
    public static Vector3 GuardEnemyOffset = new Vector3(0, 0.6f, 0);
    public static Vector3 BatEnemyOffset = new Vector3(0, 0.6f, 0);
}
