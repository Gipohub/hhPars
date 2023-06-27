﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsAPICodePack;
using WindowsAPICodePack.Dialogs;
using WpfApp1Tech;

namespace WpfApp1Tech
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[]? DateOfList;
        Settings settings = new();
        public MainWindow()
        {
            InitializeComponent();
            ListBoxGenerate();



        }
        void ListBoxGenerate()
        {
           // List<TodoItem> items = new List<TodoItem>();
           // items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Completion = 45 });
           // items.Add(new TodoItem() { Title = "Learn C#", Completion = 80 });
           // items.Add(new TodoItem() { Title = "Wash the car", Completion = 0 });
           // lbTodoList.ItemsSource = items;
        }
        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings settWindow = new()
            {
                Owner = this
            };
            settWindow.Show();
        }
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            //((Expander)sender).Content = new Button() { Width = 80, Height = 30, Content = "Привет" };
            StackPanel expanderPanel = new StackPanel();
            expanderPanel.Children.Add(new CheckBox { Content = "WinForms" });
            expanderPanel.Children.Add(new CheckBox { Content = "WPF" });
            expanderPanel.Children.Add(new CheckBox { Content = "ASP.NET" });

            Expander expander = new Expander();

            expander.Content = expanderPanel;
            ((Expander)sender).Header = "Выберите технологию";
            ((Expander)sender).Content = expanderPanel;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Экспандер свернут");
            ((Expander)sender).Content = new Button() { Width = 80, Height = 30, Content = "Привет" };

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
       

        public class DateItem
        {
            public string Title { get; set; }
            public string Search { get; set; }

            public int CountOfFiles { get; set; }
            public DateItem(string title, string search, int countOfFiles)
            {
                Title = title;
                Search = search;
                CountOfFiles = countOfFiles;
            }
        }

        private void ButtonHoldList_Click(object sender, RoutedEventArgs e)
        {
            if (ParsDateList.IsEnabled is true)
            {
                ParsDateList.IsEnabled = false;
                ButtonHoldList.Content = "расхолдить";
                StartButton.IsEnabled = true;
                ButtonHoldList.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFE4FCDF");/*#3CACDC*/
                //ParsDateList.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFE4FCDF");/*#3CACDC*/
                //var l = ParsDateList.SelectedItems;
                //dynamic[] Booth = ParsDateList.SelectedItem as dynamic;
               // List<DateItem> items = new List<DateItem>();
                foreach (DateItem item in ParsDateList.SelectedItems)
                {
                    MessageBox.Show(item.Title + "");
                    
                    //items.Add(new DateItem(item.Title,item.Search,1));
                }
                //ListTechResult.ItemsSource = items;
                //ShortTextResult.Text.Add() = Booth.Search;

                //ShortTextResult.Text = l.GetEnumerator().ToString();


            }
            else
            {
                ParsDateList.IsEnabled = true;
                ButtonHoldList.Content = "захолдить";
                StartButton.IsEnabled = false;
                ButtonHoldList.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFDDDDDD");/*#3CACDC*/
                //ParsDateList.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");/*#3CACDC*/


            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Lemmizator1.Lemmization(DateOfList);
            StartButton.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFE4FCDF");/*#3CACDC*/
            var ip = new Interpreter();
            ip.DicReview(DateOfList[0]);
            TopTechResult.ItemsSource = ip.VecTech;
            TopNoTechResult.ItemsSource = ip.VecNoTech;
        }

        private void SearchBox_Initialized(object sender, EventArgs e)
        {
            string[] allfolders = Directory.GetDirectories(settings.ParsFolder);
            //IEnumerable<string> allfiles = Directory.EnumerateFiles(Settings.ssDef.ParsFolder);
            int length = settings.ParsFolder.Length + 1;
            for (int i = 0; i < allfolders.Length; i++)
                {
                allfolders[i] = allfolders[i].Remove(0,length);
                }
            //allfolders =  allfolders.TrimPath();
            SearchBox.ItemsSource = allfolders;
            
        }

        private void SearchBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            /*string parsdate = System.IO.Path.Combine(Settings.ssDef.ParsFolder, (string)sender);
            string[] allfolders = Directory.GetDirectories(parsdate);
            if (allfolders.Length == 0)
            {
                ParsDateList.ItemsSource = "Подобных поисков не было";
            }
            else
            {
                List<DateItem> items = new List<DateItem>();
                for (int i = 0; i < allfolders.Length; i++)
                {
                    items.Add(new DateItem() { Title = allfolders[i], CountOfFiles = i });
                }
                 
                
                //items.Add(new DateItem() { Title = "Complete this WPF tutorial", CountOfFiles = 45 });
                //items.Add(new DateItem() { Title = "Learn C#", CountOfFiles = 80 });
                //items.Add(new DateItem() { Title = "Wash the car", CountOfFiles = 0 });

                ParsDateList.ItemsSource = items;
            }*/
        }
        private void SearchBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            
        }
        private void SearchBox_Drop(object sender, DragEventArgs e)
        {
            
        }
        private void SearchBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                
                if (SearchBox.SelectedValue != null)
                {
                    SearchBox.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#3CACDC");/*#FFE4FCDF*/
                    string? kek = SearchBox.SelectedValue.ToString();
                    string parsdate = System.IO.Path.Combine(settings.ParsFolder, kek);
                    string[] shortFolders = Directory.GetDirectories(parsdate);
                    //  string[] allFolders = shortFolders;
                    DateOfList = shortFolders;
                    int length = settings.ParsFolder.Length + 1;
                    for (int i = 0; i < shortFolders.Length; i++)
                    {
                    //  shortFolders[i].Remove(0, length);
                    }
                    if (shortFolders.Length == 0)
                    {
                        ParsDateList.ItemsSource = "Подобных поисков не было";
                    }
                    else
                    {
                        List<DateItem> items = new List<DateItem>();
                        for (int i = 0; i < shortFolders.Length; i++)
                        {
                            items.Add(new DateItem(shortFolders[i].Remove(0, length), kek, i));
                        }


                        //items.Add(new DateItem() { Title = "Complete this WPF tutorial", CountOfFiles = 45 });
                        //items.Add(new DateItem() { Title = "Learn C#", CountOfFiles = 80 });
                        //items.Add(new DateItem() { Title = "Wash the car", CountOfFiles = 0 });

                        ParsDateList.ItemsSource = items;
                     
                    }
                    ButtonHoldList.IsEnabled = true;
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Lemmizator lem = new()
            {
                Owner = this
            };
            lem.ShowDialog();
            Lemmizator.Lemmization(settings.ParsFolder);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string[] Alls = Directory.GetDirectories(settings.ParsFolder);
            Lemm2.Lemmization(Alls);
        }
       
    }
}
