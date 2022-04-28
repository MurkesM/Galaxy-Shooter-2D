using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    
    [SerializeField] Sprite[] _liveSprites;
    [SerializeField] Image _liveImage;
    [SerializeField] Text _scoreText;
    [SerializeField] Text _ammoText;
    [SerializeField] Text _gameOverText;
    [SerializeField] Text _restartText;
    [SerializeField] Text _pressLShiftText;
    private GameManager _gameManager;

    bool _pressLShiftActive = false;
    
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: 15/15"; 
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _pressLShiftText.gameObject.SetActive(false);

        if (_gameManager == null)
            Debug.Log("GameManager is null");
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        _ammoText.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
    }
    
    public void UpdateLiveSprites(int currentLives)
    {
        _liveImage.sprite = _liveSprites[currentLives];
       
        if (currentLives < 1)
        {
            GameOverSequence();
        }
    }

    public void PressLeftShiftUI()
    {
        _pressLShiftActive = true;
        StartCoroutine(PressLeftShiftFlickerRoutine());
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.75f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.75f);
        }
    }

    IEnumerator PressLeftShiftFlickerRoutine()
    {
        while (_pressLShiftActive == true)
        {
            _pressLShiftText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            _pressLShiftText.gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            _pressLShiftText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            _pressLShiftText.gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            _pressLShiftText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            _pressLShiftText.gameObject.SetActive(false);
            _pressLShiftActive = false;
        }
    }
}