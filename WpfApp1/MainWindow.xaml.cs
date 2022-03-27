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
        private string C = null;
        private string klucz;
        private char[,] tab = null;
        int a, b;
        string alfabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WczytajDane()
        {
            M = TekstPodany.Text;
            klucz = Klucz.Text;
        }

        private void RailFence_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            int w = M.Length;
            k = Convert.ToInt32(klucz);
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

            C = null;
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if(tab[i,j] > 0)
                    {
                        C += tab[i, j];
                    }
                }
            }

            TekstWynikowy.Text = C;
        }

        private void RailFenceDeszyfrowanie_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            int w = M.Length;
            k = Convert.ToInt32(klucz);
            tab = new char[k, w];
            bool do_dolu = false;
            int wiersz = 0;

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

            C = null;
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

                C += tab[wiersz, i];

                if (do_dolu == true) { wiersz++; }
                else { wiersz--; }
            }

            TekstWynikowy.Text = C;
        }

        private void Vigenere_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();

            C = null;
            
            for (int i = 0; i < M.Length; i++)
            {
                a = alfabet.IndexOf(char.ToUpper(M.ElementAt(i)));
                b = alfabet.IndexOf(char.ToUpper(klucz.ElementAt(i % klucz.Length)));

                C += alfabet.ElementAt((a + b) % alfabet.Length);
            }

            TekstWynikowy.Text = C;
        }

        private void VigenereDeszyfrowanie_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();

            C = null;
            int r;
            for (int i = 0; i < M.Length; i++)
            {
                a = alfabet.IndexOf(char.ToUpper(M.ElementAt(i)));
                b = alfabet.IndexOf(char.ToUpper(klucz.ElementAt(i % klucz.Length)));
                r = a - b;
                if(r < 0)
                {
                    C += alfabet.ElementAt((r+alfabet.Length) % alfabet.Length);
                }
                else
                {
                    C += alfabet.ElementAt(r % alfabet.Length);
                }
            }

            TekstWynikowy.Text = C;
        }

        private void PrzestawienieA_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            k  = Int16.Parse(klucz);
            C = null;

            for (int i = 0; i < Math.Ceiling((double)M.Length / (double)klucz.Length); i++)
                for (int j = 0; j < klucz.Length; j++)
                    if ((i * klucz.Length + (int)Char.GetNumericValue(klucz[j]) - 1)<M.Length) C += M[i * klucz.Length + (int)Char.GetNumericValue(klucz[j]) -1];

            TekstWynikowy.Text = C;
        }

        private void PrzestawienieADeszyfrowanie_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            C = null;

            string swapKlucz = null;

            for (int i = 0; i < klucz.Length; i++)
                swapKlucz += klucz.Length+1 - (int)Char.GetNumericValue(klucz[i]);

            for (int i = 0; i < Math.Floor((double)M.Length / (double)klucz.Length); i++)
                for (int j = 0; j < swapKlucz.Length; j++)
                    if ((i * swapKlucz.Length + (int)Char.GetNumericValue(klucz[j]) - 1) < M.Length) C += M[i * swapKlucz.Length + (int)Char.GetNumericValue(swapKlucz[j]) - 1];

            if (C.Length < M.Length)
            {
                String newKlucz = null;
                for (int i = 0; i < klucz.Length; i++)
                    if ((int)Char.GetNumericValue(klucz[i]) <= (M.Length - C.Length))
                        newKlucz += klucz[i];
                int Last = C.Length;

                M = M.Substring(C.Length, M.Length - C.Length);
                Char[] LastPart = new Char[M.Length];

                for (int j = 0; j < newKlucz.Length; j++)
                    LastPart[(int)Char.GetNumericValue(newKlucz[j])-1] = M[j];

                for (int j = 0; j < newKlucz.Length; j++)
                    C += LastPart[j];
            }

            TekstWynikowy.Text = C;
        }

    }
}
