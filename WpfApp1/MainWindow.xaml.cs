using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string M;
        private int k;
        private string C;
        private char[,] tab = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WczytajDane()
        {
            M = TekstPodany.Text;
            k = Convert.ToInt32(Klucz.Text);
        }

        private void RailFence_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            int w = M.Length;
            tab = new char[k, w];
            bool do_dolu = false;
            int wiersz = 0;

            for (int i=0; i<w; i++)
            {
               if(wiersz == 0 || wiersz == k - 1)
               {
                    do_dolu = !do_dolu;
               }

               tab[wiersz, i] = M[i];

                if (do_dolu==true) { wiersz++; }
                else { wiersz--; }
            }

            String zaszyfrowany = null;

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if(tab[i,j] > 0)
                    {
                        zaszyfrowany += tab[i, j];
                    }
                }
            }

            TekstWynikowy.Text = zaszyfrowany;
        }

        private void RailFenceDeszyfrowanie_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            int w = M.Length;
            tab = new char[k, w];
            bool do_dolu = false;
            int wiersz = 0;

            Console.WriteLine(M);
            for (int i = 0; i < w; i++)
            {
                if (wiersz == 0 || wiersz == k - 1)
                {
                    do_dolu = !do_dolu;
                }

                tab[wiersz, i] = '.';

                if (do_dolu == true) { wiersz++; }
                else { wiersz--; }
            }

            String oryginalny = null;
            int m = 0;

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if (tab[i, j] == '.')
                    {
                        tab[i, j] = M.ElementAt(m);
                        m++;
                    }
                }
            }

            do_dolu = false;
            wiersz = 0;

            for (int i = 0; i < w; i++)
            {
                if (wiersz == 0 || wiersz == k - 1)
                {
                    do_dolu = !do_dolu;
                }

                oryginalny += tab[wiersz, i];

                if (do_dolu == true) { wiersz++; }
                else { wiersz--; }
            }

            TekstWynikowy.Text = oryginalny;
        }

    }
}
