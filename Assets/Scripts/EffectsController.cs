using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectsController : MonoBehaviour
{
    public static EffectsController instance;
    private Volume volume;
    private Bloom bloom;
    private ChromaticAberration chromatic;

    float effectCounter = 0.0f;
    public float fadeSpeed = 1.0f, chromaticAberrationHit = 0.5f, chromaticAberrationPassive = 0.3f;

    float passiveChromatic = 0.0f;
    
    private void Awake(){
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        }else{
            instance = this;
        }

        volume = GetComponent<Volume>();
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out chromatic);
    }

    private void Update() {
        if(effectCounter >= 0){
            effectCounter -= Time.deltaTime * fadeSpeed;
            chromatic.intensity.value = passiveChromatic + (chromaticAberrationHit * effectCounter);
        }
    }

    public void HitEffect(){
        effectCounter = 1.0f;
    }

    public void SetPassiveCA(float ca){
        passiveChromatic = ca * chromaticAberrationPassive;
        chromatic.intensity.value = passiveChromatic + (chromaticAberrationHit * effectCounter);
    }

}
