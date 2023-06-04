using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Menus : MonoBehaviour
{
    public Animation RockAnim;
    public AnimationClip RockClip;

    public Animation PaperAnim;
    public AnimationClip PaperClip;

    public Animation ScissAnim;
    public AnimationClip ScissClip;

    public Animation Ex1Anim;
    public AnimationClip Ex1Clip;

    public Animation Ex2Anim;
    public AnimationClip Ex2Clip;

    public Animation Ex3Anim;
    public AnimationClip Ex3Clip;

    public Animation GunAnim;
    public AnimationClip GunClip;

    public Animation BlockAnim;
    public AnimationClip BlockClip;

    public GameObject NamesPanel;
    public GameObject AudioObj;

    private void Start()
    {
        List<GameObject> AudioContainers = new List<GameObject>();
        AudioContainers.AddRange(GameObject.FindGameObjectsWithTag("AudioObj"));

        if (AudioContainers.Count > 1)
        {
            Destroy(AudioContainers[1]);
        }

        StartCoroutine(MenuAnim());
    }

    private void Update()
    {
       
    }

    private IEnumerator MenuAnim()
    {
        Debug.Log("INITIALIZING MENU");

        yield return new WaitForSeconds(0.5f);

        RockAnim.clip.Equals(RockClip);
        RockAnim.Play();

        AudioHandler.Instance.PlayWoosh();

        while (RockAnim.isPlaying)

        {
            yield return null;
        }

        AudioHandler.Instance.PlayRock();

        PaperAnim.clip.Equals(PaperClip);
        PaperAnim.Play();

        AudioHandler.Instance.PlayWoosh();

        NamesPanel.GetComponent<ShakeScreen>().TriggerCameraShake(0.3f, 5);

        while (PaperAnim.isPlaying)
        {
            yield return null;
        }

        AudioHandler.Instance.PlayPaper();

        ScissAnim.clip.Equals(ScissClip);
        ScissAnim.Play();
        AudioHandler.Instance.PlayWoosh();

        NamesPanel.GetComponent<ShakeScreen>().TriggerCameraShake(0.3f, 5);

        while (ScissAnim.isPlaying)
        {
            yield return null;
        }

        AudioHandler.Instance.PlayScissors();

        yield return new WaitForSeconds(0.25f);

        Ex1Anim.clip.Equals(Ex1Clip);
        Ex1Anim.Play();
        AudioHandler.Instance.PlayWoosh();

        NamesPanel.GetComponent<ShakeScreen>().TriggerCameraShake(0.3f, 7);

        while (Ex1Anim.isPlaying)
        {
            yield return null;
        }

        AudioHandler.Instance.PlayFire();

        Ex2Anim.clip.Equals(Ex2Clip);
        Ex2Anim.Play();
        AudioHandler.Instance.PlayWoosh();
        NamesPanel.GetComponent<ShakeScreen>().TriggerCameraShake(0.3f, 7);

        while (Ex2Anim.isPlaying)
        {
            yield return null;
        }

        AudioHandler.Instance.PlayWater();

        Ex3Anim.clip.Equals(Ex3Clip);
        Ex3Anim.Play();
        AudioHandler.Instance.PlayWoosh();
        NamesPanel.GetComponent<ShakeScreen>().TriggerCameraShake(0.3f, 7);

        while (Ex3Anim.isPlaying)
        {
            yield return null;
        }

        AudioHandler.Instance.PlayLeaf();

        yield return new WaitForSeconds(1.5f);

        GunAnim.clip.Equals(GunClip);
        GunAnim.Play();
        AudioHandler.Instance.PlayWoosh();
        NamesPanel.GetComponent<ShakeScreen>().TriggerCameraShake(0.75f, 10);

        while (GunAnim.isPlaying)
        {
            yield return null;
        }

        AudioHandler.Instance.PlayShot();

        AudioHandler.Instance.PlayDraw();
        BlockAnim.clip.Equals(BlockClip);
        BlockAnim.Play();

        if(AudioHandler.Instance.src.clip == AudioHandler.Instance.MenuTrack)
        {
            Debug.Log("ALREADY Playing Menu Track");
            yield return null;
        }
        else
        {
            Debug.Log("Playing Menu Track");
            AudioHandler.Instance.PlayMenuTrack();

            yield return null;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Assets/Scenes/TimeSelection.unity");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToTut()
    {
        this.gameObject.GetComponent<MenuInputs>().CallTutTransition();
    }
}
