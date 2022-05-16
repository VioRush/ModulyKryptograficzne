    using System;
using System.Collections;
using System.Linq;

namespace WpfApp1.Core
{
    internal class DES
    {
        public static string[] K;
        public static ArrayList Sn;

        byte[] dane;

        public DES(byte[] binaryInput, byte[] key)
        {
            //input and generator(key)
            dane = binaryInput;
            KeysGenerator(key);

            Sn = new ArrayList();
            Sn.Add(S1);
            Sn.Add(S2);
            Sn.Add(S3);
            Sn.Add(S4);
            Sn.Add(S5);
            Sn.Add(S6);
            Sn.Add(S7);
            Sn.Add(S8);

        }

        public void KeysGenerator(byte[] key)
        {
            char[] C = new char[28],
                   D = new char[28];
            string dataKey = string.Concat(key.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')));
            K = new string[16];

            for(int i = 0; i < 28; i++)
            {
                C[i] = dataKey[PC1[i] - 1];
                D[i] = dataKey[PC1[i + 28] - 1];
            }

            for(int i = 0; i < 16; i++)
            {
                C = Shift(C, i + 1);
                D = Shift(D, i + 1);

                string previousKey = new string(C) + new string(D);
                char[] newKey = new char[PC2.Length];

                for (int k = 0; k < PC2.Length; k++) newKey[k] = previousKey[PC2[k] - 1];
                K[i] = new string(newKey);
            }

        }

        public char[] Shift(char[] tab, int k)
        {
            char[] shiftResult = new char[tab.Length];
            int shiftsOut = LEFTSHIFTS[k - 1];

            int count = 0;

            Array.Copy(tab, shiftsOut, shiftResult, 0, tab.Length - shiftsOut);
            
            for(int i = tab.Length - shiftsOut; i < tab.Length; i++)
            {
                shiftResult[i] = tab[count];
                count++;
            }

            return shiftResult;
        }

        public string Permutation(string input, int[] inv)
        {
            char[] output = new char[input.Length];

            for (int i = 0; i < input.Length; i++) output[i] = input[inv[i] - 1];

            string newOutput = new string(output);

            return newOutput;
        }

        private string Calculate(string input)
        {
            string L = input.Substring(0, 32);
            string R = input.Substring(32, 32);
            string data;

            for (int i = 0; i < 16; i++)
            {
                data = R;
                R = XOR(L, F(data, K[i]));
                L = data;
            }

            return R + L;
        }

        private string DeCalculate(string input)
        {
            string L = input.Substring(0, 32);
            string R = input.Substring(32, 32);
            string data;

            for (int i = 15; i >= 0; i--)
            {
                data = R;
                R = XOR(L, F(data, K[i]));
                L = data;
            }

            return R + L;
        }

        private string F(string R, string K)
        {
            string R48 = EPermutation(R);
            string XORresult = XOR(R48, K);
            
            string input = "";

            for (int i = 0; i < 8; i += 2)
            {
                input += S(XORresult.Substring(i * 6, 6), Sn[i] as int[,]) + S(XORresult.Substring((i + 1) * 6, 6), Sn[i + 1] as int[,]);
            }

            char[] output = new char[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                output[i] = input[P[i] - 1];
            }

            string output32 = new string(output);
            return output32;
        }

        private string XOR(string a, string b )
        {
            string XORresult = "";

            if (a.Length != b.Length)
            {
                return null;
            }
            
            for (int i = 0; i < a.Length; i++)
            {
                XORresult += a[i] ^ b[i];
            }
            return XORresult;
        }

        private string EPermutation(string input)
        {
            char[] output = new char[E.Length];

            for (int i = 0; i < E.Length; i++)
            {
                output[i] = input[E[i] - 1];
            }

            return new string(output);
        }

        private string S(string input, int[,] selection)
        {
            string row = "" + input[0] + input[5];
            string column = "" + input[1] + input[2] + input[3] + input[4];
            return Convert.ToString(selection[Convert.ToInt32(row, 2), Convert.ToInt32(column, 2)], 2).PadLeft(4, '0');
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

        public static int[] IPINV =
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

        public static int[] LEFTSHIFTS = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };

        public string Zaszyf()
        {

            string doZaszyf;
            doZaszyf = Permutation(string.Concat(dane.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))).PadRight(64, '0'), IP); 
            string resultat = DeCalculate(doZaszyf);
            resultat = Permutation(resultat, IPINV); 
            return resultat;
        }

        public string Razszyf()
        {
            string doRazszyf;
            doRazszyf = Permutation(string.Concat(dane.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))), IP);
            string resultat = Calculate(doRazszyf);
            resultat = Permutation(resultat, IPINV);
            return resultat;
        }
    }
}
