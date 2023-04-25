using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    private void Start()
    {
        _playButton.onClick.AddListener(StartGame);
    }

    public async void StartGame()
    {
        StartCountdownAnimation();
        _gridManager.OnStartGame();
        await Task.Delay(1000);
        _playerOne.OnGridSetup();
        _playerTwo.OnGridSetup();
        InitPlayers();
        _playerOne.OnCharacterInstantiate();
        _playerTwo.OnCharacterInstantiate();
    }

    public void InitPlayers()
    {
        Instantiate(_characterPrefabs.Where(x => x.name == _playerOneCharacterSelector.chosenCharacter).First(), _playerOne.transform, false);
        Instantiate(_characterPrefabs.Where(x => x.name == _playerTwoCharacterSelector.chosenCharacter).First(), _playerTwo.transform, false);
    }

    public void StartCountdownAnimation()
    {
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
}
