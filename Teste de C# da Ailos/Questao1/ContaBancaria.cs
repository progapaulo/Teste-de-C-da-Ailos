using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria {
        public int NumeroConta { get; set; }
        public string Titular { get; set; }
        public double Saldo { get; set; }
        private const double TaxaSaque = 3.50;

        // Contructor
        public ContaBancaria(int numeroConta, string titular, double depositoInicial = 0)
        {
            NumeroConta = numeroConta;
            Titular = titular;
            Saldo = depositoInicial;
        }

        // Método para realizar o depśito
        public void Depositar(double valorDeposito)
        {
            if (valorDeposito <= 0)
            {
                Console.WriteLine("Deposito deve ser maior que zero.");
                return;
            }
            Saldo += valorDeposito;
        }

        // Método para realizar o saque
        public void Sacar(double valorSaque)
        {
            var totalSaque = valorSaque + (double)TaxaSaque;

            if (totalSaque <= 0)
            {
                Console.WriteLine("Saque deve ser maior que zero.");
                return;
            }
            
            Saldo -= totalSaque;
        }

        // Método para alterar o nome do titular
        public void AlterarTitular(string newTitular)
        {
            Titular = newTitular;
        }

        // Método para exibir os dados da conta
        public void ExibirDados()
        {
            Console.WriteLine($"Conta {NumeroConta}, titular: {Titular}, Saldo: $ {Saldo}");
        }
    }
}
