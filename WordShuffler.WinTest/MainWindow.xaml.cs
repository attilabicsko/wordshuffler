using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordShuffler.Models;

namespace WordShufflerWinTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            init();
        }

        private WordShuffler.WordShuffler _wordShuffler;

        private ShuffleModel _currentShuffleModel;


        const string wordsListPath = "Res/wordsEn.txt";

        private int _currentSize = 5;

        public void init()
        {
            tb_size.Text = _currentSize.ToString();
            b_generate.Click += new RoutedEventHandler(b_generate_Click);
        
        
        }

        void b_generate_Click(object sender, RoutedEventArgs e)
        {
            int currSizeInt = -1;
            var currSizeStr = tb_size.Text;
            if (int.TryParse(currSizeStr, out currSizeInt))
            {
                if (_currentSize != currSizeInt || _wordShuffler == null)
                {
                    _currentSize = currSizeInt;
                    _wordShuffler = new WordShuffler.WordShuffler(wordsListPath, _currentSize);
                }

                _currentShuffleModel = _wordShuffler.GetNextModel();
                
            }
            else
            {
                _currentShuffleModel = _wordShuffler.GetNextModel();
            }

            printResult();
            
        }

        void printResult()
        {
            tb_matrix.Text = getMatrixString();

            var list = "";

            foreach (var matrixWord in _currentShuffleModel.MatrixWords)
            {
                list += string.Format("{0} ({1})\n", matrixWord.Word, getPathString(matrixWord.Path));
            }


            tb_words.Text = list;

        }


        string getPathString(CharCoordinate[] coords)
        {
            var pathString = "";

            foreach (var charCoordinate in coords)
            {
                pathString += charCoordinate.ToString() + ", ";
            }
            pathString = pathString.Substring(0, pathString.Length - 2);

            return pathString;
        }


        string getMatrixString()
        {

            string ret = "";

            var matrix = _currentShuffleModel.Matrix.GetMatrix();

            for (int i = 0; i < _currentSize; i++)
            {
                for (int j = 0; j < _currentSize; j++)
                {
                     ret += " [" + matrix[i, j] + "]\t";
                }
                ret += "\n";
            }

            return ret;
        }

        

    }
}
