using System;
using System.Collections;
using System.Collections.Generic;
using AI.Stats;
using UnityEngine;
using UnityEngine.UI;


namespace AI.Manager
{
    public class CanvasEnemyManager : MonoBehaviour
    {
        private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 1f;
        private const float DAMAGED_HEALTH_FADE_APPEAR = 3f;
        
        
        
        public Image HealthBar;
        public Image HeathBarFade;
        public CanvasGroup CanvasGroup;
        public float DamageHealthFadeTimer;
        public float DamageHealthFadeAppear;


        private EnemyStats _enemyStats;


        private void Awake()
        {
            _enemyStats = GetComponentInParent<EnemyStats>();
        }

        private void Update()
        {
            FadeHealthBar();

            FadeHealthSlider();
        }

        private void FadeHealthSlider()
        {
            DamageHealthFadeAppear -= Time.deltaTime;

            if (DamageHealthFadeAppear < 0)
            {
                if (CanvasGroup.alpha > 0)
                {
                    float speed = 5f;
                    CanvasGroup.alpha -= speed * Time.deltaTime;
                }

            }
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
            CanvasGroup.alpha = 0;
        }

        public void UpdateHealthValue(float value)
        {
            HealthBar.fillAmount = value;
            CanvasGroup.alpha = 1;
            DamageHealthFadeAppear = DAMAGED_HEALTH_FADE_APPEAR;
            DamageHealthFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;
        }
    }

}
