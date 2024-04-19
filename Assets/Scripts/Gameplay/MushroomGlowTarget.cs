using Ltg8.Inventory;
using UnityEngine;

namespace Ltg8.Gameplay
{
    public class MushroomGlowTarget : ItemTarget
    {
        [SerializeField] private float maxBrightness;
        [SerializeField] private float duration;
        [SerializeField] private Light lightSource;
        [SerializeField] private float animSpeed = 7;

        private float _remainingTime;

        private void Update()
        {
            _remainingTime -= Time.deltaTime;
            float t = Mathf.Clamp01(_remainingTime / duration);
            lightSource.intensity = Mathf.Lerp(lightSource.intensity, t * maxBrightness, animSpeed * Time.deltaTime);
        }

        public override bool CanReceiveItem(ItemData data)
        {
            return data.guid == "items/glowshroom";
        }

        public override bool WillConsumeItem() => false;

        public override void ReceiveItem(ItemData item)
        {
            base.ReceiveItem(item);
            _remainingTime = duration;
        }
    }
}
