using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using ProjectEulerSolver.Model;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text;

namespace ProjectEulerSolver.Problems
{
    public class Problem59 : BaseProblem, IProblem
    {
        public Problem59()
        {
            Number = 59;
            Prompt = "Each character on a computer is assigned a unique code and the preferred standard is ASCII (American Standard Code for Information Interchange). For example, uppercase A = 65, asterisk (*) = 42, and lowercase k = 107. " +
                     "A modern encryption method is to take a text file, convert the bytes to ASCII, then XOR each byte with a given value, taken from a secret key. The advantage with the XOR function is that using the same encryption key on the cipher text, restores the plain text; for example, 65 XOR 42 = 107, then 107 XOR 42 = 65. " +
                     "For unbreakable encryption, the key is the same length as the plain text message, and the key is made up of random bytes. The user would keep the encrypted message and the encryption key in different locations, and without both 'halves', it is impossible to decrypt the message. " +
                     "Unfortunately, this method is impractical for most users, so the modified method is to use a password as a key. If the password is shorter than the message, which is likely, the key is repeated cyclically throughout the message. The balance for this method is using a sufficiently long password key for security, but short enough to be memorable. " +
                     "Your task has been made easy, as the encryption key consists of three lower case characters. Using cipher.txt (right click and 'Save Link/Target As...'), a file containing the encrypted ASCII codes, and the knowledge that the plain text must contain common English words, decrypt the message and find the sum of the ASCII values in the original text.";
            Input = "79,59,12,2,79,35,8,28,20,2,3,68,8,9,68,45,0,12,9,67,68,4,7,5,23,27,1,21,79,85,78,79,85,71,38,10,71,27,12,2,79,6,2,8,13,9,1,13,9,8,68,19,7,1,71,56,11,21,11,68,6,3,22,2,14,0,30,79,1,31,6,23,19,10,0,73,79,44,2,79,19,6,28,68,16,6,16,15,79,35,8,11,72,71,14,10,3,79,12,2,79,19,6,28,68,32,0,0,73,79,86,71,39,1,71,24,5,20,79,13,9,79,16,15,10,68,5,10,3,14,1,10,14,1,3,71,24,13,19,7,68,32,0,0,73,79,87,71,39,1,71,12,22,2,14,16,2,11,68,2,25,1,21,22,16,15,6,10,0,79,16,15,10,22,2,79,13,20,65,68,41,0,16,15,6,10,0,79,1,31,6,23,19,28,68,19,7,5,19,79,12,2,79,0,14,11,10,64,27,68,10,14,15,2,65,68,83,79,40,14,9,1,71,6,16,20,10,8,1,79,19,6,28,68,14,1,68,15,6,9,75,79,5,9,11,68,19,7,13,20,79,8,14,9,1,71,8,13,17,10,23,71,3,13,0,7,16,71,27,11,71,10,18,2,29,29,8,1,1,73,79,81,71,59,12,2,79,8,14,8,12,19,79,23,15,6,10,2,28,68,19,7,22,8,26,3,15,79,16,15,10,68,3,14,22,12,1,1,20,28,72,71,14,10,3,79,16,15,10,68,3,14,22,12,1,1,20,28,68,4,14,10,71,1,1,17,10,22,71,10,28,19,6,10,0,26,13,20,7,68,14,27,74,71,89,68,32,0,0,71,28,1,9,27,68,45,0,12,9,79,16,15,10,68,37,14,20,19,6,23,19,79,83,71,27,11,71,27,1,11,3,68,2,25,1,21,22,11,9,10,68,6,13,11,18,27,68,19,7,1,71,3,13,0,7,16,71,28,11,71,27,12,6,27,68,2,25,1,21,22,11,9,10,68,10,6,3,15,27,68,5,10,8,14,10,18,2,79,6,2,12,5,18,28,1,71,0,2,71,7,13,20,79,16,2,28,16,14,2,11,9,22,74,71,87,68,45,0,12,9,79,12,14,2,23,2,3,2,71,24,5,20,79,10,8,27,68,19,7,1,71,3,13,0,7,16,92,79,12,2,79,19,6,28,68,8,1,8,30,79,5,71,24,13,19,1,1,20,28,68,19,0,68,19,7,1,71,3,13,0,7,16,73,79,93,71,59,12,2,79,11,9,10,68,16,7,11,71,6,23,71,27,12,2,79,16,21,26,1,71,3,13,0,7,16,75,79,19,15,0,68,0,6,18,2,28,68,11,6,3,15,27,68,19,0,68,2,25,1,21,22,11,9,10,72,71,24,5,20,79,3,8,6,10,0,79,16,8,79,7,8,2,1,71,6,10,19,0,68,19,7,1,71,24,11,21,3,0,73,79,85,87,79,38,18,27,68,6,3,16,15,0,17,0,7,68,19,7,1,71,24,11,21,3,0,71,24,5,20,79,9,6,11,1,71,27,12,21,0,17,0,7,68,15,6,9,75,79,16,15,10,68,16,0,22,11,11,68,3,6,0,9,72,16,71,29,1,4,0,3,9,6,30,2,79,12,14,2,68,16,7,1,9,79,12,2,79,7,6,2,1,73,79,85,86,79,33,17,10,10,71,6,10,71,7,13,20,79,11,16,1,68,11,14,10,3,79,5,9,11,68,6,2,11,9,8,68,15,6,23,71,0,19,9,79,20,2,0,20,11,10,72,71,7,1,71,24,5,20,79,10,8,27,68,6,12,7,2,31,16,2,11,74,71,94,86,71,45,17,19,79,16,8,79,5,11,3,68,16,7,11,71,13,1,11,6,1,17,10,0,71,7,13,10,79,5,9,11,68,6,12,7,2,31,16,2,11,68,15,6,9,75,79,12,2,79,3,6,25,1,71,27,12,2,79,22,14,8,12,19,79,16,8,79,6,2,12,11,10,10,68,4,7,13,11,11,22,2,1,68,8,9,68,32,0,0,73,79,85,84,79,48,15,10,29,71,14,22,2,79,22,2,13,11,21,1,69,71,59,12,14,28,68,14,28,68,9,0,16,71,14,68,23,7,29,20,6,7,6,3,68,5,6,22,19,7,68,21,10,23,18,3,16,14,1,3,71,9,22,8,2,68,15,26,9,6,1,68,23,14,23,20,6,11,9,79,11,21,79,20,11,14,10,75,79,16,15,6,23,71,29,1,5,6,22,19,7,68,4,0,9,2,28,68,1,29,11,10,79,35,8,11,74,86,91,68,52,0,68,19,7,1,71,56,11,21,11,68,5,10,7,6,2,1,71,7,17,10,14,10,71,14,10,3,79,8,14,25,1,3,79,12,2,29,1,71,0,10,71,10,5,21,27,12,71,14,9,8,1,3,71,26,23,73,79,44,2,79,19,6,28,68,1,26,8,11,79,11,1,79,17,9,9,5,14,3,13,9,8,68,11,0,18,2,79,5,9,11,68,1,14,13,19,7,2,18,3,10,2,28,23,73,79,37,9,11,68,16,10,68,15,14,18,2,79,23,2,10,10,71,7,13,20,79,3,11,0,22,30,67,68,19,7,1,71,8,8,8,29,29,71,0,2,71,27,12,2,79,11,9,3,29,71,60,11,9,79,11,1,79,16,15,10,68,33,14,16,15,10,22,73";
        }
        public override void Solve()
        {
            LogList = new List<string>();
            KeyOffSpace();
            
        }
        public void GetPossibleValues(byte value)
        { 
            string testString = "";
            for (int i = 97; i <= 122; i++)
            {
                byte testItem = Convert.ToByte(value ^ Convert.ToByte(i));
                testString += "   " + Encoding.ASCII.GetString(new byte[] { testItem });
            }
            LogList.Add(Convert.ToString(value) + " :    " + testString);
            LogToFile();
            LogList.Clear();
        }
        public byte GetSpaceKey(byte value)
        {
            for (int i = 97; i <= 122; i++)
            {
                byte testItem = Convert.ToByte(value ^ Convert.ToByte(i));
                if(Encoding.ASCII.GetString(new byte[] { testItem }) == " ")
                {
                    return Convert.ToByte(i);
                }
            }
            return Convert.ToByte(0);
        }
        public void KeyOffSpace()
        {
            List<byte> inputs = Input.Split(',').Select(x => Convert.ToByte(int.Parse(x))).ToList();
            double y = (double)inputs.Count() / 3;
            int wholeIterations = (int)Math.Floor(y);
            int remainder = (int)Math.Ceiling((y - wholeIterations) * 3);
            List<byte> decryptCandidate1 = new List<byte>();
            List<byte> decryptCandidate2 = new List<byte>();
            List<byte> decryptCandidate3 = new List<byte>();

            for (int x = 0; x < wholeIterations; x += 3)
            {
                decryptCandidate1.Add(Convert.ToByte(inputs[x]));
                decryptCandidate2.Add(Convert.ToByte(inputs[x + 1]));
                decryptCandidate3.Add(Convert.ToByte(inputs[x + 2]));
            }
            for (int x = 0; x < remainder; x++)
            {
                if (x == 0)
                {
                    decryptCandidate1.Add(inputs[(wholeIterations * 3) + x]);
                }
                if (x == 1)
                {
                    decryptCandidate2.Add(inputs[(wholeIterations * 3) + x]);
                }
            }


            Dictionary<byte, int> counts1 = new Dictionary<byte, int>();
            Dictionary<byte, int> counts2 = new Dictionary<byte, int>();
            Dictionary<byte, int> counts3 = new Dictionary<byte, int>();


            foreach (byte input in decryptCandidate1.Distinct())
            {
                counts1[input] = decryptCandidate1.Where(x => x == input).Count();
            }
            foreach (byte input in decryptCandidate2.Distinct())
            {
                counts2[input] = decryptCandidate2.Where(x => x == input).Count();
            }
            foreach (byte input in decryptCandidate3.Distinct())
            {
                counts3[input] = decryptCandidate3.Where(x => x == input).Count();
            }
            
            byte[] decryptKey = new byte[3];
            decryptKey[0] = GetSpaceKey(counts1.OrderByDescending(x => x.Value).Select(x => x.Key).ToList().First());
            decryptKey[1] = GetSpaceKey(counts2.OrderByDescending(x => x.Value).Select(x => x.Key).ToList().First());
            decryptKey[2] = GetSpaceKey(counts3.OrderByDescending(x => x.Value).Select(x => x.Key).ToList().First());
            DecryptInputAsList(decryptKey, inputs, wholeIterations, remainder);
        }
        public void EvaluateCounts()
        {
            List<byte> inputs = Input.Split(',').Select(x => Convert.ToByte(int.Parse(x))).ToList();
            double y = (double)inputs.Count() / 3;
            int wholeIterations = (int)Math.Floor(y);
            int remainder = (int)Math.Ceiling((y - wholeIterations) * 3);
            List<byte> decryptCandidate1 = new List<byte>();
            List<byte> decryptCandidate2 = new List<byte>();
            List<byte> decryptCandidate3 = new List<byte>();

            for (int x = 0; x < wholeIterations; x+=3)
            {
                decryptCandidate1.Add(Convert.ToByte(inputs[x]));
                decryptCandidate2.Add(Convert.ToByte(inputs[x + 1]));
                decryptCandidate3.Add(Convert.ToByte(inputs[x + 2]));
            }
            for (int x = 0; x < remainder; x++)
            {
                if (x == 0)
                {
                    decryptCandidate1.Add(inputs[(wholeIterations * 3) + x]);
                }
                if (x == 1)
                {
                    decryptCandidate2.Add(inputs[(wholeIterations * 3) + x]);
                }
            }


            Dictionary<byte, int> counts1 = new Dictionary<byte, int>();
            Dictionary<byte, int> counts2 = new Dictionary<byte, int>();
            Dictionary<byte, int> counts3 = new Dictionary<byte, int>();


            foreach (byte input in decryptCandidate1.Distinct())
            {
                counts1[input] = decryptCandidate1.Where(x => x == input).Count();
            }
            foreach (byte input in decryptCandidate2.Distinct())
            {
                counts2[input] = decryptCandidate2.Where(x => x == input).Count();
            }
            foreach (byte input in decryptCandidate3.Distinct())
            {
                counts3[input] = decryptCandidate3.Where(x => x == input).Count();
            }
            foreach (byte key in counts1.OrderByDescending(x => x.Value).Select(x => x.Key).ToList())
            {
                LogList.Add(key.ToString() + ", " + counts1[key].ToString());
                if(counts1[key] > 10)
                {
                    GetPossibleValues(key);
                }
            }
            foreach (byte key in counts2.OrderByDescending(x => x.Value).Select(x => x.Key).ToList())
            {
                LogList.Add(key.ToString() + ", " + counts2[key].ToString());
                if (counts2[key] > 10)
                {
                    GetPossibleValues(key);
                }
            }
            foreach (byte key in counts3.OrderByDescending(x => x.Value).Select(x => x.Key).ToList())
            {
                LogList.Add(key.ToString() + ", " + counts3[key].ToString());
                if (counts3[key] > 10)
                {
                    GetPossibleValues(key);
                }
            }
            LogToFile();

        }
        public void TestManualKeys()
        {
            List<byte> inputs = Input.Split(',').Select(x => Convert.ToByte(int.Parse(x))).ToList();
            double y = (double)inputs.Count() / 3;
            int wholeIterations = (int)Math.Floor(y);
            int remainder = (int)Math.Ceiling((y - wholeIterations) * 3);
            byte[] decryptKey = new byte[3];
            decryptKey[0] = Convert.ToByte(111);
            decryptKey[1] = Convert.ToByte(101);
            decryptKey[2] = Convert.ToByte(105);
            string message = DecryptInput(decryptKey, inputs, wholeIterations, remainder);
            LogList.Add(message);
            LogToFile();
            LogList.Clear();
            decryptKey[0] = Convert.ToByte(105);
            decryptKey[1] = Convert.ToByte(111);
            decryptKey[2] = Convert.ToByte(101);
            message = DecryptInput(decryptKey, inputs, wholeIterations, remainder);
            LogList.Add(message);
            LogToFile();
            LogList.Clear();
            decryptKey[0] = Convert.ToByte(101);
            decryptKey[1] = Convert.ToByte(105);
            decryptKey[2] = Convert.ToByte(111);
            message = DecryptInput(decryptKey, inputs, wholeIterations, remainder);
            LogList.Add(message);
            LogToFile();
            LogList.Clear();

        }
        public void TestKeys()
        {
            List<byte> inputs = Input.Split(',').Select(x => Convert.ToByte(int.Parse(x))).ToList();
            double y = (double)inputs.Count() / 3;
            int wholeIterations = (int)Math.Floor(y);
            int remainder = (int)Math.Ceiling((y - wholeIterations) * 3);
            // ascii lc = 97 - 122
            for (int i = 97; i <= 122; i++)
            {
                for (int j = 97; j <= 122; j++)
                {
                    for (int k = 97; k <= 122; k++)
                    {
                        byte[] decryptKey = new byte[3];
                        decryptKey[0] = Convert.ToByte(i);
                        decryptKey[1] = Convert.ToByte(j);
                        decryptKey[2] = Convert.ToByte(k);
                        string message = DecryptInput(decryptKey, inputs, wholeIterations, remainder);
                        LogList.Add(@"'[" + i.ToString() + "," + j.ToString() + "," + k.ToString() + @"]','" + message.Replace("'", "") + @"'");
                        LogToFile();
                        LogList.Clear();
                    }

                }
            }
        }
        
