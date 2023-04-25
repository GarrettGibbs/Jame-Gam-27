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

    public Player player;
    public string chosenCharacter;

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
        _characterIndex = 0;
        _animator.Play(_names[_characterIndex]);
        _nameText.text = _names[_characterIndex];
        chosenCharacter = _names[_characterIndex];

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
        chosenCharacter = _names[_characterIndex];
    }

    private void OnClickPrevious()
    {
        if (_characterIndex == 0) _characterIndex = _names.Length -1;
        else _characterIndex--;
        _animator.Play(_names[_characterIndex]);
        _nameText.text = _names[_characterIndex];
        chosenCharacter = _names[_characterIndex];
    }
}
