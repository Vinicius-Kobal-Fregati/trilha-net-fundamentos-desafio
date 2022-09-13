using System.Text.RegularExpressions;

namespace DesafioFundamentos.Models
{
    /// <summary>
    /// Classe que representa um estacionamento em forma de "CRUD", com ela podemos adicionar, 
    /// remover e listar veículos, calcular o preço a pagar baseado no valor de entrada e tempo de permanência 
    /// no estacionamento.
    /// </summary>
    public class Estacionamento
    {
        private decimal PrecoInicial = 0;
        private decimal PrecoPorHora = 0;
        private Dictionary<string, DateTime> Veiculos = new Dictionary<string, DateTime>();

        /// <summary>
        /// Construtor da classe estacionamento, para poder utilizar seus métodos, algumas informações
        /// iniciais devem ser passadas, como o preço inicial e o valor por hora, eles precisam ser positivos.
        /// </summary>
        /// <param name="PrecoInicial">Indica quanto que deve pagar para o veículo adentrar no estacionamento.</param>
        /// <param name="PrecoPorHora">Indica qual valor será cobrado por hora que o veículo permaneceu no estacionamento.</param>
        public Estacionamento(decimal precoInicial, decimal precoPorHora)
        {
            do
            {
                if(precoInicial < 0)
                {
                    Console.WriteLine("Digite o preco inicial, ele deve ser positivo.");
                    Decimal.TryParse(Console.ReadLine(), out precoInicial); // Evita o FormatException
                }
                if(precoPorHora < 0)
                {
                    Console.WriteLine("Digite o preco por hora, ele deve ser positivo.");
                    Decimal.TryParse(Console.ReadLine(), out precoPorHora);  
                }
            } while (precoInicial < 0 || PrecoPorHora < 0);

            this.PrecoInicial = precoInicial;
            this.PrecoPorHora = precoPorHora;
        }

        /// <summary>
        /// Adiciona um veículo ao estacionamento. Será verificado a validade
        /// da placa, sendo aceito o modelo antigo (AAA-1111) e novo (AAA1A11), 
        /// além de também gravar o horário de entrada.
        /// </summary>
        public void AdicionarVeiculo()
        {
            Console.WriteLine("Digite a placa do veículo para estacionar:");
            string placa = Console.ReadLine().ToUpper();

            if (Veiculos.ContainsKey(placa))
                Console.WriteLine("Placa já cadastrada, por favor, adicione outra!");
            else if(ConferirPlaca(placa))
            {
                DateTime horarioEntrada = DateTime.Now;
                Veiculos.Add(placa, horarioEntrada);
            }
            else
                Console.WriteLine("Placa inválida, por favor, adicione uma placa no modelo correto, como AAA-1111, ou AAA1A11");
        }

        /// <summary>
        /// Remove um veículo do estacionamento, realiza uma busca pela placa passada e calcula o 
        /// custo total, levando em consideração o valor de entrada, valor por hora e quantidade
        /// de horas estacionado. Pode utilizar a diferença de horário entre o período de cadastro
        /// e o atual (onde cada minuto é considerado como uma hora), ou receber um valor manualmente.
        /// </summary>
        public void RemoverVeiculo()
        {
            Console.WriteLine("Digite a placa do veículo para remover:");
            string placa = Console.ReadLine().ToUpper();

            // Verifica se o veículo existe
            if (Veiculos.ContainsKey(placa))
            {
                Console.WriteLine("Digite sua opção:");
                Console.WriteLine("1 - Utilizar horário do sistema");
                Console.WriteLine("2 - Digitar horário manualmente");

                Int16 horas = Convert.ToInt16(DateTime.Now.Subtract(Veiculos[placa]).Minutes);

                switch(Convert.ToInt16(Console.ReadLine()))
                {//Colocar em um case
                    case 1:
                        Console.WriteLine("Será usado o horário do sistema");
                        break;
                    case 2:
                        do
                        {
                            Console.WriteLine("Digite a quantidade de horas que o veículo permaneceu estacionado:");
                            Int16.TryParse(Console.ReadLine(), out horas);

                            if (horas < 0)
                                Console.WriteLine("O valor de horas precisa ser positivo!");
                        } while (horas < 0);
                        break;
                    default:
                        Console.WriteLine("Valor inválido, será usado o horário do sistema");
                        break;
                }

                decimal valorTotal = PrecoInicial + PrecoPorHora * horas;

                Veiculos.Remove(placa);
                if (horas != 1)
                    Console.WriteLine($"O veículo {placa} foi removido, ele ficou estacionado por {horas} horas. O preço total foi de: R$ {valorTotal.ToString("0.00")}");
                else
                    Console.WriteLine($"O veículo {placa} foi removido, ele ficou estacionado por 1 hora. O preço total foi de: R$ {valorTotal.ToString("0.00")}");
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
            if (Veiculos.Any())
            {
                Console.WriteLine("Os veículos estacionados são:");

                // Uma boa forma de iterar um dictionary, facilitando a obtenção das chaves e valores.
                foreach(KeyValuePair<string, DateTime> veiculo in Veiculos) 
                {
                    Console.WriteLine(veiculo.Key);
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
        /// <returns>Seu retorno é um booleano, caso a placa seja compatível com o padrão, retorna true, caso não, false</returns>
        public bool ConferirPlaca(string placa)
        {
            Regex padraoDePlacaAntigo = new Regex("^[A-Za-z]{3}-[0-9]{4}$");
            Regex padraoDePlacaNovo = new Regex("^[A-Za-z]{3}[0-9][A-Za-z][0-9]{2}$");

            if (padraoDePlacaAntigo.IsMatch(placa) 
            || padraoDePlacaNovo.IsMatch(placa))
                return true;
            else
                return false;
        }
    }
}