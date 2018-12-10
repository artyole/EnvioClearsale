using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ClearsaleCadastro
{
    public class ClienteRepository
    {
        private static int _offset = 0;
        private static int _limit = 0;

        private readonly string ConnectionString;
        
        private readonly string SqlListarEndereco = "SELECT * FROM ClienteEndereco INNER JOIN Endereco ON ClienteEndereco.IdEndereco = Endereco.IdEndereco WHERE IdCliente = @IdCliente";

        public ClienteRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public int CountTotal()
        {
            int total = 0;
            string SqlTotal = "SELECT COUNT(CLIENTE.IDCLIENTE) AS Total FROM CLIENTE(nolock) WHERE CLIENTE.FLAGATIVA = 1 AND EXISTS(SELECT TOP 1 * FROM  COMPRA WHERE COMPRA.IDCLIENTE = CLIENTE.IDCLIENTE AND COMPRA.Data >= DATEADD(YEAR, -5, GETDATE()))";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(SqlTotal, conn))
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;

                    try
                    {
                        using (DbDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                total = int.Parse(dr["Total"].ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }
            }

            return total;
        }

        public List<SendDataAccountRequest> List(int offset, int limit)
        {
            _offset = offset;
            _limit = limit;

            var clientes = new List<SendDataAccountRequest>(0);

            string SqlListarcliente =   " SELECT * " +
                                        " FROM CLIENTE (NOLOCK)" +
                                        " WHERE CLIENTE.FLAGATIVA = 1 " +
                                        "AND EXISTS (SELECT TOP 1 * FROM  COMPRA (NOLOCK) WHERE COMPRA.IdCliente = CLIENTE.IdCliente AND COMPRA.Data >= DATEADD(YEAR, -5, GETDATE()))" +
                                        " ORDER BY IdCliente OFFSET " + _offset + " ROWS FETCH NEXT " + _limit + " ROWS ONLY";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(SqlListarcliente, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    using (DbDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var cliente = new SendDataAccountRequest();
                            cliente.Code = dr["IdCliente"].ToString();
                            cliente.Date = dr["DataCriacao"].ToString();
                            cliente.SessionId = dr["SessionId"].ToString();
                            cliente.Name = dr["Nome"].ToString();
                            cliente.PersonDocument = dr["CpfCnpj"].ToString();
                            cliente.BirthDate = dr["DataNascimento"].ToString();
                            cliente.Gender = dr["Sexo"].ToString();
                            cliente.CompanyDocument = dr["CpfCnpj"].ToString();
                            cliente.Email = dr["Email"].ToString();
                            cliente.PasswordHash = dr["SenhaHash"].ToString();
                            cliente.OptinEmail = dr["FlagNews"].ToString();
                            cliente.OptinMobile = dr["FlagSmsNews"].ToString();

                            string telefoneCelular = dr["TelefoneCelular"].ToString();
                            string telefoneResidencial = dr["TelefoneResidencial"].ToString();
                            string telefoneComercial = dr["TelefoneComercial"].ToString();

                            cliente.Phones = new List<SendDataAccountPhones>();
                            if (!string.IsNullOrEmpty(telefoneCelular) && telefoneCelular.Length > 2)
                            {
                                SendDataAccountPhones telefone = new SendDataAccountPhones
                                {
                                    Ddi = "55",
                                    Ddd = telefoneCelular.Substring(0, 2),
                                    Number = telefoneCelular.Substring(2)
                                };
                                cliente.Phones.Add(telefone);
                            }

                            if (!string.IsNullOrEmpty(telefoneResidencial) && telefoneResidencial.Length > 2)
                            {
                                SendDataAccountPhones telefone = new SendDataAccountPhones
                                {
                                    Ddi = "55",
                                    Ddd = telefoneResidencial.Substring(0, 2),
                                    Number = telefoneResidencial.Substring(2)
                                };
                                cliente.Phones.Add(telefone);
                            }

                            if (!string.IsNullOrEmpty(telefoneComercial) && telefoneComercial.Length > 2)
                            {
                                SendDataAccountPhones telefone = new SendDataAccountPhones
                                {
                                    Ddi = "55",
                                    Ddd = telefoneComercial.Substring(0, 2),
                                    Number = telefoneComercial.Substring(2)
                                };
                                cliente.Phones.Add(telefone);
                            }

                            clientes.Add(cliente);
                        }
                    }

                }

                foreach (var cliente in clientes)
                {
                    using (SqlCommand cmd = new SqlCommand(SqlListarEndereco, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("IdCliente", cliente.Code);

                        cliente.Addresses = new List<SendDataAccountAddress>();

                        using (DbDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var endereco = new SendDataAccountAddress();
                                endereco.Name = dr["Nome"].ToString();
                                endereco.Street = dr["Rua"].ToString();
                                endereco.Number = dr["Numero"].ToString();
                                endereco.Complement = dr["Complemento"].ToString();
                                endereco.County = dr["Bairro"].ToString();
                                endereco.City = dr["Municipio"].ToString();
                                endereco.State = dr["Estado"].ToString();
                                endereco.Country = dr["Pais"].ToString();
                                endereco.ZipCode = dr["Cep"].ToString();
                                endereco.Reference = dr["PontoReferencia"].ToString();

                                cliente.Addresses.Add(endereco);
                            }
                        }

                    }
                }

            }
            return clientes;
        }
    }
}
