using TMPro;  // Import for TextMeshPro
using UnityEngine;
using UnityEngine.UI;  // Import for UI buttons

public class StepsManager : MonoBehaviour
{
    public TextMeshProUGUI stepText;  // Reference to the TextMeshPro UI component
    public Button previousButton;     // Reference to the "Previous" button
    public Button nextButton;         // Reference to the "Next" button
    public Animator sunAnimator;
    public Animator earthAnimator;
    public ParticleSystem auroraEffect;

    public GameObject sunFieldLines;
    public GameObject earthSatellites;

    public AudioSource stepAudioSource;
    public AudioClip[] stepsAudioClips;

    TypeWriterEffect typeWriterEffect;

    private string[] steps = new string[]
    {
        "The Sun releases a burst of energy in the form of a solar flare or a coronal mass ejection (CME), sending billions of tons of charged particles (plasma) into space. These particles are carried by the solar wind and travel toward Earth, usually taking 1 to 3 days to arrive.",
        "When the charged particles from the solar wind collide with Earth's magnetic field (the magnetosphere), they cause disturbances and fluctuations. The solar wind compresses the magnetosphere on the side facing the Sun while stretching it on the opposite side, generating shockwaves that disturb the magnetic field. This interaction leads to variations in Earth's magnetic environment.",
        "As the charged particles are funneled toward Earth's poles, they interact with the atmosphere, releasing energy that produces stunning auroras. These interactions also cause geomagnetic disturbances, which can disrupt satellite communications, GPS, power grids, and radio signals. In severe cases, these storms may even result in power outages and damage spacecraft systems."
    };

    private int currentStep = 0;  // Index for the current step

    void Start()
    {
        // Set initial text
        stepText.text = steps[currentStep];
        PlayAnimationForCurrentStep();
        PlayStepAudio();

        // Add listeners to buttons
        previousButton.onClick.AddListener(PreviousStep);
        nextButton.onClick.AddListener(NextStep);

        typeWriterEffect = gameObject.GetComponent<TypeWriterEffect>();

        // Disable "Previous" button at the start
        previousButton.gameObject.SetActive(false);
    }

    void PlayAnimationForCurrentStep()
    {
        // Play different animations based on the current step
        switch (currentStep)
        {
            case 0:
                sunAnimator.Play("Solar Energy Animation");
                earthAnimator.Play("New State");
                auroraEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                sunFieldLines.SetActive(true);
                earthSatellites.SetActive(false);
                break;

            case 1:
                sunAnimator.Play("Sun Field Animation");
                earthAnimator.Play("Earth Field");
                auroraEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                earthSatellites.SetActive(false);
                sunFieldLines.SetActive(true);
                break;

            case 2:
                sunAnimator.Play("New State");
                earthAnimator.Play("New State");
                auroraEffect.Play();
                earthSatellites.SetActive(true);
                sunFieldLines.SetActive(false);
                break;
        }
    }

    // Method to go to the next step
    void NextStep()
    {
        if (currentStep < steps.Length - 1)
        {
            currentStep++;
            stepText.text = steps[currentStep];
            PlayAnimationForCurrentStep();
            PlayStepAudio();
            typeWriterEffect.StartTyping(steps[currentStep]);
        }

        // Manage button interactivity
        if (currentStep == steps.Length - 1)
        {
            nextButton.gameObject.SetActive(false);
        }

        previousButton.gameObject.SetActive(true);
    }

    // Method to go to the previous step
    void PreviousStep()
    {
        if (currentStep > 0)
        {
            currentStep--;
            stepText.text = steps[currentStep];
            PlayAnimationForCurrentStep();
            PlayStepAudio();
            typeWriterEffect.StartTyping(steps[currentStep]);
        }

        // Manage button interactivity
        if (currentStep == 0)
        {
            previousButton.gameObject.SetActive(false);
        }
        nextButton.gameObject.SetActive(true);
    }

    void PlayStepAudio()
    {
        if (stepAudioSource.isPlaying)
        {
            stepAudioSource.Stop();
        }

        stepAudioSource.PlayOneShot(stepsAudioClips[currentStep]);
    }
}
