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
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private byte[] StringToByte(string data)
        {
            return Enumerable.Range(0, data.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(data.Substring(x, 2), 16))
                             .ToArray();
        }

        private void Zaszyfruj_Click(object sender, RoutedEventArgs e)
        {
            string plikOriginalny = "", plikZaszyfrowany;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Podaj  plik";
            if (ofd.ShowDialog() == true)
            {
                plikOriginalny = ofd.FileName;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            if (sfd.FileName != null)
            {
                plikZaszyfrowany = sfd.FileName;
            }

            BinaryWriter bw = new BinaryWriter(new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write));
            BinaryReader br = new BinaryReader(File.Open(plikOriginalny, FileMode.Open));

            int dlygoscPliku = (int)br.BaseStream.Length;
            while (br.BaseStream.Position < (dlygoscPliku / 8) * 8)
            {
                byte[] dane;
             
                dane = br.ReadBytes(8);

                Des des = new Des(dane, StringToByte(Klucz.Text));
                string zaszyfrowane = des.Zaszyf();
                byte[] bytes = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    bytes[i] = Convert.ToByte(zaszyfrowane.Substring(8 * i, 8), 2);
                }
                bw.Write(bytes);
                bw.Flush();
            }

            if (dlygoscPliku % 8 != 0)
            {
                byte[] dane;
              
                int ostatnie = dlygoscPliku - (int)br.BaseStream.Position;
                dane = br.ReadBytes(ostatnie);
                Des des2 = new Des(dane, StringToByte(Klucz.Text));
                string zaszyfrowane = des2.Zaszyf();
                byte[] bytes = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    bytes[i] = Convert.ToByte(zaszyfrowane.Substring(8 * i, 8), 2);
                }
                bw.Write(bytes);
                bw.Flush();
                bw.Write(8 - ostatnie);
                bw.Flush();
            }

            bw.Close();
            br.Close();
             
        }

        private void Odszyfruj_Click(object sender, RoutedEventArgs e)
        {
            string plikZaszyfrowany = "", plikOdszyfrowany;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Podaj  plik";
            if (ofd.ShowDialog() == true)
            {
                plikZaszyfrowany = ofd.FileName;
            }

             SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            if (sfd.FileName != null)
            {
                plikOdszyfrowany = sfd.FileName;
            }
            else return;

            BinaryWriter bw = new BinaryWriter(new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write));
            BinaryReader br = new BinaryReader(File.Open(plikZaszyfrowany, FileMode.Open));

            int dlygoscPliku = (int)br.BaseStream.Length;
            while (br.BaseStream.Position < (dlygoscPliku / 8) * 8)
            {
                byte[] dane;
                dane = br.ReadBytes(8);
                Des des = new Des(dane, StringToByte(Klucz.Text));

                string decoded = des.Razszyf();
                byte[] bytes = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    bytes[i] = Convert.ToByte(decoded.Substring(8 * i, 8), 2);
                }
                bw.Write(bytes);
                bw.Flush();
            }
            if (dlygoscPliku % 8 != 0)
            {
                uint temp;
                temp = br.ReadUInt32();
                bw.BaseStream.SetLength(bw.BaseStream.Length - temp);
            }
            bw.Close();
            br.Close();
        }

        private void Wstecz_Click(object sender, RoutedEventArgs e)
        {
            MainWindow nowe = new MainWindow();
            nowe.Show();
            Close();
        }
    }
}
