using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Overrank.Patching;
using UnityEngine;

namespace Overrank.Behaviors
{
    public class PPChapterSelect : LinkedBehavior<ChapterSelectButton, PPChapterSelect>
    {
        public override void OnDisable()
        {
            base.OnDisable();

        }

        public void CheckPP()
        {
            Overrank.Log("Chapter PP Check");
            if (parent.notComplete)
            {
                return;
            }

            int ppCount = 0;
            foreach (var item in parent.layersInChapter)
            {
                if (PPLayerSelect.AttachLink(item).pp)
                {
                    ppCount++;
                }
            }
            Overrank.Log($"Chapter PP Count: {ppCount} == {parent.layersInChapter.Length}");

            if (ppCount == parent.layersInChapter.Length)
            {
                parent.rankText.fontSize = 27;
                parent.rankText.text = $"<color=#FFFFFF>{Database.Resource.ppRankName}</color>";
                parent.rankButton.color = Database.Resource.ppRankColor;
                parent.rankButton.sprite = parent.rankOnP;
                if (parent.golds == parent.layersInChapter.Length)
                {
                    parent.buttonBg.color = Database.Resource.ppRankColor;
                    parent.buttonBg.sprite = parent.buttonOnP;
                }

                return;
            }
            else parent.rankText.fontSize = 30;
        }
    }
}
