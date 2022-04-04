using System.Diagnostics;
using Calculadora.Models;
using Microsoft.AspNetCore.Mvc;

namespace Calculadora.Controllers
{
    public class HomeController : Controller{
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger){
            _logger = logger;
        }


        [HttpGet] //esta anotação não seria necessária
                  //porque por predefenição os pedidos HTTP são GET
        public IActionResult Index(){

            //inicializar os dados para a calculadora funcionar
            ViewBag.Visor = "0";

            return View();
        }


        /// <summary>
        /// processa a interação com a calculador
        /// </summary>
        /// <param name="botao">valor do botão selecionado pelo utilizador</param>
        /// <param name="visor">valor existente no Visor da calculadora</param>
        /// <param name="primeiroOperando">valor a ser utilizado na operação algébrica</param>
        /// <param name="operador">operador a ser utilizado na operação</param>
        /// <param name="limpaEcra">'flag' a indicar se se deve, ou não, limpar o exrã</param>
        /// <returns></returns>

        [HttpPost]
        public IActionResult Index(string botao, string visor, string primeiroOperando, string operador, string limpaEcra)
        {

            //vamos deidir o que fazer com o valor 'botão'
            switch (botao)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    //o utilizador pressionou um algarismo

                    if (visor == "0") { visor = botao; }
                    else { visor = visor + botao; }
                    break;

                case ",":
                    //foi pressionada a ','
                    if(!visor.Contains(',')) { visor += botao; }

                    break;

                case "+/-":
                    //vamos 'inverter' o valor do visor
                    //pode ser através de um expressão algébrica
                    //ou, por manipulação de strings
                    if (visor.StartsWith('-')) visor = visor.Substring(1);
                    else visor = "-" + visor;

                    break;

                case "+":
                case "-":
                case "x":
                case ":":
                    //foi pressionaod um 'operador'

                    if (string.IsNullOrEmpty(operador)) {
                        // não é a primeira vez que se executa o código

                        //vamos executar a operação
                        double operandoUm = Convert.ToDouble(primeiroOperando);
                        double operandoDois = Convert.ToDouble(visor.Replace(',','.'));

                        switch (operador){
                            case "+":
                                visor = operandoUm + operandoDois + "";
                                break;
                            case "-":
                                visor = operandoUm - operandoDois + "";
                                break;
                            case "x":
                                visor = operandoUm * operandoDois + "";
                                break;
                            case ":":
                                visor = operandoUm / operandoDois + "";
                                break;
                        }
                        primeiroOperando = visor;
                        operador = botao;
                        limpaEcra = "sim";

                    }
                    break;
            }
            //preparar os dados a serem enviados para a View
            ViewBag.Visor = visor;
            ViewBag.PrimeiroOperando = visor;
            ViewBag.Operador = operador;
            ViewBag.LimpaEcra = limpaEcra;


            return View();
        }

        public IActionResult Privacy(){
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(){
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}