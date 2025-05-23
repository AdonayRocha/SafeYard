using System.Text.RegularExpressions;

namespace SafeYard.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }

        /* public static bool ValidarCpf(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;

            cpf = cpf.Replace(".", "").Replace("-", ""); 

            if (cpf.Length != 11 || new string(cpf[0], 11) == cpf)
                return false; 

            int[] peso1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] peso2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma1 = 0, soma2 = 0;
            for (int i = 0; i < peso1.Length; i++)
            {
                soma1 += (cpf[i] - '0') * peso1[i];
                soma2 += (cpf[i] - '0') * peso2[i];
            }

            int digito1 = (soma1 % 11) < 2 ? 0 : 11 - (soma1 % 11);
            soma2 += digito1 * peso2[9];
            int digito2 = (soma2 % 11) < 2 ? 0 : 11 - (soma2 % 11);

            return cpf[9] - '0' == digito1 && cpf[10] - '0' == digito2;
        } */ 
    }
}
