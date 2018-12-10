using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearsaleCadastro.Helpers
{
    public class DbHelper
    {
        public static Dictionary<string, string> ListConnectionString()
        {
            var connections = new Dictionary<string, string>();
            var connectionStrings = ConfigurationManager.ConnectionStrings;
            foreach (var connectionString in connectionStrings)
            {
                connections.Add(((ConnectionStringSettings)connectionString).Name, connectionString.ToString());
            }

            return connections;
        }
    }
}
