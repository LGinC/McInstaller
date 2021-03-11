using System;
using System.Linq;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;

namespace McInstaller
{
    public partial class Form1 : Form
    {
        string baseUrl, jre32bitUrl, jre64bitUrl, mcLauncherUrl, jrePath, launcherPath;
        private void Form1_Load(object sender, EventArgs e)
        {
            baseUrl = ConfigurationManager.AppSettings["BaseUrl"]?.Trim();
            baseUrl = baseUrl?.EndsWith("/") ?? false ? baseUrl.Remove(baseUrl.Length - 2) : baseUrl;
            jre32bitUrl = FormatUrl(ConfigurationManager.AppSettings["Jre32BitUrl"]?.Trim());
            jre64bitUrl = FormatUrl(ConfigurationManager.AppSettings["Jre64BitUrl"]?.Trim());
            mcLauncherUrl = FormatUrl(ConfigurationManager.AppSettings["McLauncherUrl"]?.Trim());
            launcherPath = ConfigurationManager.AppSettings["McLauncherPath"]?.Trim();
            jrePath = ConfigurationManager.AppSettings["JrePath"]?.Trim();
            jrePath = String.IsNullOrEmpty(jrePath) ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "jre.exe") : jrePath;

            CheckUrls();
            Task.Run(CheckInstallAsync);
        }

