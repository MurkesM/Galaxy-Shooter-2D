using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text _scoreText;
    [SerializeField] Sprite[] _liveSprites;
    [SerializeField] Image _liveImage;
    [SerializeField] Text _gameOverText;
    private bool _gameIsOver = false;
    
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
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
            _gameIsOver = true;
            StartCoroutine(GameOverFlickerRoutine());
        }
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(_gameIsOver == true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.75f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.75f);
        }
    }
}
