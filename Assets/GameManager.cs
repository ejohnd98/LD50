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

    public SpriteModifier healthSpriteMod;
    public int levelIndex = 0;
    public int levelsCompleted = 0;
    public bool disableSaving = false;

    public GameObject[] LevelButtons;
    public Text[] LevelScoreTexts;
    public int[] levelScores;

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
        levelScores = new int[3];
    }

    private void Start() {
        if (!disableSaving){
            LoadGame();
        }
        scoreUI.text = "-";
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
        if(fraction < 0.99f){
            healthSpriteMod.FlashWhite();
        }
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
        levelScores[levelIndex-1] = Mathf.Max(levelScores[levelIndex-1], score);
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
        score = 0;
        if(index == 0){
            scoreUI.text = "-";
        }else{
            scoreUI.text = "0";
        }

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
            if(levelScores[i] > 0){
                LevelScoreTexts[i].text = levelScores[i].ToString();
            }else{
                LevelScoreTexts[i].text = "";
            }
            sprRenderer.color = col;
            
        }
    }

    public void SaveGame(){
        PlayerPrefs.SetInt("levelsCompleted", levelsCompleted);
        PlayerPrefs.SetInt("score", score);

        PlayerPrefs.SetInt("score1", levelScores[0]);
        PlayerPrefs.SetInt("score2", levelScores[1]);
        PlayerPrefs.SetInt("score3", levelScores[2]);
    }

    public void LoadGame(){
        if(PlayerPrefs.HasKey("score")){
            levelsCompleted = PlayerPrefs.GetInt("levelsCompleted");
            score = PlayerPrefs.GetInt("score");

            levelScores[0] = PlayerPrefs.GetInt("score1");
            levelScores[1] = PlayerPrefs.GetInt("score2");
            levelScores[2] = PlayerPrefs.GetInt("score3");
        }
    }
}
