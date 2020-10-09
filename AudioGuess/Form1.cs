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
using NAudio.Wave;

namespace AudioGuess
{
    public partial class Form1 : Form
    {
        public string tempFolderPath = "";
        public string currentSound = "";

        public List<string> soundPaths = new List<string>();
        public List<string> soundsAllPaths = new List<string>();
        public List<Button> chooseButtons = new List<Button>();

        public bool isStarted = false;

        public System.Media.SoundPlayer snd = null;
        
        public Form1()
        {
            InitializeComponent();
            this.Text = "Угадай мелодию";
            Init();
        }

        public void Init()
        {
            tempFolderPath = "";
            currentSound = "";
            soundPaths = new List<string>();
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
            a3.Text = "Вариант ответа";
            startButton.Enabled = true;
            pathLabel.Text = "";
        }

        public void TrimMp3To30Sec(string mp3path,string outputpath,int minute)
        {
            TrimMp3(mp3path, outputpath, TimeSpan.FromMinutes(minute), TimeSpan.FromMinutes(minute + 0.5f));
        }

        private void TrimMp3(string inputPath,string outputPath, TimeSpan? begin, TimeSpan? end)
        {
            if (begin.HasValue && end.HasValue && begin > end)
                throw new ArgumentOutOfRangeException("end", "end should be greater than begin");

            using(var reader=new Mp3FileReader(inputPath))
            using(var writer= File.Create(outputPath))
            {
                Mp3Frame frame;
                while((frame=reader.ReadNextFrame()) != null)
                {
                    if(reader.CurrentTime>=begin || !begin.HasValue)
                    {
                        if (reader.CurrentTime <= end || !end.HasValue)
                        {
                            writer.Write(frame.RawData, 0, frame.RawData.Length);
                        }
                        else break;
                    }
                }
            }
        }

        private static void ConvertMp3ToWav(string _inPath,string _outPath)
        {
            using(Mp3FileReader mp3 = new Mp3FileReader(_inPath))
            {
                using(WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    WaveFileWriter.CreateWaveFile(_outPath, pcm);
                }
            }
        }

        private void SelectFilesButton_Click(object sender, EventArgs e)
        {
            using(var fdb = new FolderBrowserDialog())
            {
                DialogResult result = fdb.ShowDialog();

                if(result == DialogResult.OK && !string.IsNullOrWhiteSpace(fdb.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fdb.SelectedPath);
                    if (files.Length < 3)
                    {
                        MessageBox.Show("Выберите папку с музыкальными файлами в кол-ве больше или равном 3!");
                        return;
                    }
                    pathLabel.Text = fdb.SelectedPath;
                    var outputpath = Path.Combine(fdb.SelectedPath, "Temp");
                    tempFolderPath = outputpath;
                    Directory.CreateDirectory(outputpath);
                    foreach(String soundPath in files)
                    {
                        var filename = Path.Combine(outputpath, soundPath.Substring(soundPath.LastIndexOf('\\') + 1));
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
                        File.Delete(file);

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
            }else
            {
                MessageBox.Show("Вы еще не начали игру!");
            }
        }

        private void ObChooseButtonPressed(object sender, EventArgs e)
        {
            if (isStarted)
            {
                var currentSoundFilename = currentSound.Substring(currentSound.LastIndexOf('\\') + 1);
                Button pressedButton = sender as Button;
                if(currentSoundFilename == pressedButton.Text)
                {
                    if (snd != null)
                    {
                        snd.Stop();
                    }
                    SelectRandomSound();
                }else
                {
                    MessageBox.Show("Неверно! Попробуйте еще раз!");
                }
            }else
            {
                MessageBox.Show("Вы еще не начали игру!");
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (tempFolderPath != "")
            {
                isStarted = true;
                Button startButton = sender as Button;
                startButton.Enabled = false;
                if (Directory.Exists(tempFolderPath))
                {
                    soundPaths = new List<string>(Directory.GetFiles(tempFolderPath));
                    soundsAllPaths = new List<string>(soundPaths);
                    SelectRandomSound();
                }
            }
            else
            {
                MessageBox.Show("Выберите папку!");
            }
        }

        public void SelectRandomSound()
        {
            Random r = new Random();
            int index = r.Next(0, soundPaths.Count);
            if (soundPaths.Count <= 0)
            {
                MessageBox.Show("Список песен кончился!");
                DeleteTemp();
                Init();
                return;
            }
            currentSound = soundPaths[index];
            soundPaths.RemoveAt(index);
            SetChooseButtons();
        }

        public void SetChooseButtons()
        {
            Random r = new Random();
            int rnd = r.Next(0, 3);
            var currentSoundFilename = currentSound.Substring(currentSound.LastIndexOf('\\') + 1);
            var tempSoundsAllPaths = new List<string>(soundsAllPaths);
            for (int i = 0; i < chooseButtons.Count; i++)
            {
                if (rnd == i)
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

        private void PlayButton_Click(object sender,EventArgs e)
        {
            PlaySound();
        }
    }
}
