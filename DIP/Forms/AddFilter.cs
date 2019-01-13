using DIP.Core;
using DIP.Core.Filters;
using System;
using System.Windows.Forms;

namespace DIP.Forms
{
    public partial class AddFilter : Form
    {
        private MainForm _callingForm;
        private FilterType _filterType;

        public AddFilter(MainForm callingForm, FilterType filterType)
        {
            InitializeComponent();

            _callingForm = callingForm;
            _filterType = filterType;

            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IFilter filter = null;

            if (_filterType == FilterType.Median)
            {
                filter = new Median();
            }
            else if (_filterType == FilterType.Minimum)
            {
                filter = new Minimum();
            }
            else if (_filterType == FilterType.Maximum)
            {
                filter = new Maximum();
            }

            int matrixSize = 0;

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    matrixSize = 3;
                    break;
                case 1:
                    matrixSize = 5;
                    break;
                case 2:
                    matrixSize = 7;
                    break;
                case 3:
                    matrixSize = 10;
                    break;
                case 4:
                    matrixSize = 13;
                    break;
                default:
                    matrixSize = 3;
                    break;
            }
            
            _callingForm.PGMImage = filter.Apply(_callingForm.PGMImage, matrixSize);
            _callingForm.UpdatePicture(_callingForm.PGMImage.ToBitmap());
        }
    }
}
