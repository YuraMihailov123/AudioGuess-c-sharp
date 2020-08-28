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
        public Form1()
        {
            InitializeComponent();

            //File.Delete(@"C:\Users\recon\OneDrive\Рабочий стол\test.trimmed.mp3");

            var mp3Path = @"C:\Users\recon\OneDrive\Рабочий стол\test.mp3";
            var outputPath = Path.ChangeExtension(mp3Path, ".trimmed.mp3");

            //TrimMp3To30Sec(mp3Path, outputPath, 2);
        }

        public void TrimMp3To30Sec(string mp3Path, string outputPath,int minute)
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
    }
}
