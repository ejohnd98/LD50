using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int health = 3;
    public int currentHealth;
    public bool enemy = true;
    public UnityEvent deathEvents;
    bool dead = false;
    public bool giveScore = true;
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
        deathEvents?.Invoke();
        dead = true;
        if(giveScore){
            GameManager.instance.AddScore(health);
        }
        GameManager.instance.DestroyShip(gameObject, !enemy);
    }
}
