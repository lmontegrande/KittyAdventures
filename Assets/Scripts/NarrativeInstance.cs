using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeInstance : MonoBehaviour {

    public bool playOnStart = false;
    public bool isPlayInOrder = false;
    public float backgroundFadeInTime = 1f;
    public float characterFadeInTime = 1f;
    public float textFadeInTime = 1f;
    public float fadeOutTime = 1f;

    public GameObject narrativeCanvas;
    public Image backGroundImage;
    public Image characterImage;
    public Text text;

    private bool _isDone;
    public bool isDone {
        get { return _isDone; }
    }
    private bool _isDisplaying;
    public bool isDisplaying
    {
        get { return _isDisplaying; }
    }
    public bool isFadingOut
    {
        get { return _isFadingOut; }
    }
    private bool _isFadingOut;

    private Color originalBackgroundImageColor;
    private Color originalCharacterImageColor;
    private Color originalTextColor;

    public void Awake()
    {
        _isDone = false;
        _isDisplaying = false;
        narrativeCanvas.SetActive(false);
        originalBackgroundImageColor = backGroundImage.color;
        originalCharacterImageColor = characterImage.color;
        originalTextColor = text.color;
        backGroundImage.color = Color.clear;
        characterImage.color = Color.clear;
        text.color = Color.clear;

        if (playOnStart)
            Play();
    }

    public void Play()
    {
        narrativeCanvas.SetActive(true);
        if (isPlayInOrder)
            StartCoroutine(StartPlayOrdered());
        else
            StartCoroutine(StartPlayUnOrdered());
    }

    public void Stop()
    {
        StopAllCoroutines();
        if (!isFadingOut)
            StartCoroutine(FadeOut());
        else
            Finish();
    }

    private void Finish()
    {
        backGroundImage.color = Color.clear;
        characterImage.color = Color.clear;
        text.color = Color.clear;
        _isDone = true;
    }

    private IEnumerator FadeOut()
    {
        _isFadingOut = true;
        backGroundImage.color = originalBackgroundImageColor;
        characterImage.color = originalCharacterImageColor;
        text.color = originalTextColor;
        float timer = 0f;
        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            backGroundImage.color = Color.Lerp(originalBackgroundImageColor, Color.clear, timer / backgroundFadeInTime);
            characterImage.color = Color.Lerp(originalCharacterImageColor, Color.clear, timer / characterFadeInTime);
            text.color = Color.Lerp(originalTextColor, Color.clear, timer / textFadeInTime);
            yield return null;
        }
        Finish();
    }

    private IEnumerator StartPlayUnOrdered()
    {
        float timer = 0f;
        while(timer < backgroundFadeInTime || timer < characterFadeInTime || timer < textFadeInTime)
        {
            timer += Time.deltaTime;
            backGroundImage.color = Color.Lerp(Color.clear, originalBackgroundImageColor, timer/backgroundFadeInTime);
            characterImage.color = Color.Lerp(Color.clear, originalCharacterImageColor, timer / characterFadeInTime);
            text.color = Color.Lerp(Color.clear, originalTextColor, timer / textFadeInTime);
            yield return null;
        }
        _isDisplaying = true;
    }

    private IEnumerator StartPlayOrdered()
    {
        float timer = 0f;
        while (timer < backgroundFadeInTime)
        {
            timer += Time.deltaTime;
            backGroundImage.color = Color.Lerp(Color.clear, originalBackgroundImageColor, timer / backgroundFadeInTime);
            yield return null;
        }
        timer = 0f;
        while (timer < characterFadeInTime)
        {
            timer += Time.deltaTime;
            characterImage.color = Color.Lerp(Color.clear, originalCharacterImageColor, timer / characterFadeInTime);
            yield return null;
        }
        timer = 0f;
        while (timer < textFadeInTime)
        {
            timer += Time.deltaTime;
            text.color = Color.Lerp(Color.clear, originalTextColor, timer / textFadeInTime);
            yield return null;
        }
        _isDisplaying = true;
    }
}
