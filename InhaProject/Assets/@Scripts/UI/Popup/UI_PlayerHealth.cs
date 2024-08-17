using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHeart : MonoBehaviour
{
	public GameObject heart;
	public List<Image> hearts;

	int playerHealth = 5;

	private void Start()
	{
		for ( int i = 0; i < playerHealth; i++ )
		{
			GameObject heartObj = Instantiate(heart, this.transform);
			hearts.Add(heartObj.GetComponent<Image>());
		}
	}

	//   UI_PlayerHeart instance;

	//   public int maxHealth;
	//   int health;

	//   void Awake()
	//   {
	//       if ( instance == null)
	//       {
	//           instance = this;
	//       }
	//   }

	//private void Start()
	//{
	//       health = maxHealth;
	//}

	//   public void TakeDamage()
	//   {
	//       Debug.Log("health: " + health); 
	//       if ( health <= 0 )
	//       {
	//           return;
	//       }

	//       health -= 1;
	//   }
}
