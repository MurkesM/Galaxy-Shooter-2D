using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    [SerializeField] Sprite[] _liveSprites;
    [SerializeField] Image _liveImage;
    [SerializeField] Text _scoreText;
    [SerializeField] Text _ammoText;
    [SerializeField] Text _gameOverText;
    [SerializeField] Text _restartText;
    [SerializeField] Text _pressLShiftText;
    [SerializeField] Text _bossIsDeadText;
    [SerializeField] Image _bossHealth;
    [SerializeField] Image _bossHealthBG;
    [SerializeField] Button _mainMenuButton;

    GameManager _gameManager;
    SpawnManager _spawnManager;

    bool _pressLShiftActive = false;
    bool _bossIsDead = false;
    
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: 20/20"; 
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _pressLShiftText.gameObject.SetActive(false);
        _bossIsDeadText.gameObject.SetActive(false);
        _bossHealth.gameObject.SetActive(false);
        _bossHealthBG.gameObject.SetActive(false);
        _mainMenuButton.gameObject.SetActive(false);
        _bossHealth.fillAmount = 1;

        if (_gameManager == null)
            Debug.Log("GameManager is null");
        if (_spawnManager == null)
            Debug.Log("SpawnManager is null");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _bossIsDead == true)
        {
            ContinueGame();
        }
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

    public void BossIsDeadUI()
    {
        _bossIsDead = true;
        _bossHealth.gameObject.SetActive(false);
        _bossHealthBG.gameObject.SetActive(false);
        _bossIsDeadText.gameObject.SetActive(true);
        _spawnManager.StopSpawning();
    }

    public void UpdateBossHealth()
    {
        _bossHealth.fillAmount -= 0.05f;
    }

    public void BossHealthUI()
    {
        _bossHealth.gameObject.SetActive(true);
        _bossHealthBG.gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0); //the Main Menu
    }
    
    public void PlayerIsDeadUI()
    {
        _mainMenuButton.gameObject.SetActive(true);
    }

    void ContinueGame()
    {
        _bossIsDead = false;
        _bossIsDeadText.gameObject.SetActive(false);
        _spawnManager.StartSpawning();
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