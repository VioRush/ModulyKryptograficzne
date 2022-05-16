using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Des
    {
        public static string[] K;
        public static ArrayList SF;
        byte[] dane;
        public Des() { }
        public Des(byte[] wejscie, byte[] klucz)
        {
            dane = wejscie;
            GenerujKlucze(klucz);
            SF = new ArrayList();
            SF.Add(S1);
            SF.Add(S2);
            SF.Add(S3);
            SF.Add(S4);
            SF.Add(S5);
            SF.Add(S6);
            SF.Add(S7);
            SF.Add(S8);

        }


        private void GenerujKlucze(byte[] klucz)
        {
            string czasKlucz = string.Concat(klucz.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
            char[] C = new char[28], D = new char[28];
            for (int i = 0; i < 28; i++)
            {
                C[i] = czasKlucz[PC1[i] - 1];
                D[i] = czasKlucz[PC1[i + 28] - 1];
            }
            K = new string[16];
            for (int i = 0; i < 16; i++)
            {
                C = Przes(C, i + 1);
                D = Przes(D, i + 1);
                string preK = new string(C) + new string(D);
                char[] nowK = new char[PC2.Length];
                for (int j = 0; j < PC2.Length; j++)
                {
                    nowK[j] = preK[PC2[j] - 1];
                }
                K[i] = new string(nowK);
            }

        }

        private char[] Przes(char[] tab, int iter)
        {
            char[] resultat = new char[tab.Length];
            int przes = PrzeswLewo[iter - 1];
            Array.Copy(tab, przes, resultat, 0, tab.Length - przes);
            int licznik = 0;
            for (int i = tab.Length - przes; i < tab.Length; i++)
            {
                resultat[i] = tab[licznik];
                licznik++;
            }
            return resultat;
        }

        private byte[] StringToByte(string dane)
        {
            return Enumerable.Range(0, dane.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(dane.Substring(x, 2), 16))
                             .ToArray();
        }

        public string Zaszyf()
        {

            string doZaszyf;
            doZaszyf = Permut(string.Concat(dane.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))).PadRight(64, '0'), IP); 
            string resultat = Liczyc(doZaszyf);
            resultat = Permut(resultat, IPm1); 
            return resultat;
        }

        public string Razszyf()
        {
            string doRazszyf;
            doRazszyf = Permut(string.Concat(dane.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))), IP);
            string resultat = odLiczyc(doRazszyf);
            resultat = Permut(resultat, IPm1);
            return resultat;
        }

        private string Permut(string dane, int[] permutacja)
        {
            char[] resultat = new char[dane.Length];
            for (int i = 0; i < dane.Length; i++)
            {
                resultat[i] = dane[permutacja[i] - 1];
            }
            return new string(resultat);
        }

        private string Liczyc(string dane)
        {
            string L = dane.Substring(0, 32);
            string R = dane.Substring(32, 32);
            string czasowe;
            for (int i = 0; i < 16; i++)
            {
                czasowe = R;
                R = XOR(L, f(czasowe, K[i]));
                L = czasowe;
            }
            return R + L;
        }

        private string odLiczyc(string dane)
        {
            string L = dane.Substring(0, 32);
            string R = dane.Substring(32, 32);
            string tmp;
            for (int i = 15; i >= 0; i--)
            {
                tmp = R;
                R = XOR(L, f(tmp, K[i]));
                L = tmp;
            }
            return R + L;
        }

        private string XOR(string A, string B)
        {
            if (A.Length != B.Length)
            {
                return null;
            }
            string resultat = "";
            for (int i = 0; i < A.Length; i++)
            {
                resultat += (A[i] ^ B[i]);
            }
            return resultat;
        }

        private string f(string dane, string klucz)
        {
            string skrot = e(dane);
            string binary = XOR(skrot, klucz);
            string przed = "";
            for (int i = 0; i < 8; i += 2)
            {
                przed += S(binary.Substring(i * 6, 6), SF[i] as int[,]) + S(binary.Substring((i + 1) * 6, 6), SF[i + 1] as int[,]);
            }
            char[] po = new char[przed.Length];
            for (int i = 0; i < przed.Length; i++)
            {
                po[i] = przed[P[i] - 1];
            }
            return new string(po);
        }

        private string e(string dane)
        {
            char[] resultat = new char[E.Length];
            for (int i = 0; i < E.Length; i++)
            {
                resultat[i] = dane[E[i] - 1];
            }
            return new string(resultat);
        }

        private string S(string dane, int[,] wybor)
        {
            string liniaS = "" + dane[0] + dane[5];
            string kolonnaS = "" + dane[1] + dane[2] + dane[3] + dane[4];
            int linia = Convert.ToInt32(liniaS, 2);
            int kolonna = Convert.ToInt32(kolonnaS, 2);
            return Convert.ToString(wybor[linia, kolonna], 2).PadLeft(4, '0');
        }
 
        public static int[] IP =
        {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };

        public static int[] IPm1 =
        {
            40, 8, 48, 16, 56, 24, 64, 32,
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25
        };

        public static int[] E =
        {
            32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
        };

        int[,] S1 =
        {
            {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
            { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
            { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
            { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }
        };

        int[,] S2 =
        {
            { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
            { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
            { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
            { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
        };

        int[,] S3 =
        {
            { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
            { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
            { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
            { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 }
        };

        int[,] S4 =
        {
            { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
            { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
            { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
            { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
        };

        int[,] S5 =
        {
            { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
            { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
            { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
            { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
        };

        int[,] S6 =
        {
            { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
            { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
            { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
            { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }
        };

        int[,] S7 =
        {
            { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
            { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
            { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
            { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
        };

        int[,] S8 =
        {
            { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
            { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
            { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
            { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
        };

        public static int[] P =
        {
            16, 7, 20, 21,
            29, 12, 28, 17,
            1, 15, 23, 26,
            5, 18, 31, 10,
            2, 8, 24, 14,
            32, 27, 3, 9,
            19, 13, 30, 6,
            22, 11, 4, 25
        };

        public static int[] PC1 =
        {
            57, 49, 41, 33, 25, 17, 9,
            1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27,
            19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29,
            21, 13, 5, 28, 20, 12, 4
        };

        public static int[] PC2 =
        {
            14, 17, 11, 24, 1, 5,
            3, 28, 15, 6, 21, 10,
            23, 19, 12, 4, 26, 8,
            16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55,
            30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53,
            46, 42, 50, 36, 29, 32
        };

        public static int[] PrzeswLewo = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
    }
}