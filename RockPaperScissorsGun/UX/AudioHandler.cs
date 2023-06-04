using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public AudioSource src;

    public AudioClip MenuTrack;

    public AudioClip ResultsTrack;

    public AudioClip Track1;
    public AudioClip Track2;
    public AudioClip Track3;

    public AudioClip SFX_Water;
    public AudioClip SFX_Fire;
    public AudioClip SFX_Leaf;
    public AudioClip SFX_Rock;
    public AudioClip SFX_Paper;
    public AudioClip SFX_Scissors;
    public AudioClip SFX_Shot;
    public AudioClip SFX_Cheer;
    public AudioClip SFX_Woosh;
    public AudioClip SFX_Boo;
    public AudioClip SFX_Gasp;

    public AudioClip SFX_Add;
    public AudioClip SFX_Draw;

    public AudioClip VX_M01;
    public AudioClip VX_M02;
    public AudioClip VX_M03;
    public AudioClip VX_M04;
    public AudioClip VX_M05;
    public AudioClip VX_M06;

    public void PlayMenuTrack()
    {
        src.clip = MenuTrack;
        src.Play();
    }

    public void PlayResultsTrack()
    {
        src.clip = ResultsTrack;
        src.Play();
    }

    public void PlayTrack01()
    {
        // Debug.Log("Playing Track 1");
        src.clip = Track1;
        src.Play();
    }

    public void PlayTrack02()
    {
        //Debug.Log("Playing Track 2");
        src.clip = Track2;
        src.Play();
    }

    public void PlayTrack03()
    {
        //Debug.Log("Playing Track 3");
        src.clip = Track3;
        src.Play();
    }

    public void PlayWater()
    {
        src.PlayOneShot(SFX_Water);
    }

    public void PlayFire()
    {
        src.PlayOneShot(SFX_Fire);
    }

    public void PlayLeaf()
    {
        src.PlayOneShot(SFX_Leaf);
    }

    public void PlayRock()
    {
        src.PlayOneShot(SFX_Rock);
    }

    public void PlayPaper()
    {
        src.PlayOneShot(SFX_Paper);
    }

    public void PlayScissors()
    {
        src.PlayOneShot(SFX_Scissors);
    }

    public void PlayShot()
    {
        src.PlayOneShot(SFX_Shot);
    }

    public void PlayWoosh()
    {
        src.PlayOneShot(SFX_Woosh);
    }

    public void PlayCheer()
    {
        src.PlayOneShot(SFX_Cheer);
    }

    public void PlayBoo()
    {
        src.PlayOneShot(SFX_Boo);
    }

    public void PlayGasp()
    {
        src.PlayOneShot(SFX_Gasp);
    }

    public void PlayAdd()
    {
        src.PlayOneShot(SFX_Add);
    }

    public void PlayDraw()
    {
        src.PlayOneShot(SFX_Draw);
    }

    public void PlayM01()
    {
        src.PlayOneShot(VX_M01);
    }

    public void PlayM02()
    {
        src.PlayOneShot(VX_M02);
    }

    public void PlayM03()
    {
        src.PlayOneShot(VX_M03);
    }

    public void PlayM04()
    {
        src.PlayOneShot(VX_M04);
    }
    public void PlayM05()
    {
        src.PlayOneShot(VX_M05);
    }

    public void PlayM06()
    {
        src.PlayOneShot(VX_M06);
    }
}
