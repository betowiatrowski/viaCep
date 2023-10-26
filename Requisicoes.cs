using LogControleAplicacoes;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace viaCep
{
    public class Requisicoes
    {
        public Retorno BuscaCep(int cep)
        {
            Retorno retorno = new Retorno();
            string status = "";
            string url = $"https://viacep.com.br/ws/{cep}/json/";
            var client = new RestClient(url);
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var request = new RestRequest(Method.GET);
                var response = client.Execute(request);
                status = response.StatusCode.ToString();

                JsonDeserializer deserializer = new JsonDeserializer();
                retorno = deserializer.Deserialize<Retorno>(response);

                retorno.logradouro = RemoverAcentos(retorno.logradouro);
                retorno.localidade = RemoverAcentos(retorno.localidade);
                retorno.complemento = RemoverAcentos(retorno.complemento);
                retorno.bairro = RemoverAcentos(retorno.bairro);                
            }
            catch (Exception ex)
            {
                LogErroAplicacao.Log.Insere(ex, $"BuscaCep");
                Logger.Insere(81, ex);
            }
            return retorno;
        }
        static string RemoverAcentos(string texto)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(texto.ToUpper());
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
