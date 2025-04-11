using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overrank.Patching
{
    public class PPLayerSelect : LinkedBehavior<LayerSelect, PPLayerSelect>
    {
        public override void OnDisable()
        {
            Overrank.Log($"Layer Select Reset {parent.layerNumber}");
            pp = false;
            pps = 0;
        }

        public bool pp;
        private int pps;
        public void GivePP()
        {
            if (pps < parent.levelAmount)
                pps++;
            CheckPP();
        }

        public void CheckScores()
        {
            pp = false;
            pps = 0;
        }

        public bool CheckPP()
        {
            if (parent.scoresChecked != parent.levelAmount)
            {
                return false;
            }

            if (pps == parent.levelAmount)
            {
                if (parent.levelAmount != 0 && (parent.noSecretMission || parent.secretMission))
                {
                    parent.GetComponent<Image>().color = Color.Lerp(Database.Resource.ppRankColor, Color.clear, 0.5f);
                    pp = true;
                }

                parent.rankText.text = $"<color=#FFFFFF>{Database.Resource.ppRankName}</color>";
                parent.rankText.fontSize = 40;
                parent.rankImage.color = Database.Resource.ppRankColor;
                parent.rankImage.sprite = parent.rankSpriteOnP;
                return true;
            }
            
            parent.rankText.fontSize = 60;
            return false;
        }
    }
}
