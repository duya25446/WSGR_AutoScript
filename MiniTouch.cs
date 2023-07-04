using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSGR_AutoScript
{
    class MiniTouch
    {
        string _target;
        string _RemotePath;
        public async Task MiniTouchInit()
        {
            var adb = new ADB();
            var abi =await adb.ADBGetabi(_target);
            var sdk =await adb.ADBGetSDK(_target);
            string MiniTouchPath;
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
            adb.ADBRun(_target, _RemotePath+"minitouch");
        }
        public MiniTouch(string target,string remote) 
        {
            _target = target;
            _RemotePath = remote;
            //await MiniTouchInit(target);
        }
    }
}
