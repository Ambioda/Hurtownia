﻿using Hurtownia.DAL;
using Hurtownia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hurtownia.Infrastruktura
{
    public class KoszykMenager
    {
        private ProduktyContext db;
        private ISessionManager session;
        public KoszykMenager(ISessionManager session, ProduktyContext db)
        {
            this.session = session;
            this.db = db;
        }
        public List<PozycjaKoszyka> PobierzKoszyk()
        {
            List<PozycjaKoszyka> koszyk;
            if (session.Get<List<PozycjaKoszyka>>(Consts.KoszykSessionKey)==null)
            {
                koszyk = new List<PozycjaKoszyka>();
            }
            else
            {
                koszyk = session.Get<List<PozycjaKoszyka>>(Consts.KoszykSessionKey) as List<PozycjaKoszyka>;
            }
            return koszyk;
        }
        public void DodajDoKoszyka(int produktId, int ilosc)
        {
            var koszyk = PobierzKoszyk();
            var pozycjaKoszyka = koszyk.Find(k => k.Produkt.ProduktId == produktId);

            if (pozycjaKoszyka != null)
            {
                pozycjaKoszyka.Ilosc += ilosc;
            }
            else
            {
                var produktDoDodania = db.Produkty.Where(k => k.ProduktId == produktId).SingleOrDefault();

                if (produktDoDodania != null)
                {
                    var nowaPozycjaKoszyka = new PozycjaKoszyka()
                    {
                        Produkt = produktDoDodania,
                        Ilosc = ilosc,
                        Wartosc = produktDoDodania.CenaProduktu * ilosc
                    };
                    koszyk.Add(nowaPozycjaKoszyka);
                }
            }
            session.Set(Consts.KoszykSessionKey, koszyk);
        }
        public int UsunZKoszyka(int produktId)
        {
            var koszyk = PobierzKoszyk();
            var pozycjaKoszyka = koszyk.Find(k => k.Produkt.ProduktId == produktId);
            if (pozycjaKoszyka != null)
            {
                if(pozycjaKoszyka.Ilosc > 1 )
                {
                    pozycjaKoszyka.Ilosc--;
                    return pozycjaKoszyka.Ilosc;
                }
                else
                {
                    koszyk.Remove(pozycjaKoszyka);
                }
            }
            return 0;
        }
        public decimal PobierzWartoscKoszyka()
        {
            var koszyk = PobierzKoszyk();
            return koszyk.Sum(k => (k.Ilosc * k.Produkt.CenaProduktu));
        }
        public int PobierzIloscPozycjiKoszyka()
        {
            var koszyk = PobierzKoszyk();
            int ilosc = koszyk.Sum(k => k.Ilosc);
            return ilosc;
        }
        public Zamowienie UtworzZamowienie(Zamowienie noweZamowienie, string userId)
        {
            var koszyk = PobierzKoszyk();
            noweZamowienie.DataDodania = DateTime.Now;
            noweZamowienie.UserId = userId;

            db.Zamowienia.Add(noweZamowienie);
            if (noweZamowienie.PozycjeZamowienia == null)
                noweZamowienie.PozycjeZamowienia = new List<PozycjaZamowienia>();
            decimal koszykWartosc = 0;

            foreach (var koszykElement in koszyk)
            {
                var nowaPozycjaZamowienia = new PozycjaZamowienia()
                {
                    ProduktId = koszykElement.Produkt.ProduktId,
                    Ilosc = koszykElement.Ilosc,
                    CenaZakupu = koszykElement.Produkt.CenaProduktu
                };
                koszykWartosc += (koszykElement.Ilosc * koszykElement.Produkt.CenaProduktu);

                var produkt = db.Produkty.FirstOrDefault(p => p.ProduktId == koszykElement.Produkt.ProduktId);

                if (produkt != null)
                {
                    if (produkt.DostepnaIlosc >= koszykElement.Ilosc)
                    {
                        produkt.DostepnaIlosc -= koszykElement.Ilosc;
                    }
                }

                noweZamowienie.PozycjeZamowienia.Add(nowaPozycjaZamowienia);
            }

            noweZamowienie.WartoscZamowienia = koszykWartosc;
            db.SaveChanges();
            return noweZamowienie;
        }


        public void PustyKoszyk()
        {
            session.Set<List<PozycjaKoszyka>>(Consts.KoszykSessionKey, null);
        }
    }
}