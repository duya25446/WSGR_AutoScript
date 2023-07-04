namespace WSGR_AutoScript
{
    class MiniTouch
    {
        string _target;
        string _RemotePath;
        public async Task MiniTouchInit()
        {
            var adb = new ADB(_target);
            var abi = await adb.ADBGetabi();
            var sdk = await adb.ADBGetSDK();
            string MiniTouchPath;
            if (sdk == 0)
            {
                Console.WriteLine("没有找到设备，请检查设备链接");
                return;
            }
            if (sdk >= 16)
            {
                MiniTouchPath = "minitouch-prebuilt\\prebuilt\\" + abi + "\\bin\\minitouch";
            }
            else
            {
                MiniTouchPath = "minitouch-prebuilt\\prebuilt\\" + abi + "\\bin\\minitouch-nopie";
            }
            await adb.ADBpush(MiniTouchPath, _RemotePath);
            await adb.ManiproADB("-s " + _target + " forward tcp:1111 localabstract:minitouch");
            await adb.ADBRun(_RemotePath + "minitouch");
        }
        public MiniTouch(string target, string remote, string ni)
        {
            _target = target;
            _RemotePath = remote;
        }

    }
}
