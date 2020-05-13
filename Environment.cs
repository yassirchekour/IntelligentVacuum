using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Input;


namespace IA_TP1
{
    public class Environment
    {
        public string[,] villaMap = new string[5, 5];
        private MainWindow main;
        private int actSec = 0;
        private static int EnvPerformance; // Pour le moment : +3 case propre / -1 case sale / -1 diamant par terre

        private static Random rnd;
        private static object syncObj = new object();


        

        public Environment(MainWindow mw)
        {
            EnvPerformance = 0;
            main = mw;
            for(int i =0; i<5; i++)
            {
                for(int j = 0;j<5;j++)
                {
                    villaMap[i, j] = "E";
                    EnvPerformance += 3;
                }
            }
            villaMap[3, 2] = "S";
            EnvPerformance -= 1;
        }

        public static int getEnvPerformance() { return EnvPerformance; }

        private static void InitRandomNumber(int seed)
        {
            rnd = new Random(seed);
        }
        private static int GenerateRandomNumber(int max)
        {
            lock(syncObj)
            {
                if(rnd == null)
                {
                    rnd = new Random();
                }
                return rnd.Next(0,max);
            }

        }

        public void Start()
        {
            int waitingTurn = 0;
            while(true)
            {
                Update();
                Thread.Sleep(1000);
                actSec++;
                waitingTurn++;
                
                if (waitingTurn>3)
                {
                    waitingTurn = 0;
                    RandomDust(); 
                    RandomDiamond();
                    CalculPerformance();
                }
            }
        }


        public void Update()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { main.RefreshEnv(); }));
            
        }

        public int CalculPerformance()
        {
            EnvPerformance = 0;
            for(int i =0; i<5; i++)
            {
                for(int j = 0;j<5;j++)
                {
                    if (villaMap[i, j] == "E")
                    {
                        EnvPerformance += 3;
                    }
                    if (villaMap[i, j] == "S")
                    {
                        EnvPerformance -= 1;
                    }
                    if (villaMap[i, j] == "D")
                    {
                        EnvPerformance -= 1;
                    }
                    if (villaMap[i, j] == "DS")
                    {
                        EnvPerformance -= 2;
                    }
                }
            }
            return EnvPerformance;
        }

        public void RandomDust()
        {

            // pour le moment 1 chance sur 2
            int isNewDust = GenerateRandomNumber(2);

            if (isNewDust == 0)
            {
                int x = GenerateRandomNumber(5);
                int y = GenerateRandomNumber(5);

                Console.WriteLine("Nouvelle poussière en " + x + " " + y);

                if (villaMap[x,y].Equals("E"))            
                {           
                    villaMap[x, y] = "S";                  
                }
         
                else if(villaMap[x,y].Equals("D"))        
                {        
                    villaMap[x, y] = "DS";         
                }
            }
        }

        public void RandomDiamond()
        {
            // pour le moment 1 chance sur 5
            int isNewDiamond = GenerateRandomNumber(5);

            if (isNewDiamond == 0)
            {
                int x = GenerateRandomNumber(5);
                int y = GenerateRandomNumber(5);

                Console.WriteLine("Nouveau diamant en " + x + " " + y);
               
                if(villaMap[x,y].Equals("E"))
                {
                    villaMap[x, y] = "D";
                }
                if(villaMap[x,y].Equals("S"))
                {
                    villaMap[x, y] = "DS";
                } 
            }
        }       
    }    
}
    


