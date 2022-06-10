using System;
using System.Collections;
using System.Collections.Generic;
using AI.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AI.Manager
{
    public class CanvasBossManager : MonoBehaviour
    {
        private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 1f;

        
        public Image HealthBar;
        public Image HeathBarFade;
        public float DamageHealthFadeTimer;
        public GameObject HUDBossPanel;
        public TextMeshProUGUI BossName;


        private EnemyStats _enemyStats;


        private void Awake()
        {
            _enemyStats = GetComponentInParent<EnemyStats>();
        }

        private void Update()
        {
            FadeHealthBar();
        }
        

        private void FadeHealthBar()
        {
            DamageHealthFadeTimer -= Time.deltaTime;

            if (DamageHealthFadeTimer < 0)
            {
                if (_enemyStats.healthSystem.GetHealthNormalized() < HeathBarFade.fillAmount)
                {
                    float speed = 1f;

                    HeathBarFade.fillAmount -= speed * Time.deltaTime;
                }
            }
        }

        public void StartHealthValue(float value)
        {
            HealthBar.fillAmount = value;
            HeathBarFade.fillAmount = value;
        }

        public void UpdateHealthValue(float value)
        {
            HealthBar.fillAmount = value;
            DamageHealthFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;
        }

        public void SetHUDBossPanel(bool value)
        {
            HUDBossPanel.SetActive(value);
            BossName.text = _enemyStats.gameObject.name;
        }
    }
}