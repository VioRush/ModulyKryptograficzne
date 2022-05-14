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
            if (TekstPodany.Text.Length < 1)
            {
                TekstPodany.Background = Brushes.Red;
                TekstPodany.ToolTip = "Nie podano tekst do zaszyfrowania. Pole jest wymagane!";
            }
            else if (Klucz.Text.Length < 1)
            {
                Klucz.Background = Brushes.Red;
                Klucz.ToolTip = "Niepoprawnie podano wzrost! Pole jest wymagane!";
            }
            else
            {
                TekstPodany.Background = Brushes.White;
                TekstPodany.ToolTip = "";
            }
            M = TekstPodany.Text;
            klucz = Klucz.Text;
        }

        private void RailFence_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            int w = M.Length;
            if (!(int.TryParse(klucz, out k) || klucz == ".") || Convert.ToInt32(klucz) < 2)  //sprawdzenie, czy podany klucz jest liczbą > 1
            {
                Klucz.Background = Brushes.Red;
                Klucz.ToolTip = "Niepoprawnie podano klucz! Podaj liczbę całkowitą.";
            }
            else
            {
                Klucz.Background = Brushes.White;
                Klucz.ToolTip = "";
                k = Convert.ToInt32(klucz);
                tab = new char[k, w];
                bool do_dolu = false;
                int wiersz = 0;

                for (int i = 0; i < w; i++)  //wypełnienie utworzonej tabeli według schematu
                {
                    if (wiersz == 0 || wiersz == k - 1)
                    {
                        do_dolu = !do_dolu;  //zmiana kierunku, gdy dojdzie do ostatniego albo pierwszego wierszu 
                    }

                    tab[wiersz, i] = M[i];

                    if (do_dolu == true) { wiersz++; }
                    else { wiersz--; }
                }

                C = null;
                for (int i = 0; i < k; i++)  //odczytanie tekstu zaszyfrowanego według schematu wierszami
                {
                    for (int j = 0; j < w; j++)
                    {
                        if (tab[i, j] > 0)
                        {
                            C += tab[i, j];
                        }
                    }
                }

                TekstWynikowy.Text = C;
            }
        }

        private void RailFenceDeszyfrowanie_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            int w = M.Length;
            if (!(int.TryParse(klucz, out k) || klucz == ".") || Convert.ToInt32(klucz) < 2) ////sprawdzenie, czy podany klucz jest liczbą
            {
                Klucz.Background = Brushes.Red;
                Klucz.ToolTip = "Niepoprawnie podano klucz! Podaj liczbę całkowitą.";
            }
            else
            {
                Klucz.Background = Brushes.White;
                Klucz.ToolTip = "";
                k = Convert.ToInt32(klucz);
                tab = new char[k, w];
                bool do_dolu = false;
                int wiersz = 0;

                for (int i = 0; i < w; i++)   //oznaczenie miejsc, dzie mają być litery znakiem kropki
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

                for (int i = 0; i < k; i++)   //uzupełnienie utworzonej tabeli wierszami, gdzie jest kropka
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

                for (int i = 0; i < w; i++)  //odczytanie oryginalnego tekstu przechodząc po przekątnych
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
        }

        private void Vigenere_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();

            C = null;

            if (int.TryParse(klucz, out k)) ////sprawdzenie, czy podany klucz nie jest liczbą
            {
                Klucz.Background = Brushes.Red;
                Klucz.ToolTip = "Niepoprawnie podano klucz! Podaj napis.";
            }
            else
            {
                Klucz.Background = Brushes.White;
                Klucz.ToolTip = "";
                for (int i = 0; i < M.Length; i++)  //przechodząc po wartościach tekstu oryginalnego podmieniamy litery
                {
                    a = alfabet.IndexOf(char.ToUpper(M.ElementAt(i)));
                    b = alfabet.IndexOf(char.ToUpper(klucz.ElementAt(i % klucz.Length)));

                    C += alfabet.ElementAt((a + b) % alfabet.Length); //reszta z dzielenia sum indeksów liter tekstu i klucza daje indeks litery
                                                                      //na którą podmieniamy
                }

                TekstWynikowy.Text = C;
            }
        }

        private void VigenereDeszyfrowanie_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();

            C = null;
            int r;
            if (int.TryParse(klucz, out k)) ////sprawdzenie, czy podany klucz nie jest liczbą
            {
                Klucz.Background = Brushes.Red;
                Klucz.ToolTip = "Niepoprawnie podano klucz! Podaj napis.";
            }
            else
            {
                Klucz.Background = Brushes.White;
                Klucz.ToolTip = "";
                for (int i = 0; i < M.Length; i++) //przechodząc po wartościach zasyfrowanego tekstu podmieniamy litery
                {
                    a = alfabet.IndexOf(char.ToUpper(M.ElementAt(i)));
                    b = alfabet.IndexOf(char.ToUpper(klucz.ElementAt(i % klucz.Length)));
                    r = a - b;
                    if (r < 0)  //w zależności od znaku różnicy indeksów stosujemy tą albo inną formulę
                    {
                        C += alfabet.ElementAt((r + alfabet.Length) % alfabet.Length);
                    }
                    else
                    {
                        C += alfabet.ElementAt(r % alfabet.Length);
                    }
                }

                TekstWynikowy.Text = C;
            }
        }

        private void PrzestawienieA_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();
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
                for (int j = 0; j < klucz.Length; j++)
                    if ((int)Char.GetNumericValue(klucz[j]) == i+1)
                        swapKlucz += (j+1).ToString();

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

        private void PrzestawienieC_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();
            //M = "HERE IS A SECRET MESSAGE ENCIPHERED BY TRANSPOSITION";
            //klucz = "CONVENIENCE";

            C = null;
            Char[,] tab = new char[M.Length + klucz.Length, M.Length + klucz.Length];
            int[] values = new int[klucz.Length];

            for (int i = 0; i <= klucz.Length; i++)
            {
                Char min = Char.MaxValue;
                int minPos = 0;
                for (int j = 0; j < klucz.Length; j++)
                {
                    if ((klucz[j] < min) && (values[j] == 0)) { min = klucz[j]; minPos = j; }
                }
                values[minPos] = i;
            }

            int element = 0;
            int line = 0;
            int column = 0;
            int exceed = 0;
            while (element < M.Length)
            {
                if (Char.IsLetter(M[element]))
                {
                    tab[line + klucz.Length * exceed, column] = M[element];
                    element++;
                    if (line == values[column]-1)
                    {
                        column = 0;
                        line++;
                    }
                    else
                    {
                        column++;
                    }
                    if (line >= klucz.Length)
                    {
                        column = 0;
                        line = 0;
                        exceed++;
                    }
                }
                else element++;
            }

            for (int i = 0; i < klucz.Length; i++)
            {
                for (int j = 0; j < klucz.Length; j++)
                {
                    if (values[j] == i+1)
                    {
                        for (int k = 0; k < klucz.Length * (exceed + 1); k++)
                        {
                            if (Char.IsLetter(tab[k, j])) C += tab[k, j];
                        }
                        C += ' ';
                    }
                }
            }

            TekstWynikowy.Text = C;
        }

        private void PrzestawienieCDeszyfrowanie_button(object sender, RoutedEventArgs e)
        {
            WczytajDane();

            C = null;
            Char[,] tab = new char[M.Length + klucz.Length, M.Length + klucz.Length];
            int[] values = new int[klucz.Length];

            for (int i = 0; i <= klucz.Length; i++)
            {
                Char min = Char.MaxValue;
                int minPos = 0;
                for (int j = 0; j < klucz.Length; j++)
                {
                    if ((klucz[j] < min) && (values[j] == 0)) { min = klucz[j]; minPos = j; }
                }
                values[minPos] = i;
            }

            int element = 0;
            int line = 0;
            int column = 0;
            int exceed = 0;
            while (element < M.Length)
            {
                if (Char.IsLetter(M[element]))
                {
                    tab[line + klucz.Length * exceed, column] = M[element];
                    element++;
                    if (line == values[column] - 1)
                    {
                        column = 0;
                        line++;
                    }
                    else
                    {
                        column++;
                    }
                    if (line >= klucz.Length)
                    {
                        column = 0;
                        line = 0;
                        exceed++;
                    }
                }
                else element++;
            }

            element = 0;
            for (int i = 0; i < klucz.Length; i++)
            {
                for (int j = 0; j < klucz.Length; j++)
                {
                    if (values[j] == i + 1)
                    {
                        for (int k = 0; k < klucz.Length * (exceed + 1); k++)
                        {
                            if (Char.IsLetter(tab[k, j])) { while (!Char.IsLetter(M[element])) element++;  tab[k, j] = M[element]; element++; }
                        }
                    }
                }
            }

            for (int i = 0; i < M.Length + klucz.Length; i++)
            {
                for (int j = 0; j < M.Length + klucz.Length; j++)
                {
                    if (Char.IsLetter(tab[i, j])) C += tab[i, j];
                }
            }

            /*for (int i = 0; i < klucz.Length; i++)
            {
                for (int j = 0; j < klucz.Length; j++)
                {
                    if (values[j] == i + 1)
                    {
                        for (int k = 0; k < klucz.Length * (exceed + 1); k++)
                        {
                            if (Char.IsLetter(tab[j, k])) C += tab[j, k];
                        }
                    }
                }
            }*/

            TekstWynikowy.Text = C;
        }

        private void PrzestawienieB_button(object sender, RoutedEventArgs e)
        {
            TekstWynikowy.Text = SzyfrowaniePrzykladB();
        }

        private void PrzestawienieBDeszyfrowanie_button(object sender, RoutedEventArgs e)
        {
            TekstWynikowy.Text = DeszyfrowaniePrzykladB();
        }

        private void SzyfrCezara_button(object sender, RoutedEventArgs e)
        {
            TekstWynikowy.Text = Szyfrowanie(TekstPodany.Text);
        }

        private void SzyfrCezaraDeszyfrowanie_button(object sender, RoutedEventArgs e)
        {
            TekstWynikowy.Text = Deszyfrowanie(TekstPodany.Text);
        }


        public string Szyfrowanie(string inp)
        {
            WczytajDane();
            StringBuilder code = new StringBuilder();
            string message = M;

            for (int i = 0; i < message.Length; i++)
                for (int j = 0; j < alfabet.Length; j++)
                    if (message[i] == alfabet[j])
                        code.Append(alfabet[(j + Convert.ToInt32(klucz)) % alfabet.Length]);

            return code.ToString();
        }
        public string Deszyfrowanie(string inp)
        {
            WczytajDane();
            StringBuilder code = new StringBuilder();
            string message = M;
            int k = Convert.ToInt32(klucz);

            for (int i = 0; i < message.Length; i++)
                for (int j = 0; j < alfabet.Length; j++)
                    if (message[i] == alfabet[j])
                        code.Append(alfabet[(j - k + alfabet.Length) % alfabet.Length]);

            return code.ToString();
        }

        public string SzyfrowaniePrzykladB()
        {
            WczytajDane();
            string message = M;
            int kolumn = klucz.Length;
            int wiersz = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(message.Length) / kolumn));
            string szyfr = "";


            char[,] message_char = new char[wiersz, kolumn];

            int value = 0;

            for (int i = 0; i < wiersz; i++)
                for (int j = 0; j < kolumn; j++)
                {
                    if (value >= message.Length)
                        continue;
                    if (message[value] == ' ')
                        value++;
                    message_char[i, j] = message[value];
                    value++;
                }

            for (int i = 0; i < alfabet.Length; i++)
                for (int j = 0; j < klucz.Length; j++)
                    if (alfabet[i] == klucz[j])
                        for (int g = 0; g <= wiersz; g++)
                        {
                            if (g == wiersz)
                            {
                                szyfr += " ";
                                break;
                            }
                            szyfr += message_char[g, j];
                        }

            return szyfr;
        }

        public string DeszyfrowaniePrzykladB()
        {
            WczytajDane();
            string message = M;
            int kolumn = klucz.Length;
            int wiersz = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(message.Length) / kolumn));
            string deszyfr = "";

            char[,] message_char = new char[wiersz, kolumn];

            int value = 0;

            for (int i = 0; i < alfabet.Length; i++)
                for (int j = 0; j < klucz.Length; j++)
                    if (alfabet[i] == klucz[j])
                        if (message[value] == ' ')
                        {
                            value++;
                            for (int g = 0; g < wiersz - 1; g++)
                            {
                                message_char[g, j] = message[value];
                                value++;
                            }
                        }
                        else
                        {
                            for (int g = 0; g < wiersz; g++)
                            {
                                message_char[g, j] = message[value];
                                value++;
                            }

                        }

            for (int i = 0; i < wiersz; i++)
                for (int j = 0; j < kolumn; j++)
                {
                    deszyfr += message_char[i, j];
                }

            return deszyfr;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Window2 nowe = new Window2();
            nowe.Show();
            Close();
        }

        private void DES_button(object sender, RoutedEventArgs e)
        {
            Window1 nowe = new Window1();
            nowe.Show();
            Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            int result;

            if (!(int.TryParse(e.Text, out result) || e.Text == "."))
            {
                e.Handled = true;
            }
        }
    }
}
