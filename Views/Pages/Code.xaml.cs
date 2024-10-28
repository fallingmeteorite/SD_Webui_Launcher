﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using Awake.Models;
using Wpf.Ui.Common.Interfaces;
using Awake.Views.Windows;
using System.Windows.Controls;
using Awake.Services;
using System.Security.Policy;


namespace Awake.Views.Pages
{
    public partial class Code
    {
        public string currHash;
        public List<CommitItem> commits;
        public List<CommitItem> commits2;
        public ObservableCollection<CommitItem> CommiteCollection = new();
        public ObservableCollection<CommitItem> CommiteCollection2 = new();

        public Code()
        {

            if (initialize.启用自定义路径)
            {
                initialize.加载路径 = initialize.本地路径;
                initialize.gitPath_use = initialize.gitPath + @"\git.exe";
                if (!File.Exists(initialize.gitPath_use))
                {
                    System.Windows.MessageBox.Show("自定义GIT路径错误或未选择，程序错误即将关闭！");
                    Process.GetCurrentProcess().Kill();
                }
            }
            else
            {
                initialize.加载路径 = initialize.工作路径;
                initialize.gitPath_use = initialize.工作路径 + @"\GIT\mingw64\bin\git.exe";
                if (!File.Exists(initialize.gitPath_use))
                {
                    System.Windows.MessageBox.Show("工作路径下即整合包未存在GIT，程序错误即将关闭！");
                    Process.GetCurrentProcess().Kill();
                }

            }

            Process process3 = new Process();
            ProcessStartInfo startInfo3 = new ProcessStartInfo();
            startInfo3.FileName = initialize.gitPath_use;
            startInfo3.Arguments = " fetch  --all"; //同步云端更新日志到本地
            startInfo3.UseShellExecute = false;
            startInfo3.RedirectStandardOutput = true;
            startInfo3.RedirectStandardError = false;
            startInfo3.CreateNoWindow = true;
            startInfo3.WorkingDirectory = initialize.加载路径;

            Process process1 = new Process();
            ProcessStartInfo startInfo1 = new ProcessStartInfo();
            startInfo1.FileName = initialize.gitPath_use;
            startInfo1.Arguments = " log --oneline --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo1.UseShellExecute = false;
            startInfo1.RedirectStandardOutput = true;
            startInfo1.RedirectStandardError = false;
            startInfo1.CreateNoWindow = true;
            startInfo1.WorkingDirectory = initialize.加载路径;

            process1.StartInfo = startInfo1;
            process1.Start();
            process1.WaitForExit();

            string msg1 = process1.StandardOutput.ReadToEnd();
            currHash = msg1.Split("^^")[0];
            //Debug.WriteLine(msg);

            process1 = new Process();
            startInfo1 = new ProcessStartInfo();
            startInfo1.FileName = initialize.gitPath_use;
            startInfo1.Arguments = " remote -v";
            startInfo1.UseShellExecute = false;
            startInfo1.RedirectStandardOutput = true;
            startInfo1.RedirectStandardError = false;
            startInfo1.CreateNoWindow = true;
            startInfo1.WorkingDirectory = initialize.加载路径;

            process1.StartInfo = startInfo1;
            process1.Start();
            process1.WaitForExit();

            string msg12 = process1.StandardOutput.ReadToEnd();

            CommiteCollection.Clear();
            CommiteCollection2.Clear();
            InitializeData(30);
            InitializeComponent();

            lblCurrHash.Content = currHash;
            lblCurrDate.Content = msg1.Split("^^")[2];
            lblCurrMessage.Content = msg1.Split("^^")[1];
            lblCurrGit.Content = msg12.Split("\\n")[0].Split(" ")[0];

            process1 = new Process();
            startInfo1 = new ProcessStartInfo();
            startInfo1.FileName = initialize.gitPath_use;
            startInfo1.Arguments = " log --oneline origin/master --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo1.UseShellExecute = false;
            startInfo1.RedirectStandardOutput = true;
            startInfo1.RedirectStandardError = false;
            startInfo1.CreateNoWindow = true;
            startInfo1.WorkingDirectory = initialize.加载路径;

            process1.StartInfo = startInfo1;
            process1.Start();
            process1.WaitForExit();

            msg1 = process1.StandardOutput.ReadToEnd();
            currHash = msg1.Split("^^")[0];

            commit.ItemsSource = CommiteCollection;


            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use;
            startInfo.Arguments = " log --oneline --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg = process.StandardOutput.ReadToEnd();
            currHash = msg.Split("^^")[0];
            //Debug.WriteLine(msg);

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use;
            startInfo.Arguments = " remote -v";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg2 = process.StandardOutput.ReadToEnd();

            CommiteCollection.Clear();
            CommiteCollection2.Clear();
            InitializeData(30);
            InitializeComponent();

            lblCurrHash.Content = currHash;
            lblCurrDate.Content = msg.Split("^^")[2];
            lblCurrMessage.Content = msg.Split("^^")[1];
            lblCurrGit.Content = msg2.Split("\\n")[0].Split(" ")[0];

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use;
            startInfo.Arguments = " log --oneline origin/main --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            msg = process.StandardOutput.ReadToEnd();
            currHash = msg.Split("^^")[0];

            commit2.ItemsSource = CommiteCollection2;
        }
        private void InitializeData(int setting_show)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use;
            startInfo.Arguments = " --no-pager log origin/master --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1000";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;

