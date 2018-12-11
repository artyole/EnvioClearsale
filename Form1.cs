using ClearsaleCadastro.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ClearsaleCadastro
{
    public partial class Form1 : Form
    {
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var conn in connections)
            {
                var total = 0;
                total = ListarTotal(conn.Value);
                var offset = 0;
                var limit = 1000;

                for (int i = 0; i < total; i = i + limit)
                {
                    List<SendDataAccountRequest> clientes = ListarClientes(conn.Value, limit, offset);

                    int x = 1;
                    pb_status.Value = 0;
                    
                    string url = $"http://api-track.{conn.Key}.com.br/clearsale/sendDataAccountCreateAsync";

                    foreach (var cliente in clientes)
                    {
                        EnviarContasClearSale(cliente, url);
                        pb_status.Value = (x * 100) / clientes.Count;
                        x++;
                    }

                    offset = offset + limit;

                    pb_status.Value = 100;
                }
            }
        }

        private string GerarToken()
        {
            byte[] bytes = Encoding.GetEncoding(28591).GetBytes(DateTime.Now.ToString());
            string tokenRequest = System.Convert.ToBase64String(bytes);
            return tokenRequest;
        }

        public readonly Dictionary<string, string> connections = DbHelper.ListConnectionString();

        public Form1()
        {
            InitializeComponent();
        }

        public void EnviarContasClearSale(SendDataAccountRequest cliente, string url)
        {
            WebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Token", GerarToken());

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(cliente);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
 
            try
            {
                WebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
            }

        }

        public List<SendDataAccountRequest> ListarClientes(string conn, int limit, int offset)
        {
            List<SendDataAccountRequest> retorno = new List<SendDataAccountRequest>();
            ClienteRepository clienteRepository = new ClienteRepository(conn);
            retorno = clienteRepository.List(offset, limit);


            return retorno;
        }

        public int ListarTotal(string conn)
        {
            ClienteRepository clienteRepository = new ClienteRepository(conn);
            return clienteRepository.CountTotal();
        }
    }
}
