using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletPattern{
    SINGLE,
    CIRCLE,
    ARC
}

public class BulletCreator : MonoBehaviour
{
    public BulletPattern pattern;
    public int totalBursts = 10;
    public bool endless = true;
    public bool automatic = true;
    public float period = 0.5f;
    public float delay = 0.0f;

    public int burstAmount = 1;
    public float burstDelay = 0.1f;

    public int circlePoints = 10;
    public float arcAngle = 45;

    public Vector2 mainDir = Vector2.down;

    //soundSettings
    public AudioClip bulletSound;
    public bool playForEachBullet = true;


    BulletSetting bulletSettings;
    int usedBursts = 0;
    float counter = 0.0f;

    bool firingBursts = false;
    float burstCounter = 0.0f;
    int burstIndex = 0;

    public bool buttonHeld = false;

    void Awake(){
        counter = delay;
        bulletSettings = GetComponent<BulletSetting>();
    }

    // Update is called once per frame
    void Update()
    {
        if(endless || usedBursts < totalBursts){
            if(counter <= 0.0f && (automatic || (!automatic && buttonHeld))){
                firingBursts = true;
                burstIndex = 0;
                burstCounter = 0.0f;
                counter = period;
            }else{
                counter -= Time.deltaTime;
            }
        }
        if(firingBursts){
            if(burstCounter <= 0.0f){
                CreateBurst();
                if(playForEachBullet || burstIndex == 0){
                    SoundManager.instance.PlaySound(bulletSound);
                }
                burstIndex++;
                burstCounter = burstDelay;
                if(burstIndex >= burstAmount){
                    firingBursts = false;
                }
                
            }else{
                burstCounter -= Time.deltaTime;
            }
        }
    }

    void CreateBurst(){
        usedBursts++;
        switch(pattern){
            case BulletPattern.SINGLE:
                BulletManager.instance.CreateBullet(bulletSettings, mainDir, transform.position);
            break;
            case BulletPattern.CIRCLE:
                for(int i = 0; i < circlePoints; i++){
                    float fraction = (float)i/(float)circlePoints;
                    Vector2 dir = new Vector2(Mathf.Cos(fraction * 2*Mathf.PI), Mathf.Sin(fraction * 2*Mathf.PI));
                    BulletManager.instance.CreateBullet(bulletSettings, dir, transform.position);
                }
                
            break;
            case BulletPattern.ARC:
                float angle = arcAngle/(float)(circlePoints-1);

                for(int i = 0; i < circlePoints; i++){
                    float rotateAngle = (arcAngle*-0.5f) + angle*i;
                    Vector2 dir = Quaternion.Euler(0, 0, rotateAngle) * mainDir;
                    BulletManager.instance.CreateBullet(bulletSettings, dir, transform.position);
                }

                // }else{
                //     for(int i = -(circlePoints/2); i <= circlePoints/2; i++){
                //         float rotateAngle = i* arcAngle/(circlePoints/2);
                //         Vector2 dir = Quaternion.Euler(0, 0, rotateAngle) * mainDir;
                //         BulletManager.instance.CreateBullet(bulletSettings, dir, transform.position);
                //     }
                // }
                
            break;
        }
        
    }
}
