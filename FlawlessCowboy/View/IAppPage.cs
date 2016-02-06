using CortanaExtension.Shared.Utility.Cortana;
using System.Threading.Tasks;

namespace FlawlessCowboy.View
{
    public interface IAppPage
    {
        Task RespondToVoice(CortanaCommand command);
    }
}
