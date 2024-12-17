using UnityEngine;
using System.Collections;
 
public class SpriteAnimator : MonoBehaviour
{
    [System.Serializable]
    public class AnimationTrigger
    {
        public string name;
        public int frame;
    }
 
    [System.Serializable]
    public class SpriteAnimation
    {
        public string name;
        public int fps;
        public Sprite[] frames;
 
        public string sequenceCode;
        public string cue;
 
        public AnimationTrigger[] triggers;
    }
 
    // sequence code format:
    // startFrame-endFrame:time(chance)
    // time: also be set to "forever" - this will loop the sequence indefinitely
    // chance: float value from 0-1, chance that the sequence will play (if not played, it will be skipped)
    // time and chance can both be ignored, this will mean the sequence plays through once
 
    // sequence code examples:
    // TV: 0-1:3, 2-3:3, 4-5:4, 6-7:4, 8:3, 9:3
    // Idle animation with random fidgets: 0-59, 60-69, 10-59, 0-59(.25), 70-129(.75)
    // Jump animation with looping finish: 0-33, 20-33:forever
 
    public SpriteRenderer spriteRenderer;
    public SpriteAnimation[] animations;
 
    public bool playing { get; private set; }
    public SpriteAnimation currentAnimation { get; private set; }
    public int currentFrame { get; private set; }
    [HideInInspector]
    public bool loop;
    public float speedMultiplier = 1f;
 
    public string playAnimationOnStart;
 
    bool looped;
 
    void Start()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
 
        if (playAnimationOnStart != "")
            Play(playAnimationOnStart);
    }
    
    void OnDisable()
    {
        playing = false;
        currentAnimation = null;
    }
 
    public void Play(string name, bool loop = true, int startFrame = 0)
    {
        SpriteAnimation animation = GetAnimation(name);
        if (animation != null)
        {
            this.loop = loop;
            currentAnimation = animation;
            playing = true;
            currentFrame = startFrame;
            StopAllCoroutines();
            StartCoroutine(PlayAnimation(currentAnimation));
        }
        else
        {
            Debug.LogWarning("Could not find animation: " + name);
        }
    }
 
    public void ForcePlay(string name, bool loop = true, int startFrame = 0)
    {
        SpriteAnimation animation = GetAnimation(name);
        if (animation != null)
        {
            this.loop = loop;
            currentAnimation = animation;
            playing = true;
            currentFrame = startFrame;
            spriteRenderer.sprite = animation.frames[currentFrame];
            StopAllCoroutines();
            StartCoroutine(PlayAnimation(currentAnimation));
        }
        else
        {
            Debug.LogWarning("Could not find animation: " + name);
        }
    }
 
    public void SlipPlay(string name, int wantFrame, params string[] otherNames)
    {
        for (int i = 0; i < otherNames.Length; i++)
        {
            if (currentAnimation != null && currentAnimation.name == otherNames[i])
            {
                Play(name, true, currentFrame);
                break;
            }
        }
        Play(name, true, wantFrame);
    }
 
    public bool IsPlaying(string name)
    {
        return (currentAnimation != null && currentAnimation.name == name);
    }
 
    public SpriteAnimation GetAnimation(string name)
    {
        foreach (var anim in animations)
        {
            if (anim.name == name)
            {
                return anim;
            }
        }
        return null;
    }
 
    IEnumerator CueAnimation(string animationName, float minTime, float maxTime)
    {
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        ForcePlay(animationName, false);
    }
 
    private IEnumerator PlayAnimation(SpriteAnimation animation)
    {
        Debug.Log("Starting animation: " + animation.name);
        playing = true;
        currentFrame = 0;

        while (playing)
        {
            Debug.Log($"Displaying frame {currentFrame} of {animation.frames.Length}");
            if (currentFrame >= animation.frames.Length)
            {
                if (loop)
                {
                    currentFrame = 0;
                }
                else
                {
                    playing = false;
                    break;
                }
            }

            spriteRenderer.sprite = animation.frames[currentFrame];

            // Wait for the specified time before showing the next frame
            yield return new WaitForSeconds(1f / (animation.fps * speedMultiplier));

            currentFrame++;
        }
    }
 
    void NextFrame(SpriteAnimation animation)
    {
        looped = false;
        currentFrame++;
        foreach (AnimationTrigger animationTrigger in animation.triggers)
        {
            if (animationTrigger.frame == currentFrame)
            {
                gameObject.SendMessageUpwards(animationTrigger.name);
            }
        }
 
        if (currentFrame >= animation.frames.Length)
        {
            if (loop)
                currentFrame = 0;
            else
                currentFrame = animation.frames.Length - 1;
        }
    }
 
    public int GetFacing()
    {
        return (int)Mathf.Sign(spriteRenderer.transform.localScale.x);
    }
 
    public void FlipTo(float dir)
    {
        if (dir < 0f)
            spriteRenderer.transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            spriteRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
    }
 
    public void FlipTo(Vector3 position)
    {
        float diff = position.x - transform.position.x;
        if (diff < 0f)
            spriteRenderer.transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            spriteRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}