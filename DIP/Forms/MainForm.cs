using DIP.Common;
using DIP.Core;
using DIP.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace DIP.Forms
{
    public partial class MainForm : Form
    {
        public string DefaultTitle { get; } = "DIP";
        public PGMImage PGMImage { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = DefaultTitle;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PGM (*.pgm)|*.pgm";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string fileName = new FileInfo(filePath).Name;
                    
                    try
                    {
                        // Load/Parse PGM
                        PGMImage = PGMParser.LoadFromFile(filePath);
                        
                        // Display image in the picture box
                        pictureBox1.Image = PGMImage.ToBitmap();
                        Text = $"{fileName.ToLower()} [{PGMImage.Width}x{PGMImage.Height}]";
                    }
                    catch (PGMFormatException ex)
                    {
                        PGMImage = null;
                        pictureBox1.Image = null;
                        Text = DefaultTitle;
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PGMImage == null)
            {
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PGM (*.pgm)|*.pgm|JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFileDialog.FileName;

                    switch (Path.GetExtension(savePath).ToLower())
                    {
                        case ".pgm":
                            PGMImage.Save(savePath);
                            break;
                        case ".jpg":
                            pictureBox1.Image.Save(savePath, ImageFormat.Jpeg);
                            break;
                        case ".png":
                            pictureBox1.Image.Save(savePath, ImageFormat.Png);
                            break;
                        case ".bmp":
                            pictureBox1.Image.Save(savePath, ImageFormat.Bmp);
                            break;
                    }

                    MessageBox.Show("Image saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void medianFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PGMImage == null)
            {
                return;
            }

            using (AddFilter addFilter = new AddFilter(this, FilterType.Median))
            {
                addFilter.ShowDialog();
            }
        }

        private void minimumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PGMImage == null)
            {
                return;
            }

            using (AddFilter addFilter = new AddFilter(this, FilterType.Minimum))
            {
                addFilter.ShowDialog();
            }
        }

        private void maximumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PGMImage == null)
            {
                return;
            }

            using (AddFilter addFilter = new AddFilter(this, FilterType.Maximum))
            {
                addFilter.ShowDialog();
            }
        }

        public void UpdatePicture(Bitmap bitmap)
        {
            pictureBox1.Image = bitmap;
        }
    }
}
