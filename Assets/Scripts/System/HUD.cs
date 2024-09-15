using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    public GameObject[] lifePoint;
    public GameObject gameOver;
    public int hp;
    private int hpPreviour;
    public bool inPlay;
    private void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        hp = scene.buildIndex == 2 ? PlayerPrefs.GetInt("life"): lifePoint.Length;
        inPlay = scene.buildIndex == 1;
    }
    void Start()
    {
        UpdateHUD();
    }

    void Update()
    {
        hp = hp > lifePoint.Length ? lifePoint.Length : hp < 0 ? 0 : hp;

        if (hpPreviour != hp)
        {
            hpPreviour = hp;
            UpdateHUD();
        }

        gameOver.SetActive(hp <= 0);
    }

    void UpdateHUD()
    {
        for (int index = 0; index < lifePoint.Length; index++)
        {
            lifePoint[index].SetActive(hp > index);
        }
    }
}
