using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private CharacterSelector _playerOneCharacterSelector;
    [SerializeField] private CharacterSelector _playerTwoCharacterSelector;
    [SerializeField] private GameObject _menuObject;
    [SerializeField] private Button _playButton;

    [Header("Game")]
    [SerializeField] private PlayerMovement _playerOne;
    [SerializeField] private PlayerMovement _playerTwo;
    [SerializeField] private GameObject[] _characterPrefabs;
    [SerializeField] private GridManager _gridManager;


    [Header("Countdown")]
    [SerializeField] private GameObject _3Image;
    [SerializeField] private GameObject _2Image;
    [SerializeField] private GameObject _1Image;
    [SerializeField] private GameObject _MOWImage;
    
    [SerializeField] private TMP_Text _WinnerText;
    [SerializeField] private GameObject _EndButtons;

    [Header("References")]
    public AudioManager audioManager;
    [SerializeField] Timer timer;
    public ScoreBar scoreBar;
    [SerializeField] InSceneSettings settings;
    public bool inGame = false;

    private void Awake() 
    {
        audioManager = FindObjectOfType<AudioManager>();
        if(audioManager.startRightAway) StartGame();
        audioManager.startRightAway = false;
    }

    private void Start()
    {
        _playButton.onClick.AddListener(StartGame);
    }

    public async void StartGame()
    {
        StartCountdownAnimation();
        _gridManager.OnStartGame();
        timer.gameObject.SetActive(true);
        scoreBar.gameObject.SetActive(true);
        await Task.Delay(1000);
        _playerOne.OnGridSetup();
        _playerTwo.OnGridSetup();
        InitPlayers();
        _playerOne.OnCharacterInstantiate();
        _playerTwo.OnCharacterInstantiate();
        await Task.Delay(3000);
        inGame = true;
        timer.StartTimer();
    }

    public void InitPlayers()
    {
        Instantiate(_characterPrefabs.Where(x => x.name == PlayerPrefs.GetString(Player.playerOne.ToString(), "Jude")).First(), _playerOne.transform, false);
        Instantiate(_characterPrefabs.Where(x => x.name == PlayerPrefs.GetString(Player.playerTwo.ToString(), "Jude")).First(), _playerTwo.transform, false);
    }

    public void StartCountdownAnimation()
    {
        ResetCountdown();
        audioManager.PlayCountdown(false);
        _menuObject.SetActive(false);
        LeanTween.alphaCanvas(_3Image.GetComponent<CanvasGroup>(), 1, 0);
        LeanTween.scale(_3Image, new Vector3(1.5f, 1.5f, 1.5f), 1);
        LeanTween.alphaCanvas(_3Image.GetComponent<CanvasGroup>(), 0, 1);

        LeanTween.alphaCanvas(_2Image.GetComponent<CanvasGroup>(), 1, 0).setDelay(1);
        LeanTween.scale(_2Image, new Vector3(1.5f, 1.5f, 1.5f), 1).setDelay(1);
        LeanTween.alphaCanvas(_2Image.GetComponent<CanvasGroup>(), 0, 1).setDelay(1);

        LeanTween.alphaCanvas(_1Image.GetComponent<CanvasGroup>(), 1, 0).setDelay(2);
        LeanTween.scale(_1Image, new Vector3(1.5f, 1.5f, 1.5f), 1).setDelay(2);
        LeanTween.alphaCanvas(_1Image.GetComponent<CanvasGroup>(), 0, 1).setDelay(2);

        LeanTween.alphaCanvas(_MOWImage.GetComponent<CanvasGroup>(), 1, 0).setDelay(3);
        LeanTween.scale(_MOWImage, new Vector3(1.5f, 1.5f, 1.5f), 1).setDelay(3);
        LeanTween.alphaCanvas(_MOWImage.GetComponent<CanvasGroup>(), 0, 1).setDelay(3);
    }

    private void ResetCountdown() {
        LeanTween.alphaCanvas(_3Image.GetComponent<CanvasGroup>(), 0, 0);
        LeanTween.alphaCanvas(_2Image.GetComponent<CanvasGroup>(), 0, 0);
        LeanTween.alphaCanvas(_2Image.GetComponent<CanvasGroup>(), 0, 0);
        LeanTween.alphaCanvas(_MOWImage.GetComponent<CanvasGroup>(), 0, 0);
    }

    public async void EndGame() {
        inGame = false;
        settings.gameEnded = true;
        audioManager.StopMowerIdle(1);
        audioManager.StopMowerIdle(2);
        int winner = scoreBar.GetWinner();
        if (winner == 0) _WinnerText.text = "TIE!";
        else if (winner == 1) _WinnerText.text = $"{PlayerPrefs.GetString(Player.playerOne.ToString()).ToUpper()} WINS!";
        else if (winner == 2) _WinnerText.text = $"{PlayerPrefs.GetString(Player.playerTwo.ToString()).ToUpper()} WINS!";
        _WinnerText.gameObject.SetActive(true);
        LeanTween.scale(_WinnerText.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 2);
        await Task.Delay(5000);
        _EndButtons.SetActive(true);
    }
}
