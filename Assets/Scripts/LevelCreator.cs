using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EntryType{
    POPIN,
    LERP,
}

public class LevelCreator : MonoBehaviour
{
    

    public class EnemyWave
    {
        public GameObject enemy;
        public float entryTime;
        public float activeTime;
        public EntryType entryPattern;
        public Vector2 entryDirection;

        public bool entering = true, leaving = false, finished = false;
        public Vector3 entryA, entryB;
        public float entryCounter = 0.0f, activeCounter = 0.0f;

        public EnemyWave(GameObject obj, float timeOffset = 0.0f){
            enemy = obj;
            Enemy temp = obj.GetComponent<Enemy>();
            entryTime = timeOffset + temp.entryTime;
            activeTime = temp.activeTime;
            entryPattern = temp.entryPattern;
            entryDirection = temp.entryDirection;
        }
    }

    List<EnemyWave> enemies;
    List<EnemyWave> activeEnemies;
    List<EnemyWave> wavesToRemove;

    public bool levelActive = false;
    public float levelLength = 60.0f;
    public AudioClip levelMusic;
    public UnityEvent levelStartEvents;
    public bool hasBoss = true;
    float levelTime = 0.0f;
    
    int enemyIndex = 0;

    // Start is called before the first frame update
    void Awake(){
        enemies = new List<EnemyWave>();
        activeEnemies = new List<EnemyWave>();
        wavesToRemove = new List<EnemyWave>();

        //generate EnemyWave classes
        for(int i = 0; i < transform.childCount; i++){
            if(transform.GetChild(i).GetComponent<Enemy>() != null){
                enemies.Add(new EnemyWave(transform.GetChild(i).gameObject));
                transform.GetChild(i).gameObject.SetActive(false);
            }else{
                Transform childObj = transform.GetChild(i);
                float timeOffset = 0.0f;
                WaveOffset offset = childObj.GetComponent<WaveOffset>();
                if(offset != null){
                    timeOffset = offset.timeOffset;
                }
                for(int j = 0; j < childObj.childCount; j++){
                    if(childObj.GetChild(j).GetComponent<Enemy>() != null){
                        enemies.Add(new EnemyWave(childObj.GetChild(j).gameObject, timeOffset));
                        childObj.GetChild(j).gameObject.SetActive(false);
                    }
                }
            }
            
        }
    }

    // Update is called once per frame
    void Update(){
        if(levelActive){
            GameManager.instance.UpdateProgress(levelTime, levelLength);
            while(enemyIndex < enemies.Count && enemies[enemyIndex].entryTime <= levelTime){
                ActivateEnemy(enemies[enemyIndex]);
                enemyIndex++;
            }
            foreach(EnemyWave enemy in activeEnemies){
                HandleEnemy(enemy);
            }

            if(wavesToRemove.Count > 0){
                foreach(EnemyWave enemy in wavesToRemove){
                    activeEnemies.Remove(enemy);
                }
                wavesToRemove.Clear();
            }

            levelTime += Time.deltaTime;
            if(levelTime >= levelLength){
                levelActive = false;
                if(hasBoss){
                    return;
                }else{
                    GameManager.instance.CompleteLevel();
                }
                
            }
        }
    }

    void ActivateEnemy(EnemyWave enemyWave){
        //final position on screen:
        enemyWave.entryB = enemyWave.enemy.transform.position;

        //compute starting position off screen
        Vector2 pos = enemyWave.entryB;
        while(pos.x > BulletManager.boundMin.x && pos.x < BulletManager.boundMax.x
            && pos.y > BulletManager.boundMin.y && pos.y < BulletManager.boundMax.y){
            
            pos -= enemyWave.entryDirection;
        }
        enemyWave.entryA = pos;
        enemyWave.enemy.transform.position = pos;
        enemyWave.enemy.SetActive(true);
        activeEnemies.Add(enemyWave);
    }

    void HandleEnemy(EnemyWave enemy){
        if(enemy.enemy == null){
            enemy.finished = true;
            wavesToRemove.Add(enemy);
            return;
        }
        if(enemy.finished){
            enemy.enemy.SetActive(false);
            wavesToRemove.Add(enemy);
            return;
        }

        if(enemy.entering){
            //switch statement later depending on entry type
            enemy.enemy.transform.position = Vector2.Lerp(enemy.entryA, enemy.entryB,  Easings.EaseInOutQuad(enemy.entryCounter));
            enemy.entryCounter += Time.deltaTime;
            if(enemy.entryCounter >= 1.0f){
                enemy.entering = false;
                enemy.enemy.GetComponent<Movement>().InitMovement(enemy.enemy.GetComponent<Enemy>());
                enemy.enemy.GetComponent<Enemy>().SetMoving(true);
            }
        }else if(enemy.leaving){
            //switch statement later depending on entry type
            enemy.enemy.transform.position = Vector2.Lerp(enemy.entryB, enemy.entryA,  Easings.EaseInOutQuad(enemy.entryCounter));
            enemy.entryCounter += Time.deltaTime;
            if(enemy.entryCounter >= 1.0f){
                enemy.leaving = false;
                enemy.finished = true;
            }
        }else{
            if(enemy.activeCounter >= enemy.activeTime){
                enemy.leaving = true;
                enemy.entryB = enemy.enemy.transform.position;
                enemy.entryCounter = 0.0f;
                enemy.enemy.GetComponent<Enemy>().SetMoving(false);
            }else{
                enemy.activeCounter += Time.deltaTime;
            }
        }
    }

    public void StartLevel(){
        levelActive = true;
        levelStartEvents?.Invoke();
        if(levelMusic != null){
            SoundManager.instance.PlayMusic(levelMusic);
        }
    }

    public void CleanUp(){
        levelActive = false;
        foreach(EnemyWave enemy in activeEnemies){
            enemy.enemy.GetComponent<SpriteModifier>().CreateDeathSprite();
            GameObject.Destroy(enemy.enemy.gameObject);
        }
        activeEnemies.Clear();
    }
}
