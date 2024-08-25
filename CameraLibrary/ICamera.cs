using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraLibrary;

public interface ICamera
{

    public bool IsCapturing { get; }
    public void StartCapture();
    public void StopCapture();
    public byte[] GetImage();
    public bool HasImages();

}