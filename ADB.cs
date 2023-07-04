using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace WSGR_AutoScript
{
    class ADB ///storage/emulated/0/adbdata
    {
        string _ADBPath = "SDK_TOOL\\platform-tools-windows\\adb.exe";
        string _ADBtarget = "test";
        public ADB(string target)
        {
            if (target == null) { return; }
            _ADBtarget = target;

        }
        public async Task ADBInit()
        {
            if (IsIpPortFormat(_ADBtarget))
            {
                var devices = await ADBGetDevices();
                if (devices.Contains(_ADBtarget))
                {
                    Console.WriteLine("ADB目标已连接");
                }
                else
                {
                    await ADBConnect();
                }

            }
        }
        static bool IsIpPortFormat(string input)
        {

            string pattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}$";

            bool i = Regex.IsMatch(input, pattern);

            return i;
        }
        public async Task<String> ManiproADB(string Args)
        {
            if (_ADBtarget == "test")
            {
                Console.WriteLine("请输入正确的目标ID后重启程序");
                while (true) ;
            }
            ProcessStartInfo ADBStartInfo = new ProcessStartInfo
            {
                FileName = _ADBPath,
                Arguments = Args,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            try
            {
                using (Process ADB = new Process())
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    ADB.StartInfo = ADBStartInfo;
                    ADB.Start();
                    var output = ADB.StandardOutput.ReadToEnd();
                    var outputencoding = Console.OutputEncoding;
                    output = Encoding.UTF8.GetString(outputencoding.GetBytes(output));
                    Console.WriteLine(output);
                    await ADB.WaitForExitAsync();
                    return output;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<List<String>> ADBGetDevices()
        {
            string output;
            //string mnoutput = "List of devices attached\r\nemulator-5554   device\r\n192.168.0.101:5555   device\r\ndevice-1234    device";
            ProcessStartInfo ADBStartInfo = new ProcessStartInfo
            {
                FileName = _ADBPath,
                Arguments = "devices",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            try
            {
                using (Process ADB = new Process())
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    ADB.StartInfo = ADBStartInfo;
                    ADB.Start();
                    output = ADB.StandardOutput.ReadToEnd();
                    var outputencoding = Console.OutputEncoding;
                    output = Encoding.UTF8.GetString(outputencoding.GetBytes(output));
                    Console.WriteLine(output);
                    await ADB.WaitForExitAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            List<String> devices = output.Split("\r\n").Skip(1).Select(i => i.Split()[0]).ToList();
            return devices;
        }

        public async Task ADBConnect()
        {
            string args = "connect " + _ADBtarget;
            string output = await ManiproADB(args);
        }
        public async Task ADBpush(string local, string remote)
        {
            string args = "-s " + _ADBtarget + " push " + local + " " + remote;
            string output = await ManiproADB(args);
        }
        public async Task<string> ADBGetabi()
        {
            string args = "-s " + _ADBtarget + " shell getprop ro.product.cpu.abi";
            string output = await ManiproADB(args);
            return output.Replace("\r\n", "");
        }
        public async Task<int> ADBGetSDK()
        {
            string args = "-s " + _ADBtarget + " shell getprop ro.build.version.sdk";
            string output = await ManiproADB(args);
            output = output.Replace("\r\n", "");
            try
            {
                return int.Parse(output);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }
        }

        public async Task ADBRun(string path)
        {
            string args = "-s " + _ADBtarget + " shell " + "chmod 777 " + path;
            string output = await ManiproADB(args);
            args = "-s " + _ADBtarget + " shell " + path;
            ProcessStartInfo ADBStartInfo = new ProcessStartInfo
            {
                FileName = _ADBPath,
                Arguments = args,
                RedirectStandardOutput = false,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            try
            {
                using (Process ADB = new Process())
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    ADB.StartInfo = ADBStartInfo;
                    ADB.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            //output = await ManiproADB(args);
        }
        public async Task ADBKill(string ID)
        {
            string args = "-s " + _ADBtarget + " shell " + "kill " + ID;
            string output = await ManiproADB(args);
        }
        public async Task Click(int x, int y)
        {
            string args = "-s " + _ADBtarget + " shell " + "input tap " + x.ToString() + " " + y.ToString();
            string output = await ManiproADB(args);
        }
        public async Task Swipe(int x1, int y1, int x2, int y2, int time)
        {
            string args = "-s " + _ADBtarget + " shell " + "input swipe " + x1.ToString() + " " + y1.ToString() + " " + x2.ToString() + " " + y2.ToString() + " " + time.ToString();
            string output = await ManiproADB(args);
        }
    }
}
