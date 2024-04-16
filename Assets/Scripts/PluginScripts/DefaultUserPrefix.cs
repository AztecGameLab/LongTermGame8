using UnityEngine;

namespace poetools.Console
{
[CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Default User Prefix")]
    public class DefaultUserPrefix : UserPrefix
    {
        [SerializeField]
        private Color prefixColor;

        public override string GenerateMessage(string input)
        {
            return $"<b><color=#{ColorUtility.ToHtmlStringRGB(prefixColor)}>></color></b> {input}";
        }
    }
}
