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
        public string currentSound = "";
        public List<string> soundsPaths = new List<string>();
        public List<string> soundsAllPaths = new List<string>();
        public List<Button> chooseButtons = new List<Button>();
        public bool isStarted = false;
        public System.Media.SoundPlayer snd = null;


        public Form1()
        {
            InitializeComponent();
            this.Text = "Угадай мелодию";
            

            Init();

            var mp3Path = @"C:\Users\recon\OneDrive\Рабочий стол\test.mp3";
            var outputPath = Path.ChangeExtension(mp3Path, ".trimmed.mp3");

            //TrimMp3To30Sec(mp3Path, outputPath, 2);
        }

        public void Init()
        {
            tempFolderPath = "";
            currentSound = "";
            soundsPaths = new List<string>();
            soundsAllPaths = new List<string>();
            chooseButtons = new List<Button>();
            isStarted = false;
            snd = null;
            chooseButtons.Add(a1);
            chooseButtons.Add(a2);
            chooseButtons.Add(a3);
            ResetComponents();
        }

        public void ResetComponents()
        {
            a1.Text = "Вариант ответа";
            a2.Text = "Вариант ответа";
            a2.Text = "Вариант ответа";
            button2.Enabled = true;
            folderPathLabel.Text = "";
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
                    
                    string[] files = Directory.GetFiles(fbd.SelectedPath);
                    if (files.Length < 3)
                    {
                        MessageBox.Show("Выберите папку с музыкальными файлами в количестве больше или равном трем!");
                        return;
                    }
                    folderPathLabel.Text = fbd.SelectedPath;
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
            DeleteTemp();
        }

        public void DeleteTemp()
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
            if (isStarted)
            {
                snd = new System.Media.SoundPlayer(currentSound);
                snd.Load();
                snd.Play();
            }
            else
            {
                MessageBox.Show("Вы еще не начали игру!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tempFolderPath != "")
            {
                isStarted = true;
                Button startButton = sender as Button;
                startButton.Enabled = false;
                if (Directory.Exists(tempFolderPath))
                {
                    soundsPaths = new List<string>(Directory.GetFiles(tempFolderPath));
                    soundsAllPaths = new List<string>(soundsPaths);
                    SelectRandomSound();
                }
            }else
            {
                MessageBox.Show("Выберите папку!");
            }
        }

        private void OnChooseButtonPressed(object sender, EventArgs e)
        {
            if (isStarted)
            {
                var currentSoundFilename = currentSound.Substring(currentSound.LastIndexOf('\\') + 1);
                Button pressedButton = sender as Button;
                if (currentSoundFilename == pressedButton.Text)
                {
                    if (snd != null)
                    {
                        snd.Stop();
                    }
                    SelectRandomSound();
                }
                else
                {
                    MessageBox.Show("Неверно! Попробуйте еще раз!");
                }
            }
            else
            {
                MessageBox.Show("Вы еще не начали игру!");
            }
        }

        private void SelectRandomSound()
        {
            Random r = new Random();
            int index = r.Next(0, soundsPaths.Count);
            if (soundsPaths.Count <= 0)
            {
                MessageBox.Show("Список песен кончился!");
                DeleteTemp();
                Init();
                return;
            }
            currentSound = soundsPaths[index];
            soundsPaths.RemoveAt(index);
            SetChooseButtons();
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

        public void SetChooseButtons()
        {
            Random r = new Random();
            int rnd = r.Next(0, 3);
            var currentSoundFilename = currentSound.Substring(currentSound.LastIndexOf('\\') + 1);
            var tempSoundsAllPaths = new List<String>(soundsAllPaths);
            for (int i = 0; i < chooseButtons.Count; i++)
            {
               if(rnd == i)
                {
                    chooseButtons[i].Text = currentSoundFilename;
                }
                else
                {
                    var rnds = r.Next(0, tempSoundsAllPaths.Count);
                    var randomSoundPath = tempSoundsAllPaths[rnds];
                    var randomSoundFilename = randomSoundPath.Substring(randomSoundPath.LastIndexOf('\\') + 1);
                    while(randomSoundFilename == currentSoundFilename)
                    {
                        rnds = r.Next(0, tempSoundsAllPaths.Count);
                        randomSoundPath = tempSoundsAllPaths[rnds];
                        randomSoundFilename = randomSoundPath.Substring(randomSoundPath.LastIndexOf('\\') + 1);
                    }
                    tempSoundsAllPaths.Remove(randomSoundPath);
                    chooseButtons[i].Text = randomSoundFilename;
                }
            }
        }

        private void play_Click(object sender, EventArgs e)
        {
            PlaySound();
        }
    }
}
