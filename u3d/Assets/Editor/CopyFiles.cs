using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CopyFiles : MonoBehaviour {

    public static void Copy(string src, string dst) {
        CopyFolder(src, dst);
    }

    public static void CopyOnlyFolder(string formPath_, string toPath_) {
        Directory.CreateDirectory(toPath_);

        string[] directories = Directory.GetDirectories(formPath_, "*", SearchOption.AllDirectories);
        for (int i = 0; i < directories.Length; ++i) {
            string sourcePath = directories[i];
            string destPath = sourcePath.Replace(formPath_, toPath_);
            if (!Directory.Exists(destPath)) {
                Directory.CreateDirectory(destPath);
            }
        }
    }

    public static void CopyFolder(string formPath_, string toPath_) {
        CopyOnlyFolder(formPath_, toPath_);

        string[] files = Directory.GetFiles(formPath_, "*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; ++i ) {
            string sourcePath = files[i];
            string destPath = sourcePath.Replace(formPath_, toPath_);
            File.Copy(sourcePath, destPath, true);
        }
    }
}
