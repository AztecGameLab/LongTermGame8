using System;
using UnityEngine;

namespace poetools.Console
{
    // todo: add a scriptableObject search + create editor window!
    // that would help w/ one-off SO like this that need to be made.

    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Default Log Prefix")]
    public class DefaultLogPrefix : LogPrefix
    {
        [SerializeField]
        private Color color = Color.red;

        public override string GenerateMessage(string category)
        {
            string formattedCategory = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{category.ToLower()}</color>";
            string formattedTime = $"<b>{DateTime.Now.ToShortTimeString()}</b>";
            return $"[{formattedCategory}@{formattedTime}]  ";
        }
    }
}
