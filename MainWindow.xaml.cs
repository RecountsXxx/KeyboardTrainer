using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;


namespace KeyboardTrainer
{


    internal sealed partial class MainWindow : Window
    {
     
        private ColorDialog colorDialog = new ColorDialog();
        private Brush brush = null;
        private List<Border> border = new List<Border>();
        private StackPanel stackPanel = null;
        private int FailsCount;
        private DateTime startTime;
        private bool CapslockBool;
        public MainWindow()
        {
            InitializeComponent();
            //капслок
            TextPreview.Visibility = Visibility.Hidden;
            GenerateText.Visibility = Visibility.Hidden;
            if (Console.CapsLock == true)
            {
                stackPanel = HighStackPanel;
                stackPanel.Visibility = Visibility.Visible;
                LowerStackPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                stackPanel = LowerStackPanel;
                stackPanel.Visibility = Visibility.Visible;
                HighStackPanel.Visibility = Visibility.Hidden;
            }
            //это замена цвета кнопок пользователь может поставтить сам цвет кнопок
            
            
        }
    //start
    private void Button_Click(object sender, RoutedEventArgs e)
        {
            //ставлю всё по нулям 
            FailsCount = 0;
            Fails.Text = "0";
            SpeedText.Text = "0";
            startTime = DateTime.Now;
            TextPreview.Text = "";
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            TextPreview.Visibility = Visibility.Visible;
            GenerateText.Visibility = Visibility.Visible;
            CaseSensetive.IsEnabled = false;
            SpecialChars.IsEnabled = false;
            Difficult.IsEnabled = false;
            GenerateText.Text = "";

            //генерация слова
            int num_letters = new Random().Next(4,8);
            int num_words = 7;
            char[] letters;
            if (SpecialChars.IsChecked == true)
            {
                letters = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()_+".ToCharArray();
            }
            else
            {
                letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            }

            Random rand = new Random();
            for (int i = 1; i <= num_words; i++)
            {
                string word = "";
                for (int j = 1; j <= num_letters; j++)
                {

                    int letter_num;
                    if(SpecialChars.IsChecked == true)
                    {
                        letter_num = rand.Next(0, letters.Length);
                    }
                    else
                    {
                        letter_num = rand.Next(0, (int)Difficult.Value);
                    }
                    word += letters[letter_num];
                }
               
                GenerateText.Text += word + " ";
              
            }
            GenerateText.Text = GenerateText.Text.Remove(GenerateText.Text.Length - 1);


        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            //меняю 
            CaseSensetive.IsEnabled = true;
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            SpecialChars.IsEnabled = true;
            TextPreview.Visibility = Visibility.Hidden;
            GenerateText.Visibility = Visibility.Hidden;
            Difficult.IsEnabled = true;
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int index = TextPreview.Text.Length - 1;
            //если нажат CapsLock
            if (e.KeyboardDevice.IsKeyToggled(Key.CapsLock) || CapslockBool == true)
            {
                stackPanel = HighStackPanel;
                stackPanel.Visibility = Visibility.Visible;
                LowerStackPanel.Visibility = Visibility.Hidden;

            }
            else
            {
                stackPanel = LowerStackPanel;
                stackPanel.Visibility = Visibility.Visible;
                HighStackPanel.Visibility = Visibility.Hidden;
            }


            foreach (FrameworkElement fe1 in stackPanel.Children)
            {
                if (fe1 is StackPanel)
                {
                    StackPanel st = (StackPanel)fe1;

                    foreach (FrameworkElement fe2 in st.Children)
                    {
                        if (fe2 is Grid)
                        {
                            Grid br = (Grid)fe2;
                            foreach (FrameworkElement fe3 in br.Children)
                            {
                                if (fe3 is Border)
                                {
                                    Border brq = (Border)fe3;
                                    //делаю цвет клавишам
                                    brq.Background = Brushes.LightGreen;
                                   
                                    FrameworkElement el = (FrameworkElement)brq.Child;
                                    if (el is TextBlock)
                                    {
                                        TextBlock tb = (TextBlock)el;
                                        //меняю цвет системных клавиш
                                        if (tb.Text == "Tab" | tb.Text == "Caps Lock" | tb.Text == "Shift" | tb.Text == "Ctrl" | tb.Text == "Win" | tb.Text == "Alt" | tb.Text == "Space" | tb.Text == "Enter" | tb.Text == "Backspace")
                                        {
                                            brq.Background = Brushes.Coral;
                                        }

                                        //зажат ли шифт
                                        if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                                        {
                                            Window_KeyUp(sender, e);
                                        }

                                        //у меня символ береться с имени textbloca там 2 языка а название нельзя одинаковые вот и написал Two
                                        if (tb.Name.Contains("Two"))
                                        {
                                            tb.Name = tb.Name.Remove(tb.Name.IndexOf("Two"));
                                        }

                                        //проверяю будет маленькие буквы или больше
                                        if (tb.Text == e.Key.ToString() || tb.Name == e.Key.ToString() || tb.Text == e.Key.ToString().ToLower())
                                        {
                                            //цвета
                                         
                                            //выкидываю эти клавиши от ввода в текстблок
                                            if (tb.Text == "Tab" | tb.Text == "Caps Lock" | tb.Text == "Shift" | tb.Text == "Ctrl" | tb.Text == "Win" | tb.Text == "Alt" | tb.Text == "Space" | tb.Text == "Enter" | tb.Text == "Backspace")
                                            {

                                            }
                                            else
                                            {
                                                //если нажата кнопка старт
                                                if (StartButton.IsEnabled == true)
                                                {
                                                    TextPreview.Text = "";
                                                    TextPreview.Visibility = Visibility.Hidden;
                                                    SpeedText.Text = "0";
                                                    Fails.Text = "0";
                                                }
                                                else
                                                {
                                                    TextPreview.Text += tb.Text;
                                                }
                                            }
                                            //если пробел то ставим пробел
                                            if (e.Key == Key.Space)
                                            {
                                                TextPreview.AppendText(" ");
                                            }
                                            //если стереть то стираем 1 символ
                                            if (e.Key == Key.Back)
                                            {
                                                if (TextPreview.Text.Length > 0)
                                                {
                                                    TextPreview.Text = TextPreview.Text.Remove(TextPreview.Text.Length - 1);
                                                    if (index >= TextPreview.Text.Length)
                                                    {
                                                        index = TextPreview.Text.Length;
                                                        TextPreview.Foreground = new SolidColorBrush(Colors.Black);
                                                    }
                                                    TextPreview.Select(0, index);
                                                }
                                                return;
                                            }
                                            //проверяем и ставим ей другой цвет
                                     
                                            if (!e.IsRepeat)
                                            {
                                                brush = brq.Background;
                                                border.Add(brq);
                                                brq.Background = Brushes.Red;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

        }
        private void SpecialCharsColor(object sender, RoutedEventArgs e)
        {
           
        }
        private void AlphabetCharsColor(object sender, RoutedEventArgs e)
        {
           
        }
        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!e.IsRepeat)
            {
               if(border != null)
                {

                    //меняю цвет на прошлый
                        border[border.Count - 1].Background = brush;
  
                    //это всё относиться к повышению регистра у Shifta
                    if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                    {
                        if (!e.IsRepeat)
                        {
                            stackPanel = HighStackPanel;
                            stackPanel.Visibility = Visibility.Visible;
                            LowerStackPanel.Visibility = Visibility.Hidden;
                            CapslockBool = true;
                        }
                        if (e.IsUp)
                        {
                            stackPanel = LowerStackPanel;
                            stackPanel.Visibility = Visibility.Visible;
                            HighStackPanel.Visibility = Visibility.Hidden;
                            CapslockBool = false;

                        }
                    }   
                }                        
            }
        }

        private void Difficult_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //беру Value с ползунка
            DifficultTextBlock.Text = Math.Round(e.NewValue, 0).ToString();
        }
        private void TextPreview_TextChanged(object sender, TextChangedEventArgs e)
        {
            string generateText;
            string textPreview;
            //если включен case sensetive
            if (CaseSensetive.IsChecked == true)
            {
                generateText = GenerateText.Text;
                textPreview = TextPreview.Text;
            }
            else
            {
                generateText = GenerateText.Text.ToLower();
                textPreview = TextPreview.Text.ToLower();
    
            }
            int index = TextPreview.Text.Length - 1;

            //try для того что бь не было проблем с индексами
            try
            {
                if (generateText[index] == textPreview[index])
                    index++;

                if (textPreview.Length <= generateText.Length)
                    if (generateText[textPreview.Length - 1] != textPreview[TextPreview.Text.Length - 1])
                    {
                        FailsCount++;
                        Fails.Text = FailsCount.ToString();
                    }


                if (index == TextPreview.Text.Length)
                    TextPreview.Foreground = new SolidColorBrush(Colors.Black); 
                else
                    TextPreview.Foreground = new SolidColorBrush(Colors.Red);

                SpeedText.Text = FailsCount.ToString();
                SpeedText.Text = Math.Round(index / (DateTime.Now - startTime).TotalMinutes).ToString();
                TextPreview.Select(0, index); 

                if (TextPreview.Text.Length == GenerateText.Text.Length)
                {
                    MessageBox.Show($"You have finished typing.\nYour statics:\nFails - {FailsCount}\nSpeed typing - {SpeedText.Text} chars/min","Victory!", (MessageBoxButtons)MessageBoxButton.OK);
                    StopButton.IsEnabled = false;
                    StartButton.IsEnabled = true;
                    TextPreview.Visibility = Visibility.Hidden;
                    GenerateText.Visibility = Visibility.Hidden;
                }
            }
            catch
            {

            }         
        }
    }
}