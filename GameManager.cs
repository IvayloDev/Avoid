using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

/// <summary> TODO
/// Sounds
/// pick ups
/// score UI
/// Menu
/// 
/// </summary>

public class GameManager : MonoBehaviour {

    private enum Sides { Left, Right, Up, Down };
    private Sides side;
    private Vector3 spawnPos;

    public GameObject playerGO;

    public Text scoreText;
    public Text highScoreText;

    public GameObject followingEnemyPrefab, fastEnemyPrefab, bigEnemyPrefab;

    public GameObject EnemyIncomingIndicator;

    public int Score;
    private int highScore;


    public static GameManager instance;

    public List<GameObject> Enemies;

    private bool burst;
    public int whenToBurst;

    private int MusicInt;
    public Sprite musicOn, musicOff;
    public GameObject musicButt, playButt;

    public GameObject shieldPrefab;
    public GameObject shieldGO;
    public bool shieldUp;

    private bool activateFollowEnemy;
    private int adCount;

    private float spawnRate = 0.5f;

    private void Awake() {
        highScore = PlayerPrefs.GetInt("highScore", 0);
        MusicInt = PlayerPrefs.GetInt("music", 0);
    }

    private void Start() {

        if (instance != null) {
            Debug.Log("GameManager Null");
        } else {
            instance = this;
        }
    }

    void StartGame() {

        shieldGO.SetActive(false);

        Destroy(GameObject.FindGameObjectWithTag("Shield"));

        activateFollowEnemy = false;
        playerGO.GetComponent<MovementController>().ResetTransform();
        playerGO.SetActive(true);

        musicButt.SetActive(false);
        playButt.SetActive(false);
        highScoreText.gameObject.SetActive(false);

        Score = 0;
        burst = true;
        whenToBurst = 20;

        GameObject[] shields = GameObject.FindGameObjectsWithTag("Shield");

        for (int i = 0; i < shields.Length; i++) {
            Destroy(shields[i]);
        }

        StartCoroutine(SpawnEnemy(fastEnemyPrefab, spawnRate, false));

        StartCoroutine("SpawnShield");

    }


    private void Update() {

        scoreText.text = Score.ToString();
        highScoreText.text = "Highscore: " + highScore;

        IncreaseSpawnRate();

        if (shieldUp) {
            shieldGO.SetActive(true);
        } else {
            shieldGO.SetActive(false);
        }

        if (Score >= highScore) {
            highScore = Score;
        }

        HandleEventsInGame();

        HandleMusicOnOff();

    }

    void IncreaseSpawnRate() {

        switch (Score) {
            case 25:
                spawnRate = 0.45f;
                break;
            case 60:
                spawnRate = 0.42f;
                break;
            case 100:
                spawnRate = 0.37f;
                break;
            case 200:
                spawnRate = 0.3f;
                break;
            default:
                break;
        }
    }

    void HandleEventsInGame() {

        if (Score == whenToBurst && burst == true) {

            burst = false;
            whenToBurst += 20;

            StartCoroutine(SpawnEnemy(fastEnemyPrefab, 0.5f, true));

        }

        if (Score > 43 && !activateFollowEnemy) {

            activateFollowEnemy = true;

            StartCoroutine(SpawnEnemy(followingEnemyPrefab, 0.7f, false));

        }
    }

    void HandleMusicOnOff() {

        if (MusicInt == 1) {
            musicButt.GetComponent<Image>().sprite = musicOff;
            AudioListener.volume = 0;
        } else if (MusicInt == 0) {
            musicButt.GetComponent<Image>().sprite = musicOn;
            AudioListener.volume = 1;
        }

    }


