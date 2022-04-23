using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType{
    STRAIGHT,
    SIN,
    CIRCLE,

    NONE
}

public class Bullet : MonoBehaviour
{
    Movement movement;
    CircleCollider2D circleCollider;
    SpriteRenderer spriteRenderer;

    private void Awake() {
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<Movement>();
        SetMoving(true);
    }

    public void SetBullet(BulletSetting setting){
        transform.localScale = Vector3.one * setting.size;
        spriteRenderer.sprite = setting.spr;
        spriteRenderer.color = setting.color;
        movement.InitMovement(setting);
    }

    public void SetMovement(Vector2 dir){
        movement.SetMovement(dir);
    }

    private void Update() {
        CheckBounds();
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

    public void SetMoving(bool state){
        movement.moving = state;
    }
}