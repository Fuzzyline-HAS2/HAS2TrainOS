using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Dmo;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace HAS2TrainOS
{
    public class Mp3Player
    {
        public WaveOutEvent outputDevice;
        public AudioFileReader audioFile;
        public int nDeviceNum = 0;
        public int nDeviceVolume = 50;
        public void PlayMp3(String filename)
        {
            if (outputDevice != null)
            {
                outputDevice.Stop();
                //Thread.Sleep(150);
            }
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent()
                {
                    DeviceNumber = nDeviceNum
                };
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null)
            {
                String strTemp = "C:\\Users\\user\\Downloads\\" + filename + ".mp3";
                audioFile = new AudioFileReader(@strTemp);
                outputDevice.Init(audioFile);
                audioFile.Volume = nDeviceVolume / 100f;
                outputDevice.Play();
            }
        }
        public void StopMp3()
        {
            outputDevice.Stop();
        }
        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            outputDevice.Dispose();
            outputDevice = null;
            audioFile.Dispose();
            audioFile = null;
        }
    }
}
