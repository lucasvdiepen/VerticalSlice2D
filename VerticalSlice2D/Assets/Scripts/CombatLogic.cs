using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CombatLogic : MonoBehaviour
{
    [SerializeField] private GameObject combatUIHolder;
    private Vector2 movement;
    private int deadSliderCount;
    [HideInInspector] public bool isInCombat = false;
    [SerializeField] private GameObject[] sliders;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private float moveSpeed;
    private int normalHits = 0;
    private int criticalHits = 0;

    public IEnumerator StartCombat() 
    {
        //randomizing the position 
        isInCombat = true;
        combatUIHolder.SetActive(true);
        for (int i = 0; i < sliders.Length; i++) 
        {
            sliders[i].SetActive(true);
            sliders[i].transform.position = spawnPositions[i].position + new Vector3(UnityEngine.Random.Range(0, 8f), 0, 0);
            yield return new WaitForSeconds(UnityEngine.Random.Range(.05f, .1f));
        }
    }
    private void Update() 
    {
        //slider movement
        SliderMovement();
        //do things when press button
        HandleInput();
        
    }
    private void HandleInput() 
    {
        //test input for debugging the combat
        if (Input.GetKeyDown(KeyCode.Backspace) && !isInCombat) 
        {
            StartCoroutine(StartCombat());
        }

        //hit slider
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            GameObject obj = GetNearestSlider();
            if(obj != null) {
                FindObjectOfType<AudioManager>().Play("Slash");
                obj.GetComponent<CombatSlider>().canHit = true;
                obj.GetComponent<CombatSlider>().CheckHit();
            }
        }
    }
    private void SliderMovement() 
    {
        //moving the sliders
        movement = new Vector2(-moveSpeed, 0);
        if (isInCombat) 
        {
            for (int i = 0; i < sliders.Length; i++) 
            {
                sliders[i].transform.position = new Vector3(sliders[i].transform.position.x - moveSpeed * Time.deltaTime, sliders[i].transform.position.y, 0);
            }
        }
    }

    //deactivates and resets a specific slider and stops combat if all sliders are dead
    public void SliderDied(GameObject slider) 
    {
        slider.SetActive(false);
        deadSliderCount++;
        if (deadSliderCount >= sliders.Length) 
        {
            isInCombat = false;
            for (int i = 0; i < sliders.Length; i++) 
            {
                sliders[i].transform.position = spawnPositions[i].position;
            }

            deadSliderCount = 0;

            Invoke("CombatDone", 1f);
        }
    }

    public void RegisterHit(bool critical)
    {
        if (critical) criticalHits++;
        else normalHits++;
    }

    private void CombatDone()
    {
        int damageToDeal = (120 * criticalHits) + (72 * normalHits);

        GameObject enemy = FindObjectOfType<GameManager>().GetCurrentEnemy();
        enemy.GetComponent<Health>().TakeDamage(damageToDeal);

        normalHits = 0;
        criticalHits = 0;

        combatUIHolder.SetActive(false);

        FindObjectOfType<GameManager>().RunCombatAnimations();
        
        FindObjectOfType<GameManager>().DoNextAction();
    }

    //returns the nearest slider to refresh the update order
    public GameObject GetNearestSlider() 
    {
        GameObject nearestSlider = null;
        for (int i = 0; i < sliders.Length; i++) 
        {
            if (sliders[i].gameObject.activeSelf) 
            {
                if (nearestSlider == null) 
                {
                    nearestSlider = sliders[i];
                }
                else 
                {
                    if (sliders[i].GetComponent<RectTransform>().anchoredPosition.x < nearestSlider.GetComponent<RectTransform>().anchoredPosition.x) 
                    {
                        nearestSlider = sliders[i];
                    }
                }
            }
        }
        return nearestSlider;
    }
}
        
       