    IEnumerator SpawnEnemy(GameObject spawnedObjectPrefab, float delay, bool _burst) {

        // Normal fast (yellow) enemies
        if (!_burst) {

            while (true) {

                yield return new WaitForSeconds(spawnRate);

                Score++;

                side = (Sides)Random.Range(0, 4);

                switch (side) {
                    case Sides.Left:
                        spawnPos = new Vector3(-0.1f, Random.value, 0);
                        break;
                    case Sides.Right:
                        spawnPos = new Vector3(1.1f, Random.value, 0);
                        break;
                    case Sides.Up:
                        spawnPos = new Vector3(Random.value, 1.1f, 0);
                        break;
                    case Sides.Down:
                        spawnPos = new Vector3(Random.value, -0.1f, 0);
                        break;
                }

                Vector3 spawnX = Camera.main.ViewportToWorldPoint(spawnPos);

                Instantiate(spawnedObjectPrefab, spawnX, Quaternion.identity);
            }
        } else {

            // BURST 

            side = (Sides)Random.Range(0, 4);

            switch (side) {
                case Sides.Left:
                    spawnPos = new Vector3(-0.1f, Random.value, 0);
                    break;
                case Sides.Right:
                    spawnPos = new Vector3(1.1f, Random.value, 0);
                    break;
                case Sides.Up:
                    spawnPos = new Vector3(Random.value, 1.1f, 0);
                    break;
                case Sides.Down:
                    spawnPos = new Vector3(Random.value, -0.1f, 0);
                    break;
            }


            Camera.main.GetComponent<CameraShake>().ShakeCamera(1.3f, 5);

            AudioManager.instance.PlaySound("WaveIn");

            yield return new WaitForSeconds(1f);


            for (int i = 0; i < 15; i++) {

                Vector3 spawnX = Camera.main.ViewportToWorldPoint(spawnPos);

                if (side == Sides.Up || side == Sides.Down) {
                    spawnX.x += i;
                } else spawnX.y += i;

                Instantiate(spawnedObjectPrefab, spawnX, Quaternion.identity);
            }

            burst = true;

        }
    }

    IEnumerator SpawnShield() {

        while (true) {

            yield return new WaitForSeconds(8);
            //check if spawned previously
            if (GameObject.FindGameObjectWithTag("Shield") != null) {
                yield return null;
            } else {

                Vector3 spawnPosRaw = new Vector3(Random.value, Random.value, 10);

                Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(spawnPosRaw);

                GameObject _shield = Instantiate(shieldPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    IEnumerator IncomingEnemyIndicator(float delay) {

        // Spawn

        while (true) {
            yield return new WaitForSeconds(delay);


            side = (Sides)Random.Range(0, 4);

            switch (side) {
                case Sides.Left:
                    spawnPos = new Vector3(-0.1f, Random.value, 10);
                    break;
                case Sides.Right:
                    spawnPos = new Vector3(1.1f, Random.value, 10);
                    break;
                case Sides.Up:
                    spawnPos = new Vector3(Random.value, 1.1f, 10);
                    break;
                case Sides.Down:
                    spawnPos = new Vector3(Random.value, -0.1f, 10);
                    break;
            }

            Vector3 spawnX = Camera.main.ViewportToWorldPoint(spawnPos);

            GameObject indicator = Instantiate(EnemyIncomingIndicator, spawnX, Quaternion.identity);

            //Set orientation
            Vector3 dir = FindObjectOfType<Player>().transform.position - indicator.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            indicator.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);



            //move forward few units
            indicator.transform.position += indicator.transform.up * 3;

            yield return new WaitForSeconds(1f);

            indicator.SetActive(false);

            Instantiate(bigEnemyPrefab, spawnX, Quaternion.identity);

            yield return new WaitForSeconds(1f);

            //Shake camera
            Camera.main.GetComponent<CameraShake>().ShakeCamera(0.3f, 5);

        }
    }


    private void OnDestroy() {

        PlayerPrefs.SetInt("highScore", highScore);
        PlayerPrefs.SetInt("music", MusicInt);
        PlayerPrefs.Save();
    }
    private void OnApplicationPause(bool pause) {
        PlayerPrefs.SetInt("highScore", highScore);
        PlayerPrefs.SetInt("music", MusicInt);
        PlayerPrefs.Save();
    }
    private void OnApplicationQuit() {
        PlayerPrefs.SetInt("highScore", highScore);
        PlayerPrefs.SetInt("music", MusicInt);
        PlayerPrefs.Save();
    }

    public void GameOver() {

        AudioManager.instance.PlaySound("Death");

        adCount++;

        if (adCount >= 5) {
            Advertisement.Show();
            adCount = 0;
        }

        GameObject[] followEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < followEnemies.Length; i++) {
            Destroy(followEnemies[i]);
        }

        StopAllCoroutines();

        playerGO.SetActive(false);


        musicButt.SetActive(true);
        playButt.SetActive(true);

        highScoreText.gameObject.SetActive(true);

    }


    public void onMusicClick() {

        AudioManager.instance.PlaySound("Click");

        if (MusicInt == 0) {
            MusicInt = 1;
            GetComponent<AudioSource>().Pause();
        } else {
            MusicInt = 0;
            GetComponent<AudioSource>().UnPause();
        }

    }

    public void OnPlayClick() {

        AudioManager.instance.PlaySound("Click");

        StartCoroutine(StartGameCoroutine());

    }

    IEnumerator StartGameCoroutine() {

        yield return new WaitForSeconds(0.5f);

        StartGame();


    }

}
