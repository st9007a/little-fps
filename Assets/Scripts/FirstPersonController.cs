using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour {

	private const string ANIM_IDLE = "Idle";
	private const string ANIM_FIRE = "Fire";
	private const string ANIM_RELOAD = "Reload";

	public float speed;
	public float idleDuration;
	public GameObject gun;
	public AnimationClip[] gunClip;
	public GameObject bullets;
	public GameObject mCamera;

	private string animState;
	private float idleCounter;
	private Dictionary<string, int> clipMapping;
	private int bulletCount;
	private Ray aim;

	void Awake () {
		animState = "Idle";
		idleCounter = idleDuration;

		clipMapping = new Dictionary<string, int>();

		for(int i = 0; i < gunClip.Length; i++) {
			clipMapping.Add(gunClip[i].name, i);
		}


		bulletCount = bullets.transform.childCount;
	}

	void Start () {
		Cursor.visible = false;
		aim = mCamera.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
	}

	void Update () {
		RaycastHit hit;
		Vector3 dir = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;
		dir.Normalize();

		GetComponent<Rigidbody>().velocity = new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime + Vector3.up * GetComponent<Rigidbody>().velocity.y;

		if (GetComponent<Rigidbody>().velocity.x != 0 || GetComponent<Rigidbody>().velocity.z != 0) {
			ResetAnimState();
		}

		if (Input.GetMouseButtonDown(0)) {
			animState = ANIM_FIRE;
			
			if (Physics.Raycast(aim, out hit)) {
				Debug.Log(hit.collider.gameObject.name);
			}
		}

		if (Input.GetKeyDown("space")) {
			animState = ANIM_RELOAD;
		}

		switch (animState) {
			case ANIM_IDLE:
				Idle();
				break;
			case ANIM_FIRE:
				Fire();
				break;
			case ANIM_RELOAD:
				Reload();
				break;
			default:
				break;
		}
	}

	void PlayAnim (string clipName) {
		Animation anim = gun.GetComponent<Animation>();
		anim.AddClip(gunClip[clipMapping[clipName]], clipName);
		anim.Play(clipName);
	}

	void ResetAnimState () {
		animState = ANIM_IDLE;
		idleCounter = idleDuration;
	}

	void Idle () {
		if (idleCounter <= 0) {
			PlayAnim(ANIM_IDLE);
			idleCounter = idleDuration;
		}

		idleCounter -= Time.deltaTime;
	}

	void Fire () {
		PlayAnim(ANIM_FIRE);
		ResetAnimState();

		// ui
		if (bulletCount <= 0) {
			return;
		}

		bulletCount -= 1;
		bullets.transform.GetChild(bulletCount).gameObject.SetActive(false);
	}

	void Reload() {
		PlayAnim(ANIM_RELOAD);
		ResetAnimState();

		// ui
		bulletCount = bullets.transform.childCount;
		for (int i = 0; i < bulletCount; i++) {
			bullets.transform.GetChild(i).gameObject.SetActive(true);
		}
	}
}
