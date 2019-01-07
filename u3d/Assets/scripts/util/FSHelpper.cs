using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace qhUtil {
    /// <summary>
    /// FileSystemHelpper
    /// </summary>
    public class FSHelpper
    {
        static public void WriteStringToFile(string path, string content)
        {
            FileStream fs = new FileStream(path, FileMode.Create);

            // convert to binary & write file
            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(content);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
        static public string ReadStringFromFile(string path)
        {
            // read file
            FileStream fs = new FileStream(path, FileMode.Open);

            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();

            // convert binary to string
            string s = new System.Text.UTF8Encoding().GetString(bytes);

            return s;
        }

        static public string RemoveExtension(string path)
        {
            string ext = Path.GetExtension(path);
            if (ext.Length > 0)
            {
                string pathWithoutExt = path.Remove(path.Length - ext.Length);
                return pathWithoutExt;
            }
            else
            {
                return path;
            }
        }
        static public void CreateDirIfNotExist(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        static public void DeleteDirIfExist(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        public static void CopyOnlyFolder(string formPath_, string toPath_)
        {
            Directory.CreateDirectory(toPath_);

            string[] directories = Directory.GetDirectories(formPath_, "*", SearchOption.AllDirectories);
            for (int i = 0; i < directories.Length; ++i)
            {
                string sourcePath = directories[i];
                string destPath = sourcePath.Replace(formPath_, toPath_);

                CreateDirIfNotExist(destPath);
            }
        }

        public static void CopyFolder(string formPath_, string toPath_)
        {
            CopyOnlyFolder(formPath_, toPath_);

            string[] files = Directory.GetFiles(formPath_, "*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; ++i)
            {
                string sourcePath = files[i];
                string destPath = sourcePath.Replace(formPath_, toPath_);
                System.IO.File.Copy(sourcePath, destPath, true);
            }
        }

        static public string CombineAssetPath(string bunlePath, string assetName)
        {
            return string.Format("{0}/{1}", bunlePath, assetName);
        }
    }
}