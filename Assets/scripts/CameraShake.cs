///Daniel Moore (Firedan1176) - Firedan1176.webs.com/
///26 Dec 2015
///
///Shakes camera parent object

using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private bool Shaking;
    private float ShakeDecay;
    private float ShakeIntensity;

    private Vector3 OriginalPos;
    private Quaternion OriginalRot;

    private Transform shakeTransform;

    void Start()
    {
        Shaking = false;
    }

    void Update()
    {
        if (ShakeIntensity > 0)
        {
            shakeTransform.position = OriginalPos + Random.insideUnitSphere * ShakeIntensity;
            shakeTransform.rotation = new Quaternion(OriginalRot.x + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                            OriginalRot.y + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                            OriginalRot.z + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                            OriginalRot.w + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f);

            ShakeIntensity -= ShakeDecay;
        }   
        else if (Shaking)
        {
            shakeTransform.eulerAngles = Vector3.zero;
            Shaking = false;
        }
    }

    public void DoShake(CamShakeDto _data, Transform _shakeTransform = null)
    {
        if(_shakeTransform == null) {
            shakeTransform = transform;
        }
        else
        {
            shakeTransform = _shakeTransform;
        }

        OriginalPos = shakeTransform.position;
        OriginalRot = shakeTransform.rotation;

        ShakeIntensity = _data.intensity;
        ShakeDecay = _data.length;
        Shaking = true;
    }
}