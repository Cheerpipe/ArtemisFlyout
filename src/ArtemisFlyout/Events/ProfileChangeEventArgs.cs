using System;

namespace ArtemisFlyout.Events
{

    public class ProfileChangeEventArgs : EventArgs
    {
        internal ProfileChangeEventArgs(string profileName)
        {
            ProfileName = profileName;
        }

        public string ProfileName { get; }
    }
}