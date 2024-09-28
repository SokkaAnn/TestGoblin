using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    public Player Player;
    public List<Enemie> Enemies;
    public GameObject Lose;
    public GameObject Win;
    public Text TWave;
    public Text CWave;

    public Enemie Enemie;

    private int currWave = 0;
    [SerializeField] private LevelConfig Config;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnWave();
    }

    public void AddEnemie(Enemie enemie)
    {
        Enemies.Add(enemie);
    }

    public void RemoveEnemie(Enemie enemie)
    {
        Enemies.Remove(enemie);
        Player.Hp += 1;
        if(Enemies.Count == 0)
        {
            SpawnWave();
        }
    }

    public void GameOver()
    {
        Lose.SetActive(true);
    }

    private void SpawnWave()
    {
        if (currWave >= Config.Waves.Length)
        {
            Win.SetActive(true);
            CWave.gameObject.SetActive(false);
            TWave.gameObject.SetActive(false);
            return;
        }

        var wave = Config.Waves[currWave];
        CWave.text= "Current Wave: "+ (currWave+1);
        TWave.text = "Total Waves: " + Config.Waves.Length;



        

        foreach (var character in wave.Characters)
        {
            character.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
            Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            if (currWave == Random.Range(0, 6))
            { character.tag = "Double";
                if (character.tag == "Double")
                {
                    Enemie.Hp = 3;
                    Enemie.Damage = 2;
                    
                    character.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
                   
                }
                else character.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
                Instantiate(character, pos, Quaternion.identity);
            }
            else
            {
                character.tag = "Untagged";
                Instantiate(character, pos, Quaternion.identity); character.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
            }
          

          
        }
        currWave++;

    }

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    

}
