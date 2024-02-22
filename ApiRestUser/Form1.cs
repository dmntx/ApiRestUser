using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Microsoft.Win32;

namespace ApiRestUser
{
    public partial class Form1 : Form
    {
        HttpClient client = new HttpClient();
        string urlIn = "http://localhost:8000/api/v1/newUser/Practicante";
        String urlOut = "http://localhost:8000/api/v1/modUser/Practicante/1";
        String showUser = "http://localhost:8000/api/v1/obtenerUser/Practicante/";
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        public String GetHttp(String url)
        {
            WebRequest wr = WebRequest.Create(url);

            WebResponse wre = wr.GetResponse();
            StreamReader sr = new StreamReader(wre.GetResponseStream());

            return sr.ReadToEnd().Trim();
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            string apiUrl = "http://localhost:8000/api/v1/obtenerUser/Practicante/" + textBox7.Text;

            try
            {
                List<Practicante> jsonResponse = await GetApiResponse(apiUrl);
                string[] text = new string[11];
                // Aquí puedes trabajar con los datos, por ejemplo, mostrarlos en un cuadro de texto
                //textBox1.Text = jsonResponse[0];
                foreach (var practicante in jsonResponse)
                {
                    textBox1.Text = Convert.ToString(DateTime.Now);
                    textBox2.Text = practicante.nombre_completo;
                    textBox3.Text = practicante.carrera;
                    textBox4.Text = practicante.hora_llegada;
                    textBox5.Text = practicante.hora_salida;
                    text[0] = practicante.id;
                    text[1] = practicante.nombre_completo;
                    text[2] = practicante.carrera;
                    text[3] = practicante.codigo;
                    text[4] = practicante.fecha;
                    text[5] = practicante.hora_llegada;
                    text[6] = practicante.hora_salida;
                    text[7] = practicante.horas_trabajadas;
                    text[8] = practicante.minutos_trabajados;
                    text[9] = practicante.total_horas;
                    text[10] = practicante.horas_extras;

                }
                List<PracticanteSimple> practicantesSimples = SeleccionarDatos(jsonResponse);
                await EnviarDatos(text);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        static async Task<List<Practicante>> GetApiResponse(string apiUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Practicante>>(jsonResponse);
                }
                else
                {
                    throw new HttpRequestException($"La solicitud al API falló con el código de estado: {response.StatusCode}");
                }
            }
        }

        private List<PracticanteSimple> SeleccionarDatos(List<Practicante> practicantes)
        {
            // Crear una lista de PracticanteSimple con solo los campos necesarios
            List<PracticanteSimple> practicantesSimples = new List<PracticanteSimple>();
            foreach (var practicante in practicantes)
            {
                practicantesSimples.Add(new PracticanteSimple
                {
                    nombre_completo = practicante.nombre_completo,
                    carrera = practicante.carrera,
                    codigo = Convert.ToString(DateTime.Now),
                    fecha = practicante.fecha,
                    hora_llegada = Convert.ToString(DateTime.Now),
                    hora_salida = Convert.ToString(DateTime.Now),
                    horas_trabajadas = "0",
                    minutos_trabajados = "0",
                    total_horas = "0",
                    horas_extras = "0"
                    
                    // Agregar otros campos necesarios
                });
            }
            return practicantesSimples;
        }

        static async Task EnviarDatos(string[] practicantes)
        {
                // Crear el contenido del formulario
                var formData = new MultipartFormDataContent();
            DateTime horaActual = DateTime.Now;
            DateTime fechaActual = DateTime.Now;
            string hora = horaActual.ToString("HH:mm:ss");
            string fecha = fechaActual.ToString("yyyy-MM-dd");
            // Agregar campos al formulario
            formData.Add(new StringContent(practicantes[1], Encoding.UTF8), "nombre_completo");
            formData.Add(new StringContent(practicantes[2], Encoding.UTF8), "carrera");
            formData.Add(new StringContent(practicantes[3], Encoding.UTF8), "codigo");
            formData.Add(new StringContent(fecha, Encoding.UTF8), "fecha");
            formData.Add(new StringContent(hora, Encoding.UTF8), "hora_llegada");
            formData.Add(new StringContent(hora, Encoding.UTF8), "hora_salida");
                
            formData.Add(new StringContent(practicantes[7], Encoding.UTF8), "horas_trabajadas");
            formData.Add(new StringContent(practicantes[8], Encoding.UTF8), "minutos_trabajados");
            formData.Add(new StringContent(practicantes[9], Encoding.UTF8), "total_horas");
            formData.Add(new StringContent(practicantes[10], Encoding.UTF8), "horas_extras");

                // Opcionalmente, agregar archivos al formulario
                //formData.Add(new ByteArrayContent(File.ReadAllBytes("archivo.txt")), "archivo", "archivo.txt");

                // Crear el cliente HTTP
                using (var client = new HttpClient())
                {
                    try
                    {
                        // Enviar la solicitud POST a la URL de destino
                        HttpResponseMessage response = await client.PostAsync("http://localhost:8000/api/v1/newUser/Practicante", formData);

                        // Leer la respuesta
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Imprimir la respuesta
                        Console.WriteLine(responseBody);

                        // Verificar si la solicitud fue exitosa
                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("La solicitud fue exitosa.");
                        }
                        else
                        {
                            Console.WriteLine($"La solicitud falló con el código de estado: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
 
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            string url = "http://localhost:8000/api/v1/modUser/Practicante/1";
            DateTime horaActual = DateTime.Now;
            string hora = horaActual.ToString("HH:mm:ss");
            // Parámetros a enviar
            var parametros = new Dictionary<string, string>
            {
                { "codigo", textBox7.Text },
                { "hora_salida", hora }
            };

            // Crear el cliente HTTP
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Crear el contenido de la solicitud con los parámetros
                    var content = new FormUrlEncodedContent(parametros);

                    // Concatenar los parámetros a la URL (opcional)
                    url += "?codigo="+textBox7.Text+"&hora_salida=" + hora +" ";

                    // Realizar la solicitud PUT
                    HttpResponseMessage response = await client.PutAsync(url, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("La solicitud PUT fue exitosa.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"La solicitud PUT falló con el código de estado: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            }

        private void button2_Click(object sender, EventArgs e)
        {
           // Insertar();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //Actualizar();
        }
        
    }

    public class Practicante
    {
        public string id { get; set; }
        public string nombre_completo { get; set; }
        public string carrera { get; set; }
        public string codigo { get; set; }
        public string fecha { get; set; }
        public string hora_llegada { get; set; }
        public string hora_salida { get; set; }
        public string horas_trabajadas { get; set; }
        public string minutos_trabajados { get; set; }
        public string total_horas { get; set; }
        public string horas_extras { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
    public class PracticanteSimple
    {
        public string nombre_completo { get; set; }
        public string carrera { get; set; }
        public string codigo { get; set; }
        public string fecha { get; set; }
        public string hora_llegada { get; set; }
        public string hora_salida { get; set; }
        public string horas_trabajadas { get; set; }
        public string minutos_trabajados { get; set; }
        public string total_horas { get; set; }
        public string horas_extras { get; set; }
    }

    public class Root
    {
        public List<Practicante> Practicante{ get; set; }
    }
}
