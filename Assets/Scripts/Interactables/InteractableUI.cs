using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InteractableItems
{
    public class InteractableUI : MonoBehaviour
    {
        public TextMeshProUGUI InteractableText;
        public GameObject InteractableGameObject;
        public GameObject InteractableObjectPicked;
        public MMF_Player FeedbackOnCollected;
        public Image ImageItem;


        public void PlayFeedbackOnCollected()
        {
            FeedbackOnCollected.StopFeedbacks();
            FeedbackOnCollected.PlayFeedbacks();
        }

    }
    
}

