using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;


public class Player : MonoBehaviour
{
    public float Hp;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;

    private float lastAttackTime = 0;
    private float distance;
    private float closestDistance;
    private bool isDead = false;
    public Animator AnimatorController;


    public Text HPText;
    public Button AttackButton;
    public Button DoubleAttackButton;
    private Button currentClickedButton;
    public Enemie Enemie;


    public void Update()
    {
        var enemies = SceneManager.Instance.Enemies;
        Enemie closestEnemie = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemie = enemies[i];
            if (enemie == null)
            {
                continue;
            }

            if (closestEnemie == null)
            {
                closestEnemie = enemie;
                continue;
            }

            distance = Vector3.Distance(transform.position, enemie.transform.position);
            closestDistance = Vector3.Distance(transform.position, closestEnemie.transform.position);

            if (distance < closestDistance)
            {
                closestEnemie = enemie;
            }

        }
        HPText.text = "HP:"+Hp;
        if (isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
            Die();
            return;
        }

        if (currentClickedButton == AttackButton)
        {
            if (Time.time - lastAttackTime > AtackSpeed)
            {
                AttackButton.image.fillAmount += 0.05f;
                if (AttackButton.image.fillAmount == 1)
                {
                    AttackButton.enabled = true;
                }
                else { AttackButton.enabled = false; }
            }
            if (Time.time - lastAttackTime <= AtackSpeed)
            {
                AttackButton.image.fillAmount -= 0.1f;
            }
        }
        else if (currentClickedButton == DoubleAttackButton) 
        {
            if (Time.time - lastAttackTime > AtackSpeed*2)
            {
                DoubleAttackButton.image.fillAmount += 0.05f;
                if (DoubleAttackButton.image.fillAmount == 1)
                {
                    DoubleAttackButton.enabled = true;
                }
                else { DoubleAttackButton.enabled = false; }
            }
            if (Time.time - lastAttackTime <= AtackSpeed)
            {
                DoubleAttackButton.image.fillAmount -= 0.1f;
            }
        } 

    }

    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
       
    }

  



    public void OnButtonClick(Button Clicked)
    {

        var enemies = SceneManager.Instance.Enemies;
        Enemie closestEnemie = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemie = enemies[i];
            if (enemie == null)
            {
                continue;
            }

            if (closestEnemie == null)
            {
                closestEnemie = enemie;
                continue;
            }

            distance = Vector3.Distance(transform.position, enemie.transform.position);
            closestDistance = Vector3.Distance(transform.position, closestEnemie.transform.position);

            if (distance < closestDistance)
            {
                closestEnemie = enemie;
            }

        }
        currentClickedButton = Clicked;

       
        if (Clicked == AttackButton)
        {
          //  Debug.Log("Button 1 clicked");
            if (closestEnemie != null)
            {
              
                if (distance <= AttackRange)
                {
                    if (Time.time - lastAttackTime > AtackSpeed && AttackButton.image.fillAmount == 1)
                    {
                        Damage = 1;
                        //transform.LookAt(closestEnemie.transform);
                        transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);

                        lastAttackTime = Time.time;
                        closestEnemie.Hp -= Damage;
                        AnimatorController.SetTrigger("Attack");
                        if (closestEnemie.Hp <= 0 && closestEnemie.tag == "Double")
                        {


                            Enemie.Hp = 1;
                            closestEnemie.tag = "Smoll";
                            Instantiate(closestEnemie, closestEnemie.transform.position, Quaternion.identity); closestEnemie.Hp = 1; closestEnemie.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                            Instantiate(closestEnemie, closestEnemie.transform.position, Quaternion.identity); closestEnemie.Hp = 1; closestEnemie.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

                            closestEnemie.tag = "Untagged";


                        }
                    }

                }
            }
            AnimatorController.SetTrigger("Attack");
        }
        else if (Clicked == DoubleAttackButton)
        {
         //   Debug.Log("Button 2 clicked");
            if (closestEnemie != null)
            {
               
                if (distance <= AttackRange)
                {
                    if (Time.time - lastAttackTime > AtackSpeed && AttackButton.image.fillAmount == 1)
                    {
                        Damage = 2;
                        //transform.LookAt(closestEnemie.transform);
                        transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);

                        lastAttackTime = Time.time;
                        closestEnemie.Hp -= Damage;
                        AnimatorController.SetTrigger("DoubleAttack");

                        if (closestEnemie.Hp <= 0 && closestEnemie.tag == "Double")
                        {
                            

                            Enemie.Hp = 1;
                                 closestEnemie.tag = "Smoll";
                                Instantiate(closestEnemie, closestEnemie.transform.position, Quaternion.identity); closestEnemie.Hp = 1; closestEnemie.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                                Instantiate(closestEnemie, closestEnemie.transform.position, Quaternion.identity); closestEnemie.Hp = 1; closestEnemie.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                            
                            closestEnemie.tag = "Untagged";
                        
                           
                        }
                    }
                }
                if (distance> AttackRange) { DoubleAttackButton.enabled = false;  }
            }
        
        }
    }


}
