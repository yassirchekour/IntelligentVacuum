using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace IA_TP1
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Image[,] dustMap = new Image[5, 5];
        private Image[,] diamondMap = new Image[5, 5];
        public Environment villa;
        private Aspi aspi;
        private Thread villaThread;
        private Thread aspiThread;
        private TextBlock perAspiText;        
        public MainWindow()
        {
            InitializeComponent();
            perAspiText = this.FindName("perAspText") as TextBlock ;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    dustMap[i, j] = this.FindName("dust" + i.ToString() + j) as Image;
                    dustMap[i, j].Visibility = Visibility.Hidden;
                    diamondMap[i, j] = this.FindName("diamond" + i.ToString() + j) as Image;
                    diamondMap[i, j].Visibility = Visibility.Hidden;
                }
            }
            villa = new Environment(this);
            aspi = new Aspi(this,2,2);

            villaThread = new Thread(new ThreadStart(villa.Start));
            aspiThread = new Thread(new ThreadStart(aspi.Start));

            UpdateAspiratorPosition(2, 2);
            addDust(3, 3);
            addDiamond(4, 4);

        }

        public void UpdateAspiratorPosition(int x, int y)
        {
            if (x < 0 || y < 0 || x >= 5 || y >= 5) throw new Exception("Les coordonnées de l'aspirateur ne sont pas valides");

            Image aspirator = this.FindName("Aspirator") as Image;
            Thickness margin = aspirator.Margin;
            margin.Left = 25 + x * 100;
            margin.Top = 25 + y * 100;
            aspirator.Margin = margin;

        }

        public void addDust(int x, int y)
        {
            dustMap[x, y].Visibility = Visibility.Visible;
        }
        public void removeDust(int x, int y)
        {
            dustMap[x, y].Visibility = Visibility.Hidden;
        }
        public void addDiamond(int x, int y)
        {
            diamondMap[x, y].Visibility = Visibility.Visible;
        }
        public void removeDiamond(int x, int y)
        {
            diamondMap[x, y].Visibility = Visibility.Hidden;
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            
            villaThread.Start();
            aspiThread.Start();

            button.Visibility = Visibility.Hidden;
        }

        public void RefreshEnv()
        {
            perEnvText.Text = villa.CalculPerformance().ToString();
            string[,] villaState = villa.villaMap;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    switch (villaState[i, j])
                    {
                        case "E":
                            dustMap[i, j].Visibility = Visibility.Hidden;
                            diamondMap[i, j].Visibility = Visibility.Hidden;
                            break;
                        case "S":
                            dustMap[i, j].Visibility = Visibility.Visible;
                            diamondMap[i, j].Visibility = Visibility.Hidden;
                            break;
                        case "D":
                            dustMap[i, j].Visibility = Visibility.Hidden;
                            diamondMap[i, j].Visibility = Visibility.Visible;
                            break;
                        case "DS":
                            dustMap[i, j].Visibility = Visibility.Visible;
                            diamondMap[i, j].Visibility = Visibility.Visible;
                            break;

                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            villaThread.Abort();
            aspiThread.Abort();

            System.Windows.Forms.Application.Exit();
        }
    }
}
