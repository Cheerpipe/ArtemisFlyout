using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtemisFlyout.Services
{
    public interface IDeviceService
    {
        bool SetState(string deviceName, bool state);
        bool GetState(string deviceName, bool defaultState = false);
    }
}
