
namespace Assets.PauseController.Scripts
{
    public interface IPausable
    {
        /// <summary>
        /// Called when resumes from pause.
        /// </summary>
        void OnResume();

        /// <summary>
        /// Called when the game is paused.
        /// </summary>
        void OnPause();
    }
}
