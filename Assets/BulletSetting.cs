using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSetting : MonoBehaviour
{
    //visuals
    public Sprite spr;
    public Color color;

    //movement
    public MovementType moveType;
    public float speed;
    public float size;
    public float effectStrength = 0.5f;
    public float effectSpeed = 1.0f;
    public bool playerBullet = false;
}
