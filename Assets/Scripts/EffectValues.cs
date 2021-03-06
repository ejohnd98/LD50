using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectValues : MonoBehaviour
{

    public static float GetSin(float progress){
        return Mathf.Sin(progress * 2 * Mathf.PI);
    }

    public static float GetCos(float progress){
        return Mathf.Cos(progress * 2 * Mathf.PI);
    }
}
