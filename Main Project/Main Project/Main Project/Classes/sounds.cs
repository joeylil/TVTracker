using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Main_Project.Classes
{
    class sounds
    {
        static Plugin.SimpleAudioPlayer.ISimpleAudioPlayer Player = null;

        public static void TapButton()
        {
            if (Application.Current.Properties.ContainsKey("sound"))
            {
                if ((bool)Application.Current.Properties["sound"] == false)
                {
                    return;
                }
                if (Player == null)
                {
                    Player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;                   
                }
                Player.Load("TapButton.wav");
                Player.Play();
            }            
        }

        public static void SlideSwitch()
        {
            if (Application.Current.Properties.ContainsKey("sound"))
            {
                if ((bool)Application.Current.Properties["sound"] == false)
                {
                    return;
                }
                if (Player == null)
                {
                    Player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;                   
                }
                Player.Load("SlideSwitch.wav");
                Player.Play();
            }
        }
    }
}

