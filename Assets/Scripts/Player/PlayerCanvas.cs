using System;
using System.Collections;
using System.Collections.Generic;
using Ability.Manager;
using MoreMountains.Feedbacks;
using Player.Stats;
using Sirenix.OdinInspector;
using TooltipManager;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Canvas
{
    public class PlayerCanvas : MonoBehaviour
    {
        [Title("Parameters Panel")]
        public Slider HPSlider;


        [Title("Ability Panel")] 
        public Image Ability2Image;
        public Image Ability3Image;
        public Image Ability4Image;
        public Color OnAbilityColor;
        public Color OfAbilityColor;
        public Image CooldownAbility2;
        public Image CooldownAbility3;
        public Image CooldownAbility4;

        [Title("Inventory Panel")] 
        public GameObject PanelInventory;
        public GameObject PrefabItemInventory;
        public Transform PanelItems;
        public int SizeInventory;

        [Title("Feedbacks")] 
        public MMF_Player FeedbackActivateAbility2;
        public MMF_Player FeedbackActivateAbility3;
        public MMF_Player FeedbackActivateAbility4;

        
        
        private AbilityManager AbilityManager;


        private void Awake()
        {
            AbilityManager = GetComponentInParent<AbilityManager>();

            if (AbilityManager != null)
            {
                Ability2Image.sprite = (AbilityManager.Ability2) ? AbilityManager.Ability2.SpriteImage : null;
                Ability3Image.sprite = (AbilityManager.Ability3) ? AbilityManager.Ability3.SpriteImage : null;
                Ability4Image.sprite = (AbilityManager.Ability4) ? AbilityManager.Ability4.SpriteImage : null;
            }

            CooldownAbility2.fillAmount = 1;
            CooldownAbility3.fillAmount = 1;
            CooldownAbility4.fillAmount = 1;
        }

        private void Start()
        {
            if (AbilityManager != null)
            {
                if (AbilityManager.Ability2) AbilityManager.OnActivateAbility2 += AbilityManagerOnOnActivateAbility2;
                if (AbilityManager.Ability3) AbilityManager.OnActivateAbility3 += AbilityManagerOnOnActivateAbility3;
                if (AbilityManager.Ability4) AbilityManager.OnActivateAbility4 += AbilityManagerOnOnActivateAbility4;
            }

            if (SizeInventory > 0)
            {
                for (int i = 1; i <= SizeInventory; i++)
                {
                    Instantiate(PrefabItemInventory, Vector3.zero, Quaternion.identity, PanelItems);
                }
            }
            
            CloseInventory();
        }
        
        private void Update()
        {
            if(AbilityManager == null) return;

            if (AbilityManager.Ability2 != null)
            {
                if (AbilityManager.CanUseAbility2 == false)
                {
                    CooldownAbility2.fillAmount =
                        (AbilityManager.cooldownAbility2 - 0) / (AbilityManager.Ability2.Cooldown - 0);
                    Ability2Image.color = OfAbilityColor;
                }
            }
            
            if (AbilityManager.Ability3 != null)
            {
                if (AbilityManager.CanUseAbility3 == false)
                {
                    CooldownAbility3.fillAmount =
                        (AbilityManager.cooldownAbility3 - 0) / (AbilityManager.Ability3.Cooldown - 0);
                    Ability3Image.color = OfAbilityColor;
                }
            }
            
            if (AbilityManager.Ability4 != null)
            {
                if (AbilityManager.CanUseAbility4 == false)
                {
                    CooldownAbility4.fillAmount =
                        (AbilityManager.cooldownAbility4 - 0) / (AbilityManager.Ability4.Cooldown - 0);
                    Ability4Image.color = OfAbilityColor;
                }
            }
        }
        private void AbilityManagerOnOnActivateAbility4()
        {
            Ability4Image.color = OnAbilityColor;
            FeedbackActivateAbility4.PlayFeedbacks();
        }

        private void AbilityManagerOnOnActivateAbility3()
        {
            Ability3Image.color = OnAbilityColor;
            FeedbackActivateAbility3.PlayFeedbacks();
        }

        private void AbilityManagerOnOnActivateAbility2()
        {
            Ability2Image.color = OnAbilityColor;
            FeedbackActivateAbility2.PlayFeedbacks();
        }

        public void SetHPSlider(int value)
        {
            HPSlider.maxValue = value;
            HPSlider.value = value;
        }

        public void SetCurrentHealth(int value)
        {
            HPSlider.value = value;
        }

        public void OpenInventory()
        {
            PanelInventory.GetComponent<CanvasGroup>().alpha = 1;
            Extensions.ShowCursor();
        }

        public void CloseInventory()
        {
            PanelInventory.GetComponent<CanvasGroup>().alpha = 0;
            Extensions.HideCursor();
            TooltipSystem.Hide();
        }
    }
}