            int idx = 0;
            int number_show = 0;
            commits = new List<CommitItem>();
            process.ErrorDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e)
            {

            });
            process.OutputDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e)
            {
                if (e.Data == null) return;
                CommitItem item1 = new CommitItem();
                string[] itemarr = e.Data.Split("^^");
                if (itemarr.Length < 3)
                {
                    return;
                }

                if (number_show != setting_show) 
                {
                    number_show += 1;
                    item1.Hash = itemarr[0];
                    item1.Message = itemarr[1];
                    item1.Date = itemarr[2];
                    item1.Id = idx++;
                    item1.Enable = true;
                    item1.Checked = false;

                    if (currHash == item1.Hash)
                    {
                        item1.Enable = false;
                        item1.Checked = true;
                    }

                    commits.Add(item1);
                }


            });

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.WaitForExit();

            for (int i = 0; i < commits.Count(); i++)
            {
                CommiteCollection.Add(commits[i]);
            }



            Process process1 = new Process();
            ProcessStartInfo startInfo1 = new ProcessStartInfo();
            startInfo1.FileName = initialize.gitPath_use;
            startInfo1.Arguments = " --no-pager log origin/main --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1000";
            startInfo1.UseShellExecute = false;
            startInfo1.RedirectStandardOutput = true;
            startInfo1.RedirectStandardError = true;
            startInfo1.CreateNoWindow = true;
            startInfo1.WorkingDirectory = initialize.加载路径;

            process1.StartInfo = startInfo1;

            int idx1 = 0;
            int number_show1 = 0;
            commits2 = new List<CommitItem>();
            process1.ErrorDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e)
            {

            });
            process1.OutputDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e)
            {
                if (e.Data == null) return;
                CommitItem item2 = new CommitItem();
                string[] itemarr = e.Data.Split("^^");
                if (itemarr.Length < 3)
                {
                    return;
                }

                if (number_show1 != setting_show) 
                {
                    
                    number_show1 += 1;
                    item2.Hash = itemarr[0];
                    item2.Message = itemarr[1];
                    item2.Date = itemarr[2];
                    item2.Id = idx1++;
                    item2.Enable = true;
                    item2.Checked = false;

                    if (currHash == item2.Hash)
                    {
                        item2.Enable = false;
                        item2.Checked = true;
                    }

                    commits2.Add(item2);
                }


            });

            process1.Start();
            process1.BeginErrorReadLine();
            process1.BeginOutputReadLine();
            process1.WaitForExit();

            for (int i = 0; i < commits2.Count(); i++)
            {
                CommiteCollection2.Add(commits2[i]);
            }

        }




        private void setup_Click(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;
            string hash = btn.Tag.ToString();

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use;
            startInfo.Arguments = " reset --hard"; //回退版本到
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use;
            startInfo.Arguments = " checkout " + hash; //切换到指定分支
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg = process.StandardOutput.ReadToEnd();
            Debug.WriteLine(msg);

            CommiteCollection.Clear();

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use;
            startInfo.Arguments = " log --oneline --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            msg = process.StandardOutput.ReadToEnd();
            currHash = msg.Split("^^")[0];

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use;
            startInfo.Arguments = "  remote -v";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg2 = process.StandardOutput.ReadToEnd();

            CommiteCollection.Clear();
            CommiteCollection2.Clear();
            InitializeData(30);

            lblCurrHash.Content = currHash;
            lblCurrDate.Content = msg.Split("^^")[2];
            lblCurrMessage.Content = msg.Split("^^")[1];
            lblCurrGit.Content = msg2.Split("\\n")[0].Split(" ")[0];

            commit.ItemsSource = CommiteCollection;
            System.Windows.MessageBox.Show("安装完成,请继续操作");
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CommiteCollection.Clear();

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use;
            startInfo.Arguments = " log --oneline --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg = process.StandardOutput.ReadToEnd();
            currHash = msg.Split("^^")[0];
            //Debug.WriteLine(msg);

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use;
            startInfo.Arguments = " remote -v";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg2 = process.StandardOutput.ReadToEnd();


            CommiteCollection.Clear();
            CommiteCollection2.Clear();
            InitializeData(30);
            InitializeComponent();

            lblCurrHash.Content = currHash;
            lblCurrDate.Content = msg.Split("^^")[2];
            lblCurrMessage.Content = msg.Split("^^")[1];
            lblCurrGit.Content = msg2.Split("\\n")[0].Split(" ")[0];

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use;
            startInfo.Arguments = " log --oneline origin master --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            msg = process.StandardOutput.ReadToEnd();
            currHash = msg.Split("^^")[0];

            commit.ItemsSource = CommiteCollection;
        }
        private void UpdateCode_Click(object sender, RoutedEventArgs e)
        {
            Process process2 = new Process();
            ProcessStartInfo startInfo2 = new ProcessStartInfo();
            startInfo2.FileName = initialize.gitPath_use;
            startInfo2.Arguments = " pull ";
            startInfo2.UseShellExecute = true;
            startInfo2.RedirectStandardOutput = false;
            startInfo2.CreateNoWindow = false;
            startInfo2.WorkingDirectory = initialize.加载路径;

            process2.StartInfo = startInfo2;
            process2.Start();
            process2.WaitForExit();

            btnUpdateCode.IsEnabled = false;

            CommiteCollection.Clear();
            CommiteCollection2.Clear();

            InitializeData(30);

            commit.ItemsSource = CommiteCollection;
        }

        private void DataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = e.OriginalSource as ScrollViewer;
            if (e.VerticalOffset != 0 && e.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                CommiteCollection.Clear();
                CommiteCollection2.Clear();
                InitializeData(80);
                
            }
        }

    }
    public class TagItem
    {
        public string Hash { get; set; }

        public string Message { get; set; }

        public string Date { get; set; }
    }

}
