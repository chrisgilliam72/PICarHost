
namespace PanTiltHatLib
{
    public interface IPanTiltService
    {
        int Up();
        int Down();
        int Left();
        int Right();
        void Reset();
        int CurrentHPosition();
        int CurrentVPosition();
        bool Init(int busAddress, int Frequency);
        void Stop();
        int HPos(double pos);
        int VPos(double pos);
        int HPos(double pos, int min, int max);
        int VPos(double pos, int min, int max);
    }
}

