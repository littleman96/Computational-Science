using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalSci
{
    class Program
    {
        private static Random rng = new Random();
        public static double startLoc = 1;
        static void Main(string[] args)
        {
            //var randomList = new List<Tuple<double, double>>();
            //for (int i = 0; i < 10000; i++)
            //{
            //    randomList.Add(GetRandom());
            //}
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter("randomResults" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".csv"))
            //{
            //    foreach (var random in randomList)
            //    {
            //        file.WriteLine( random.Item1 + "," + random.Item2);
            //    }
            //}
            var p1History = PartOne(0.01);

            var sample = 10;
            var sampleFrequency = 10;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("PartOneResults001-" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".csv"))
            {


                file.WriteLine("T,x,U");
                foreach (var pastRecord in p1History)
                {
                    if (sample == sampleFrequency)
                    {
                        file.WriteLine(String.Join(",", pastRecord));
                        sample = 1;
                    }
                    else
                    {
                        sample++;
                    }
                }
            }

            var p1History01 = PartOne(0.1);


            var p1History075 = PartOne(0.75);


            var p1History05 = PartOne(0.5);


            var p1History025 = PartOne(0.25);
            var p1History1 = PartOne(1);
            var p1History15 = PartOne(1.5);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("PartOneFull-" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".csv"))
            {
                int x01Count = 10;
                int x025Count = 25;
                int x05Count = 50;
                int x075Count = 75;
                int x1Count = 100;
                int x15Count = 150;

                file.WriteLine("T,X h=0.001,X h=0.01,X h=0.25,X h=0.5,X h=0.75");
                for (int i = 0; i < 1500; i++)
                {
                    double t = p1History[i][0];
                    double x001 = p1History[i][1];

                    string x01 = "", x025 = "", x05 = "", x1 = "", x075 = "", x15 = "";
                    if(i != 0)
                    {
                        if ((i+1) % x01Count == 0)
                            x01 = p1History01[((i+1) / x01Count)-1][1].ToString();

                        if ((i+1) % x025Count == 0)
                            x025 = p1History025[((i+1) / x025Count)-1][1].ToString();

                        if ((i+1) % x05Count == 0)
                            x05 = p1History05[((i+1) / x05Count)-1][1].ToString();

                        if ((i+1) % x075Count == 0)
                            x075 = p1History075[((i+1) / x075Count)-1][1].ToString();

                        if ((i+1) % x1Count == 0)
                            x1 = p1History1[((i+1) / x1Count) -1][1].ToString();

                        if ((i+1) % x15Count == 0)
                            x15 = p1History15[((i+1) / x15Count) -1][1].ToString();
                    }                  
                                       

                    string line = t + "," + x001 + "," + x01.ToString() + "," + x025.ToString() + "," + x05.ToString() + "," + x075.ToString()+ "," + x1.ToString()+ "," + x15.ToString();
                    file.WriteLine(line);

                }
            }

            var partTwoHistory = PartTwo(PartOne(0.01));

            var SampledPartTwo = new List<double[]>();
            sample = 10;
            foreach (var pastRecord in partTwoHistory)
            {
                if (sample == sampleFrequency)
                {
                    SampledPartTwo.Add(pastRecord);
                    sample = 1;
                }
                else
                {
                    sample++;
                }
            }
            sample = 10;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("PartTwoResults" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".csv"))
            {
                file.WriteLine("T,x,U");
                foreach (var pastRecord in SampledPartTwo)
                {
                    file.WriteLine(String.Join(",", pastRecord));

                }
            }

            PartThree(SampledPartTwo);

           

        }

        private static void PartThree(List<double[]> history)
        {
            List<double[]> inputs = new List<double[]>();
            //Normalise data
            foreach (var inputValue in history)
            {
                var normalisedValue = inputValue;
                normalisedValue[1] = (inputValue[1] - startLoc) / (3 - startLoc);
                inputs.Add(normalisedValue);
            }
            Random r = new Random();
            //Learning rate - The rate at which the weights change.
            double learningRate = 0.01;
            double totalError = 1;
            double meanSquaredError = 1;
            //Inputs
            // 2 Dimentions means we have 3 inputs. 
            double[] x = new double[3];
            x[0] = 0;
            x[1] = 0;
            x[2] = 0;
            //Weights 1 weight for each input.
            //Define inital rates 
            double maxNumber = 0.5;double minNumber = -0.5;
            double[] weights = new double[4];
            weights[0] = (r.NextDouble() * (maxNumber - minNumber)) + minNumber;
            weights[1] = (r.NextDouble() * (maxNumber - minNumber)) + minNumber;
            weights[2] = (r.NextDouble() * (maxNumber - minNumber)) + minNumber;
            weights[3] = (r.NextDouble() * (maxNumber - minNumber)) + minNumber;

            
            List<double> meanSquaredErrorList = new List<double>();
            List<double> ErrorList = new List<double>();
            int epochs = 0;

            List<double[]> results = new List<double[]>();
            do // Train the Perceptron
            {
                x[0] = inputs[0][1];
                x[1] = inputs[0][1];
                x[2] = inputs[0][1];
                totalError = new double();
                ErrorList = new List<double>();
                results = new List<double[]>();
                //Loop through the data with the current Weights
                for (int i = 0; i < inputs.Count - 1; i++)
                {
                    x[0] = inputs[i][1];
                    if (i - 1 >= 0)
                    {
                        x[1] = inputs[i - 1][1];
                    }
                    if (i - 2 >= 0)
                    {
                        x[2] = inputs[i - 2][1];
                    }

                    //perceptron Logic goes here.
                    double output = (x[0] * weights[0]) + (x[1] * weights[1]) + (x[2] * weights[2]) + weights[3];

                    //Activation fuinction.
                    //var result = (output >= 0) ? 1 : 0;

                    //Logistic Sigmoid activation function 
                    var result = 1 / (1 + Math.Exp(-output));

                    double error = inputs[i + 1][1] - result;

                    var wd0 = error * x[0] * learningRate;
                    var wd1 = error * x[1] * learningRate;
                    var wd2 = error * x[2] * learningRate;

                    weights[0] += wd0;
                    weights[1] += wd1;
                    weights[2] += wd2;
                    weights[3] += error * learningRate;

                    totalError += Math.Abs(error * error);
                    ErrorList.Add(error * error);



                    var resultsToSave = new double[3];
                    resultsToSave[0] = inputs[i + 1][0];
                    resultsToSave[1] = inputs[i + 1][1];
                    resultsToSave[2] = result;
                    results.Add(resultsToSave);
                }
                meanSquaredError = totalError / ErrorList.Count();
                meanSquaredErrorList.Add(meanSquaredError);

                epochs++;
                if (epochs > 50000)
                {
                    break;
                }               

            } while (meanSquaredError > 0.0025);//If Error is below target end training.           

            using (System.IO.StreamWriter file = new System.IO.StreamWriter("PartThreeResuslts" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".csv"))
            {
                file.WriteLine("Item, Expected, Predicted");
                foreach (var result in results)
                {

                    file.WriteLine((String.Join(",", result)));

                }
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("ErrorOverTime" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".csv"))
            {
                file.WriteLine("error");
                foreach (var result in meanSquaredErrorList)
                {

                    file.WriteLine(result);

                }
            }

        }
        private static List<double[]> PartTwo(List<double[]> PartTwoHistory)
        {
            for (int x = 0; x < PartTwoHistory.Count - 1; x = x + 2)
            {
                var randomNumbers = GetRandom();

                PartTwoHistory[x][1] = PartTwoHistory[x][1] + randomNumbers.Item1;

                PartTwoHistory[x + 1][1] = PartTwoHistory[x + 1][1] + randomNumbers.Item2;

            }
            return PartTwoHistory;
        }

        static List<double[]> PartOne(double h)
        {
            List<double[]> history = new List<double[]>();
            double k = 0;
            double T = 0;
            double U = 0;

            double a = -2;
            double[] x = new double[2];
            x[0] = startLoc;

            while (T <= 15)
            {
                if (T <= 5)
                {
                    U = 2;
                }
                else if (T <= 10)
                {
                    U = 1;
                }
                else if (T <= 15)
                {
                    U = 3;
                }

                x[1] = x[0] + (h * (a * x[0] + 2 * U));

               
                k = k + 1;

                var record = new double[3];
                T = T + h;
                record[0] = T;
                record[1] = x[1];
                record[2] = U;
                history.Add(record);
                
                x[0] = x[1];
            }
            return history;
        }

        static Tuple<double, double> GetRandom()
        {
            double maximum = 2 * Math.PI;
            double minimum = 0;

            var deviation = 0.001;

            double a = (rng.NextDouble() * (maximum - minimum)) + minimum; ;
            double rand = rng.NextDouble();
            var b = deviation * Math.Sqrt(-2 * Math.Log(rand));

            var x1 = (b * Math.Sin(a)) + 0;
            var x2 = (b * Math.Cos(a)) + 0;

          
            var touple = new Tuple<double, double>(x1, x2);

            return touple;
        }
    }
}
