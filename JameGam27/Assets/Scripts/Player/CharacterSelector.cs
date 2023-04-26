using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum Player { playerOne, playerTwo };

public class CharacterSelector : MonoBehaviour
{
    [Header("Menu UI")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] GameManager _gameManager;

    public Player player;
    //public string chosenCharacter;

    private string[] _names;
    private int _characterIndex;

    void Start()
    {
        _names = new string[]
        {
            "Jude",
            "Manny",
            "Roy",
            "Marvin",
            "Phil",
            "Cathy",
            "Darcie",
            "Anna",
            "Hanna",
            "Stephanie"
        };

        //set default character
        _characterIndex = 50;
        string savedName = PlayerPrefs.GetString(player.ToString(), "Jude");
        for (int i = 0; i < _names.Length; i++) {
            if(savedName == _names[i]) {
                _characterIndex = i;
                break;
            }
        }
        _nameText.text = _names[_characterIndex];
        _animator.Play(savedName);
        //chosenCharacter = _names[_characterIndex];

        //add button handlers
        _nextButton.onClick.AddListener(OnClickNext);
        _previousButton.onClick.AddListener(OnClickPrevious);
    }

    private void Update() {
        if (_gameManager.inGame) return;
        if (Keyboard.current.enterKey.wasPressedThisFrame) {
            _gameManager.StartGame();
        } else if (Keyboard.current.aKey.wasPressedThisFrame && player == Player.playerOne) {
            OnClickPrevious();
        } else if (Keyboard.current.dKey.wasPressedThisFrame && player == Player.playerOne) {
            OnClickNext();
        } else if (Keyboard.current.leftArrowKey.wasPressedThisFrame && player == Player.playerTwo) {
            OnClickPrevious();
        } else if (Keyboard.current.rightArrowKey.wasPressedThisFrame && player == Player.playerTwo) {
            OnClickNext();
        }
    }

    private void OnEnable()
    {
        _animator.Play(_nameText.text);
    }

    private void OnClickNext()
    {
        if (_characterIndex == _names.Length - 1) _characterIndex = 0;
        else _characterIndex++;
        _animator.Play(_names[_characterIndex]);
        _nameText.text = _names[_characterIndex];
        //chosenCharacter = _names[_characterIndex];
        PlayerPrefs.SetString(player.ToString(), _names[_characterIndex]);
        _gameManager.audioManager.PlaySound("Switch_Click");
    }

    private void OnClickPrevious()
    {
        if (_characterIndex == 0) _characterIndex = _names.Length -1;
        else _characterIndex--;
        _animator.Play(_names[_characterIndex]);
        _nameText.text = _names[_characterIndex];
        PlayerPrefs.SetString(player.ToString(), _names[_characterIndex]);
        _gameManager.audioManager.PlaySound("Switch_Click");
    }
}
