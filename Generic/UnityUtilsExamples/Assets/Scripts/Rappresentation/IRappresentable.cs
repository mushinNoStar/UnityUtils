namespace Rappresentation
{
    /// <summary>
    /// a rappresentable item is something that has the property of being visible by the player.
    /// </summary>
    public interface IRappresentable
    {
        void show();
        void hide();
        bool isVisible();
        RappresentationData getRappresentationData();
        void setRappresentationData(RappresentationData data);
        IRappresentation getRappresentation();
        void setRappresentation(IRappresentation rapp);
    }
}
