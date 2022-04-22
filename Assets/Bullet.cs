using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType{
    STRAIGHT,

    NONE
}

public class Bullet : MonoBehaviour
{
    CircleCollider2D circleCollider;
    SpriteRenderer spriteRenderer;
    Vector2 direction;
    float speed;
    MovementType movementType;

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
    }

    public void SetMovement(Vector2 dir){
        direction = dir;
    }

    private void Update() {
        HandleMovement();
        CheckBounds();
    }

    private void HandleMovement(){
        Vector3 move = Vector3.zero;
        switch(movementType){
            case MovementType.STRAIGHT:
                move = direction;
                break;
            default:
            break;
        }

        transform.position += move * speed * Time.deltaTime;
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