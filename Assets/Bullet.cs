using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType{
    STRAIGHT,
    SIN,

    NONE
}

public class Bullet : MonoBehaviour
{
    CircleCollider2D circleCollider;
    SpriteRenderer spriteRenderer;
    Vector2 direction;
    float speed;
    float effectStrength;
    float effectSpeed;
    float moveProgress;
    MovementType movementType;

    Vector2 originalPos;
    Vector2 moveOffset;
    Vector2 effectOffset;

    private void Awake() {
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetBullet(BulletSetting setting){
        transform.localScale = Vector3.one * setting.size;
        spriteRenderer.sprite = setting.spr;
        spriteRenderer.color = setting.color;
        movementType = setting.moveType;
        speed = setting.speed;
        effectSpeed = setting.effectSpeed;
        effectStrength = setting.effectStrength;
        moveProgress = 0.0f;
        originalPos = transform.position;
        moveOffset = Vector2.zero;
        effectOffset = Vector2.zero;
    }

    public void SetMovement(Vector2 dir){
        direction = dir;
    }

    private void Update() {
        HandleMovement();
        CheckBounds();
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
        }
        originalPos += newMovement * speed * Time.deltaTime;
        transform.position = originalPos + moveOffset + effectOffset;
    }

    public void DisableBullet(){
        gameObject.SetActive(false);
    }

    private void CheckBounds(){
        Vector2 pos = transform.position;

        if(pos.x < BulletManager.boundMin.x || pos.x > BulletManager.boundMax.x
            || pos.y < BulletManager.boundMin.y || pos.y > BulletManager.boundMax.y){

            DisableBullet();
        }
    }
}