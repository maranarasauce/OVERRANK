using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overrank.Patching
{
    public class PPLayerSelect
    {
        #region LINK_LOGIC
        private static Dictionary<LayerSelect, PPLayerSelect> Link = new Dictionary<LayerSelect, PPLayerSelect>();
        public static PPLayerSelect AddLayerSelect(LayerSelect select)
        {
            if (Link.TryGetValue(select, out PPLayerSelect np))
                return np;
            PPLayerSelect ppSelect = new PPLayerSelect();
            ppSelect.ls = select;
            Link.Add(select, ppSelect);

            foreach (var key in Link.Keys.ToArray())
            {
                if (key == null)
                    Link.Remove(key);
            }

            return ppSelect;
        }
        public static void PopLayerSelect(LayerSelect select)
        {
            select.rankText.fontSize = 60;
            Link.Remove(select);
        }
        public static void AchievePP(LayerSelect select)
        {
            PPLayerSelect ps = null;
            if (!Link.TryGetValue(select, out ps))
            {
                ps = AddLayerSelect(select);
            }

            ps.PP();
        }
        public static bool CheckPP(LayerSelect select)
        {
            if (Link.TryGetValue(select, out PPLayerSelect ps))
            {
                return ps.CheckPP();
            }
            else return false;
        }
        #endregion

        private bool pp;
        private int pps;
        private LayerSelect ls;
        private void PP()
        {
            if (pps < ls.levelAmount)
                pps++;
            CheckPP();
        }

        private bool CheckPP()
        {
            Overrank.Log($"Checking PP for {ls.layerNumber}... {pps}");
            if (pps == ls.levelAmount)
            {
                if (ls.levelAmount != 0 && (ls.noSecretMission || ls.secretMission))
                {
                    ls.GetComponent<Image>().color = Color.Lerp(Database.Resource.ppRankColor, Color.clear, 0.5f);
                    pp = true;
                }

                ls.rankText.text = $"<color=#FFFFFF>{Database.Resource.ppRankName}</color>";
                ls.rankText.fontSize = 40;
                ls.rankImage.color = Database.Resource.ppRankColor;
                ls.rankImage.sprite = ls.rankSpriteOnP;
                return true;
            }
            
            ls.rankText.fontSize = 60;
            return false;
        }
    }
}
