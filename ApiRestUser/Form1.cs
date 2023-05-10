using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

using Newtonsoft.Json;

namespace ApiRestUser
{
    public partial class Form1 : Form
    {
        HttpClient client = new HttpClient();
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void Mostrar()
        {
            try
            {
                //Para uso del Servidor "Servicios Digitales Plus"
                String url = textBox6.Text;

                String line = GetHttp(url); //Se obtienen los mensajes
                //richTextBox1.Text = line;

                //Conversión de una cadena JSON en un objeto
                Root obj = JsonConvert.DeserializeObject<Root>(line);

                String cadux = "";

                    if (obj != null)
                    {
                        for (int i = 0; i < obj.Alumnos.Count; i++)
                        {

                                //Para mostrar el mensaje que se está validando
                                cadux += "|  " + obj.Alumnos[i].id + "     |   ";

                                //Para mostrar la fecha del envío
                                cadux += obj.Alumnos[i].nombre + "   |   ";
                                cadux += obj.Alumnos[i].apellidoP + "   |   ";
                                cadux += obj.Alumnos[i].apellidoM + "   |   ";
                                cadux += obj.Alumnos[i].grado + "  | \n";

                            //Con esto se empiezan a leer todos los mensajes
                            //richTextBox1.Text += obj.Chat[i].descripcion + "\n";
                    }
                    //richTextBox1.Rtf = cadux;
                    richTextBox1.Text = cadux;
                    }
                }
            catch
            {

            }
        }
        public String GetHttp(String url)
        {
            WebRequest wr = WebRequest.Create(url);

            WebResponse wre = wr.GetResponse();
            StreamReader sr = new StreamReader(wre.GetResponseStream());

            return sr.ReadToEnd().Trim();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Mostrar();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MostrarId();
        }
        private void MostrarId()
        {
            try
            {
                //Para uso del Servidor "Servicios Digitales Plus"
                String url = textBox6.Text + "?id=" + textBox7.Text;

                String line = GetHttp(url); //Se obtienen los mensajes
                //richTextBox1.Text = line;

                //Conversión de una cadena JSON en un objeto
                Root obj = JsonConvert.DeserializeObject<Root>(line);

                String cadux = "";

                if (obj != null)
                {
                    for (int i = 0; i < obj.Alumnos.Count; i++)
                    {

                        //Para mostrar el mensaje que se está validando
                        cadux += "|  " + obj.Alumnos[i].id + "     |   ";

                        //Para mostrar la fecha del envío
                        cadux += obj.Alumnos[i].nombre + "   |   ";
                        cadux += obj.Alumnos[i].apellidoP + "   |   ";
                        cadux += obj.Alumnos[i].apellidoM + "   |   ";
                        cadux += obj.Alumnos[i].grado + "  | \n";

                        //Con esto se empiezan a leer todos los mensajes
                        //richTextBox1.Text += obj.Chat[i].descripcion + "\n";
                    }
                    //richTextBox1.Rtf = cadux;
                    richTextBox1.Text = cadux;
                }
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Insertar();
        }

        public void Insertar()
        {
            if(textBox1.Text == ""|| textBox2.Text =="" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Faltan Datos");
            }
            else
            {
                string url = "http://localhost:8080/Examen3/service.php";
                var request = (HttpWebRequest)WebRequest.Create(url);

                var postData = "id=" + Uri.EscapeDataString(textBox1.Text);
                postData += "&nombre=" + Uri.EscapeDataString(textBox2.Text);
                postData += "&apellidoP=" + Uri.EscapeDataString(textBox3.Text);
                postData += "&apellidoM=" + Uri.EscapeDataString(textBox4.Text);
                postData += "&grado=" + Uri.EscapeDataString(textBox5.Text);
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Actualizar();
        }

        private void Actualizar()
        {
           /* if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Faltan Datos");
            }
            else
            {*/
                // Crea la URL del endpoint
                string url = "http://localhost:8080/Examen3/service.php?";

                // Crea un objeto que contiene los datos a actualizar en el API RESTful
                string data = "id=2222222&nombre=222222&apellidoP=22222221&apellidoM=2222221&grado=22222221";

                // Convierte los datos en un arreglo de bytes
                byte[] postData = Encoding.UTF8.GetBytes(data);

                // Crea una instancia de la clase HttpWebRequest
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                // Configura la solicitud para utilizar el método PUT
                request.Method = "PUT";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;

                // Escribe los datos en la solicitud
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                }

                // Obtiene la respuesta del servidor
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Verifica si la solicitud se completó correctamente
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // La solicitud se completó correctamente
                    }
                    else
                    {
                        // La solicitud no se completó correctamente
                    }
                }


           // }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Eliminar();
        }

        private void Eliminar()
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("Faltan Datos");
            }
            else
            {
                string url = "http://localhost:8080/Examen3/service.php?id=" + textBox1.Text;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "DELETE";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
            }
        }
    }

    public class Alumnos
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string apellidoP { get; set; }
        public string apellidoM { get; set; }
        public string grado { get; set; }
    }

    public class Root
    {
        public List<Alumnos> Alumnos{ get; set; }
    }
}
