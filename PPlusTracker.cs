using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overrank
{
    public class PPlusTracker : MonoSingleton<PPlusTracker>
    {
        private NewMovement mov;
        private StyleHUD hud;
        private OverrankResource config;
        private float styleTimer;
        private bool neverHurt;
        private bool passedStyle;

        private Slider styleMeter;
        private Image styleMeterFill;
        private GameObject healthFailUI;

        private void Awake()
        {
            config = Database.Resource;
            mov = NewMovement.Instance;
            hud = StyleHUD.Instance;
            neverHurt = true;

        }

        public void SetUI(GameObject inst)
        {
            styleMeter = inst.GetComponentInChildren<Slider>();
            styleMeterFill = styleMeter.fillRect.gameObject.GetComponent<Image>();
            healthFailUI = inst.transform.Find("Health Failed").gameObject;
            healthFailUI.gameObject.GetComponent<TextMeshProUGUI>().text = $"GOT BELOW {config.healthRequirement} HP";

            if (!neverHurt)
            {
                healthFailUI.SetActive(true);
                styleMeter.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (mov.hp < config.healthRequirement)
            {
                neverHurt = false;
                if (healthFailUI)
                {
                    healthFailUI.SetActive(true);
                    styleMeter.gameObject.SetActive(false);
                }
                enabled = false;
            }

            if (hud.rankIndex > 7)
            {
                int index = (hud.rankIndex - 8);
                float speed = config.styleTimeSpeeds[index];
                styleTimer += speed * Time.deltaTime;
                if (styleTimer > config.styleTime)
                {
                    passedStyle = true;
                }
            } else
            {
                styleTimer = 0f;
            }

            if (styleMeter != null)
            {
                if (passedStyle)
                {
                    styleMeterFill.color = Color.green;
                    styleMeter.value = 1f;
                }
                else
                {
                    styleMeter.value = styleTimer / config.styleTime;
                }
            }
        }

        public bool AchievedUPlus()
        {
            return passedStyle && neverHurt;
        }
    }
}
