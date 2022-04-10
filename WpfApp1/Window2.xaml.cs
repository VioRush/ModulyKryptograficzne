using System;
using System.Collections.Generic;
using System.IO;
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
        string wielomian_prev;
        string ciąg_bitow;
        bool zatrzymaj = false;

        private void Generuj_button(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            wielomian = WielomianPodany.Text;
            if (wielomian_prev != wielomian)
            {
                wielomian_prev = wielomian;
                ciąg_bitow = null;
                TekstWynikowy.Text = "";
            }
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

        int next = 0;
        public byte NextByte()
        {
            byte b = 0;
            for (int i = 7; i >= 0; i--)
            {
                if (next >= ciąg_bitow.Length) next = 0;
                if (ciąg_bitow[next] == '1') b = (byte)((b + Math.Pow(2, i)) % byte.MaxValue);
                next++;
            }
            //Console.WriteLine(b);
            return b;
        }

        public void Cipher_file(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog() {};
            if (ofd.ShowDialog() == false) return;
            string Cipher_me = File.ReadAllText(ofd.FileName);
            string Ciphered = "";
            next = 0;
            for (int i = 0; i < Cipher_me.Length; i++)
            {
                Ciphered += (Char)((Char)Cipher_me[i] + NextByte());
            }
            using (StreamWriter sw = File.CreateText(ofd.FileName))
            {
                sw.Write(Ciphered);
            }
        }

        public void Decipher_file(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog() { };
            if (ofd.ShowDialog() == false) return;
            string Cipher_me = File.ReadAllText(ofd.FileName);
            string Ciphered = "";
            next = 0;
            for (int i = 0; i < Cipher_me.Length; i++)
            {
                Ciphered += (Char)((Char)Cipher_me[i] - NextByte());
            }
            using (StreamWriter sw = File.CreateText(ofd.FileName))
            {
                sw.Write(Ciphered);
            }
        }

        private void Zatrzymaj_button(object sender, RoutedEventArgs e)
        {
            zatrzymaj = true;
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            MainWindow nowe = new MainWindow();
            nowe.Show();
            Close();
        }
    }
}
