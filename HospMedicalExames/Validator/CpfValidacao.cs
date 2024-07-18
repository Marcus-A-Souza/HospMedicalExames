using System.ComponentModel.DataAnnotations;

namespace HospMedicalExames.Validator
{
    public class CpfValidacao
    {
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
        public class CpfAttribute : ValidationAttribute
        {
#pragma warning disable CS8765 // A nulidade do tipo de parâmetro não corresponde ao membro substituído (possivelmente devido a atributos de nulidade).
            public override bool IsValid(object value)
#pragma warning restore CS8765 // A nulidade do tipo de parâmetro não corresponde ao membro substituído (possivelmente devido a atributos de nulidade).
            {
#pragma warning disable CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
                string cpf = value as string;
#pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.

                if (string.IsNullOrWhiteSpace(cpf))
                    return false;


                cpf = new string(cpf.Where(char.IsDigit).ToArray());


                if (cpf.Length != 11)
                    return false;


                if (cpf.Distinct().Count() == 1)
                    return false;


                return this.BeAValidCPF(cpf);
            }
            private bool BeAValidCPF(string cpf)
            {
                return ValidateCPF(cpf);
            }

            private bool ValidateCPF(string cpf)
            {

                cpf = cpf.Replace(".", "").Replace("-", "");

                if (cpf.Length != 11 || IsAllSameDigits(cpf))
                    return false;

                int sum = 0;
                for (int i = 0; i < 9; i++)
                    sum += int.Parse(cpf[i].ToString()) * (10 - i);

                int remainder = sum % 11;
                remainder = remainder < 2 ? 0 : 11 - remainder;

                if (remainder != int.Parse(cpf[9].ToString()))
                    return false;

                sum = 0;
                for (int i = 0; i < 10; i++)
                    sum += int.Parse(cpf[i].ToString()) * (11 - i);

                remainder = sum % 11;
                remainder = remainder < 2 ? 0 : 11 - remainder;

                if (remainder != int.Parse(cpf[10].ToString()))
                    return false;

                return true;
            }

            private bool IsAllSameDigits(string cpf)
            {
                for (int i = 1; i < cpf.Length; i++)
                {
                    if (cpf[i] != cpf[0])
                    {
                        return false;
                    }
                }
                return true;
            }


        }
    }
}
