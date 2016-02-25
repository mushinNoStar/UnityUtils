namespace Rappresentation
{
    public interface IRappresentation
    {
        void show();
        void hide();
        bool isVisible();
        RappresentationData getRappresentationData();
        void update();
        void setRappresentationData(RappresentationData data);
        
    }
}