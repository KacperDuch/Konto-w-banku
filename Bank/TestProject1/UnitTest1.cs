using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bank;

namespace BankTests
{
    [TestClass]
    public class KontoTests
    {
        [TestMethod]
        public void TworzenieKonta_PoprawneDane_Sukces()
        {
            var konto = new Konto("Nowak", 200);
            Assert.AreEqual("Nowak", konto.Nazwa);
            Assert.AreEqual(200, konto.Saldo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TworzenieKonta_NegatywneSaldo_ArgumentException()
        {
            new Konto("Nowak", -100);
        }

        [TestMethod]
        public void Wplata_PoprawnaKwota_Sukces()
        {
            var konto = new Konto("Nowak", 200);
            konto.Wplac(50);
            Assert.AreEqual(250, konto.Saldo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Wplata_NegatywnaKwota_ArgumentException()
        {
            var konto = new Konto("Nowak", 200);
            konto.Wplac(-50);
        }

        [TestMethod]
        public void Wyplata_PoprawnaKwota_Sukces()
        {
            var konto = new Konto("Nowak", 200);
            konto.Wyplac(100);
            Assert.AreEqual(100, konto.Saldo);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Wyplata_ZbytDuzaKwota_InvalidOperationException()
        {
            var konto = new Konto("Nowak", 100);
            konto.Wyplac(200);
        }

        [TestMethod]
        public void Blokowanie_OdblokowanieKonta_Sukces()
        {
            var konto = new Konto("Nowak", 200);
            konto.Zablokuj();
            Assert.IsTrue(konto.CzyZablokowane);

            konto.Odblokuj();
            Assert.IsFalse(konto.CzyZablokowane);
        }
    }
}