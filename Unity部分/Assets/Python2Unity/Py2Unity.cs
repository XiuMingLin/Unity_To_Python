using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Py2Unity
{
    private static Py2Unity _instance;
    
    public static Py2Unity Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Py2Unity();
            }
            return _instance;
        }
    }

    private Socket socket;

    private Py2Unity()
    {
        if (!PlayerPrefs.HasKey("pythonexe") || !PlayerPrefs.HasKey("pythonfile"))
        {
            Debug.LogError("python path has been not setted!");
            return;
        }
        StartPython();
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect("127.0.0.1", 7758);
    }

    private void StartPython()
    {
        System.Diagnostics.Process compiler = new System.Diagnostics.Process()
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = PlayerPrefs.GetString("pythonexe"),
                Arguments = PlayerPrefs.GetString("pythonfile"),
                UseShellExecute = true,
                CreateNoWindow = true,
            }
        };

        compiler.Start();
    }

    public void SendToPython(string data)
    {
        socket.Send(Encoding.UTF8.GetBytes(data));
    }

    public string RecFromPython()
    {
        byte[] buffer = new byte[1024];
        socket.Receive(buffer);
        return Encoding.UTF8.GetString(buffer);
    }

    public void Dispose()
    {
        SendToPython("exit");
        socket.Close();
    }
}
