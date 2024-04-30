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

    [Header("���Դ򿪵Ĵ���")]
    public int EndCount = 3;

    [Header("�Ƿ���MAC��ַ�ж�")]
    public bool isMainID = true;

    [Header("�Ƿ���ʱ���ж�")]
    public bool isEndTime = true;

    [Header("�Ƿ��������ж�")]
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

    /// ��ȡ����

    /// </summary>

    private string[] getJsonData()

    {
        string[] strs = File.ReadAllText(path).Split('|');

        if (strs.Length != 2 || strs[0] == null || strs[1] == null)

        {
            //��ʼ��

            string data = GetMacAddress() + "|0";

            File.WriteAllBytes(path, Encoding.UTF8.GetBytes(data));
        }

        return strs;
    }

    #region ��ȡMAC��ַ

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

    /// MAC��ַ

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

    #endregion ��ȡMAC��ַ

    #region ʱ��Ƚ�

    public bool ComPareGetEndTime()

    {
        string EndTime = year + "-" + month + "-" + day;

        //��ǰ��ʱ��

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

    #endregion ʱ��Ƚ�

    #region �����Ƚ�

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

    #endregion �����Ƚ�
}
