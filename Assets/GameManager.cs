using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public AudioClip explosionNoise;

    public GameObject player;

    public GameObject healthPivot;
    public GameObject progressPivot, progressShip, progressShipSpot;
    public SpriteModifier screenFlash;

    public LevelCreator[] levels;
    public LevelCreator currentLevelClone;
    public int levelIndex = 0;

    public Text scoreUI, levelUI;

    private void Awake(){
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        }else{
            instance = this;
        }
        levels = GetComponentsInChildren<LevelCreator>();
        foreach(LevelCreator lvl in levels){
            lvl.gameObject.SetActive(false);
        }

        
    }

    private void Start() {
        scoreUI.text = "0";
        levelUI.text = "-";
        StartNextLevel();
    }

    public void DestroyShip(GameObject ship, bool player = false){
        SoundManager.instance.PlaySound(explosionNoise);
        if(player){
            FinishLevel(false);
            StartNextLevel();
        }else{
            if(ship.GetComponent<Enemy>().isBoss){
                FinishLevel();
            }
            AddScore(ship);
            GameObject.Destroy(ship);
        }
    }

    public void UpdatePlayerHealth(int current, int max){
        float fraction = (float)current/(float)max;
        EffectsController.instance.SetPassiveCA(1.0f - fraction);
        Vector3 newScale = Vector3.one;
        newScale.y = fraction;
        healthPivot.transform.localScale = newScale;
    }

    public void UpdateProgress(float current, float max){
        float fraction = current/max;
        Vector3 newScale = Vector3.one;
        newScale.y = fraction;
        progressPivot.transform.localScale = newScale;
        progressShip.transform.position = progressShipSpot.transform.position;
    }

    public void AddScore(GameObject ship){
        //determine score later
        score += 10;
        scoreUI.text = score.ToString();
    }

    public void FinishLevel(bool advance = true){
        currentLevelClone.CleanUp();
        BulletManager.instance.ClearBullets();
        screenFlash.CreateDeathSprite();
        GameObject.Destroy(currentLevelClone.gameObject);
        if(advance && levelIndex + 1 < levels.Length){
            levelIndex++;
            StartNextLevel();
        }
    }

    public void StartNextLevel(){
        if(levelIndex >= levels.Length){
            return;
        }
        //update player health
        player.GetComponent<Health>().currentHealth = player.GetComponent<Health>().health;
        UpdatePlayerHealth(1,1);
        UpdateProgress(0,1);

        //create level:
        GameObject newLevel = Instantiate(levels[levelIndex].gameObject, transform);
        newLevel.SetActive(true);
        currentLevelClone = newLevel.GetComponent<LevelCreator>();
        currentLevelClone.StartLevel();
        levelUI.text = (levelIndex+1).ToString();
    }
}
