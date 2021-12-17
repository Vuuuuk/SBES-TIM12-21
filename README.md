# SBES - Projektni zadatak 21.

Implementirati servis sa ulogom servera za skladištenje i upravljanje korisničkim nalozima
(CredentialsStore). 

CredentialsStore server treba da obezbedi:
1. Bazu korisničkih naloga
2. Bazu pravila za upravljanje korisničkim nalozima
3. CredentialsStore komponentu
4. AuthenticationService komponentu

## Dizajn sistema

![SBES - Projektni zadatak 21.](https://i.ibb.co/YfB3qmn/SBES.png)

## Formatiranje GitHub repozitorijuma
 
1. Svaki od članova tima je dužan da kreira svoju "granu" na samom repozitorijumu

2. Ime grane sledećeg formata, broj indeksa-godina (PR-158-2018)

3. Prilikom kreiranja COMMIT poruka pratiti sledeću alteraciju Karma konvencije:

        <type>: <subject>
            <body>

        Dozvoljenje <type> vrednosti:
            - feat for a new feature.
            - fix for a bug fix for the user or a fix to a build script.
            - perf for performance improvements.
            - docs for changes to the documentation.
            - style for code formatting changes or UI changes.
            - refactor for refactoring production code.

        <subject> predstavlja kratak opis rešenog problema ili implementacije koja se commit-uje.
            * obavezan deo svakog commit-a

        <body> predstavlja detaljniji opis samo koda ili promena koje su potrebne kako bi se neki problem rešio 
            * možda će biti korisno kasnije, nije obavezno.

        Primer commit-a (bez tela) - perf: added parallelization for taking in requests in the Engine

4. PULL REQUEST-ove kreiramo posle završavanja svake od celine za koju se dogovorimo da je neko zadužen (ko kako završi dok celine međusobno nisu povezane).

5. Prilikom kreiranja PULL REQUEST-ova svaki član tima je dužan da zatraži odobrenje od svih ostalih članova.

## Formatiranje koda
1. camelCase notacija, kapitalizacija prvog slova druge reči.