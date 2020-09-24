using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioGuess
{
    public partial class Form1 : Form
    {
        public string tempFolderPath = "";
        public List<string> soundsPaths = new List<string>();
        public List<string> soundsAllPaths = new List<string>();
        public string currentSound = "";
        public Form1()
        {
            InitializeComponent();
            this.Text = "Угадай мелодию";
            //File.Delete(@"C:\Users\recon\OneDrive\Рабочий стол\test.trimmed.mp3");

            var mp3Path = @"C:\Users\recon\OneDrive\Рабочий стол\test.mp3";
            var outputPath = Path.ChangeExtension(mp3Path, ".trimmed.mp3");

            //TrimMp3To30Sec(mp3Path, outputPath, 2);
        }

       
        public void TrimMp3To30Sec(string mp3Path, string outputPath, int minute)
        {

            TrimMp3(mp3Path, outputPath, TimeSpan.FromMinutes(minute), TimeSpan.FromMinutes(minute + 0.5f));
        }

        private void TrimMp3(string inputPath, string outputPath, TimeSpan? begin, TimeSpan? end)
        {
            if (begin.HasValue && end.HasValue && begin > end)
                throw new ArgumentOutOfRangeException("end", "end should be greater than begin");

            using (var reader = new Mp3FileReader(inputPath))
            using (var writer = File.Create(outputPath))
            {
                Mp3Frame frame;
                while ((frame = reader.ReadNextFrame()) != null)
                    if (reader.CurrentTime >= begin || !begin.HasValue)
                    {
                        if (reader.CurrentTime <= end || !end.HasValue)
                            writer.Write(frame.RawData, 0, frame.RawData.Length);
                        else break;
                    }
            }
        }

        private void selectFilesButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    folderPathLabel.Text = fbd.SelectedPath;
                    string[] files = Directory.GetFiles(fbd.SelectedPath);
                    var outputPath = Path.Combine(fbd.SelectedPath, "Temp");
                    tempFolderPath = outputPath;
                    Directory.CreateDirectory(outputPath);
                    foreach (String soundPath in files)
                    {
                        var filename = Path.Combine(outputPath, soundPath.Substring(soundPath.LastIndexOf('\\') + 1));
                        TrimMp3To30Sec(soundPath, filename, 2);
                        ConvertMp3ToWav(filename, Path.ChangeExtension(filename, ".wav"));
                        File.Delete(filename);
                    }
                }
            }
        }

        private void OnClosedForm(object sender, FormClosedEventArgs e)
        {
            if (tempFolderPath != "")
            {
                if (Directory.Exists(tempFolderPath))
                {
                    string[] files = Directory.GetFiles(tempFolderPath);

                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                    Directory.Delete(tempFolderPath);
                }
            }
        }
        public void PlaySound()
        {            
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(currentSound);
            snd.Load();
            snd.PlaySync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tempFolderPath != "")
            {
                Button startButton = sender as Button;
                startButton.Enabled = false;
                if (Directory.Exists(tempFolderPath))
                {
                    soundsPaths = new List<string>(Directory.GetFiles(tempFolderPath));
                    soundsAllPaths = soundsPaths;
                    SelectRandomSound();
                }
            }else
            {
                MessageBox.Show("Выберите папку!");
            }
        }
        private void SelectRandomSound()
        {
            Random r = new Random();
            int index = r.Next(0, soundsPaths.Count);
            currentSound = soundsPaths[index];
            soundsPaths.RemoveAt(index);
        }

        private static void ConvertMp3ToWav(string _inPath_, string _outPath_)
        {
            using (Mp3FileReader mp3 = new Mp3FileReader(_inPath_))
            {
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    WaveFileWriter.CreateWaveFile(_outPath_, pcm);
                   
                }
            }
        }

        private void play_Click(object sender, EventArgs e)
        {
            PlaySound();
        }
    }
}
