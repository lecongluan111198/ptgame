using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotifyBoardUI : MonoBehaviour {
    [SerializeField]
    private GameObject board;
    [SerializeField]
    private Text title;
    [SerializeField]
    private Text content;

    public static NotifyBoardUI Instance = null;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake () {
        if (Instance == null) {
            Instance = this;

        }
    }

    public void isNull () {
        Debug.Log (board == null);
        Debug.Log (title == null);
    }
    public void _CancelClick () {
        board.SetActive (false);
        Time.timeScale = 1f;
    }

    public void _Active (string title, string content) {
        board.SetActive (true);
        this.title.text = title;
        this.content.text = content;
        Time.timeScale = 0f;
    }
}