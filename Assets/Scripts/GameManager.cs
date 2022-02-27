using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private TextMeshProUGUI resultText;
    private TextMeshProUGUI bulletsText;
    public GameObject retryButton;

    private static int enemiesAlive = 3;

    // number of bullets
    public int birdShootsNumber = 2;

    private static int birdShootCounter;

    // Start is called before the first frame update
    void Start()
    {
        resultText = GameObject.Find("ResultText").GetComponent<TMPro.TextMeshProUGUI>();
        bulletsText = GameObject.Find("BulletsText").GetComponent<TMPro.TextMeshProUGUI>();

        resultText.enabled = false;
        retryButton.SetActive(false);

        birdShootCounter = birdShootsNumber;

        SlingShot.onGameOver += SlingShot_OnGameOver;
        SlingShot.onSuccess += SlingShot_OnSuccess;
        SlingShot.onShoot += SlingShot_OnShoot;

    }

    private void Update()
    {
        bulletsText.text = "Bullets: " + (birdShootCounter).ToString();
    }

    private void OnDestroy()
    {
        SlingShot.onGameOver -= SlingShot_OnGameOver;
        SlingShot.onSuccess -= SlingShot_OnSuccess;
        SlingShot.onShoot -= SlingShot_OnShoot;
    }

    private void SlingShot_OnGameOver()
    {
        AudioSource[] sounds = GetComponents<AudioSource>();
        sounds[1].Play();
        resultText.enabled = true;
        resultText.fontSize = 20;
        resultText.text = "No more angry bullets :( !";
        retryButton.SetActive(true);
    }

    private void SlingShot_OnSuccess()
    {
        resultText.enabled = true;
        resultText.color = Color.green;
        resultText.text = "Congrats!";

        AudioSource[] sounds = GetComponents<AudioSource>();
        sounds[2].Play();

        Scene m_Scene = SceneManager.GetActiveScene();

        if (m_Scene.name != "AngryBirdLevel2") 
            Invoke("GoNextLevel", 3);
        else
            retryButton.SetActive(true);
            
    }

    private void SlingShot_OnShoot()
    {
        AudioSource[] sounds = GetComponents<AudioSource>();
        sounds[0].Play();
    }

    public void RestartGame()
    {
        enemiesAlive = 3;
        birdShootCounter = birdShootsNumber;
        SceneManager.LoadScene(0);
    }

    private void GoNextLevel()
    {
        enemiesAlive = 3;
        birdShootCounter = birdShootsNumber;
        SceneManager.LoadScene("AngryBirdLevel2");
    }

    public static int GetAliveEnemiesNumber()
    {
        return enemiesAlive;
    }

    public static void SetAliveEnemiesNumber(int num)
    {
        enemiesAlive = num;
    }

    public static int GetBirdShootCounter()
    {
        return birdShootCounter;
    }

    public static void DecreseBirdShootCounter()
    {
        birdShootCounter--;
    }
}
