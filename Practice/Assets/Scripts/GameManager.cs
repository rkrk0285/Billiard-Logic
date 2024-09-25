using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("GameObject")]
    [SerializeField] private GameObject[] turns;

    [Header("Components")]
    [SerializeField] private Button readyButton;
    private int _turnIndex;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        _turnIndex = 0;        
    }
    public void GoNextTurn()
    {
        _turnIndex++;
        if (_turnIndex >= turns.Length)
        {
            _turnIndex -= 4;
        }
        readyButton.interactable = true;
    }

    public void OnClickReadyButton()
    {
        turns[_turnIndex].GetComponent<BallController>().ChangeState(E_BallState.Ready);
        readyButton.interactable = false;
    }
    public void OnClickReloadButton()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
