using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Misc")]
    [SerializeField] private MusicController _musicController;
    [SerializeField] private GameObject globalVolume;
    [SerializeField] private CameraController playerCamera;
    [SerializeField] private ChunkLoader _chunkLoader;

    [Header("Entities")]
    [SerializeField] private BorderBehavior borderBehavior;
    [SerializeField] private BasicCharacterController player;
    [SerializeField] private List<PredatorController> dinosaurs;
    
    [Header("Animation Settings")]
    [SerializeField] private Vector3 normalPos;
    [SerializeField] private Vector3 exitPos;
    [SerializeField] private Vector3 enterPos;
    [SerializeField] private float speed;
    
    [Header("UI")]
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private DisplayGameover _gameoverDisplay;
    [SerializeField] private BabyThrowing _eggThrowBehavior;
    [SerializeField] private GameObject _ddrDisplay;

    private void Start()
    {
        PauseGame();
        gameUI.SetActive(false);
        startButton.interactable = false;
        Enter();
    }

    public void PauseGame() {
        Cursor.lockState = CursorLockMode.None;
        gameUI.SetActive(false);
        globalVolume.SetActive(true);
        player.Freeze();
        Time.timeScale = 0;
    }

    public void PlayGame()
    {
        Reset();
        globalVolume.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        player.Unfreeze();
        _musicController.playGameMusic();
    }

    public void EndResults() {
        _gameoverDisplay.Reset();
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

    private void Reset()
    {
        borderBehavior.Reset();
        playerCamera.Reset();
        player.Reset();
        foreach (PredatorController dinosaur in dinosaurs) dinosaur.Reset();
        _ddrDisplay.SetActive(false);
        _chunkLoader.Reset();
        _gameoverDisplay.DinosFed = 0;
        _eggThrowBehavior.AccumulatedAmmo = 0;
        _eggThrowBehavior.AmmoCount = 0;
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
