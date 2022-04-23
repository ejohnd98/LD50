using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Collider2D playerArea;

    public BulletCreator bulletCreatorA, bulletCreatorB; //turn into list of creators later
    
    //player stats
    float hMovementSpeed = 4.0f;
    float vMovementSpeed = 3.5f;

    // Update is called once per frame
    private void Update(){

        //handle firing
        bulletCreatorA.buttonHeld = InputHandler.instance.attack_button_held;
        bulletCreatorB.buttonHeld = InputHandler.instance.attack2_button_held;
        
        //handle movement
        Vector3 movement = new Vector3(InputHandler.instance.h_axis * hMovementSpeed, InputHandler.instance.v_axis * vMovementSpeed, 0.0f) * Time.deltaTime;
        Vector3 newPos = transform.position + movement;
        newPos.x = Mathf.Clamp(newPos.x, playerArea.bounds.min.x, playerArea.bounds.max.x);
        newPos.y = Mathf.Clamp(newPos.y, playerArea.bounds.min.y, playerArea.bounds.max.y);

        transform.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag.Equals("enemyBullet")){
            Debug.Log("Player Hit!");
            other.GetComponent<Bullet>().DisableBullet();
        }
    }
}
