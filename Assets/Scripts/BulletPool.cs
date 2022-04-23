using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script only handles bullet object pooling

public class BulletPool : MonoBehaviour
{

    public GameObject bulletPrefab;
    public GameObject bulletContainer;
    public List<GameObject> bulletPool;
    public int bulletCount;

    private void Start() {
        bulletPool = GenerateBullets(bulletCount);
    }

    private List<GameObject> GenerateBullets(int num){
        for(int i = 0; i < num; i++){
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.parent = bulletContainer.transform;
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }

        return bulletPool;
    }

    public GameObject RequestBullet(){
        foreach(GameObject obj in bulletPool){
            if(!obj.activeInHierarchy){
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.parent = bulletContainer.transform;
        bulletPool.Add(bullet);

        return bullet;
    }

    public void ClearBullets(){
        foreach(GameObject obj in bulletPool){
            obj.SetActive(false);
        }
    }
}
