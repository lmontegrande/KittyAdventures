using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeManager : MonoBehaviour {

    public Image miniFadeImage;
    public NarrativeInstance[] narratives;
    [Tooltip("e.g. 0, 1, 2 <- this gives you a scene that plays narratives 0, 1, and 2")]
    public string[] narrativeScenes;
    public bool playOnStart;
    public float fadeTime = .5f;
    public float readTime = 2f;

    private Color startingMiniFadeColor;

    public void Start()
    {
        startingMiniFadeColor = miniFadeImage.color;
        miniFadeImage.color = Color.clear;
        miniFadeImage.gameObject.SetActive(true);
        if (playOnStart)
            Play(0);
    }

    public void Play(int narrativeSceneNum) {
        List<NarrativeInstance> scene = new List<NarrativeInstance>();
        foreach(string sceneNumString in narrativeScenes[narrativeSceneNum].Split(',')) {
            int sceneNum = System.Int32.Parse(sceneNumString);
            scene.Add(narratives[sceneNum]);
        }
        StartCoroutine(Play(scene));
    }

    public IEnumerator Play(List<NarrativeInstance> scene)
    {
        // Fade In
        float fadeTimer = 0;
        while (fadeTimer < fadeTime)
        {
            fadeTimer += Time.deltaTime;
            miniFadeImage.color = Color.Lerp(Color.clear, startingMiniFadeColor, fadeTimer/fadeTime);
            yield return null;
        }
        miniFadeImage.color = startingMiniFadeColor;

        // Play
        foreach (NarrativeInstance narrativePrefab in scene)
        {
            NarrativeInstance narrative = Instantiate(narrativePrefab);
            narrative.Play();
            float readTimer = 0f;
            while (!narrative.isDone)
            {
                if (narrative.isDisplaying)
                    readTimer += Time.deltaTime;
                if (Input.GetButtonDown("Pause") || (readTimer >= readTime && !narrative.isFadingOut))
                {
                    narrative.Stop();
                }
                yield return null;
            }
            Destroy(narrative.gameObject);
        }

        // Fade out
        fadeTimer = 0;
        while (fadeTimer < fadeTime)
        {
            fadeTimer += Time.deltaTime;
            miniFadeImage.color = Color.Lerp(startingMiniFadeColor, Color.clear, fadeTimer / fadeTime);
            yield return null;
        }
        miniFadeImage.color = Color.clear;
    }
}
