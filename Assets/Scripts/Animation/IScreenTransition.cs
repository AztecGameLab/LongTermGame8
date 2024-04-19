using Cysharp.Threading.Tasks;

namespace Ltg8
{
    public interface IScreenTransition
    {
        UniTask Show();
        UniTask Hide();
    }
}
