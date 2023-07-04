namespace WSGR_AutoScript
{
    class MiniTouch
    {
        string _target;
        string _RemotePath;
        string _NI;
        public async Task MiniTouchInit()
        {
            var adb = new ADB();
            if (_NI == "IP")
            {
                await adb.ADBConnect(_target);
            }
            var abi = await adb.ADBGetabi(_target);
            var sdk = await adb.ADBGetSDK(_target);
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
            await adb.ADBpush(_target, MiniTouchPath, _RemotePath);
            await adb.ManiproADB("-s " + _target + " forward tcp:1111 localabstract:minitouch");
            await adb.ADBRun(_target, _RemotePath + "minitouch");
        }
        public MiniTouch(string target, string remote, string ni)
        {
            _target = target;
            _RemotePath = remote;
            _NI = ni;
            //await MiniTouchInit(target);
        }
    }
}
