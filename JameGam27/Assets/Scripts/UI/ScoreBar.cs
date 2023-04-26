using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour {
    [SerializeField] private Slider _slider1;
    [SerializeField] private Slider _slider2;

    [SerializeField] private float _number1 = 50f;
    [SerializeField] private float _number2 = 50f;

    [SerializeField] TMP_Text player1Name;
    [SerializeField] TMP_Text player2Name;

    [SerializeField] private ParticleSystem _player1Plus;
    [SerializeField] private ParticleSystem _player2Plus;
    [SerializeField] private ParticleSystem _player1Minus;
    [SerializeField] private ParticleSystem _player2Minus;
    //private float _updateAmount = 1f;

    void Start() {
        // Set the slider values to 0 initially
        _slider1.value = 0;
        _slider2.value = 0;

        player1Name.text = PlayerPrefs.GetString(Player.playerOne.ToString());
        player2Name.text = PlayerPrefs.GetString(Player.playerTwo.ToString());
    }

    //void Update()
    //{
    //    UpdateSliderValues();
    //}
    #region TEST FUNCTIONS
    public void IncreaseLeft() {
        _number1 += 5;
        UpdateSliderValues();
    }

    public void IncreaseRight() {
        _number2 += 5;
        UpdateSliderValues();
    }

    public void DecreaseLeft() {
        _number1 -= 5;
        UpdateSliderValues();
    }

    public void DecreaseRight() {
        _number2 -= 5;
        UpdateSliderValues();
    }
    #endregion

    public void UpdateLeft(int updateAmount) {
        //print(_number1);
        //print(updateAmount);
        _number1 -= updateAmount;
        if (_number1 < 0)
        {
            _player1Plus.Play();
            _player2Minus.Play();
        }
        else 
        { 
            _player1Minus.Play();
            _player2Plus.Play();
        } 

        UpdateSliderValues();
    }

    public void UpdateRight(int updateAmount) {
        _number2 -= updateAmount;
        if (_number1 < 0)
        {
            _player2Plus.Play();
            _player1Minus.Play();
        }
        else
        {
            _player2Minus.Play();
            _player1Plus.Play();
        }
        UpdateSliderValues();
    }

    private void UpdateSliderValues() {
        if (_number1 == _number2) {
            _slider1.value = 0;
            _slider2.value = 0;
        } else if (_number1 > _number2) {
            _slider1.value = (_number1 - _number2) / _number1;
            _slider2.value = 0;
        } else {
            _slider1.value = 0;
            _slider2.value = (_number2 - _number1) / _number2;
        }
        //print($"Player 1: {_number1}\nPlayer 2: {_number2}");
    }

    public int GetWinner() {
        if (_number1 == _number2) return 0;
        else if (_number1 > _number2) return 2;
        else return 1;
    }
}
