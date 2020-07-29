using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace InsaneOne.EcsRts.Storing
{
	[CreateAssetMenu]
	public class GameStartData : ScriptableObject
	{
		[Header("Match start parameters")]
		public UnitData StartUnit;
		[Range(1, 4)] public int StartPlayersCount = 2;
		public int StartMoney = 1000;

		[Header("Camera settings")]
		public float CameraSpeed = 6;

		[Header("Misc")]
		public List<Color> PlayerColors = new List<Color>()
		{
			new Color(0.9f, 0.4f, 0.3f, 1f),
			new Color(0.2f, 0.4f, 0.9f, 1f),
			new Color(0.3f, 0.9f, 0.4f, 1f),
			new Color(0.9f, 0.8f, 0.4f, 1f),
		};

		[Header("UI Templates")] 
		public GameObject HealthbarTemplate;
		public GameObject BuyButtonTemplate;
	}
}