using Ltg8.Inventory;
using UnityEngine;

namespace Ltg8
{
    public static class Ltg8
    {
        public static Ltg8Settings Settings;
        public static ISaveSerializer Serializer;
        public static SaveData Save;
        public static FmodValueAnimator FmodValueAnimator;
        public static PersistentAudio PersistentAudio;
        public static AsyncStateMachine<IGameState> GameState;
        public static TextBoxPresenter TextBoxPresenter;
        public static Ltg8Controls Controls;
        public static Camera MainCamera;
        public static ItemRegistry ItemRegistry;
    }
}
