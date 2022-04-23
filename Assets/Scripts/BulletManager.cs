using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;
    public static Vector2 boundMin, boundMax;

    BulletPool bulletPool;
    public BoxCollider2D bulletBounds;

    private void Awake(){
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        }else{
            instance = this;
        }
        bulletPool = GetComponent<BulletPool>();
        boundMin = bulletBounds.bounds.min;
        boundMax = bulletBounds.bounds.max;
    }

    public void CreateBullet(BulletSetting setting, Vector2 dir, Vector2 position){
        GameObject obj = bulletPool.RequestBullet();
        Bullet bullet = obj.GetComponent<Bullet>();

        obj.transform.position = position;
        if(setting.playerBullet){
            obj.tag = "playerBullet";
        }else{
            obj.tag = "enemyBullet";
        }
        bullet.SetBullet(setting);
        bullet.SetMovement(dir);
    }

}
