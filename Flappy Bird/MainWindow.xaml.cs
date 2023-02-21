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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Flappy_Bird
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>    
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        double score;
        int gravity = 0;
        bool gameOver;
        Rect BirdRectHitBox;
        double BestScore = 0;
        double speed = 20;


        public MainWindow()
        {
            InitializeComponent();
            StartMessageBox();

            gameTimer.Interval = TimeSpan.FromMilliseconds(speed);
            gameTimer.Tick += MainGameStarter;
            
            txtScoreOver.Content = " ";
            txtGameStart.Content = "START !";
            
        }

        private void MainGameStarter(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;
            
            BirdRectHitBox = new Rect(Canvas.GetLeft(FlappyBird), Canvas.GetTop(FlappyBird), FlappyBird.Width, FlappyBird.Height);

            Canvas.SetTop(FlappyBird, Canvas.GetTop(FlappyBird) + gravity);

            if (Canvas.GetTop(FlappyBird) < -10 || Canvas.GetTop(FlappyBird) > 458)
            {
                EndGame();
            }

            foreach (var x in Pole.Children.OfType<Image>())
            {
                if ((string)x.Tag == "obs1" || (string)x.Tag == "obs2" || (string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);
                    

                    if (Canvas.GetLeft(x) < -100)
                    {
                        Canvas.SetLeft(x, 800);
                        score += .5;

                    }
                    Rect pipeRectHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (BirdRectHitBox.IntersectsWith(pipeRectHitBox))
                    {
                        EndGame();
                    }

                }
                if ((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 2);

                    if (Canvas.GetLeft(x) < -250)
                    {
                        Canvas.SetLeft(x, 550);
                    }
                }
            }
        }
        private void StartMessageBox()
        {
            MessageBoxResult result = MessageBox.Show("Zdravím Vás ve hře Flappy bird. Při zakliknutí Yes: standartní rychlost, při No: extreme rychlost", "Start", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    StartGame();

                    break;
                case MessageBoxResult.No:
                    speed -= 10;
                    StartGame();

                    break;
            }
        }

        
        private void KeyNotPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                FlappyBird.RenderTransform = new RotateTransform(-15, FlappyBird.Width / 2, FlappyBird.Height / 2);
                txtGameStart.Content = " ";
                gravity = -8;

            }
            if (e.Key == Key.R && gameOver == true)
            {
                
                StartGame();
                txtGameStart.Content = "START !";
                Pole.Background = new SolidColorBrush(Colors.LightBlue);

            }
            if (e.Key == Key.L && gameOver == true)
            {

                App.Current.Shutdown();

            }

        }

        private void KeyPress(object sender, KeyEventArgs e)
        {
            FlappyBird.RenderTransform = new RotateTransform(5, FlappyBird.Width / 2, FlappyBird.Height / 2);
            gravity = 8;

        }

        private void StartGame()
        {

            Pole.Focus();
            int temp = 300;
            FlappyBird.IsEnabled = true;
            score = 0;
            txtGameOver.Content = " ";
            gameOver = false;
            Canvas.SetTop(FlappyBird, 190);
            txtScoreOver.Content = "";

            foreach (var x  in Pole.Children.OfType<Image>())
            {
                if ((string)x.Tag == "obs1")
                {
                    Canvas.SetLeft(x, 500);
                }
                if ((string)x.Tag == "obs2")
                {
                    Canvas.SetLeft(x, 800);
                }
                if ((string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, 1100);
                }
                if ((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, 300 + temp);
                    temp = 800;
                }

                gameTimer.Start();
            }

        }

        
        private void EndGame()
        {
            gameTimer.Stop();
            gameOver = true;
            txtGameStart.Content = " ";
            FlappyBird.IsEnabled = false;
            
            if(score > BestScore)
            {
                BestScore += score;
            }
            txtScoreOver.Content += "Prohrál jsi, klikni 'R' pro restart nebo 'L' pro ukončení" + " nejlepší dosažené skore: " + BestScore ;
            txtGameOver.Content += "GAME OVER";
            txtGameOver.IsEnabled = true;
            Pole.Background = new SolidColorBrush(Colors.Red);

        }

       

        

        
    }
}
