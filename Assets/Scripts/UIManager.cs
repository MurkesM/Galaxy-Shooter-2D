using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    
    [SerializeField] Sprite[] _liveSprites;
    [SerializeField] Image _liveImage;
    [SerializeField] Text _scoreText;
    [SerializeField] Text _gameOverText;
    [SerializeField] Text _restartText;
    private GameManager _gameManager;
    
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        if (_gameManager == null)
            Debug.Log("GameManager is null");
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }
    
    public void UpdateLiveSprites(int currentLives)
    {
        _liveImage.sprite = _liveSprites[currentLives];
       
        if (currentLives < 1)
        {
            GameOverSequence();
        }
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
}