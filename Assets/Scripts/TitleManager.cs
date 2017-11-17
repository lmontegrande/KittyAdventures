using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {

    public Image fadeImage;
    public AudioClip playAudioClip;

    private bool isStarting = false;

	void Update () {
		if (Input.GetButtonDown("Pause") && !isStarting)
        {
            StartCoroutine(FadeStart());
        }
	}

    IEnumerator FadeStart()
    {
        GetComponent<AudioSource>().PlayOneShot(playAudioClip);
        isStarting = true;
        float timer = 0f;
        float fadeTime = 1f;
        while (timer <= fadeTime)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(Color.clear, Color.black, timer/fadeTime);
            yield return null;
        }

        SceneManager.LoadScene("Intro");
    }
}
