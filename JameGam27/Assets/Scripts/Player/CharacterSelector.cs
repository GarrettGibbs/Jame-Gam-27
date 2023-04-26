using TMPro;
using UnityEngine;
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
        _animator.Play(_names[_characterIndex]);
        _nameText.text = _names[_characterIndex];
        //chosenCharacter = _names[_characterIndex];

        //add button handlers
        _nextButton.onClick.AddListener(OnClickNext);
        _previousButton.onClick.AddListener(OnClickPrevious);
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
