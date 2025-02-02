﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

/// <summary>
/// 이 클래스는 사진들을 찍고 저장, 저장된 사진을 불러오는 함수들을 가집니다.
/// </summary>
public static class PhotoUtils
{
    static public string appPath = Application.persistentDataPath + "/Photos/";

    /// <summary>
    /// <para>이 함수는 현재 보고 있는 화면(ManoMotion Background)을 Jpg파일로 저장합니다.</para>
    /// </summary>
    /// <param name="backGroundTexture">저장할 백그라운드. 예를들어 ManomotionManager.Instance.Visualization_info.rgb_image 이게 Manomotion의 Background입니다.</param>
    /// <param name="folderName">저장할 폴더위치. 예를들어 Application.persistentDataPath + "/Photons" 이런식으로 경로를 설정하면, 이 Photons폴더경로에 Jpg들이 저장됩니다.</param>
    static public void TakePhoto(Texture2D backGroundTexture, string folderName)
    {
        //폴더의 위치
        string folderPath = appPath + folderName;
        //폴더가 없을 경우 예외처리 및 폴더 생성
        if (Directory.Exists(folderPath) == false)
        {
            Debug.LogError("디렉토리가 없음. 폴더를 새로 생성함");
            MakeFolder(folderName);
        }
        //BackGroundTexture를 jpg로 인코딩 및 저장
        byte[] bytes = backGroundTexture.EncodeToJPG();
        string fileName = string.Format("{0}/{1}.jpg", folderPath, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        File.WriteAllBytes(fileName, bytes);
    }

    /// <summary>
    /// <para>이 함수는 인자로 받는 폴더 경로에서부터 모든 Jpg파일들을 읽고, Texture2D의 배열로 반환해줍니다.</para>
    /// </summary>
    /// <param name="folderName">읽어들일 파일들의 폴더 경로</param>
    /// <returns></returns>
    static public List<Texture2D> ReadTexturesInFolder(string folderName)
    {
        //폴더가 없을 경우 예외처리 및 폴더 생성
        if (Directory.Exists(appPath) == false)
        {
            Debug.Log("디렉토리가 없음. 폴더를 새로 생성함");
            MakeInitFolder();
            //폴더가 처음 만들어졌다면 파일이 없으므로 null 반환
            return null;
        }

        //파일이름의 배열을 구하고 그 길이만큼 리스트 초기화
        string[] fileNames = Directory.GetFiles(appPath + folderName);
        List<Texture2D> textures = new List<Texture2D>();

        //메타파일 제외하고 다른 파일들로 리스트 구성
        for (int i = 0; i < fileNames.Length; i++)
        {
            bool isMeta = fileNames[i].Contains(".meta");
            if (!isMeta)
            {
                textures.Add(new Texture2D(2, 2, TextureFormat.BGRA32, false));
            }
        }

        //구성된 리스트의 텍스쳐에 이미지 로드
        for(int i = 0; i < textures.Count; i++)
        {
            byte[] bytes = File.ReadAllBytes(fileNames[i]);
            textures[i].LoadImage(bytes);
        }

        //구성된 텍스쳐리스트를 반환
        return textures;
    }

    //최초로 폴더를 만드는 메서드
    static public void MakeInitFolder()
    {
        StringBuilder sb = new StringBuilder(150);
        sb.Append(Application.persistentDataPath);
        sb.Append("/Photos/");
        Directory.CreateDirectory(sb.ToString());
    }

    /// <summary>
    /// <para>이 함수는 인자로 받는 경로에 인자로 받는 이름으로 폴더를 생성합니다. 기본값으로 Application.persistentDataPath에 생성합니다.</para>
    /// </summary>
    /// <param name="folderName">생성할 폴더 이름</param>
    static public void MakeFolder(string folderName)
    {
        //폴더가 없을 경우 예외처리 및 폴더 생성
        if (Directory.Exists(appPath) == false)
        {
            Debug.LogError("디렉토리가 없음. 폴더를 새로 생성함");
            MakeInitFolder();
        }

        //persistentDataPath에 인자로 받은 폴더이름으로 폴더 생성
        StringBuilder sb = new StringBuilder(150);
        sb.Append(appPath);
        sb.Append(folderName);
        Directory.CreateDirectory(sb.ToString());
    }
}
