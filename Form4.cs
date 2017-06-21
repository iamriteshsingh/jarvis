using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jarvis1
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        SpeechSynthesizer jarvis = new SpeechSynthesizer();
        PromptBuilder pBuilder = new PromptBuilder();
        SpeechRecognitionEngine sRecognize = new SpeechRecognitionEngine();
        private void Form4_Load(object sender, EventArgs e)
        {
            jarvis.Speak("TELL THE PASSOWRD TO CONFIRM YOUR IDENTITY");
            Choices sList = new Choices();
            sList.Add(new String[] { "password","bike" });
            Grammar g = new Grammar(new GrammarBuilder(sList));
            try
            {
                sRecognize.RequestRecognizerUpdate();
                sRecognize.LoadGrammar(g);
                sRecognize.SpeechRecognized += sRegonige_SpeechRecognized;
                sRecognize.SetInputToDefaultAudioDevice();
                sRecognize.RecognizeAsync(RecognizeMode.Single);
            }
            catch
            {
                return;
            }
        }

        private void sRegonige_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            jarvis.Speak("enter");
            if(e.Result.Text=="password")
            {
                jarvis.Speak("Identity verified...welcome sir jarvis is ready for you");
                Application.Run(new Form1());
                Close();
            }
            else
            {
                jarvis.Speak("Wrong password");
            }
        }
    }
}
