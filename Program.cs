using System.Text;

namespace WSGR_AutoScript
{

    class MainApp
    {
        
        static async Task Main(string[] args)
        {
            string RemotePath = "/data/local/tmp/";
            ADB adb = new ADB();
            var devices = await adb.ADBGetDevices();
            MiniTouch mt = new MiniTouch(devices[1],RemotePath);
            await mt.MiniTouchInit();
            
            //await adb.ADBRun(devices[0], "/storage/emulated/0/WSGR_ASdata/");
        }
    }
}