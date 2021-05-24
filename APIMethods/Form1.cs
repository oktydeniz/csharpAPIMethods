using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net; 
using System.IO;
using System.Net.Http;
using System.Text.Json;

namespace APIMethods
{
    public partial class Form1 : Form
    {

        String login = String.Format("http://kavramatik.com/api/login");
        String register = String.Format("http://kavramatik.com/api/register");
        String updateScore = String.Format("http://kavramatik.com/api/setScore");
        String formatPassword = String.Format("http://kavramatik.com/api/formatPassword");
       
       
        public Form1()
        {
            InitializeComponent();
        }

	//Giriş Yap
        async private void button1_Click(object sender, EventArgs e)
        {
            String mail = textBox1.Text.ToString();
            String pssw = textBox2.Text.ToString();

            var values = new Dictionary<String, String>
            {
                {"user_email",mail },
                { "user_password",pssw}
            };
            var data = new FormUrlEncodedContent(values);
            using var client = new HttpClient();
            var response = await client.PostAsync(login, data);
            var result = response.Content.ReadAsStringAsync().Result;
            richTextBox1.Text = result.ToString();
          
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

	//Kayıt Ol
       async private void button2_Click(object sender, EventArgs e)
        {
            String mail = textBox3.Text.ToString(); 
            String name = textBox4.Text.ToString();
            String password = textBox5.Text.ToString();
            String passwordConfirm = textBox6.Text.ToString();

            if(password.Trim() == passwordConfirm.Trim())
            {
                    var values = new Dictionary<String, String>
                    {
                        {"user_email",mail },
                        { "user_password",passwordConfirm},
                        {"user_name",name }
                    };
                var data = new FormUrlEncodedContent(values);
                using var client = new HttpClient();
                var response = await client.PostAsync(register, data);
                var result = response.Content.ReadAsStringAsync().Result;
                richTextBox2.Text = result.ToString();

            }

        }

	//Yeni şifre alma (Değiştirme)
       async private void button3_Click(object sender, EventArgs e)
        {

            String mail = textBox7.Text.ToString();
            String code = textBox8.Text.ToString();
            String password = textBox11.Text.ToString();
            String passwordConfirm = textBox12.Text.ToString();

            if (password.Trim() == passwordConfirm.Trim())
            {
                var values = new Dictionary<String, String>
                    {
                        {"user_email",mail },
                        { "user_password",passwordConfirm},
                        {"verification",code }
                    };
                var data = new FormUrlEncodedContent(values);
                using var client = new HttpClient();
                var response = await client.PostAsync(formatPassword, data);
                var result = response.Content.ReadAsStringAsync().Result;
                richTextBox3.Text = result.ToString();

            }

          
        }
	// Profil Bilgilerini Getir
        async private void button4_Click(object sender, EventArgs e)
        {

            String mail = textBox9.Text.ToString();
           
            String showProfile = String.Format("http://kavramatik.com/api/showProfile?user_email=" + mail);
            var request = WebRequest.Create(showProfile);
            request.Method = "GET";

            using var webResponse = request.GetResponse();
            using var webStream = webResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();
            richTextBox4.Text = data.ToString();
        }

	// Şifre Sıfırlamak için mail atma
       async private void button5_Click(object sender, EventArgs e)
        {
            String mail = textBox10.Text.ToString();
            String sendMailForPassword = String.Format("http://kavramatik.com/api/sendMailForPassword?user_email=" + mail);
            var request = WebRequest.Create(sendMailForPassword);
            request.Method = "GET";

            using var webResponse = request.GetResponse();
            using var webStream = webResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();
            richTextBox5.Text = data.ToString();
        }

	//Puan güncleme
       async private void button6_Click(object sender, EventArgs e)
        {
            String mail = textBox14.Text.ToString();
            int newScore = Int32.Parse(textBox13.Text);

            Score scoreUser = new Score(mail, newScore);

           
            var json = JsonSerializer.Serialize(scoreUser);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

           
            using var client = new HttpClient();

            var response = await client.PostAsync(updateScore, data);

            string result = response.Content.ReadAsStringAsync().Result;
            richTextBox6.Text = result;
        }

	//Skor günclleme için skor sınıfı
        class Score
        {
            public String user_email { get; set; }
            public int score { get; set; }
            public Score(String user_email, int score)
            {
                this.user_email = user_email;
                this.score = score;
            }

        }
    }
}
