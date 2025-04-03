using System;

namespace Bank
{
    public class Konto
    {
        protected string klient;
        protected decimal saldo;
        protected bool zablokowane = false;

        public Konto(string klient, decimal poczatkoweSaldo = 0)
        {
            if (string.IsNullOrWhiteSpace(klient))
                throw new ArgumentException("Nazwa klienta nie może być pusta.");

            if (poczatkoweSaldo < 0)
                throw new ArgumentException("Początkowe saldo nie może być ujemne.");

            this.klient = klient;
            this.saldo = poczatkoweSaldo;
        }

        public string Nazwa => klient;
        public virtual decimal Saldo => saldo;
        public bool CzyZablokowane => zablokowane;

        public virtual void Wplac(decimal kwota)
        {
            if (zablokowane)
                throw new InvalidOperationException("Operacja niedozwolona. Konto jest zablokowane.");

            if (kwota <= 0)
                throw new ArgumentException("Kwota wpłaty musi być większa od zera.");

            saldo += kwota;
        }

        public virtual void Wyplac(decimal kwota)
        {
            if (zablokowane)
                throw new InvalidOperationException("Operacja niedozwolona. Konto jest zablokowane.");

            if (kwota <= 0)
                throw new ArgumentException("Kwota wypłaty musi być większa od zera.");

            if (kwota > saldo)
                throw new InvalidOperationException("Niewystarczające środki na koncie.");

            saldo -= kwota;
        }

        public void Zablokuj()
        {
            zablokowane = true;
        }

        public void Odblokuj()
        {
            zablokowane = false;
        }
    }

    public class KontoRozszerzone : Konto
    {
        private decimal limitDebetu;
        private bool debetWykorzystany = false;

        public KontoRozszerzone(string klient, decimal poczatkoweSaldo, decimal limitDebetu) : base(klient, poczatkoweSaldo)
        {
            if (limitDebetu < 0)
                throw new ArgumentException("Limit debetu nie może być ujemny.");

            this.limitDebetu = limitDebetu;
        }

        public decimal LimitDebetu
        {
            get => limitDebetu;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Limit debetu nie może być ujemny.");
                limitDebetu = value;
            }
        }

        public override decimal Saldo => saldo + (debetWykorzystany ? 0 : limitDebetu);

        public override void Wyplac(decimal kwota)
        {
            if (zablokowane)
                throw new InvalidOperationException("Operacja niedozwolona. Konto jest zablokowane.");

            if (kwota <= 0)
                throw new ArgumentException("Kwota wypłaty musi być większa od zera.");

            if (kwota > Saldo)
                throw new InvalidOperationException("Kwota przekracza dostępne saldo i limit debetu.");

            saldo -= kwota;

            if (saldo < 0)
            {
                debetWykorzystany = true;
                Zablokuj();
            }
        }

        public override void Wplac(decimal kwota)
        {
            if (kwota <= 0)
                throw new ArgumentException("Kwota wpłaty musi być większa od zera.");

            saldo += kwota;

            if (saldo >= 0 && zablokowane)
            {
                debetWykorzystany = false;
                Odblokuj();
            }
        }
    }

    public class KontoZLimitem
    {
        private Konto konto;
        private decimal limit;
        private bool debetAktywny = false;

        public KontoZLimitem(string klient, decimal poczatkoweSaldo = 0, decimal limit = 100)
        {
            if (limit < 0)
                throw new ArgumentException("Limit nie może być ujemny.");

            this.konto = new Konto(klient, poczatkoweSaldo);
            this.limit = limit;
        }

        public decimal Limit
        {
            get => limit;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Limit nie może być ujemny.");
                limit = value;
            }
        }

        public string Nazwa => konto.Nazwa;
        public decimal Saldo => konto.Saldo + (debetAktywny ? 0 : limit);
        public bool CzyZablokowane => konto.CzyZablokowane;

        public void Wplac(decimal kwota)
        {
            konto.Wplac(kwota);
            if (konto.Saldo >= 0 && konto.CzyZablokowane)
            {
                debetAktywny = false;
                konto.Odblokuj();
            }
        }

        public void Wyplac(decimal kwota)
        {
            if (konto.CzyZablokowane)
                throw new InvalidOperationException("Operacja niedozwolona. Konto jest zablokowane.");

            if (kwota <= 0)
                throw new ArgumentException("Kwota wypłaty musi być większa od zera.");

            if (kwota > Saldo)
                throw new InvalidOperationException("Kwota przekracza dostępne saldo i limit.");

            konto.Wyplac(kwota);

            if (konto.Saldo < 0)
            {
                debetAktywny = true;
                konto.Zablokuj();
            }
        }

        public void Zablokuj()
        {
            konto.Zablokuj();
        }

        public void Odblokuj()
        {
            konto.Odblokuj();
        }
    }
}