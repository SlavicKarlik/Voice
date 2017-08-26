using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;

namespace Voice_Recognition
{
    public partial class Form1 : Form
    {
        
        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
        SpeechSynthesizer synth = new SpeechSynthesizer();
           
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
           //synth.SelectVoice("Microsoft Helena Desktop");
           
            recEngine.RecognizeAsync(RecognizeMode.Multiple);
            btnDisable.Enabled = true;
            btnEnable.Enabled = false;

            synth.SetOutputToDefaultAudioDevice();

            synth.Speak("Hola, Que tal?");
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Text += ("\nZacznij się śmiać. \n");

            Choices command = new Choices();
            command.Add(new string[] {"time", "date", "ha", "ha ha", "ha ha ha", "ha ha ha ha", "exit"});
            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(command);
            Grammar grammar = new Grammar(gBuilder);

            recEngine.LoadGrammarAsync(grammar);
            recEngine.SetInputToDefaultAudioDevice();

            //Wyswietlanie informacji na temat zainstalowanych silnikow czytania tekstu
            foreach (InstalledVoice voice in synth.GetInstalledVoices())
            {
                VoiceInfo info = voice.VoiceInfo;

                richTextBox1.Text += "\nName: " + info.Name;
                richTextBox1.Text += "\nCulture:" + info.Culture;
                richTextBox1.Text += "\nAge:" + info.Age;
                richTextBox1.Text += "\nGender:" + info.Gender;
                richTextBox1.Text += "\nDescription:" + info.Description;
                richTextBox1.Text += "\nId:" + info.Id;
                richTextBox1.Text += "\nEnabled:" + voice.Enabled;
                richTextBox1.Text += "\n-----------------------\n";
                synth.Speak(info.Name);
            }

            //Wyswietlanie informacji na temat zainstalowanych silnikow rozpoznawania mowy
            foreach (RecognizerInfo ri in SpeechRecognitionEngine.InstalledRecognizers())
            { 
                richTextBox1.Text += "\nName: " + ri.Name;
                richTextBox1.Text += "\nCulture: " + ri.Culture;
                richTextBox1.Text += "\nDescription: " + ri.Description;
                richTextBox1.Text += "\nId: " + ri.Id;
                richTextBox1.Text += "\nSupportedAudioFormat:" + ri.SupportedAudioFormats;
                richTextBox1.Text += "\nAdditionalInfo:" + ri.AdditionalInfo;
                richTextBox1.Text += "\n-----------------------\n";
            }
            

            recEngine.SpeechRecognized += recEngine_SpeechRecognized;
            
        }

        void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "ha":
                    richTextBox1.Text += ("\nZaśmiałaś/eś się jeden raz.");
                    break;
                case "ha ha":
                    richTextBox1.Text += "\nZaśmiałaś/eś się dwa razy.";
                    break;
                case "ha ha ha":
                    richTextBox1.Text += ("\nZaśmiałaś/eś się trzy razy.");
                    break;
                case "ha ha ha ha":
                    richTextBox1.Text += ("\nZaśmiałaś/eś się cztery razy.");
                    break;
                case "time":
                    richTextBox1.Text += (DateTime.Now.ToString("h:mm:ss tt"));
                    synth.Speak(DateTime.Now.ToString("h:mm:ss tt"));
                    break;
                case "date":
                    richTextBox1.Text += (DateTime.Today.ToString("D"));
                    synth.Speak(DateTime.Today.ToString("D"));
                    break;
                case "exit":
                    richTextBox1.Text += "\nWychodzę.";
                    Thread.Sleep(3000);
                    Application.Exit();
                    break;

                default:
                    richTextBox1.Text += "\nNie rozpoznano śmiechu.";
                    break;
            }
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsyncStop();
            btnEnable.Enabled = true;
            btnDisable.Enabled = false;
        }
    }
}
