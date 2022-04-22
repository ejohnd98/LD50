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

    private void Awake() {
        bulletTag = enemy? "playerBullet" : "enemyBullet";
        currentHealth = health;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag.Equals(bulletTag)){
            other.GetComponent<Bullet>().DisableBullet();

            health--;
            if(health <= 0){
                Kill();
            }
        }
    }

    void Kill(){
        dead = true;
        if(enemy){
            Destroy(gameObject);
        }else{
            Debug.Log("Player died");
        }
    }
}
