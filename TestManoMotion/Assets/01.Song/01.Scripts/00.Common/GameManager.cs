﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public Transform camPos;

	public Transform mainPos;

	public Book_v2 masterBook;
	public PrimeHand hand;
	public ManoVisualization mano;
	public FadeChange fadeCanvas;

	public MasterBookCreator master;

	//master책을 생성하기위한 공간
	public GameObject bookZone;

	public GameObject keyZone;
	public GameObject bookKey;

	//델리게이트 이벤트, FadeOut / In -> 행성 UI 표시 
	public event VoidNotier OnStartInfo;
	

	private void Awake()
	{
		mainPos = GameObject.Find("MainPosition").transform;

		hand = FindObjectOfType<PrimeHand>();
		mano = FindObjectOfType<ManoVisualization>();
		fadeCanvas = FindObjectOfType<FadeChange>();
		master = FindObjectOfType<MasterBookCreator>();

        if (instance == null) instance = GetComponent<GameManager>();
		else Destroy(this);

		camPos = Camera.main.transform.parent.transform;
		//mainPos.position = camPos.position;
	}
	
	public void BackGroundOn(bool temp)
	{
		GameManager.instance.mano.Show_background_layer = temp;
		GameManager.instance.mano._layer_background.gameObject.SetActive(temp);
		GameManager.instance.mano._layer_background.enabled = temp;
	}

	//World 나가는 메소드
	public IEnumerator ExitWorld()
	{
		//bookZone.SetActive(true);

		//핸드 모드 entryMode 변경
		GameManager.instance.hand.mode.ModeChange(hand.entryMode);
		fadeCanvas.FadeOut();
		BackGroundOn(true);
		yield return new WaitForSeconds(3.0f);
		
		//밖으로 나왔을때 transform 초기화 
		camPos.position = mainPos.position;
		camPos.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
		fadeCanvas.FadeIn();
		SoundManager.instance.StartBackGroundSound();
	}

	//World 들어가는 메소드
	public IEnumerator EnterWorld()
	{
		//bookZone.SetActive(false);

		fadeCanvas.FadeOut();
		masterBook.CloseBook();
		yield return new WaitForSeconds(3.0f);
		BackGroundOn(false);
		fadeCanvas.FadeIn();
		//3초뒤에 해당 UI 생성 
		yield return new WaitForSeconds(3.0f);
		//MoonUIMGR 구독중
		OnStartInfo?.Invoke();
	}

	[HideInInspector]
	public GameObject zone;
	public bool isCanCreateBook = false;

	//BOOKZONE 버튼으로 활성화
	public void OpenBookZone()
	{
		Quaternion rotation = Quaternion.identity;
		rotation.eulerAngles = new Vector3(-90, 0, 0);

		zone = Instantiate(bookZone, camPos.position + new Vector3(0, -0.5f, 1), rotation);
		zone.SetActive(true);
		mainPos.position = zone.transform.position + new Vector3(0, 0.5f, -1);

		var msg = ">> 마스터북을 생성할 공간을 만들었습니다. <<" + "\n" + "\n"
			+ "GRAB 제스처 후 Release 제스처를 하여 " + "\n"
			+ "다음 미션을 수행합시다.";
		StartCoroutine(UIManager.instance.ShowMissionUI(msg));

		Animator zoneAnim = zone.GetComponent<Animator>();
		SoundManager.instance.soundPlayer.PlayOneShot(SoundManager.instance.bookZone);
		zoneAnim.SetTrigger("OpenZone");

		//Zone을 생성하고 다음에 제스처를 통해서 책을 생성한다. 
		isCanCreateBook = true;
		Destroy(GameObject.Find("BookZone_Btn"));
	}

	//리셋 버튼을 클릭했을때 다시 메뉴창으로 넘어간다. 
	public void GotoMenu()
	{
		SceneManager.LoadScene("IntroScene");
	}
}
