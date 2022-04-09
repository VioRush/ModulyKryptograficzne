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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        string wielomian;
        string ciąg_bitow;
        bool zatrzymaj = false;

        private void Generuj_button(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            wielomian = WielomianPodany.Text;
            for (int i = 0; i < wielomian.Length; i++)
            {
                ciąg_bitow += rnd.Next(2);
            }
            Console.WriteLine(ciąg_bitow);
            TekstWynikowy.Text += "Losowy ciąg bitów: " + ciąg_bitow + "\nWyniki: ";

            LFSR();
        }

        public void LFSR()
        {
            //do momentu zatrzymania przez użytkownika
                var wynik = 0;
                var ostatni_bit = ciąg_bitow.ElementAt(ciąg_bitow.Length - 1);
                for (int i = 0; i < wielomian.Length; i++)
                {
                    if (wielomian[i] == '1')
                    {
                        wynik = xor(ciąg_bitow[i], ostatni_bit);
                    }

                }

                string tmp = "";
                tmp += wynik;

                for (int i = 1; i < ciąg_bitow.Length; i++)
                {
                    tmp += ciąg_bitow[i - 1];
                }

                ciąg_bitow = tmp;
                TekstWynikowy.Text += tmp + "\n";
            
            
        }

        public int xor(int a, int b)
        {
            if (a == b) return 0;
            else return 1;

        }


        private void Zatrzymaj_button(object sender, RoutedEventArgs e)
        {
            zatrzymaj = true;
        }
    }
}
