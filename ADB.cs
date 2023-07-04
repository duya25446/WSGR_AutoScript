using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WSGR_AutoScript
{
    class ADB ///storage/emulated/0/adbdata
    {
        string _ADBPath = "SDK_TOOL\\platform-tools-windows\\adb.exe";
      
        public async Task<String> ManiproADB(string Args)
        {
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
            string mnoutput = "List of devices attached\r\nemulator-5554   device\r\n192.168.0.101:5555   device\r\ndevice-1234    device";
            var output = await ManiproADB("devices");
            List<String> devices = output.Split("\r\n").Skip(1).Select(i => i.Split()[0]).ToList();
            return devices;
        }

        public async Task ADBConnect(string target)
        {
            string args = "connect " + target;
            string output = await ManiproADB(args);
        }
        public async Task ADBpush(string target,string local,string remote)
        {
            string args ="-s "+target+ " push "+local+" "+remote;
            string output = await ManiproADB(args);
        }
        public async Task<string> ADBGetabi(string target)
        {
            string args = "-s " + target + " shell getprop ro.product.cpu.abi";
            string output = await ManiproADB(args);
            return output.Replace("\r\n","");
        }
        public async Task<int> ADBGetSDK(string target)
        {
            string args = "-s " + target + " shell getprop ro.build.version.sdk";
            string output = await ManiproADB(args);
            output = output.Replace("\r\n", "");
            return int.Parse(output);
        }
        public async Task ADBRun(string target,string path)
        {
            string args = "-s " + target + " shell " + "chmod 777 " + path;
            string output = await ManiproADB(args);
            args = "-s " + target + " shell " + path;
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
                    await ADB.WaitForExitAsync();
                    Console.WriteLine("minitouch out");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            //output = await ManiproADB(args);
        }
    }
}
