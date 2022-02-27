using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject gameUI;
    [SerializeField] private MusicController _musicController;
    [SerializeField] private BasicCharacterController player;
    [SerializeField] private GameObject globalVolume;
    [SerializeField] private GameObject crosshair;

    [SerializeField] private Vector3 normalPos;
    [SerializeField] private Vector3 exitPos;
    [SerializeField] private Vector3 enterPos;
    [SerializeField] private float speed;

    [SerializeField] private Button startButton;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameOver;


    private void Start()
    {
        PauseGame();
        gameUI.SetActive(false);
        startButton.interactable = false;
        Enter();
    }

    public void PauseGame() {
        Cursor.lockState = CursorLockMode.None;
        globalVolume.SetActive(true);
        player.Freeze();
        Time.timeScale = 0;
    }

    public void PlayGame() {
        globalVolume.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        player.Unfreeze();
        _musicController.playGameMusic();
    }

    public void EndResults() {
        mainMenu.SetActive(false);
        gameOver.SetActive(true);
        _musicController.playTitleMusic();
        StartCoroutine(LerpFunction(enterPos, normalPos));
    }

    public void Enter() {
        StartCoroutine(LerpFunction(enterPos, normalPos));
    }

    public void Exit()
    {
        StartCoroutine(LerpFunction(normalPos, exitPos));
        PlayGame();
        gameUI.SetActive(true);

    }

    IEnumerator LerpFunction(Vector3 from, Vector3 to)
    {
        transform.localPosition = from;
        while ((transform.localPosition - to).magnitude >= 0.5f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, to, speed * Time.unscaledDeltaTime);
            yield return null;
        }
        transform.localPosition = to;
        startButton.interactable = true;
        //Debug.Log("Made it");
    }


}
