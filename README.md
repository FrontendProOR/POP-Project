## SR16-2022-POP
## Index : SR16-2022 Ognjen Radic
### Projekat iz Platformi za objektno programiranje 2023/2024 godina

***

Potrebno je realizovati stand-alone GUI .NET aplikaciju u WPF tehnologiji za administraciju poslovanja hotela.
Aplikacija treba da omogući administraciju:

1. Hotelskih soba
- Za svaku sobu evidentira se broj sobe, tip sobe (broj kreveta u sobi), da li ima TV, da li ima Mini bar

2. Iznajmljivanje hotelskih soba. Gosti hotela koriste sobe, pri čemu se evidentira:
- tip iznajmljivanja (noćenje ili dnevni boravak),
- ime, prezime i broj lične karte svakog od gostiju koji koriste sobu,
- datum i vreme početka iznajmljivanja,
- datum i vreme završetka iznajmljivanja,
- ukupna cena iznajmljivanja (računa se po završetku boravka).

3. Cenovnika hotela
- Cenovnik hotela za svaki tip sobe definiše cenu noćenja i cenu dnevnog boravka

4. Korisnika aplikacije. Evidentiraju se osobe koje imaju pravo da koriste aplikaciju. Za svakog korisnika evidentira se:
- ime, prezime, JMBG, korisničko ime, lozinka, tip korisnika
- postoje dva tipa korisnika aplikacije - administrator i recepcioner 

Administracija navedenih podataka podrazumeva pregled, unos, izmenu i brisanje podataka.
Sva brisanja su logička (element se proglašava neaktivnim, a ne uklanja se fizički).
Aplikaciju može koristiti samo ulogovani korisnik. Korisnike, cenovnik i sobe ažurira administrator.
Recepcioner administrira iznajmljivanje soba.

Za sve navedene entitete, pri prikazu je potrebno omogućiti:

1. Sortiranje po svakom od entiteta

2. Pretragu podataka
- Sobe je potrebno pretraživati po tipu sobe i stanju sobe (slobodna/zauzeta)
- Iznajmljivanja je potrebno pretraživati po broju sobe, datumu dolaska i datumu odlaska
- Takođe, potrebno je omogućiti prikaz aktuelnih iznajmljivanja (podaci o trenutno korišćenim sobama i gostima koji ih koriste)
- Korisnike je potrebno pretraživati po korisničkom imenu.

Pri iznajmljivanju sobe, aplikacija treba da omogući prikaz svih slobodnih soba određenog tipa.
Pri napuštanju sobe, na osnovu cenovnika se obračunava iznos koji gost treba da plati.
Perzistenciju podataka realizovati korišćenjem relacione baze podataka.