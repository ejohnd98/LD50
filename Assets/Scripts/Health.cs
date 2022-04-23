using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 3;
    public int currentHealth;
    public bool enemy = true;
    bool dead = false;
    string bulletTag;

    SpriteModifier spriteMod;

    private void Awake() {
        bulletTag = enemy? "playerBullet" : "enemyBullet";
        currentHealth = health;
        spriteMod = GetComponent<SpriteModifier>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag.Equals(bulletTag)){
            other.GetComponent<Bullet>().DisableBullet();
            currentHealth--;
            
            if(!enemy){
                ScreenShake.instance.StartShortShake(0.05f);
                EffectsController.instance.HitEffect();
                GameManager.instance.UpdatePlayerHealth(currentHealth, health);
            }
            if(currentHealth <= 0){
                currentHealth = 0;
                spriteMod.CreateDeathSprite();
                Kill();
            }else{
                SoundManager.instance.PlayHitSound();
                spriteMod.FlashWhite();
            }
        }
    }

    void Kill(){
        dead = true;
        GameManager.instance.DestroyShip(gameObject, !enemy);
    }
}