        private async Task CheckInstallAsync()
        {
            BeginInvoke((Action)(async () =>
            {
                //安装jre
                if (CheckJreExisted())
                {
                    CB_JreInstalled.Checked = true;
                    CB_JreDownloaded.Checked = true;
                }
                else
                {
                    await DownloadJreAsync();
                    InstallJreAsync();
                }

                //下载启动器
                //1.检查下载路径配置
                if (string.IsNullOrWhiteSpace(launcherPath))
                {
                    var dialog = new FolderBrowserDialog();
                    if (dialog.ShowDialog() == DialogResult.OK)//TODO: 选择不为ok 则重复打开选择窗口
                    {
                        launcherPath = dialog.SelectedPath;
                        UpdateConfiguration("McLauncherPath", launcherPath);
                    }
                }
                //2.检查启动器是否存在
                string fileName = mcLauncherUrl.Split('/').LastOrDefault();
                var path = Path.Combine(launcherPath, fileName);
                if (!File.Exists(path))
                {
                    //下载启动器
                    await DownloadLauncherAsync(path);
                }
                CB_LauncherDownloaded.Checked = true;

                //3.配置启动器
                var ff = new FileInfo(fileName);
                string folder = Path.Combine(launcherPath, ff.Name.Replace(ff.Extension, string.Empty));
                if(!Directory.Exists(folder) || Directory.GetFiles(folder).Any(f=> f.Contains(".exe") || f.Contains(".EXE")))
                {
                    //如果文件夹不存在或者无exe文件则解压
                    ZipFile.ExtractToDirectory(path, folder);
                }

                string exePath = Path.Combine(folder, Directory.GetFiles(folder).First(f => f.Contains(".exe") || f.Contains(".EXE")));
                //桌面快捷方式
                string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "MineCraft Launcher.lnk");
                bool exist = false;
                var shell = new IWshRuntimeLibrary.WshShell();
                if (!File.Exists(shortcutPath))
                {
                    foreach (var item in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "*.lnk"))
                    {
                        var ws = (IWshRuntimeLibrary.WshShortcut)shell.CreateShortcut(item);
                        if (ws.TargetPath == exePath)
                        {
                            exist = true;
                            break;
                        }
                    }
                }

                if (!exist)
                {
                    var shortcut = (IWshRuntimeLibrary.WshShortcut)shell.CreateShortcut(shortcutPath);
                    shortcut.TargetPath = exePath;
                    shortcut.Arguments = string.Empty;
                    shortcut.Description = "MC 启动器";
                    shortcut.WorkingDirectory = folder;
                    shortcut.IconLocation = exePath;
                    shortcut.Save();
                }

                CB_LauncherConfig.Checked = true;
                //启动启动器
                var p = new Process();
                p.StartInfo.FileName = exePath;
                p.EnableRaisingEvents = false;
                p.Start();
                Application.Exit();
            }));
        }

        private async Task DownloadLauncherAsync(string path)
        {
            if (await DownloadAsync(mcLauncherUrl, path) == false)
            {
                MessageBox.Show("启动器 下载失败，请检查网络后重试");
                Application.Exit();
            }

        }

        private void UpdateConfiguration(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.AppSettings.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appsettings");
        }

        private void InstallJreAsync()
        {
            using (var p = new Process())
            {
                p.StartInfo.FileName = jrePath;
                p.EnableRaisingEvents = true;
                bool succeed = false;
                p.Exited += (s, e) =>
                {
                    succeed = (s as Process).ExitCode == 0;
                };
                p.Start();
                p.WaitForExit();
                //安装程序异常退出
                if (!succeed)
                {
                    MessageBox.Show("jre 安装失败，请稍后重试");
                    Application.Exit();
                }
            }

            CB_JreInstalled.Checked = true;
        }

        /// <summary>
        /// 下载JRE
        /// </summary>
        /// <returns></returns>
        private async Task DownloadJreAsync()
        {
            if (File.Exists(jrePath))
            {
                CB_JreDownloaded.Checked = true;
                return;
            }

            
            if(await DownloadAsync(Environment.Is64BitOperatingSystem ? jre64bitUrl : jre32bitUrl, jrePath) == false)
            {
                MessageBox.Show("JRE 下载失败，请检查网络后重试");
                Application.Exit();
            }

            CB_JreDownloaded.Checked = true;
        }

        private async Task<bool> DownloadAsync(string url, string path)
        {
            using(var client = new HttpClient())
            {
                int i = 0;
                //下载失败重复3次
                for (; i < 3; i++)
                {
                    try
                    {
                        var response = await client.GetAsync(url);
                        if (!response.IsSuccessStatusCode)
                        {
                            continue;
                        }

                        var bytes = await response.Content.ReadAsByteArrayAsync();
                        if (bytes != null && bytes.Length > 0)
                        {
                            File.WriteAllBytes(path, bytes);
                            i = 0;//下载完成重置索引
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }

                return i == 0;
            }
        }

        public Form1()
        {
            InitializeComponent();
            L_SysInfo.Text = Environment.Is64BitOperatingSystem ? "64位" : "32位";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        private void CheckUrls()
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                msg = $"BaseUrl未设置 {Environment.NewLine}";
            }

            if (!Environment.Is64BitOperatingSystem && string.IsNullOrWhiteSpace(jre32bitUrl))
            {
                msg = $"{msg} Jre32BitUrl未设置 {Environment.NewLine}";
            }

            if(Environment.Is64BitOperatingSystem && string.IsNullOrWhiteSpace(jre64bitUrl))
            {
                msg = $"{msg} Jre64BitUrl未设置{Environment.NewLine}";
            }

            if (string.IsNullOrWhiteSpace(mcLauncherUrl))
            {
                msg = $"{msg} McLauncherUrl未设置{Environment.NewLine}";
            }

            if (!string.IsNullOrWhiteSpace(msg))
            {
                MessageBox.Show(msg, "配置不完全");
                Application.Exit();
            }
        }

        private string FormatUrl(string url)
        {
            if(string.IsNullOrWhiteSpace(baseUrl) || string.IsNullOrWhiteSpace(url) || url.Contains("http://") || url.Contains("https://"))
            {
                return url;
            }

            return $"{baseUrl}{(url.StartsWith("/")?url:$"/{url}")}";
        }

        private bool CheckJreExisted()
        {
            Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "java.exe",
                    Arguments = "-version",
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            try
            {
                p.Start();
                var result = p.StandardError.ReadToEnd();
                return result.Contains("java version");
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
