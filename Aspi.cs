using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace IA_TP1
{

    public class Aspi
    {
        private MainWindow main;
        int x;
        int y;
        private int AspiPerformance;
        private bool isCleaning = false;


        public Aspi(MainWindow mw, int x0, int y0)
        {
            main = mw;
            x = x0;
            y = y0;
            AspiPerformance = 0;
        }
        public void Start()
        {
            while (true)
            {
                Update();
                Thread.Sleep(1000);
                //Move('S');     
                //Aspire();

                S(ObserveEnvironmentWithAllMySensors());
            }
        }

        public void Update()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { main.UpdateAspiratorPosition(x,y); }));
            Application.Current.Dispatcher.Invoke(new Action(() => { main.RefreshEnv(); }));
        }

        public void ResetAspiPerformance()
        {
            AspiPerformance = Environment.getEnvPerformance();
        }

        public void Move(char M)
        {
            //Se déplacer coûte de l'electricité
            AspiPerformance -= 1;

            switch(M)
            {
                case 'Z':
                    y = y - 1;
                    break;
                case 'Q':
                    x = x - 1;
                    break;
                case 'S':
                    y = y + 1;
                    break;
                case 'D':
                    x = x + 1;
                    break;
            }
        }

        public void Aspire()
        {
            //Chaque action coûte de l'électricité
            AspiPerformance -=1 ;

            if ((main.villa.villaMap[y, x] == "DS") || (main.villa.villaMap[y, x] == "D"))
            {
                //Aspirer un diamant fait baisser la mesure de performance de l'aspirateur
                AspiPerformance -= 10;
            }
            main.villa.villaMap[y, x] = "E";
        }

        public void PickUp()
        {
            //Chaque action coûte de l'électricité
            AspiPerformance -=1 ;

            if (main.villa.villaMap[y, x] == "DS")
            {
                 main.villa.villaMap[y, x] = "S";
            }            
            if (main.villa.villaMap[y, x] == "D")
            {
                 main.villa.villaMap[y, x] = "E";
            }
        }

        public bool GoalTest(string[,] map)
        {
            for(int i =0; i<5; i++)
            {
                for(int j = 0;j<5;j++)
                {
                    if (map[i, j] != "E") {return false;}
                }
            }
            return true;
        }

        //Défintion d'un état
        public struct Etat
        {
            public string[,] map {get; set; }
            public int x {get; set; }
            public int y {get; set; }
        }

        //Definition d'un noeud
        public class Noeud
        {
            public Noeud parent;
            public string action;
            public Etat etat;
            public int profondeur;
        }

        //Definition d'un arbre
        public struct Arbre
        {
            HashSet<Noeud> arbre;
        }

        //retourne l'état courant observé par l'agent
        public Etat ObserveEnvironmentWithAllMySensors()
        {
            string [,] map = new string[5, 5];
            for(int i =0; i<5; i++)
            {
                for(int j = 0;j<5;j++)
                {
                    map[i,j]=main.villa.villaMap[i,j];
                }
            }

            Etat ret = new Etat();
            ret.map = map;
            ret.x = x;
            ret.y = y;

            return ret;
        }

        public void printEtat(Etat e)
        {
            Console.WriteLine("x ="+e.x+" y="+e.y);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.WriteLine(" / "+e.map[i, j]+" /");
                }
            }

        }

        //fonction de succession : à un etat x (map + pos aspi) on associe tous les états où l'on peut aller avec les actions disponibles (Z,Q,S,D,A(spire),P(ickUp))
        //TODO Ajoutez memoire pour eviter boucles
        public (Etat,string)[] S(Etat e)
        {
            Etat[] ret = new Etat[6];
            (Etat,string)[] succ = new (Etat,String)[6];
            //Z
            ret[0]=e;
            ret[0].y = y-1;
            succ[0]=(ret[0],"Z");
            /*Console.WriteLine("Z =>");
            printEtat(ret[0]);*/

            //Q
            ret[1]=e;
            ret[1].x = x-1;
            succ[1]=(ret[1],"Q");
            /*Console.WriteLine("Q =>");
            printEtat(ret[1]);*/

            //S
            ret[2]=e;
            ret[2].y = y+1;
            succ[2]=(ret[2],"S");
            /*Console.WriteLine("S =>");
            printEtat(ret[2]);*/

            //D
            ret[3]=e;
            ret[3].x = x+1;
            succ[3]=(ret[3],"D");
            /*Console.WriteLine("D =>");
            printEtat(ret[3]);*/

            //A
            ret[4]=e;
            ret[4].map[e.x,e.y] = "E";
            succ[4]=(ret[4],"A");
            /*Console.WriteLine("A =>");
            printEtat(ret[4]);*/

            //P
            ret[5]=e;
            if (ret[5].map[e.x,e.y] == "DS")
            {
                 ret[5].map[e.x,e.y] = "S";
            }            
            if (ret[5].map[e.x,e.y] == "D")
            {
                 ret[5].map[e.x,e.y] = "E";
            }
            succ[5]=(ret[5],"P");
            /*Console.WriteLine("P =>");
            printEtat(ret[5]);*/

            return succ;
        }

        public (Etat,string,int)[] SwithPerf(Etat e)
        {
            int p1,p2,p3,p4,p5,p6;
            Etat[] ret = new Etat[6];
            (Etat,string,int)[] succ = new (Etat,String,int)[6];
            //Z
            ret[0]=e;
            ret[0].y = y-1;
            p1 = AspiPerformance-1;
            succ[0]=(ret[0],"Z",p1);
            /*Console.WriteLine("Z =>");
            printEtat(ret[0]);*/

            //Q
            ret[1]=e;
            ret[1].x = x-1;
            p2 = AspiPerformance-1;
            succ[1]=(ret[1],"Q",p2);
            /*Console.WriteLine("Q =>");
            printEtat(ret[1]);*/

            //S
            ret[2]=e;
            ret[2].y = y+1;
            p3 = AspiPerformance-1;
            succ[2]=(ret[2],"S",p3);
            /*Console.WriteLine("S =>");
            printEtat(ret[2]);*/

            //D
            ret[3]=e;
            ret[3].x = x+1;
            p4 = AspiPerformance-1;
            succ[3]=(ret[3],"D",p4);
            /*Console.WriteLine("D =>");
            printEtat(ret[3]);*/

            //A
            ret[4]=e;
            ret[4].map[e.x,e.y] = "E";
            p5 = AspiPerformance-1;
            if ((main.villa.villaMap[y, x] == "DS") || (main.villa.villaMap[y, x] == "D"))
            {
                //Aspirer un diamant fait baisser la mesure de performance de l'aspirateur
                p5 -= 10;
            }
            succ[4]=(ret[4],"A",p5);
            /*Console.WriteLine("A =>");
            printEtat(ret[4]);*/

            //P
            ret[5]=e;
            if (ret[5].map[e.x,e.y] == "DS")
            {
                 ret[5].map[e.x,e.y] = "S";
            }            
            if (ret[5].map[e.x,e.y] == "D")
            {
                 ret[5].map[e.x,e.y] = "E";
            }
            p6 = AspiPerformance-1;
            succ[5]=(ret[5],"P",p6);
            /*Console.WriteLine("P =>");
            printEtat(ret[5]);*/

            return succ;
        }

        public HashSet<Noeud> expand(Noeud node)
        {
            HashSet<Noeud> ret = new HashSet<Noeud>();
            (Etat,string)[] next = S(node.etat);
            int length = next.Length;

            for (int i = 0; i < length; i++)
            {
                Noeud s = new Noeud();
                s.parent = node ;
                s.action = next[i].Item2;
                s.etat = next[i].Item1;
                s.profondeur = node.profondeur +1;
                ret.Add(s);
             
            }

            return ret;
        }

        //Parcours en largeur d'un arbre modélisé par une liste
        public Noeud Tree_Search()
        {
            //Initialisation de la racine
            Noeud initial = new Noeud();
            initial.etat = ObserveEnvironmentWithAllMySensors();
            initial.parent = null;
            initial.action = null;
            initial.profondeur = 1;

            //On initialise l'arbre
            int i = 0;
            List<Noeud> arbre = new List<Noeud>();
            arbre.Add(initial);

            //Noeud que l'on visite dans le parcours en largeur
            Noeud courant = initial;

            //Set où l'on va stocker les resultats du expand
            HashSet<Noeud> new_nodes = new HashSet<Noeud>();

            //Agent test si le but est atteint
            while (!GoalTest(courant.etat.map))
            {
                //On étend l'arbre 
                new_nodes = expand(courant);
                for(int j =0; j<new_nodes.Count;j++)
                {
                    arbre.Add(new_nodes.ElementAt(j));
                }

                i += 1;
                courant = arbre.ElementAt(i);
            }
            return courant;
        }

        //Renvoie la liste des actions que doit faire l'aspirateur pour arriver à la solution
        public List<String> liste_actions(Noeud solution)
        {
            List<String> ret = new List<String>();

            Noeud courant = solution;

            while(courant.profondeur!=1)
            {
                ret.Add(courant.action);
                courant = courant.parent;
            }

            return ret;
        }


        public void startCleaning(Noeud solution)
        {
            isCleaning = true;
            List<String> sol = liste_actions(solution);
            for(int i = sol.Count-1;i>=0;i--)
            {
                string action = sol[i];
                switch(action)
                {
                    case "A":
                        Aspire();
                        break;
                    case "P":
                        PickUp();
                        break;
                    default:
                        Move(action[0]);
                        break;
                }
                Thread.Sleep(500);
            }
            isCleaning = false;
        }  
    
        public void GreedySearch()
        {
            isCleaning = true;
            int initialPerformance = AspiPerformance;
            List<int> ComparaisonPerformances = new List<int>();
            int maxIndex;
            List<String> ret = new List<String>();
            string[] actions = {"Z","Q","S","D","A","P"};

            //Initialisation du noeud courant
            Noeud courant = new Noeud();
            courant.etat = ObserveEnvironmentWithAllMySensors();
            courant.parent = null;
            courant.action = null;
            courant.profondeur = 1;

            while (!GoalTest(courant.etat.map))
            {
                (Etat,string,int)[] next = SwithPerf(courant.etat);
                foreach (var elem in next)
                {
                    ComparaisonPerformances.Add(elem.Item3);
                }

                //Recherche index de la performance maximale
                int maxValue = ComparaisonPerformances.Max();
                maxIndex = ComparaisonPerformances.ToList().IndexOf(maxValue);
                ret.Add(actions[maxIndex]);

                if (actions[maxIndex]=="A")
                {
                    Aspire();
                }
                if (actions[maxIndex]=="P")
                {
                    PickUp();
                }
                else 
                {
                    Move(char.Parse(actions[maxIndex]));
                }
                //Mise à jour de l'état
                courant.etat = ObserveEnvironmentWithAllMySensors();
            }
            isCleaning = false;
        }

    }
}
