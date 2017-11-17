using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

    public static GameState instance;

    public bool firstTimeLoading = true;
    public bool isLevel1Done;
    public bool isLevel2Done;
    public bool isLevel3Done;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "Intro")
        {
            if (firstTimeLoading)
            {
                StartCoroutine(DelayPlay(0));
                firstTimeLoading = false;
            }

            if (isLevel1Done && isLevel2Done && isLevel3Done)
            {
                StartCoroutine(DelayPlay(1));
                GameObject.Find("Cat").GetComponent<Animator>().SetBool("isHuman", true);
            }
            else
            {
                if (isLevel1Done)
                    GameObject.Find("Door_Level1").SetActive(false);
                if (isLevel2Done)
                    GameObject.Find("Door_Level2").SetActive(false);
                if (isLevel3Done)
                    GameObject.Find("Door_Level3").SetActive(false);
            }
        }

        if (scene.name == "Level1")
            isLevel1Done = true;
        if (scene.name == "Level2")
            isLevel2Done = true;
        if (scene.name == "Level3")
            isLevel3Done = true;
    }

    private IEnumerator DelayPlay(int sceneNum)
    {
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("NarrativeManager").GetComponent<NarrativeManager>().Play(sceneNum);
    }
}
