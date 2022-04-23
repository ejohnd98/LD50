using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool moving = false;

    Vector2 direction;
    float speed;
    float effectStrength;
    float effectSpeed;
    float moveProgress;
    MovementType movementType;

    Vector2 originalPos;
    Vector2 moveOffset;
    Vector2 effectOffset;

    public void InitMovement(BulletSetting setting){
        movementType = setting.moveType;
        speed = setting.speed;
        effectSpeed = setting.effectSpeed;
        effectStrength = setting.effectStrength;
        moveProgress = 0.0f;
        originalPos = transform.position;
        moveOffset = Vector2.zero;
        effectOffset = Vector2.zero;
    }

    public void InitMovement(Enemy setting){
        movementType = setting.moveType;
        speed = setting.speed;
        effectSpeed = setting.effectSpeed;
        effectStrength = setting.effectStrength;
        moveProgress = 0.0f;
        originalPos = transform.position;
        moveOffset = Vector2.zero;
        effectOffset = Vector2.zero;
        direction = setting.movementDir;
    }

    public void SetMovement(Vector2 dir){
        direction = dir.normalized;
    }

    private void Update() {
        if(moving){
            HandleMovement();
        }
    }

    private void HandleMovement(){
        Vector2 newMovement = Vector2.zero;
        switch(movementType){
            case MovementType.STRAIGHT:
                newMovement = direction;
            break;
            case MovementType.SIN:
                newMovement = direction;
                effectOffset = Vector2.Perpendicular(direction) * effectStrength * EffectValues.GetSin(moveProgress);
                moveProgress += Time.deltaTime * effectSpeed;
            break;
            case MovementType.CIRCLE:
                newMovement = direction;
                effectOffset = Vector2.down*effectStrength + new Vector2(EffectValues.GetSin(moveProgress), EffectValues.GetCos(moveProgress)) * effectStrength;
                moveProgress += Time.deltaTime * effectSpeed;
            break;
        }
        moveOffset += newMovement * speed * Time.deltaTime;
        transform.position = originalPos + moveOffset + effectOffset;
    }
}