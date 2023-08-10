using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Light beep")]
    public Light blipLight;
    public float defaultLightRange;
    public float finalLightRange;
    public float defaultLightIntensity;
    public float finalLightIntensity;

    public float lerpDuration = 2f;
    public float degradeDuration;

    private float currentTime = 0f;

    public float currentRange;
    public float currentIntensity;

    public bool blipping;
    public bool unblipping;

    public float bleepCd;

    [Header("Curse")]
    public GameObject curseObj;
    public Transform curseStartPoint;
    public Transform curseFinalPoint;
    public float curseSpeed;
    public bool curseMove;

    [Header("Player")]
    public GameObject player;

    [Header("SoundEndLevel")]
    public AudioSource audioLevelEnd;

    [Header("Slider")]
    public Slider playerSlide;
    public Slider curseSlide;
    public Transform startPoint;

    [Header("EndGame")]
    public GameObject endGamePanel;


    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            // If the instance is null, find or create the object in the scene
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    // Update is called once per frame
    private void Start() 
    {
        audioLevelEnd.Play();
    }
    void Update()
    {
        blipLight.range = currentRange;
        if(blipping)
        {

            currentTime += Time.deltaTime;

            // Calculate the interpolation fraction (t) as the current time divided by the interpolation duration
            float t = currentTime / lerpDuration;

            // Use Mathf.Clamp01 to ensure that t stays between 0 and 1
            t = Mathf.Clamp01(t);

            // Interpolate the value from startValue to endValue using Mathf.Lerp
            float interpolatedValue1 = Mathf.Lerp(defaultLightRange, finalLightRange, t);
            

            // For example, you can use it to move an object along a straight path:
            currentRange = interpolatedValue1;
            //currentIntensity = interpolatedValue2;

            if (currentTime >= lerpDuration)
            {
                currentTime = 0;
                blipping = false;
                unblipping = true;
            }
        }
        else if(unblipping)
        {
            currentTime += Time.deltaTime;

            // Calculate the interpolation fraction (t) as the current time divided by the interpolation duration
            float t = currentTime / degradeDuration;

            // Use Mathf.Clamp01 to ensure that t stays between 0 and 1
            t = Mathf.Clamp01(t);

            // Interpolate the value from startValue to endValue using Mathf.Lerp
            float interpolatedValue2 = Mathf.Lerp(finalLightRange ,defaultLightRange, t);

            // For example, you can use it to move an object along a straight path:
            currentRange = interpolatedValue2;

            if (currentTime >= degradeDuration)
            {
                currentTime = 0f;
                unblipping = false;
                StartCoroutine(beepCD());
            }
        }
        if (curseMove)
        {
            curseObj.transform.Translate(0,0,curseSpeed*Time.deltaTime);
        }   
        playerSlide.value = player.transform.position.z + 21;
        curseSlide.value = curseObj.transform.position.z + 21;
    }

    IEnumerator beepCD()
    {
        yield return new WaitForSeconds(bleepCd);
        audioLevelEnd.pitch = Random.Range(1f, 1.5f);
        audioLevelEnd.Play();
        blipping = true;
    }

    public void EndTheGame()
    {
        endGamePanel.SetActive(true);
        curseMove = false;
    }
    
}
