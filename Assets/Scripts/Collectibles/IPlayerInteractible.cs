namespace Collectibles
{
    public interface IPlayerInteractable
    {
        /// <summary>
        /// Method to handle interaction from the player. <br/>
        /// 
        /// If other components are needed, i.e. the player's inventory,
        /// those should be added with public accessors/getters in <see cref="T:Collectibles.PlayerInteractController"/>.
        /// </summary>
        /// <param name="playerInteractController"></param>
        public void Interact(PlayerInteractController playerInteractController);
    }
}
