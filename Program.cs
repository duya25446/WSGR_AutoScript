using System.Diagnostics;

namespace WSGR_AutoScript
{

    class MainApp
    {

        static async Task Main(string[] args)
        {
            string adbid = "127.0.0.1:62001";
            string RemotePath = "/data/local/tmp/";
            Stopwatch stopwatch = new Stopwatch();
            ADB adb = new ADB(adbid);
            await adb.ADBInit();
            //MiniTouch mt = new MiniTouch("127.0.0.1:5555",RemotePath,"IP");
            //await Task.Run(()=> mt.MiniTouchInit());
            for (int i = 0; i < 10; i++)
            {
                stopwatch.Restart();
                await adb.Click(100, 100);
                stopwatch.Stop();
                Console.WriteLine(stopwatch.Elapsed.ToString());
            }
            await adb.Swipe(100, 100, 1000, 500, 500);
            //Console.WriteLine("test");
            //await adb.ADBRun(devices[0], "/storage/emulated/0/WSGR_ASdata/");
        }
    }
}