        public static string DecryptInput(byte[] decryptKey, List<byte> inputs, int wholeIterations, int remainder)
        {
            
            List<byte> decryptCandidate = new List<byte>();
            for (int x = 0; x < wholeIterations; x+=3)
            {
                decryptCandidate.Add(Convert.ToByte(inputs[x] ^ decryptKey[0]));
                decryptCandidate.Add(Convert.ToByte(inputs[x + 1] ^ decryptKey[1]));
                decryptCandidate.Add(Convert.ToByte(inputs[x + 2] ^ decryptKey[2]));
            }
            for (int x = 0; x < remainder; x++)
            {
                decryptCandidate.Add(Convert.ToByte(inputs[(wholeIterations * 3) + x] ^ decryptKey[x]));
            }
            
            return Encoding.ASCII.GetString(decryptCandidate.ToArray());
        }
        public void DecryptInputAsList(byte[] decryptKey, List<byte> inputs, int wholeIterations, int remainder)
        {

            List<byte> decryptCandidate = new List<byte>();
            for (int x = 0; x < wholeIterations * 3; x += 3)
            {
                decryptCandidate.Add(Convert.ToByte(inputs[x] ^ decryptKey[0]));
                decryptCandidate.Add(Convert.ToByte(inputs[x + 1] ^ decryptKey[1]));
                decryptCandidate.Add(Convert.ToByte(inputs[x + 2] ^ decryptKey[2]));
            }
            for (int x = 0; x < remainder; x++)
            {
                decryptCandidate.Add(Convert.ToByte(inputs[(wholeIterations * 3) + x] ^ decryptKey[x]));
            }
            Console.WriteLine(Encoding.ASCII.GetString(decryptCandidate.ToArray()));
            int result = 0;
            foreach (byte item in decryptCandidate)
            {
                result += Convert.ToInt32(item);
            }
            LogToFile();
            Output = result.ToString();
        }
    }
}
