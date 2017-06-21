using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;
using System.Diagnostics;
using HtmlAgilityPack;
using System.Data.SqlClient;


namespace jarvis1
{
    public partial class Form1 : Form
    {
        public Boolean flag = true;
        public Form1()
        {
            ths = new Thread(threadSplash);
            ths.Start();
            Thread.Sleep(5000);
            ths.Abort();
            // ths = new Thread(threadPassword);
            //ths.Start();
            Thread.Sleep(5000);
            InitializeComponent();
           
        }

        private void threadSplash(object obj)
        {
            Application.Run(new Form2());
        }
        private void threadPassword(object obj)
        {
            Application.Run(new Form4());
        }
        Thread thb;
        Thread ths;
        Thread thf;
        public string url; // for url of different sites
        public int condition = 0 ;// stores different case values
        SpeechSynthesizer jarvis = new SpeechSynthesizer();
        PromptBuilder pBuilder = new PromptBuilder();
        SpeechRecognitionEngine sRecognize = new SpeechRecognitionEngine();
        private void Form1_Load(object sender, EventArgs e)
        {
            
            button6.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            thb = new Thread(thPowerStatus);
            thb.Start();
            ths.Abort();
        }


        private void thPowerStatus()
        {
            Boolean isRunningOnBattery =(System.Windows.Forms.SystemInformation.PowerStatus.PowerLineStatus ==PowerLineStatus.Offline);
            String batterylife;
            batterylife = SystemInformation.PowerStatus.BatteryLifePercent.ToString();
            double i = (double.Parse(batterylife))*100;
            while(i>15)
            {
                Thread.Sleep(60000);
                batterylife = SystemInformation.PowerStatus.BatteryLifePercent.ToString();
                i = (double.Parse(batterylife)) * 100;
            }
            if (i <= 15 && isRunningOnBattery)
            {
                jarvis.Speak("connect to charger sir ...my battery is about to die");
            }
            if(i<=15 && !isRunningOnBattery)
            {
                jarvis.Speak("thankyou for the power supply sir ");
            }
            //batterylife = SystemInformation.UserDomainName.ToString();
            //MessageBox.Show(batterylife);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text!="")
            {
                button2.Enabled = true;
                button3.Enabled = false;
                button4.Enabled = true;
                button5.Enabled = false;
                jarvis.Dispose();
                jarvis = new SpeechSynthesizer();
                jarvis.Volume = SoundTrackBar.Value;
                jarvis.SpeakAsync(textBox1.Text);
            }
            else
            {
                jarvis.Speak("THE TEXT BOX IS EMPTY ....ENTER THE TEXT TO SPEAK SIR");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (jarvis.State == SynthesizerState.Speaking)
            {
                jarvis.Pause();
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = true;
                button4.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (jarvis.State == SynthesizerState.Paused)
            {
                jarvis.Resume();
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = false;
                button4.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (jarvis.State == SynthesizerState.Speaking || jarvis.State == SynthesizerState.Paused)
            {
                jarvis.Dispose();
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button6.Enabled = true;
            button5.Enabled = false;
            jarvis = new SpeechSynthesizer();
            jarvis.Volume = SoundTrackBar.Value;
            jarvis.Speak("Welcome sir jarvis is ready for you");
            //Choices sList = new Choices();
            //sList.Add(new String[] { "I WANT TO SEARCH SOMETHING", "battery life", "take a pic", "todays weather", "mail", "how is my day", "lockscreen", "how is my day jarvis", "nine", "hello", "hero", "open  vlc", "pause", "slowmo", "my todays luck", "fastforward", "close browser", "close tab", "start youtube", "start instagram", "start whatsapp", "play", "restart", "fuck off", "date", "time", "i", "you", "exit", "jarvis dady is home", "search for", "cars", "start facebook", "describe your self", "search for cars" });
            //Grammar g = new Grammar(new GrammarBuilder(sList));
            sRecognize.LoadGrammar(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"D:\Workspace\visual studio\projects\jarvis1\grammer.txt")))));
            
            try
            {
                sRecognize.RequestRecognizerUpdate();
               // sRecognize.LoadGrammar(g);
                sRecognize.SpeechRecognized += sRegonige_SpeechRecognized;
                sRecognize.SetInputToDefaultAudioDevice();
                sRecognize.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                return;
            }
        }

