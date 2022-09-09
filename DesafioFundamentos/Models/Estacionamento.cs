using System.Text.RegularExpressions;

namespace DesafioFundamentos.Models
{
    /// <summary>
    /// Classe que representa um estacionamento em forma de "CRUD", com ela podemos adicionar veículos, 
    /// remover veículos, listar, calcular o preço a pagar baseado no valor de entrada e tempo de permanencia 
    /// no estacionamento.
    /// </summary>
    public class Estacionamento
    {
        private decimal precoInicial = 0;
        private decimal precoPorHora = 0;
        private List<string> veiculos = new List<string>();

        /// <summary>
        /// Construtor da classe estacionamento, para poder utilizar seus métodos, algumas informações
        /// iniciais devem ser passadas, como o preço inicial e o preço por hora.
        /// </summary>
        /// <param name="precoInicial">Indica quanto que deve pagar para o veículo adentrar no estacionamento.</param>
        /// <param name="precoPorHora">Indica qual valor será cobrado por hora que o veículo permaneceu no estacionamento.</param>
        public Estacionamento(decimal precoInicial, decimal precoPorHora)
        {
            this.precoInicial = precoInicial;
            this.precoPorHora = precoPorHora;
        }

        /// <summary>
        /// Adicionar um veículo ao estacionamento. Será verificado a validade
        /// da placa, sendo aceito o modelo antigo (AAA-1111) e novo (AAA1A11).
        /// </summary>
        public void AdicionarVeiculo()
        {
            Console.WriteLine("Digite a placa do veículo para estacionar:");
            string placa = Console.ReadLine();
            if (ConferePlaca(placa))
                veiculos.Add(placa);
            else
                Console.WriteLine("Placa inválida, por favor, adicione uma placa no modelo correto, como AAA-1111, ou AAA1A11");
        }

        /// <summary>
        /// Remove um veículo do estacionamento, realiza uma busca pela placa passada e analisa a viabilidade
        /// das horas no estacionamento.
        /// </summary>
        public void RemoverVeiculo()
        {
            Console.WriteLine("Digite a placa do veículo para remover:");

            string placa = Console.ReadLine();

            // Verifica se o veículo existe
            if (veiculos.Any(x => x.ToUpper() == placa.ToUpper()))
            {
                Int16 horas;

                do
                {
                    Console.WriteLine("Digite a quantidade de horas que o veículo permaneceu estacionado:");
                    horas = Convert.ToInt16(Console.ReadLine());
                    if (horas < 0)
                        Console.WriteLine("O valor de horas precisa ser positivo!");
                } while (horas < 0);

                decimal valorTotal = precoInicial + precoPorHora * horas;

                veiculos.Remove(placa);
                Console.WriteLine($"O veículo {placa} foi removido e o preço total foi de: R$ {valorTotal.ToString("0.00")}");
            }
            else
                Console.WriteLine("Desculpe, esse veículo não está estacionado aqui. Confira se digitou a placa corretamente");
        }

        /// <summary>
        /// Lista todos os veículos presentes no estacionamento.
        /// </summary>
        public void ListarVeiculos()
        {
            // Verifica se há veículos no estacionamento
            if (veiculos.Any())
            {
                Console.WriteLine("Os veículos estacionados são:");

                foreach(string veiculo in veiculos)
                {
                    Console.WriteLine(veiculo);
                }
            }
            else
            {
                Console.WriteLine("Não há veículos estacionados.");
            }
        }

        /// <summary>
        /// Confere a validez da placa, aceitando o modelo antigo (AAA-1111) e novo (AAA1A11).
        /// </summary>
        /// <param name="placa">Placa do veículo que deseja verificar.</param>
        /// <returns></returns>
        public bool ConferePlaca(string placa)
        {
            Regex padraoDePlacaAntigo = new Regex("^[A-Za-z]{3}-[0-9]{4}$");
            Regex padraoDePlacaNovo = new Regex("^[A-Za-z]{3}[0-9][A-Za-z][0-9]{2}");

            if (padraoDePlacaAntigo.IsMatch(placa) 
            || padraoDePlacaNovo.IsMatch(placa))
                return true;
            else
                return false;
        }
    }
}
