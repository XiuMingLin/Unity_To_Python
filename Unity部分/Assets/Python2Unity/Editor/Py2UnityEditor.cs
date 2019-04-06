using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class Py2UnityEditor : EditorWindow
{
    private string pythonexePath = "select python.exe path!";
    private string pythonFilePath = "select python file path!";
    private string unityExample = "";
    private string pythonExample = "";

    [MenuItem("Py2Unity/Py")]
    static void Init()
    {
        var window = GetWindow<Py2UnityEditor>();
        window.ShowUtility();
        var position = window.position;
        position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
        window.position = position;
        window.unityExample = Resources.Load<TextAsset>("UnityExample").text;
        window.pythonExample = Resources.Load<TextAsset>("PythonExample").text;
    }

    private void OnGUI()
    {
        //GUI.BeginScrollView(new Rect(5, 5, 600, 450), Vector2.one, new Rect(5, 5, 300, 100));
        GUI.Label(new Rect(5, 5, 100, 20), "Python.exe path:");
        GUI.TextField(new Rect(115, 5, 400, 20), pythonexePath);
        if (GUI.Button(new Rect(523, 5, 80, 20), "..."))
        {
            OpenFileName ofn = new OpenFileName();

            ofn.structSize = System.Runtime.InteropServices.Marshal.SizeOf(ofn);
            ofn.filter = "EXE文件(*.exe)\0*.exe";
            ofn.file = new string(new char[256]);
            ofn.maxFile = ofn.file.Length;
            ofn.fileTitle = new string(new char[64]);
            ofn.maxFileTitle = ofn.fileTitle.Length;
            ofn.title = "Select Python.exe";
            ofn.defExt = "EXE";
            ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

            if (WindowDll.GetOpenFileName(ofn))
            {
                pythonexePath = ofn.file;
                PlayerPrefs.SetString("pythonexe", ofn.file);
                Debug.Log(ofn.file);
            }
        }
        GUI.Label(new Rect(5, 30, 140, 20), "Excute python file path:");
        GUI.TextField(new Rect(155, 30, 360, 20), pythonFilePath);
        if (GUI.Button(new Rect(523, 30, 80, 20), "..."))
        {
            OpenFileName ofn = new OpenFileName();

            ofn.structSize = System.Runtime.InteropServices.Marshal.SizeOf(ofn);
            ofn.filter = "Python文件(*.py)\0*.py";
            ofn.file = new string(new char[256]);
            ofn.maxFile = ofn.file.Length;
            ofn.fileTitle = new string(new char[64]);
            ofn.maxFileTitle = ofn.fileTitle.Length;
            ofn.title = "Select Python File";
            ofn.defExt = "py";
            ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

            if (WindowDll.GetOpenFileName(ofn))
            {
                pythonFilePath = ofn.file;
                PlayerPrefs.SetString("pythonfile", ofn.file);
                Debug.Log(ofn.file);
            }
        }
        if (GUI.Button(new Rect(260, 60, 160, 40), "create python model"))
        {
            string path = Path.Combine(Path.GetDirectoryName(pythonFilePath), "Py2Unity.py");
            if (File.Exists(path))
            {
                Debug.LogWarning("python model has been created allready!");
            }
            else
            {
                FileInfo fileInfo = new FileInfo(path);
                StreamWriter sw = fileInfo.CreateText();
                sw.WriteLine("import socket");
                sw.WriteLine("import sys");
                sw.WriteLine();
                sw.WriteLine("class Py2Unity:");
                sw.WriteLine("    def __init__(self):");
                sw.WriteLine("        self.__conn = self.__Connect()");
                sw.WriteLine("    def SendToUnity(self,data):");
                sw.WriteLine("        self.__conn.sendall(bytes(str(data),encoding='utf-8'))");
                sw.WriteLine();
                sw.WriteLine("    def RecFromUnity(self):");
                sw.WriteLine("        data = self.__conn.recv(1024)");
                sw.WriteLine("        if data == \"exit\":");
                sw.WriteLine("            sys.exit()");
                sw.WriteLine("        return data");
                sw.WriteLine();
                sw.WriteLine("    def __Connect(self):");
                sw.WriteLine("        s = socket.socket()");
                sw.WriteLine("        s.bind((\"127.0.0.1\",7758))");
                sw.WriteLine("        s.listen(1)");
                sw.WriteLine("        print(\"wait for connect...\")");
                sw.WriteLine("        return s.accept()[0]");
                sw.Flush();
                sw.Close();
                Debug.Log("created!");
            }

        }

        GUI.Label(new Rect(5, 110, 2000, 20), "click this button to create python model,you will find a python file named \"Py2Unity.py\" in your select python file folder");
        
        GUI.Label(new Rect(100, 150, 100, 20), "Unity Example");
        GUI.Label(new Rect(440, 150, 100, 20), "Python Example");
        GUI.TextArea(new Rect(30, 180, 250, 400), unityExample);
        GUI.TextArea(new Rect(350, 180, 250, 400), pythonExample);
    }
}