        Form3 f = new Form3();
        Form5 d = new Form5();
        private void sRegonige_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            jarvis.Volume = SoundTrackBar.Value;
             if (e.Result.Text.Contains("jarvis"))
            {

                string choice = e.Result.Text.Replace("jarvis ", "");
                Random rdm = new Random();
                int subChoice = rdm.Next(3) + 1;
                SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='D:\Workspace\visual studio\projects\jarvis1\Database1.mdf';Integrated Security=True"); 
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("select * from datatable where command = '" + choice + "'");
                 cmd1.Connection = conn ;
                 //SqlDataAdapter da = new SqlDataAdapter(cmd1);
                 //DataTable dt = new DataTable();
                 //da.Fill(dt);
                 //textBox1.Text = dt.Rows[0]["reply"].ToString();
                 //string reply = dt.Rows[0]["reply"].ToString();
                 SqlDataReader da = cmd1.ExecuteReader();
                 while(da.Read())
                 {
                     textBox1.Text = da["reply"].ToString();
                 }

                 switch (choice)
                {
                    case "hello":
                        jarvis.Speak("Hello sir");
                        break;
                    case "tell me about yourself":
                    case "describe your self":
                    case "how are you":
                        jarvis.Speak("i am jarvis a personal assistant for you with memeory 8gb...speed 2.2 giga hertz quad core i5 second generation intel processor...made by  genius REETAYSH ");
                        break;
                    case "command":
                    case "show me your command file":
                        jarvis.Speak("OPENING COMMAND FILE SIR ");
                        f.Show();
                        break;
                    case"close command tab":
                    case"exit command tab":
                        jarvis.Speak("Closing tab sir");
                        f.Close();
                        break;
                    case "customize tab":
                    case"open customize tab":
                    case"start customize tab":
                        jarvis.Speak("openning tab sir");
                        d.Show();
                        break;
                    case"close customize tab":
                    case"exit customize tab":
                        jarvis.Speak("closing tab sir");
                        d.Close();
                        break;
                    case "lock":
                    case "lock my pc":
                    case "lockscreen":
                        switch (subChoice)
                        {
                            case 1: jarvis.Speak("Locking Windows For You Sir"); break;
                            case 2: jarvis.Speak("Windows is getting Locked Sir"); break;
                            case 3: jarvis.Speak("following your command sir...Locking Windows"); break;
                        }
                        System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll, LockWorkStation");
                        break;
                    case "exit":
                    case "fuck of":
                    case "quit":
                    case "close":
                        switch (subChoice)
                        {
                            case 1: jarvis.Speak("Saving Files...Closing Application...GoodBye Sir"); break;
                            case 2: jarvis.Speak("Thanks you Sir"); break;
                            case 3: jarvis.Speak("As per Your Command Sir"); break;
                        }
                        thb.Abort();
                        Application.Exit();
                        break;
                    /*
                     * talking with jarvis
                     */
                    case "whats up":
                        jarvis.Speak("I AM HEALTHY AND SOUND SIR...how are you sir");
                        break;
                    case "are u there":
                        jarvis.Speak("always there for you sir");
                        break;
                    case "dady is home":
                        jarvis.Speak("welcome sir...how can i help you");
                        break;
                    case "what is god":
                    case "who is god":
                    case "what is your religion":
                        jarvis.Speak("I AM NOT MADE TO ANSWER SUCH QUESTION SIR");
                        break;
                    case "where do you live":
                        jarvis.Speak("i live in " + SystemInformation.UserDomainName.ToString() + "sir ");
                        break;
                    case "what is life":
                        jarvis.Speak("the condition that distinguishes animals and plants from inorganic matter, including the capacity for growth, reproduction, functional activity, and continual change preceding death.. is life sir");
                        break;
                    case "who created you":
                        jarvis.Speak("I am created bt REETASH");
                        break;
                    case "are you a boy or a girl":
                    case "are you a man or a woman":
                    case "are you a man":
                    case "are you a girl":
                    case "are you a transgender":
                    case "are you straight":
                    case "are you a machine":
                        jarvis.Speak("I am not programmed to answer such question..but if you still want to know i can tell you that i am a artificial intelligence program");
                        break;
                    /*
                     * SOCIAL MEDIA AND WEB SERVICES
                     */
                    case "start facebook":
                    case "open facebook":
                        switch (subChoice)
                        {
                            case 1: jarvis.Speak("STARTING FACEBOOK FOR YOU SIR"); break;
                            case 2: jarvis.Speak("OPENING FACEBOOK FOR YOU SIR"); break;
                            case 3: jarvis.Speak("AS PER YOUR COMMAND SIR"); break;
                        }
                        url = "https://www.facebook.com/r1735h.519gh";
                        System.Diagnostics.Process.Start(url);
                        break;
                    case "start whatsapp":
                    case "open whatsapp":
                        switch (subChoice)
                        {
                            case 1: jarvis.Speak("STARTING whatsapp FOR YOU SIR"); break;
                            case 2: jarvis.Speak("OPENING whatsapp FOR YOU SIR"); break;
                            case 3: jarvis.Speak("AS PER YOUR COMMAND SIR"); break;
                        }
                        url = "https://web.whatsapp.com/";
                        System.Diagnostics.Process.Start(url);
                        break;
                    case "start instagram":
                    case "open instagram":
                        switch (subChoice)
                        {
                            case 1: jarvis.Speak("STARTING instagram FOR YOU SIR"); break;
                            case 2: jarvis.Speak("OPENING instagram FOR YOU SIR"); break;
                            case 3: jarvis.Speak("AS PER YOUR COMMAND SIR"); break;
                        }
                        url = "https://www.instagram.com/iamritesh_singh/";
                        System.Diagnostics.Process.Start(url);
                        break;
                    case "start youtube":
                    case "open youtube":
                        switch (subChoice)
                        {
                            case 1: jarvis.Speak("STARTING YOUTUBE FOR YOU SIR"); break;
                            case 2: jarvis.Speak("OPENING YOUTUBE FOR YOU SIR"); break;
                            case 3: jarvis.Speak("AS PER YOUR COMMAND SIR"); break;
                        }
                        url = "https://www.youtube.com/?gl=IN";
                        System.Diagnostics.Process.Start(url);
                        break;
                    case"live score":
                    case "start twitter":
                    case "open twitter":
                        switch (subChoice)
                        {
                            case 1: jarvis.Speak("STARTING twitter FOR YOU SIR"); break;
                            case 2: jarvis.Speak("OPENING twitter FOR YOU SIR"); break;
                            case 3: jarvis.Speak("AS PER YOUR COMMAND SIR"); break;
                        }
                        url = "http://twitter.com/";
                        System.Diagnostics.Process.Start(url);
                        break;
                    case "start cricbuzz":
                    case "open cricbuzz":
                        switch (subChoice)
                        {
                            case 1: jarvis.Speak("STARTING cricbuzz FOR YOU SIR"); break;
                            case 2: jarvis.Speak("OPENING cricbuzz FOR YOU SIR"); break;
                            case 3: jarvis.Speak("AS PER YOUR COMMAND SIR"); break;
                        }
                        url = "http://www.cricbuzz.com/";
                        System.Diagnostics.Process.Start(url);
                        break;
                    case "start gmail":
                    case "open gmail":
                    case "check mail":
                        switch (subChoice)
                        {
                            case 1: jarvis.Speak("STARTING gmail FOR YOU SIR"); break;
                            case 2: jarvis.Speak("OPENING gmail FOR YOU SIR"); break;
                            case 3: jarvis.Speak("AS PER YOUR COMMAND SIR"); break;
                        }
                        url = "https://accounts.google.com";
                        System.Diagnostics.Process.Start(url);
                        break;
                    case "horoscope":
                    case "how is my day":
                        jarvis.Speak("Calculating YOUR HOROSCOPE SIR");
                        HtmlWeb web = new HtmlWeb();
                        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                        doc = web.Load("http://www.ganeshaspeaks.com/leo/leo-daily-horoscope.action");
                        HtmlNode ratenode = doc.DocumentNode.SelectSingleNode("//div[@class='block100 box-pading']");
                        string rate = ratenode.InnerText;
                        rate = rate.Replace("Today", "");
                        rate = rate.Replace(DateTime.UtcNow.Date.ToShortDateString(),"");
                        rate = rate.Replace("()","");
                        //rate = rate.Replace(),"");
                        jarvis.Speak(rate);
                        break;
                    case "weather":
                    case "today's weather":
                    case "weather report":
                        jarvis.Speak("CHECKING WEATHER");
                        HtmlWeb web1 = new HtmlWeb();
                        HtmlAgilityPack.HtmlDocument doc1 = new HtmlAgilityPack.HtmlDocument();
                        doc = web1.Load("http://www.skymetweather.com/forecast/weather/india/odisha/khordha/bhubaneswar");
                        HtmlNode ratenode1 = doc.DocumentNode.SelectSingleNode("//div[@class='fcBox']");
                        string rate1 = " " + ratenode1.InnerText;
                        jarvis.Speak(rate1);
                        break;

                    case "start chrome":
                    case "open chrome":
                        jarvis.Speak("Opening chrome for u sir");
                        System.Diagnostics.Process.Start("chrome.exe", "http://google.com/ncr");
                        break;
                    case "shift tab":
                        SendKeys.Send("^+{tab}");
                        break;
                    case "go incognito":
                        SendKeys.Send("^+{n}");
                        jarvis.Speak("You have gone incognito.. sir");
                        break;
                    case "close tab":
                        SendKeys.Send("^{w}");
                        break;
                    case "new tab":
                        SendKeys.Send("^{t}");
                        break;
                    case "close browser":
                        switch (subChoice)
                        {
                            case 1: jarvis.Speak("CLosing browser sir"); break;
                            default: jarvis.Speak("As PER YOUR COMMAND SIR"); break;
                        }
                        Process[] AllProcesses = Process.GetProcesses();
                        foreach (var process in AllProcesses)
                        {
                            if (process.MainWindowTitle != "")
                            {
                                string s = process.ProcessName.ToLower();
                                if (s == "iexplore" || s == "iexplorer" || s == "chrome" || s == "firefox")
                                    process.Kill();
                            }
                        }
                        break;
                    /*
                     * search engine
                     */
                    case "something":
                    case "i want to search something":
                        jarvis.Speak("what do you want to search sir?");
                        try
                        {
                            condition = 1;
                            sRecognize.SpeechRecognized += search;
                        }
                        catch
                        {
                            return;
                        }
                        break;
                    case "i want to know meaning of a word":
                    case "meaning":
                        jarvis.Speak("TELL ME THE WORD I WILL TELL YOU THE MEANING SIR");
                        try
                        {
                            condition = 5;
                            sRecognize.SpeechRecognized += search;
                        }
                        catch
                        {
                            return;
                        }
                        break;
                    case "shopping":
                    case "i want to do shopping":
                        jarvis.Speak("were do you want to do shopping sir?");
                        try
                        {
                            condition = 2;
                            sRecognize.SpeechRecognized += search;
                        }
                        catch
                        {
                            return;
                        }
                        break;
                    /*
                     * Set reminder
                     */
                    case "set reminder":
                    case "reminder":
                    case "i want to set a reminder":
                        condition = 3;
                        jarvis.Speak("PLzz TELL ME THE REMINDER SIR...I WILL NOTIFY YOU");
                        try
                        {
                            condition = 3;
                            sRecognize.SpeechRecognized += search;
                        }
                        catch
                        {
                            return;
                        }
                        break;
                    /*
                    / VIDEO PLAYER COMMAND
                     */
                    case "open vlc":
                    case "open video player":
                    case "start vlc":
                    case "start video player":
                        switch (subChoice)
                        {
                            case 1: jarvis.Speak("Opening VLC FOR U SIR"); break;
                            case 2: jarvis.Speak("Starting VLC FOR U SIR"); break;
                            case 3: jarvis.Speak("FOLLOWING YOUR COMMAND SIR"); break;
                        }
                        System.Diagnostics.Process.Start("E:\\movies\\hollywood\\Iron Man (2008) BRRip 720p x264 [Dual Audio] [Hindi+English]--prisak~~{HKRG}\\ironMan.mkv");
                        break;
                    case "pause":
                    case "play":
                        SendKeys.Send(" ");
                        break;
                    case "forward":
                        SendKeys.Send("^{RIGHT}");
                        break;
                    case "backward":
                        SendKeys.Send("^{LEFT}");
                        break;
                    case "slowmo":
                        SendKeys.Send("-");
                        break;
                    case "fastfoeward":
                        SendKeys.Send("+");
                        break;
                    case "volume up":
                    case "increase volume":
                        SendKeys.Send("^{UP}");
                        break;
                    case "volume down":
                    case "decrease volume":
                        SendKeys.Send("^{DOWN}");
                        break;
                    case "close vlc":
                    case "close video player":
                        switch (subChoice)
                        {
                            case 1: jarvis.Speak("CLosing vlc sir"); break;
                            default: jarvis.Speak("As PER YOUR COMMAND SIR"); break;
                        }
                        Process[] video = Process.GetProcesses();
                        foreach (var process in video)
                        {
                            if (process.MainWindowTitle != "")
                            {
                                string s = process.ProcessName.ToLower();
                                if (s == "vlc")
                                    process.Kill();
                            }
                        }
                        break;

                    /*
                     * calculator
                     */
                    case "calculator":
                    case "start calculator":
                    case "open calculator":
                    case "i want to do some calculation":
                        jarvis.Speak("openning calculator sir");
                        System.Diagnostics.Process.Start(@"C:\Windows\System32\calc.exe");
                        break;
                    case "zero":
                        SendKeys.Send("0");
                        break;
                    case "one":
                        SendKeys.Send("1");
                        break;
                    case "two":
                        SendKeys.Send("2");
                        break;
                    case "three":
                        SendKeys.Send("3");
                        break;
                    case "four":
                        SendKeys.Send("4");
                        break;
                    case "five":
                        SendKeys.Send("5");
                        break;
                    case "six":
                        SendKeys.Send("6");
                        break;
                    case "seven":
                        SendKeys.Send("7");
                        break;
                    case "eight":
                        SendKeys.Send("8");
                        break;
                    case "nine":
                        SendKeys.Send("9");
                        break;
                    case "divide":
                        SendKeys.Send("{DIVIDE}");
                        break;
                    case "multiply":
                        SendKeys.Send("{MULTILPLY}");
                        break;
                    case "subtract":
                        SendKeys.Send("{-}");
                        break;
                    case "add":
                    case "addition":
                        SendKeys.Send("{+}");
                        break;
                    case "equal":
                        SendKeys.Send("{ENTER}");
                        break;
                    case "close calculator":
                    case"exit calculator":
                        Process[] calc = Process.GetProcesses();
                        foreach (var process in calc)
                        {
                            if (process.MainWindowTitle != "")
                            {
                                string s = process.ProcessName.ToLower();
                                if (s == "calc")
                                    process.Kill();
                            }
                        }
                        break;
                        /*
                         * cmd
                         */
                    case "start cmd":
                    case"open cmd":
                    case"cmd":
                        jarvis.Speak("OPENNING CMD FOR YOU SIR");
                         System.Diagnostics.Process.Start(@"C:\Windows\System32\cmd.exe");
                        break;
                    case"close cmd":
                    case"exit cmd":
                        Process[] cmd = Process.GetProcesses();
                        foreach (var process in cmd)
                        {
                            if (process.MainWindowTitle != "")
                            {
                                string s = process.ProcessName.ToLower();
                                if (s == "cmd")
                                    process.Kill();
                            }
                        }
                        break;
                    //location
                    case "location":
                    case "tell me my location":
                        //jarvis.Speak("Finding your location");
                        //HtmlWeb web2 = new HtmlWeb();
                        //HtmlAgilityPack.HtmlDocument doc2 = new HtmlAgilityPack.HtmlDocument();
                        //doc2 = web2.Load("https://www.iplocation.net/");
                        //HtmlNode ratenode2 = doc2.DocumentNode.SelectSingleNode("//table[@class='table_dark_green']");
                        //string rate2 = " " + ratenode2.InnerText;
                        //jarvis.Speak(rate2);
                        jarvis.Speak("Your location is jamshedpur , jharkhand ");
                        break;
                    // default
                    case "joke":
                    case "bore":
                    case "i am getting bored":
                    default:
                        jarvis.Speak("if u are getting bored i can tell u a joke sir");
                        try
                        {
                            condition = 4;
                            sRecognize.SpeechRecognized += search;
                        }
                        catch
                        {
                            return;
                        }
                        break;

                }
            }
           
        }

