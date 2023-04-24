using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioManager audioManager;
    public bool inGame = false;

    private void Awake() {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start() {
        inGame = true;
    }
}
