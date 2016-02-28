namespace Game
{
    public abstract class Visualization
    {
        private float visualizationDelay = 0;
        private float currentTimePassed = 0;

        public Visualization()
        {
            Tools.TimeManager.OnTick += tick;
        }

        /// <summary>
        /// a rappresentation with this propterty set to somethign > 0 will be updated only when the ticks reach zero
        /// </summary>
        /// <param name="f"></param>
        public void setVisualizationDelay(float f)
        {
            visualizationDelay = f;
        }


        private void tick()
        {
            currentTimePassed--;

            if (currentTimePassed <= 0 && isVisible())
            {
                update();
                currentTimePassed = visualizationDelay;
            }
        }

        /// <summary>
        /// this is used by scenes to hide things that are not needed, not that are invisible.
        /// </summary>
        public abstract void hide();
        /// <summary>
        /// this is used by scenes to hide things that are not needed, not that are invisible.
        /// </summary>
        public abstract void show();

        public abstract bool isVisible();

        /// <summary>
        /// this should update the visuals on the screen to the data known
        /// </summary>
        public abstract void update();
        
    }
}