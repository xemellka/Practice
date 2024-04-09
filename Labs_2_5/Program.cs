using System;

class Program
{
    static void Main(string[] args)
    {
        Surgeon surgeon1 = new Surgeon("Ivan Pipa", 15, 30000);
        Neurosurgeon neurosurgeon1 = new Neurosurgeon("Olena Hopi", 20, 40000);
        PlasticSurgeon plasticSurgeon1 = new PlasticSurgeon("John Ser", 18, 35000);
        Dentist dentist1 = new Dentist("Sergei Porin", 10, 20000);
        Orthodontist orthodontist1 = new Orthodontist("Oleg Mlg", 12, 25000);
        DentalTechnician dentalTechnician1 = new DentalTechnician("Vasya top1", 8, 14000);

        surgeon1.DisplayExperience();
        neurosurgeon1.DisplayExperience();
        dentist1.DisplayExperience();
        orthodontist1.DisplayExperience();
        plasticSurgeon1.DisplayExperience();
        dentalTechnician1.DisplayExperience();

        int clientPayment = 5000;
        dentist1.CalculatePayment(clientPayment);
        orthodontist1.CalculatePayment(clientPayment);
        dentalTechnician1.CalculatePayment(clientPayment);
    }
}
