using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEulerSolver.Legacy
{
/// Summary
/// Class to hold the code for the first fifteen Euler solutions before I started
/// trying to keep the code base organized   
    class FirstFifteenProblems
    {
        private static Int64 GetPaths()
        {
            int result = 0;
            int x = 0;
            int y = 0;
            while(x < 20 && y < 20)
            {
                
            }
            for(int i = 0; i < 21; i++)
            {
                for(int j = 0; j < 21; j++)
                {

                }
            }

            return result;
        }
        private static Int64 CollatzNumbers()
        {
            Int64 result = 0;
            Int64 highestCount = 0;
            for(Int64 i = 1000000; i > 0; i--)
            {
                Int64 x = i;
                List<Int64> collatzes = new List<Int64>();
                while(Collatz(x) > 1)
                {
                    x = Collatz(x);
                    collatzes.Add(x);
                }
                if(collatzes.Count > highestCount)
                {
                    Console.WriteLine("--" + i.ToString());
                    foreach(Int64 item in collatzes)
                    {
                        Console.WriteLine("-----" + item.ToString());
                    }
                    
                    highestCount = collatzes.Count;
                    result = i;
                }
            }

            return result;
        }
        private static Int64 Collatz(Int64 x)
        {
            if(x%2 == 0)
            {
                return x/2;
            }
            return 3*x + 1;
        }
        static Int64 GetTriangleNumberWithDivisors(Int64 divisorCount)
        {
            string filePath = @"";
            Int64 triangleNumber = 0;
            Int64 divisor = 0;
            Int64 nextNatural = 1;
            using (System.IO.StreamWriter file = 
            new System.IO.StreamWriter(filePath))
            {
                while(divisor < divisorCount + 1)
                {
                    triangleNumber = triangleNumber + nextNatural;
                    nextNatural++;
                    //divisor = GetFactorCount(triangleNumber);
                    divisor = GetDivisorCount(triangleNumber);
                    file.WriteLine(triangleNumber.ToString() + "    " + divisor.ToString());
                }
            }
            return triangleNumber;
        }
        private static string SumLargeNumbers(string number1, string number2)
        {
            string result = "";
            Console.WriteLine(number1);
            Console.WriteLine(number2);
            
            int maxLength = Math.Min(number1.Length, number2.Length);
            int carry = 0;
            for(int i = 1; i <= maxLength; i++)
            {
                int j = int.Parse(number1.Substring(number1.Length - i, 1));
                int k = int.Parse(number2.Substring(number2.Length - i, 1));
                string l = (j + k + carry).ToString();
                if(l.Length > 1)
                {
                    carry = 1;
                    result = l.Substring(1,1) + result;
                }
                else
                {
                    carry = 0;
                    result = l + result;
                }
            }
            if(number1.Length > number2.Length)
            {
                int difference = number1.Length - number2.Length;
                if(carry == 0)
                {
                    result = number1.Substring(0, difference) + result;
                }
                else
                {
                    for(int i = difference; i >= 0; i--)
                    {
                        int j = int.Parse(number1.Substring(i,1));
                        string l = (j + carry).ToString();
                        if(l.Length > 1)
                        {
                            carry = 1;
                            result = l.Substring(1,1) + result;
                        }
                        else
                        {
                            carry = 0;
                            result = l + result;
                        }
                    }
                    if(carry != 0)
                    {
                        result = "1" + result;
                    }
                }
            }
            else if(number2.Length > number1.Length)
            {
                int difference = number2.Length - number1.Length;
                if(carry == 0)
                {
                    result = number2.Substring(0, difference) + result;
                }
                else
                {
                    for(int i = difference; i >= 0; i--)
                    {
                        int j = int.Parse(number2.Substring(i,1));
                        string l = (j + carry).ToString();
                        if(l.Length > 1)
                        {
                            carry = 1;
                            result = l.Substring(1,1) + result;
                        }
                        else
                        {
                            carry = 0;
                            result = l + result;
                        }
                    }
                    if(carry != 0)
                    {
                        result = "1" + result;
                    }
                }
            }
            else
            {
                if(carry != 0)
                {
                    result = "1" + result;
                }
            }
            return result;
        }
        private static string SumListOfVeryLargeNumbers(string[] numbersToSum)
        {
            string answer = numbersToSum[0];

            for(int i = 1; i < numbersToSum.Length; i++)
            {
                answer = SumLargeNumbers(answer, numbersToSum[i]);
            }

            return answer;
        }
        private static string RunSumming()
        {

            //    "37107287533902102798797998220837590246510135740250",
            //    "46376937677490009712648124896970078050417018260538",
            //                                                54000788
            //83484225211392112511446123117807668296927154000788

            string[] numbersToSum = new string[]
            {
                "37107287533902102798797998220837590246510135740250",
                "46376937677490009712648124896970078050417018260538",
                "74324986199524741059474233309513058123726617309629",
                "91942213363574161572522430563301811072406154908250",
                "23067588207539346171171980310421047513778063246676",
                "89261670696623633820136378418383684178734361726757",
                "28112879812849979408065481931592621691275889832738",
                "44274228917432520321923589422876796487670272189318",
                "47451445736001306439091167216856844588711603153276",
                "70386486105843025439939619828917593665686757934951",
                "62176457141856560629502157223196586755079324193331",
                "64906352462741904929101432445813822663347944758178",
                "92575867718337217661963751590579239728245598838407",
                "58203565325359399008402633568948830189458628227828",
                "80181199384826282014278194139940567587151170094390",
                "35398664372827112653829987240784473053190104293586",
                "86515506006295864861532075273371959191420517255829",
                "71693888707715466499115593487603532921714970056938",
                "54370070576826684624621495650076471787294438377604",
                "53282654108756828443191190634694037855217779295145",
                "36123272525000296071075082563815656710885258350721",
                "45876576172410976447339110607218265236877223636045",
                "17423706905851860660448207621209813287860733969412",
                "81142660418086830619328460811191061556940512689692",
                "51934325451728388641918047049293215058642563049483",
                "62467221648435076201727918039944693004732956340691",
                "15732444386908125794514089057706229429197107928209",
                "55037687525678773091862540744969844508330393682126",
                "18336384825330154686196124348767681297534375946515",
                "80386287592878490201521685554828717201219257766954",
                "78182833757993103614740356856449095527097864797581",
                "16726320100436897842553539920931837441497806860984",
                "48403098129077791799088218795327364475675590848030",
                "87086987551392711854517078544161852424320693150332",
                "59959406895756536782107074926966537676326235447210",
                "69793950679652694742597709739166693763042633987085",
                "41052684708299085211399427365734116182760315001271",
                "65378607361501080857009149939512557028198746004375",
                "35829035317434717326932123578154982629742552737307",
                "94953759765105305946966067683156574377167401875275",
                "88902802571733229619176668713819931811048770190271",
                "25267680276078003013678680992525463401061632866526",
                "36270218540497705585629946580636237993140746255962",
                "24074486908231174977792365466257246923322810917141",
                "91430288197103288597806669760892938638285025333403",
                "34413065578016127815921815005561868836468420090470",
                "23053081172816430487623791969842487255036638784583",
                "11487696932154902810424020138335124462181441773470",
                "63783299490636259666498587618221225225512486764533",
                "67720186971698544312419572409913959008952310058822",
                "95548255300263520781532296796249481641953868218774",
                "76085327132285723110424803456124867697064507995236",
                "37774242535411291684276865538926205024910326572967",
                "23701913275725675285653248258265463092207058596522",
                "29798860272258331913126375147341994889534765745501",
                "18495701454879288984856827726077713721403798879715",
                "38298203783031473527721580348144513491373226651381",
                "34829543829199918180278916522431027392251122869539",
                "40957953066405232632538044100059654939159879593635",
                "29746152185502371307642255121183693803580388584903",
                "41698116222072977186158236678424689157993532961922",
                "62467957194401269043877107275048102390895523597457",
                "23189706772547915061505504953922979530901129967519",
                "86188088225875314529584099251203829009407770775672",
                "11306739708304724483816533873502340845647058077308",
                "82959174767140363198008187129011875491310547126581",
                "97623331044818386269515456334926366572897563400500",
                "42846280183517070527831839425882145521227251250327",
                "55121603546981200581762165212827652751691296897789",
                "32238195734329339946437501907836945765883352399886",
                "75506164965184775180738168837861091527357929701337",
                "62177842752192623401942399639168044983993173312731",
                "32924185707147349566916674687634660915035914677504",
                "99518671430235219628894890102423325116913619626622",
                "73267460800591547471830798392868535206946944540724",
                "76841822524674417161514036427982273348055556214818",
                "97142617910342598647204516893989422179826088076852",
                "87783646182799346313767754307809363333018982642090",
                "10848802521674670883215120185883543223812876952786",
                "71329612474782464538636993009049310363619763878039",
                "62184073572399794223406235393808339651327408011116",
                "66627891981488087797941876876144230030984490851411",
                "60661826293682836764744779239180335110989069790714",
                "85786944089552990653640447425576083659976645795096",
                "66024396409905389607120198219976047599490197230297",
                "64913982680032973156037120041377903785566085089252",
                "16730939319872750275468906903707539413042652315011",
                "94809377245048795150954100921645863754710598436791",
                "78639167021187492431995700641917969777599028300699",
                "15368713711936614952811305876380278410754449733078",
                "40789923115535562561142322423255033685442488917353",
                "44889911501440648020369068063960672322193204149535",
                "41503128880339536053299340368006977710650566631954",
                "81234880673210146739058568557934581403627822703280",
                "82616570773948327592232845941706525094512325230608",
                "22918802058777319719839450180888072429661980811197",
                "77158542502016545090413245809786882778948721859617",
                "72107838435069186155435662884062257473692284509516",
                "20849603980134001723930671666823555245252804609722",
                "53503534226472524250874054075591789781264330331690"
            };
            string total = "";
            int carry = 0;
            for(int i = 49; i >= 0; i--)
            {
                int columnTotal = carry;
                carry = 0;
                for(int j = 0; j < 100; j++)
                {
                    columnTotal += int.Parse(numbersToSum[j].Substring(i,1));
                }
                string temp = columnTotal.ToString();
                total = temp.Substring(temp.Length - 1, 1) + total;
                Console.WriteLine(temp);
                Console.WriteLine(temp.Substring(temp.Length - 1, 1));
                if(temp.Length > 1)
                {
                    carry = columnTotal - int.Parse(temp.Substring(temp.Length - 1, 1));
                    carry = carry/10;
                    Console.WriteLine(carry.ToString());
                }
                Console.WriteLine(total);
            }
            total = carry.ToString() + total;
            return total.Substring(0,10);
        }
        
        private static Int64 GetDivisorCount(Int64 n)
        {
            Int64 count = 2;
            if(n%2 == 0)
            {
                count+=2;
                Int64 max = n/2;
                for(Int64 i = 3; i < max; i+=1)
                {
                    if(n%i == 0)
                    {
                        count++;
                    }
                } 
            }
            else
            {
                Int64 max = (n+1)/2;
                for(Int64 i = 3; i < max; i+=2)
                {
                    if(n%i == 0)
                    {
                        count++;
                    }
                }   
            }
            return count;
        }
        private static Int64 GetLargestProductFromSquareMatrix(int dimension, int productFactorCount, int digitLength, string numbersMatrix)
        {
            Int64 highestProduct = 1;
            int vectorLength = productFactorCount * digitLength;
            int rowLength = (dimension * digitLength) - vectorLength;
            int oneFullRowOffset = dimension * digitLength;
            int oneFullColumnOffset = digitLength;
            for(int i = 0; i < dimension; i++)
            {
                for(int j = 0; j < dimension; j++)
                {
                    Int64 horizontalProduct = 1;
                    Int64 verticalProduct = 1;
                    Int64 rightDiagonalProduct = 1;
                    Int64 leftDiagonalProduct = 1;
                    string horizontalVector = "";
                    string verticalVector = "";
                    string rightDiagonalVector = "";
                    string leftDiagonalVector = "";
                    for(int k = 0; k < productFactorCount; k++)
                    {
                        int position = i*oneFullRowOffset + j*oneFullColumnOffset + k*oneFullColumnOffset;
                        if(position < numbersMatrix.Length)
                        {
                            horizontalVector += numbersMatrix.Substring(position, digitLength);
                            horizontalProduct *= int.Parse(numbersMatrix.Substring(i*oneFullRowOffset + j*oneFullColumnOffset + k*oneFullColumnOffset, digitLength));
                        }
                        else
                        {
                            horizontalVector = "";
                            horizontalProduct = 0;
                        }

                        position = i*oneFullColumnOffset + j*oneFullRowOffset + k*oneFullRowOffset;
                        if(position < numbersMatrix.Length)
                        {
                            verticalVector += numbersMatrix.Substring(position, digitLength);
                            verticalProduct *= int.Parse(numbersMatrix.Substring(position, digitLength));
                        }
                        else
                        {
                            verticalVector = "";
                            verticalProduct = 0;
                        }

                        position = i*oneFullRowOffset + j*oneFullColumnOffset + k*oneFullColumnOffset + k*oneFullRowOffset;
                        if(position < numbersMatrix.Length)
                        {
                            rightDiagonalVector += numbersMatrix.Substring(position, digitLength);
                            rightDiagonalProduct *= int.Parse(numbersMatrix.Substring(position, digitLength));
                        }
                        else
                        {
                            rightDiagonalVector = "";
                            rightDiagonalProduct = 0;
                        }
                        
                        position = i*oneFullRowOffset + (oneFullRowOffset - (j+1)*oneFullColumnOffset) - (k)*oneFullColumnOffset + k*oneFullRowOffset;
                        if(position < numbersMatrix.Length)
                        {
                            leftDiagonalVector += numbersMatrix.Substring(position, digitLength);
                            leftDiagonalProduct *= int.Parse(numbersMatrix.Substring(position, digitLength));
                        }
                        else
                        {
                            leftDiagonalVector = "";
                            leftDiagonalProduct = 0;
                        }
                    }
                    //Console.WriteLine("Vector Starting at Position: [" + i.ToString() + ", " + j.ToString() + "]");
                    //Console.WriteLine("Horizontal Vector: " + horizontalVector);
                    //Console.WriteLine("Vertical Vector: " + verticalVector);
                    //Console.WriteLine("Right Vector: " + rightDiagonalVector);
                    //Console.WriteLine("Left Vector: " + leftDiagonalProduct);
                    if(horizontalProduct > highestProduct)
                    {
                        highestProduct = horizontalProduct;
                    }
                    if(verticalProduct > highestProduct)
                    {
                        highestProduct = verticalProduct;
                    }
                    if(rightDiagonalProduct > highestProduct)
                    {
                        highestProduct = rightDiagonalProduct;
                    }
                    if(leftDiagonalProduct > highestProduct)
                    {
                        highestProduct = leftDiagonalProduct;
                    }
                }
            }
           return highestProduct;
        }
        private void Storage()
        {
             int vectorLength = 13;
            
            string secondNumber =   "73167176531330624919225119674426574742355349194934" +
                                    "96983520312774506326239578318016984801869478851843" +
                                    "85861560789112949495459501737958331952853208805511" +
                                    "12540698747158523863050715693290963295227443043557" +
                                    "66896648950445244523161731856403098711121722383113" +
                                    "62229893423380308135336276614282806444486645238749" +
                                    "30358907296290491560440772390713810515859307960866" +
                                    "70172427121883998797908792274921901699720888093776" +
                                    "65727333001053367881220235421809751254540594752243" +
                                    "52584907711670556013604839586446706324415722155397" +
                                    "53697817977846174064955149290862569321978468622482" +
                                    "83972241375657056057490261407972968652414535100474" +
                                    "82166370484403199890008895243450658541227588666881" +
                                    "16427171479924442928230863465674813919123162824586" +
                                    "17866458359124566529476545682848912883142607690042" +
                                    "24219022671055626321111109370544217506941658960408" +
                                    "07198403850962455444362981230987879927244284909188" +
                                    "84580156166097919133875499200524063689912560717606" +
                                    "05886116467109405077541002256983155200055935729725" +
                                    "71636269561882670428252483600823257530420752963450";

            string matrixProduct =  "0802229738150040007504050778521250779108" + 
                                    "4949994017811857608717409843694804566200" +
                                    "8149317355791429937140675388300349133665" +
                                    "5270952304601142692468560132567137023691" +
                                    "2231167151676389419236542240402866331380" +
                                    "2447326099034502447533537836842035171250" +
                                    "3298812864236710263840675954706618386470" +
                                    "6726206802621220956394396308409166499421" +
                                    "2455580566739926971778789683148834896372" +
                                    "2136230975007644204535140061339734313395" +
                                    "7817532822753167159403800462161409535692" +
                                    "1639054296353147555888240017542436298557" +
                                    "8656004835718907054444374460215851541758" +
                                    "1980816805944769287392138652177704895540" +
                                    "0452088397359916079757321626267933279866" +
                                    "8836688757622072034633674655123263935369" +
                                    "0442167338253911249472180846293240627636" +
                                    "2069364172302388346299698267598574043616" +
                                    "2073352978319001743149714886811623570554" +
                                    "0170547183515469169233486143520189196748";
            Console.WriteLine(vectorLength.ToString() + secondNumber + matrixProduct);

        }
        private static string GetHighestProductVector(string numberToParse, int vectorLength)
        {
            int maxLength = numberToParse.Length - vectorLength;
            string vectorToOutput = "";
            int highestProduct = 1;
            for(int i = 0; i < maxLength; i++)
            {
                int product = 1;
                string vector = numberToParse.Substring(i, 13);
                for(int j = 0; j < 13; j++)
                {
                    product *= int.Parse(vector.Substring(j,1));
                }
                if(product > highestProduct)
                {
                    highestProduct = product;
                    vectorToOutput = vector;
                }
            }
            return vectorToOutput;
        }
        private static Int64 GetHighestProduct(string numberToParse, int vectorLength)
        {
            int maxLength = numberToParse.Length - vectorLength;
            Console.WriteLine(maxLength);
            Int64 highestProduct = 1;
            for(int i = 0; i < maxLength; i++)
            {
                Int64 product = 1;
                string vector = numberToParse.Substring(i, 13);
                for(int j = 0; j < 13; j++)
                {
                    product *= int.Parse(vector.Substring(j,1));
                }
                Console.WriteLine("[" + i.ToString() + "] - Vector: " + vector + " Product: " + product.ToString());
                
                if(product > highestProduct)
                {
                    highestProduct = product;
                }
            }
            return highestProduct;
        }
        private static string GetPythagTrips()
        {
            for(int i = 1; i < 1001; i++)
            {
                for(int j = 1; j < 1001; j++)
                {
                    for(int k = 1; k < 1001; k++)
                    {
                        if(i + j + k == 1000)
                        {
                            if(i*i + j*j == k*k)
                            {
                                Int64 solution = i * j * k;
                                return solution.ToString();
                                //return i.ToString() + ", " + j.ToString() + ", " + k.ToString(); 
                            }
                        }
                    }
                }
            }

            return "";
        }
        private static int GetDifferenceBetweenSumOfSquares()
        {
            int sum = 0;
            int squares = 0;
            for(int i = 1; i < 101; i++)
            {
                sum += i;
                squares += i*i;

            }
            return Math.Abs(sum*sum - squares);
        }
        private static Int64 SumSievePrimesFoAllBelowMax(int max)
        {
            Dictionary<Int64, bool> isPrime = new Dictionary<Int64, bool>();
            for (int i = 2; i <= max; i++) 
            {
                isPrime[i] = true;
            }

            for (int i = 2; i <= max; i++)
            {
                if (isPrime[i])
                {
                    for (int j = i * 2; j <= max; j += i)
                    {
                        isPrime[j] = false;
                    }
                }
            }
            return isPrime.Where(p => p.Value).Select(p => p.Key).Sum();
            
                                     
        }
        private static int SievePrimesForNth(int max, int Nth)
        {
            Dictionary<int, bool> isPrime = new Dictionary<int, bool>();
            for (int i = 2; i <= max; i++) 
            {
                isPrime[i] = true;
            }

            for (int i = 2; i <= max; i++)
            {
                if (isPrime[i])
                {
                    for (int j = i * 2; j <= max; j += i)
                    {
                        isPrime[j] = false;
                    }
                }
            }
            List<int> primes = isPrime.Where(p => p.Value).Select(p => p.Key).OrderBy(k => k).ToList();
            if(primes.Count > Nth)
            {
                return primes[Nth];
            }
            else
            {
                return SievePrimesForNth(max*2, Nth);
            }
                                     
        }
        private static Int64 GetLargestPrimeFactor(Int64 n)
        {
            while(n%2 == 0)
            {
                n = n/2;
            }

            for (int i = 3; i <= Math.Sqrt(n); i = i+2)
            {
                while (n%i == 0)
                {
                    n = n/i;
                }
            }

            return n;
        }
        private static Int64 GetMultiples()
        {
            var finalFactors = GetPrimeFactorCounts(1);
            for(int i = 1; i < 21; i++)
            {
                var nextPrimeFactors = GetPrimeFactorCounts(i);
                foreach(int key in nextPrimeFactors.Keys)
                {
                    if(finalFactors.ContainsKey(key))
                    {
                        if(finalFactors[key] < nextPrimeFactors[key])
                        {
                            finalFactors[key] = nextPrimeFactors[key];
                        }
                    }
                    else
                    {
                        finalFactors[key] = nextPrimeFactors[key];
                    }
                }
            }
            Int64 solution = 0;
            foreach(int key in finalFactors.Keys)
            {
                solution =+ key^finalFactors[key];
            }
            return solution;
        }

        private static Dictionary<int, int> GetPrimeFactorCounts(Int64 n)
        {
            var primeList = new  Dictionary<int, int>();
            GetPrimeFactors(n).GroupBy(s => s).Select(s => new { primeList = s.Key, Count = s.Count() });
            return primeList;
        }
        private static int GetFactorCount(int n)
        {
            int result = GetPrimeFactors(n).GroupBy(s => s).Select(s => s.Count() + 1).Aggregate((a, x) => a * x);
            return result;
        }
        private static List<int> GetPrimeFactors(Int64 n)
        {
            var PrimeFactors = new List<int>();
                PrimeFactors.Add(1);


            if(n%2 == 0)
            {
                while(n%2 == 0)
                {
                    PrimeFactors.Add(2);
                    n = n/2;
                }
            }

            for (int i = 3; i <= Math.Sqrt(n); i = i+2)
            {
                if(n%i == 0)
                {
                    while (n%i == 0)
                    {
                        PrimeFactors.Add(i);
                        n = n/i;
                    }
                }
            }
            return PrimeFactors;
        }
        private static string GetLargestPalindromeProduct()
        {

            var Products = new List<int>();
            for (int i = 999; i > 99; i--)
            {
                for (int j = i; j > 99; j--)
                {
                    Products.Add(i * j);
                }
            }
            int n = 0;
            var orderedProducts = Products.Distinct().OrderByDescending(i => i).ToList();
            foreach(int i in orderedProducts)
            {
                if(i.ToString().Reverse().Aggregate(0, (b, x) => 10 * b + x - '0').ToString() == i.ToString())
                {
                    n = i;
                    break;
                }
            }



            return n.ToString();
        }

        private static string GetSmallestMultiple(int a, int b)
        {
            //smallest n that is evenly divisible by all natural numbers between 1 and 20
            // get abs(a*b)/gcd(a,b)
            int n = Math.Abs(a*b)/GetGreatestCommonDivisor(a,b);
            return n.ToString();
        }
        private static int GetGreatestCommonDivisor(int a, int b)
        {
            if(a < b)
            {
                int temp = a;
                a = b;
                b = temp;
            }
            int remainder; 
            int priorRemainder;
            int nextRemainder;
            int quotient  = Math.DivRem(a, b, out priorRemainder);
            if(priorRemainder != 0)
            {
                quotient  = Math.DivRem(b, priorRemainder, out remainder);
                if(remainder != 0)
                {
                    nextRemainder = remainder;
                    while(remainder != 0)
                    {
                        quotient = Math.DivRem(priorRemainder, nextRemainder, out remainder);
                        priorRemainder = nextRemainder;
                        nextRemainder = remainder;
                    }
                    return priorRemainder;
                }
                else
                {
                    return priorRemainder;
                }
            }
            else
            { 
                return b;
            }
        }
    }
}
