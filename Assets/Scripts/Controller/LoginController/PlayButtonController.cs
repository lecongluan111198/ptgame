using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;
using UnityEngine.Networking;
using System.Text;
using Assets.Scripts.Logic.Network;
using System;

public class PlayButtonController : MonoBehaviour
{
    // Reference to input field
    public InputField userName;
    public InputField passWord;

    public GameObject errorMessage;
    public GameObject loadingMessage;
    public GameObject loadingMapScreen;
    public Slider slider;

    public void PlayGame()
    {
        // Set username
        LoginPlayer();
    }


    public void LoginPlayer()
    {
        CallAPILogin();
    }

    public void CallAPILogin()
    {
        //APIManager.Instance.login();
        //StartCoroutine(APIManager.Instance._Login("letuongqui", "1", (rp) =>
        //{
        //    if (rp)
        //    {
        //        StartCoroutine(_loadingMap());
        //    }
        //    else
        //    {
        //        errorMessage.SetActive(true);
        //    }
        //}));


        StartCoroutine(APIManager.Instance._Login(userName.text, passWord.text, (rp) =>
        {
            if (rp)
            {
                StartCoroutine(_loadingMap());
            }
            else
            {
                errorMessage.SetActive(true);
            }
        }));
    }

    private IEnumerator _loadingMap()
    {
        loadingMapScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(String.SCENE_MAIN);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
    }
}
