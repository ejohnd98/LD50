using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float entryTime;
    public float activeTime = 5.0f;
    public EntryType entryPattern;
    public Vector2 entryDirection;

    public MovementType moveType;
    public Vector2 movementDir = Vector2.down;
    public float speed = 1.0f;
    public float effectSpeed = 1.0f;
    public float effectStrength = 1.0f;
    public bool isBoss = false;

    float moveAnimCounter = 0.0f;

    Vector2 originalPos;
    Vector2 moveOffset;
    Vector2 effectOffset;

    Movement movement;

    public bool debugUpdate = false;

    private void Awake() {
        movementDir = movementDir.normalized;
        movement = GetComponent<Movement>();
    }

    public void SetMoving(bool state){
        movement.moving = state;
    }

    private void Update() {
        if(debugUpdate){
            debugUpdate = false;
            movement.InitMovement(this);
            SetMoving(true);
        }
    }

    public void StopShooting(){
        BulletCreator bc = GetComponent<BulletCreator>();
        if(bc!=null){
            bc.automatic = false;
            bc.burstAmount = 0;
        }
    }

    [ContextMenu("Rename object")]
    void RenameObject()
    {
        if(isBoss){
            gameObject.name = "Boss";
            return;
        }
        string newName = "";

        BulletSetting bulletSetting = GetComponent<BulletSetting>();
        BulletCreator bulletCreator = GetComponent<BulletCreator>();
        switch(bulletSetting.moveType){
            case MovementType.STRAIGHT:
                newName += "Straight ";
            break;
            case MovementType.CIRCLE:
                newName += "Circle ";
            break;
            case MovementType.SIN:
                newName += "Sin ";
            break;
        }
        switch(bulletCreator.pattern){
            case BulletPattern.SINGLE:
                newName += "Single ";
            break;
            case BulletPattern.ARC:
                newName += "Arc";
                newName += bulletCreator.circlePoints.ToString();
            break;
            case BulletPattern.CIRCLE:
                newName += "Spin";
                newName += bulletCreator.circlePoints.ToString();
            break;
        }
        
        gameObject.name = newName;
    }
}
