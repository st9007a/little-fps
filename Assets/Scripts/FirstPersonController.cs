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

	private string animState;
	private float idleCounter;
	private Dictionary<string, int> clipMapping;

	void Awake () {
		animState = "Idle";
		idleCounter = idleDuration;

		clipMapping = new Dictionary<string, int>();

		for(int i = 0; i < gunClip.Length; i++) {
			clipMapping.Add(gunClip[i].name, i);
		}
	}

	void Update () {
		Vector3 dir = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;
		dir.Normalize();

		GetComponent<Rigidbody>().velocity = new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime + Vector3.up * GetComponent<Rigidbody>().velocity.y;

		if (GetComponent<Rigidbody>().velocity.x != 0 || GetComponent<Rigidbody>().velocity.z != 0) {
			ResetAnimState();
		}

		if (Input.GetMouseButtonDown(0)) {
			animState = ANIM_FIRE;
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
	}

	void Reload() {
		PlayAnim(ANIM_RELOAD);
		ResetAnimState();
	}
}