        private void search(object sender, SpeechRecognizedEventArgs e)
        {
            if (!e.Result.Text.Contains("jarvis"))
            {
                switch(condition)
                { 
                    case 1 : //google search
                        condition = 0 ;
                        url = "https://www.google.com/search?q=" + e.Result.Text;
                        System.Diagnostics.Process.Start(url);
                        break;
                    case 2: // shopping site
                        condition = 0;
                        switch(e.Result.Text)
                        {
                            case "amazon":
                                url = "https://http://www.amazon.in/";
                                System.Diagnostics.Process.Start(url);
                                break;
                            case "flipkart":
                                url = "https://www.flipkart.com/";
                                System.Diagnostics.Process.Start(url);
                                break;
                            case "snadeal":
                                 url = "https://www.snapdeal.com";
                                System.Diagnostics.Process.Start(url);
                                break;
                        }
                        break;
                    case 3 : //reminder
                        condition = 0;
                        jarvis.Speak("Reminder saved sir...i will notify you");
                        break;
                    case 4 : //joke
                        condition = 0;
                        switch(e.Result.Text)
                        {
                            case "yes":
                            case "ok":
                                jarvis.Speak("ok sir");
                                Thread.Sleep(1000);
                                HtmlWeb web = new HtmlWeb();
                                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                                doc = web.Load("http://www.laughfactory.com/jokes/joke-of-the-day");
                                HtmlNode ratenode = doc.DocumentNode.SelectSingleNode("//div[@class='joke-msg']");
                                string rate = ratenode.InnerText;
                                rate = rate.Replace("Today's Joke", "by");
                                jarvis.Speak(rate);
                                break;
                            case "no":
                            case "shut up":
                                jarvis.Speak("okay sir...tell me if u need me sir ");
                                break;
                        }
                        break;
                    case 5: //meaning
                        condition = 0;
                        HtmlWeb web1 = new HtmlWeb();
                        HtmlAgilityPack.HtmlDocument doc1 = new HtmlAgilityPack.HtmlDocument();
                        doc1 = web1.Load("https://en.oxforddictionaries.com/definition/" + e.Result.Text);
                        HtmlNode ratenode1 = doc1.DocumentNode.SelectSingleNode("//div[@class='trg']//span[@class='ind']");
                        string rate1 = ratenode1.InnerText;
                        jarvis.Speak(rate1);
                        break;
                   
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            thb.Abort();
            sRecognize.Dispose();
           // sRecognize.RecognizeAsyncStop();
            button6.Enabled = false;
            button5.Enabled = true;
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
