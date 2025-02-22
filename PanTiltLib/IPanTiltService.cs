
namespace PanTiltHatLib
{
    public interface IPanTiltService
    {
        int Up();
        int Down();
        int Left();
        int Right();
        int Left(int angle);
        int Right(int angle);
        void Reset();
        void MoveTo(int hPos, int vPos);
        int CurrentHPosition();
        int CurrentVPosition();
        bool Init(int busAddress, int Frequency);
        void Stop();
        int HPos(double pos);
        int VPos(double pos);
        int HPos(double pos, int min, int max);
        int VPos(double pos, int min, int max);

        int GetPanWidth();
        int GetTiltHeight();
    }
}

