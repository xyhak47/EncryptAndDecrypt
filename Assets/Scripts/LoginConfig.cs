using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine;

public class LoginConfig : MonoBehaviour
{
    [Range(2021, 2025)]
    public int year = 2021;

    [Range(1, 12)]
    public int month = 1;

    [Range(1, 31)]
    public int day = 1;

    [Header("可以打开的次数")]
    public int EndCount = 3;

    [Header("是否开启MAC地址判定")]
    public bool isMainID = true;

    [Header("是否开启时间判定")]
    public bool isEndTime = true;

    [Header("是否开启次数判定")]
    public bool isCount = true;

    private string path = Application.streamingAssetsPath + "/config.txt";

    public int rest_count;

    private void Awake()

    {
        if(!File.Exists(path))
        {
            File.Create(path);
        }

        getJsonData();

        if (isMainID)

        {
            if (!GetmainID(GetMacAddress()))

            {
                Application.Quit();
            }
        }

        if (isEndTime)

        {
            if (!ComPareGetEndTime())

            {
                Application.Quit();
            }
        }

        if (isCount)

        {
            if (!CompareCount())

            {
                Application.Quit();
            }
        }
    }

    /// <summary>

    /// 获取配置

    /// </summary>

    private string[] getJsonData()

    {
        string[] strs = File.ReadAllText(path).Split('|');

        if (strs.Length != 2 || strs[0] == null || strs[1] == null)

        {
            //初始化

            string data = GetMacAddress() + "|0";

            File.WriteAllBytes(path, Encoding.UTF8.GetBytes(data));
        }

        return strs;
    }

    #region 获取MAC地址

    private bool GetmainID(string macid)
    {
        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            string newMAC = getJsonData()[0];

            if (newMAC.Equals(GetMacAddress()) && GetMacAddress() != null)

            {
                return true;
            }
            else

            {
                return false;
            }
        }
        return true;

    }

    /// <summary>

    /// MAC地址

    /// </summary>

    /// <returns></returns>

    private static string GetMacAddress()

    {
        string physicalAddress = "";

        NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface adaper in nice)

        {
            Debug.Log(adaper.Description);

            if (adaper.Description == "en0")

            {
                physicalAddress = adaper.GetPhysicalAddress().ToString();

                break;
            }
            else

            {
                physicalAddress = adaper.GetPhysicalAddress().ToString();

                if (physicalAddress != "")

                {
                    break;
                };
            }
        }

        return physicalAddress;
    }

    #endregion 获取MAC地址

    #region 时间比较

    public bool ComPareGetEndTime()

    {
        string EndTime = year + "-" + month + "-" + day;

        //当前的时间

        DateTime nowTimeDate = DateTime.Today;

        DateTime endDataTime = Convert.ToDateTime(EndTime);

        try

        {
            if (endDataTime.CompareTo(nowTimeDate) < 0)

            {
                return false;
            }
            else

            {
                return true;
            }
        }
        catch (Exception)

        {
            throw;
        }
    }

    #endregion 时间比较

    #region 次数比较

    private bool CompareCount()

    {
        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            int index = int.Parse(getJsonData()[1]);

            int value = index + 1;

            string newData = getJsonData()[0] + "|" + value.ToString();

            File.WriteAllBytes(path, Encoding.UTF8.GetBytes(newData));

            rest_count = EndCount - index;

            if (EndCount > index)

            {
                return true;
            }
            else

            {
                return false;
            }
        }
        return true;
    }

    #endregion 次数比较
}
