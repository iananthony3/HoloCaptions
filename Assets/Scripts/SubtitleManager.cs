using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SubtitleManager : MonoBehaviour
{
    //[Tooltip("A text area for the recognizer to display the recognized strings.")]
    public Text DictationDisplay;

    private DictationRecognizer dictationRecognizer;

    // Cache the text currently displayed in the text box.
    private StringBuilder textSoFar;


    bool isListening = true;

    public Text statusText;

    public RawImage statusImage;



    void Awake()
    {
        // DictationRecognizer created and assigned to dictationRecognizer variable.
        dictationRecognizer = new DictationRecognizer();

        // As the recognizer listens, it provides text of what it's heard so far.
        // This event is fired while user is talking.
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;

        // The full recognized string is returned here.
        // This event is fired after user pauses, typically at end of sentence.
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;

        // This event is fired when the recognizer stops.
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;

        dictationRecognizer.DictationError += DictationRecognizer_DictationError;

        // Cache the text currently displayed in the text box.
        textSoFar = new StringBuilder();

        // Start dictation recognizer.
        dictationRecognizer.Start();

        dictationRecognizer.InitialSilenceTimeoutSeconds = (10000);
        dictationRecognizer.AutoSilenceTimeoutSeconds = (10000);
        

    }

    /// <summary>
    /// This event is fired while the user is talking.
    /// </summary>
    /// <param name="text">The currently hypothesized recognition.</param>
    private void DictationRecognizer_DictationHypothesis(string text)
    {
        // DictationDisplay text is set to be textSoFar and new hypothesized text
        DictationDisplay.text = text  + "...";
        SetThinking();
    }

    /// <summary>
    /// This event is fired after the user pauses, typically at the end of a sentence.
    /// </summary>
    /// <param name="text">The text that was heard by the recognizer.</param>
    /// <param name="confidence">A representation of how confident the recognizer is of this recognition.</param>
    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        // Append textSoFar with latest text
        textSoFar.Append(text + ". ");

        // DictationDisplay text set to be textSoFar
        DictationDisplay.text = text;
        
        SetListening();
    }

    /// <summary>
    /// This event is fired when the recognizer stops.
    /// </summary>
    /// <param name="cause">An enumerated reason for the session completing.</param>
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        dictationRecognizer.Stop();
        SetSleeping();
        dictationRecognizer.Dispose();
        dictationRecognizer.Start();
    }



    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        // DictationDisplay text set to be the error string.
        DictationDisplay.text = error + "\nHRESULT: " + hresult;
    }

    private void SetListening()
    {
        var listeningTexture = (Texture2D)Resources.Load("mic_icon_red");

        statusImage.texture = listeningTexture;

        statusText.text = "Listening";
        
    }

    private void SetSleeping()
    {
        var sleepingTexture = (Texture2D)Resources.Load("mic_icon_gray");

        statusImage.texture = sleepingTexture;

        statusText.text = "Sleeping";
        DictationDisplay.text = string.Empty;
    }

    private void SetThinking()
    {
        var thinkingTexture = (Texture2D)Resources.Load("mic_icon");

        statusImage.texture = thinkingTexture;

        statusText.text = "Thinking";
    }
}
