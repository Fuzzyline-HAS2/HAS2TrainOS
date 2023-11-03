using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Dmo;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Windows.Forms;

namespace HAS2TrainOS
{
    public class Mp3Player
    {
        public WaveOutEvent outputDevice = new WaveOutEvent();
        public AudioFileReader audioFile;
        public int nDeviceNum = 0;
        public int nDeviceVolume = 50;
        public void PlayMp3(String filename)
        {
            try
            {
                if (outputDevice != null)
                {
                    outputDevice.Stop();
                    outputDevice.Dispose();
                    Thread.Sleep(150);
                }
                if (audioFile != null)
                {
                    audioFile.Dispose();
                    audioFile = null;
                }
                if (outputDevice == null)
                {
                    outputDevice.Stop();
                    outputDevice.Dispose();
                    outputDevice = null;
                    //outputDevice.PlaybackStopped += OnPlaybackStopped;
                }
                if (audioFile == null)
                {
                    //String strTemp = "C:\\Users\\user\\Downloads\\" + filename + ".wav";
                    Console.WriteLine(filename);
                    audioFile = new AudioFileReader(@filename);
                    audioFile.Volume = nDeviceVolume / 100f;
                    outputDevice.DeviceNumber = nDeviceNum;
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occured while trying to stop the MP3: {e.Message}");
            }
        }
        public void StopAndPlay(String filename)
        {
            outputDevice = new WaveOutEvent()
            {
                DeviceNumber = nDeviceNum
            };
            if (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                outputDevice.Stop();
                outputDevice.Dispose();
                outputDevice = new WaveOutEvent();
            }
            PlayAudio(filename);
        }
        private void PlayAudio(string filePath)
        {
            if (File.Exists(filePath))
            {
                Console.WriteLine($"Playing: {Path.GetFileName(filePath)}");
                using (var waveFileReader = new WaveFileReader(filePath))
                {
                    outputDevice.Init(waveFileReader);
                    outputDevice.Play();
                    //while (outputDevice.PlaybackState == PlaybackState.Playing)
                    //{
                    //    Application.DoEvents();
                    //    System.Threading.Thread.Sleep(100);
                    //}
                }
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }
        public void StopMp3()
        {
            outputDevice.Stop();
        }
        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            try
            {
                outputDevice.Dispose();
                outputDevice = null;
                audioFile.Dispose();
                audioFile = null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occured while trying to stop the MP3: {e.Message}");
            }
        }
    }
}
