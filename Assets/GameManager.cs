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
    public int levelsCompleted = 0;
    public bool disableSaving = false;

    public GameObject[] LevelButtons;

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
        if (!disableSaving){
            LoadGame();
        }
        scoreUI.text = score.ToString();
        levelUI.text = "-";
        StartLevel(0);
    }

    public void DestroyShip(GameObject ship, bool player = false){
        SoundManager.instance.PlaySound(explosionNoise);
        if(player){
            StartLevel(0);
        }else{
            if(ship.GetComponent<Enemy>().isBoss){
                CompleteLevel();
            }
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

    public void AddScore(int amount){
        //determine score later
        score += amount;
        scoreUI.text = score.ToString();
    }

    public void CompleteLevel(){
        levelsCompleted = Mathf.Max(levelIndex, levelsCompleted);
        StartLevel(0);
    }

    public void StartLevel(int index){
        if(levelIndex >= levels.Length){
            return;
        }

        //finish current level:
        if(currentLevelClone != null){
            currentLevelClone.CleanUp();
            BulletManager.instance.ClearBullets();
            screenFlash.CreateDeathSprite();
            GameObject.Destroy(currentLevelClone.gameObject);
        }
        UpdateLevelButtons();

        levelIndex = index;

        //update player health
        player.GetComponent<Health>().currentHealth = player.GetComponent<Health>().health;
        UpdatePlayerHealth(1,1);
        UpdateProgress(0,1);

        //create level:
        GameObject newLevel = Instantiate(levels[levelIndex].gameObject, transform);
        newLevel.SetActive(true);
        currentLevelClone = newLevel.GetComponent<LevelCreator>();
        currentLevelClone.StartLevel();
        levelUI.text = (levelIndex == 0)? "Menu" : levelIndex.ToString();
        if (!disableSaving){
            SaveGame();
        }
    }

    public void ExitGame(){
        Application.Quit();
    }

    void UpdateLevelButtons(){
        for(int i = 0; i < LevelButtons.Length; i++){
            SpriteRenderer sprRenderer = LevelButtons[i].GetComponent<SpriteRenderer>();
            Color col = sprRenderer.color;
            if(i <= levelsCompleted){
                col.a = 1.0f;
                LevelButtons[i].GetComponent<BoxCollider2D>().enabled = true;
            }else{
                col.a = 0.2f;
                LevelButtons[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            sprRenderer.color = col;
            
        }
    }

    public void SaveGame(){
        PlayerPrefs.SetInt("levelsCompleted", levelsCompleted);
        PlayerPrefs.SetInt("score", score);
    }

    public void LoadGame(){
        if(PlayerPrefs.HasKey("score")){
            levelsCompleted = PlayerPrefs.GetInt("levelsCompleted");
            score = PlayerPrefs.GetInt("score");
        }
    }
}
