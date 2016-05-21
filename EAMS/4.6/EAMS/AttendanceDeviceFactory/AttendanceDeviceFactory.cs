using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAttendanceDevice;

namespace AttendanceDeviceFactory
{
    public static class AttendanceDeviceFactory
    {
        public static string DevName { get; private set; }
        private static Assembly _asse;
        public static IAttendanceDevice.IAttendanceDevice Create(string path = null, string devName = "HWATT")
        {
            DevName = devName;
            string assePath = string.IsNullOrEmpty(path) ? AppDomain.CurrentDomain.BaseDirectory : path;
            IAttendanceDevice.IAttendanceDevice device;
            _asse = getAssembly(assePath.EndsWith("\\") || assePath.EndsWith("/") ? assePath : assePath + "\\");
            if (_asse != null)
            {
                string clsName = "AttendanceDevice." + DevName.ToUpper() + ".Device";
                device = (IAttendanceDevice.IAttendanceDevice)_asse.CreateInstance(clsName);
                device.cardEmployee = createEmployee();
                device.cardTime = createCardTime();
                device.cardBll = createIBLL();
                return device;
            }
            else return null;
        }
        private static Assembly getAssembly(string path)
        {
            string file = "AttendanceDevice." + DevName + ".dll";
            if (!string.IsNullOrEmpty(path))
            {
                string [] files=System.IO.Directory.GetFiles(path, file, System.IO.SearchOption.AllDirectories);
                if (files != null && System.IO.File.Exists(files[0]))
                    _asse = Assembly.LoadFile(files[0]);
                else _asse = null;
            }
            else _asse = null;
            //是否要做单例？
            return _asse;
        }
        public static ICardTime createCardTime()
        {
            string clsName = "AttendanceDevice." + DevName.ToUpper() + "." + "CardTime";
            return (ICardTime)_asse.CreateInstance(clsName);
        }
        public static IEmployee createEmployee()
        {
            string clsName = "AttendanceDevice." + DevName.ToUpper() + "." + "Employee";
            return (IEmployee)_asse.CreateInstance(clsName);
        }
        public static IBLL createIBLL()
        {
            string clsName = "AttendanceDevice." + DevName.ToUpper() + "." + "BLL";
            return (IBLL)_asse.CreateInstance(clsName);
        }
    }
}
