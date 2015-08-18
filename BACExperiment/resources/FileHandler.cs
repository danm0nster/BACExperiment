using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32.SafeHandles;
using WiimoteLib;
using System.Runtime.InteropServices;

namespace BACExperiment.Model
{

    /*
    
    This class is intended to handle the connection and comunication to the wii remote. 
    It is beeing built around Brian Peek's article " Managed library for Windows " which 
    can be found at the following link :https://channel9.msdn.com/coding4fun/articles/Managed-Library-for-Nintendos-Wiimote

    As stated in his article we need to get acces to the wii remote , and that is done with Win32 commands. In C# however we
    Do not have direct acces to them , for that purpose I have installed P/Invoke on this computer 

    The process to begin communication with the Wiimote is as follows:

    1. Get the GUID of the HID class defined by Windows
    2. Get a handle to the list of all devices which are part of the HID class 
    3. Enumerate through those devices and get detailed information about each 
    4. Compare the Vendor ID and Product ID of each device to the known Wiimote's VID and PID
    5. When found, create a FileStream to read/write to the device
    6. Clean up the device list

    13-08-2015 Having a bit of trouble with using the HIDImports class . I made a reference to the WiimoteLib.dll but can't 
               refference the HIDImports class.

    14-08-2015 After searching for some time , I found that the HIDImports class from the WiimoteLib is unnaccesible because it's not speccified as a public class
    Fort that reason I made a exact copy of it in the project Model and renamed it HIDImport2.
    */

    class FileHandler
    {
        //First we get the tools to conenct ready

        //This is a read/write handle to the device 
        private SafeFileHandle mHandle;

        // a .Net stream to read and write from
        private FileStream mStream;
        private bool found = false;
        private Guid guid;
        private int index = 0;

        //Wii remote comunicates via Bluetooth trough the form of reports . The length of these reports are 22.
        private const int REPORT_LENGTH = 22;
        private short VID_old = 0x057e;
        private short PID_old = 0x0306;
        private short VID_new = 0x057e;
        private short PID_new = 0x0330;


        // Definind the constructor. Everytime we make a FileHandler to get info from 1 Wii remote we go through the list of HID devices connected to the computer 
        // and we listen to the wii remote 

        public FileHandler()
        {

            Search_For_Wiimotes();

        }


        private void Search_For_Wiimotes()
        {
            // 1. Get the GUID of the HID class 
            HIDImports2.HidD_GetHidGuid(out guid);

            // 2. get a handle to all devices that are part of the HID class
            IntPtr hDevInfo = HIDImports2.SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, HIDImports2.DIGCF_DEVICEINTERFACE);// | HIDImports.DIGCF_PRESENT);

            // create a new interface data struct and initialize its size
            HIDImports2.SP_DEVICE_INTERFACE_DATA diData = new HIDImports2.SP_DEVICE_INTERFACE_DATA();
            diData.cbSize = Marshal.SizeOf(diData);

            // 3. get a device interface to a single device (enumerate all devices)
            while (HIDImports2.SetupDiEnumDeviceInterfaces(hDevInfo, IntPtr.Zero, ref guid, index, ref diData))
            {
                // create a detail struct and set its size
                HIDImports2.SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new HIDImports2.SP_DEVICE_INTERFACE_DETAIL_DATA();
                diDetail.cbSize = 5; //should be: (uint)Marshal.SizeOf(diDetail);, but that's the wrong size

                UInt32 size = 0;

                // get the buffer size for this device detail instance (returned in the size parameter)
                HIDImports2.SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, IntPtr.Zero, 0, out size, IntPtr.Zero);

                // actually get the detail struct
                if (HIDImports2.SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, ref diDetail, size, out size, IntPtr.Zero))
                {
                    // open a read/write handle to our device using the DevicePath returned
                    mHandle = HIDImports2.CreateFile(diDetail.DevicePath, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, HIDImports2.EFileAttributes.Overlapped, IntPtr.Zero);

                    // 4. create an attributes struct and initialize the size
                    HIDImports2.HIDD_ATTRIBUTES attrib = new HIDImports2.HIDD_ATTRIBUTES();
                    attrib.Size = Marshal.SizeOf(attrib);

                    // get the attributes of the current device
                    if (HIDImports2.HidD_GetAttributes(mHandle.DangerousGetHandle(), ref attrib))
                    {
                        // if the vendor and product IDs match up
                        if ((attrib.VendorID == VID_old && attrib.ProductID == PID_old) || (attrib.VendorID == VID_new && attrib.ProductID == PID_new))
                        {
                            // 5. create a nice .NET FileStream wrapping the handle above
                            mStream = new FileStream(mHandle, FileAccess.ReadWrite, REPORT_LENGTH, true);
                            Console.WriteLine("Wii remote found");
                        }
                        else
                            mHandle.Close();
                    }
                }

                // move to the next device
                index++;
            }

            // 6. clean up our list
            HIDImports2.SetupDiDestroyDeviceInfoList(hDevInfo);

        } 
       



            // report buffer
        private byte[] mBuff = new byte[REPORT_LENGTH];

        private void BeginAsyncRead()
        {
            // if the stream is valid and ready
            if (mStream.CanRead)
            {
                // create a read buffer of the report size
                byte[] buff = new byte[REPORT_LENGTH];

                // setup the read and the callback
                mStream.BeginRead(buff, 0, REPORT_LENGTH, new AsyncCallback(OnReadData), buff);
            }
        }

        private void OnReadData(IAsyncResult ar)
        {
            // grab the byte buffer
            byte[] buff = (byte[])ar.AsyncState;

            // end the current read
            mStream.EndRead(ar);

            // start reading again
            BeginAsyncRead();

            // handle data....
        }



    }
}
