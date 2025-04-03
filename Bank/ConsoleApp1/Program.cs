using Bank;
using System;

namespace Bank
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Tworzenie standardowego konta...");
            Konto konto = new Konto("Molenda", 100);
            Console.WriteLine($"Konto utworzone dla {konto.Nazwa} z saldem: {konto.Saldo}");

            Console.WriteLine("\nWpłata 200...");
            konto.Wplac(50);
            Console.WriteLine($"Nowe saldo: {konto.Saldo}");

            Console.WriteLine("\nPróba wypłaty 200...");
            try
            {
                konto.Wyplac(200);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }

            Console.WriteLine("\nKonwersja konta na KontoRozszerzone z limitem debetu 100...");
            KontoRozszerzone kontoRozszerzone = new KontoRozszerzone(konto.Nazwa, konto.Saldo, 100);
            Console.WriteLine($"KontoRozszerzone utworzone dla {kontoRozszerzone.Nazwa} z saldem: {kontoRozszerzone.Saldo} i limitem debetu: {kontoRozszerzone.LimitDebetu}");

            Console.WriteLine("\nWypłata 300, korzystając z debetu...");

            try
            {
                kontoRozszerzone.Wyplac(300);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            Console.WriteLine($"Nowe saldo: {kontoRozszerzone.Saldo}, Konto zablokowane: {kontoRozszerzone.CzyZablokowane}");

            Console.WriteLine("\nWpłata 150 w celu odblokowania konta...");
            kontoRozszerzone.Wplac(150);
            Console.WriteLine($"Nowe saldo: {kontoRozszerzone.Saldo}, Konto zablokowane: {kontoRozszerzone.CzyZablokowane}");

            Console.WriteLine("\nKonwersja z powrotem na standardowe konto...");
            konto = new Konto(kontoRozszerzone.Nazwa, kontoRozszerzone.Saldo);
            Console.WriteLine($"Standardowe konto przywrócone dla {konto.Nazwa} z saldem: {konto.Saldo}");

            Console.WriteLine("\nTworzenie KontoZLimitem z wewnętrznym obiektem Konto...");
            KontoZLimitem kontoZLimitem = new KontoZLimitem("Kowalski", 200, 150);
            Console.WriteLine($"KontoZLimitem utworzone dla {kontoZLimitem.Nazwa} z saldem: {kontoZLimitem.Saldo} i limitem: {kontoZLimitem.Limit}");
        }
    }
}