using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace IA_TP1
{
    public class Agent
    {
        private MainWindow main;
        int x;
        int y;
        int i;
        int j;
        float[] proportions = new float[4];

        public Agent(MainWindow mw,int x0,int y0)
        {
            main = mw;
            x = x0;
            y = y0;
        }

        public void ObserveEnvironmentWithAllMySensors(int x0, int y0)
        {
            //Observation de la zone1
            for (i = 0; i < 2; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    if ((main.villa.villaMap[j, i] == "S" ) || (main.villa.villaMap[j, i] == "DS"))
                    {
                        proportions[0] += 1;
                    }
                }
            }

            //Observation de la zone2
            for (i = 2; i < 5; i++)
            {
                for (j = 0; j < 2; j++)
                {
                    if ((main.villa.villaMap[j, i] == "S") || (main.villa.villaMap[j, i] == "DS"))
                    {
                        proportions[1] += 1;
                    }
                }
            }

            //Observation de la zone3
            for (i = 0; i < 3; i++)
            {
                for (j = 3; j < 5; j++)
                {
                    if ((main.villa.villaMap[j, i] == "S") || (main.villa.villaMap[j, i] == "DS"))
                    {
                        proportions[2] += 1;
                    }
                }
            }

            //Observation de la zone4
            for (i = 3; i < 5; i++)
            {
                for (j = 2; j < 5; j++)
                {
                    if ((main.villa.villaMap[j, i] == "S") || (main.villa.villaMap[j, i] == "DS"))
                    {
                        proportions[3] += 1;
                    }
                }
            }
            //Ajout de la gestion de la case centrale à la zone4
            if ((main.villa.villaMap[2, 2] == "S") || (main.villa.villaMap[2, 2] == "DS"))
            {
                proportions[3] += 1;
            }
        }
    }
}

