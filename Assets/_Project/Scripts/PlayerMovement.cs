using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Joystick m_Joystick;
	public float moveSpeed = 5f; // Base movement speed
	public float maxCounterShift = 5f; // Max counter shift force
	public float maxTilt = 10f; // Max tilt before dropping an item// Max tilt before dropping an item
	public Vector2 randomWeightShiftInterval;
	public float weightShiftRange;

	public RectTransform balanceBar;							// UI balance indicator
	public Image balanceFill;									// UI balance bar color
	public Color safeColor = Color.green;
	public Color dangerColor = Color.red;

	private Rigidbody2D rb;
	private Vector2 movementInput;
	private float balance = 0f;
	private float timer;// Player's tilt balance
	private float weightShiftInterval;

	void Start()
	{
		ResetTimer();
		rb = GetComponent<Rigidbody2D>();
		//InvokeRepeating("RandomBalanceShift", 2f, 2f);			// Random shifts every 2 sec
	}


	void Update()
	{
		RandomBalanceShift();

		// Get joystick input (simulating a slider effect)
		float inputY = m_Joystick.Vertical; // Up/Down movement
		float counterForce = Mathf.Abs(inputY) * maxCounterShift; // Stronger force further from center	

		movementInput = m_Joystick.Direction;

		// Apply counter-movement
		balance -= Mathf.Sign(inputY) * counterForce * Time.deltaTime;
		balance = Mathf.Clamp(balance, -maxTilt, maxTilt);

		// Update UI balance bar
		UpdateBalanceUI();
	}

	void FixedUpdate()
	{
		// Move player based on input
		rb.linearVelocity = (movementInput * moveSpeed) + (Vector2.down * balance);
	}

	void RandomBalanceShift()
	{
		timer += Time.deltaTime;
		if (timer >= weightShiftInterval)
		{
			ResetTimer();
		}
		else return;


		// Randomly tilts the player to one side
		balance += Random.Range(-weightShiftRange, weightShiftRange);
		balance = Mathf.Clamp(balance, -maxTilt, maxTilt);

		// Drop item if balance is too high
		if (Mathf.Abs(balance) >= maxTilt)
		{
			DropItem();
		}
	}
	private void ResetTimer()
	{
		timer = 0;
		weightShiftInterval = Random.Range(randomWeightShiftInterval.x, randomWeightShiftInterval.y);
	}

	void DropItem()
	{
		Debug.Log("Item Dropped!");
		// Logic to remove an item from inventory
	}

	void UpdateBalanceUI()
	{
		// Map balance (-maxTilt to maxTilt) to a rotation angle for UI
		balanceBar.rotation = Quaternion.Euler(0, 0, -balance * 10);

		// Change color based on tilt level
		float t = Mathf.Abs(balance) / maxTilt;
		balanceFill.color = Color.Lerp(safeColor, dangerColor, t);
	}
}
